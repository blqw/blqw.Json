using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 允许对象控制自己的反序列化行为
    /// </summary>
    public interface IJsonToObject
    {
        /// <summary> 使用 IJsonObject 的值填充到当前对象中
        /// </summary>
        /// <param name="jsonObject">反序列化Json字符串得到的值</param>
        void Fill(IJsonObject jsonObject);
    }
}
