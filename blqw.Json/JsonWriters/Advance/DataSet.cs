using System;
using System.Data;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class DataSetWrite : IJsonWriter
    {
        private JsonWriterWrapper _wrapper;
        public JsonWriterWrapper Wrapper => _wrapper ?? (_wrapper = GetWrap(Type));
        public Type Type => typeof(DataSet);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                NullWriter.Write(null, args);
                return;
            }
            var ds = (DataSet) obj;
            var writer = args.Writer;
            var comma = new CommaHelper(writer);
            writer.Write('{');
            for (int i = 0, length = ds.Tables.Count; i < length; i++)
            {
                comma.AppendCommaIgnoreFirst();
                var table = ds.Tables[i];
                JsonWriterContainer.StringWriter.Write(table.TableName, args);
                writer.Write(':');
                Wrapper.Writer.Write(table, args);
            }
            writer.Write('}');
        }
    }
}