using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

//可空值类型转换,枚举转换未完成 2014.08.08

namespace blqw.Obsolete
{
    /// <summary> 字符串转换类
    /// </summary>
    public static class StringConverter
    {
        #region 初始化ChangedType<T>

        delegate T ConvertTo<T>(string s, T defaultValue, bool throwOnError);

        private readonly static object[] Methods = InitConvertMethods();

        private static object[] InitConvertMethods()
        {
            var arr = new object[19];
            arr[(int)TypeCode.Boolean] = (ConvertTo<Boolean>)ToBoolean;
            arr[(int)TypeCode.Byte] = (ConvertTo<Byte>)ToByte;
            arr[(int)TypeCode.Char] = (ConvertTo<Char>)ToChar;
            arr[(int)TypeCode.DateTime] = (ConvertTo<DateTime>)ToDateTime;
            arr[(int)TypeCode.Decimal] = (ConvertTo<Decimal>)ToDecimal;
            arr[(int)TypeCode.Double] = (ConvertTo<Double>)ToDouble;
            arr[(int)TypeCode.Int16] = (ConvertTo<Int16>)ToInt16;
            arr[(int)TypeCode.Int32] = (ConvertTo<Int32>)ToInt32;
            arr[(int)TypeCode.Int64] = (ConvertTo<Int64>)ToInt64;
            arr[(int)TypeCode.SByte] = (ConvertTo<SByte>)ToSByte;
            arr[(int)TypeCode.Single] = (ConvertTo<Single>)ToSingle;
            arr[(int)TypeCode.UInt16] = (ConvertTo<UInt16>)ToUInt16;
            arr[(int)TypeCode.UInt32] = (ConvertTo<UInt32>)ToUInt32;
            arr[(int)TypeCode.UInt64] = (ConvertTo<UInt64>)ToUInt64;
            arr[(int)TypeCode.Object] = (ConvertTo<Guid>)((v, d, e) => {
                return ToGuid(v, d, e);
            });
            return arr;
        }

