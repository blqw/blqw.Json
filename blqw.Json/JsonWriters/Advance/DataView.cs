using System;
using System.Data;

namespace blqw.Serializable.JsonWriters
{
    internal class DataViewWrite : IJsonWriter
    {
        public Type Type => typeof(DataView);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var view = (DataView) obj;
            var writer = args.Writer;
            var comma1 = new CommaHelper(writer);
            var columns = (view.Table ?? view.ToTable()).Columns;
            var length = columns.Count;
            args.BeginArray();
            for (int j = 0, count = view.Count; j < count; j++)
            {
                comma1.AppendCommaIgnoreFirst();
                var row = view[j];
                var comma = new CommaHelper(writer);
                for (var i = 0; i < length; i++)
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
                    args.WriterContainer.GetWriter<string>().Write(column.ColumnName, args);
                    writer.Write(':');
                    if (value == null || value is DBNull)
                    {
                        args.WriterContainer.GetNullWriter().Write(null, args);
                    }
                    else
                    {
                        args.WriterContainer.GetWriter(value.GetType()).Write(value,args);
                    }
                }
            }
            args.EndArray();
        }
    }
}