using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.Reflection;

namespace blqw.Serializable
{
    /// <summary>
    /// 
    /// </summary>
    [InheritedExport(typeof(IJsonWriter))]
    public interface IJsonWriter
    {
        /// <summary>
        /// 匹配类型
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// 写入一个对象
        /// </summary>
        /// <param name="obj"> 需要写入的对象 </param>
        /// <param name="args"> 写入对象时需要使用的参数 </param>
        void Write(object obj, JsonWriterArgs args);
    }
}