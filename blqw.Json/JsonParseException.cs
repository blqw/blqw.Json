using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    class JsonParseException : Exception
    {
        public JsonParseException(string message, string json, Exception ex = null)
            : base(message + ",原JSON字符串详见Exception.Data[\"SourceJsonString\"]", ex)
        {
            base.Data["SourceJsonString"] = json;
        }
    }
}
