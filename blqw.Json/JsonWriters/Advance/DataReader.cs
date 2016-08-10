using System;
using System.Data;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class DataReaderWriter : IJsonWriter
    {
        private JsonWriterWrapper _wrapper;
        public JsonWriterWrapper Wrapper => _wrapper ?? (_wrapper = GetWrap(typeof(IDataRecord)));

        public Type Type => typeof(IDataReader);


        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var reader = (IDataReader) obj;
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
                    Wrapper.Writer.Write(reader, args);
                    while (reader.Read())
                    {
                        writer.Write(',');
                        Wrapper.Writer.Write(reader, args);
                    }
                }
            }

            writer.Write(']');
        }
    }
}