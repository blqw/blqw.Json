using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class SingleWriter : IJsonWriter
    {
        public Type Type => typeof(float);

        public void Write(object obj, JsonWriterArgs args)
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