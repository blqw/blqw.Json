using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public struct JsonValue : IJsonObject
    {
        public readonly static JsonValue Undefined = new JsonValue(null) { IsUndefined = true };

        public JsonValue(string value)
        {
            _value = value;
        }

        private string _value;

        public IJsonObject this[string key]
        {
            get { return Undefined; }
        }

        public string[] Keys
        {
            get { return new string[0]; }
        }

        public JsonTypeCode TypeCode
        {
            get { return JsonTypeCode.Value; }
        }

        public bool IsUndefined { get; private set; }

        public override string ToString()
        {
            return _value;
        }
    }
}
