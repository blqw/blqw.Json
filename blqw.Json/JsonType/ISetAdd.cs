using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    class SetAdd<K, V> : ISetAdd
    {
        public void ICollectionT(object target, object value)
        {
            ((ICollection<V>)target).Add((V)value);
        }

        public void IDictionary(object target, object key, object value)
        {
            ((IDictionary)target).Add(key, value);
        }

        public void IDictionaryT(object target, object key, object value)
        {
            ((IDictionary<K,V>)target).Add((K)key,(V)value);
        }

        public void IList(object target, object value)
        {
            ((IList)target).Add((V)value);
        }
        
    }

    interface ISetAdd
    {
        void IDictionaryT(object target, object key, object value);
        void IDictionary(object target, object key, object value);
        void IList(object target, object value);
        void ICollectionT(object target, object value);
    }
}
