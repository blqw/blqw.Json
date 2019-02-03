using System.Collections;
using System.Collections.Generic;

namespace blqw.JsonServices
{
    /// <summary>
    /// <see cref="IAddOrSet"/> 接口实现
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    internal sealed class AddOrSet<K, V> : IAddOrSet
    {
        /// <summary>
        /// 将<see cref="value"/>添加到泛型集合
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void ICollectionT(object target, object value) => ((ICollection<V>) target).Add((V) value);

        /// <summary>
        /// 将<see cref="key"/>和<see cref="value"/>添加到非泛型字典
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void IDictionary(object target, object key, object value) => ((IDictionary) target).Add(key, value);

        /// <summary>
        /// 将<see cref="key"/>和<see cref="value"/>添加到泛型字典
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void IDictionaryT(object target, object key, object value) => ((IDictionary<K, V>) target).Add((K) key, (V) value);

        /// <summary>
        /// 将<see cref="value"/>添加到非泛型集合
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void IList(object target, object value) => ((IList) target).Add((V) value);
    }

    /// <summary>
    /// 该接口用于定义Set或Add行为
    /// </summary>
    internal interface IAddOrSet
    {
        /// <summary>
        /// 将<see cref="key"/>和<see cref="value"/>添加到泛型字典
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void IDictionaryT(object target, object key, object value);
        /// <summary>
        /// 将<see cref="key"/>和<see cref="value"/>添加到非泛型字典
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void IDictionary(object target, object key, object value);
        /// <summary>
        /// 将<see cref="value"/>添加到非泛型集合
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        void IList(object target, object value);
        /// <summary>
        /// 将<see cref="value"/>添加到泛型集合
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        void ICollectionT(object target, object value);
    }
}