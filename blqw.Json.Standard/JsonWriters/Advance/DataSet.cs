using System;
using System.Data;

namespace blqw.JsonServices.JsonWriters
{
    internal class DataSetWrite : IJsonWriter
    {
        public Type Type => typeof(DataSet);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
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
                args.Write(table.TableName);
                writer.Write(':');
                args.WriteObject(table);
            }
            args.EndObject();
        }
    }
}