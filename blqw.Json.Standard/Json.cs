using System;
using blqw.ConvertServices;
using blqw.JsonServices;
using blqw.JsonServices;
using Microsoft.Extensions.DependencyInjection;

namespace blqw
{
    /// <summary>
    /// 操作Json序列化/反序列化的静态对象
    /// </summary>
    public static class Json
    {
        private static IServiceProvider CreateServiceProvider() =>
                  ConvertSettings.Global.ServiceProvider.Aggregate(new ServiceCollection().AddJsonService().AddConvert4().BuildServiceProvider());

        public static IServiceProvider ServiceProvider { get; private set; } = CreateServiceProvider();
        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public static string ToJsonString(this object obj) =>
            obj.ToJsonString(JsonBuilderSettings.Default);

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="settings"> 序列化Json字符串时使用的设置参数 </param>
        public static string ToJsonString(this object obj, JsonBuilderSettings settings)
        {
            using (var buffer = new QuickStringWriter(4096))
            {
                var args = new JsonWriterSettings(buffer, ServiceProvider, settings);
                args.WriteObject(obj);
                return buffer.ToString();
            }
        }

        /// <summary>
        /// 将json字符串转换为指定对象
        /// </summary>
        public static T ToObject<T>(string json) =>
            string.IsNullOrEmpty(json) ? default : (T)ToObject(typeof(T), json);

        /// <summary>
        /// 将json字符串转换IDictionary或者IList
        /// </summary>
        public static object ToObject(string json) =>
            string.IsNullOrEmpty(json) ? null : new JsonParser().ToObject(null, json);

        /// <summary>
        /// 将json字符串转换成动态类型
        /// </summary>
        public static dynamic ToDynamic(string json) =>
          (string.IsNullOrEmpty(json) ? null : new JsonParser().ToObject(null, json)).ToDynamic();

        /// <summary>
        /// 将json字符串转换为指定对象
        /// </summary>
        public static object ToObject(Type type, string json) =>
            string.IsNullOrEmpty(json) ? null : new JsonParser().ToObject(type, json);
    }
}