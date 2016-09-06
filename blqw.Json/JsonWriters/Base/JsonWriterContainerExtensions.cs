using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using blqw.IOC;

namespace blqw.Serializable
{
    /// <summary>
    /// 容器扩展方法
    /// </summary>
    public static class JsonWriterContainerExtensions
    {
        private static class Cache<T>
        {
            public static ServiceItem Wapper { get; set; }
        }

        private static ServiceItem _NullWapper;

        /// <summary>
        /// 获取写入器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        public static IJsonWriter GetWriter<T>(this ServiceContainer container)
        {
            if (object.ReferenceEquals(JsonWriterContainer2.Instance, container))
            {
                return (IJsonWriter)(Cache<T>.Wapper ?? (Cache<T>.Wapper = container.GetServiceItem(typeof(T))))?.Value;
            }
            return (IJsonWriter)container.GetServiceItem(typeof(T))?.Value;
        }

        /// <summary>
        /// 获取写入器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"> </param>
        /// <returns></returns>
        public static IJsonWriter GetWriter(this ServiceContainer container,Type type) => (IJsonWriter)container.GetService(type);

        /// <summary>
        /// 获取null的写入器
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static IJsonWriter GetNullWriter(this ServiceContainer container)
        {
            if (object.ReferenceEquals(JsonWriterContainer2.Instance, container))
            {
                return (IJsonWriter)(_NullWapper ?? (_NullWapper = container.GetServiceItem(typeof(void))))?.Value;
            }
            return (IJsonWriter)container.GetServiceItem(typeof(void))?.Value;
        }
    }
}
