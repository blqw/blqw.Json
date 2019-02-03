using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace blqw.JsonServices
{
    internal class JsonDictionary : NameObjectCollectionBase, IDictionary<string, object>, IDictionary
    {
        #region IEnumerable<KeyValuePair<string,object>> 成员

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            var length = Count;
            for (var i = 0; i < length; i++)
            {
                yield return new KeyValuePair<string, object>(BaseGetKey(i), BaseGet(i));
            }
        }

        #endregion IEnumerable<KeyValuePair<string,object>> 成员

        #region ICollection<KeyValuePair<string,object>> 成员

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => base.IsReadOnly;

        #endregion ICollection<KeyValuePair<string,object>> 成员

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            var length = Count;
            for (var i = 0; i < length; i++)
            {
                yield return new KeyValuePair<string, object>(BaseGetKey(i), BaseGet(i));
            }
        }

        #endregion IEnumerable 成员

        #region Methods

        public override string ToString()
        {
            return this.ToJsonString();
        }

        #endregion Methods

        #region IDictionary<string,object> 成员

        public object this[string key]
        {
            get { return BaseGet(key); }
            set
            {
                if (value == null)
                {
                    BaseRemove(key);
                }
                else
                {
                    BaseSet(key, value);
                }
            }
        }

        public void Add(string key, object value)
        {
            this[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return BaseGet(key) != null;
        }

        public bool Remove(string key)
        {
            BaseRemove(key);
            return true;
        }

        public bool TryGetValue(string key, out object value)
        {
            value = BaseGet(key);
            return value != null;
        }

        #endregion IDictionary<string,object> 成员

        #region ICollection<KeyValuePair<string,object>> 成员

        public void Add(KeyValuePair<string, object> item)
        {
            this[item.Key] = item.Value;
        }

        public void Clear()
        {
            BaseClear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            var value = BaseGet(item.Key);
            if (value == null)
                return false;
            return value == item.Value;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "index不能小于0");
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("");
            }
            var length = Count;
            for (var i = 0; i < length; i++)
            {
                array[index++] = new KeyValuePair<string, object>(BaseGetKey(i), BaseGet(i));
            }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        #endregion ICollection<KeyValuePair<string,object>> 成员

        #region IDictionary<string,object> 成员

        ICollection<string> IDictionary<string, object>.Keys => BaseGetAllKeys();

        ICollection<object> IDictionary<string, object>.Values => BaseGetAllValues();

        #endregion IDictionary<string,object> 成员

        #region IDictionary 成员

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.IsReadOnly => true;

        ICollection IDictionary.Keys => base.Keys;

        ICollection IDictionary.Values => BaseGetAllValues(typeof(object));

        object IDictionary.this[object key]
        {
            get { return BaseGet(Parse(key)); }
            set { BaseSet(Parse(key), value); }
        }

        void IDictionary.Add(object key, object value)
        {
            BaseAdd(Parse(key), value);
        }

        void IDictionary.Clear()
        {
            BaseClear();
        }

        bool IDictionary.Contains(object key)
        {
            return BaseGet(Parse(key)) != null;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() => new Enumerator(this);

        void IDictionary.Remove(object key)
        {
            BaseRemove(Parse(key));
        }

        private string Parse(object key)
        {
            var k = key as string;
            if (k == null && key != null)
            {
                throw new ArgumentException("key只能是字符串");
            }
            return k;
        }

        private class Enumerator : IDictionaryEnumerator, IObjectReference
        {
            #region Fields

            private int _index = -1;
            private JsonDictionary _jsonDictionary;

            #endregion Fields

            #region Constructors

            public Enumerator(JsonDictionary jsonDictionary)
            {
                _jsonDictionary = jsonDictionary;
            }

            #endregion Constructors

            #region Properties

            public object Current => this;

            public DictionaryEntry Entry => new DictionaryEntry(Key, Value);

            public object Key => _jsonDictionary.BaseGetKey(_index);

            public object Value => _jsonDictionary.BaseGet(_index);

            #endregion Properties

            #region Methods

            public Type GetCustomType() => typeof(NameObjectCollectionBase);

            public object GetRealObject(StreamingContext context) => _jsonDictionary;

            public bool MoveNext() => ++_index < _jsonDictionary.Count;

            public void Reset() => _index = -1;

            public object Unwrap() => _jsonDictionary;

            #endregion Methods
        }

        #endregion IDictionary 成员

        #region ICollection 成员

        int ICollection.Count
        {
            get { return base.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        #endregion ICollection 成员
    }
}