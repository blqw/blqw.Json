using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 允许对象控制自己的反序列化行为
    /// </summary>
    public interface ILoadJson
    {
        /// <summary> 使用 IJsonObject 的值导入到当前对象中
        /// </summary>
        /// <param name="jsonObject">反序列化Json字符串得到的值</param>
        void LoadJson(IJsonObject jsonObject);
    }
}
