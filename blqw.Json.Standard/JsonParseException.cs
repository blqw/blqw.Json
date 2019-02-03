using System;

namespace blqw.JsonServices
{
    internal sealed class JsonParseException : Exception
    {
        public JsonParseException(string message, string json, Exception ex = null)
            : base(message + ",原JSON字符串详见Exception.Data[\"SourceJsonString\"]", ex)
        {
            Data["SourceJsonString"] = json;
        }
    }
}