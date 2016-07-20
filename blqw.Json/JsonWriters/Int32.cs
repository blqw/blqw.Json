using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class Int32Writer : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(int);
            }
        }

        public void Write(TextWriter writer, object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
