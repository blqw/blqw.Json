using System;

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
        public static string ToJsonString(object obj)
        {
            return new QuickJsonBuilder().ToJsonString(obj);
        }

        /// <summary> 将对象转换为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="settings">序列化Json字符串时使用的设置参数</param>
        public static string ToJsonString(object obj, JsonBuilderSettings settings)
        {
            return new QuickJsonBuilder(settings).ToJsonString(obj);
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

        static Type DynamicType;
        static void InitDynamicType()
        {
            DynamicType = Type.GetType("System.Dynamic.ExpandoObject, System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            if (DynamicType != null)
            {
                return;
            }
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            var length = ass.Length;
            for (int i = 0; i < length; i++)
            {
                if (ass[i].GetName().Name == "System.Core")
                {
                    DynamicType = ass[i].GetType("System.Dynamic.ExpandoObject");
                    break;
                }
            }
            if (DynamicType == null)
            {
                throw new TypeLoadException("dynamic类型加载失败!");
            }
        }
        public static object ToDynamic(string json)
        {
            if (json == null || json.Length == 0)
            {
                return null;
            }
            if (DynamicType == null)
            {
                InitDynamicType();
            }
            return new JsonParser(null, DynamicType, v => new JsonValue2(v)).ToObject(DynamicType, json);
        }

        public static IJsonObject ToJsonObject(string json)
        {
            var obj = ToObject(json);
            return JsonObject.ToJsonObject(obj);
        }


    }
}
