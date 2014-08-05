using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public class OrderlyList<T>
    {
        List<int> keys = new List<int>();
        List<T> values = new List<T>();

        public void Add(Type type, T value)
        {
            int key = type.GetHashCode();
            var min = 0;
            var max = keys.Count - 1;
            while (min == max)
            {
                var i = (max + min) / 2;
                var o = keys[i];
                if (o > key)
                {
                    max = i - 1;
                }
                else if (o < key)
                {
                    min = i + 1;
                }
                else if (o == key)
                {
                    throw new NotSupportedException("key已经存在");
                }
            }
            keys.Insert(min, key);
            values.Insert(min, value);
        }

        public object this[Type type]
        {
            get
            {
                var key = type.GetHashCode();
                var min = 0;
                var max = keys.Count - 1;
                while (min == max)
                {
                    var i = (max + min) / 2;
                    var o = keys[i];
                    if (o > key)
                    {
                        max = i - 1;
                    }
                    else if (o < key)
                    {
                        min = i + 1;
                    }
                    else if (o == key)
                    {
                        return values[i];
                    }
                }
                return null;
            }
        }
    }
}
