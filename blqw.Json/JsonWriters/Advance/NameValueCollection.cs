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

        private static bool IsNull(string[] str)
        {
            if (str == null)
            {
                return true;
            }
            if (str.Length == 0)
            {
                return true;
            }
            if (str.Length > 1)
            {
                return false;
            }
            if (str[0] == null)
            {
                return true;
            }
            return false;
        }

        private IJsonWriterWrapper _wrapper = GetWrap(typeof(string[]));

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (NameValueCollection)obj;
            var comma = new CommaHelper(writer);
            for (int i = 0,length= value.Count; i < length; i++)
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
                    _wrapper.Writer.Write(array, args);
                }

            }

        }
    }
}
