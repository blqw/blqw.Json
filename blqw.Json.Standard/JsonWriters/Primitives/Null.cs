using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class NullWriter : IJsonWriter
    {
        public Type Type => typeof(void);

        public void Write(object obj, JsonWriterSettings args)
        {
            args.Writer.Write("null");
        }
    }

    internal sealed class DBNullWriter : IJsonWriter
    {
        public Type Type => typeof(DBNull);

        public void Write(object obj, JsonWriterSettings args)
        {
            args.Writer.Write("null");
        }
    }
}