        #endregion
        public static bool ToBoolean(string s, bool defaultValue = default(bool), bool throwOnError = false)
        {
            if (s == null)
            {
                return ReturnOrThrow(s, defaultValue, throwOnError);
            }
            if (char.IsWhiteSpace(s, 0))
            {
                s = s.Trim();
            }
            if (s.Length == 1)
            {
                switch (s[0])
                {
                    case '1':
                    case 'T':
                    case 't':
                    case '对':
                    case '對':
                    case '真':
                    case '是':
                        return true;
                    case '0':
                    case 'F':
                    case 'f':
                    case '错':
                    case '錯':
                    case '假':
                    case '否':
                        return false;
                }
            }
            bool i;
            if (bool.TryParse(s, out i))
            {
                return i;
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static byte ToByte(string s, byte defaultValue = default(byte), bool throwOnError = false)
        {
            if (s != null)
            {
                byte i;
                if (byte.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, byte.MaxValue))
                {
                    return (byte)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static char ToChar(string s, char defaultValue = default(char), bool throwOnError = false)
        {
            if (s == null || s.Length != 1)
            {
                return ReturnOrThrow(s, defaultValue, throwOnError);
            }
            return s[0];
        }
#if !NF2
        public static DateTime ToDateTime(string s, string format, DateTime defaultValue = default(DateTime), bool throwOnError = false)
        {
            if (s != null)
            {
                DateTime i;
                if (DateTime.TryParseExact(s, format, null, DateTimeStyles.None, out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
#endif
        public static DateTime ToDateTime(string s, DateTime defaultValue = default(DateTime), bool throwOnError = false)
        {
            if (s != null)
            {
                DateTime i;
                if (DateTime.TryParse(s, out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
#if !NF2
        public static TimeSpan ToTimeSpan(string s, string format, TimeSpan defaultValue = default(TimeSpan), bool throwOnError = false)
        {
            if (s != null)
            {
                TimeSpan i;
                if (TimeSpan.TryParseExact(s, format, null, TimeSpanStyles.None, out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        } 
#endif
        public static TimeSpan ToTimeSpan(string s, TimeSpan defaultValue = default(TimeSpan), bool throwOnError = false)
        {
            if (s != null)
            {
                TimeSpan i;
                if (TimeSpan.TryParse(s, out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }

        public static decimal ToDecimal(string s, decimal defaultValue = default(decimal), bool throwOnError = false)
        {
            if (s != null)
            {
                decimal i;
                if (decimal.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, decimal.MaxValue))
                {
                    return hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static double ToDouble(string s, double defaultValue = default(double), bool throwOnError = false)
        {
            if (s != null)
            {
                double i;
                if (double.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, int.MaxValue))
                {
                    return (double)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static Guid ToGuid(string s, Guid defaultValue = default(Guid), bool throwOnError = false)
        {
            if (s != null)
            {
#if NF2
                try
                {
                    if (s.Length > 30)
                    {
                        return new Guid(s);
                    }
                    return new Guid(Convert.FromBase64String(s));
                }
                catch
                {
                    if (throwOnError)
                        throw;
                    return defaultValue;
                }
#else
                Guid g;
                if (Guid.TryParse(s, out g))
                {
                    return g;
                }
                try
                {
                    return new Guid(Convert.FromBase64String(s));
                }
                catch
                {
                    if (throwOnError)
                        throw;
                    return defaultValue;
                }
#endif
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static short ToInt16(string s, short defaultValue = default(short), bool throwOnError = false)
        {
            if (s != null)
            {
                short i;
                if (short.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, short.MaxValue))
                {
                    return (short)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static int ToInt32(string s, int defaultValue = default(int), bool throwOnError = false)
        {
            if (s != null)
            {
                int i;
                if (int.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, int.MaxValue))
                {
                    return (int)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static long ToInt64(string s, long defaultValue = default(long), bool throwOnError = false)
        {
            if (s != null)
            {
                long i;
                if (long.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, long.MaxValue))
                {
                    return (long)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static sbyte ToSByte(string s, sbyte defaultValue = default(sbyte), bool throwOnError = false)
        {
            if (s != null)
            {
                sbyte i;
                if (sbyte.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, sbyte.MaxValue))
                {
                    return (sbyte)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static float ToSingle(string s, float defaultValue = default(float), bool throwOnError = false)
        {
            if (s != null)
            {
                float i;
                if (float.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, int.MaxValue))
                {
                    return (float)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static ushort ToUInt16(string s, ushort defaultValue = default(ushort), bool throwOnError = false)
        {
            if (s != null)
            {
                ushort i;
                if (ushort.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, ushort.MaxValue))
                {
                    return (ushort)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static uint ToUInt32(string s, uint defaultValue = default(uint), bool throwOnError = false)
        {
            if (s != null)
            {
                uint i;
                if (uint.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, uint.MaxValue))
                {
                    return (uint)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static ulong ToUInt64(string s, ulong defaultValue = default(ulong), bool throwOnError = false)
        {
            if (s != null)
            {
                ulong i;
                if (ulong.TryParse(s, out i))
                {
                    return i;
                }
                decimal hex;
                if (ToHexNumber(s, out hex, ulong.MaxValue))
                {
                    return (ulong)hex;
                }
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }
        public static decimal ToHexNumber(string s, decimal defaultValue = default(decimal), bool throwOnError = false)
        {
            decimal hex;
            if (ToHexNumber(s, out hex, decimal.MaxValue))
            {
                return hex;
            }
            return ReturnOrThrow(s, defaultValue, throwOnError);
        }

        public static object ChangedType(string s, Type type, bool throwOnError = false)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return ToBoolean(s, throwOnError: throwOnError);
                case TypeCode.Byte:
                    return ToByte(s, throwOnError: throwOnError);
                case TypeCode.Char:
                    return ToChar(s, throwOnError: throwOnError);
                case TypeCode.DateTime:
                    return ToDateTime(s, throwOnError: throwOnError);
                case TypeCode.Decimal:
                    return ToDecimal(s, throwOnError: throwOnError);
                case TypeCode.Double:
                    return ToDouble(s, throwOnError: throwOnError);
                case TypeCode.Int16:
                    return ToInt16(s, throwOnError: throwOnError);
                case TypeCode.Int32:
                    return ToInt32(s, throwOnError: throwOnError);
                case TypeCode.Int64:
                    return ToInt64(s, throwOnError: throwOnError);
                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        return ToGuid(s, throwOnError: throwOnError);
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        return ToTimeSpan(s, throwOnError: throwOnError);
                    }
                    else if (TypesHelper.IsNullable(type))
                    {
                        if (s == null)
                        {
                            return null;
                        }
                        return ChangedType(s, type.GetGenericArguments()[0], throwOnError);
                    }
                    else if (type == typeof(object))
                    {
                        return s;
                    }
                    break;
                case TypeCode.SByte:
                    return ToSByte(s, throwOnError: throwOnError);
                case TypeCode.Single:
                    return ToSingle(s, throwOnError: throwOnError);
                case TypeCode.String:
                    return s;
                case TypeCode.UInt16:
                    return ToUInt16(s, throwOnError: throwOnError);
                case TypeCode.UInt32:
                    return ToUInt32(s, throwOnError: throwOnError);
                case TypeCode.UInt64:
                    return ToUInt64(s, throwOnError: throwOnError);
                case TypeCode.Empty:
                    return null;
                case TypeCode.DBNull:
                    return DBNull.Value;
                default:
                    break;
            }
            return ReturnOrThrow<object>(s, null, throwOnError);
        }

        public static T ChangedType<T>(string s, T defaultValue = default(T), bool throwOnError = false)
        {
            if (s is T)
            {
                return (T)(object)s;
            }
            var type = typeof(T);
            var code = (int)Type.GetTypeCode(type);

            if (code == (int)TypeCode.Object && type != typeof(Guid))
            {
                if (TypesHelper.IsNullable(type))
                {
                    if (s == null)
                    {
                        return default(T);
                    }
                    code = (int)Type.GetTypeCode(type.GetGenericArguments()[0]);
                }
                else
                {
                    return (T)ReturnOrThrow<T>(s, defaultValue, throwOnError);
                }
            }
            var met = Methods[code];
            if (met == null)
            {
                return (T)ReturnOrThrow<T>(s, defaultValue, throwOnError);
            }
            return ((ConvertTo<T>)(met))(s, defaultValue, throwOnError);
        }

        public static Converter<string, object> CreateConverter(Type type, bool throwOnError = false)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return s => ToBoolean(s, throwOnError: throwOnError);
                case TypeCode.Byte:
                    return s => ToByte(s, throwOnError: throwOnError);
                case TypeCode.Char:
                    return s => ToChar(s, throwOnError: throwOnError);
                case TypeCode.DateTime:
                    return s => ToDateTime(s, throwOnError: throwOnError);
                case TypeCode.Decimal:
                    return s => ToDecimal(s, throwOnError: throwOnError);
                case TypeCode.Double:
                    return s => ToDouble(s, throwOnError: throwOnError);
                case TypeCode.Int16:
                    return s => ToInt16(s, throwOnError: throwOnError);
                case TypeCode.Int32:
                    return s => ToInt32(s, throwOnError: throwOnError);
                case TypeCode.Int64:
                    return s => ToInt64(s, throwOnError: throwOnError);
                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        return s => ToGuid(s, throwOnError: throwOnError);
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        return s => ToTimeSpan(s, throwOnError: throwOnError);
                    }
                    else if (TypesHelper.IsNullable(type))
                    {
                        var conv = CreateConverter(type.GetGenericArguments()[0], throwOnError);
                        return s => s == null ? null : conv(s);
                    }
                    break;
                case TypeCode.SByte:
                    return s => ToSByte(s, throwOnError: throwOnError);
                case TypeCode.Single:
                    return s => ToSingle(s, throwOnError: throwOnError);
                case TypeCode.String:
                    return s => s;
                case TypeCode.UInt16:
                    return s => ToUInt16(s, throwOnError: throwOnError);
                case TypeCode.UInt32:
                    return s => ToUInt32(s, throwOnError: throwOnError);
                case TypeCode.UInt64:
                    return s => ToUInt64(s, throwOnError: throwOnError);
                case TypeCode.Empty:
                    return s => null;
                    break;
                case TypeCode.DBNull:
                    return s => DBNull.Value;
                default:
                    break;
            }
            var obj = ReturnOrThrow<object>("System.String", null, throwOnError);
            return s => obj;
        }

        public static Converter<string, T> CreateConverter<T>(T defaultValue = default(T), bool throwOnError = false)
        {
            var type = typeof(T);
            var code = (int)Type.GetTypeCode(type);

            if (code == (int)TypeCode.String)
            {
                return s => (T)(object)s;
            }
            if (code == (int)TypeCode.Object && type != typeof(Guid))
            {
                if (TypesHelper.IsNullable(type))
                {
                    var obj = ReturnOrThrow<object>("System.String", defaultValue, throwOnError);
                    return s => (T)obj;
                }
            }
            var met = Methods[code];
            if (met == null)
            {
                var obj = ReturnOrThrow<object>("System.String", defaultValue, throwOnError);
                return s => (T)obj;
            }
            return s => ((ConvertTo<T>)(met))(s, defaultValue, throwOnError);
        }

        private static T ReturnOrThrow<T>(string s, T defaultValue, bool throwOnError)
        {
            if (throwOnError)
            {
                throw new InvalidCastException(string.Concat("值' ", s ?? "<NULL>", "' 无法转为 ", typeof(T).FullName, " 类型"));
            }
            else
            {
                return defaultValue;
            }
        }

        private static bool ToHexNumber(string s, out decimal value, decimal max)
        {
            if (s == null)
            {
                value = 0;
                return false;
            }
            if (char.IsWhiteSpace(s, 0))
            {
                s = s.Trim();
            }
            if (s.Length > 2)
            {
                switch (s[0])
                {
                    case '0':
                        switch (s[1])
                        {
                            case 'x':
                            case 'X':
                                s = s.Remove(0, 2);
                                break;
                        }
                        break;
                    case '&':
                        switch (s[1])
                        {
                            case 'h':
                            case 'H':
                                s = s.Remove(0, 2);
                                break;
                            default:
                                value = 0;
                                return false;
                        }
                        break;
                }
            }
            decimal i;
            if (decimal.TryParse(s, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out i))
            {
                if (i <= max)
                {
                    value = i;
                    return true;
                }
            }
            value = 0;
            return false;
        }

    }
}
