using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Runtime.Serialization;


namespace blqw.Serializable
{
    /// <summary> 用于将C#转换为Json字符串
    /// </summary>
    public class JsonBuilder
    {
        /// <summary> 将未知对象按属性名和值转换为Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="obj">非null的位置对象</param>
        protected virtual void AppendOther(object obj)
        {
            UnsafeAppend('{');

            var jtype = JsonType.Get(obj.GetType());
            var ms = jtype.Members;
            var comma = false;

            if (SerializableType)
            {
                AppendKey("$Type$", false);
                AppendString(jtype.Type.AssemblyQualifiedName);
                comma = true;
            }

            var length = SerializableField ? ms.Length : jtype.PropertyCount;
            for (int i = 0; i < length; i++)
            {
                var member = ms[i];
                if (member.NonSerialized && FormatAllMember == false)
                {
                    continue;
                }

                if (member.CanRead == false)
                {
                    continue;
                }

                var value = member.GetValue(obj);

                if (member.MustFormat && value != null && value is DBNull == false)
                {
                    if (comma)
                    {
                        UnsafeAppend(',');
                    }
                    else
                    {
                        comma = true;
                    }
                    AppendKey(member.JsonName, false);
                    AppendFormattable((IFormattable)value, member.FormatString, member.FormatProvider);
                }
                else
                {
                    comma = AppendObject(member.JsonName, false, value, comma) || comma;
                }

            }
            UnsafeAppend('}');
        }

        #region private

