using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

        /// <summary>
        /// 已经编码的名称,包括双引号
        /// </summary>
        internal readonly string EncodedJsonName;

        private readonly JsonWriterWrapper _jsonWriterWrapper;

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
            using (var sw = new StringWriter())
            {
                var args = new JsonWriterArgs(sw, 0);
                JsonWriterContainer.StringWriter.Write(JsonName, args);
                EncodedJsonName = sw.ToString();
            }

            InitGetSet(out Type, out GetValue, out SetValue);
            if (Type.IsValueType || Type.IsSealed)
                _jsonWriterWrapper = JsonWriterContainer.GetWrap(Type);
            CanWrite = SetValue != null;
            CanRead = GetValue != null;
            NonSerialized = ignoreSerialized;

            var format = member.GetCustomAttribute<JsonFormatAttribute>(true);

            if (format == null || typeof(IFormattable).IsAssignableFrom(Type) == false) return;

            MustFormat = true;
            FormatString = format.Format;
            FormatProvider = format.Provider;
        }

        public JsonType JsonType => _jsonType ?? (_jsonType = JsonType.Get(Type));

        public IJsonWriter JsonWriter => _jsonWriterWrapper?.Writer;

        /// <summary>
        /// 创建Json成员对象,如果成员被指定为忽略反序列化则返回null
        /// </summary>
        /// <param name="member"> 对象成员信息 </param>
        public static JsonMember Create(MemberInfo member)
        {
            var jsonIgnore = (JsonIgnoreAttribute)Attribute.GetCustomAttribute(member, typeof(JsonIgnoreAttribute));
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
            type = (Member as PropertyInfo)?.PropertyType ?? (Member as FieldInfo)?.FieldType;
            if (ComponentServices.GetGeter != null)
            {
                get = ComponentServices.GetGeter(Member);
            }
            else
            {
                switch (Member.MemberType)
                {
                    case MemberTypes.Property:
                        get = ((PropertyInfo)Member).GetValue;
                        break;
                    case MemberTypes.Field:
                        get = ((FieldInfo)Member).GetValue;
                        break;
                    default:
                        get = null;
                        break;
                }
            }

            if (ComponentServices.GetSeter != null)
            {
                set = ComponentServices.GetSeter(Member);
            }
            else
            {
                switch (Member.MemberType)
                {
                    case MemberTypes.Property:
                        set = ((PropertyInfo)Member).SetValue;
                        break;
                    case MemberTypes.Field:
                        set = ((FieldInfo)Member).SetValue;
                        break;
                    default:
                        set = null;
                        break;
                }
            }
        }
    }
}