using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class Int32Writer : IJsonWriter
    {
        public Type Type => typeof(int);

        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (int) obj;
            var writer = args.Writer;
            if (args.QuotWrapNumber)
            {
                writer.Write('"');
                writer.Write(value);
                writer.Write('"');
            }
            else
            {
                writer.Write(value);
            }
        }
    }
}