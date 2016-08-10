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
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;

            writer.Write('[');
            var ee = (IEnumerator) obj;
            if (ee.MoveNext())
            {
                args.WriteCheckLoop(ee.Current, null);
                while (ee.MoveNext())
                {
                    args.Writer.Write(',');
                    args.WriteCheckLoop(ee.Current, null);
                }
            }
            writer.Write(']');
        }
    }
}