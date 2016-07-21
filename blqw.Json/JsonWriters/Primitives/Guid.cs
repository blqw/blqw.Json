using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class GuidWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(Guid);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (Guid)obj;
            JsonWriterContainer.StringWriter.Write(value.ToString(args.GuidHasHyphens ? "D" : "N"), args);
        }
    }
}