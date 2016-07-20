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
            var writer = args.Writer;
            switch (value.GetTypeCode())
            {
                case TypeCode.Decimal:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToDecimal(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToDecimal(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Double:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToDouble(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToDouble(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Single:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToSingle(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToSingle(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Int16:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToInt16(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToInt16(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Int32:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToInt32(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToInt32(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Int64:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToInt64(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToInt64(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.SByte:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToSByte(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToSByte(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Byte:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToByte(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToByte(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.UInt16:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToUInt16(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToUInt16(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.UInt32:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToUInt32(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToUInt32(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.UInt64:
                    if (args.QuotWrapNumber)
                    {
                        writer.Write('"');
                        writer.Write(value.ToUInt64(CultureInfo.InvariantCulture));
                        writer.Write('"');
                    }
                    else
                    {
                        writer.Write(value.ToUInt64(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Empty:
                    writer.Write("null");
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.DBNull:
                    writer.Write("null");
                    break;
                case TypeCode.Boolean:
                    break;
                case TypeCode.Char:
                    break;
                case TypeCode.DateTime:
                    break;
                case TypeCode.String:
                    break;
                default:
                    break;
            }
        }
    }
}
