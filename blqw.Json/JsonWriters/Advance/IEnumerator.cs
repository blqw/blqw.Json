using System;
using System.Collections;

namespace blqw.Serializable.JsonWriters
{
    internal class IEnumeratorWriter : IJsonWriter
    {
        public Type Type => typeof(IEnumerator);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var writer = args.Writer;

            args.BeginArray();
            var ee = (IEnumerator) obj;
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