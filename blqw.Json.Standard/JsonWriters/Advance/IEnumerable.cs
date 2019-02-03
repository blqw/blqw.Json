using System;
using System.Collections;

namespace blqw.JsonServices.JsonWriters
{
    internal class IEnumerableWriter : IJsonWriter
    {
        public Type Type => typeof(IEnumerable);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var writer = args.Writer;

            args.BeginArray();
            var ee = ((IEnumerable) obj).GetEnumerator();
            if (ee.MoveNext())
            {
                args.WriteCheckLoop(ee.Current, null);
                while (ee.MoveNext())
                {
                    args.Common();
                    args.WriteCheckLoop(ee.Current, null);
                }
            }
            args.EndArray();
        }
    }
}