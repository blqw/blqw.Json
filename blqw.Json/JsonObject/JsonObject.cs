using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    struct JsonObject : IJsonObject, IDictionary<string, object>
    {
        internal static IJsonObject ToJsonObject(object obj, string key = null)
        {
            if (obj is string)
            {
                return new JsonValue(key, (string)obj);
            }
            else if (obj is IDictionary<string, object>)
            {
                return new JsonObject(key, (IDictionary<string, object>)obj);
            }
            else if (obj is IList)
            {
                return new JsonArray(key, (IList)obj);
            }
            else if (obj is IConvertible)
            {
                return new JsonValue(key, (IConvertible)obj);
            }
            else if (obj == null)
            {
                return new JsonValue(key, null);
            }
            return new JsonValue(key, obj.ToString());
        }

        public JsonObject(string key,IDictionary<string, object> dict)
            : this()
        {
            _dict = dict;
            Key = key;
        }

        private IDictionary<string, object> _dict;

        public IJsonObject this[string key]
        {
            get
            {
                object value;
                if (_dict.TryGetValue(key, out value))
                {
                    return ToJsonObject(value);
                }
                return JsonValue.Undefined;
            }
        }

        public IJsonObject this[int index]
        {
            get { return this[index + ""]; }
        }

        public ICollection<string> Keys
        {
            get
            {
                return _dict.Keys;
            }
        }

        public JsonTypeCode TypeCode
        {
            get { return JsonTypeCode.Object; }
        }

        public bool IsUndefined { get; private set; }

        public void Add(string key, object value)
        {
            _dict.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _dict.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return _dict.Values; }
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                object value;
                if (_dict.TryGetValue(key, out value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                _dict[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _dict.Add(item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dict.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dict.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return _dict.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dict.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }




        public object Value
        {
            get { return null; }
        }

        public string Key { get; private set; }

        IEnumerator<IJsonObject> IEnumerable<IJsonObject>.GetEnumerator()
        {
            foreach (var item in _dict)
            {
                yield return ToJsonObject(item.Value, item.Key);
            }
        }
    }
}
