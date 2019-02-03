using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace blqw.JsonServices
{
    /// <summary>
    /// 用于处于Type类型为Key时的缓存
    /// </summary>
    internal class TypeCache<T>
    {
        /// <summary>
        /// 标准的字典缓存
        /// </summary>
        private readonly ConcurrentDictionary<Type, T> _cache = new ConcurrentDictionary<Type, T>();
        private readonly Func<Type, T> _creator;

        public TypeCache(Func<Type, T> creator) => _creator = creator;

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key"> 缓存键 </param>
        /// <param name="item"> 缓存值 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="key" /> is <see langword="null" />. </exception>
        /// <exception cref="TargetInvocationException"> 初始化泛型缓存时出现错误 </exception>
        public T Set(Type key, T item)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _cache[key] = item;
            if (typeof(void) != key && (!key.IsGenericType || key.IsConstructedGenericType)) //如果是泛型定义类型 就不能加入泛型缓存
            {
                try
                {
                    typeof(GenericCache<>)
                        .MakeGenericType(typeof(T), key)
                        .GetMethod("Initialize")?
                        .Invoke(null, new object[] { item });
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex, $"{nameof(TypeCache<T>)}.{nameof(Set)}异常(T:{typeof(T).FullName})");
                    throw new TargetInvocationException("初始化泛型缓存时出现错误", ex);
                }
            }
            return item;
        }

        /// <summary>
        /// 获取缓存值,如果指定缓存不存在则使用create参数获取缓存
        /// </summary>
        /// <param name="key"> 缓存键 </param>
        /// <param name="create"> 用于创建缓存项委托 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="key" /> is <see langword="null" />. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="create" /> is <see langword="null" />. </exception>
        /// <exception cref="OverflowException"> 缓存中已包含元素的最大数目 (<see cref="F:System.Int32.MaxValue" />)。 </exception>
        /// <exception cref="TargetInvocationException"> 初始化泛型缓存时出现错误 </exception>
        /// <exception cref="TargetException"> <paramref name="create" /> 方法内部异常. </exception>
        public T Get(Type key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _cache.GetOrAdd(key, t =>
            {
                try
                {
                    var item = _creator(t);
                    Set(t, item);
                    return item;
                }
                catch (Exception ex)
                {
                    throw new TargetException($"{nameof(_creator)}方法内部异常", ex);
                }
            });
        }

        /// <summary>
        /// 获取缓存值,如果指定缓存不存在则使用create参数获取缓存
        /// </summary>
        /// <typeparam name="TKey"> 缓存键的类型 </typeparam>
        /// <param name="create"> 用于创建缓存项委托 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="create" /> is <see langword="null" />. </exception>
        /// <exception cref="TargetException"> <paramref name="create" /> 方法内部异常. </exception>
        /// <exception cref="TargetInvocationException"> 初始化泛型缓存时出现错误. </exception>
        /// <exception cref="OverflowException"> 缓存中已包含元素的最大数目 (<see cref="F:System.Int32.MaxValue" />)。 </exception>
        public T Get<TKey>()
        {
            if (GenericCache<TKey>.IsInitialized)
                return GenericCache<TKey>.Item;
            var key = typeof(TKey);
            return _cache.GetOrAdd(key, t =>
            {
                try
                {
                    var item = _creator(t);
                    GenericCache<TKey>.Initialize(item);
                    return item;
                }
                catch (Exception ex)
                {
                    throw new TargetException($"{nameof(_creator)}方法内部异常", ex);
                }
            });
        }

        /// <summary>
        /// 泛型缓存
        /// </summary>
        private static class GenericCache<TKey>
        {
            /// <summary>
            /// 缓存项
            /// </summary>
            public static T Item { get; private set; }

            /// <summary>
            /// 是否已经初始化完成
            /// </summary>
            public static bool IsInitialized { get; private set; }

            /// <summary>
            /// 初始化泛型缓存
            /// </summary>
            /// <param name="item"> </param>
            public static void Initialize(T item)
            {
                IsInitialized = true;
                Item = item;
            }
        }
    }
}