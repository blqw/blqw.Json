using System;
using System.Data;

namespace blqw.JsonServices.JsonWriters
{
    internal class DataTableWriter : IJsonWriter
    {
        public Type Type => typeof(DataTable);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var table = (DataTable)obj;
            var writer = args.Writer;
            var comma1 = new CommaHelper(writer);
            var columns = table.Columns;
            var length = columns.Count;
            args.BeginArray();
            for (int j = 0, count = table.Rows.Count; j < count; j++)
            {
                args.BeginObject();
                comma1.AppendCommaIgnoreFirst();
                var row = table.Rows[j];
                var comma = new CommaHelper(writer);
                for (var i = 0; i < length; i++)
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
            args.EndArray();
        }
    }
}