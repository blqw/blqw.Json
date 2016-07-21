using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class ObjectWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(object);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            
        }
    }
}
