using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class UInt64Writer : IJsonWriter
    {
        public Type Type => typeof(ulong);

        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (ulong) obj;
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