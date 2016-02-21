using blqw.JsonComponent;
using blqw.Serializable;
using System;
using System.ComponentModel.Composition;


namespace blqw
{
    /// <summary> 操作Json序列化/反序列化的静态对象
    /// </summary>
    public static class Json
    {
        /// <summary> 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Export("ToJsonString")]
        [ExportMetadata("Priority", 100)]
        public static string ToJsonString(this object obj)
        {
            return new JsonBuilder().ToJsonString(obj);
        }

        /// <summary> 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings">序列化Json字符串时使用的设置参数</param>
        public static string ToJsonString(this object obj, JsonBuilderSettings settings)
        {
            return new JsonBuilder(settings).ToJsonString(obj);
        }

        /// <summary> 将json字符串转换为指定对象
        /// </summary>
        public static T ToObject<T>(string json)
        {
            if (json == null || json.Length == 0)
            {
                return default(T);
            }
            return (T)ToObject(typeof(T), json);
        }
        /// <summary> 将json字符串转换IDictionary或者IList
        /// </summary>
        public static object ToObject(string json)
        {
            if (json == null || json.Length == 0)
            {
                return null;
            }
            return new JsonParser().ToObject(null, json);
        }

        /// <summary> 将json字符串转换成动态类型
        /// </summary>
        public static dynamic ToDynamic(string json)
        {
            if (json == null || json.Length == 0)
            {
                return null;
            }
            if (Component.GetDynamic == null)
            {
                return new JsonParser(null, typeof(System.Dynamic.ExpandoObject), null).ToObject(null, json);
            }
            else
            {
                return Component.GetDynamic(new JsonParser().ToObject(null, json));
            }
        }

        /// <summary> 将json字符串转换为指定对象
        /// </summary>
        [Export("ToJsonObject")]
        [ExportMetadata("Priority", 100)]
        public static Object ToObject(Type type, string json)
        {
            if (json == null || json.Length == 0)
            {
                return null;
            }
            return new JsonParser().ToObject(type, json);
        }


    }
}
