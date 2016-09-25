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
    public sealed class JsonWriterContainer : ServiceContainer
    {
        private JsonWriterContainer()
            : base(null, typeof(IJsonWriter), TypeComparer.Instance)
        {

        }

        /// <summary>
        /// 获取指定类型的写入器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"> 字典中已包含元素的最大数目 (<see cref="F:System.Int32.MaxValue" />)。 </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is <see langword="null" />. </exception>
        public IJsonWriter this[Type type] => (IJsonWriter)GetService(type);

        /// <summary>
        /// 单例
        /// </summary>
        public static JsonWriterContainer Instance { get; } = new JsonWriterContainer();

        /// <summary>
        /// 获取插件的服务类型 <see cref="T:System.Type" />, 默认 <code>plugIn.GetMetadata&lt;Type&gt;("ServiceType")</code>
        /// </summary>
        /// <param name="plugIn"> 插件 </param>
        /// <param name="value"> 插件值 </param>
        /// <returns></returns>
        protected override Type GetServiceType(PlugIn plugIn, object value) => (value as IJsonWriter)?.Type;
    }
}