using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace blqw.Serializable.JsonWriters.Advance
{
    internal class ObjectReferenceWriter : List<JsonWriterWrapper>, IMultiJsonWriters, IJsonWriter
    {
        public Type Type => typeof(IObjectReference);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (IObjectReference)obj;
            var obj1 = value.GetRealObject(new StreamingContext(StreamingContextStates.All, args));

            if (obj1 != null)
            {
                args.WriteCheckLoop(obj1, null);
            }
            else if (Count == 0)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
            }
            else
            {
                this[0].Writer.Write(obj, args);
            }
        }
    }
}
