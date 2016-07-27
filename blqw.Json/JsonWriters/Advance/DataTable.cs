using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class DataTableWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(DataTable);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var table = (DataTable)obj;
            var writer = args.Writer;
            var comma1 = new CommaHelper(writer);
            var columns = table.Columns;
            var length = columns.Count;
            writer.Write('[');
            for (int j = 0, count = table.Rows.Count; j < count; j++)
            {
                comma1.AppendCommaIgnoreFirst();
                var row = table.Rows[j];
                var comma = new CommaHelper(writer);
                for (int i = 0; i < length; i++)
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
                    JsonWriterContainer.StringWriter.Write(column.ColumnName, args);
                    writer.Write(':');
                    if (row.IsNull(column))
                    {
                        JsonWriterContainer.NullWriter.Write(null, args);
                    }
                    else
                    {
                        JsonWriterContainer.Write(row[column], args);
                    }
                }
            }

            writer.Write(']');
        }
    }
}
