using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public class OrderlyList<T>
    {
        List<int> _keys = new List<int>();
        List<T> _values = new List<T>();
        
        public void Add(int hashCode, T value)
        {
            var min = 0;
            lock (_keys)
            {
                var max = _keys.Count - 1;
                while (min <= max)
                {
                    var i = (max + min) / 2;
                    var o = _keys[i];
                    if (o > hashCode)
                    {
                        max = i - 1;
                    }
                    else if (o < hashCode)
                    {
                        min = i + 1;
                    }
                    else if (o == hashCode)
                    {
                        var t = _values[i];
                        if (object.Equals(t, value))
                        {
                            return;
                        }
                        throw new NotSupportedException("key已经存在");
                    }
                }
                _keys.Insert(min, hashCode);
                _values.Insert(min, value);
            }
        }

        public T this[int hashCode]
        {
            get
            {
                var min = 0;
                var max = _keys.Count - 1;
                while (min <= max)
                {
                    var i = (max + min) / 2;
                    var o = _keys[i];
                    if (o > hashCode)
                    {
                        max = i - 1;
                    }
                    else if (o < hashCode)
                    {
                        min = i + 1;
                    }
                    else if (o == hashCode)
                    {
                        return _values[i];
                    }
                }
                return default(T);
            }
        }
    }
}
