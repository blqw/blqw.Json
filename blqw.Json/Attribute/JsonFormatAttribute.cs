using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 指示指示某个实现IFormattable的字段或属性在序列化Json时所使用的自定义格式
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class JsonFormatAttribute : Attribute
    {
        /// <summary> 指示指示某个实现IFormattable的字段或属性在序列化Json时所使用的自定义格式
        /// </summary>
        /// <param name="format">指定要使用的自定义格式的字符串</param>
        public JsonFormatAttribute(string format)
            : this(format, null)
        {
        }
        /// <summary> 指示指示某个实现IFormattable的字段或属性在序列化Json时所使用的自定义格式 ,并指定格式化的对象的机制
        /// </summary>
        /// <param name="format">指定要使用的自定义格式的字符串</param>
        /// <param name="provider">用于格式化该值的 System.IFormatProvider</param>
        public JsonFormatAttribute(string format, IFormatProvider provider)
        {
            Format = format;
            Provider = provider;
        }
        /// <summary> 指定要使用的自定义格式的字符串
        /// </summary>
        public string Format { get; private set; }
        /// <summary> 用于格式化该值的对象的机制
        /// </summary>
        public IFormatProvider Provider { get; private set; }
    }
}
