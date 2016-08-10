using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class BooleanWriter : IJsonWriter
    {
        public Type Type => typeof(bool);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (bool) obj;
            if (args.BooleanToNumber)
            {
                JsonWriterContainer.Int32Writer.Write(value ? 1 : 0, args);
            }
            else if (args.QuotWrapBoolean)
            {
                args.Writer.Write('"');
                args.Writer.Write(value);
                args.Writer.Write('"');
            }
            else
            {
                args.Writer.Write(value);
            }
        }
    }
}