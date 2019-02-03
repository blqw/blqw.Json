using System;
using System.Collections;

namespace blqw.JsonServices.JsonWriters
{
    internal class IEnumeratorWriter : IJsonWriter
    {
        public Type Type => typeof(IEnumerator);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
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