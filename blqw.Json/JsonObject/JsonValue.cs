using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public struct JsonValue : IJsonObject, IConvertible
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
    }
}
