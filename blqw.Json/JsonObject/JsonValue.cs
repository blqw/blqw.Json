using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    struct JsonValue : IJsonObject, IConvertible
    {
        public readonly static JsonValue Undefined = new JsonValue(null) { IsUndefined = true };

        public JsonValue(IConvertible value)
            : this()
        {
            _value = value;
        }

        private IConvertible _value;

        public IJsonObject this[string key]
        {
            get { return Undefined; }
        }

        public IJsonObject this[int index]
        {
            get { return JsonValue.Undefined; }
        }

        public ICollection<string> Keys
        {
            get { return new string[0]; }
        }

        public JsonTypeCode TypeCode
        {
            get { return JsonTypeCode.Value; }
        }

        public bool IsUndefined { get; private set; }

        public override string ToString()
        {
            return _value.ToString(null);
        }

        TypeCode IConvertible.GetTypeCode()
        {
            return _value.GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return _value.ToBoolean(provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return _value.ToByte(provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return _value.ToChar(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return _value.ToDateTime(provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return _value.ToDecimal(provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return _value.ToDouble(provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return _value.ToInt16(provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return _value.ToInt32(provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return _value.ToInt64(provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return _value.ToSByte(provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return _value.ToSingle(provider);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return _value.ToString(provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return _value.ToType(conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return _value.ToUInt16(provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return _value.ToUInt32(provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return _value.ToUInt64(provider);
        }

        public static implicit operator int(JsonValue o)
        {
            return o._value.ToInt32(null);
        }

        public static implicit operator long(JsonValue o)
        {
            return o._value.ToInt64(null);
        }

        public static implicit operator bool(JsonValue o)
        {
            return o._value.ToBoolean(null);
        }

        public static implicit operator string(JsonValue o)
        {
            return o._value.ToString();
        }

        public static implicit operator DateTime(JsonValue o)
        {
            return o._value.ToDateTime(null);
        }

        public static implicit operator Decimal(JsonValue o)
        {
            return o._value.ToDecimal(null);
        }

        public static implicit operator Single(JsonValue o)
        {
            return o._value.ToSingle(null);
        }

        public static implicit operator Double(JsonValue o)
        {
            return o._value.ToDouble(null);
        }

        public static implicit operator Byte(JsonValue o)
        {
            return o._value.ToByte(null);
        }

        public static implicit operator Guid(JsonValue o)
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
