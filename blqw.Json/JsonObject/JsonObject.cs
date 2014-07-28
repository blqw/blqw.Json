using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public struct JsonObject : IJsonObject, IDictionary<string, object>
    {
        internal static IJsonObject ToJsonObject(object obj)
        {
            if (obj is IDictionary<string, object>)
            {
                return new JsonObject((IDictionary<string, object>)obj);
            }
            else if (obj is IList)
            {
                return new JsonArray((IList)obj);
            }
            else if (obj is string)
            {
                return new JsonValue((string)obj);
            }
            else if (obj == null)
            {
                return new JsonValue(null);
            }
            return JsonValue.Undefined;
        }


        public JsonObject(IDictionary<string, object> dict)
        {
            _dict = dict;
        }

        private IDictionary<string, object> _dict;

        public IJsonObject this[string key]
        {
            get
            {
                object obj;
                if (_dict.TryGetValue(key, out obj))
                {
                    return ToJsonObject(obj);
                }
                return JsonValue.Undefined;
            }
        }

        public IEnumerable<string> Keys
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
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            throw new NotImplementedException();
        }

        public ICollection<object> Values
        {
            get { throw new NotImplementedException(); }
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
