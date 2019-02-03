using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class UriWriter : IJsonWriter
    {
        public Type Type => typeof(Uri);

        public void Write(object obj, JsonWriterSettings args) => args.Write(obj.ToString());
    }
}