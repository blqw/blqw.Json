﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Script.Serialization;
using blqw.IOC;

namespace blqw.Serializable
{
    /// <summary>
    /// 可以被序列化成Json成员的对象
    /// </summary>
    [DebuggerDisplay("Name = {JsonName},Member = {Member}")]
    public sealed class JsonMember
    {
        /// <summary>
        /// 是否可读
        /// </summary>
        public readonly bool CanRead;

        /// <summary>
        /// 是否可写
        /// </summary>
        public readonly bool CanWrite;

        /// <summary>
        /// 用于显示的文本
        /// </summary>
        public readonly string DisplayText;

        /// <summary>
        /// 序列化时使用的格式化机制
        /// </summary>
        public readonly IFormatProvider FormatProvider;

        /// <summary>
        /// 序列化时使用的格式化参数
        /// </summary>
        public readonly string FormatString;

        /// <summary>
        /// 获取属性或字段的值
        /// </summary>
        public readonly Func<object, object> GetValue;

        /// <summary>
        /// 序列化和反序列化时的参考Json属性名称
        /// </summary>
        public readonly string JsonName;

        private readonly JsonWriterWrapper JsonWriterWrapper;

        /// <summary>
        /// Literacy组件的成员访问对象
        /// </summary>
        public readonly MemberInfo Member;

        /// <summary>
        /// 指示序列化的时候是否需要按指定格式格式化
        /// </summary>
        public readonly bool MustFormat;

        /// <summary>
        /// 指示当前成员是否忽略序列化操作
        /// </summary>
        public readonly bool NonSerialized;

        /// <summary>
        /// 设置属性或字段的值
        /// </summary>
        public readonly Action<object, object> SetValue;

        /// <summary>
        /// 成员类型
        /// </summary>
        public readonly Type Type;

        private JsonType _jsonType;

        /// <summary>
        /// 构造函数
        /// </summary>
        private JsonMember(MemberInfo member, bool ignoreSerialized)
        {
            Member = member;
            DisplayText = TypeName.Get(member.ReflectedType) + "." + member.Name;
            JsonName = member.GetCustomAttribute<DisplayNameAttribute>(true)?.DisplayName ?? member.Name;


            InitGetSet(out Type, out GetValue, out SetValue);
            JsonWriterWrapper = JsonWriterContainer.GetWrap(Type);
            CanWrite = SetValue != null;
            CanRead = GetValue != null;
            NonSerialized = ignoreSerialized;

            var format = member.GetCustomAttribute<JsonFormatAttribute>(true);

            if (format == null || typeof(IFormattable).IsAssignableFrom(Type) == false) return;

            MustFormat = true;
            FormatString = format.Format;
            FormatProvider = format.Provider;
        }

        public JsonType JsonType
        {
            get
            {
                if (_jsonType == null)
                {
                    _jsonType = JsonType.Get(Type);
                }
                return _jsonType;
            }
        }

        public IJsonWriter JsonWriter => JsonWriterWrapper.Writer;

        /// <summary>
        /// 创建Json成员对象,如果成员被指定为忽略反序列化则返回null
        /// </summary>
        /// <param name="member"> 对象成员信息 </param>
        public static JsonMember Create(MemberInfo member)
        {
            var jsonIgnore = (JsonIgnoreAttribute) Attribute.GetCustomAttribute(member, typeof(JsonIgnoreAttribute));
            if (jsonIgnore != null)
            {
                return jsonIgnore.NonDeserialize ? null : new JsonMember(member, true);
            }
            var b = member.IsDefined(typeof(NonSerializedAttribute), true);
            if (b == false)
            {
                b = member.IsDefined(typeof(ScriptIgnoreAttribute), true);
            }
            return new JsonMember(member, b);
        }

        private void InitGetSet(out Type type, out Func<object, object> get, out Action<object, object> set)
        {
            if (ComponentServices.GetGeter != null && ComponentServices.GetSeter != null)
            {
                get = ComponentServices.GetGeter(Member);
                set = ComponentServices.GetSeter(Member);
                type = (Member as PropertyInfo)?.PropertyType ?? (Member as FieldInfo)?.FieldType;
            }
            else if (Member.MemberType == MemberTypes.Property)
            {
                var property = (PropertyInfo) Member;
                type = property.PropertyType;
                var o = Expression.Parameter(typeof(object), "o");
                Debug.Assert(property.DeclaringType != null, "property.DeclaringType != null");
                var cast = Expression.Convert(o, property.DeclaringType);
                var p = Expression.Property(cast, property);
                get = null;
                if (property.CanRead)
                {
                    var ret = Expression.Convert(p, typeof(object));
                    get = Expression.Lambda<Func<object, object>>(ret, o).Compile();
                }
                set = null;
                if (property.CanWrite)
                {
                    var v = Expression.Parameter(typeof(object), "v");
                    var val = Expression.Convert(v, type);
                    var assign = Expression.MakeBinary(ExpressionType.Assign, p, val);
                    var ret = Expression.Convert(assign, typeof(object));
                    set = Expression.Lambda<Action<object, object>>(ret, o, v).Compile();
                }
                else if (property.DeclaringType.IsGenericType &&
                         property.DeclaringType.Name.StartsWith("<>f__AnonymousType")) //匿名类型
                {
                    var fieldName = $"<{property.Name}>i__Field";
                    var field = property.DeclaringType.GetField(fieldName,
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    Debug.Assert(field != null, "field != null");
                    set = field.SetValue;
                }
            }
            else
            {
                var field = (FieldInfo) Member;
                type = field.FieldType;
                var o = Expression.Parameter(typeof(object), "o");
                var cast = Expression.Convert(o, field.DeclaringType);
                var p = Expression.Field(cast, field);
                var ret = Expression.Convert(p, typeof(object));
                get = Expression.Lambda<Func<object, object>>(ret, o).Compile();
                set = null;
                if (field.IsLiteral == false)
                {
                    var v = Expression.Parameter(typeof(object), "v");
                    var val = Expression.Convert(v, type);
                    var assign = Expression.MakeBinary(ExpressionType.Assign, p, val);
                    var ret2 = Expression.Convert(assign, typeof(object));
                    set = Expression.Lambda<Action<object, object>>(ret2, o, v).Compile();
                }
            }
        }
    }
}