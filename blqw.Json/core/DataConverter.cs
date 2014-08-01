using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Xml;

namespace blqw
{
    public static class DataConverter
    {
        #region 初始化ChangedType<T>

        delegate T ConvertTo<T>(object value, T defaultValue, bool throwOnError);

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
            arr[(int)TypeCode.String] = (ConvertTo<String>)ToString;
            arr[(int)TypeCode.UInt16] = (ConvertTo<UInt16>)ToUInt16;
            arr[(int)TypeCode.UInt32] = (ConvertTo<UInt32>)ToUInt32;
            arr[(int)TypeCode.UInt64] = (ConvertTo<UInt64>)ToUInt64;
            arr[(int)TypeCode.Object] = (ConvertTo<Guid>)((v, d, e) => {
                return ToGuid(v, d, e);
            });
            return arr;
        }

        #endregion

        public static Enum ToDbType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return DbType.Boolean;
                case TypeCode.Byte:
                    return DbType.Byte;
                case TypeCode.Char:
                    return DbType.Boolean;
                case TypeCode.DBNull:
                    return DbType.Object;
                case TypeCode.DateTime:
                    return DbType.DateTime;
                case TypeCode.Decimal:
                    return DbType.Decimal;
                case TypeCode.Double:
                    return DbType.Double;
                case TypeCode.Empty:
                    return DbType.Object;
                case TypeCode.Int16:
                    return DbType.Int16;
                case TypeCode.Int32:
                    return DbType.Int32;
                case TypeCode.Int64:
                    return DbType.Int64;
                case TypeCode.SByte:
                    return DbType.SByte;
                case TypeCode.Single:
                    return DbType.Single;
                case TypeCode.String:
                    return DbType.String;
                case TypeCode.UInt16:
                    return DbType.UInt16;
                case TypeCode.UInt32:
                    return DbType.UInt32;
                case TypeCode.UInt64:
                    return DbType.UInt64;
                case TypeCode.Object:
                default:
                    break;
            }
            if (type == typeof(Guid))
            {
                return DbType.Guid;
            }
            else if (type == typeof(byte[]))
            {
                return DbType.Binary;
            }
            else if (type == typeof(XmlDocument))
            {
                return DbType.Xml;
            }
            throw new InvalidCastException("无法将" + type.ToString() + "转换为DbType");
        }

        public static Type ToType(Enum dbtype)
        {
            if (dbtype is DbType == false)
            {
                throw new InvalidCastException("dbtype必须是System.Data.DbType类型");
            }
            switch ((DbType)dbtype)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return typeof(String);
                case DbType.Binary:
                    return typeof(Byte[]);
                case DbType.Boolean:
                    return typeof(Boolean);
                case DbType.Byte:
                    return typeof(Byte);
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                    return typeof(DateTime);
                case DbType.Decimal:
                case DbType.VarNumeric:
                case DbType.Currency:
                    return typeof(Decimal);
                case DbType.Double:
                    return typeof(Double);
                case DbType.Guid:
                    return typeof(Guid);
                case DbType.Int16:
                    return typeof(Int16);
                case DbType.Int32:
                    return typeof(Int32);
                case DbType.Int64:
                    return typeof(Int64);
                case DbType.Object:
                    return typeof(Object);
                case DbType.SByte:
                    return typeof(SByte);
                case DbType.Single:
                    return typeof(Single);
                case DbType.UInt16:
                    return typeof(UInt16);
                case DbType.UInt32:
                    return typeof(UInt32);
                case DbType.UInt64:
                    return typeof(UInt64);
                case DbType.Xml:
                    return typeof(XmlDocument);
                default:
                    throw new InvalidCastException("无效的DbType值:" + dbtype.ToString());
            }
        }

        #region 实体转换

        public static T ToModel<T>(DbDataReader reader)
        {
            Assertor.AreNull(reader, "reader");
            T model = default(T);
            FillModel(reader, ref model);
            return model;
        }

        public static bool FillModel<T>(DbDataReader reader, ref T model)
        {
            Assertor.AreNull(reader, "reader");
            if (reader.Read() == false)
            {
                return false;
            }

            var type = typeof(T);
            if (ExtendMethod.IsPrimitive(type))
            {
                model = ChangedType<T>(reader[0], default(T), true);
            }
            else
            {
                var lit = Literacy.Cache(typeof(T), true);
                if (model == null)
                {
                    model = (T)lit.NewObject();
                }
                var props = GetProperties(reader, lit);
                FillModel<T>(reader, props, model);
            }
            return true;
        }

        public static List<T> ToList<T>(DbDataReader reader)
        {
            Assertor.AreNull(reader, "reader");
            var type = typeof(T);
            var lit = Literacy.Cache(type, true);
            var props = GetProperties(reader, lit);
            var list = new List<T>();
            if (ExtendMethod.IsPrimitive(type))
            {
                while (reader.Read())
                {
                    list.Add((T)ChangedType(reader[0], type));
                }
            }
            else
            {
                while (reader.Read())
                {
                    var model = (T)lit.NewObject();
                    FillModel<T>(reader, props, model);
                    list.Add(model);
                }
            }

            return list;
        }

        public static T ToModel<T>(DataRow row)
        {
            Assertor.AreNull(row, "reader");
            var model = default(T);
            FillModel(row, ref model);
            return model;
        }

        public static bool FillModel<T>(DataRow row, ref T model)
        {
            Assertor.AreNull(row, "reader");
            if (row.HasErrors)
            {
                return false;
            }
            var type = typeof(T);
            if (ExtendMethod.IsPrimitive(type))
            {
                model = ChangedType<T>(row[0], default(T), true);
            }
            else
            {
                var lit = Literacy.Cache(typeof(T), true);
                if (model == null)
                {
                    model = (T)lit.NewObject();
                }
                var props = GetProperties(row.Table, lit);
                FillModel<T>(row, props, model);
            }
            return true;
        }

        public static List<T> ToList<T>(DataTable table)
        {
            Assertor.AreNull(table, "reader");
            var type = typeof(T);
            var lit = Literacy.Cache(typeof(T), true);
            var props = GetProperties(table, lit);
            var list = new List<T>();
            if (ExtendMethod.IsPrimitive(type))
            {
                foreach (DataRow row in table.Rows)
                {
                    list.Add((T)ChangedType(row[0], type));
                }
            }
            else
            {
                foreach (DataRow row in table.Rows)
                {
                    var model = (T)lit.NewObject();
                    FillModel<T>(row, props, model);
                    list.Add(model);
                }
            }
            return list;
        }

        #region 私有方法
        private static ObjectProperty[] GetProperties(DataTable table, Literacy lit)
        {
            var cols = table.Columns;
            var length = cols.Count;
            var props = new ObjectProperty[length];
            for (int i = 0; i < length; i++)
            {
                var p = lit.Property[cols[i].ColumnName.Replace("_", "")];
                if (p == null)
                {
                    p = lit.Property[cols[i].ColumnName];
                }
                props[i] = p;
            }
            return props;
        }
        private static ObjectProperty[] GetProperties(DbDataReader reader, Literacy lit)
        {
            var length = reader.FieldCount;
            var props = new ObjectProperty[length];
            for (int i = 0; i < length; i++)
            {
                var p = lit.Property[reader.GetName(i).Replace("_", "")];
                if (p == null)
                {
                    p = lit.Property[reader.GetName(i)];
                }
                props[i] = p;
            }
            return props;
        }
        private static void FillModel<T>(DbDataReader reader, ObjectProperty[] props, T model)
        {
            for (int i = 0; i < props.Length; i++)
            {
                var p = props[i];
                if (p != null)
                {
                    p.TrySetValue(model, ChangedType(reader[i], p.OriginalType));
                }
            }
        }
        private static void FillModel<T>(DataRow row, ObjectProperty[] props, T model)
        {
            for (int i = 0; i < props.Length; i++)
            {
                var p = props[i];
                if (p != null)
                {
                    p.TrySetValue(model, ChangedType(row[i], p.OriginalType));
                }
            }
        }
        #endregion

        #endregion

        public static object ChangedType(object value, Type type, bool throwOnError = false)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return ToBoolean(value, throwOnError: throwOnError);
                case TypeCode.Byte:
                    return ToByte(value, throwOnError: throwOnError);
                case TypeCode.Char:
                    return ToChar(value, throwOnError: throwOnError);
                case TypeCode.DateTime:
                    return ToDateTime(value, throwOnError: throwOnError);
                case TypeCode.Decimal:
                    return ToDecimal(value, throwOnError: throwOnError);
                case TypeCode.Double:
                    return ToDouble(value, throwOnError: throwOnError);
                case TypeCode.Int16:
                    return ToInt16(value, throwOnError: throwOnError);
                case TypeCode.Int32:
                    return ToInt32(value, throwOnError: throwOnError);
                case TypeCode.Int64:
                    return ToInt64(value, throwOnError: throwOnError);
                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        return ToGuid(value, throwOnError: throwOnError);
                    }
                    else if (ExtendMethod.IsNullable(type))
                    {
                        if (value is DBNull || value == null)
                        {
                            return null;
                        }
                        return ChangedType(value, type.GetGenericArguments()[0], throwOnError);
                    }
                    break;
                case TypeCode.SByte:
                    return ToSByte(value, throwOnError: throwOnError);
                case TypeCode.Single:
                    return ToSingle(value, throwOnError: throwOnError);
                case TypeCode.String:
                    return ToString(value, throwOnError: throwOnError);
                case TypeCode.UInt16:
                    return ToUInt16(value, throwOnError: throwOnError);
                case TypeCode.UInt32:
                    return ToUInt32(value, throwOnError: throwOnError);
                case TypeCode.UInt64:
                    return ToUInt64(value, throwOnError: throwOnError);
                case TypeCode.Empty:
                    if (value == null)
                    {
                        return null;
                    }
                    break;
                case TypeCode.DBNull:
                    if (value == null)
                    {
                        return DBNull.Value;
                    }
                    break;
                default:
                    break;
            }
            return ReturnOrThrow<object>(value, null, throwOnError);
        }

        public static T ChangedType<T>(object value, T defaultValue = default(T), bool throwOnError = false)
        {
            var type = typeof(T);
            var code = (int)Type.GetTypeCode(type);
            Assertor.AreInRange(code, "T", 0, 18);

            if (code == (int)TypeCode.Object && typeof(T) != typeof(Guid))
            {
                if (ExtendMethod.IsNullable(type))
                {
                    if (value is DBNull || value == null)
                    {
                        return default(T);
                    }
                    code = (int)Type.GetTypeCode(type.GetGenericArguments()[0]);
                }
                else
                {
                    return (T)ReturnOrThrow<T>(value, defaultValue, throwOnError);
                }
            }
            var met = Methods[code];
            if (met == null)
            {
                return (T)ReturnOrThrow<T>(value, defaultValue, throwOnError);
            }
            return ((ConvertTo<T>)(met))(value, defaultValue, throwOnError);
        }

        #region 类型转换

        private static T ReturnOrThrow<T>(object value, T defaultValue, bool throwOnError)
        {
            if (throwOnError)
            {
                throw new InvalidCastException(string.Concat("值' ", value ?? "<NULL>", "' 无法转为 ", typeof(T).FullName, " 类型"));
            }
            else
            {
                return defaultValue;
            }
        }


        public static BitArray ToBitArray(object value, BitArray defaultValue = null, bool throwOnError = false)
        {
            Byte[] bytes = value as Byte[];
            if (bytes != null)
            {
                return new BitArray(bytes);
            }
            var arr = value as BitArray;
            if (arr != null)
            {
                return arr;
            }
            var arr2 = value as bool[];
            if (arr2 != null)
            {
                return new BitArray(arr2);
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static byte[] ToByteArray(object value, byte[] defaultValue = null, bool throwOnError = false)
        {
            Byte[] bytes = value as Byte[];
            if (bytes != null)
            {
                return bytes;
            }
            string s = value as string;
            if (s != null)
            {
                return Encoding.UTF8.GetBytes(s);
            }
            if (value is Guid)
            {
                return ((Guid)value).ToByteArray();
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static bool ToBoolean(object value, bool defaultValue = default(bool), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Char:
                        var c = (char)value;
                        if (c == 'F' || c == 'f')
                        {
                            return false;
                        }
                        else if (c == 'T' || c == 't')
                        {
                            return true;
                        }
                        break;
                    case TypeCode.String:
                        bool i;
                        if (bool.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    case TypeCode.Boolean:
                        return conv.ToBoolean(null);
                    default:
                        if (code >= TypeCode.SByte && code <= TypeCode.Decimal)
                        {
                            return conv.GetHashCode() != 0;
                        }
                        break;
                }
            }
            else if (value != null)
            {
                bool i;
                if (bool.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static byte ToByte(object value, byte defaultValue = default(byte), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (byte)1 : (byte)0;
                    case TypeCode.String:
                        byte i;
                        if (byte.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToByte(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                byte i;
                if (byte.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static char ToChar(object value, char defaultValue = default(char), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? 'T' : 'F';
                    case TypeCode.String:
                        char i;
                        if (char.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToChar(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                char i;
                if (char.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static DateTime ToDateTime(object value, DateTime defaultValue = default(DateTime), bool throwOnError = false)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            if (value != null)
            {
                DateTime i;
                if (DateTime.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static decimal ToDecimal(object value, decimal defaultValue = default(decimal), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? 1m : 0m;
                    case TypeCode.String:
                        Decimal i;
                        if (Decimal.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToDecimal(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                Decimal i;
                if (Decimal.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static double ToDouble(object value, double defaultValue = default(double), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? 1d : 0d;
                    case TypeCode.String:
                        double i;
                        if (double.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToDouble(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                double i;
                if (double.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static Guid ToGuid(object value, Guid defaultValue = default(Guid), bool throwOnError = false)
        {
            if (value is Guid)
            {
                return (Guid)value;
            }
            Byte[] bytes = value as Byte[];
            if (bytes != null)
            {
                if (bytes.Length == 16)
                {
                    try
                    {
                        return new Guid(bytes);
                    }
                    catch
                    {
                        if (throwOnError)
                            throw;
                        return defaultValue;
                    }
                }
                return ReturnOrThrow(value, defaultValue, throwOnError);
            }
            if (value != null)
            {
#if NF2
                try
                {
                    return new Guid(value.ToString());
                }
                catch
                {
                    if (throwOnError) 
                        throw;
                    return defaultValue;
                }
#else
                Guid g;
                if (Guid.TryParse(value.ToString(), out g))
                {
                    return g;
                }
#endif

            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static short ToInt16(object value, short defaultValue = default(short), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (short)1 : (short)0;
                    case TypeCode.String:
                        short i;
                        if (short.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToInt16(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                short i;
                if (short.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static int ToInt32(object value, int defaultValue = default(int), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (int)1 : (int)0;
                    case TypeCode.String:
                        int i;
                        if (int.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToInt32(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                int i;
                if (int.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static long ToInt64(object value, long defaultValue = default(long), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? 1L : 0L;
                    case TypeCode.String:
                        long i;
                        if (long.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToInt64(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                long i;
                if (long.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static sbyte ToSByte(object value, sbyte defaultValue = default(sbyte), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (sbyte)1 : (sbyte)0;
                    case TypeCode.String:
                        sbyte i;
                        if (sbyte.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToSByte(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                sbyte i;
                if (sbyte.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static float ToSingle(object value, float defaultValue = default(float), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (float)1 : (float)0;
                    case TypeCode.String:
                        float i;
                        if (float.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToSingle(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                float i;
                if (float.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static string ToString(object value, string defaultValue = null, bool throwOnError = false)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else if (value is DBNull)
            {
                return null;
            }
            else
            {
                return value.ToString();
            }
        }
        public static ushort ToUInt16(object value, ushort defaultValue = default(ushort), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (ushort)1 : (ushort)0;
                    case TypeCode.String:
                        ushort i;
                        if (ushort.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToUInt16(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                ushort i;
                if (ushort.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static uint ToUInt32(object value, uint defaultValue = default(uint), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (uint)1 : (uint)0;
                    case TypeCode.String:
                        uint i;
                        if (uint.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToUInt32(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                uint i;
                if (uint.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        public static ulong ToUInt64(object value, ulong defaultValue = default(ulong), bool throwOnError = false)
        {
            var conv = value as IConvertible;
            if (conv != null)
            {
                var code = conv.GetTypeCode();
                switch (code)
                {
                    case TypeCode.Boolean:
                        return ((bool)value) ? (ulong)1 : (ulong)0;
                    case TypeCode.String:
                        ulong i;
                        if (ulong.TryParse((string)value, out i))
                        {
                            return i;
                        }
                        break;
                    default:
                        if (code >= TypeCode.Char && code <= TypeCode.Decimal)
                        {
                            return conv.ToUInt64(null);
                        }
                        break;
                }
            }
            else if (value != null)
            {
                ulong i;
                if (ulong.TryParse(value.ToString(), out i))
                {
                    return i;
                }
            }
            return ReturnOrThrow(value, defaultValue, throwOnError);
        }
        #endregion
    }
}
