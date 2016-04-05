using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    class JsonDictionary : NameObjectCollectionBase, IDictionary<string, object>, IDictionary
    {
        #region IDictionary<string,object> 成员

        public void Add(string key, object value)
        {
            this[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return base.BaseGet(key) != null;
        }

        public bool Remove(string key)
        {
            base.BaseRemove(key);
            return true;
        }

        public bool TryGetValue(string key, out object value)
        {
            value = base.BaseGet(key);
            return value != null;
        }


        public object this[string key]
        {
            get
            {
                return BaseGet(key);
            }
            set
            {
                if (value == null)
                {
                    base.BaseRemove(key);
                }
                else
                {
                    base.BaseSet(key, value);
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
            base.BaseClear();
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
                throw new ArgumentNullException("array");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "index不能小于0");
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException("");
            }
            var length = Count;
            for (int i = 0; i < length; i++)
            {
                array[index++] = new KeyValuePair<string, object>(base.BaseGetKey(i), base.BaseGet(i));
            }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        #endregion

        #region IDictionary<string,object> 成员


        ICollection<string> IDictionary<string, object>.Keys
        {
            get { return BaseGetAllKeys(); }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { return BaseGetAllValues(); }
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> 成员

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            var length = Count;
            for (int i = 0; i < length; i++)
            {
                yield return new KeyValuePair<string, object>(base.BaseGetKey(i), base.BaseGet(i));
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string,object>> 成员


        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return base.IsReadOnly; }
        }

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
            base.BaseAdd(Parse(key), value);
        }

        void IDictionary.Clear()
        {
            base.BaseClear();
        }

        bool IDictionary.Contains(object key)
        {
            return base.BaseGet(Parse(key)) != null;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return true; }
        }

        ICollection IDictionary.Keys
        {
            get { return base.Keys; }
        }

        void IDictionary.Remove(object key)
        {
            base.BaseRemove(Parse(key));
        }

        ICollection IDictionary.Values
        {
            get { return base.BaseGetAllValues(typeof(object)); }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return base.BaseGet(Parse(key));
            }
            set
            {
                base.BaseSet(Parse(key), value);
            }
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

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            var length = Count;
            for (int i = 0; i < length; i++)
            {
                yield return new KeyValuePair<string, object>(base.BaseGetKey(i), base.BaseGet(i));
            }
        }

        #endregion

        public override string ToString()
        {
            return Json.ToJsonString(this);
        }
    }
}
