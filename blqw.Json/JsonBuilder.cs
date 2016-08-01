using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Runtime.Serialization;


namespace blqw.Serializable
{
    /// <summary> 用于将C#转换为Json字符串
    /// </summary>
    public class JsonBuilder
    {
        private readonly JsonBuilderSettings _settings;
        
        public JsonBuilder()
            : this(JsonBuilderSettings.Default)
        {

        }

        public JsonBuilder(JsonBuilderSettings settings)
        {
            _settings = settings;
        }
        
        /// <summary> 将对象转换为Json字符串
        /// </summary>
        public string ToJsonString(object obj)
        {
            if (obj == null || obj is DBNull)
            {
                return "null";
            }
            using (var writer = new QuickStringWriter(4096))
            {
                var args = new JsonWriterArgs(writer, _settings);
                var warp = JsonWriterContainer.GetWrap(obj.GetType());
                warp.Writer.Write(obj, args);
                return writer.ToString();
            }
        }
    }
}
