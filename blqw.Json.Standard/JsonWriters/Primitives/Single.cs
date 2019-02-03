using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class SingleWriter : IJsonWriter
    {
        public Type Type => typeof(float);

        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (float) obj;
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