        private void AppendConvertible(IConvertible obj)
        {
            var @enum = obj as Enum;
            if (@enum != null)
            {
                AppendEnum(@enum);
                return;
            }
            switch (obj.GetTypeCode())
            {
                case TypeCode.Boolean:
                    AppendBoolean(obj.ToBoolean(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Byte:
                    AppendByte(obj.ToByte(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Char:
                    AppendChar(obj.ToChar(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.DBNull:
                    AppendObject(DBNull.Value);
                    break;
                case TypeCode.DateTime:
                    AppendDateTime(obj.ToDateTime(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Decimal:
                    AppendDecimal(obj.ToDecimal(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Double:
                    AppendDouble(obj.ToDouble(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Empty:
                    AppendObject(null);
                    break;
                case TypeCode.Int16:
                    AppendInt16(obj.ToInt16(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Int32:
                    AppendInt32(obj.ToInt32(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Int64:
                    AppendInt64(obj.ToInt64(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Object:
                    AppendObject(obj.ToType(typeof(object), CultureInfo.InvariantCulture));
                    break;
                case TypeCode.SByte:
                    AppendSByte(obj.ToSByte(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.Single:
                    AppendSingle(obj.ToSingle(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.String:
                    AppendString(obj.ToString(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.UInt16:
                    AppendUInt16(obj.ToUInt16(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.UInt32:
                    AppendUInt32(obj.ToUInt32(CultureInfo.InvariantCulture));
                    break;
                case TypeCode.UInt64:
                    AppendUInt64(obj.ToUInt64(CultureInfo.InvariantCulture));
                    break;
                default:
                    break;
            }
        }

        private void AppendByBuildSwitch(object obj)
        {
            _depth++;
            if (_depth > 64)
            {
                throw new NotSupportedException("对象过于复杂或存在循环引用");
            }
            if (obj is ValueType)
            {
                if (obj is IDictionary) AppendJson((IDictionary)obj);
                else if (obj is IEnumerable<KeyValuePair<string, object>>) AppendJson((IEnumerable<KeyValuePair<string, object>>)obj);
                else if (obj is IDataReader) AppendDataReader((IDataReader)obj);
                else if (obj is IList) AppendArray((IList)obj);
                else if (obj is IEnumerable) AppendArray((IEnumerable)obj);
                else if (obj is IEnumerator) AppendArray((IEnumerator)obj);
                else AppendOther(obj);
            }
            else if (CheckLoopRef == false)
            {
                if (obj is DataSet) AppendDataSet((DataSet)obj);
                else if (obj is DataTable) AppendDataTable((DataTable)obj);
                else if (obj is DataView) AppendDataView((DataView)obj);
                else if (obj is DataRow) AppendDataRow((DataRow)obj);
                else if (obj is DataRowView) AppendDataRow((DataRow)obj);
                else if (obj is NameValueCollection) AppendNameValueCollection((NameValueCollection)obj);
                else if (obj is IDictionary) AppendJson((IDictionary)obj);
                else if (obj is IEnumerable<KeyValuePair<string, object>>) AppendJson((IEnumerable<KeyValuePair<string, object>>)obj);
                else if (obj is IDataReader) AppendDataReader((IDataReader)obj);
                else if (obj is IList) AppendArray((IList)obj);
                else if (obj is IEnumerable) AppendArray((IEnumerable)obj);
                else if (obj is IEnumerator) AppendArray((IEnumerator)obj);
                else AppendOther(obj);
            }
            else if (_loopObject.Contains(obj) == false)
            {
                var index = _loopObject.Add(obj);
                if (obj is DataSet) AppendDataSet((DataSet)obj);
                else if (obj is DataTable) AppendDataTable((DataTable)obj);
                else if (obj is DataView) AppendDataView((DataView)obj);
                else if (obj is DataRow) AppendDataRow((DataRow)obj);
                else if (obj is DataRowView) AppendDataRow((DataRow)obj);
                else if (obj is NameValueCollection) AppendNameValueCollection((NameValueCollection)obj);
                else if (obj is IDictionary) AppendJson((IDictionary)obj);
                else if (obj is IEnumerable<KeyValuePair<string, object>>) AppendJson((IEnumerable<KeyValuePair<string, object>>)obj);
                else if (obj is IDataReader) AppendDataReader((IDataReader)obj);
                else if (obj is IList) AppendArray((IList)obj);
                else if (obj is IEnumerable) AppendArray((IEnumerable)obj);
                else if (obj is IEnumerator) AppendArray((IEnumerator)obj);
                else if (obj is DynamicObject) AppendDynamic((DynamicObject)obj);
                else AppendOther(obj);
                _loopObject.RemoveAt(index);
            }
            else
            {
                Buffer.Append("undefined");
            }
            _depth--;
        }

        private static IEnumerable GetDataReaderNames(IDataRecord reader)
        {
            int c = reader.FieldCount;
            for (int i = 0; i < c; i++)
            {
                yield return reader.GetName(i);
            }
        }

        private static IEnumerable GetDataReaderValues(IDataReader reader)
        {
            int c = reader.FieldCount;
            while (reader.Read())
            {
                object[] values = new object[c];
                reader.GetValues(values);
                yield return values;
            }
        }

        public static string EscapeString(string str)
        {
            if (str == null)
            {
                return null;
            }
            var size = str.Length * 2;
            if (size > ushort.MaxValue)
            {
                size = ushort.MaxValue;
            }
            QuickStringWriter buffer = null;
            try
            {
                unsafe
                {
                    var length = str.Length;
                    fixed (char* fp = str)
                    {
                        char* p = fp;
                        char* end = fp + length;
                        char* flag = fp;
                        while (p < end)
                        {
                            char c = *p;
                            switch (c)
                            {
                                case '\\':
                                case '"':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size).Append('"');
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    flag = p;
                                    break;
                                case '\n':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('n');
                                    flag = p + 1;
                                    break;
                                case '\r':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('r');
                                    flag = p + 1;
                                    break;
                                case '\t':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('t');
                                    flag = p + 1;
                                    break;
                                case '\f':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('f');
                                    flag = p + 1;
                                    break;
                                case '\0':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('0');
                                    flag = p + 1;
                                    break;
                                case '\a':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('a');
                                    flag = p + 1;
                                    break;
                                case '\b':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('b');
                                    flag = p + 1;
                                    break;
                                case '\v':
                                    if (buffer == null) buffer = new QuickStringWriter((ushort)size);
                                    buffer.Append(flag, 0, (int)(p - flag));
                                    buffer.Append('\\');
                                    buffer.Append('v');
                                    flag = p + 1;
                                    break;
                                default:
                                    break;
                            }
                            p++;
                        }
                        if (flag == fp)
                        {
                            if (buffer == null)
                            {
                                return str;
                            }
                            buffer.Append(fp, 0, length);
                        }
                        else if (p > flag)
                        {
                            if (buffer == null) buffer = new QuickStringWriter((ushort)size).Append('"');
                            buffer.Append(flag, 0, (int)(p - flag));
                        }
                    }
                }
                return buffer.ToString();
            }
            finally
            {
                if (buffer != null)
                {
                    buffer.Dispose();
                }
            }


        }

        //循环引用对象缓存区
        private IList _loopObject;

        private int _depth;

        #endregion

        protected QuickStringWriter Buffer;//字符缓冲区

        public JsonBuilder()
            : this(JsonBuilderSettings.Default)
        {

        }

        public JsonBuilder(JsonBuilderSettings settings)
        {
            FormatDate = (settings & JsonBuilderSettings.FormatDate) != 0;
            FormatTime = (settings & JsonBuilderSettings.FormatTime) != 0;
            SerializableField = (settings & JsonBuilderSettings.SerializableField) != 0;
            QuotWrapNumber = (settings & JsonBuilderSettings.QuotWrapNumber) != 0;
            BooleanToNumber = (settings & JsonBuilderSettings.BooleanToNumber) != 0;
            EnumToNumber = (settings & JsonBuilderSettings.EnumToNumber) != 0;
            CheckLoopRef = (settings & JsonBuilderSettings.CheckLoopRef) != 0;
            IgnoreEmptyTime = (settings & JsonBuilderSettings.IgnoreEmptyTime) != 0;
            QuotWrapBoolean = (settings & JsonBuilderSettings.QuotWrapBoolean) != 0;
            IgnoreNullMember = (settings & JsonBuilderSettings.IgnoreNullMember) != 0;
            SerializableType = (settings & JsonBuilderSettings.SerializableType) != 0;
            FormatAllMember = (settings & JsonBuilderSettings.FormatAllMember) != 0;
            FilterSpecialCharacter = (settings & JsonBuilderSettings.FilterSpecialCharacter) != 0;
            CastUnicode = (settings & JsonBuilderSettings.CastUnicode) != 0;
        }

        #region settings

        /// <summary> 格式化 DateTime 对象中的日期
        /// </summary>
        public bool FormatDate;
        /// <summary> 格式化 DateTime 对象中的时间
        /// </summary>
        public bool FormatTime;
        /// <summary> 同时序列化字段
        /// </summary>
        public bool SerializableField;
        /// <summary> 使用双引号包装数字的值
        /// </summary>
        public bool QuotWrapNumber;
        /// <summary> 将布尔值转为数字值 true = 1 ,false = 0
        /// </summary>
        public bool BooleanToNumber;
        /// <summary> 将枚举转为对应的数字值
        /// </summary>
        public bool EnumToNumber;
        /// <summary> 检查循环引用,发现循环应用时输出 undefined
        /// </summary>
        public bool CheckLoopRef;
        /// <summary> 格式化 DateTime 对象中的时间时忽略(00:00:00.000000) ,存在FormatTime时才生效
        /// </summary>
        public bool IgnoreEmptyTime;
        /// <summary> 
        /// 使用双引号包装布尔的值
        /// </summary>
        public bool QuotWrapBoolean;
        /// <summary> 
        /// 忽略值是null的属性
        /// </summary>
        public bool IgnoreNullMember;
        /// <summary> 
        /// 输出类型信息
        /// </summary>
        public bool SerializableType;
        /// <summary> 
        /// 输出所有属性/字段
        /// </summary>
        public bool FormatAllMember;
        /// <summary> 
        /// 过滤特殊字符
        /// </summary>
        public bool FilterSpecialCharacter;
        /// <summary>
        /// 是否将大于255的字符转为Unicode编码
        /// </summary>
        public bool CastUnicode;
        #endregion

        /// <summary> 将对象转换为Json字符串
        /// </summary>
        public string ToJsonString(object obj)
        {
            if (obj == null || obj is DBNull)
            {
                return "null";
            }
            using (Buffer = new QuickStringWriter(4096))
            {
                if (CheckLoopRef)
                {
                    _loopObject = new ArrayList(32);
                }
                AppendObject(obj);
                var json = Buffer.ToString();
                _loopObject = null;
                return json;
            }
        }

        /// <summary> 将 任意对象 转换Json字符串写入Buffer
        /// </summary>
        /// <param name="obj">任意对象</param>
        protected void AppendObject(object obj)
        {
            if (obj == null || obj is DBNull)
            {
                Buffer.Append("null");
                return;
            }

            var tojson = obj as IFormatProvider;
            if (tojson != null)
            {
                obj = tojson.GetFormat(typeof(Json));
                if (obj != null)
                {
                    AppendObject(obj);
                    return;
                }
            }

            var s = obj as string;
            if (s != null)
            {
                AppendString(s);
            }
            else
            {
                var conv = obj as IConvertible;
                if (conv != null)
                {
                    AppendConvertible(conv);
                }
                else if (obj is Guid)
                {
                    AppendGuid((Guid)obj);
                }
                else
                {
                    AppendByBuildSwitch(obj);
                }
            }
        }


        /// <summary> 将 任意对象 转换Json字符串写入Buffer
        /// </summary>
        /// <param name="key"></param>
        /// <param name="escape">key中是否有(引号,回车,制表符等)特殊字符,需要转义</param>
        /// <param name="obj">任意对象</param>
        /// <param name="hasPrevComma">是否需要在前面加逗号</param>
        protected bool AppendObject(string key, bool escape, object obj, bool hasPrevComma)
        {
            if (obj == null || obj is DBNull)
            {
                if (IgnoreNullMember)
                {
                    return false;
                }
                if (hasPrevComma)
                {
                    Buffer.Append(',');
                }
                if (key != null)
                {
                    AppendKey(key, escape);
                }
                Buffer.Append("null");
                return true;
            }

            var tojson = obj as IFormatProvider;
            if (tojson != null)
            {
                var obj1 = tojson.GetFormat(typeof(Json));
                if (obj1 != null)
                {
                    return AppendObject(key, escape, obj1, hasPrevComma);
                }
            }
            if (hasPrevComma)
            {
                Buffer.Append(',');
            }
            if (key != null)
            {
                AppendKey(key, escape);
            }
            var s = obj as string;
            if (s != null)
            {
                AppendString(s);
            }
            else
            {
                var conv = obj as IConvertible;
                if (conv != null)
                {
                    AppendConvertible(conv);
                }
                else if (obj is Guid)
                {
                    AppendGuid((Guid)obj);
                }
                else
                {
                    AppendByBuildSwitch(obj);
                }
            }
            return true;
        }

        /// <summary> 非安全方式向Buffer追加一个字符(该方法不会验证字符的有效性)
        /// </summary>
        /// <param name="value">向Buffer追加的字符</param>
        protected virtual void UnsafeAppend(Char value)
        {
            Buffer.Append(value);
        }
        /// <summary> 非安全方式向Buffer追加一个字符串(该方法不会验证字符串的有效性)
        /// </summary>
        /// <param name="value">向Buffer追加的字符串</param>
        protected virtual void UnsafeAppend(string value)
        {
            Buffer.Append(value);
        }
        /// <summary> 追加Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="escape">key中是否有(引号,回车,制表符等)特殊字符,需要转义</param>
        protected virtual void AppendKey(string key, bool escape)
        {
            if (escape)
            {
                AppendString(key);
            }
            else
            {
                Buffer.Append('"');
                Buffer.Append(key);
                Buffer.Append('"');
            }
            Buffer.Append(':');
        }


        #region BuildSwitch

        /// <summary> 将 Byte 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Byte 对象</param>
        protected virtual void AppendByte(Byte value) { AppendNumber(value); }
        /// <summary> 将 Decimal 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Decimal 对象</param>
        protected virtual void AppendDecimal(Decimal value) { AppendNumber(value); }
        /// <summary> 将 Int16 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Int16 对象</param>
        protected virtual void AppendInt16(Int16 value) { AppendNumber(value); }
        /// <summary> 将 Int32 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Int32 对象</param>
        protected virtual void AppendInt32(Int32 value) { AppendNumber(value); }
        /// <summary> 将 Int64 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Int64 对象</param>
        protected virtual void AppendInt64(Int64 value) { AppendNumber(value); }
        /// <summary> 将 SByte 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">SByte 对象</param>
        protected virtual void AppendSByte(SByte value) { AppendNumber(value); }
        /// <summary> 将 UInt16 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">UInt16 对象</param>
        protected virtual void AppendUInt16(UInt16 value) { AppendNumber(value); }
        /// <summary> 将 UInt32 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">UInt32 对象</param>
        protected virtual void AppendUInt32(UInt32 value) { AppendNumber(value); }
        /// <summary> 将 UInt64 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">UInt64 对象</param>
        protected virtual void AppendUInt64(UInt64 value) { AppendNumber(value); }
        /// <summary> 将 Double 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Double 对象</param>
        protected virtual void AppendDouble(Double value) { AppendNumber(value); }
        /// <summary> 将 Single 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Single 对象</param>
        protected virtual void AppendSingle(Single value) { AppendNumber(value); }
        /// <summary> 将 Boolean 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Boolean 对象</param>
        protected virtual void AppendBoolean(Boolean value)
        {
            if (BooleanToNumber)
            {
                AppendNumber(value ? 1 : 0);
            }
            else if (QuotWrapBoolean)
            {
                Buffer.Append('"');
                Buffer.Append(value);
                Buffer.Append('"');
            }
            else
            {
                Buffer.Append(value);
            }
        }

        /// <summary>
        /// 特殊字符表
        /// </summary>
        static readonly string[] SpecialCharacters = InitSpecialCharacters();
        private static string[] InitSpecialCharacters()
        {
            var chars = new string[65536];
            for (int i = 0; i < 256; i++)
            {
                if (char.IsControl((char)i))
                {
                    chars[i] = "\\u" + i.ToString("x4");
                }
            }
            chars['\\'] = @"\\";
            chars['\"'] = @"\""";
            chars['/'] = @"\/"; //json标准中该字符是否转换均可
            chars['\b'] = @"\b";
            chars['\f'] = @"\f";
            chars['\n'] = @"\n";
            chars['\r'] = @"\r";
            chars['\t'] = @"\t";

            //unicode编码
            for (int i = 256; i < 65536; i++)
            {
                chars[i] = "\\u" + i.ToString("x4");
            }
            return chars;
        }

        /// <summary> 将 Char 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Char 对象</param>
        protected virtual void AppendChar(Char value)
        {
            Buffer.Append('"');
            if (value < 256)
            {
                var escape = SpecialCharacters[value];
                if (escape == null)
                {
                    Buffer.Append(value);
                }
                else if (FilterSpecialCharacter == false)
                {
                    Buffer.Append(escape);
                }
            }
            else if (CastUnicode)
            {
                Buffer.Append(SpecialCharacters[value]);
            }
            else
            {
                Buffer.Append(value);
            }
            Buffer.Append('"');
        }
        /// <summary> 将 String 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Char 对象</param>
        protected virtual void AppendString(String value)
        {
            Buffer.Append('"');
            unsafe
            {
                var length = value.Length;
                fixed (char* fp = value)
                {
                    char* p = fp;
                    char* end = fp + length;
                    char* flag = fp;
                    while (p < end)
                    {
                        char c = *p;
                        if (c < 256)
                        {
                            var escape = SpecialCharacters[c];
                            if (escape != null)
                            {
                                if (FilterSpecialCharacter && escape[1] == 'u')
                                {
                                    Buffer.Append(flag, 0, (int)(p - flag));
                                }
                                else
                                {
                                    Buffer.Append(flag, 0, (int)(p - flag));
                                    Buffer.Append(escape);
                                }
                                flag = p + 1;
                            }
                        }
                        else if (CastUnicode)
                        {
                            Buffer.Append(flag, 0, (int)(p - flag));
                            Buffer.Append(SpecialCharacters[c]);
                            flag = p + 1;
                        }
                        p++;
                    }
                    if (flag == fp)
                    {
                        Buffer.Append(fp, 0, length);
                    }
                    else if (p > flag)
                    {
                        Buffer.Append(flag, 0, (int)(p - flag));
                    }
                }
            }

            Buffer.Append('"');
        }
        /// <summary> 将 DateTime 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">DateTime 对象</param>
        protected virtual void AppendDateTime(DateTime value)
        {
            Buffer.Append('"');
            if (FormatTime && IgnoreEmptyTime)
            {
                Buffer.Append(value, FormatDate, value.Millisecond > 0 || value.Hour > 0 || value.Minute > 0 || value.Second > 0, false);
            }
            else
            {
                Buffer.Append(value, FormatDate, FormatTime, false);
            }
            Buffer.Append('"');
        }
        /// <summary> 将 Guid 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Guid 对象</param>
        protected virtual void AppendGuid(Guid value)
        {
            Buffer.Append('"').Append(value).Append('"');
        }
        /// <summary> 将 枚举 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">枚举 对象</param>
        protected virtual void AppendEnum(Enum value)
        {
            if (EnumToNumber)
            {
                AppendNumber(value);
            }
            else
            {
                Buffer.Append('"').Append(value.ToString()).Append('"');
            }
        }
        /// <summary> 将 数字 类型对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="number">数字对象</param>
        protected virtual void AppendNumber(IConvertible number)
        {
            switch (number.GetTypeCode())
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    if (QuotWrapNumber)
                    {
                        Buffer.Append('"');
                        Buffer.Append(number.ToString(CultureInfo.InvariantCulture));
                        Buffer.Append('"');
                    }
                    else
                    {
                        Buffer.Append(number.ToString(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Int16:
                    if (QuotWrapNumber)
                    {
                        Buffer.Append('"');
                        Buffer.Append(number.ToInt16(CultureInfo.InvariantCulture));
                        Buffer.Append('"');
                    }
                    else
                    {
                        Buffer.Append(number.ToInt16(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Int32:
                case TypeCode.Int64:
                    if (QuotWrapNumber)
                    {
                        Buffer.Append('"');
                        Buffer.Append(number.ToInt64(CultureInfo.InvariantCulture));
                        Buffer.Append('"');
                    }
                    else
                    {
                        Buffer.Append(number.ToInt64(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.SByte:
                    if (QuotWrapNumber)
                    {
                        Buffer.Append('"');
                        Buffer.Append(number.ToSByte(CultureInfo.InvariantCulture));
                        Buffer.Append('"');
                    }
                    else
                    {
                        Buffer.Append(number.ToSByte(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Byte:
                    if (QuotWrapNumber)
                    {
                        Buffer.Append('"');
                        Buffer.Append(number.ToByte(CultureInfo.InvariantCulture));
                        Buffer.Append('"');
                    }
                    else
                    {
                        Buffer.Append(number.ToByte(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.UInt16:
                    if (QuotWrapNumber)
                    {
                        Buffer.Append('"');
                        Buffer.Append(number.ToUInt16(CultureInfo.InvariantCulture));
                        Buffer.Append('"');
                    }
                    else
                    {
                        Buffer.Append(number.ToUInt16(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    if (QuotWrapNumber)
                    {
                        Buffer.Append('"');
                        Buffer.Append(number.ToUInt64(CultureInfo.InvariantCulture));
                        Buffer.Append('"');
                    }
                    else
                    {
                        Buffer.Append(number.ToUInt64(CultureInfo.InvariantCulture));
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary> 将 数组 对象转换Json中的数组字符串写入Buffer
        /// </summary>
        /// <param name="enumerator">迭代器</param>
        protected virtual void AppendArray(IEnumerator enumerator)
        {
            Buffer.Append('[');
            if (enumerator.MoveNext())
            {
                AppendObject(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    Buffer.Append(',');
                    AppendObject(enumerator.Current);
                }
            }
            Buffer.Append(']');
        }
        /// <summary> 将 数组 对象转换Json中的数组字符串写入Buffer
        /// </summary>
        /// <param name="enumerable">迭代器</param>
        protected virtual void AppendArray(IEnumerable enumerable)
        {
            AppendArray(enumerable.GetEnumerator());
        }
        /// <summary> 将 数组 对象转换Json中的数组字符串写入Buffer
        /// </summary>
        /// <param name="list">数组或集合</param>
        protected virtual void AppendArray(IList list)
        {
            var length = list.Count;
            if (length == 0)
            {
                Buffer.Append("[]");
                return;
            }
            Buffer.Append('[');
            AppendObject(list[0]);
            for (int i = 1; i < length; i++)
            {
                Buffer.Append(',');
                AppendObject(list[i]);
            }
            Buffer.Append(']');
        }
        /// <summary> 将 键值对 对象转换Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="dict">键值对 对象</param>
        protected virtual void AppendJson(IDictionary dict)
        {
            AppendJson(dict.Keys.GetEnumerator(), dict.Values.GetEnumerator());
        }
        /// <summary> 将 键值对 对象转换Json中的键值字符串写入Buffer,兼容动态类型
        /// </summary>
        /// <param name="dict">键值对 对象</param>
        protected virtual void AppendJson(IEnumerable<KeyValuePair<string, object>> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            Buffer.Append('{');
            var comma = false;
            while (enumerator.MoveNext())
            {
                comma = AppendObject(enumerator.Current.Key, true, enumerator.Current.Value, comma) || comma;
            }
            Buffer.Append('}');
        }

        /// <summary> 将 DataSet 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="dataset">DataSet 对象</param>
        protected virtual void AppendDataSet(DataSet dataset)
        {
            Buffer.Append('{');
            var ee = dataset.Tables.GetEnumerator();
            if (ee.MoveNext())
            {
                DataTable table = (DataTable)ee.Current;
                AppendKey(table.TableName, true);
                AppendDataTable(table);
                while (ee.MoveNext())
                {
                    Buffer.Append(',');
                    table = (DataTable)ee.Current;
                    AppendKey(table.TableName, true);
                    AppendDataTable(table);
                }
            }
            Buffer.Append('}');
        }
        /// <summary> 将 DataTable 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="table">DataTable 对象</param>
        protected virtual void AppendDataTable(DataTable table)
        {
            Buffer.Append('[');

            var ee = table.Rows.GetEnumerator();
            if (ee.MoveNext())
            {
                var columns = table.Columns;
                var names = new string[columns.Count];
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = EscapeString(columns[i].ColumnName);
                }
                AppendJson(names, ((DataRow)ee.Current).ItemArray);
                while (ee.MoveNext())
                {
                    Buffer.Append(',');
                    AppendJson(names, ((DataRow)ee.Current).ItemArray);
                }
            }

            Buffer.Append(']');
        }
        /// <summary> 将 DataRow 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="row">DataRow 对象</param>
        protected virtual void AppendDataRow(DataRow row)
        {
            var table = row.Table;
            var columns = table.Columns;
            var names = new string[columns.Count];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = EscapeString(columns[i].ColumnName);
            }
            AppendJson(names, (row).ItemArray);
        }
        /// <summary> 将 DataView 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="tableView">DataView 对象</param>
        protected virtual void AppendDataView(DataView tableView)
        {
            Buffer.Append('[');
            var table = tableView.ToTable();
            var ee = tableView.GetEnumerator();
            if (ee.MoveNext())
            {
                var columns = table.Columns;
                var names = new string[columns.Count];
                var values = new object[columns.Count];
                var row = (DataRowView)ee.Current;
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = EscapeString(columns[i].ColumnName);
                    values[i] = row[i];
                }
                AppendJson(names, values);
                while (ee.MoveNext())
                {
                    row = (DataRowView)ee.Current;
                    for (int i = 0; i < names.Length; i++)
                    {
                        values[i] = row[i];
                    }
                    Buffer.Append(',');
                    AppendJson(names, values);
                }
            }

            Buffer.Append(']');
        }
        /// <summary> 将 DataRowView 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="row">DataRowView 对象</param>
        protected virtual void AppendDataRow(DataRowView row)
        {
            var table = row.DataView.ToTable();
            var columns = table.Columns;
            var names = new string[columns.Count];
            var values = new object[columns.Count];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = EscapeString(columns[i].ColumnName);
                values[i] = row[i];
            }
            AppendJson(names, values);
        }
        /// <summary> 将 IDataReader 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="reader">IDataReader 对象</param>
        protected virtual void AppendDataReader(IDataReader reader)
        {
            Buffer.Append('[');
            if (reader.Read())
            {
                var length = reader.FieldCount;
                var names = new string[reader.FieldCount];
                var values = new object[reader.FieldCount];
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = EscapeString(reader.GetName(i));
                }
                reader.GetValues(values);
                AppendJson(names, values);
                while (reader.Read())
                {
                    Buffer.Append(',');
                    reader.GetValues(values);
                    AppendJson(names, values);
                }
            }
            Buffer.Append(']');
        }

        /// <summary> 将 NameValueCollection 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="nv">NameValueCollection 对象</param>
        protected virtual void AppendNameValueCollection(NameValueCollection nv)
        {
            AppendJson(nv.AllKeys, k => (string)k, k => nv[(string)k], true);
        }

        /// <summary> 将 DynamicObject 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="obj">DynamicObject 对象</param>
        protected virtual void AppendDynamic(DynamicObject obj)
        {
            AppendJson(
                obj.GetDynamicMemberNames().GetEnumerator(),
                name => (string)name,
                name =>
                {
                    object val;
                    if (obj.TryGetMember(new MyGetMemberBinder((string)name), out val) == false)
                    {
                        obj.TryGetIndex(new MyGetIndexBinder((string)name), new object[] { name }, out val);
                    }
                    return val;
                },
                true);
        }

        #endregion


        /// <summary> 将 键的迭代器 和 值的迭代器 转换Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="keys">键的迭代器</param>
        /// <param name="values">值的迭代器</param>
        protected virtual void AppendJson(IEnumerator keys, IEnumerator values)
        {
            Buffer.Append('{');
            var comma = false;
            while (keys.MoveNext() && values.MoveNext())
            {
                comma = AppendObject(keys.Current + "", true, values.Current, comma) || comma;
            }
            Buffer.Append('}');
        }

        /// <summary> 将 对象枚举 和 值转换委托 转换Json中的数组字符串写入Buffer
        /// </summary>
        /// <param name="enumerator">迭代器</param>
        /// <param name="getVal">值转换委托</param>
        protected virtual void AppendArray(IEnumerator enumerator, Converter<object, object> getVal)
        {
            Buffer.Append('[');
            if (enumerator.MoveNext())
            {
                AppendObject(getVal(enumerator.Current));
                while (enumerator.MoveNext())
                {
                    Buffer.Append(',');
                    AppendObject(getVal(enumerator.Current));
                }
            }
            Buffer.Append(']');
        }

        /// <summary> 将 数组 对象转换Json中的数组字符串写入Buffer
        /// </summary>
        /// <param name="list">数组或集合</param>
        protected virtual void AppendArray(IList list, Converter<object, object> getVal)
        {
            var length = list.Count;
            if (length == 0)
            {
                Buffer.Append("[]");
                return;
            }
            Buffer.Append('[');
            AppendObject(getVal(list[0]));
            for (int i = 1; i < length; i++)
            {
                Buffer.Append(',');
                AppendObject(getVal(list[i]));
            }
            Buffer.Append(']');
        }

        /// <summary> 将 迭代器 和 键/值转换委托 转换Json中的键值对象字符串写入Buffer
        /// </summary>
        /// <param name="enumerator">迭代器</param>
        /// <param name="getKey">键转换委托</param>
        /// <param name="getVal">值转换委托</param>
        /// <param name="escapekey">是否需要对Key进行转义</param>
        protected virtual void AppendJson(IEnumerator enumerator, Converter<object, string> getKey, Converter<object, object> getVal, bool escapekey)
        {
            Buffer.Append('{');
            var comma = false;
            while (enumerator.MoveNext())
            {
                comma = AppendObject(getKey(enumerator.Current), escapekey, getVal(enumerator.Current), comma) || comma;
            }
            Buffer.Append('}');
        }

        /// <summary> 将 集合 和 键/值转换委托 转换Json中的键值对象字符串写入Buffer
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="getKey">键转换委托</param>
        /// <param name="getVal">值转换委托</param>
        /// <param name="escapekey">是否需要对Key进行转义</param>
        protected virtual void AppendJson(IList list, Converter<object, string> getKey, Converter<object, object> getVal, bool escapekey)
        {
            var length = list.Count;
            if (length == 0)
            {
                Buffer.Append("{}");
                return;
            }
            Buffer.Append('{');
            var comma = false;
            for (int i = 0; i < length; i++)
            {
                var obj = list[i];
                comma = AppendObject(getKey(obj), escapekey, getVal(obj), comma) || comma;
            }
            Buffer.Append('}');
        }


        /// <summary> 将 可格式化 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="val">可格式化对象</param>
        /// <param name="format">格式化参数</param>
        /// <param name="provider">格式化机制</param>
        protected virtual void AppendFormattable(IFormattable formattable, string format, IFormatProvider provider)
        {
            if (formattable is DateTime)
            {
                if (string.Equals(format, "yyyy-MM-dd HH:mm:ss", StringComparison.Ordinal))
                {
                    Buffer.Append('"');
                    Buffer.Append((DateTime)formattable, true, true, false);
                    Buffer.Append('"');
                }
                else if (string.Equals(format, "yyyy-MM-dd HH:mm:ss.fff", StringComparison.Ordinal))
                {
                    Buffer.Append('"');
                    Buffer.Append((DateTime)formattable, true, true, true);
                    Buffer.Append('"');
                }
                else if (string.Equals(format, "HH:mm:ss", StringComparison.Ordinal))
                {
                    Buffer.Append('"');
                    Buffer.Append((DateTime)formattable, false, true, false);
                    Buffer.Append('"');
                }
                else if (string.Equals(format, "HH:mm:ss.fff", StringComparison.Ordinal))
                {
                    Buffer.Append('"');
                    Buffer.Append((DateTime)formattable, false, true, true);
                    Buffer.Append('"');
                }
                else if (string.Equals(format, "yyyy-MM-dd", StringComparison.Ordinal))
                {
                    Buffer.Append('"');
                    Buffer.Append((DateTime)formattable, false, false, false);
                    Buffer.Append('"');
                }
                else if (string.Equals(format, "fff", StringComparison.Ordinal))
                {
                    Buffer.Append('"');
                    Buffer.Append((DateTime)formattable, false, false, true);
                    Buffer.Append('"');
                }
                else
                {
                    AppendString(formattable.ToString(format, provider));
                }
            }
            else if (formattable is Guid && (format == null || format.Length == 1))
            {
                Buffer.Append('"');
                Buffer.Append((Guid)formattable, format[0]);
                Buffer.Append('"');
            }
            else
            {
                AppendString(formattable.ToString(format, provider));
            }
        }

        /// <summary> 将 键集合 和 值集合 转换Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <param name="values">值集合</param>
        protected virtual void AppendJson(IList keys, IList values)
        {
            var length = keys.Count;
            if (length != values.Count)
            {
                throw new ArgumentException("键和值的数量不相同");
            }
            if (length == 0)
            {
                Buffer.Append("{}");
                return;
            }
            Buffer.Append('{');

            var comma = false;
            for (int i = 0; i < length; i++)
            {
                comma = AppendObject(keys[i] + "", true, values[i], comma) || comma;
            }
            Buffer.Append('}');
        }

        #region MyRegion
        class MyGetIndexBinder : GetIndexBinder
        {
            public MyGetIndexBinder(string name)
                : base(new CallInfo(1, name))
            {

            }
            public override DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion)
            {
                return target;
            }
        }

        class MyGetMemberBinder : GetMemberBinder
        {
            public MyGetMemberBinder(string name)
                : base(name, false)
            {

            }

            public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
            {
                return target;
            }
        }
        #endregion
    }
}
