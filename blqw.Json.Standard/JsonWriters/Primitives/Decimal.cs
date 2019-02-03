using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class DecimalWriter : IJsonWriter
    {
        public Type Type => typeof(decimal);

        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (decimal) obj;
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