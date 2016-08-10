using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class TimeSpanWriter : IJsonWriter
    {
        public Type Type => typeof(TimeSpan);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (TimeSpan)obj;
            JsonWriterContainer.StringWriter.Write(value.ToString("g"), args);
        }
    }
}