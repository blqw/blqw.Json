using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace blqw
{
    public class Converter
    {
        public static readonly Converter Instance = new Converter();

        protected Converter() { }

        /// <summary> 判断当前对象是否是一个数字类型
        /// </summary>
        public static bool IsNumber(object obj)
        {
            if (obj != null)
            {
                var tc = (int)Convert.GetTypeCode(obj);
                return tc >= 7 && tc <= 15;
            }
            return false;
        }

        public object Change(object obj, Type type, out bool succeed)
        {
            if (type.IsInstanceOfType(obj))
            {
                succeed = true;
                return obj;
            }
            switch (type.Name)
            {
                case "Boolean":
                    {
                        bool v;
                        if (succeed = TryToBoolean(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Byte":
                    {
                        Byte v;
                        if (succeed = TryToByte(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Char":
                    {
                        Char v;
                        if (succeed = TryToChar(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "DateTime":
                    {
                        DateTime v;
                        if (succeed = TryToDateTime(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Decimal":
                    {
                        Decimal v;
                        if (succeed = TryToDecimal(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Double":
                    {
                        Double v;
                        if (succeed = TryToDouble(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Int16":
                    {
                        Int16 v;
                        if (succeed = TryToInt16(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Int32":
                    {
                        Int32 v;
                        if (succeed = TryToInt32(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Int64":
                    {
                        Int64 v;
                        if (succeed = TryToInt64(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "SByte":
                    {
                        SByte v;
                        if (succeed = TryToSByte(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Single":
                    {
                        Single v;
                        if (succeed = TryToSingle(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "String":
                    {
                        String v;
                        if (succeed = TryToString(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "UInt16":
                    {
                        UInt16 v;
                        if (succeed = TryToUInt16(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "UInt32":
                    {
                        UInt32 v;
                        if (succeed = TryToUInt32(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "UInt64":
                    {
                        UInt64 v;
                        if (succeed = TryToUInt64(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                case "Guid":
                    {
                        Guid v;
                        if (succeed = TryToGuid(obj, out v))
                        {
                            return v;
                        }
                        break;
                    }
                default:
                    try
                    {
                        succeed = true;
                        return Convert.ChangeType(obj, type);
                    }
                    catch
                    {
                        succeed = false;
                    }
                    break;
            }
            return null;
        }

        public object Change(object obj, Type type)
        {
            bool b;
            obj = Change(obj, type, out b);
            return obj;
        }

        #region virtual TryTo
        public virtual bool TryToBoolean(object obj, out Boolean value)
        {
            if (obj == null)
            {
                value = default(bool);
                return false;
            }
            if (obj is bool)
            {
                value = (bool)obj;
                return true;
            }
            if (obj is bool?)
            {
                value = ((bool?)obj).Value;
                return true;
            }
            if (IsNumber(obj))
            {
                value = obj.GetHashCode() != 0;
                return true;
            }
            var v = obj.ToString();
            if (v.Length == 4)
            {
                if (v.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    value = true;
                    return true;
                }
            }
            else if (v.Length == 5)
            {
                if (v.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    value = false;
                    return true;
                }
            }


            value = default(bool);
            return false;

        }

        public virtual bool TryToByte(object obj, out Byte value)
        {
            if (obj == null)
            {
                value = default(Byte);
                return false;
            }
            if (obj is Byte)
            {
                value = (Byte)obj;
                return true;
            }
            if (obj is Byte?)
            {
                value = ((Byte?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Byte.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToByte(obj);
                return true;
            }
            catch
            {
                value = default(Byte);
                return false;
            }
        }

        public virtual bool TryToChar(object obj, out Char value)
        {
            if (obj == null)
            {
                value = default(Char);
                return false;
            }
            if (obj is Char)
            {
                value = (Char)obj;
                return true;
            }
            if (obj is Char?)
            {
                value = ((Char?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Char.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToChar(obj);
                return true;
            }
            catch
            {
                value = default(Char);
                return false;
            }
        }

        public virtual bool TryToDateTime(object obj, out DateTime value)
        {
            if (obj == null)
            {
                value = default(DateTime);
                return false;
            }
            if (obj is DateTime)
            {
                value = (DateTime)obj;
                return true;
            }
            if (obj is DateTime?)
            {
                value = ((DateTime?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return DateTime.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToDateTime(obj);
                return true;
            }
            catch
            {
                value = default(DateTime);
                return false;
            }
        }

        public virtual bool TryToDecimal(object obj, out Decimal value)
        {
            if (obj == null)
            {
                value = default(Decimal);
                return false;
            }
            if (obj is Decimal)
            {
                value = (Decimal)obj;
                return true;
            }
            if (obj is Decimal?)
            {
                value = ((Decimal?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Decimal.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToDecimal(obj);
                return true;
            }
            catch
            {
                value = default(Decimal);
                return false;
            }
        }

        public virtual bool TryToDouble(object obj, out Double value)
        {
            if (obj == null)
            {
                value = default(Double);
                return false;
            }
            if (obj is Double)
            {
                value = (Double)obj;
                return true;
            }
            if (obj is Double?)
            {
                value = ((Double?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Double.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToDouble(obj);
                return true;
            }
            catch
            {
                value = default(Double);
                return false;
            }
        }

        public virtual bool TryToInt16(object obj, out Int16 value)
        {
            if (obj == null)
            {
                value = default(Int16);
                return false;
            }
            if (obj is Int16)
            {
                value = (Int16)obj;
                return true;
            }
            if (obj is Int16?)
            {
                value = ((Int16?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Int16.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToInt16(obj);
                return true;
            }
            catch
            {
                value = default(Int16);
                return false;
            }
        }

        public virtual bool TryToInt32(object obj, out Int32 value)
        {
            if (obj == null)
            {
                value = default(Int32);
                return false;
            }
            if (obj is Int32)
            {
                value = (Int32)obj;
                return true;
            }
            if (obj is Int32?)
            {
                value = ((Int32?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Int32.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToInt32(obj);
                return true;
            }
            catch
            {
                value = default(Int32);
                return false;
            }
        }

        public virtual bool TryToInt64(object obj, out Int64 value)
        {
            if (obj == null)
            {
                value = default(Int64);
                return false;
            }
            if (obj is Int64)
            {
                value = (Int64)obj;
                return true;
            }
            if (obj is Int64?)
            {
                value = ((Int64?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Int64.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToInt64(obj);
                return true;
            }
            catch
            {
                value = default(Int64);
                return false;
            }
        }

        public virtual bool TryToSByte(object obj, out SByte value)
        {
            if (obj == null)
            {
                value = default(SByte);
                return false;
            }
            if (obj is SByte)
            {
                value = (SByte)obj;
                return true;
            }
            if (obj is SByte?)
            {
                value = ((SByte?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return SByte.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToSByte(obj);
                return true;
            }
            catch
            {
                value = default(SByte);
                return false;
            }
        }

        public virtual bool TryToSingle(object obj, out Single value)
        {
            if (obj == null)
            {
                value = default(Single);
                return false;
            }
            if (obj is bool)
            {
                value = (Single)obj;
                return true;
            }
            if (obj is bool?)
            {
                value = ((Single?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return Single.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToSingle(obj);
                return true;
            }
            catch
            {
                value = default(Single);
                return false;
            }
        }

        public virtual bool TryToString(object obj, out String value)
        {
            if (obj is string)
            {
                value = (string)obj;
                return true;
            }
            try
            {
                value = Convert.ToString(obj);
                return true;
            }
            catch
            {
                value = default(String);
                return false;
            }
        }

        public virtual bool TryToUInt16(object obj, out UInt16 value)
        {
            if (obj == null)
            {
                value = default(UInt16);
                return false;
            }
            if (obj is UInt16)
            {
                value = (UInt16)obj;
                return true;
            }
            if (obj is UInt16?)
            {
                value = ((UInt16?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return UInt16.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToUInt16(obj);
                return true;
            }
            catch
            {
                value = default(UInt16);
                return false;
            }
        }

        public virtual bool TryToUInt32(object obj, out UInt32 value)
        {
            if (obj == null)
            {
                value = default(UInt32);
                return false;
            }
            if (obj is UInt32)
            {
                value = (UInt32)obj;
                return true;
            }
            if (obj is UInt32?)
            {
                value = ((UInt32?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return UInt32.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToUInt32(obj);
                return true;
            }
            catch
            {
                value = default(UInt32);
                return false;
            }
        }

        public virtual bool TryToUInt64(object obj, out UInt64 value)
        {
            if (obj == null)
            {
                value = default(UInt64);
                return false;
            }
            if (obj is bool)
            {
                value = (UInt64)obj;
                return true;
            }
            if (obj is bool?)
            {
                value = ((UInt64?)obj).Value;
                return true;
            }
            if (obj is string)
            {
                return UInt64.TryParse((string)obj, out value);
            }
            try
            {
                value = Convert.ToUInt64(obj);
                return true;
            }
            catch
            {
                value = default(UInt64);
                return false;
            }
        }

        public virtual bool TryToGuid(object obj, out Guid value)
        {
            if (obj == null)
            {
                value = default(Guid);
                return false;
            }
            if (obj is Guid)
            {
                value = (Guid)obj;
                return true;
            }
            if (obj is Guid?)
            {
                value = ((Guid?)obj).Value;
                return true;
            }
            if (obj is byte[])
            {
                value = new Guid((byte[])obj);
            }

            try
            {
                if (obj is string)
                {
                    value = new Guid((string)obj);
                    return true;
                }
                else
                {
                    value = default(Guid);
                    return false;
                }
            }
            catch
            {
                value = default(Guid);
                return false;
            }
        }
        #endregion

        #region To

        public bool ToBoolean(object obj)
        {
            bool value;
            if (TryToBoolean(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public byte ToByte(object obj)
        {
            Byte value;
            if (TryToByte(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public char ToChar(object obj)
        {
            Char value;
            if (TryToChar(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public DateTime ToDateTime(object obj)
        {
            DateTime value;
            if (TryToDateTime(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public decimal ToDecimal(object obj)
        {
            decimal value;
            if (TryToDecimal(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public double ToDouble(object obj)
        {
            Double value;
            if (TryToDouble(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public short ToInt16(object obj)
        {
            Int16 value;
            if (TryToInt16(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public int ToInt32(object obj)
        {
            Int32 value;
            if (TryToInt32(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public long ToInt64(object obj)
        {
            Int64 value;
            if (TryToInt64(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public sbyte ToSByte(object obj)
        {
            SByte value;
            if (TryToSByte(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public float ToSingle(object obj)
        {
            Single value;
            if (TryToSingle(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public string ToString(object obj)
        {
            String value;
            if (TryToString(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public UInt16 ToUInt16(object obj)
        {
            UInt16 value;
            if (TryToUInt16(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public uint ToUInt32(object obj)
        {
            UInt32 value;
            if (TryToUInt32(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public ulong ToUInt64(object obj)
        {
            UInt64 value;
            if (TryToUInt64(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }

        public Guid ToGuid(object obj)
        {
            Guid value;
            if (TryToGuid(obj, out value))
            {
                return value;
            }
            throw new InvalidCastException();
        }
        #endregion

        #region Change

        public Boolean Change(object obj, Boolean defVal)
        {
            bool value;
            if (TryToBoolean(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Byte Change(object obj, Byte defVal)
        {
            Byte value;
            if (TryToByte(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Char Change(object obj, Char defVal)
        {
            Char value;
            if (TryToChar(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public DateTime Change(object obj, DateTime defVal)
        {
            DateTime value;
            if (TryToDateTime(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Decimal Change(object obj, Decimal defVal)
        {
            decimal value;
            if (TryToDecimal(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Double Change(object obj, Double defVal)
        {
            Double value;
            if (TryToDouble(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Int16 Change(object obj, Int16 defVal)
        {
            Int16 value;
            if (TryToInt16(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Int32 Change(object obj, Int32 defVal)
        {
            Int32 value;
            if (TryToInt32(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Int64 Change(object obj, Int64 defVal)
        {
            Int64 value;
            if (TryToInt64(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public SByte Change(object obj, SByte defVal)
        {
            SByte value;
            if (TryToSByte(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Single Change(object obj, Single defVal)
        {
            Single value;
            if (TryToSingle(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public String Change(object obj, String defVal)
        {
            String value;
            if (TryToString(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public UInt16 Change(object obj, UInt16 defVal)
        {
            UInt16 value;
            if (TryToUInt16(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public UInt32 Change(object obj, UInt32 defVal)
        {
            UInt32 value;
            if (TryToUInt32(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public UInt64 Change(object obj, UInt64 defVal)
        {
            UInt64 value;
            if (TryToUInt64(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        public Guid Change(object obj, Guid defVal)
        {
            Guid value;
            if (TryToGuid(obj, out value))
            {
                return value;
            }
            return defVal;
        }

        #endregion
    }
}
