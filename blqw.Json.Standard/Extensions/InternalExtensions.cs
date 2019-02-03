using blqw.ConvertServices;
using System;
using System.Collections;
using System.Collections.Generic;

namespace blqw.JsonServices
{
    internal static class InternalExtensions
    {
        public static IServiceProvider Aggregate(this IServiceProvider provider, params IServiceProvider[] providers)
        {
            if (providers == null || providers.Length == 0)
            {
                return provider;
            }
            if (providers.Length == 1)
            {
                return providers[0];
            }
            if (provider != null)
            {
                var arr = new IServiceProvider[providers.Length + 1];
                Array.Copy(providers, 0, arr, 1, providers.Length);
                arr[0] = provider;
                providers = arr;
            }
            return new AggregateServicesProvider(providers);
        }

        /// <summary>
        /// 在一个可枚举的对象集合中循环执行指定的动作
        /// </summary>
        /// <typeparam name="T">集合项的类型</typeparam>
        /// <param name="enumerable">可枚举对象集合</param>
        /// <param name="action">执行动作</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null || action == null)
            {
                return;
            }

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// 在一个可枚举的对象集合中循环 安全的执行指定的动作(执行<paramref name="action"/>不会产生异常)
        /// </summary>
        /// <param name="enumerable">可枚举对象集合</param>
        /// <param name="action">执行动作</param>
        public static void SafeForEach<T>(this IEnumerable enumerable, Action<T> action)
        {
            if (enumerable == null || action == null)
            {
                return;
            }
            foreach (var item in enumerable)
            {
                if (item is T t)
                {
                    try
                    {
                        action(t);
                    }
                    catch
                    {
                    }
                }

            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] Slice(this byte[] bytes, int size)
        {
            if (bytes == null || size <= 0)
            {
                return new byte[size];
            }
            if (bytes.Length == size)
            {
                return bytes;
            }
            var bs = new byte[size];
            Array.Copy(bytes, 0, bs, 0, size);
            return bs;
        }


        /// <summary>
        /// 判断是否为16进制格式的字符串,如果为true,将参数s的前缀(0x/&amp;h)去除
        /// </summary>
        /// <param name="str"> 需要判断的字符串 </param>
        /// <returns> </returns>
        public static string ToHex(this string str)
        {
            if ((str == null) || (str.Length == 0))
            {
                return null;
            }
            var c = str[0];
            if (char.IsWhiteSpace(c)) //有空格去空格
            {
                str = str.TrimStart();
            }
            if (str.Length > 2) //判断是否是0x 或者 &h 开头
            {
                switch (c)
                {
                    case '0':
                        switch (str[1])
                        {
                            case 'x':
                            case 'X':
                                str = str.Remove(0, 2);
                                return str;
                            default:
                                return null;
                        }
                    case '&':
                        switch (str[1])
                        {
                            case 'h':
                            case 'H':
                                str = str.Remove(0, 2);
                                return str;
                            default:
                                return null;
                        }
                    default:
                        return null;
                }
            }
            return null;
        }

        public static string ToBin(this string str)
        {
            throw new NotImplementedException();
        }

        public static string ToOct(this string str)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取指定类型的转换器
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="context">转换上下文</param>
        /// <returns></returns>
        public static IConvertor<T> Get<T>(this IConvertorSelector selector, ConvertContext context)
        {
            var conv = selector.Get(typeof(T), context);
            try
            {
                return (IConvertor<T>)conv;
            }
            catch (Exception e)
            {
                throw new InvalidCastException(conv.GetType().GetFriendlyName() + " 无法转为 " + typeof(IConvertor<T>).GetFriendlyName(), e);
            }
        }


        public static ConvertResult<T> ConvertResult<T>(this T value) => new ConvertResult<T>(value);
    }
}
