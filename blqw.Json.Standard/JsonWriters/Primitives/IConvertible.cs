using System;
using System.Globalization;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class ConvertibleWriter : IJsonWriter
    {
        public Type Type => typeof(IConvertible);

        /// <exception cref="TypeLoadException"> <seealso cref="IConvertible.GetTypeCode" /> 返回值错误 </exception>
        public void Write(object obj, JsonWriterSettings args)
        {
            var value = (IConvertible) obj;
            var typecode = value.GetTypeCode();
            switch (typecode)
            {
                case TypeCode.Decimal:
                    args.Selector.Get<decimal>().Write(value.ToDecimal(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Double:
                    args.Selector.Get<double>().Write(value.ToDouble(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Single:
                    args.Selector.Get<float>().Write(value.ToSingle(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int16:
                    args.Selector.Get<short>().Write(value.ToInt16(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int32:
                    args.Selector.Get<int>().Write(value.ToInt32(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Int64:
                    args.Selector.Get<long>().Write(value.ToInt64(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.SByte:
                    args.Selector.Get<sbyte>().Write(value.ToSByte(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Byte:
                    args.Selector.Get<sbyte>().Write(value.ToByte(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt16:
                    args.Selector.Get<ushort>().Write(value.ToUInt16(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt32:
                    args.Selector.Get<uint>().Write(value.ToUInt32(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.UInt64:
                    args.Selector.Get<uint>().Write(value.ToUInt64(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Object:
                    obj = value.ToType(typeof(object), CultureInfo.InvariantCulture);
                    if (obj == null || obj is DBNull)
                    {
                        args.WriteNull();
                    }
                    else
                    {
                        args.Selector.Get<object>().Write(obj, args);
                    }
                    break;
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    args.WriteNull();
                    break;
                case TypeCode.Boolean:
                    args.Selector.Get<bool>().Write(value.ToBoolean(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.Char:
                    args.Selector.Get<char>().Write(value.ToChar(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.DateTime:
                    args.Selector.Get<DateTime>()
                        .Write(value.ToDateTime(CultureInfo.InvariantCulture), args);
                    break;
                case TypeCode.String:
                    args.Write(value.ToString(CultureInfo.InvariantCulture));
                    break;
                default:
                    throw new TypeLoadException($"TypeCode:{typecode} 未知");
            }
        }
    }
}