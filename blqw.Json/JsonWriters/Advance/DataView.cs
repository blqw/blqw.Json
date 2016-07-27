using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class DataViewWrite : IJsonWriter
    {
        public Type Type { get; } = typeof(DataView);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var view = (DataView)obj;
            var writer = args.Writer;
            var comma1 = new CommaHelper(writer);
            var columns = (view.Table ?? view.ToTable()).Columns;
            var length = columns.Count;
            writer.Write('[');
            for (int j = 0, count = view.Count; j < count; j++)
            {
                comma1.AppendCommaIgnoreFirst();
                var row = view[j];
                var comma = new CommaHelper(writer);
                for (int i = 0; i < length; i++)
                {
                    var column = columns[i];
                    var value = row[i];
                    if (args.IgnoreNullMember)
                    {
                        if (value == null || value is DBNull)
                        {
                            continue;
                        }
                    }

                    comma.AppendCommaIgnoreFirst();
                    JsonWriterContainer.StringWriter.Write(column.ColumnName, args);
                    writer.Write(':');
                    JsonWriterContainer.Write(value, args);
                }
            }

            writer.Write(']');
        }
    }
}
