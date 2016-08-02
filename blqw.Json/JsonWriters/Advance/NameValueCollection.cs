using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    class NameValueCollectionWrite : IJsonWriter
    {
        public Type Type { get; } = typeof(NameValueCollection);

        private static bool IsNull(IList<string> str)
        {
            if (str == null)
            {
                return true;
            }
            if (str.Count == 0)
            {
                return true;
            }
            if (str.Count > 1)
            {
                return false;
            }
            return str[0] == null;
        }

        private JsonWriterWrapper _wrapper;
        public JsonWriterWrapper Wrapper => _wrapper ?? (_wrapper = GetWrap(typeof(string[])));


        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (NameValueCollection)obj;
            var comma = new CommaHelper(writer);
            for (int i = 0, length = value.Count; i < length; i++)
            {
                var name = value.GetKey(i);
                var array = value.GetValues(i);
                if (args.IgnoreNullMember)
                {
                    if (IsNull(array))
                    {
                        continue;
                    }
                }
                else if (IsNull(array))
                {
                    array = null;
                }
                comma.AppendCommaIgnoreFirst();
                JsonWriterContainer.StringWriter.Write(name, args);
                writer.Write(':');
                if (array?.Length == 1)
                {
                    JsonWriterContainer.StringWriter.Write(array[0], args);
                }
                else
                {
                    Wrapper.Writer.Write(array, args);
                }

            }

        }
    }
}
