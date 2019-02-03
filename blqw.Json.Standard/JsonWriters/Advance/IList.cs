using System;
using System.Collections;

namespace blqw.JsonServices.JsonWriters
{
    internal class IListWriter : IJsonWriter
    {
        public Type Type => typeof(IList);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var writer = args.Writer;
            var list = (IList) obj;
            if (list.Count == 0)
            {
                writer.Write("[]");
                return;
            }
            args.BeginArray();
            args.WriteCheckLoop(list[0], null);

            for (int i = 1, length = list.Count; i < length; i++)
            {
                args.Common();
                args.WriteCheckLoop(list[i], null);
            }
            args.EndArray();
        }
    }
}