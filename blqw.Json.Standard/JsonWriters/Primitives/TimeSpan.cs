using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class TimeSpanWriter : IJsonWriter
    {
        public Type Type => typeof(TimeSpan);

        public void Write(object obj, JsonWriterSettings args) => args.Write(((TimeSpan)obj).ToString("g"));
    }
}