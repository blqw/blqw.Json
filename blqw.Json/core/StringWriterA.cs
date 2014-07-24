using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace blqw
{
    /// <summary> 快速字符串拼接类,精简版StringBuilder
    /// </summary>
    [DebuggerDisplay("长度:{Length} 内容: {DebugInfo}")]
    public sealed class StringWriterA : IDisposable
    {
        #region 私有字段
        /// <summary> 二级缓冲区
        /// </summary>
        private StringBuilder _buffer;
        /// <summary> 数字缓冲区
        /// </summary>
        private unsafe char* _number;
        private readonly IntPtr _numberIntPtr;
        /// <summary> 一级缓冲区
        /// </summary>
        private unsafe char* _curr;
        private readonly IntPtr _currIntPtr;
        /// <summary> 一级缓冲区大小
        /// </summary>
        private readonly int _capacity;
        /// <summary> 一级缓冲区索引
        /// </summary>
        private int _position;
        #endregion

        #region 私有方法
        /// <summary> 在调试器的变量窗口中的显示的信息
        /// </summary>
        private string DebugInfo
        {
            get
            {
                string str = ToString();
                if (str.Length > 70)
                {
                    var s = str;
                    str = s.Substring(0, 30) + "  ......  ";
                    str += s.Substring(s.Length - 30);
                }
                return str;
            }
        }
        /// <summary> 尝试在一级缓冲区写入一个字符
        /// <para>如果一级缓冲区已满,将会自动调用Flush方法转移一级缓冲区中的内容</para>
        /// </summary>
        private void TryWrite()
        {
            if (_position >= _capacity)
            {
                Flush();
            }
        }
        /// <summary> 尝试在一级缓冲区写入指定数量的字符
        /// </summary>
        /// <para>如果尝试写入的字符数大于一级缓冲区的大小,返回false</para>
        /// <para>如果尝试写入的字符数超出一级缓冲区剩余容量,自动调用Flush方法,并返回true</para>
        /// <param name="count">尝试写入的字符数</param>
        /// <returns></returns>
        private void TryWrite(int count)
        {
            if (_position + count >= _capacity)
            {
                Flush();
            }
        }

        #endregion

        /// <summary> 实例化新的对象
        /// </summary>
        public StringWriterA() : this(2048) { }
        /// <summary> 实例化新的对象,并且指定初始容量
        /// </summary>
        /// <param name="capacity">初始容量</param>
        public StringWriterA(int capacity)
        {
            if (capacity < 512)
            {
                capacity = 512;
            }
            _capacity = capacity;
            _buffer = new StringBuilder(capacity);
            //_curr = new char[capacity];
            _currIntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(capacity * 2);//char是2个字节的
            _numberIntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(20 * 2);
            unsafe
            {
                _curr = (char*)_currIntPtr.ToPointer();
                _number = (char*)_numberIntPtr.ToPointer();
            }
            _disposable = new Disposable(this);
            _disposable.DisposeManaged += () => {
                _buffer.Length = 0;
                _buffer = null;
                _position = -1;
                unsafe
                {
                    _number = null;
                    _curr = null;
                }
            };
            _disposable.DisposeUnmanaged += () => { Marshal.FreeHGlobal(_currIntPtr); };
            _disposable.DisposeUnmanaged += () => { Marshal.FreeHGlobal(_numberIntPtr); };
        }

        /// <summary> 当前对象总字符数
        /// </summary>
        public int Length
        {
            get
            {
                _disposable.Assert();
                return _buffer.Length + _position;
            }
        }

        /// <summary> 刷新缓冲区，将一级缓冲区的内容写入二级缓冲区。
        /// </summary>
        public void Flush()
        {
            _disposable.Assert();
            if (_position > 0)
            {
                unsafe
                {
                    _buffer.Append(new string(_curr, 0, _position));
                    _position = 0;
                }
            }
        }

        /// <summary> 清除所有数据
        /// </summary>
        public void Clear()
        {
            _disposable.Assert();
            _position = 0;
            _buffer.Length = 0;
        }

        #region Guid

        private static char HexToChar(int a)
        {
            a &= 15;
            return a > 9 ? (char)(a - 10 + 0x61) : (char)(a + 0x30);
        }

        #endregion

        #region Append
        /// <summary> 将 Boolean 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Boolean val)
        {
            _disposable.Assert();
            unsafe
            {
                if (val)
                {
                    TryWrite(4);
                    _curr[_position++] = 't';
                    _curr[_position++] = 'r';
                    _curr[_position++] = 'u';
                    _curr[_position++] = 'e';
                }
                else
                {
                    TryWrite(5);
                    _curr[_position++] = 'f';
                    _curr[_position++] = 'a';
                    _curr[_position++] = 'l';
                    _curr[_position++] = 's';
                    _curr[_position++] = 'e';
                }
            }
            return this;
        }
        /// <summary> 将 DateTime 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(DateTime val)
        {
            _disposable.Assert();
            TryWrite(18);
            int a = val.Year;
            unsafe
            {
                #region 年
                if (a > 999)
                {
                    _curr[_position++] = (char)(a / 1000 + '0');
                    a = a % 1000;
                    _curr[_position++] = (char)(a / 100 + '0');
                    a = a % 100;
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else if (a > 99)
                {
                    _curr[_position++] = '0';
                    _curr[_position++] = (char)(a / 100 + '0');
                    a = a % 100;
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else if (a > 9)
                {
                    _curr[_position++] = '0';
                    _curr[_position++] = '0';
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _curr[_position++] = '0';
                    _curr[_position++] = '0';
                    _curr[_position++] = '0';
                }

                _curr[_position++] = (char)(a + '0');
                #endregion
                _curr[_position++] = '-';
                a = val.Month;
                #region 月
                if (a > 9)
                {
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _curr[_position++] = '0';
                }
                _curr[_position++] = (char)(a + '0');
                #endregion
                a = val.Day;
                _curr[_position++] = '-';
                #region 日
                if (a > 9)
                {
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _curr[_position++] = '0';
                }
                _curr[_position++] = (char)(a + '0');
                #endregion
                a = val.Hour;
                _curr[_position++] = ' ';
                #region 时
                if (a > 9)
                {
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _curr[_position++] = '0';
                }
                _curr[_position++] = (char)(a + '0');
                #endregion
                a = val.Minute;
                _curr[_position++] = ':';
                #region 分
                if (a > 9)
                {
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _curr[_position++] = '0';
                }
                _curr[_position++] = (char)(a + '0');
                #endregion
                a = val.Second;
                _curr[_position++] = ':';
                #region 秒
                if (a > 9)
                {
                    _curr[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _curr[_position++] = '0';
                }
                _curr[_position++] = (char)(a + '0');
                #endregion
            }
            return this;
        }
        /// <summary> 将 Guid 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Guid val)
        {
            Append(val, 'd');
            return this;
        }
        /// <summary> 将 Guid 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Guid val, char format)
        {
            _disposable.Assert();
            unsafe
            {
                char flag = '\0';
                switch (format)
                {
                    case 'D':
                    case 'd':
                        flag = (char)1;
                        TryWrite(36);
                        break;
                    case 'N':
                    case 'n':
                        TryWrite(32);
                        break;
                    case 'P':
                    case 'p':
                        TryWrite(38);
                        _curr[_position++] = '(';
                        flag = ')';
                        break;
                    case 'B':
                    case 'b':
                        TryWrite(38);
                        _curr[_position++] = '{';
                        flag = '}';
                        break;
                    default:
                        Append(val.ToString(format.ToString()));
                        return this;
                }
                var bs = val.ToByteArray();
                _curr[_position++] = HexToChar(bs[3] >> 4);
                _curr[_position++] = HexToChar(bs[3]);
                _curr[_position++] = HexToChar(bs[2] >> 4);
                _curr[_position++] = HexToChar(bs[2]);
                _curr[_position++] = HexToChar(bs[1] >> 4);
                _curr[_position++] = HexToChar(bs[1]);
                _curr[_position++] = HexToChar(bs[0] >> 4);
                _curr[_position++] = HexToChar(bs[0]);
                if (flag > '\0')
                {
                    _curr[_position++] = '-';
                }
                _curr[_position++] = HexToChar(bs[5] >> 4);
                _curr[_position++] = HexToChar(bs[5]);
                _curr[_position++] = HexToChar(bs[4] >> 4);
                _curr[_position++] = HexToChar(bs[4]);
                if (flag > '\0')
                {
                    _curr[_position++] = '-';
                }
                _curr[_position++] = HexToChar(bs[7] >> 4);
                _curr[_position++] = HexToChar(bs[7]);
                _curr[_position++] = HexToChar(bs[6] >> 4);
                _curr[_position++] = HexToChar(bs[6]);
                if (flag > '\0')
                {
                    _curr[_position++] = '-';
                }
                _curr[_position++] = HexToChar(bs[8] >> 4);
                _curr[_position++] = HexToChar(bs[8]);
                _curr[_position++] = HexToChar(bs[9] >> 4);
                _curr[_position++] = HexToChar(bs[9]);
                if (flag > '\0')
                {
                    _curr[_position++] = '-';
                }
                _curr[_position++] = HexToChar(bs[10] >> 4);
                _curr[_position++] = HexToChar(bs[10]);
                _curr[_position++] = HexToChar(bs[11] >> 4);
                _curr[_position++] = HexToChar(bs[11]);
                _curr[_position++] = HexToChar(bs[12] >> 4);
                _curr[_position++] = HexToChar(bs[12]);
                _curr[_position++] = HexToChar(bs[13] >> 4);
                _curr[_position++] = HexToChar(bs[13]);
                _curr[_position++] = HexToChar(bs[14] >> 4);
                _curr[_position++] = HexToChar(bs[14]);
                _curr[_position++] = HexToChar(bs[15] >> 4);
                _curr[_position++] = HexToChar(bs[15]);
                if (flag > (char)1)
                {
                    _curr[_position++] = flag;
                }
            }
            return this;
        }
        /// <summary> 将 Decimal 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Decimal val)
        {
            Append(val.ToString(CultureInfo.InvariantCulture));
            return this;
        }
        /// <summary> 将 Double 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Double val)
        {
            Append(val.ToString(CultureInfo.InvariantCulture));
            return this;
        }
        /// <summary> 将 Single 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Single val)
        {
            Append(val.ToString(CultureInfo.InvariantCulture));
            return this;
        }
        /// <summary> 将 SByte 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(SByte val)
        {
            Append((long)val);
            return this;
        }
        /// <summary> 将 Int16 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Int16 val)
        {
            Append((long)val);
            return this;
        }
        /// <summary> 将 Int32 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Int32 val)
        {
            Append((long)val);
            return this;
        }
        /// <summary> 将 Int64 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Int64 val)
        {
            _disposable.Assert();
            unsafe
            {
                if (val == 0)
                {
                    TryWrite();
                    _curr[_position++] = '0';
                    return this;
                }
                var zero = (long)'0';

                var pos = 19;
                var f = val < 0;
                if (f)
                {
                    _number[pos] = (char)(~(val % 10L) + (long)'1');
                    if (val < -10)
                    {
                        val = val / -10;
                        _number[--pos] = (char)(val % 10L + zero);
                    }
                }
                else
                {
                    _number[pos] = (char)(val % 10L + zero);
                }
                while ((val = val / 10L) != 0L)
                {
                    _number[--pos] = (char)(val % 10L + zero);
                }
                if (f)
                {
                    _number[--pos] = '-';
                }
                var length = 20 - pos;
                Append(_number, pos, length);

            }
            return this;
        }
        /// <summary> 将 Char 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Char val)
        {
            _disposable.Assert();
            TryWrite();
            unsafe
            {
                _curr[_position++] = val;
            }
            return this;
        }
        /// <summary> 将 Byte 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(Byte val)
        {
            Append((ulong)val);
            return this;
        }
        /// <summary> 将 UInt16 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(UInt16 val)
        {
            Append((ulong)val);
            return this;
        }
        /// <summary> 将 UInt32 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(UInt32 val)
        {
            Append((UInt64)val);
            return this;
        }
        /// <summary> 将 UInt64 对象转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(UInt64 val)
        {
            _disposable.Assert();
            unsafe
            {
                if (val == 0)
                {
                    TryWrite();
                    _curr[_position++] = '0';
                    return this;
                }
                var zero = (ulong)'0';

                var pos = 19;
                _number[pos] = (char)(val % 10 + zero);

                while ((val = val / (ulong)10) != (ulong)0)
                {
                    _number[--pos] = (char)(val % (ulong)10 + zero);
                }
                var length = 20 - pos;
                Append(_number, pos, length);

            }
            return this;
        }
        /// <summary> 将字符串追加到当前实例。
        /// </summary>
        public StringWriterA Append(String val)
        {
            _disposable.Assert();
            if (val == null)
            {
                return this;
            }
            var length = val.Length;
            if (length == 0)
            {
                return this;
            }
            TryWrite(length);

            unsafe
            {
                if (length <= 3)
                {
                    _curr[_position++] = val[0];
                    if (length > 2)
                    {
                        _curr[_position++] = val[1];
                        _curr[_position++] = val[2];
                    }
                    else if (length > 1)
                    {
                        _curr[_position++] = val[1];
                    }
                }
                else if (length > _capacity / 2)
                {
                    _buffer.Append(val);
                }
                else
                {
                    fixed (char* c = val)
                    {
                        Append(c, 0, val.Length);
                    }
                }
            }
            return this;
        }

        /// <summary> 将可格式化对象,按指定的格式转换为字符串追加到当前实例。
        /// </summary>
        public StringWriterA AppendFormat(IFormattable val, string format)
        {
            Append(val.ToString(format, null));
            return this;
        }

        /// <summary> 将字符串集合追加到当前实例。
        /// </summary>
        /// <param name="strings">字符串集合</param>
        /// <returns></returns>
        public StringWriterA Append(IEnumerable<string> strings)
        {
            foreach (var str in strings)
            {
                Append(str);
            }
            return this;
        }
        /// <summary> 将字符串集合追加到当前实例并追加回车换行。
        /// </summary>
        /// <param name="str">追加到集合的字符串</param>
        /// <returns></returns>
        public StringWriterA AppendLine(string str)
        {
            Append(str);
            Append(Environment.NewLine);
            return this;
        }

        /// <summary> 将内存中的字符串追加到当前实例。
        /// </summary>
        /// <param name="point">内存指针</param>
        /// <param name="offset">指针偏移量</param>
        /// <param name="length">字符长度</param>
        /// <returns></returns>
        public unsafe void Append(char* point, int offset, int length)
        {
            if (length > 0)
            {
                char* c = point + offset;
                if ((length & 1) != 0)
                {
                    _curr[_position++] = c[0];
                    c++;
                    length--;
                }
                char* p = _curr + _position;

                int* p1 = (int*)p;
                int* p2 = ((int*)c);
                _position += length;
                while (length >= 8)
                {
                    (*p1++) = *(p2++);
                    (*p1++) = *(p2++);
                    (*p1++) = *(p2++);
                    (*p1++) = *(p2++);
                    length -= 8;
                }
                if ((length & 4) != 0)
                {
                    (*p1++) = *(p2++);
                    (*p1++) = *(p2++);
                }
                if ((length & 2) != 0)
                {
                    (*p1) = *(p2);
                }
            }
        }

        /// <summary> 在此实例的结尾追加指定的 Unicode 字符子数组的字符串表示形式。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public StringWriterA Append(char[] value, int startIndex, int charCount)
        {
            Assertor.AreNull(value, "value");
            Assertor.AreInRange(startIndex, "startIndex", 0, value.Length - 1);
            if (charCount == 0)
            {
                return this;
            }
            Assertor.AreInRange(charCount, "charCount", 0, value.Length - 1 - startIndex);
            unsafe
            {
                fixed (char* p = value)
                {
                    Append(p, startIndex, charCount);
                    return this;
                }
            }
        }
        #endregion

        public override string ToString()
        {
            Flush();
            return _buffer.ToString();
        }

        #region IDisposable

        Disposable _disposable;

        /// <summary> 析构函数
        /// </summary>
        ~StringWriterA()
        {
            _disposable.Destructor();
        }
        /// <summary> 释放资源
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        #endregion

    }
}