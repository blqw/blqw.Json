using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    class DataReaderWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(IDataReader);

        private IJsonWriterWrap _wrap = GetWrap(typeof(IDataRecord));

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var reader = (IDataReader)obj;
            if (reader.IsClosed)
            {
                throw new NotImplementedException("IDataReader已经关闭");
            }
            writer.Write('[');

            if (reader.FieldCount == 1)
            {
                var comma = new CommaHelper(writer);
                while (reader.Read())
                {
                    var value = reader.GetValue(0);
                    if (args.IgnoreNullMember)
                    {
                        if (value == null || value is DBNull)
                            continue;
                    }

                    comma.AppendCommaIgnoreFirst();
                    JsonWriterContainer.Write(value, args);
                }
            }
            else
            {
                if (reader.Read())
                {
                    _wrap.Writer.Write(reader, args);
                    while (reader.Read())
                    {
                        writer.Write(',');
                        _wrap.Writer.Write(reader, args);
                    }
                }
            }

            writer.Write(']');
        }
    }
}
