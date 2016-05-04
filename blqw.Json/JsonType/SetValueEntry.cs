using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    static class SetValueEntryFactory
    {
        class SetValueEntry<K, V> : ISetValueEntry
        {
            public void ICollectionT(object target, object value)
            {
                ((ICollection<V>)target).Add((V)value);
            }

            public void IDictionary(object target, object key, object value)
            {
                ((IDictionary)target)[key] = value;
            }

            public void IDictionaryT(object target, object key, object value)
            {
                ((IDictionary<K, V>)target)[(K)key] = (V)value;
            }

            public void IList(object target, object value)
            {
                ((IList)target).Add((V)value);
            }

        }
        public static ISetValueEntry Create(Type keyType, Type valueType)
        {
            return ((ISetValueEntry)Activator.CreateInstance(typeof(SetValueEntry<,>).MakeGenericType(keyType, valueType)));
        }

        public static ISetValueEntry Create(Type valueType)
        {
            return ((ISetValueEntry)Activator.CreateInstance(typeof(SetValueEntry<,>).MakeGenericType(typeof(object), valueType)));
        }
    }


    interface ISetValueEntry
    {
        void IDictionaryT(object target, object key, object value);
        void IDictionary(object target, object key, object value);
        void IList(object target, object value);
        void ICollectionT(object target, object value);
    }
}
