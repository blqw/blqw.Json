using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    class DataSetWrite : IJsonWriter
    {
        public Type Type => typeof(DataSet);
        
        private IJsonWriterWrapper _wrapper;
        public IJsonWriterWrapper Wrapper
        {
            get
            {
                return _wrapper ?? (_wrapper = GetWrap(Type));
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var ds = (DataSet)obj;
            var writer = args.Writer;
            var comma = new CommaHelper(writer);
            writer.Write('{');
            for (int i = 0, length = ds.Tables.Count; i < length; i++)
            {
                comma.AppendCommaIgnoreFirst();
                var table = ds.Tables[i];
                JsonWriterContainer.StringWriter.Write(table.TableName, args);
                writer.Write(':');
                Wrapper.Writer.Write(table, args);

            }
            writer.Write('}');
        }
    }
}
