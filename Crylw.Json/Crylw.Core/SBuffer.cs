using System;
using System.IO;
using System.Runtime.CompilerServices;
namespace Crylw.Core
{
	internal class SBuffer
	{
		private int pos;
		private char[] buffer;
		private const int Capacity = 1004;
		private unsafe readonly char* cb;
		private TextWriter tw;
		private Standby standby;
		private const int MaxCapacity = 1024;
		private unsafe readonly char* PNumber;
		[ThreadStatic]
		private static char[] Cached;
		public unsafe SBuffer(char[] array, char* pointer)
		{
			this.cb = pointer;
			this.buffer = array;
			this.PNumber = this.cb + 1004;
		}
		public unsafe SBuffer(char[] array, char* pointer, TextWriter writer)
		{
			this.tw = writer;
			this.cb = pointer;
			this.buffer = array;
			this.PNumber = this.cb + 1004;
		}
		public static void Release(char[] array)
		{
			if (array.Length <= 1024)
			{
				SBuffer.Cached = array;
			}
		}
		public static char[] Acquire(int capacity = 1024)
		{
			if (capacity <= 1024)
			{
				char[] cached = SBuffer.Cached;
				if (cached != null)
				{
					SBuffer.Cached = null;
					return cached;
				}
			}
			return new char[capacity];
		}
		public unsafe static void wstrcpy(int* src, int* dest, int count)
		{
			while (count >= 8)
			{
				*dest = *src;
				dest[1] = *(src + 1);
				dest[2] = *(src + 2);
				dest[3] = *(src + 3);
				src += 4;
				dest += 4;
				count -= 8;
			}
			if ((count & 4) != 0)
			{
				*dest = *src;
				dest[1] = *(src + 1);
				src += 2;
				dest += 2;
			}
			if ((count & 2) != 0)
			{
				*dest = *src;
				src++;
				dest++;
			}
			if ((count & 1) != 0)
			{
				*(short*)dest = (short)(*(ushort*)src);
			}
		}
		public unsafe void Append2(int* value)
		{
			if (this.pos > 1022)
			{
				this.Flush();
			}
			*(int*)(this.cb + this.pos) = *value;
			this.pos += 2;
		}
		public unsafe void Append4(long* value)
		{
			if (this.pos > 1020)
			{
				this.Flush();
			}
			*(long*)(this.cb + this.pos) = *value;
			this.pos += 4;
		}
		public unsafe void Append5(long* value)
		{
			if (this.pos > 1019)
			{
				this.Flush();
			}
			*(long*)(this.cb + this.pos) = *value;
			(this.cb + this.pos)[4] = (char)(*(ushort*)(value + 1));
			this.pos += 5;
		}
		public unsafe void Append9(long* value)
		{
			if (this.pos > 1019)
			{
				this.Flush();
			}
			*(long*)(this.cb + this.pos) = *value;
			*(long*)(this.cb + this.pos + 4) = value[1];
			(this.cb + this.pos)[8] = (char)(*(ushort*)(value + 2));
			this.pos += 9;
		}
		public void Release()
		{
			SBuffer.Release(this.buffer);
			this.buffer = null;
		}
		public void FlushAndRelease()
		{
			this.tw.Write(this.buffer, 0, this.pos);
			this.pos = 0;
			SBuffer.Release(this.buffer);
			this.buffer = null;
			this.tw = null;
		}
		public override string ToString()
		{
			if (this.standby == null)
			{
				return new string(this.cb, 0, this.pos);
			}
			if (this.pos > 0)
			{
				this.standby.Write(new string(this.cb, 0, this.pos));
				this.pos = 0;
			}
			return this.standby.ToString();
		}
		public string GetStringAndClear()
		{
			string arg_21_0 = this.ToString();
			this.pos = 0;
			if (this.standby != null)
			{
				this.standby.Count = 0;
			}
			return arg_21_0;
		}
		public string GetStringAndRelease()
		{
			string arg_1F_0 = this.ToString();
			SBuffer.Release(this.buffer);
			this.buffer = null;
			this.standby = null;
			return arg_1F_0;
		}
		private void Flush()
		{
			if (this.pos > 0)
			{
				if (this.tw != null)
				{
					this.tw.Write(this.buffer, 0, this.pos);
				}
				else
				{
					if (this.standby == null)
					{
						this.standby = new Standby();
					}
					this.standby.Write(new string(this.cb, 0, this.pos));
				}
				this.pos = 0;
			}
		}
		private void Flush(string value)
		{
			if (this.tw != null)
			{
				if (this.pos > 0)
				{
					this.tw.Write(this.buffer, 0, this.pos);
					this.pos = 0;
				}
				this.tw.Write(value);
				return;
			}
			if (this.standby == null)
			{
				this.standby = new Standby();
			}
			if (this.pos > 0)
			{
				this.standby.Write(new string(this.cb, 0, this.pos));
				this.pos = 0;
			}
			this.standby.Write(value);
		}
		private bool TryWrite(int count)
		{
			if (count > 1024)
			{
				return false;
			}
			if (this.pos > 1024 - count)
			{
				this.Flush();
			}
			return true;
		}
		private void Flush(char[] value, int start, int count)
		{
			if (this.tw != null)
			{
				if (this.pos > 0)
				{
					this.tw.Write(this.buffer, 0, this.pos);
					this.pos = 0;
				}
				this.tw.Write(value, start, count);
				return;
			}
			if (this.standby == null)
			{
				this.standby = new Standby();
			}
			if (this.pos > 0)
			{
				this.standby.Write(new string(this.cb, 0, this.pos));
				this.pos = 0;
			}
			this.standby.Write(new string(value, start, count));
		}
		public unsafe SBuffer Append(int value)
		{
			if (value <= 0)
			{
				if (this.pos > 1023)
				{
					this.Flush();
				}
				if (value == 0)
				{
					this.cb[this.pos] = '0';
					this.pos++;
					return this;
				}
				value = 0 - value;
				this.cb[this.pos] = '-';
				this.pos++;
			}
			if (value < 10)
			{
				if (this.pos > 1023)
				{
					this.Flush();
				}
				this.cb[this.pos] = *(CharEx.PDigit + value);
				this.pos++;
				return this;
			}
			int num = 19;
			this.PNumber[num] = *(CharEx.PDigit + value % 10);
			while ((value /= 10) != 0)
			{
				this.PNumber[(IntPtr)(--num) * 2] = *(CharEx.PDigit + value % 10);
			}
			value = 20 - num;
			if (this.pos > 1003)
			{
				this.Flush();
			}
			SBuffer.wstrcpy((int*)(this.PNumber + num), (int*)(this.cb + this.pos), value);
			this.pos += value;
			return this;
		}
		public unsafe SBuffer Append(long value)
		{
			if (value <= 0L)
			{
				if (this.pos > 1023)
				{
					this.Flush();
				}
				if (value == 0L)
				{
					this.cb[this.pos] = '0';
					this.pos++;
					return this;
				}
				value = 0L - value;
				this.cb[this.pos] = '-';
				this.pos++;
			}
			if (value < 10L)
			{
				if (this.pos > 1023)
				{
					this.Flush();
				}
				this.cb[this.pos] = (char)(value + 48L);
				this.pos++;
				return this;
			}
			int num = 19;
			this.PNumber[num] = (char)(value % 10L + 48L);
			while ((value /= 10L) != 0L)
			{
				this.PNumber[(IntPtr)(--num) * 2] = (char)(value % 10L + 48L);
			}
			int num2 = 20 - num;
			if (this.pos > 1003)
			{
				this.Flush();
			}
			SBuffer.wstrcpy((int*)(this.PNumber + num), (int*)(this.cb + this.pos), num2);
			this.pos += num2;
			return this;
		}
		public unsafe SBuffer Append(uint value)
		{
			if (value < 10u)
			{
				if (this.pos > 1023)
				{
					this.Flush();
				}
				this.cb[this.pos] = (char)(value + 48u);
				this.pos++;
				return this;
			}
			int num = 19;
			this.PNumber[num] = (char)(value % 10u + 48u);
			while ((value /= 10u) != 0u)
			{
				this.PNumber[(IntPtr)(--num) * 2] = (char)(value % 10u + 48u);
			}
			int num2 = 20 - num;
			if (this.pos > 1003)
			{
				this.Flush();
			}
			SBuffer.wstrcpy((int*)(this.PNumber + num), (int*)(this.cb + this.pos), num2);
			this.pos += num2;
			return this;
		}
		public unsafe SBuffer Append(ulong value)
		{
			if (value < 10uL)
			{
				if (this.pos > 1023)
				{
					this.Flush();
				}
				this.cb[this.pos] = (char)(value + 48uL);
				this.pos++;
				return this;
			}
			int num = 19;
			this.PNumber[num] = (char)(value % 10uL + 48uL);
			while ((value /= 10uL) != 0uL)
			{
				this.PNumber[(IntPtr)(--num) * 2] = (char)(value % 10uL + 48uL);
			}
			int num2 = 20 - num;
			if (this.pos > 1003)
			{
				this.Flush();
			}
			SBuffer.wstrcpy((int*)(this.PNumber + num), (int*)(this.cb + this.pos), num2);
			this.pos += num2;
			return this;
		}
		public unsafe SBuffer AppendLine()
		{
			if (this.pos > 1022)
			{
				this.Flush();
			}
			*(int*)(this.cb + this.pos) = *CharEx.PLine;
			this.pos += 2;
			return this;
		}
		public SBuffer AppendTab(int count = 1)
		{
			while (count-- > 0)
			{
				this.Append4(CharEx.PSpace);
			}
			return this;
		}
		public unsafe SBuffer Append(char value)
		{
			if (this.pos > 1023)
			{
				this.Flush();
			}
			this.cb[this.pos] = value;
			this.pos++;
			return this;
		}
		public unsafe SBuffer Append(char c1, char c2)
		{
			if (this.pos > 1022)
			{
				this.Flush();
			}
			this.cb[this.pos] = c1;
			this.cb[this.pos + 1] = c2;
			this.pos += 2;
			return this;
		}
		public unsafe SBuffer Append(char c1, char c2, char c3)
		{
			if (this.pos > 1021)
			{
				this.Flush();
			}
			this.cb[this.pos] = c1;
			this.cb[this.pos + 1] = c2;
			this.cb[this.pos + 2] = c3;
			this.pos += 3;
			return this;
		}
		public unsafe SBuffer Append(char[] value)
		{
			if (value != null)
			{
				int num = value.Length;
				if (num > 2)
				{
					if (this.TryWrite(num))
					{
						fixed (char* ptr = value)
						{
							SBuffer.wstrcpy((int*)ptr, (int*)(this.cb + this.pos), num);
						}
						this.pos += num;
					}
					else
					{
						this.Flush(value, 0, num);
					}
					return this;
				}
				if (num == 2)
				{
					if (this.pos > 1022)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[0];
					this.cb[this.pos + 1] = value[1];
					this.pos += 2;
					return this;
				}
				if (num == 1)
				{
					if (this.pos > 1023)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[0];
					this.pos++;
				}
			}
			return this;
		}
		public unsafe SBuffer Append(string value)
		{
			if (value != null)
			{
				int length = value.Length;
				if (length > 2)
				{
					if (this.TryWrite(length))
					{
						fixed (string text = value)
						{
							char* ptr = text;
							if (ptr != null)
							{
								ptr += RuntimeHelpers.OffsetToStringData / 2;
							}
							SBuffer.wstrcpy((int*)ptr, (int*)(this.cb + this.pos), length);
						}
						this.pos += length;
					}
					else
					{
						this.Flush(value);
					}
					return this;
				}
				if (length == 2)
				{
					if (this.pos > 1022)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[0];
					this.cb[this.pos + 1] = value[1];
					this.pos += 2;
					return this;
				}
				if (length == 1)
				{
					if (this.pos > 1023)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[0];
					this.pos++;
				}
			}
			return this;
		}
		public unsafe SBuffer Append(char* value, int count)
		{
			if (count > 0)
			{
				if (this.TryWrite(count))
				{
					SBuffer.wstrcpy((int*)value, (int*)(this.cb + this.pos), count);
					this.pos += count;
				}
				else
				{
					this.Flush(new string(value, 0, count));
				}
			}
			return this;
		}
		public unsafe SBuffer Append(char[] value, int start, int count)
		{
			if (value != null)
			{
				if (start < 0)
				{
					Throw.ArgumentOutOfRange("start", "ArgumentOutOfRange_GenericPositive", true);
				}
				if (start > value.Length - count)
				{
					Throw.ArgumentOutOfRange("count", "ArgumentOutOfRange_Index", true);
				}
				if (count > 2)
				{
					if (this.TryWrite(count))
					{
						fixed (char* ptr = &value[start])
						{
							SBuffer.wstrcpy((int*)ptr, (int*)(this.cb + this.pos), count);
						}
						this.pos += count;
					}
					else
					{
						this.Flush(value, start, count);
					}
					return this;
				}
				if (count == 2)
				{
					if (this.pos > 1022)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[start];
					this.cb[this.pos + 1] = value[start + 1];
					this.pos += 2;
					return this;
				}
				if (count == 1)
				{
					if (this.pos > 1023)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[start];
					this.pos++;
				}
			}
			return this;
		}
		public unsafe SBuffer Append(string value, int start, int count)
		{
			if (value != null)
			{
				if (start < 0)
				{
					Throw.ArgumentOutOfRange("start", "ArgumentOutOfRange_GenericPositive", true);
				}
				if (start > value.Length - count)
				{
					Throw.ArgumentOutOfRange("count", "ArgumentOutOfRange_Index", true);
				}
				if (count > 2)
				{
					if (this.TryWrite(count))
					{
						fixed (string text = value)
						{
							char* ptr = text;
							if (ptr != null)
							{
								ptr += RuntimeHelpers.OffsetToStringData / 2;
							}
							SBuffer.wstrcpy((int*)(ptr + start), (int*)(this.cb + this.pos), count);
						}
						this.pos += count;
					}
					else
					{
						this.Flush(value.Substring(start, count));
					}
					return this;
				}
				if (count == 2)
				{
					if (this.pos > 1022)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[start];
					this.cb[this.pos + 1] = value[start + 1];
					this.pos += 2;
					return this;
				}
				if (count == 1)
				{
					if (this.pos > 1023)
					{
						this.Flush();
					}
					this.cb[this.pos] = value[start];
					this.pos++;
				}
			}
			return this;
		}
		public unsafe SBuffer Append(DateTime value, bool date = true, int time = 32, int ms = 0)
		{
			if (this.pos > 1000)
			{
				this.Flush();
			}
			int num = this.pos;
			if (date)
			{
				int num2 = value.Year;
				if (num2 > 999)
				{
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 1000);
					num2 %= 1000;
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 100);
					num2 %= 100;
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else if (num2 > 99)
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 100);
					num2 %= 100;
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else if (num2 > 9)
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
					this.cb[(IntPtr)(num++) * 2] = '0';
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
					this.cb[(IntPtr)(num++) * 2] = '0';
					this.cb[(IntPtr)(num++) * 2] = '0';
				}
				this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2);
				this.cb[(IntPtr)(num++) * 2] = '-';
				num2 = value.Month;
				if (num2 > 9)
				{
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
				}
				this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2);
				this.cb[(IntPtr)(num++) * 2] = '-';
				num2 = value.Day;
				if (num2 > 9)
				{
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
				}
				this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2);
			}
			if (time > 0)
			{
				if (date)
				{
					this.cb[(IntPtr)(num++) * 2] = (char)time;
				}
				int num2 = value.Hour;
				if (num2 > 9)
				{
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
				}
				this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2);
				this.cb[(IntPtr)(num++) * 2] = ':';
				num2 = value.Minute;
				if (num2 > 9)
				{
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
				}
				this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2);
				this.cb[(IntPtr)(num++) * 2] = ':';
				num2 = value.Second;
				if (num2 > 9)
				{
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
				}
				this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2);
			}
			if (ms > 0)
			{
				if (time > 0)
				{
					this.cb[(IntPtr)(num++) * 2] = '.';
				}
				int num2 = value.Millisecond;
				if (num2 > 99)
				{
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 100);
					num2 %= 100;
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else if (num2 > 9)
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
					this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2 / 10);
					num2 %= 10;
				}
				else
				{
					this.cb[(IntPtr)(num++) * 2] = '0';
					this.cb[(IntPtr)(num++) * 2] = '0';
				}
				this.cb[(IntPtr)(num++) * 2] = *(CharEx.PDigit + num2);
				if (ms > 32)
				{
					this.cb[(IntPtr)(num++) * 2] = (char)ms;
				}
			}
			this.pos = num;
			return this;
		}
		public unsafe SBuffer AppendJsonEncode(char value)
		{
			if (!value.isJsonEncode())
			{
				return this.Append(value);
			}
			return this.Append('\\', CharEx.PJsonEncode[(IntPtr)value]);
		}
		public unsafe SBuffer AppendJsonEncode(char[] value)
		{
			if (value != null && value.Length != 0)
			{
				fixed (char* ptr = value)
				{
					return this.AppendJsonEncode(ptr, value.Length);
				}
			}
			return this;
		}
		public unsafe SBuffer AppendJsonEncode(string value)
		{
			if (value != null && value.Length > 0)
			{
				char* ptr = value;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				return this.AppendJsonEncode(ptr, value.Length);
			}
			return this;
		}
		public unsafe SBuffer AppendJsonEncode(char* value, int count)
		{
			if (count > 0)
			{
				int i = 0;
				int num = 0;
				int num2;
				while (i < count)
				{
					if (value[i].isJsonEncode())
					{
						num2 = i - num;
						if (num2 > 0)
						{
							this.Append(value + num, num2);
						}
						this.Append('\\').Append(CharEx.PJsonEncode[(IntPtr)value[i]]);
						num = i + 1;
					}
					i++;
				}
				num2 = i - num;
				if (num2 > 0)
				{
					this.Append(value + num, num2);
				}
			}
			return this;
		}
	}
}
