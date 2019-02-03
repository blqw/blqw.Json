using System;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class VersionWriter : IJsonWriter
    {
        public Type Type => typeof(Version);

        public void Write(object obj, JsonWriterSettings args) => args.Write(obj.ToString());
    }
}