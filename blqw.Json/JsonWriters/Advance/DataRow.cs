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
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var row = (DataRow) obj;
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
                args.WriterContainer.GetWriter<string>().Write(column.ColumnName, args);
                writer.Write(':');
                if (row.IsNull(column))
                {
                    args.WriterContainer.GetNullWriter().Write(null, args);
                }
                else
                {
                    var value = row[column];
                    args.WriterContainer.GetWriter(value.GetType()).Write(value,args);
                }
            }
            args.EndObject();
        }
    }
}