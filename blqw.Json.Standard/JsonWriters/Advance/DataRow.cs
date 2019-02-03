using System;
using System.Data;

namespace blqw.JsonServices.JsonWriters
{
    internal class DataRowWriter : IJsonWriter
    {
        public Type Type => typeof(DataRow);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var row = (DataRow)obj;
            var writer = args.Writer;
            var comma = new CommaHelper(writer);
            args.BeginObject();
            var columns = row.Table.Columns;
            for (int i = 0, length = columns.Count; i < length; i++)
            {
                var column = columns[i];
                if (args.IgnoreNullMember)
                {
                    if (row.IsNull(column))
                    {
                        continue;
                    }
                }

                comma.AppendCommaIgnoreFirst();
                args.Write(column.ColumnName);
                writer.Write(':');
                if (row.IsNull(column))
                {
                    args.WriteNull();
                }
                else
                {
                    var value = row[column];
                    args.WriteObject(value);
                }
            }
            args.EndObject();
        }
    }
}