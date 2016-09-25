using System;
using System.Collections.Generic;


namespace blqw.Serializable.JsonWriters
{
    internal class IListTWriter : IGenericJsonWriter
    {
        public Type Type => typeof(IList<>);

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

        public void Write(object obj, JsonWriterArgs args)
        {
            throw new NotImplementedException();
        }

        private class InnerWriter<T> : IJsonWriter
        {
            public Type Type { get; } = typeof(IList<T>);

            public void Write(object obj, JsonWriterArgs args)
            {
                if (obj == null)
                {
                    args.WriterContainer.GetNullWriter().Write(null, args);
                    return;
                }
                var writer = TypeService.IsImmutable<T>() ? args.WriterContainer.GetWriter<T>() : null;
                var list = (IList<T>)obj;
                if (list.Count == 0)
                {
                    args.BeginArray();
                    args.EndArray();
                    return;
                }
                args.BeginArray();
                args.WriteCheckLoop(list[0], writer);

                for (int i = 1, length = list.Count; i < length; i++)
                {
                    args.Common();
                    args.WriteCheckLoop(list[i], writer);
                }

                args.EndArray();
            }

        }
    }
}