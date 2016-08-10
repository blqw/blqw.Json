using System;
using System.ComponentModel.Composition;
using blqw.Serializable;

namespace blqw
{
    /// <summary>
    /// 操作Json序列化/反序列化的静态对象
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        [Export("ToJsonString")]
        [ExportMetadata("Priority", 105)]
        public static string ToJsonString(this object obj) => ToJsonString(obj, JsonBuilderSettings.Default);

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj"> </param>
        /// <param name="settings"> 序列化Json字符串时使用的设置参数 </param>
        public static string ToJsonString(this object obj, JsonBuilderSettings settings)
        {
            if (obj == null || obj is DBNull)
            {
                return "null";
            }
            using (var writer = new QuickStringWriter(4096))
            {
                var args = new JsonWriterArgs(writer, settings);
                var warp = JsonWriterContainer.GetWrap(obj.GetType());
                warp.Writer.Write(obj, args);
                return writer.ToString();
            }
        }

        /// <summary>
        /// 将json字符串转换为指定对象
        /// </summary>
        public static T ToObject<T>(string json) => string.IsNullOrEmpty(json) ? default(T) : (T) ToObject(typeof(T), json);

        /// <summary>
        /// 将json字符串转换IDictionary或者IList
        /// </summary>
        public static object ToObject(string json) => string.IsNullOrEmpty(json) ? null : new JsonParser().ToObject(null, json);

        /// <summary>
        /// 将json字符串转换成动态类型
        /// </summary>
        public static dynamic ToDynamic(string json) => new JsonParser().ToObject(null, json).ToDynamic();

        /// <summary>
        /// 将json字符串转换为指定对象
        /// </summary>
        [Export("ToJsonObject")]
        [ExportMetadata("Priority", 105)]
        public static object ToObject(Type type, string json) => string.IsNullOrEmpty(json) ? null : new JsonParser().ToObject(type, json);
    }
}