using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class UriWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(Uri);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (Uri)obj;
            var writer = args.Writer;
        }
    }
}