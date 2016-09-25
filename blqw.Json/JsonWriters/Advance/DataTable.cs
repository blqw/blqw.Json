using System;
using System.Data;

namespace blqw.Serializable.JsonWriters
{
    internal class DataTableWriter : IJsonWriter
    {
        public Type Type => typeof(DataTable);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var table = (DataTable) obj;
            var writer = args.Writer;
            var comma1 = new CommaHelper(writer);
            var columns = table.Columns;
            var length = columns.Count;
            args.BeginArray();
            for (int j = 0, count = table.Rows.Count; j < count; j++)
            {
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
                    args.WriterContainer.GetWriter<string>().Write(column.ColumnName, args);
                    writer.Write(':');
                    if (row.IsNull(column))
                    {
                        args.WriterContainer.GetNullWriter().Write(null, args);
                    }
                    else
                    {
                        var value = row[column];
                        args.WriterContainer.GetWriter(value.GetType()).Write(value, args);
                    }
                }
            }
            args.EndArray();
        }
    }
}