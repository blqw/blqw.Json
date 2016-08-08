using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class IEnumerableWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(IEnumerable);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;

            writer.Write('[');
            var ee = ((IEnumerable)obj).GetEnumerator();
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
