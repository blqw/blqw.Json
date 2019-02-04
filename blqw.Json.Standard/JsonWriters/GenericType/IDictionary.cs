﻿using System;
using System.Collections.Generic;
using System.IO;


namespace blqw.JsonServices.JsonWriters
{
    internal class IDictionaryTWriter : IGenericJsonWriter
    {
        public Type Type => typeof(IDictionary<,>);

        public IJsonWriter MakeGenericType(Type genericType)
        {
            foreach (var item in genericType.GetInterfaces())
            {
                if (item.IsGenericType
                    && item.IsGenericTypeDefinition == false
                    && item.GetGenericTypeDefinition() == Type)
                {
                    var t = typeof(InnerWriter<,>).MakeGenericType(item.GetGenericArguments());
                    return (IJsonWriter)Activator.CreateInstance(t);
                }
            }
            if (genericType.IsInterface)
            {
                if (genericType.IsGenericType
                    && genericType.IsGenericTypeDefinition == false
                    && genericType.GetGenericTypeDefinition() == Type)
                {
                    var t = typeof(InnerWriter<,>).MakeGenericType(genericType.GetGenericArguments());
                    return (IJsonWriter)Activator.CreateInstance(t);
                }
            }
            throw new NotImplementedException();
        }

        public void Write(object obj, JsonWriterSettings args)
        {
            throw new NotImplementedException();
        }

        private class InnerWriter<TKey, TValue> : IJsonWriter
        {
            public Type Type { get; } = typeof(IDictionary<TKey, TValue>);

            public readonly bool Sealed = (typeof(TValue).IsValueType || typeof(TValue).IsSealed) && typeof(TValue).IsGenericTypeDefinition;

            /// <exception cref="IOException"> 发生 I/O 错误。</exception>
            /// <exception cref="ObjectDisposedException"> <see cref="T:System.IO.TextWriter" /> 是关闭的。</exception>
            public void Write(object obj, JsonWriterSettings args)
            {
                if (obj == null)
                {
                    args.WriteNull();
                    return;
                }
                var writer = TypeService.IsSealed<TValue>() ? args.Selector.Get<TValue>() : null;

                args.BeginObject();
                var comma = new CommaHelper(args.Writer);
                foreach (var item in (IDictionary<TKey, TValue>)obj)
                {
                    var value = item.Value;
                    if (args.IgnoreNullMember)
                    {
                        if (value == null || value is DBNull)
                        {
                            continue;
                        }
                    }
                    comma.AppendCommaIgnoreFirst();

                    args.Write(item.Key as string ?? item.Key.To<string>());
                    args.Colon();
                    args.WriteCheckLoop(value, writer);
                }

                args.EndObject();
            }
        }
    }
}