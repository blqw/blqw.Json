using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class VersionWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(Version);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (Version)obj;
            var writer = args.Writer;
            JsonWriterContainer.StringWriter.Write(value.ToString(), args);
        }
    }
}