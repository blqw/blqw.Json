using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace blqw.Serializable
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

        #endregion

        #region ICollection<KeyValuePair<string,object>> 成员

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => base.IsReadOnly;

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            var length = Count;
            for (var i = 0; i < length; i++)
            {
                yield return new KeyValuePair<string, object>(BaseGetKey(i), BaseGet(i));
            }
        }

        #endregion

        public override string ToString()
        {
            return this.ToJsonString();
        }

        #region IDictionary<string,object> 成员

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

        #endregion

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

        #endregion

        #region IDictionary<string,object> 成员

        ICollection<string> IDictionary<string, object>.Keys => BaseGetAllKeys();

        ICollection<object> IDictionary<string, object>.Values => BaseGetAllValues();

        #endregion

        #region IDictionary 成员

        private string Parse(object key)
        {
            var k = key as string;
            if (k == null && key != null)
            {
                throw new ArgumentException("key只能是字符串");
            }
            return k;
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

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.IsReadOnly => true;

        ICollection IDictionary.Keys => base.Keys;

        void IDictionary.Remove(object key)
        {
            BaseRemove(Parse(key));
        }

        ICollection IDictionary.Values => BaseGetAllValues(typeof(object));

        object IDictionary.this[object key]
        {
            get { return BaseGet(Parse(key)); }
            set { BaseSet(Parse(key), value); }
        }

        #endregion

        #region ICollection 成员

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

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

        #endregion
    }
}