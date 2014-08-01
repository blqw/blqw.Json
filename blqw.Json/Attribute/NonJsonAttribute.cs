using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 指示某个字段或属性不应被序列化成Json。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class NonJsonAttribute : Attribute
    {
        /// <summary> 指示某个字段或属性不应被序列化成Json,但可以从Json中反序列化该成员的值。 
        /// </summary>
        public NonJsonAttribute()
            : this(false)
        {
        }

        /// <summary> 指示某个字段或属性不应被序列化成Json,并设置是否可以从Json中反序列化该成员的值。 
        /// </summary>
        public NonJsonAttribute(bool nonDeserialize)
        {
            NonDeserialize = nonDeserialize;
        }

        public bool NonDeserialize { get; private set; }
    }
}
