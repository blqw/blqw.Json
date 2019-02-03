using blqw.JsonServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

namespace blqw.JsonServices
{
    /// <summary>
    /// 高效的字符串拼接类, 无法继承此类
    /// </summary>
    [DebuggerDisplay("长度:{Length} 内容: {DebugInfo}")]
    public sealed unsafe class QuickStringWriter : TextWriter
    {
        /// <summary>
        /// 初始化对象,使用默认大小的缓冲区(4096字节)
        /// </summary>
        public QuickStringWriter()
            : this(4096)
        {
        }

        /// <summary>
        /// 初始化对象,并指定缓冲区大小
        /// </summary>
        /// <param name="size"> </param>
        public QuickStringWriter(ushort size)
        {
            //生成字符串缓冲指针 ,一个char是2个字节,所以要乘以2
            _intPtr = Marshal.AllocHGlobal(size * 2);

            //前20个用来放数字,char足够放下long 和 ulong
            _number = (char*)_intPtr.ToPointer();
            _current = _number + 20;

            //确定最后一个字符的位置  长度-1
            _endPosition = size - 1 - 20;
        }

        /// <summary>
        /// 初始化对象,并指定缓冲区大小
        /// </summary>
        /// <param name="p"> </param>
        /// <param name="size"> </param>
        public QuickStringWriter(char* p, ushort size)
        {
            //前20个用来放数字,char足够放下long 和 ulong
            _number = p;
            _current = _number + 20;

            //确定最后一个字符的位置  长度-1
            _endPosition = size - 1 - 20;
        }


        /// <summary>
        /// 获取当前实例中的字符串总长度
        /// </summary>
        public int Length => _length + _position;

        public override Encoding Encoding => Encoding.UTF8;

        /// <summary>
        /// 清空所有的数据
        /// </summary>
        public void Clear()
        {
            if (_length <= 0) return;
            Buffer.Clear();
            _length = 0;
            _position = 0;
        }

        /// <summary>
        /// 关闭当前实例
        /// </summary>
        public override void Close()
        {
            Clear();
            _current = null;
        }

        /// <summary>
        /// 清理当前实例的一级缓冲区的内容，使所有缓冲数据写入二级缓冲区。
        /// </summary>
        public override void Flush()
        {
            if (_position <= 0) return;
            _length += _position;
            Buffer.Append(_current, _position);
            _position = 0;
        }

        /// <summary>
        /// 返回当前实例中的字符串
        /// </summary>
        public override string ToString()
        {
            Flush();
            return Buffer.ToString();
        }

        #region 字段

        /// <summary>
        /// 数字缓冲指针
        /// </summary>
        private readonly char* _number;

        /// <summary>
        /// 指针句柄
        /// </summary>
        private IntPtr _intPtr;

        /// <summary>
        /// 一级缓冲指针
        /// </summary>
        private char* _current;

        /// <summary>
        /// 二级缓冲
        /// </summary>
        private StringBuilder _buffer;

        /// <summary>
        /// 备用二级缓冲索引
        /// </summary>
        /// <summary>
        /// 总字符数
        /// </summary>
        private int _length;

        /// <summary>
        /// 结束位,一级缓冲长度减一
        /// </summary>
        private readonly int _endPosition;

        /// <summary>
        /// 一级缓冲当前位置
        /// </summary>
        private int _position;

        #endregion

        #region 私有方法

        /// <summary>
        /// 在调试器的变量窗口中的显示的信息
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private string DebugInfo
        {
            get
            {
                var str = ToString();
                if (str.Length <= 70) return str;
                var s = str;
                str = s.Substring(0, 30) + "  ......  ";
                str += s.Substring(s.Length - 30);
                return str;
            }
        }

        public StringBuilder Buffer => _buffer ?? (_buffer = new StringBuilder((int)_endPosition));

        /// <summary>
        /// 
        /// 尝试在一级缓冲区写入一个字符
        /// <para> 如果一级缓冲区已满,将会自动调用Flush方法转移一级缓冲区中的内容 </para>
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

        /// <summary>
        /// 尝试在一级缓冲区写入指定数量的字符
        /// </summary>
        /// <para> 如果尝试写入的字符数大于一级缓冲区的大小,返回false </para>
        /// <para> 如果尝试写入的字符数超出一级缓冲区剩余容量,自动调用Flush方法 </para>
        /// <param name="count"> 尝试写入的字符数 </param>
        /// <returns> </returns>
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

        #region Write

