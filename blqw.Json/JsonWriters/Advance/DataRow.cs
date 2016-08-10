using System;
using System.Data;

namespace blqw.Serializable.JsonWriters
{
    internal class DataRowWriter : IJsonWriter
    {
        public Type Type => typeof(DataRow);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var row = (DataRow) obj;
            var writer = args.Writer;
            var comma = new CommaHelper(writer);
            writer.Write('{');
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
            writer.Write('}');
        }
    }
}