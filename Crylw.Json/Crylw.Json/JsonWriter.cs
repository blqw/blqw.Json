using Crylw.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
namespace Crylw.Json
{
	public class JsonWriter
	{
		private int _Depth;
		private IList _Loop;
		private int _Indent;
		private bool _IgnoreNull;
		private bool _IsNeedComma;
		private bool _IsNeedFormat;
		private bool _FormatOutput;
		private bool _EnumToNumber;
		private bool _NameAllowNull;
		private bool _BooleanToNumber;
		private bool _DateTimeToNumber;
		private bool _CheckLoopReference;
		internal SBuffer sb;
		internal bool _SerializableField;
		internal JsonWriter(SBuffer buffer, JsonWriterSettings settings)
		{
			this.sb = buffer;
			this._IgnoreNull = ((settings & JsonWriterSettings.IgnoreNull) > (JsonWriterSettings)0);
			this._FormatOutput = ((settings & JsonWriterSettings.FormatOutput) > (JsonWriterSettings)0);
			this._EnumToNumber = ((settings & JsonWriterSettings.EnumToNumber) > (JsonWriterSettings)0);
			this._NameAllowNull = ((settings & JsonWriterSettings.NameAllowNull) > (JsonWriterSettings)0);
			this._BooleanToNumber = ((settings & JsonWriterSettings.BooleanToNumber) > (JsonWriterSettings)0);
			this._DateTimeToNumber = ((settings & JsonWriterSettings.DateTimeToNumber) > (JsonWriterSettings)0);
			this._SerializableField = ((settings & JsonWriterSettings.SerializableField) > (JsonWriterSettings)0);
			if ((settings & JsonWriterSettings.CheckLoopReference) != (JsonWriterSettings)0)
			{
				this._CheckLoopReference = true;
				this._Loop = new ArrayList();
			}
		}
		internal SBuffer PreName()
		{
			if (this._IsNeedComma)
			{
				this.sb.Append(',');
				this._IsNeedComma = false;
			}
			if (this._FormatOutput && this._IsNeedFormat)
			{
				this._IsNeedFormat = false;
				this.sb.AppendLine().AppendTab(this._Indent);
			}
			return this.sb;
		}
		internal SBuffer PreValue()
		{
			if (this._IsNeedComma)
			{
				this.sb.Append(',');
			}
			else
			{
				this._IsNeedComma = true;
			}
			if (this._FormatOutput)
			{
				if (this._IsNeedFormat)
				{
					this.sb.AppendLine().AppendTab(this._Indent);
				}
				else
				{
					this._IsNeedFormat = true;
				}
			}
			return this.sb;
		}
		public JsonWriter ArrayEnd()
		{
			if (this._FormatOutput)
			{
				this._Indent--;
				this.sb.AppendLine().AppendTab(this._Indent);
			}
			this.sb.Append(']');
			this._IsNeedComma = true;
			return this;
		}
		public JsonWriter ArrayStart()
		{
			if (this._IsNeedComma)
			{
				this.sb.Append(',');
				this._IsNeedComma = false;
			}
			if (this._FormatOutput)
			{
				if (this._IsNeedFormat)
				{
					this.sb.AppendLine().AppendTab(this._Indent);
				}
				else
				{
					this._IsNeedFormat = true;
				}
				this._Indent++;
			}
			this.sb.Append('[');
			return this;
		}
		public JsonWriter ObjectEnd()
		{
			if (this._FormatOutput)
			{
				this._Indent--;
				this.sb.AppendLine().AppendTab(this._Indent);
			}
			this.sb.Append('}');
			this._IsNeedComma = true;
			return this;
		}
		public JsonWriter ObjectStart()
		{
			if (this._IsNeedComma)
			{
				this.sb.Append(',');
				this._IsNeedComma = false;
			}
			if (this._FormatOutput)
			{
				if (this._IsNeedFormat)
				{
					this.sb.AppendLine().AppendTab(this._Indent);
				}
				else
				{
					this._IsNeedFormat = true;
				}
				this._Indent++;
			}
			this.sb.Append('{');
			return this;
		}
		private void AppendInteger(IConvertible value)
		{
			switch (value.GetTypeCode())
			{
			case TypeCode.SByte:
			case TypeCode.Int16:
			case TypeCode.Int32:
				this.sb.Append(value.ToInt32(null));
				return;
			case TypeCode.Byte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
				this.sb.Append(value.ToUInt32(null));
				return;
			case TypeCode.Int64:
				this.sb.Append(value.ToInt64(null));
				return;
			case TypeCode.UInt64:
				this.sb.Append(value.ToUInt64(null));
				return;
			default:
				return;
			}
		}
		public JsonWriter Name()
		{
			if (!this._NameAllowNull)
			{
				Throw.Error("JsonName 不能为 null");
			}
			this.PreName().Append('"', '"', ':');
			return this;
		}
		public JsonWriter Name(int name)
		{
			this.PreName().Append('"').Append(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(uint name)
		{
			this.PreName().Append('"').Append(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(long name)
		{
			this.PreName().Append('"').Append(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(ulong name)
		{
			this.PreName().Append('"').Append(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(char name)
		{
			this.PreName().Append('"').AppendJsonEncode(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(string name)
		{
			if (name == null)
			{
				return this.Name();
			}
			this.PreName().Append('"').AppendJsonEncode(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(object name)
		{
			if (name == null)
			{
				return this.Name();
			}
			IConvertible convertible = name as IConvertible;
			if (convertible != null)
			{
				if (name is Enum)
				{
					this.PreName().Append('"');
					this.AppendInteger(convertible);
					this.sb.Append2(CharEx.PNameEnd);
					return this;
				}
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Empty:
				case TypeCode.DBNull:
					return this.Name();
				case TypeCode.Boolean:
					return this.Name(convertible.ToBoolean(null), false);
				case TypeCode.Char:
					return this.Name(convertible.ToChar(null));
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
					return this.Name(convertible.ToInt32(null));
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
					return this.Name(convertible.ToUInt32(null));
				case TypeCode.Int64:
					return this.Name(convertible.ToInt64(null));
				case TypeCode.UInt64:
					return this.Name(convertible.ToUInt64(null));
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return this.Name(convertible.ToString(NumberFormatInfo.InvariantInfo));
				case TypeCode.DateTime:
					return this.Name(convertible.ToDateTime(null), false);
				case TypeCode.String:
					return this.Name(name.ToString());
				}
			}
			if (name is Guid)
			{
				return this.Name((Guid)name, null);
			}
			return this.Name(name.ToString());
		}
		public JsonWriter RawName(char name)
		{
			this.PreName().Append('"').Append(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter RawName(string name)
		{
			if (name == null)
			{
				return this.Name();
			}
			this.PreName().Append('"').Append(name).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(bool name, bool number = false)
		{
			this.PreName().Append('"');
			if (number)
			{
				if (name)
				{
					this.sb.Append('1');
				}
				else
				{
					this.sb.Append('0');
				}
			}
			else if (name)
			{
				this.sb.Append4(CharEx.PTrue);
			}
			else
			{
				this.sb.Append5(CharEx.PTrue);
			}
			this.sb.Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(Enum name, bool number = false)
		{
			this.PreName().Append('"');
			if (number)
			{
				this.AppendInteger(name);
			}
			else
			{
				this.sb.Append(name.ToString());
			}
			this.sb.Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(Guid name, string format = null)
		{
			this.PreName().Append('"').Append(name.ToString(format)).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(DateTime name, bool number = false)
		{
			this.PreName().Append('"');
			if (number)
			{
				this.sb.Append(name.getTime());
			}
			else
			{
				this.sb.Append(name, true, 32, 0);
			}
			this.sb.Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Name(DateTime name, bool date, int time, int ms)
		{
			this.PreName().Append('"').Append(name, date, time, ms).Append2(CharEx.PNameEnd);
			return this;
		}
		public JsonWriter Value()
		{
			this.PreValue().Append4(CharEx.PNull);
			return this;
		}
		public JsonWriter Value(int value)
		{
			this.PreValue().Append(value);
			return this;
		}
		public JsonWriter Value(uint value)
		{
			this.PreValue().Append(value);
			return this;
		}
		public JsonWriter Value(byte value)
		{
			this.PreValue().Append((uint)value);
			return this;
		}
		public JsonWriter Value(long value)
		{
			this.PreValue().Append(value);
			return this;
		}
		public JsonWriter Value(ulong value)
		{
			this.PreValue().Append(value);
			return this;
		}
		public JsonWriter Value(ushort value)
		{
			this.PreValue().Append((uint)value);
			return this;
		}
		public JsonWriter Value(bool value)
		{
			this.PreValue();
			if (this._BooleanToNumber)
			{
				if (value)
				{
					this.sb.Append('1');
				}
				else
				{
					this.sb.Append('0');
				}
			}
			else if (value)
			{
				this.sb.Append4(CharEx.PTrue);
			}
			else
			{
				this.sb.Append5(CharEx.PFalse);
			}
			return this;
		}
		public JsonWriter Value(char value)
		{
			this.PreValue().Append('"').AppendJsonEncode(value).Append('"');
			return this;
		}
		public JsonWriter Value(char[] value)
		{
			this.PreValue().Append('"').AppendJsonEncode(value).Append('"');
			return this;
		}
		public JsonWriter Value(string value)
		{
			this.PreValue().Append('"').AppendJsonEncode(value).Append('"');
			return this;
		}
		public JsonWriter Value(float value)
		{
			this.PreValue().Append(value.ToString(NumberFormatInfo.InvariantInfo));
			return this;
		}
		public JsonWriter Value(double value)
		{
			this.PreValue().Append(value.ToString(NumberFormatInfo.InvariantInfo));
			return this;
		}
		public JsonWriter Value(decimal value)
		{
			this.PreValue().Append(value.ToString(NumberFormatInfo.InvariantInfo));
			return this;
		}
		public JsonWriter Value(Enum value)
		{
			this.PreValue();
			if (this._EnumToNumber)
			{
				this.AppendInteger(value);
			}
			else
			{
				this.sb.Append('"').Append(value.ToString()).Append('"');
			}
			return this;
		}
		public JsonWriter Value(Guid value)
		{
			this.PreValue().Append('"').Append(value.ToString()).Append('"');
			return this;
		}
		public JsonWriter Value(byte[] value)
		{
			if (value == null)
			{
				return this.Value();
			}
			this.PreValue().Append('"');
			this.sb.Append(Convert.ToBase64String(value, 0, value.Length, Base64FormattingOptions.None));
			this.sb.Append('"');
			return this;
		}
		public JsonWriter Value(IToJson value)
		{
			value.WriteTo(this);
			return this;
		}
		public JsonWriter Value(DateTime value)
		{
			this.PreValue();
			if (this._DateTimeToNumber)
			{
				this.sb.Append(value.getTime());
			}
			else
			{
				this.sb.Append('"').Append(value, true, 32, 0).Append('"');
			}
			return this;
		}
		public JsonWriter ValueToNumber(bool value)
		{
			this.PreValue().Append(value ? '1' : '0');
			return this;
		}
		public JsonWriter ValueToNumber(Enum value)
		{
			this.PreValue();
			this.AppendInteger(value);
			return this;
		}
		public JsonWriter ValueToNumber(DateTime value)
		{
			this.PreValue().Append(value.getTime());
			return this;
		}
		public JsonWriter RawJson(string value)
		{
			this.PreValue().Append(value);
			return this;
		}
		public JsonWriter RawJson(char[] value, int start, int count)
		{
			this.PreValue().Append(value, start, count);
			return this;
		}
		public JsonWriter RawJson(string value, int start, int count)
		{
			this.PreValue().Append(value, start, count);
			return this;
		}
		public JsonWriter RawValue(char value)
		{
			this.PreValue().Append('"').Append(value).Append('"');
			return this;
		}
		public JsonWriter RawValue(string value)
		{
			this.PreValue().Append('"').Append(value).Append('"');
			return this;
		}
		public JsonWriter RawValue(char[] value, int start, int count)
		{
			this.PreValue().Append('"').Append(value, start, count).Append('"');
			return this;
		}
		public JsonWriter RawValue(string value, int start, int count)
		{
			this.PreValue().Append('"').Append(value, start, count).Append('"');
			return this;
		}
		public JsonWriter Value(object value)
		{
			if (value == null)
			{
				return this.Value();
			}
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				if (!this._EnumToNumber && value is Enum)
				{
					this.PreValue().Append('"').Append(value.ToString()).Append('"');
					return this;
				}
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Empty:
				case TypeCode.DBNull:
					return this.Value();
				case TypeCode.Boolean:
					return this.Value(convertible.ToBoolean(null));
				case TypeCode.Char:
					return this.Value(convertible.ToChar(null));
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
					this.PreValue().Append(convertible.ToInt32(null));
					return this;
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
					this.PreValue().Append(convertible.ToUInt32(null));
					return this;
				case TypeCode.Int64:
					this.PreValue().Append(convertible.ToInt64(null));
					return this;
				case TypeCode.UInt64:
					this.PreValue().Append(convertible.ToUInt64(null));
					return this;
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					this.PreValue().Append(convertible.ToString(NumberFormatInfo.InvariantInfo));
					return this;
				case TypeCode.DateTime:
					return this.Value(convertible.ToDateTime(null));
				case TypeCode.String:
					return this.Value(value.ToString());
				}
			}
			IToJson toJson = value as IToJson;
			if (toJson != null)
			{
				toJson.WriteTo(this);
				return this;
			}
			JsonWriterEx ex = JsonWriterEx.GetEx(value.GetType());
			if (ex.IsSimpleType)
			{
				ex.Invoke(this, value);
				return this;
			}
			if (!this._CheckLoopReference)
			{
				if (this._Depth > 32)
				{
					Throw.NotSupported("对象过于复杂或存在循环引用", false);
				}
				this._Depth++;
				if (ex.IsCustomType)
				{
					ex.WriteTo(this, value);
				}
				else
				{
					ex.Invoke(this, value);
				}
				this._Depth--;
				return this;
			}
			if (this._Loop.Contains(value))
			{
				this.PreValue().Append9(CharEx.PUndefined);
			}
			else
			{
				int index = this._Loop.Add(value);
				if (ex.IsCustomType)
				{
					ex.WriteTo(this, value);
				}
				else
				{
					ex.Invoke(this, value);
				}
				this._Loop.RemoveAt(index);
			}
			return this;
		}
		public JsonWriter Value(IFormattable value, string format, IFormatProvider provider = null)
		{
			if (value is DateTime)
			{
				if (format == null || format.Length == 0 || format.Equals("yyyy-MM-dd HH:mm:ss", StringComparison.Ordinal))
				{
					this.PreValue().Append('"').Append((DateTime)value, true, 32, 0).Append('"');
					return this;
				}
				if (format.Equals("number", StringComparison.Ordinal))
				{
					this.PreValue().Append(((DateTime)value).getTime());
					return this;
				}
				if (format.Equals("date", StringComparison.Ordinal))
				{
					this.PreValue().Append9(CharEx.PDate);
					this.sb.Append(((DateTime)value).getTime()).Append(")");
					return this;
				}
				if (format.Equals("yyyy-MM-dd", StringComparison.Ordinal))
				{
					this.PreValue().Append('"').Append((DateTime)value, true, 0, 0).Append('"');
					return this;
				}
				if (format.Equals("HH:mm:ss", StringComparison.Ordinal))
				{
					this.PreValue().Append('"').Append((DateTime)value, false, 1, 0).Append('"');
					return this;
				}
				if (format.Equals("yyyy-MM-ddTHH:mm:ss.fffZ", StringComparison.Ordinal))
				{
					this.PreValue().Append('"').Append((DateTime)value, true, 84, 90).Append('"');
					return this;
				}
				if (format.Equals("yyyy-MM-dd HH:mm:ss.fff", StringComparison.Ordinal))
				{
					this.PreValue().Append('"').Append((DateTime)value, true, 32, 3).Append('"');
					return this;
				}
				if (format.Equals("HH:mm:ss.fff", StringComparison.Ordinal))
				{
					this.PreValue().Append('"').Append((DateTime)value, false, 1, 3).Append('"');
					return this;
				}
				if (format.Equals("fff", StringComparison.Ordinal))
				{
					this.PreValue().Append('"').Append((DateTime)value, false, 0, 3).Append('"');
					return this;
				}
			}
			return this.Value(value.ToString(format, provider));
		}
		public JsonWriter Concat(int[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				this.PreValue().Append(value[i]);
			}
			return this;
		}
		public JsonWriter Concat(char[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				this.PreValue().Append('"').AppendJsonEncode(value[i]).Append('"');
			}
			return this;
		}
		public JsonWriter Concat(string[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				this.PreValue().Append('"').AppendJsonEncode(value[i]).Append('"');
			}
			return this;
		}
		public JsonWriter Concat(DateTime[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				this.PreValue();
				if (this._DateTimeToNumber)
				{
					this.sb.Append(value[i].getTime());
				}
				else
				{
					this.sb.Append('"').Append(value[i], true, 32, 0).Append('"');
				}
			}
			return this;
		}
		public JsonWriter Concat(IEnumerable value)
		{
			foreach (object current in value)
			{
				this.Value(current);
			}
			return this;
		}
		public JsonWriter Concat(IEnumerable<int> value)
		{
			foreach (int current in value)
			{
				this.PreValue().Append(current);
			}
			return this;
		}
		public JsonWriter Concat(IEnumerable<string> value)
		{
			foreach (string current in value)
			{
				this.PreValue().Append('"').AppendJsonEncode(current).Append('"');
			}
			return this;
		}
		public JsonWriter Concat(IEnumerable<DateTime> value)
		{
			foreach (DateTime current in value)
			{
				this.PreValue();
				if (this._DateTimeToNumber)
				{
					this.sb.Append(current.getTime());
				}
				else
				{
					this.sb.Append('"').Append(current, true, 32, 0).Append('"');
				}
			}
			return this;
		}
		public JsonWriter Concat(IEnumerator value)
		{
			while (value.MoveNext())
			{
				this.Value(value.Current);
			}
			return this;
		}
		public JsonWriter Concat(IEnumerator<int> value)
		{
			while (value.MoveNext())
			{
				this.PreValue().Append(value.Current);
			}
			return this;
		}
		public JsonWriter Concat(IEnumerator<string> value)
		{
			while (value.MoveNext())
			{
				this.PreValue().Append('"').AppendJsonEncode(value.Current).Append('"');
			}
			return this;
		}
		public JsonWriter Concat(IEnumerator<DateTime> value)
		{
			while (value.MoveNext())
			{
				this.PreValue();
				if (this._DateTimeToNumber)
				{
					this.sb.Append(value.Current.getTime());
				}
				else
				{
					this.sb.Append('"').Append(value.Current, true, 32, 0).Append('"');
				}
			}
			return this;
		}
		public JsonWriter Extend(IDictionary value)
		{
			foreach (DictionaryEntry dictionaryEntry in value)
			{
				this.Name(dictionaryEntry.Key).Value(dictionaryEntry.Value);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerable<KeyValuePair<string, int>> value)
		{
			foreach (KeyValuePair<string, int> current in value)
			{
				this.Name(current.Key);
				this.PreValue().Append(current.Value);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerable<KeyValuePair<string, bool>> value)
		{
			foreach (KeyValuePair<string, bool> current in value)
			{
				this.Name(current.Key).PreValue();
				if (this._BooleanToNumber)
				{
					if (current.Value)
					{
						this.sb.Append('1');
					}
					else
					{
						this.sb.Append('0');
					}
				}
				else if (current.Value)
				{
					this.sb.Append4(CharEx.PTrue);
				}
				else
				{
					this.sb.Append5(CharEx.PFalse);
				}
			}
			return this;
		}
		public JsonWriter Extend(IEnumerable<KeyValuePair<string, string>> value)
		{
			foreach (KeyValuePair<string, string> current in value)
			{
				this.Name(current.Key).Value(current.Value);
			}
			return this;
		}
		public JsonWriter Extend(IDictionaryEnumerator value)
		{
			while (value.MoveNext())
			{
				this.Name(value.Key).Value(value.Value);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<KeyValuePair<string, int>> value)
		{
			while (value.MoveNext())
			{
				KeyValuePair<string, int> current = value.Current;
				this.Name(current.Key);
				this.PreValue().Append(current.Value);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<KeyValuePair<string, bool>> value)
		{
			while (value.MoveNext())
			{
				KeyValuePair<string, bool> current = value.Current;
				this.Name(current.Key).PreValue();
				if (this._BooleanToNumber)
				{
					if (current.Value)
					{
						this.sb.Append('1');
					}
					else
					{
						this.sb.Append('0');
					}
				}
				else if (current.Value)
				{
					this.sb.Append4(CharEx.PTrue);
				}
				else
				{
					this.sb.Append5(CharEx.PFalse);
				}
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<KeyValuePair<string, string>> value)
		{
			while (value.MoveNext())
			{
				KeyValuePair<string, string> current = value.Current;
				this.Name(current.Key).Value(current.Value);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator keys, IEnumerator values)
		{
			while (keys.MoveNext() && values.MoveNext())
			{
				this.Name(keys.Current).Value(values.Current);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<int> keys, IEnumerator values)
		{
			while (keys.MoveNext() && values.MoveNext())
			{
				this.PreName().Append('"').Append(keys.Current).Append2(CharEx.PNameEnd);
				this.Value(values.Current);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<Guid> keys, IEnumerator values)
		{
			while (keys.MoveNext() && values.MoveNext())
			{
				SBuffer arg_23_0 = this.PreName().Append('"');
				Guid current = keys.Current;
				arg_23_0.Append(current.ToString()).Append2(CharEx.PNameEnd);
				this.Value(values.Current);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<string> keys, IEnumerator values)
		{
			while (keys.MoveNext() && values.MoveNext())
			{
				this.Name(keys.Current).Value(values.Current);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<int> keys, IEnumerator<string> values)
		{
			while (keys.MoveNext() && values.MoveNext())
			{
				this.PreName().Append('"').Append(keys.Current).Append2(CharEx.PNameEnd);
				this.PreValue().Append('"').AppendJsonEncode(values.Current).Append('"');
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<string> keys, IEnumerator<int> values)
		{
			while (keys.MoveNext() && values.MoveNext())
			{
				this.Name(keys.Current);
				this.PreValue().Append(values.Current);
			}
			return this;
		}
		public JsonWriter Extend(IEnumerator<string> keys, IEnumerator<string> values)
		{
			while (keys.MoveNext() && values.MoveNext())
			{
				this.Name(keys.Current);
				this.PreValue().Append('"').AppendJsonEncode(values.Current).Append('"');
			}
			return this;
		}
		public JsonWriter Concat(DataView value)
		{
			IEnumerator enumerator = value.GetEnumerator();
			if (enumerator.MoveNext())
			{
				DataColumnCollection columns = value.Table.Columns;
				string[] array = new string[columns.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = columns[i].ColumnName;
				}
				Json.Encode(array);
				DataRowView dataRowView = enumerator.Current as DataRowView;
				this.ObjectStart();
				for (int j = 0; j < array.Length; j++)
				{
					this.RawName(array[j]).Value(dataRowView[j]);
				}
				this.ObjectEnd();
				while (enumerator.MoveNext())
				{
					dataRowView = (enumerator.Current as DataRowView);
					this.ObjectStart();
					for (int k = 0; k < array.Length; k++)
					{
						this.RawName(array[k]).Value(dataRowView[k]);
					}
					this.ObjectEnd();
				}
			}
			return this;
		}
		public JsonWriter Concat(DataTable value)
		{
			IEnumerator enumerator = value.Rows.GetEnumerator();
			if (enumerator.MoveNext())
			{
				DataColumnCollection columns = value.Columns;
				string[] array = new string[columns.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = columns[i].ColumnName;
				}
				Json.Encode(array);
				DataRow dataRow = enumerator.Current as DataRow;
				this.ObjectStart();
				for (int j = 0; j < array.Length; j++)
				{
					this.RawName(array[j]).Value(dataRow[j]);
				}
				this.ObjectEnd();
				while (enumerator.MoveNext())
				{
					dataRow = (enumerator.Current as DataRow);
					this.ObjectStart();
					for (int k = 0; k < array.Length; k++)
					{
						this.RawName(array[k]).Value(dataRow[k]);
					}
					this.ObjectEnd();
				}
			}
			return this;
		}
		public JsonWriter Concat(IDataReader value)
		{
			if (value.IsClosed)
			{
				return this;
			}
			if (value.Read())
			{
				int fieldCount = value.FieldCount;
				string[] array = new string[fieldCount];
				object[] array2 = new object[fieldCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = value.GetName(i);
				}
				Json.Encode(array);
				value.GetValues(array2);
				this.ObjectStart();
				for (int j = 0; j < fieldCount; j++)
				{
					this.RawName(array[j]).Value(array2[j]);
				}
				this.ObjectEnd();
				while (value.Read())
				{
					value.GetValues(array2);
					this.ObjectStart();
					for (int k = 0; k < fieldCount; k++)
					{
						this.RawName(array[k]).Value(array2[k]);
					}
					this.ObjectEnd();
				}
			}
			if (!value.IsClosed)
			{
				value.Close();
			}
			return this;
		}
		public JsonWriter Extend(DataSet value)
		{
			foreach (DataTable dataTable in value.Tables)
			{
				this.Name(dataTable.TableName).ArrayStart().Concat(dataTable).ArrayEnd();
			}
			return this;
		}
		public JsonWriter Extend(DataRow value)
		{
			DataColumnCollection columns = value.Table.Columns;
			int count = columns.Count;
			for (int i = 0; i < count; i++)
			{
				this.Name(columns[i].ColumnName).Value(value[i]);
			}
			return this;
		}
		public JsonWriter Extend(DataRowView value)
		{
			DataColumnCollection columns = value.DataView.Table.Columns;
			int count = columns.Count;
			for (int i = 0; i < count; i++)
			{
				this.Name(columns[i].ColumnName).Value(value[i]);
			}
			return this;
		}
		public JsonWriter Extend(NameValueCollection value)
		{
			string[] allKeys = value.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				this.Name(allKeys[i]).Value(value[i]);
			}
			return this;
		}
	}
}
