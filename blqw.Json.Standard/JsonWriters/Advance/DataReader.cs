using System;
using System.Data;

namespace blqw.JsonServices.JsonWriters
{
    internal class DataReaderWriter : IJsonWriter
    {
        public Type Type => typeof(IDataReader);

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <exception cref="InvalidOperationException">IDataReader已经关闭</exception>
        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var reader = (IDataReader) obj;
            if (reader.IsClosed)
            {
                throw new InvalidOperationException("IDataReader已经关闭");
            }
            args.BeginArray();

            if (reader.FieldCount == 1)
            {
                var comma = new CommaHelper(args.Writer);
                while (reader.Read())
                {
                    var value = reader.GetValue(0);
                    if (args.IgnoreNullMember)
                    {
                            continue;
                    }
                    if (value == null || value is DBNull)
                    {
                        if (args.IgnoreNullMember)
                        {
                            continue;
                        }
                        args.WriteNull();
                    }
                    else
                    {
                        args.WriteObject(value);
                    }
                    comma.AppendCommaIgnoreFirst();
                }
            }
            else
            {
                var writer = args.Selector.Get(Type);
                if (reader.Read())
                {
                    writer.Write(reader, args);
                    while (reader.Read())
                    {
                        args.Common();
                        writer.Write(reader, args);
                    }
                }
            }
            args.EndArray();
        }
    }
}