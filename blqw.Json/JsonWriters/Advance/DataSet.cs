using System;
using System.Data;

namespace blqw.Serializable.JsonWriters
{
    internal class DataSetWrite : IJsonWriter
    {
        public Type Type => typeof(DataSet);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var ds = (DataSet) obj;
            var writer = args.Writer;
            var comma = new CommaHelper(writer);
            args.BeginObject();
            for (int i = 0, length = ds.Tables.Count; i < length; i++)
            {
                comma.AppendCommaIgnoreFirst();
                var table = ds.Tables[i];
                args.WriterContainer.GetWriter<string>().Write(table.TableName, args);
                writer.Write(':');
                args.WriterContainer.GetWriter(Type).Write(table, args);
            }
            args.EndObject();
        }
    }
}