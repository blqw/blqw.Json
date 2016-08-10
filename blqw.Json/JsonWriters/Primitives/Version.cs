using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class VersionWriter : IJsonWriter
    {
        public Type Type => typeof(Version);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (Version) obj;
            JsonWriterContainer.StringWriter.Write(value.ToString(), args);
        }
    }
}