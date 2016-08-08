using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class IListWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(IList);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var list = (IList)obj;
            if (list.Count == 0)
            {
                writer.Write("[]");
                return;
            }
            writer.Write('[');
            args.WriteCheckLoop(list[0], null);

            for (int i = 1, length = list.Count; i < length; i++)
            {
                args.Writer.Write(',');
                args.WriteCheckLoop(list[i], null);
            }

            writer.Write(']');
        }
    }
}