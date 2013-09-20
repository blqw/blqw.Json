using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public class Json
    {
        public static string ToJsonString(object obj)
        {
            return new QuickJsonBuilder().ToJsonString(obj);
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

        /// <summary> 将json字符串转换为指定对象
        /// </summary>
        public static Object ToObject(string json)
        {
            if (json == null || json.Length == 0)
            {
                return null;
            }
            return new JsonParser().ToObject(null, json);
        }
        /// <summary> 将json字符串转换为指定对象
        /// </summary>
        public static Object ToObject(Type type, string json)
        {
            object obj;
            if (type == null)
            {
                return null;
            }
            if (json == null || json.Length == 0)
            {
                return null;
            }
            return new JsonParser().ToObject(type, json);
        }
    }
}
