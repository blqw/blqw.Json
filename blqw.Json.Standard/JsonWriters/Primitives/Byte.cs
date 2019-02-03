using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class ByteWriter : IJsonWriter
    {
        public Type Type => typeof(byte);

        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (byte) obj;
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