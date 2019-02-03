using System;
using System.Data;

namespace blqw.JsonServices.JsonWriters
{
    internal class DataViewWrite : IJsonWriter
    {
        public Type Type => typeof(DataView);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var view = (DataView)obj;
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
                    args.Write(column.ColumnName);
                    writer.Write(':');
                    if (value == null || value is DBNull)
                    {
                        args.WriteNull();
                    }
                    else
                    {
                        args.WriteObject(value);
                    }
                }
            }
            args.EndArray();
        }
    }
}