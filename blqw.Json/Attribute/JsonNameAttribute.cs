using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 指示某个字段或属性在Json的序列化和反序列化过程中使用的新名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class JsonNameAttribute : Attribute
    {
        /// <summary> 指示某个字段或属性在Json的序列化和反序列化过程中使用的新名称
        /// </summary>
        /// <param name="name">在Json的序列化和反序列化过程中使用的新名称</param>
        public JsonNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary> 在Json的序列化和反序列化过程中使用的新名称
        /// </summary>
        public string Name { get; private set; }
    }
}
