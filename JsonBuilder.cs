using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace blqw
{
    /// <summary> 用于将C#转换为Json字符串
    /// </summary>
    public class JsonBuilder
    {
        private Dictionary<object, object> _LoopObject = new Dictionary<object, object>();//循环引用对象缓存区
        private UnsafeStringWriter Buffer = new UnsafeStringWriter();//字符缓冲区

        /// <summary> 将对象转换为Json字符串
        /// </summary>
        public string ToJsonString(object obj)
        {
            string str;
            unsafe
            {
                char[] arr = new char[4096];
                fixed (char* p = arr)
                {
                    Buffer.Ready(p, 4096);
                }
                AppendObject(obj);
                str = Buffer.ToString();
                Buffer.Close();
            }
            return str;
        }
        /// <summary> 将 任意对象 转换Json字符串写入Buffer
        /// </summary>
        /// <param name="obj">任意对象</param>
        protected void AppendObject(object obj)
        {
            if (obj == null || obj is DBNull) Buffer.Append("null");
            else if (obj is String) AppendString((String)obj);
            else if (obj is ValueType) AppendValueType(obj);//值类型
            else if (_LoopObject.ContainsKey(obj) == false)
            {
                _LoopObject.Add(obj, null);
                if (obj is IDictionary) AppendJson((IDictionary)obj);
                else if (obj is IEnumerable) AppendArray((IEnumerable)obj);
                else if (obj is DataSet) AppendDataSet((DataSet)obj);
                else if (obj is DataTable) AppendDataTable((DataTable)obj);
                else if (obj is DataView) AppendDataView((DataView)obj);
                else AppendOther(obj);
                _LoopObject.Remove(obj);
            }
            else
            {
                Buffer.Append("undefined");
            }
        }

        private static System.Reflection.Module SystemModule = typeof(int).Module;
        private void AppendValueType(object obj)
        {
            var type = obj.GetType();
            if (obj is Enum) AppendEnum((Enum)obj);
            else if (obj.GetType().Module == SystemModule)//如果是系统模块中的值类型对象
            {
                if (obj is Int32) AppendInt32((Int32)obj);
                else if (obj is Boolean) AppendBoolean((Boolean)obj);
                else if (obj is DateTime) AppendDateTime((DateTime)obj);
                else if (obj is Double) AppendDouble((Double)obj);
                else if (obj is Decimal) AppendDecimal((Decimal)obj);
                else if (obj is Char) AppendChar((Char)obj);
                else if (obj is Single) AppendSingle((Single)obj);
                else if (obj is Guid) AppendGuid((Guid)obj);
                else if (obj is Byte) AppendByte((Byte)obj);
                else if (obj is Int16) AppendInt16((Int16)obj);
                else if (obj is Int64) AppendInt64((Int64)obj);
                else if (obj is SByte) AppendSByte((SByte)obj);
                else if (obj is UInt32) AppendUInt32((UInt32)obj);
                else if (obj is UInt64) AppendUInt64((UInt64)obj);
                else AppendString(obj.ToString());
            }
            else if (_LoopObject.ContainsKey(obj) == false)
            {
                _LoopObject.Add(obj, null);
                AppendOther(obj);
                _LoopObject.Remove(obj);
            }
            else
            {
                Buffer.Append("undefined");
            }
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
        /// <summary> 将未知对象按属性名和值转换为Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="obj">非null的位置对象</param>
        protected virtual void AppendOther(object obj)
        {
            Type t = obj.GetType();
            Buffer.Append('{');
            string fix = "";
            foreach (var p in t.GetProperties())
            {
                if (p.CanRead)
                {
                    Buffer.Append(fix);
                    AppendKey(p.Name, false);
                    object value = p.GetValue(obj, null);
                    AppendObject(value);
                    fix = ",";
                }
            }
            Buffer.Append('}');
        }
        /// <summary> "
        /// </summary>
        public const char Quot = '"';
        /// <summary> :
        /// </summary>
        public const char Colon = ':';
        /// <summary> ,
        /// </summary>
        public const char Comma = ',';
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
                Buffer.Append(Quot);
                Buffer.Append(key);
                Buffer.Append(Quot);
            }
            Buffer.Append(Colon);
        }
        /// <summary> Byte 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Byte 对象</param>
        protected virtual void AppendByte(Byte value) { AppendNumber(value); }
        /// <summary> Decimal 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Decimal 对象</param>
        protected virtual void AppendDecimal(Decimal value) { AppendNumber(value); }
        /// <summary> Int16 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Int16 对象</param>
        protected virtual void AppendInt16(Int16 value) { AppendNumber(value); }
        /// <summary> Int32 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Int32 对象</param>
        protected virtual void AppendInt32(Int32 value) { AppendNumber(value); }
        /// <summary> Int64 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Int64 对象</param>
        protected virtual void AppendInt64(Int64 value) { AppendNumber(value); }
        /// <summary> SByte 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">SByte 对象</param>
        protected virtual void AppendSByte(SByte value) { AppendNumber(value); }
        /// <summary> UInt16 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">UInt16 对象</param>
        protected virtual void AppendUInt16(UInt16 value) { AppendNumber(value); }
        /// <summary> UInt32 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">UInt32 对象</param>
        protected virtual void AppendUInt32(UInt32 value) { AppendNumber(value); }
        /// <summary> UInt64 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">UInt64 对象</param>
        protected virtual void AppendUInt64(UInt64 value) { AppendNumber(value); }
        /// <summary> Double 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Double 对象</param>
        protected virtual void AppendDouble(Double value) { AppendNumber(value); }
        /// <summary> Single 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Single 对象</param>
        protected virtual void AppendSingle(Single value) { AppendNumber(value); }
        /// <summary> Boolean 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Boolean 对象</param>
        protected virtual void AppendBoolean(Boolean value) { Buffer.Append(value); }
        /// <summary> Char 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Char 对象</param>
        protected virtual void AppendChar(Char value)
        {
            Buffer.Append(Quot);
            switch (value)
            {
                case '\\':
                case '\n':
                case '\r':
                case '\t':
                case '"':
                    Buffer.Append('\\');
                    break;
            }
            Buffer.Append(value);
            Buffer.Append(Quot);
        }

        /// <summary> String 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Char 对象</param>
        protected virtual void AppendString(String value)
        {
            Buffer.Append(Quot);
            //Buffer.Append(value);
            //Buffer.Append(Quot);
            //return;
            unsafe
            {
                var length = value.Length;
                fixed (char* fp = value)
                {
                    char* p = fp;
                    char* end = fp + length;
                    char* flag = fp;
                    while (p <= end)
                    {
                        char c = *p;
                        switch (c)
                        {
                            case '\\':
                            case '"':
                                Buffer.Append(flag, 0, (int)(p - flag));
                                Buffer.Append('\\');
                                flag = p;
                                break;
                            case '\n':
                                Buffer.Append(flag, 0, (int)(p - flag));
                                Buffer.Append('\\');
                                Buffer.Append('n');
                                flag = p + 1;
                                break;
                            case '\r':
                                Buffer.Append(flag, 0, (int)(p - flag));
                                Buffer.Append('\\');
                                Buffer.Append('r');
                                flag = p + 1;
                                break;
                            case '\t':
                                Buffer.Append(flag, 0, (int)(p - flag));
                                Buffer.Append('\\');
                                Buffer.Append('t');
                                flag = p + 1;
                                break;
                            case '\f':
                                Buffer.Append(flag, 0, (int)(p - flag));
                                Buffer.Append('\\');
                                Buffer.Append('f');
                                flag = p + 1;
                                break;
                            default:
                                break;
                        }
                        p++;
                    }
                    if (flag == fp)
                    {
                        Buffer.Append(fp, 0, length);
                    }
                    else
                    {
                        Buffer.Append(flag, 0, (int)(p - flag) - 1);
                    }
                }
            }

            Buffer.Append(Quot);
        }
        /// <summary> DateTime 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">DateTime 对象</param>
        protected virtual void AppendDateTime(DateTime value)
        {
            Buffer.Append(Quot);
            Buffer.Append(value);
            Buffer.Append(Quot);
        }
        /// <summary> Guid 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">Guid 对象</param>
        protected virtual void AppendGuid(Guid value)
        {
            Buffer.Append(Quot).Append(value.ToString()).Append(Quot);
        }

        /// <summary> 枚举 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="value">枚举 对象</param>
        protected virtual void AppendEnum(Enum value)
        {
            Buffer.Append(Quot).Append(value.ToString()).Append(Quot);
        }
        /// <summary> 数字 类型对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="number">数字对象</param>
        protected virtual void AppendNumber(IConvertible number)
        {
            switch (number.GetTypeCode())
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    Buffer.Append(number.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
                    break;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                    Buffer.Append(number.ToInt64(System.Globalization.NumberFormatInfo.InvariantInfo));
                    break;
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    Buffer.Append(number.ToUInt64(System.Globalization.NumberFormatInfo.InvariantInfo));
                    break;
                default:
                    break;
            }
        }
        /// <summary> 数组 对象转换Json中的数组字符串写入Buffer
        /// </summary>
        /// <param name="array">数组对象</param>
        protected virtual void AppendArray(IEnumerable array)
        {
            Buffer.Append('[');
            var ee = array.GetEnumerator();
            if (ee.MoveNext())
            {
                AppendObject(ee.Current);
                while (ee.MoveNext())
                {
                    Buffer.Append(Comma);
                    AppendObject(ee.Current);
                }
            }
            Buffer.Append(']');
        }
        /// <summary> 键值对 对象转换Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="dict">键值对 对象</param>
        protected virtual void AppendJson(IDictionary dict)
        {
            AppendJson(dict.Keys, dict.Values);
        }
        /// <summary> 键枚举 和 值枚举 转换Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="keys">键枚举</param>
        /// <param name="values">值枚举</param>
        protected virtual void AppendJson(IEnumerable keys, IEnumerable values)
        {
            Buffer.Append('{');
            var ke = keys.GetEnumerator();
            var ve = values.GetEnumerator();
            if (ke.MoveNext() && ve.MoveNext())
            {
                AppendKey(ke.Current + "", true);
                AppendObject(ve.Current);
                while (ke.MoveNext() && ve.MoveNext())
                {
                    Buffer.Append(Comma);
                    AppendKey(ke.Current + "", true);
                    AppendObject(ve.Current);
                }
            }
            Buffer.Append('}');
        }
        /// <summary> 将 对象枚举 和 值转换委托 转换Json中的数组字符串写入Buffer
        /// </summary>
        /// <param name="enumer">对象枚举</param>
        /// <param name="getVal">值转换委托</param>
        protected virtual void AppendArray(IEnumerable enumer, Converter<object, object> getVal)
        {
            Buffer.Append('[');
            var ee = enumer.GetEnumerator();
            if (ee.MoveNext())
            {
                AppendObject(getVal(ee.Current));
                while (ee.MoveNext())
                {
                    Buffer.Append(Comma);
                    AppendObject(getVal(ee.Current));
                }
            }
            Buffer.Append(']');
        }
        /// <summary> 将 对象枚举 和 键/值转换委托 转换Json中的键值对象字符串写入Buffer
        /// </summary>
        /// <param name="enumer">对象枚举</param>
        /// <param name="getKey">键转换委托</param>
        /// <param name="getVal">值转换委托</param>
        /// <param name="escapekey">是否需要对Key进行转义</param>
        protected virtual void AppendJson(IEnumerable enumer, Converter<object, string> getKey, Converter<object, object> getVal, bool escapekey)
        {
            Buffer.Append('{');

            var ee = enumer.GetEnumerator();
            if (ee.MoveNext())
            {
                AppendKey(getKey(ee.Current), escapekey);
                AppendObject(getVal(ee.Current));
                while (ee.MoveNext())
                {
                    Buffer.Append(Comma);
                    AppendKey(getKey(ee.Current), true);
                    AppendObject(getVal(ee.Current));
                }
            }
            Buffer.Append('}');
        }
        /// <summary> DataSet 对象转换Json字符串写入Buffer
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
                    Buffer.Append(Comma);
                    table = (DataTable)ee.Current;
                    AppendKey(table.TableName, true);
                    AppendDataTable(table);
                }
            }
            Buffer.Append('}');
        }
        /// <summary> DataTable 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="table">DataTable 对象</param>
        protected virtual void AppendDataTable(DataTable table)
        {
            Buffer.Append("{\"columns\":");
            AppendArray(table.Columns, o => ((DataColumn)o).ColumnName);
            Buffer.Append(",\"rows\":");
            AppendArray(table.Rows, o => ((DataRow)o).ItemArray);
            Buffer.Append('}');
        }
        /// <summary> DataView 对象转换Json字符串写入Buffer
        /// </summary>
        /// <param name="tableView">DataView 对象</param>
        protected virtual void AppendDataView(DataView tableView)
        {
            Buffer.Append("{\"columns\":");
            AppendArray(tableView.Table.Columns, o => ((DataColumn)o).ColumnName);
            Buffer.Append(",\"rows\":");
            AppendArray(tableView, o => ((DataRowView)o).Row.ItemArray);
            Buffer.Append('}');
        }
    }
}
