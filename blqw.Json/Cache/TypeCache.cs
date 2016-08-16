using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace blqw.Serializable
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

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key"> 缓存键 </param>
        /// <param name="item"> 缓存值 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="key" /> is <see langword="null" />. </exception>
        /// <exception cref="TargetInvocationException"> 初始化泛型缓存时出现错误 </exception>
        [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
        public void Set(Type key, T item)
        {
            Debug.Assert(_cache != null, "_cache != null");
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _cache[key] = item;
            if (key.IsGenericTypeDefinition || (typeof(void) == key)) //如果是泛型定义类型 就不能加入泛型缓存
                return;
            try
            {
                typeof(GenericCache<>)
                    .MakeGenericType(typeof(T), key)
                    .GetMethod("Initialize")?
                    .Invoke(null, new object[] {item});
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, $"{nameof(TypeCache<T>)}.{nameof(Set)}异常(T:{typeof(T).FullName})");
                throw new TargetInvocationException("初始化泛型缓存时出现错误", ex);
            }
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"> 缓存键 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="key" /> is <see langword="null" />. </exception>
        public T Get(Type key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            T item;
            Debug.Assert(_cache != null, "_cache != null");
            return _cache.TryGetValue(key, out item) ? item : default(T);
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="TKey"> 缓存键的类型 </typeparam>
        public T Get<TKey>()
        {
            return GenericCache<TKey>.Item;
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
        public T GetOrCreate(Type key, Func<Type, T> create)
        {
            Debug.Assert(_cache != null, "_cache != null");

            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (create == null)
                throw new ArgumentNullException(nameof(create));


            return _cache.GetOrAdd(key, t =>
            {
                T item;
                try
                {
                    item = create(key);
                }
                catch (Exception ex)
                {
                    throw new TargetException($"{nameof(create)}方法内部异常", ex);
                }
                Set(key, item);
                return item;
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
        public T GetOrCreate<TKey>(Func<Type, T> create)
        {
            if (create == null)
                throw new ArgumentNullException(nameof(create));

            if (GenericCache<TKey>.IsInitialized)
                return GenericCache<TKey>.Item;
            var key = typeof(TKey);
            return _cache.GetOrAdd(key, t =>
            {
                T item;
                try
                {
                    item = create(key);
                }
                catch (Exception ex)
                {
                    throw new TargetException($"{nameof(create)}方法内部异常", ex);
                }
                Set(key, item);
                return item;
            });
        }

        /// <summary>
        /// 泛型缓存
        /// </summary>
        // ReSharper disable once UnusedTypeParameter (泛型缓存)
        private static class GenericCache<TKey>
        {
            /// <summary>
            /// 缓存项
            /// </summary>
            public static T Item { get; private set; }

            /// <summary>
            /// 是否已经初始化完成
            /// </summary>
            // ReSharper disable once StaticMemberInGenericType
            public static bool IsInitialized { get; private set; }

            /// <summary>
            /// 初始化泛型缓存
            /// </summary>
            /// <param name="item"> </param>
            /// <exception cref="NotSupportedException"> 重复初始化 </exception>
            // ReSharper disable once UnusedMember.Local (反射调用)
            public static void Initialize(T item)
            {
                if (IsInitialized)
                    throw new NotSupportedException("重复初始化");
                IsInitialized = true;
                Item = item;
            }
        }
    }
}