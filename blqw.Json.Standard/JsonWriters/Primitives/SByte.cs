using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class SByteWriter : IJsonWriter
    {
        public Type Type => typeof(sbyte);

        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (sbyte) obj;
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