using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 表示一个Json对象
    /// </summary>
    public interface IJsonObject : IEnumerable<IJsonObject>
    {
        /// <summary> 当前对象的键
        /// </summary>
        string Key { get; }
        /// <summary> 根据键获取内部Json对象
        /// </summary>
        IJsonObject this[string key] { get; }
        /// <summary> 根据索引获取内部Json对象
        /// </summary>
        IJsonObject this[int index] { get; }
        /// <summary> 获取Json中的值
        /// </summary>
        object Value { get; }
        /// <summary> 获取Json对象的键集合
        /// </summary>
        ICollection<string> Keys { get; }
        /// <summary> 获取当前Json对象的类型
        /// </summary>
        JsonTypeCode TypeCode { get; }
        /// <summary> 判断当前Json对象是否是未定义对象
        /// </summary>
        bool IsUndefined { get; }
    }
}
