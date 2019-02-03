using System;

namespace blqw
{
    /// <summary>
    /// 指示某个字段或属性不应被序列化成Json。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class JsonIgnoreAttribute : Attribute
    {
        /// <summary>
        /// 指示某个字段或属性不应被序列化成Json,但可以从Json中反序列化该成员的值。
        /// </summary>
        public JsonIgnoreAttribute()
            : this(false)
        {
        }

        /// <summary>
        /// 指示某个字段或属性不应被序列化成Json,并设置是否可以从Json中反序列化该成员的值。
        /// </summary>
        public JsonIgnoreAttribute(bool nonDeserialize) => NonDeserialize = nonDeserialize;

        /// <summary>
        /// 指示
        /// </summary>
        public bool NonDeserialize { get; private set; }

        /// <summary> 当在派生类中重写时，返回一个指示此实例是否等于指定对象的值。 </summary>
        /// <returns> 如果该实例等于 <paramref name="obj" />，则为 true；否则，为 false。 </returns>
        /// <param name="obj"> 与 <see cref="T:System.Attribute" /> 的此实例进行比较的 <see cref="T:System.Object" />。 </param>
        /// <filterpriority> 2 </filterpriority>
        public override bool Match(object obj) =>
            base.Match(obj) || (string.Equals(obj?.GetType().Name, "ScriptIgnoreAttribute", StringComparison.OrdinalIgnoreCase));
    }
}