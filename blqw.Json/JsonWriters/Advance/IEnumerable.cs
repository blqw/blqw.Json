using System;
using System.Collections;

namespace blqw.Serializable.JsonWriters
{
    internal class IEnumerableWriter : IJsonWriter
    {
        public Type Type => typeof(IEnumerable);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
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