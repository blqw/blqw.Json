/*
 * 如果引入 blqw.Core.dll 可以删除此文件 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace blqw
{
    /// <summary> 高效的字符串拼接类, 无法继承此类
    /// </summary>
    [DebuggerDisplay("长度:{Length} 内容: {DebugInfo}")]
    public unsafe sealed class QuickStringWriter : IDisposable
    {
        /// <summary> 初始化对象,使用默认大小的缓冲区(4096字节)
        /// </summary>
        public QuickStringWriter()
            : this(4096)
        {

        }

        /// <summary> 初始化对象,并指定缓冲区大小
        /// </summary>
        /// <param name="size"></param>
        public QuickStringWriter(ushort size)
        {
            //确定最后一个字符的位置  长度-1
            _endPosition = size - 1;
            //生成字符串缓冲指针 ,一个char是2个字节,所以要乘以2
            _currIntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size * 2);
            _current = (char*)_currIntPtr.ToPointer();
            //申请数字缓冲区内存 ,20个char足够放下long 和 ulong
            _numberIntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(20 * 2);
            _number = (char*)_numberIntPtr.ToPointer();
        }

        #region 字段
        /// <summary> 数字缓冲指针
        /// </summary>
        private unsafe char* _number;
        private IntPtr _numberIntPtr;
        /// <summary> 指针句柄
        /// </summary>
        private IntPtr _currIntPtr;
        /// <summary> 一级缓冲指针
        /// </summary>
        private char* _current;
        /// <summary> 二级缓冲
        /// </summary>
        private string[] _buffer = new string[8];
        /// <summary> 备用二级缓冲索引
        /// </summary>
        private int _bufferIndex;
        /// <summary> 总字符数
        /// </summary>
        private int _length;
        /// <summary> 结束位,一级缓冲长度减一
        /// </summary>
        private int _endPosition;
        /// <summary> 一级缓冲当前位置
        /// </summary>
        private int _position;
        #endregion

        /// <summary> 获取当前实例中的字符串总长度
        /// </summary>
        public int Length
        {
            get
            {
                return _length + _position;
            }
        }

        #region 私有方法
        /// <summary> 在调试器的变量窗口中的显示的信息
        /// </summary>
        // ReSharper disable once UnusedMember.Local
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
            if (_position > _endPosition)
            {
                Flush();
            }
            else if (_endPosition == int.MaxValue)
            {
                throw new Exception("指针尚未准备就绪!");
            }
        }
        /// <summary> 尝试在一级缓冲区写入指定数量的字符
        /// </summary>
        /// <para>如果尝试写入的字符数大于一级缓冲区的大小,返回false</para>
        /// <para>如果尝试写入的字符数超出一级缓冲区剩余容量,自动调用Flush方法</para>
        /// <param name="count">尝试写入的字符数</param>
        /// <returns></returns>
        private bool TryWrite(int count)
        {
            if (count >= _endPosition)
            {
                return false;
            }
            var pre = _position + count;
            if (pre >= _endPosition)
            {
                Flush();
            }
            else if (_endPosition == int.MaxValue)
            {
                throw new Exception("指针尚未准备就绪!");
            }
            return true;
        }
        #endregion

        #region Append
        private static char HexToChar(int a)
        {
            a &= 15;
            return a > 9 ? (char)(a - 10 + 0x61) : (char)(a + 0x30);
        }

        /// <summary> 将 Guid 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Guid val, char format = 'd')
        {
            int flag;
            switch (format)
            {
                case 'd':
                case 'D':
                    flag = 1;
                    TryWrite(36);
                    break;
                case 'N':
                case 'n':
                    flag = 0;
                    TryWrite(32);
                    break;
                case 'P':
                case 'p':
                    TryWrite(38);
                    _current[_position++] = '(';
                    flag = ')';
                    break;
                case 'B':
                case 'b':
                    TryWrite(38);
                    _current[_position++] = '{';
                    flag = '}';
                    break;
                default:
                    Append(val.ToString(format.ToString()));
                    return this;
            }
            var bs = val.ToByteArray();
            _current[_position++] = HexToChar(bs[3] >> 4);
            _current[_position++] = HexToChar(bs[3]);
            _current[_position++] = HexToChar(bs[2] >> 4);
            _current[_position++] = HexToChar(bs[2]);
            _current[_position++] = HexToChar(bs[1] >> 4);
            _current[_position++] = HexToChar(bs[1]);
            _current[_position++] = HexToChar(bs[0] >> 4);
            _current[_position++] = HexToChar(bs[0]);
            if (flag > 0)
            {
                _current[_position++] = '-';
            }
            _current[_position++] = HexToChar(bs[5] >> 4);
            _current[_position++] = HexToChar(bs[5]);
            _current[_position++] = HexToChar(bs[4] >> 4);
            _current[_position++] = HexToChar(bs[4]);
            if (flag > 0)
            {
                _current[_position++] = '-';
            }
            _current[_position++] = HexToChar(bs[7] >> 4);
            _current[_position++] = HexToChar(bs[7]);
            _current[_position++] = HexToChar(bs[6] >> 4);
            _current[_position++] = HexToChar(bs[6]);
            if (flag > 0)
            {
                _current[_position++] = '-';
            }
            _current[_position++] = HexToChar(bs[8] >> 4);
            _current[_position++] = HexToChar(bs[8]);
            _current[_position++] = HexToChar(bs[9] >> 4);
            _current[_position++] = HexToChar(bs[9]);
            if (flag > 0)
            {
                _current[_position++] = '-';
            }
            _current[_position++] = HexToChar(bs[10] >> 4);
            _current[_position++] = HexToChar(bs[10]);
            _current[_position++] = HexToChar(bs[11] >> 4);
            _current[_position++] = HexToChar(bs[11]);
            _current[_position++] = HexToChar(bs[12] >> 4);
            _current[_position++] = HexToChar(bs[12]);
            _current[_position++] = HexToChar(bs[13] >> 4);
            _current[_position++] = HexToChar(bs[13]);
            _current[_position++] = HexToChar(bs[14] >> 4);
            _current[_position++] = HexToChar(bs[14]);
            _current[_position++] = HexToChar(bs[15] >> 4);
            _current[_position++] = HexToChar(bs[15]);
            if (flag > 1)
            {
                _current[_position++] = (char)flag;
            }
            return this;
        }
        /// <summary> 将 Boolean 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Boolean val)
        {
            if (val)
            {
                TryWrite(4);
                _current[_position++] = 't';
                _current[_position++] = 'r';
                _current[_position++] = 'u';
                _current[_position++] = 'e';
            }
            else
            {
                TryWrite(5);
                _current[_position++] = 'f';
                _current[_position++] = 'a';
                _current[_position++] = 'l';
                _current[_position++] = 's';
                _current[_position++] = 'e';
            }
            return this;
        }

        /// <summary> 将 DateTime 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(DateTime val, bool date, bool time, bool millisecond)
        {
            TryWrite((date ? 10 : 0) + 1 + (time ? 8 : 0) + 1 + (millisecond ? 10 : 3));
            int a;
            if (date)
            {
                a = val.Year;
                #region 年
                if (a > 999)
                {
                    _current[_position++] = (char)(a / 1000 + '0');
                    a = a % 1000;
                    _current[_position++] = (char)(a / 100 + '0');
                    a = a % 100;
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else if (a > 99)
                {
                    _current[_position++] = '0';
                    _current[_position++] = (char)(a / 100 + '0');
                    a = a % 100;
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else if (a > 9)
                {
                    _current[_position++] = '0';
                    _current[_position++] = '0';
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _current[_position++] = '0';
                    _current[_position++] = '0';
                    _current[_position++] = '0';
                }

                _current[_position++] = (char)(a + '0');
                #endregion
                _current[_position++] = '-';
                a = val.Month;
                #region 月
                if (a > 9)
                {
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _current[_position++] = '0';
                }
                _current[_position++] = (char)(a + '0');
                #endregion
                a = val.Day;
                _current[_position++] = '-';
                #region 日
                if (a > 9)
                {
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _current[_position++] = '0';
                }
                _current[_position++] = (char)(a + '0');
                #endregion
                if (time)
                {
                    _current[_position++] = ' ';
                }
            }
            if (time)
            {
                a = val.Hour;
                #region 时
                if (a > 9)
                {
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _current[_position++] = '0';
                }
                _current[_position++] = (char)(a + '0');
                #endregion
                a = val.Minute;
                _current[_position++] = ':';
                #region 分
                if (a > 9)
                {
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _current[_position++] = '0';
                }
                _current[_position++] = (char)(a + '0');
                #endregion
                a = val.Second;
                _current[_position++] = ':';
                #region 秒
                if (a > 9)
                {
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _current[_position++] = '0';
                }
                _current[_position++] = (char)(a + '0');
                #endregion
                if (millisecond)
                {
                    _current[_position++] = '.';
                }
            }
            if (millisecond)
            {
                a = val.Millisecond;
                #region 毫秒
                if (a > 99)
                {
                    _current[_position++] = '0';
                    _current[_position++] = (char)(a / 100 + '0');
                    a = a % 100;
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else if (a > 9)
                {
                    _current[_position++] = '0';
                    _current[_position++] = '0';
                    _current[_position++] = (char)(a / 10 + '0');
                    a = a % 10;
                }
                else
                {
                    _current[_position++] = '0';
                    _current[_position++] = '0';
                    _current[_position++] = '0';
                }

                _current[_position++] = (char)(a + '0');
                #endregion
            }

            return this;
        }
        /// <summary> 将 DateTime 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(DateTime val)
        {
            return Append(val, true, true, false);
        }
        /// <summary> 将 Decimal 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Decimal val)
        {
            return Append(val.ToString(CultureInfo.InvariantCulture));
        }
        /// <summary> 将 Double 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Double val)
        {
            return Append(val.ToString(CultureInfo.InvariantCulture));
        }
        /// <summary> 将 Single 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Single val)
        {
            return Append(val.ToString(CultureInfo.InvariantCulture));
        }
        /// <summary> 将 SByte 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(SByte val)
        {
            return Append((long)val); ;
        }
        /// <summary> 将 Int16 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Int16 val)
        {
            return Append((long)val);
        }
        /// <summary> 将 Int32 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Int32 val)
        {
            return Append((long)val);
        }
        /// <summary> 将 Int64 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Int64 val)
        {
            if (val == 0)
            {
                TryWrite();
                _current[_position++] = '0';
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
            return this;
        }
        /// <summary> 将 Char 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Char val)
        {
            TryWrite();
            _current[_position++] = val;
            return this;
        }
        /// <summary> 将 Byte 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(Byte val)
        {
            return Append((UInt64)val);
        }
        /// <summary> 将 UInt16 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(UInt16 val)
        {
            return Append((UInt64)val);
        }
        /// <summary> 将 UInt32 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(UInt32 val)
        {
            return Append((UInt64)val);
        }
        /// <summary> 将 UInt64 对象转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(UInt64 val)
        {
            if (val == 0)
            {
                TryWrite();
                _current[_position++] = '0';
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
            return this;
        }
        /// <summary> 将字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter Append(String val)
        {
            if (val == null)
            {
                return this;
            }
            var length = val.Length;
            if (length == 0)
            {
                return this;
            }
            if (length <= 3)
            {
                _current[_position++] = val[0];
                TryWrite(length);
                if (length > 2)
                {
                    _current[_position++] = val[1];
                    _current[_position++] = val[2];
                }
                else if (length > 1)
                {
                    _current[_position++] = val[1];
                }
            }
            else if (TryWrite(length))
            {
                fixed (char* c = val)
                {
                    int* p2;
                    if ((length & 1) != 0)
                    {
                        _current[_position++] = c[0];
                        p2 = ((int*)(c + 1));
                        length--;
                    }
                    else
                    {
                        p2 = ((int*)c);
                    }
                    int* p1 = (int*)&_current[_position];


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
            else
            {
                Flush();
                _buffer[_bufferIndex++] = val;
                _length += val.Length;
            }
            return this;
        }
        /// <summary> 
        /// </summary>
        /// <param name="charArray"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public QuickStringWriter Append(char[] charArray, int offset, int length)
        {
            fixed (char* p = charArray)
            {
                return Append(p, offset, length);
            }
        }
        /// <summary> 将内存中的字符串追加到当前实例。
        /// </summary>
        /// <param name="point">内存指针</param>
        /// <param name="offset">指针偏移量</param>
        /// <param name="length">字符长度</param>
        /// <returns></returns>
        public QuickStringWriter Append(char* point, int offset, int length)
        {
            if (length > 0)
            {
                if (TryWrite(length))
                {
                    char* c = point + offset;
                    if ((length & 1) != 0)
                    {
                        _current[_position++] = c[0];
                        c++;
                        length--;
                    }
                    int* p1 = (int*)&_current[_position];
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
                else
                {
                    Flush();
                    _buffer[_bufferIndex++] = new string(point, offset, length);
                    _length += length;
                }
            }

            return this;
        }
        /// <summary> 将可格式化对象,按指定的格式转换为字符串追加到当前实例。
        /// </summary>
        public QuickStringWriter AppendFormat(string format, params object[] args)
        {
            Append(string.Format(format, args));
            return this;
        }

        /// <summary> 将字符串集合追加到当前实例。
        /// </summary>
        /// <param name="strings">字符串集合</param>
        /// <returns></returns>
        public QuickStringWriter Append(IEnumerable<string> strings)
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
        public QuickStringWriter AppendLine(string str)
        {
            Append(str);
            Append(Environment.NewLine);
            return this;
        }

        /// <summary> 将任意对象追加到当前实例。
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <returns></returns>
        public QuickStringWriter AppendLine(object obj)
        {
            if (obj == null)
            {
                return this;
            }
            var conv = obj as IConvertible;
            if (conv != null)
            {
                switch (conv.GetTypeCode())
                {
                    case TypeCode.Boolean: Append(conv.ToBoolean(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Char: Append(conv.ToChar(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.DateTime: Append(conv.ToDateTime(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64: Append(conv.ToInt64(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single: Append(conv.ToString(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64: Append(conv.ToUInt64(CultureInfo.InvariantCulture));
                        break;
                    default: Append(obj.ToString());
                        break;
                }
            }
            else if (obj is Guid)
            {
                Append((Guid)obj);
            }
            else
            {
                Append(obj.ToString());
            }
            return this;
        }
        /// <summary> 将可格式化对象,经过格式化参数处理后,追加到当前实例。
        /// </summary>
        /// <param name="val">可格式化对象</param>
        /// <param name="format">格式化参数</param>
        /// <param name="provider">格式化机制</param>
        public QuickStringWriter Append(IFormattable val, string format, IFormatProvider provider)
        {
            if (val is DateTime)
            {
                if (string.Equals(format, "yyyy-MM-dd HH:mm:ss", StringComparison.Ordinal))
                {
                    return Append((DateTime)val, true, true, false);
                }
                if (string.Equals(format, "yyyy-MM-dd HH:mm:ss.fff", StringComparison.Ordinal))
                {
                    return Append((DateTime)val, true, true, true);
                }
                if (string.Equals(format, "HH:mm:ss", StringComparison.Ordinal))
                {
                    return Append((DateTime)val, false, true, false);
                }
                if (string.Equals(format, "HH:mm:ss.fff", StringComparison.Ordinal))
                {
                    return Append((DateTime)val, false, true, true);
                }
                if (string.Equals(format, "yyyy-MM-dd", StringComparison.Ordinal))
                {
                    return Append((DateTime)val, false, false, false);
                }
                if (string.Equals(format, "fff", StringComparison.Ordinal))
                {
                    return Append((DateTime)val, false, false, true);
                }
            }
            else if (val is Guid && (format == null || format.Length == 1))
            {
                return Append((Guid)val, format[0]);
            }
            return Append(val.ToString(format, provider));
        }
        #endregion

        /// <summary> 清空所有的数据
        /// </summary>
        public void Clear()
        {
            _buffer[0] = _buffer[1] =
            _buffer[2] = _buffer[3] =
            _buffer[4] = _buffer[5] =
            _buffer[6] = _buffer[7] = null;
            _length = 0;
            _position = 0;
        }

        /// <summary> 关闭当前实例
        /// <para>
        /// 该行为将清空所有缓冲区中的内容,
        /// 并阻止除Ready,Close以外的方法调用,直到再次调用Ready方法
        /// </para>
        /// </summary>
        public void Close()
        {
            Clear();
            _endPosition = int.MaxValue;
            _current = null;
        }

        /// <summary> 清理当前实例的一级缓冲区的内容，使所有缓冲数据写入二级缓冲区。
        /// </summary>
        public void Flush()
        {
            if (_position > 0)
            {
                _length += _position;
                if (_bufferIndex == 8)
                {
                    _buffer[0] = string.Concat(_buffer[0], _buffer[1], _buffer[2], _buffer[3],
                                 _buffer[4], _buffer[5], _buffer[6], _buffer[7],
                                 new string(_current, 0, _position));
                    _bufferIndex = 1;
                }
                else
                {
                    _buffer[_bufferIndex++] = new string(_current, 0, _position);
                }
                _position = 0;
            }
        }

        /// <summary> 返回当前实例中的字符串
        /// </summary>
        public override string ToString()
        {
            switch (_bufferIndex)
            {
                case 0:
                    return new string(_current, 0, _position);
                case 1:
                    return string.Concat(_buffer[0], new string(_current, 0, _position));
                case 2:
                    return string.Concat(_buffer[0], _buffer[1], new string(_current, 0, _position));
                case 3:
                    return string.Concat(_buffer[0], _buffer[1], _buffer[2], new string(_current, 0, _position));
                case 4:
                    return string.Concat(_buffer[0], _buffer[1], _buffer[2], _buffer[3], new string(_current, 0, _position));
                case 5:
                    return string.Concat(_buffer[0], _buffer[1], _buffer[2], _buffer[3], _buffer[4], new string(_current, 0, _position));
                case 6:
                    return string.Concat(_buffer[0], _buffer[1], _buffer[2], _buffer[3], _buffer[4], _buffer[5], new string(_current, 0, _position));
                case 7:
                    return string.Concat(_buffer[0], _buffer[1], _buffer[2], _buffer[3], _buffer[4], _buffer[5], _buffer[6], new string(_current, 0, _position));
                case 8:
                    return string.Concat(_buffer[0], _buffer[1], _buffer[2], _buffer[3], _buffer[4], _buffer[5], _buffer[6], _buffer[7], new string(_current, 0, _position));
                default:
                    throw new NotSupportedException();
            }
        }


        #region Dispose
        private int _disposeMark = 0;

        public void Dispose()
        {
            var mark = System.Threading.Interlocked.Exchange(ref _disposeMark, 2);
            if (mark > 1)
            {
                return;
            }
            if (mark == 1)
            {
                Close();
                _buffer = null;
            }
            else
            {
                GC.SuppressFinalize(this);
            }
            System.Runtime.InteropServices.Marshal.FreeHGlobal(_currIntPtr);
            _currIntPtr = IntPtr.Zero;
            System.Runtime.InteropServices.Marshal.FreeHGlobal(_numberIntPtr);
            _numberIntPtr = IntPtr.Zero;
        }

        ~QuickStringWriter()
        {
            if (_disposeMark > 0)
            {
                return;
            }
            System.Threading.Interlocked.Increment(ref _disposeMark);
            Dispose();
        }

        #endregion

    }
}