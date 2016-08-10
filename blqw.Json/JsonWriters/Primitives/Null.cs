using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class Null : IJsonWriter
    {
        public Type Type => typeof(void);

        public void Write(object obj, JsonWriterArgs args)
        {
            args.Writer.Write("null");
        }
    }
}