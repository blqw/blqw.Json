using blqw.Converts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class IConvertibleWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {

                return typeof(IConvertible);
            }
        }
        
        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (IConvertible)obj;
            var typecode = value.GetTypeCode();
            switch (typecode)
            {
                case TypeCode.Decimal:
                    JsonWriterContainer.DecimalWriter.Write(obj, args);
                    //if (args.QuotWrapNumber)
                    //{
                    //    writer.Write('"');
                    //    writer.Write(value.ToDecimal(CultureInfo.InvariantCulture));
                    //    writer.Write('"');
                    //}
                    //else
                    //{
                    //    writer.Write(value.ToDecimal(CultureInfo.InvariantCulture));
                    //}
                    break;
                case TypeCode.Double:
                    JsonWriterContainer.DoubleWriter.Write(obj, args);
                    break;
                case TypeCode.Single:
                    JsonWriterContainer.SingleWriter.Write(obj, args);
                    break;
                case TypeCode.Int16:
                    JsonWriterContainer.Int16Writer.Write(obj, args);
                    break;
                case TypeCode.Int32:
                    JsonWriterContainer.Int32Writer.Write(obj, args);
                    break;
                case TypeCode.Int64:
                    JsonWriterContainer.Int64Writer.Write(obj, args);
                    break;
                case TypeCode.SByte:
                    JsonWriterContainer.SByteWriter.Write(obj, args);
                    break;
                case TypeCode.Byte:
                    JsonWriterContainer.ByteWriter.Write(obj, args);
                    break;
                case TypeCode.UInt16:
                    JsonWriterContainer.UInt16Writer.Write(obj, args);
                    break;
                case TypeCode.UInt32:
                    JsonWriterContainer.UInt32Writer.Write(obj, args);
                    break;
                case TypeCode.UInt64:
                    JsonWriterContainer.UInt64Writer.Write(obj, args);
                    break;
                case TypeCode.Empty:
                    args.Writer.Write("null");
                    break;
                case TypeCode.Object:
                    JsonWriterContainer.ObjectWriter.Write(obj, args);
                    break;
                case TypeCode.DBNull:
                    args.Writer.Write("null");
                    break;
                case TypeCode.Boolean:
                    JsonWriterContainer.BooleanWriter.Write(obj, args);
                    break;
                case TypeCode.Char:
                    JsonWriterContainer.CharWriter.Write(obj, args);
                    break;
                case TypeCode.DateTime:
                    JsonWriterContainer.DateTimeWriter.Write(obj, args);
                    break;
                case TypeCode.String:
                    JsonWriterContainer.StringWriter.Write(obj, args);
                    break;
                default:
                    throw new TypeLoadException($"TypeCode:{typecode} 未知");
            }
        }
    }
}
