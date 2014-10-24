using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public class JsonValue2
    {
        private IConvertible _value;
        public JsonValue2(IConvertible value)
        {
            _value = value;
        }

        public static explicit operator int(JsonValue2 o)
        {
            return o._value.ToInt32(null);
        }

        public static explicit operator long(JsonValue2 o)
        {
            return o._value.ToInt64(null);
        }

        public static explicit operator bool(JsonValue2 o)
        {
            return o._value.ToBoolean(null);
        }

        public static implicit operator string(JsonValue2 o)
        {
            return o._value.ToString();
        }

        public static explicit operator DateTime(JsonValue2 o)
        {
            return o._value.ToDateTime(null);
        }

        public static explicit operator Decimal(JsonValue2 o)
        {
            return o._value.ToDecimal(null);
        }

        public static explicit operator Single(JsonValue2 o)
        {
            return o._value.ToSingle(null);
        }

        public static explicit operator Double(JsonValue2 o)
        {
            return o._value.ToDouble(null);
        }

        public static explicit operator Byte(JsonValue2 o)
        {
            return o._value.ToByte(null);
        }

        public static explicit operator Guid(JsonValue2 o)
        {
            return Convert2.ToGuid(o._value);
        }


        public bool ToBoolean()
        {
            return _value.ToBoolean(null);
        }
        public byte ToByte()
        {
            return _value.ToByte(null);
        }
        public char ToChar()
        {
            return _value.ToChar(null);
        }
        public DateTime ToDateTime()
        {
            return _value.ToDateTime(null);
        }
        public decimal ToDecimal()
        {
            return _value.ToDecimal(null);
        }
        public double ToDouble()
        {
            return _value.ToDouble(null);
        }
        public short ToInt16()
        {
            return _value.ToInt16(null);
        }
        public int ToInt32()
        {
            return _value.ToInt32(null);
        }
        public long ToInt64()
        {
            return _value.ToInt64(null);
        }
        public sbyte ToSByte()
        {
            return _value.ToSByte(null);
        }
        public float ToSingle()
        {
            return _value.ToSingle(null);
        }
        public ushort ToUInt16()
        {
            return _value.ToUInt16(null);
        }
        public uint ToUInt32()
        {
            return _value.ToUInt32(null);
        }
        public ulong ToUInt64()
        {
            return _value.ToUInt64(null);
        }
        public object Value
        {
            get { return _value; }
        }
    }
}
