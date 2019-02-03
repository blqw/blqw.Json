using System;
using System.CodeDom;
using System.Collections.Generic;


namespace blqw.JsonServices.JsonWriters
{
    public class IEnumerableTWriter : IGenericJsonWriter
    {
        public Type Type => typeof(IEnumerable<>);

        public object GetService(Type serviceType)
        {
            foreach (var item in serviceType.GetInterfaces())
            {
                if (item.IsGenericType
                    && item.IsGenericTypeDefinition == false
                    && item.GetGenericTypeDefinition() == Type)
                {
                    var t = typeof(InnerWriter<>).MakeGenericType(item.GetGenericArguments());
                    return (IJsonWriter)Activator.CreateInstance(t);
                }
            }
            if (serviceType.IsInterface)
            {
                if (serviceType.IsGenericType
                    && serviceType.IsGenericTypeDefinition == false
                    && serviceType.GetGenericTypeDefinition() == Type)
                {
                    var t = typeof(InnerWriter<>).MakeGenericType(serviceType.GetGenericArguments());
                    return (IJsonWriter)Activator.CreateInstance(t);
                }
            }
            throw new NotImplementedException();
        }

        public void Write(object obj, JsonWriterSettings args)
        {
            throw new NotImplementedException();
        }

        private class InnerWriter<T> : IJsonWriter
        {
            public Type Type { get; } = typeof(IEnumerable<T>);

            public void Write(object obj, JsonWriterSettings args)
            {
                if (obj == null)
                {
                    args.WriteNull();
                    return;
                }

                var wirter = TypeService.IsSealed<T>() ? args.Selector.Get<T>() : null;
                args.BeginArray();
                var ee = ((IEnumerable<T>)obj).GetEnumerator();
                if (ee.MoveNext())
                {
                    args.WriteCheckLoop(ee.Current, wirter);
                    while (ee.MoveNext())
                    {
                        args.Common();
                        args.WriteCheckLoop(ee.Current, wirter);
                    }
                }
                args.EndArray();
            }

        }
    }
}