        private static char HexToChar(int a)
        {
            a &= 15;
            return a > 9 ? (char)(a - 10 + 0x61) : (char)(a + 0x30);
        }

        public override void Write(char[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            base.Write(buffer, 0, buffer.Length);
        }

        public override void Write(string format, object arg0)
        {
            Write(string.Format(format, arg0));
        }

        public override void Write(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0, arg1));
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 将 Guid 对象转换为字符串追加到当前实例。
        /// </summary>
        public void Write(Guid val, char format = 'd')
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
                    Write(val.ToString(format.ToString()));
                    return;
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
        }

        /// <summary>
        /// 将 Boolean 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(bool val)
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
        }

        /// <summary>
        /// 将 DateTime 对象转换为字符串追加到当前实例。
        /// </summary>
        public void Write(DateTime val, bool date, bool time, bool millisecond)
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
        }

        /// <summary>
        /// 将 DateTime 对象转换为字符串追加到当前实例。
        /// </summary>
        public void Write(DateTime val)
        {
            Write(val, true, true, false);
        }

        /// <summary>
        /// 将 Decimal 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(decimal val)
        {
            Write(val.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 将 Double 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(double val)
        {
            Write(val.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 将 Single 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(float val)
        {
            Write(val.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 将 SByte 对象转换为字符串追加到当前实例。
        /// </summary>
        public void Write(sbyte val)
        {
            Write((long)val);
        }

        /// <summary>
        /// 将 Int16 对象转换为字符串追加到当前实例。
        /// </summary>
        public void Write(short val)
        {
            Write((long)val);
        }

        /// <summary>
        /// 将 Int32 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(int val)
        {
            Write((long)val);
        }

        /// <summary>
        /// 将 Int64 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(long val)
        {
            if (val == 0)
            {
                TryWrite();
                _current[_position++] = '0';
                return;
            }

            const long zero = (long)'0';

            var pos = 19;
            var f = val < 0;
            if (f)
            {
                _number[pos] = (char)(~(val % 10L) + '1');
                if (val < -9)
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
            Write(_number, pos, length);
        }

        /// <summary>
        /// 将 Char 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(char val)
        {
            TryWrite();
            _current[_position++] = val;
        }

        /// <summary>
        /// 将 Byte 对象转换为字符串追加到当前实例。
        /// </summary>
        public void Write(byte val)
        {
            Write((ulong)val);
        }

        /// <summary>
        /// 将 UInt16 对象转换为字符串追加到当前实例。
        /// </summary>
        public void Write(ushort val)
        {
            Write((ulong)val);
        }

        /// <summary>
        /// 将 UInt32 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(uint val)
        {
            Write((ulong)val);
        }

        /// <summary>
        /// 将 UInt64 对象转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(ulong val)
        {
            if (val == 0)
            {
                TryWrite();
                _current[_position++] = '0';
                return;
            }
            var zero = (ulong)'0';

            var pos = 19;
            _number[pos] = (char)(val % 10 + zero);

            while ((val = val / 10) != 0)
            {
                _number[--pos] = (char)(val % 10 + zero);
            }
            var length = 20 - pos;
            Write(_number, pos, length);
        }

        /// <summary>
        /// 将字符串追加到当前实例。
        /// </summary>
        public override void Write(string val)
        {
            var length = val.Length;
            if (length == 0)
            {
            }
            if (length <= 3)
            {
                TryWrite(length);
                _current[_position++] = val[0];
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
                        p2 = (int*)(c + 1);
                        length--;
                    }
                    else
                    {
                        p2 = (int*)c;
                    }
                    var p1 = (int*)&_current[_position];


                    _position += length;
                    while (length >= 8)
                    {
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                        length -= 8;
                    }
                    if ((length & 4) != 0)
                    {
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                    }
                    if ((length & 2) != 0)
                    {
                        *p1 = *p2;
                    }
                }
            }
            else
            {
                Flush();
                Buffer.Append(val);
                //_buffer[_bufferIndex++] = val;
                _length += val.Length;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="charArray"> </param>
        /// <param name="offset"> </param>
        /// <param name="length"> </param>
        /// <returns> </returns>
        public override void Write(char[] charArray, int offset, int length)
        {
            fixed (char* p = charArray)
            {
                Write(p, offset, length);
            }
        }

        /// <summary>
        /// 将内存中的字符串追加到当前实例。
        /// </summary>
        /// <param name="point"> 内存指针 </param>
        /// <param name="offset"> 指针偏移量 </param>
        /// <param name="length"> 字符长度 </param>
        /// <returns> </returns>
        public void Write(char* point, int offset, int length)
        {
            if (length > 0)
            {
                if (TryWrite(length))
                {
                    var c = point + offset;
                    if ((length & 1) != 0)
                    {
                        _current[_position++] = c[0];
                        c++;
                        length--;
                    }
                    var p1 = (int*)&_current[_position];
                    var p2 = (int*)c;
                    _position += length;
                    while (length >= 8)
                    {
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                        length -= 8;
                    }
                    if ((length & 4) != 0)
                    {
                        *p1++ = *p2++;
                        *p1++ = *p2++;
                    }
                    if ((length & 2) != 0)
                    {
                        *p1 = *p2;
                    }
                }
                else
                {
                    Flush();
                    Buffer.Append(point + offset, length);
                    _length += length;
                }
            }
        }

        /// <summary>
        /// 将可格式化对象,按指定的格式转换为字符串追加到当前实例。
        /// </summary>
        public override void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        /// <summary>
        /// 将字符串集合追加到当前实例。
        /// </summary>
        /// <param name="strings"> 字符串集合 </param>
        /// <returns> </returns>
        public void Write(IEnumerable<string> strings)
        {
            foreach (var str in strings)
            {
                Write(str);
            }
        }

        /// <summary>
        /// 将字符串集合追加到当前实例并追加回车换行。
        /// </summary>
        /// <param name="str"> 追加到集合的字符串 </param>
        /// <returns> </returns>
        public override void WriteLine(string str)
        {
            Write(str);
            Write(Environment.NewLine);
        }

        /// <summary>
        /// 将任意对象追加到当前实例。
        /// </summary>
        /// <param name="obj"> 对象实例 </param>
        /// <returns> </returns>
        public override void Write(object obj)
        {
            if (obj == null)
            {
                return;
            }
            var conv = obj as IConvertible;
            if (conv != null)
            {
                switch (conv.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        Write(conv.ToBoolean(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Char:
                        Write(conv.ToChar(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.DateTime:
                        Write(conv.ToDateTime(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        Write(conv.ToInt64(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        Write(conv.ToUInt64(CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                        return;
                    case TypeCode.Object:
                        break;
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                    case TypeCode.String:
                    default:
                        Write(conv.ToString(CultureInfo.InvariantCulture));
                        break;
                }
            }
            else if (obj is Guid)
            {
                Write((Guid)obj);
            }
            else
            {
                Write(obj.ToString());
            }
        }

        /// <summary>
        /// 将可格式化对象,经过格式化参数处理后,追加到当前实例。
        /// </summary>
        /// <param name="val"> 可格式化对象 </param>
        /// <param name="format"> 格式化参数 </param>
        /// <param name="provider"> 格式化机制 </param>
        public void Write(IFormattable val, string format, IFormatProvider provider)
        {
            if (val is DateTime)
            {
                if (string.Equals(format, "yyyy-MM-dd HH:mm:ss", StringComparison.Ordinal))
                {
                    Write((DateTime)val, true, true, false);
                }
                if (string.Equals(format, "yyyy-MM-dd HH:mm:ss.fff", StringComparison.Ordinal))
                {
                    Write((DateTime)val, true, true, true);
                }
                if (string.Equals(format, "HH:mm:ss", StringComparison.Ordinal))
                {
                    Write((DateTime)val, false, true, false);
                }
                if (string.Equals(format, "HH:mm:ss.fff", StringComparison.Ordinal))
                {
                    Write((DateTime)val, false, true, true);
                }
                if (string.Equals(format, "yyyy-MM-dd", StringComparison.Ordinal))
                {
                    Write((DateTime)val, false, false, false);
                }
                if (string.Equals(format, "fff", StringComparison.Ordinal))
                {
                    Write((DateTime)val, false, false, true);
                }
            }
            else if (val is Guid && format?.Length == 1)
            {
                Write((Guid)val, format[0]);
            }
            Write(val.ToString(format, provider));
        }

        #endregion

        #region Dispose

        private bool _disposed;

        /// <summary>
        /// 释放由 <see cref="T:System.IO.TextWriter" /> 占用的非托管资源，还可以释放托管资源。
        /// </summary>
        /// <param name="disposing"> 若要释放托管资源和非托管资源，则为 true；若仅释放非托管资源，则为 false。 </param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _disposed = true;
                Close();
            }
            if (_intPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_intPtr);
                _intPtr = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}