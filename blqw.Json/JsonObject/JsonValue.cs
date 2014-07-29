using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public struct JsonValue : IJsonObject, IConvertible
    {
        public readonly static JsonValue Undefined = new JsonValue(null) { IsUndefined = true };

        public JsonValue(string value)
            : this()
        {
            _value = value;
        }

        private string _value;

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
            return _value;
        }



        TypeCode IConvertible.GetTypeCode()
        {
            return System.TypeCode.String;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return bool.Parse(_value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return byte.Parse(_value, provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return char.Parse(_value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return DateTime.Parse(_value, provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return decimal.Parse(_value, provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return double.Parse(_value, provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return short.Parse(_value, provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return int.Parse(_value, provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return long.Parse(_value, provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return sbyte.Parse(_value, provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return float.Parse(_value, provider);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return _value;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)_value).ToType(conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ushort.Parse(_value, provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return uint.Parse(_value, provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ulong.Parse(_value, provider);
        }
    }
}
