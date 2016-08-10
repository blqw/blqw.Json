using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class UriWriter : IJsonWriter
    {
        public Type Type => typeof(Uri);
        
        public void Write(object obj, JsonWriterArgs args)
        {
            JsonWriterContainer.StringWriter.Write(obj.ToString(), args);
        }
    }
}