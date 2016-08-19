using System;
using System.Globalization;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class ConvertibleWriter : IJsonWriter
    {
        private static JsonWriterWrapper _ObjectWrapper;
        public Type Type => typeof(IConvertible);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (IConvertible)obj;
            var typecode = value.GetTypeCode();
            switch (typecode)
            {
                case TypeCode.Decimal:
                    JsonWriterContainer.DecimalWriter.Write(value.ToDecimal(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Double:
                    JsonWriterContainer.DoubleWriter.Write(value.ToDouble(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Single:
                    JsonWriterContainer.SingleWriter.Write(value.ToSingle(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int16:
                    JsonWriterContainer.Int16Writer.Write(value.ToInt16(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int32:
                    JsonWriterContainer.Int32Writer.Write(value.ToInt32(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int64:
                    JsonWriterContainer.Int64Writer.Write(value.ToInt64(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.SByte:
                    JsonWriterContainer.SByteWriter.Write(value.ToSByte(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Byte:
                    JsonWriterContainer.ByteWriter.Write(value.ToByte(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt16:
                    JsonWriterContainer.UInt16Writer.Write(value.ToUInt16(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt32:
                    JsonWriterContainer.UInt32Writer.Write(value.ToUInt32(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt64:
                    JsonWriterContainer.UInt64Writer.Write(value.ToUInt64(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Object:
                    obj = value.ToType(typeof(object), CultureInfo.InvariantCulture);
                    (_ObjectWrapper ?? (_ObjectWrapper = JsonWriterContainer.GetWrap(typeof(object)))).Writer.Write(obj, args);
                    break;
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    args.Writer.Write("null");
                    break;
                case TypeCode.Boolean:
                    JsonWriterContainer.BooleanWriter.Write(value.ToBoolean(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Char:
                    JsonWriterContainer.CharWriter.Write(value.ToChar(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.DateTime:
                    JsonWriterContainer.DateTimeWriter.Write(value.ToDateTime(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.String:
                    JsonWriterContainer.StringWriter.Write(value.ToString(CultureInfo.InvariantCulture), args);
                    break;
                default:
                    throw new TypeLoadException($"TypeCode:{typecode} 未知");
            }
        }
    }
}