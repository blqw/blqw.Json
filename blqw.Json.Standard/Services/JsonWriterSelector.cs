using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;

namespace blqw.JsonServices
{
    /// <summary>
    /// <see cref="IJsonWriter" /> 提供程序
    /// </summary>
    public sealed class JsonWriterSelector
    {
        private readonly ConcurrentDictionary<Type, IJsonWriter> _writers;
        private readonly IComparer<Type> _typeComparer;
        class Cache<T>
        {
            public static IJsonWriter Item { get; set; }
        }

        public JsonWriterSelector(IEnumerable<IJsonWriter> writers, IComparer<Type> typeComparer)
        {
            _typeComparer = typeComparer ?? throw new ArgumentNullException(nameof(typeComparer));
            if (writers == null)
            {
                throw new ArgumentNullException(nameof(writers));
            }
            _writers = new ConcurrentDictionary<Type, IJsonWriter>();
            foreach (var item in writers)
            {
                Set(item);
            }
        }

        private void Set(IJsonWriter writer)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            if (_writers.TryAdd(writer.Type, writer) && writer.Type != typeof(void))
            {
                if (!writer.Type.IsGenericType || writer.Type.IsConstructedGenericType)
                {
                    typeof(Cache<>).MakeGenericType(writer.Type).GetProperty("Item", flags).SetValue(null, writer);
                }
                if (writer.Type.IsValueType && Nullable.GetUnderlyingType(writer.Type) == null)
                {
                    Set(NullableWriter.Create(writer));
                }
            }
        }

        private sealed class NullableWriter : IJsonWriter

        {
            public static NullableWriter Create(IJsonWriter item) => new NullableWriter(item);
            private readonly IJsonWriter _item;

            private NullableWriter(IJsonWriter item)
            {
                _item = item;
                Type = typeof(Nullable<>).MakeGenericType(item.Type);
            }

            public Type Type { get; }

            public void Write(object obj, JsonWriterSettings settings)
            {
                if (obj == null || obj is DBNull)
                {
                    settings.WriteObject(obj);
                    return;
                }
                _item.Write(obj, settings);
            }

        }

        public JsonWriterSelector(IServiceProvider provider)
            : this(provider?.GetServices<IJsonWriter>() ?? throw new ArgumentNullException(nameof(provider)),
                  provider?.GetService<IComparer<Type>>() ?? throw new ArgumentNullException(nameof(provider)))
        {

        }

        /// <summary>
        /// 获取指定类型的写入器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"> 字典中已包含元素的最大数目 (<see cref="F:System.Int32.MaxValue" />)。 </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is <see langword="null" />. </exception>
        public IJsonWriter Get(Type type) => Match(type).First();

        /// <summary>
        /// 获取指定类型的写入器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"> 字典中已包含元素的最大数目 (<see cref="F:System.Int32.MaxValue" />)。 </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is <see langword="null" />. </exception>
        public IJsonWriter Get<T>() => Cache<T>.Item ?? Match(typeof(T)).First();

        /// <summary>
        /// 获取所有匹配类型的服务组件
        /// 获取所有匹配类型的服务组件
        /// </summary>
        /// <param name="serviceType"> </param>
        /// <returns> </returns>
        public IEnumerable<IJsonWriter> Match(Type serviceType)
        {

            //精确匹配当前类 或 泛型定义 (优先级最高)
            if (_writers.TryGetValue(serviceType, out var item))
            {
                yield return item;
            }
            else
            {
                item = MatchGeneric(serviceType);
                if (item != null)
                {
                    yield return item;
                }
            }

            //匹配父类和接口
            var baseTypes = GetBaseType(serviceType)
                                .Union(serviceType.GetInterfaces())
                                .OrderByDescending(it => it, _typeComparer);

            foreach (var interfaceType in baseTypes)
            {
                if (_writers.TryGetValue(interfaceType, out item))
                {
                    yield return item;
                }
                else
                {
                    item = MatchGeneric(interfaceType);
                    if (item != null)
                    {
                        yield return item;
                    }
                }
            }

            //匹配object定义
            _writers.TryGetValue(typeof(object), out item);
            yield return item;
        }

        /// <summary>
        /// 枚举所有父类
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        private static IEnumerable<Type> GetBaseType(Type type)
        {
            var baseType = type.BaseType ?? typeof(object);
            while (baseType != typeof(object))
            {
                yield return baseType;
                baseType = baseType.BaseType ?? typeof(object);
            }
        }

        /// <summary>
        /// 获取与 <paramref name="genericType" /> 的泛型定义类型匹配的 <see cref="ServiceItem" />,如果
        /// <paramref name="genericType" /> 不是泛型,返回 null
        /// </summary>
        /// <param name="genericType"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns> </returns>
        private IJsonWriter MatchGeneric(Type genericType)
        {
            if (genericType.IsGenericType && !genericType.IsConstructedGenericType)
            {
                if (_writers.TryGetValue(genericType.GetGenericTypeDefinition(), out var item))
                {
                    return item;
                }
            }
            return null;
        }
    }
}