using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public class JsonArray : IJsonObject, IEnumerable
    {
        public JsonArray(IList list)
        {
            _list = list;
        }

        private IList _list;

        public IJsonObject this[string key]
        {
            get
            {
                int index;
                if (int.TryParse(key, out index))
                {
                    if (index >= 0 && index <= _list.Count)
                    {
                        return JsonObject.ToJsonObject(_list[index]);
                    }
                }
                return JsonValue.Undefined;
            }
        }

        public IJsonObject this[int index]
        {
            get
            {
                if (index >= 0 && index <= _list.Count)
                {
                    return JsonObject.ToJsonObject(_list[index]);
                }
                return JsonValue.Undefined;
            }
        }
        public ICollection<string> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public JsonTypeCode TypeCode
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsUndefined { get; private set; }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }


    }
}
