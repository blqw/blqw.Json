using System;
using System.Collections.Generic;

namespace blqw
{
    /// <summary> 增强版键值对字典
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public class DictionaryEx<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary> 用户存储数据的字典
        /// </summary>
        private IDictionary<TKey, TValue> _items;
        /// <summary> 默认值
        /// </summary>
        private TValue _defaultValue;
        /// <summary> 用于获取值的委托
        /// </summary>
        private Converter<TKey, TValue> _getValue;
        /// <summary> 1返回_defaultValue, 2执行_getValue, 0抛出异常
        /// </summary>
        private int _mode = 0;

        #region 构造函数
        /// <summary> 初始化 DictionaryEx , key不存在时返回defaultValue
        /// </summary>
        /// <param name="defaultValue">默认值</param>
        public DictionaryEx(TValue defaultValue)
        {
            _items = new Dictionary<TKey, TValue>();
            _defaultValue = defaultValue;
            _mode = 1;
        }

        /// <summary> 
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="comparer"></param>
        public DictionaryEx(TValue defaultValue, IEqualityComparer<TKey> comparer)
        {
            _items = new Dictionary<TKey, TValue>(comparer);
            _defaultValue = defaultValue;
            _mode = 1;
        }

        /// <summary> 初始化 DictionaryEx , key不存在时返回defaultValue
        /// </summary>
        /// <param name="comparer">比较键时要使用对象,如果为null则使用默认比较方法</param>
        /// <param name="defaultValue">默认值</param>
        public DictionaryEx(IEqualityComparer<TKey> comparer, TValue defaultValue = default(TValue))
        {
            _items = new Dictionary<TKey, TValue>(comparer);
            _defaultValue = defaultValue;
            _mode = 1;
        }

        /// <summary> 初始化 DictionaryEx 只读集合, key不存在时返回defaultValue
        /// </summary>
        /// <param name="dictionary">内部字典</param>
        /// <param name="defaultValue">默认值</param>
        public DictionaryEx(IDictionary<TKey, TValue> dictionary, TValue defaultValue = default(TValue))
        {
            Assertor.AreNull(dictionary, "dictionary");
            _items = dictionary;
            IsReadOnly = true;
            _defaultValue = defaultValue;
            _mode = 1;
        }

        /// <summary> 初始化 DictionaryEx, key不存在时返回defaultValue
        /// </summary>
        /// <param name="dictionary">内部字典</param>
        /// <param name="isReadOnly">是否只读</param>
        /// <param name="defaultValue">默认值</param>
        public DictionaryEx(IDictionary<TKey, TValue> dictionary, bool isReadOnly, TValue defaultValue = default(TValue))
        {
            Assertor.AreNull(dictionary, "dictionary");
            _items = dictionary;
            IsReadOnly = isReadOnly;
            _defaultValue = defaultValue;
            _mode = 1;
        }

        /// <summary> 初始化 DictionaryEx 设定getValue委托,key不存在时执行委托,并加入集合
        /// </summary>
        /// <param name="getValue">获取值的委托</param>
        public DictionaryEx(Converter<TKey, TValue> getValue)
        {
            Assertor.AreNull(getValue, "getValue");
            _items = new Dictionary<TKey, TValue>();
            _getValue = getValue;
            _mode = 2;
        }

        /// <summary> 初始化 DictionaryEx 设定getValue委托,key不存在时执行委托,并加入集合
        /// </summary>
        /// <param name="getValue">获取值的委托</param>
        /// <param name="comparer">比较键时要使用对象,如果为null则使用默认比较方法</param>
        public DictionaryEx(Converter<TKey, TValue> getValue, IEqualityComparer<TKey> comparer)
        {
            Assertor.AreNull(getValue, "getValue");
            _items = new Dictionary<TKey, TValue>(comparer);
            _getValue = getValue;
            _mode = 2;
        }

        /// <summary> 初始化 DictionaryEx 设定getValue委托,key不存在时执行委托,并加入集合
        /// </summary>
        /// <param name="getValue">获取值的委托</param>
        /// <param name="isReadOnly">集合是否限制外部修改</param>
        public DictionaryEx(Converter<TKey, TValue> getValue, bool isReadOnly)
        {
            Assertor.AreNull(getValue, "getValue");
            _items = new Dictionary<TKey, TValue>();
            _getValue = getValue;
            IsReadOnly = isReadOnly;
            _mode = 2;
        }

        /// <summary> 初始化 DictionaryEx 设定getValue委托,key不存在时执行委托,并加入集合
        /// </summary>
        /// <param name="getValue">获取值的委托</param>
        /// <param name="comparer">比较键时要使用对象</param>
        /// <param name="isReadOnly">集合是否限制外部修改</param>
        public DictionaryEx(Converter<TKey, TValue> getValue, IEqualityComparer<TKey> comparer, bool isReadOnly)
        {
            Assertor.AreNull(getValue, "getValue");
            _items = new Dictionary<TKey, TValue>(comparer);
            _getValue = getValue;
            IsReadOnly = isReadOnly;
            _mode = 2;
        }

        /// <summary> 初始化 DictionaryEx 设定getValue委托,key不存在时执行委托,并加入集合
        /// </summary>
        /// <param name="getValue">获取值的委托</param>
        /// <param name="dictionary">内部字典</param>
        public DictionaryEx(Converter<TKey, TValue> getValue, IDictionary<TKey, TValue> dictionary)
        {
            Assertor.AreNull(getValue, "getValue");
            Assertor.AreNull(dictionary, "dictionary");
            _items = dictionary;
            _getValue = getValue;
            IsReadOnly = true;
            _mode = 2;
        }

        /// <summary> 初始化 DictionaryEx 设定getValue委托,key不存在时执行委托,并加入集合
        /// </summary>
        /// <param name="getValue">获取值的委托</param>
        /// <param name="dictionary">内部字典</param>
        /// <param name="isReadOnly">是否只读</param>
        public DictionaryEx(Converter<TKey, TValue> getValue, IDictionary<TKey, TValue> dictionary, bool isReadOnly)
        {
            _items = dictionary;
            _getValue = getValue;
            IsReadOnly = isReadOnly;
            _mode = 2;
        }

        /// <summary> 初始化 DictionaryEx 只读集合
        /// </summary>
        /// <param name="dictionary">内部字典</param>
        public DictionaryEx(IDictionary<TKey, TValue> dictionary)
        {
            Assertor.AreNull(dictionary, "dictionary");
            IsReadOnly = true;
            _items = dictionary;
            _mode = 0;
        }
        #endregion

        private TValue ReturnValue(TKey key)
        {
            switch (_mode)
            {
                case 1:
                    return _defaultValue;
                case 2:
                    var value = _getValue(key);
                    lock (this)
                    {
                        _items[key] = value;
                    }
                    return value;
                default:
                    throw new KeyNotFoundException();
            }
        }

        public void Add(TKey key, TValue value)
        {
            this[key] = value;
        }

        public bool ContainsKey(TKey key)
        {
            return _items.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _items.Keys; }
        }

        public bool Remove(TKey key)
        {
            Assertor.AreTrue(IsReadOnly, "集合为只读");
            return _items.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _items.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (_items.TryGetValue(key, out value))
                {
                    return value;
                }
                return ReturnValue(key);
            }
            set
            {
                Assertor.AreTrue(IsReadOnly, "集合为只读");
                _items[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this[item.Key] = item.Value;
        }

        public void Clear()
        {
            Assertor.AreTrue(IsReadOnly, "集合为只读");
            _items.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_items).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly { get; private set; }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
