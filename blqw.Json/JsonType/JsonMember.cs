using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace blqw
{
    /// <summary> 可以被序列化成Json成员的对象
    /// </summary>
    public sealed class JsonMember
    {
        /// <summary> 创建Json成员对象,如果成员被指定为忽略反序列化则返回null
        /// </summary>
        /// <param name="member">对象成员信息</param>
        public static JsonMember Create(MemberInfo member)
        {
            var jsonIgnore = (JsonIgnoreAttribute)Attribute.GetCustomAttribute(member, typeof(JsonIgnoreAttribute));
            if (jsonIgnore != null)
            {
                if (jsonIgnore.NonDeserialize)
                {
                    return null;
                }
                return new JsonMember(member, true);
            }
            return new JsonMember(member, false);
        }

        /// <summary> 构造函数
        /// </summary>
        private JsonMember(MemberInfo member, bool ignoreSerialized)
        {
            if (member.MemberType == MemberTypes.Property)
            {
                Member = new ObjectProperty((PropertyInfo)member);
            }
            else
            {
                Member = new ObjectProperty((FieldInfo)member);
            }

            var name = Member.Attributes.First<JsonNameAttribute>();
            JsonName = name != null ? JsonBuilder.EscapeString(name.Name) : member.Name;

            JsonFormatAttribute format = Member.Attributes.First<JsonFormatAttribute>();
            if (format != null)
            {
                if (!TypesHelper.IsChild(typeof(IFormattable), Member.MemberType))
                {
                    format = null;
                }
                MustFormat = true;
                FormatString = format.Format;
                FormatProvider = format.Provider;
            }
            NonSerialized = ignoreSerialized;
            Type = Member.MemberType;
            CanRead = Member.CanRead;
            CanWrite = Member.CanWrite;
        }
        /// <summary> Literacy组件的成员访问对象
        /// </summary>
        public readonly ObjectProperty Member;
        /// <summary> 序列化和反序列化时的参考Json属性名称
        /// </summary>
        public readonly string JsonName;
        /// <summary> 指示序列化的时候是否需要按指定格式格式化
        /// </summary>
        public readonly bool MustFormat;
        /// <summary> 序列化时使用的格式化参数
        /// </summary>
        public readonly string FormatString;
        /// <summary> 序列化时使用的格式化机制
        /// </summary>
        public readonly IFormatProvider FormatProvider;
        /// <summary> 指示当前成员是否忽略序列化操作
        /// </summary>
        public readonly bool NonSerialized;

        /// <summary> 成员类型
        /// </summary>
        public readonly Type Type;

        private JsonType _jsonType;

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

        /// <summary> 是否可读
        /// </summary>
        public readonly bool CanRead;

        /// <summary> 是否可写
        /// </summary>
        public readonly bool CanWrite;
    }
}
