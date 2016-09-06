using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using blqw.IOC;

namespace blqw.Serializable
{
    /// <summary>
    /// 基于 IoC <see cref="ServiceContainer"/> 实现的 <see cref="IJsonWriter" /> 的容器
    /// </summary>
    public sealed partial class JsonWriterContainer2 : ServiceContainer
    {
        private JsonWriterContainer2()
            : base(null, typeof(IJsonWriter), TypeComparer.Instance)
        {

        }

        /// <summary>
        /// 获取指定类型的写入器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IJsonWriter this[Type type] => (IJsonWriter)GetService(type);

        /// <summary>
        /// 单例
        /// </summary>
        public static JsonWriterContainer2 Instance { get; } = new JsonWriterContainer2();

        protected override Type GetServiceType(PlugIn plugIn, object value) => (value as IJsonWriter)?.Type;
    }
}