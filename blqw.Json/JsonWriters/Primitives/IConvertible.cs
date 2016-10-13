using System;
using System.Globalization;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class ConvertibleWriter : IJsonWriter
    {
        public Type Type => typeof(IConvertible);

        /// <exception cref="TypeLoadException"> <seealso cref="IConvertible.GetTypeCode" /> 返回值错误 </exception>
        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (IConvertible) obj;
            var typecode = value.GetTypeCode();
            switch (typecode)
            {
                case TypeCode.Decimal:
                    args.WriterContainer.GetWriter<decimal>().Write(value.ToDecimal(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Double:
                    args.WriterContainer.GetWriter<double>().Write(value.ToDouble(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Single:
                    args.WriterContainer.GetWriter<float>().Write(value.ToSingle(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int16:
                    args.WriterContainer.GetWriter<short>().Write(value.ToInt16(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int32:
                    args.WriterContainer.GetWriter<int>().Write(value.ToInt32(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int64:
                    args.WriterContainer.GetWriter<long>().Write(value.ToInt64(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.SByte:
                    args.WriterContainer.GetWriter<sbyte>().Write(value.ToSByte(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Byte:
                    args.WriterContainer.GetWriter<sbyte>().Write(value.ToByte(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt16:
                    args.WriterContainer.GetWriter<ushort>().Write(value.ToUInt16(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt32:
                    args.WriterContainer.GetWriter<uint>().Write(value.ToUInt32(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt64:
                    args.WriterContainer.GetWriter<uint>().Write(value.ToUInt64(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Object:
                    obj = value.ToType(typeof(object), CultureInfo.InvariantCulture);
                    if (obj == null || obj is DBNull)
                    {
                        args.WriterContainer.GetNullWriter().Write(null, args);
                    }
                    else
                    {
                        args.WriterContainer.GetWriter<object>().Write(obj, args);
                    }
                    break;
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    args.WriterContainer.GetNullWriter().Write(null, args);
                    break;
                case TypeCode.Boolean:
                    args.WriterContainer.GetWriter<bool>().Write(value.ToBoolean(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Char:
                    args.WriterContainer.GetWriter<char>().Write(value.ToChar(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.DateTime:
                    args.WriterContainer.GetWriter<DateTime>()
                        .Write(value.ToDateTime(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.String:
                    args.WriterContainer.GetWriter<string>().Write(value.ToString(CultureInfo.InvariantCulture), args);
                    break;
                default:
                    throw new TypeLoadException($"TypeCode:{typecode} 未知");
            }
        }
    }
}