using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 表示JsonObject的类型
    /// </summary>
    public enum JsonTypeCode
    {
        /// <summary> 数组
        /// </summary>
        Array,
        /// <summary> 键值对象
        /// </summary>
        Object,
        /// <summary> 值
        /// </summary>
        Value,
    }
}
