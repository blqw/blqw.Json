using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    struct JsonArray : IJsonObject, IEnumerable
    {
        public JsonArray(string key,IList list)
            :this()
        {
            _list = list;
            Key = key;
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
            get { return new string[0]; }
        }

        public JsonTypeCode TypeCode
        {
            get { return JsonTypeCode.Array; }
        }

        public bool IsUndefined { get; private set; }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public object Value
        {
            get { return null; }
        }

        public string Key { get; private set; }

        IEnumerator<IJsonObject> IEnumerable<IJsonObject>.GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return JsonObject.ToJsonObject(item);
            }
        }
    }
}
