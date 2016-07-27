using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class SerializationInfoWrite : IJsonWriter
    {
        public Type Type { get; } = typeof(SerializationInfo);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (SerializationInfo)obj;
            var comma = new CommaHelper(writer);
            foreach (var item in value)
            {
                if (args.IgnoreNullMember)
                {
                    if (item.Value == null || item.Value is DBNull)
                    {
                        continue;
                    }
                }

                comma.AppendCommaIgnoreFirst();

                JsonWriterContainer.StringWriter.Write(item.Name, args);
                writer.Write(':');
                JsonWriterContainer.Write(item.Value, args);
            }


            throw new NotImplementedException();
        }
    }
}
