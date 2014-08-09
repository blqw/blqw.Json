using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public class OrderlyList<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        List<TKey> _keys = new List<TKey>();
        List<TValue> _values = new List<TValue>();

        public void Add(TKey key, TValue value)
        {
            var min = 0;
            lock (_keys)
            {
                var max = _keys.Count - 1;
                while (min <= max)
                {
                    var i = (max + min) / 2;
                    var k = _keys[i];
                    var r = k.CompareTo(key);
                    if (r > 0)
                    {
                        max = i - 1;
                    }
                    else if (r == 0)
                    {
                        var t = _values[i];
                        if (object.Equals(t, value))
                        {
                            return;
                        }
                        throw new NotSupportedException("key已经存在");
                    }
                    else
                    {
                        min = i + 1;
                    }
                }
                _keys.Insert(min, key);
                _values.Insert(min, value);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                var min = 0;
                var max = _keys.Count - 1;
                while (min <= max)
                {
                    var i = (max + min) / 2;
                    var k = _keys[i];
                    var r = k.CompareTo(key);
                    if (r > 0)
                    {
                        max = i - 1;
                    }
                    else if (r == 0)
                    {
                        return _values[i];
                    }
                    else
                    {
                        min = i + 1;
                    }
                }
                return default(TValue);
            }
        }
    }
}
