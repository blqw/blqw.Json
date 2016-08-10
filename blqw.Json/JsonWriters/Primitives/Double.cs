using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class DoubleWriter : IJsonWriter
    {
        public Type Type => typeof(double);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (double) obj;
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