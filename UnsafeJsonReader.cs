using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace blqw
{
    [DebuggerDisplay("当前字符: {_Current}")]
    unsafe class UnsafeJsonReader : IDisposable
    {
        /// <summary>
        /// <para>包含1: 可以为头的字符</para>
        /// <para>包含2: 可以为单词的字符</para>
        /// <para>包含4: 可以为数字的字符</para>
        /// <para>等于8: 空白字符</para>
        /// <para>包含16:转义字符</para>
        /// <para></para>
        /// </summary>
        readonly static byte[] WordChars = new byte[char.MaxValue];
        readonly static sbyte[, ,] DateTimeWords;
        static UnsafeJsonReader()
        {
            WordChars['-'] = 1 | 4;
            WordChars['+'] = 1 | 4;

            WordChars['$'] = 1 | 2;
            WordChars['_'] = 1 | 2;
            for (char c = 'a'; c <= 'z'; c++)
            {
                WordChars[c] = 1 | 2;
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                WordChars[c] = 1 | 2;
            }

            WordChars['.'] = 1 | 2 | 4;
            for (char c = '0'; c <= '9'; c++)
            {
                WordChars[c] = 4;
            }

            //科学计数法
            WordChars['e'] |= 4;
            WordChars['E'] |= 4;

            WordChars[' '] = 8;
            WordChars['\t'] = 8;
            WordChars['\r'] = 8;
            WordChars['\n'] = 8;


            WordChars['t'] |= 16;
            WordChars['r'] |= 16;
            WordChars['n'] |= 16;
            WordChars['f'] |= 16;
            WordChars['0'] |= 16;
            WordChars['"'] |= 16;
            WordChars['\''] |= 16;
            WordChars['\\'] |= 16;
            WordChars['/'] |= 16;


            var a = new string[] { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            var b = new string[] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            DateTimeWords = new sbyte[23, 21, 25];

            for (sbyte i = 0; i < a.Length; i++)
            {
                var d = a[i];
                DateTimeWords[d[0] - 97, d[1] - 97, d[2] - 97] = (sbyte)(i + 1);
            }

            for (sbyte i = 0; i < b.Length; i++)
            {
                var d = b[i];
                DateTimeWords[d[0] - 97, d[1] - 97, d[2] - 97] = (sbyte)-(i + 1);
            }
            DateTimeWords['g' - 97, 'm' - 97, 't' - 97] = sbyte.MaxValue;
        }

        Char* _P;
        int _Position;
        int _Length;
        int _End;
        public UnsafeJsonReader(Char* origin, int length)
        {
            if (origin == null)
            {
                throw new ArgumentNullException("origin");
            }
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            _P = origin;
            _Length = length;
            _End = length - 1;
            _Position = 0;
            _Current = *origin;
        }

        Char _Current;
        public Char Current { get { return _Current; } }

        /// <summary> 当前位置
        /// </summary>
        public int Position
        {
            get { return _Position; }
            set
            {
                if (_Position >= _Length)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (_IsDisposed == false)
                {
                    _Position = value;
                    _Current = _P[_Position];
                }
            }
        }

        /// <summary> 是否已经到结尾,忽略空白
        /// </summary>
        public bool IsEnd()
        {
            if (_Position > _End)
            {
                return true;
            }
            else if (WordChars[_Current] == 8)
            {
                while (_Position < _End)
                {
                    _Position++;
                    _Current = _P[_Position];
                    if (WordChars[_Current] != 8)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 移动到下一个字符,如果已经是结尾则抛出异常
        /// </summary>
        public void MoveNext()
        {
            if (_Position < _End)
            {
                _Position++;
                _Current = _P[_Position];
            }
            else if (_Position == _End)
            {
                _Position++;
                _Current = '\0';
            }
            else
            {
                ThrowException();
            }
        }

        /// <summary> 跳过一个单词
        /// </summary>
        public void SkipWord()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }

            if (WordChars[_Current] != 3)      //只能是3 可以为开头的单词
            {
                ThrowException();
            }
            MoveNext();
            while ((WordChars[_Current] & 2) != 0)//读取下一个字符 可是是单词
            {
                MoveNext();
            }
        }

        /// <summary> 跳过一个指定字符,忽略空白,如果字符串意外结束抛出异常
        /// </summary>
        public bool SkipChar(char c)
        {
            if (IsEnd())
            {
                ThrowException();
            }
            if (_Current == c)
            {
                if (_Position > _End)
                {
                    _Position++;
                    _Current = '\0';
                }
                else
                {
                    _Position++;
                    _Current = _P[_Position];
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 跳过一个字符串
        /// </summary>
        public void SkipString()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }
            Char quot = _Current;
            if (quot != '"' && quot != '\'')
            {
                ThrowException();
            }
            MoveNext();
            while (_Current != quot)//是否是结束字符
            {
                MoveNext();
                if (_Current == '\\')//是否是转义符
                {
                    if ((WordChars[_Current] & 16) == 0)
                    {
                        ThrowException();
                    }
                    MoveNext();
                }
            }
            MoveNext();
        }
        //袖珍版字符串处理类
        class MiniBuffer
        {
            char* _p;
            string[] _str;
            int _index;
            int _position;
            public MiniBuffer(char* p)
            {
                this._p = p;
            }

            public void AddString(char* point, int offset, int length)
            {
                if (length > 0)
                {
                    if (_position + length > 255)
                    {
                        Flush();
                        if (length > 200)
                        {
                            _str[_index++] = new string(point, offset, length);
                            return;
                        }
                    }


                    char* c = point + offset;
                    if ((length & 1) != 0)
                    {
                        _p[_position++] = c[0];
                        c++;
                        length--;
                    }
                    int* p1 = (int*)&_p[_position];
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
                        (*p1++) = *(p2++);
                    }

                }
            }

            public void AddChar(char c)
            {
                if (_position == 255)
                {
                    Flush();
                }
                _p[_position++] = c;
            }

            private void Flush()
            {
                if (_str == null)
                {
                    _str = new string[3];
                    _str[_index++] = new string(_p, 0, _position);
                    _index = 1;
                }
                else if (_index < 3)
                {
                    _str[_index++] = new string(_p, 0, _position);
                }
                else
                {
                    _str[0] = string.Concat(_str[0], _str[1], _str[2], new string(_p, 0, _position));
                    _index = 1;
                }
                _position = 0;
            }

            public override string ToString()
            {
                if (_str == null)
                {
                    return new string(_p, 0, _position);
                }

                if (_index == 1)
                {
                    return string.Concat(_str[0], new string(_p, 0, _position));
                }
                else if (_index == 2)
                {
                    return string.Concat(_str[0], _str[1], new string(_p, 0, _position));
                }
                else
                {
                    return string.Concat(_str[0], _str[1], _str[2], new string(_p, 0, _position));
                }
            }
        }

        /// <summary> 读取时间类型的对象
        /// </summary>
        /// <returns></returns>
        public object ReadDateTime()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }

            Char quot = _Current;
            if (quot != '"' && quot != '\'')
            {
                ThrowException();
            }
            MoveNext();
            if (_Current == quot)
            {
                MoveNext();
                return null;
            }
            var index = _Position;

            //0年,1月,2日,3时,4分,5秒,6毫秒,7星期,8+12,9gmt,10 月
            int[] datetime = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            int numindex = 0;
            if (IsEnd())//跳过空白
            {
                ThrowException();
            }

            int number = -1;

            do
            {
                if (_Current >= '0' && _Current <= '9')
                {
                    number = ReadPositiveInteger();
                }
                else if (WordChars[_Current] == 3)
                {
                    var wk = GetDateTimeWord();
                    if (wk == 0) goto label_parse;
                    if (wk == sbyte.MaxValue)
                    {
                        if (datetime[9] >= 0) goto label_parse;
                        datetime[9] = 8;
                    }
                    else if (wk > 0)
                    {
                        if (datetime[10] >= 0) goto label_parse;
                        datetime[10] = wk;
                    }
                    else
                    {
                        if (datetime[7] >= 0) goto label_parse;
                        datetime[7] = -wk;
                    }
                }
                else
                {
                    switch (_Current)
                    {
                        case '-':
                        case '/':
                            if (number < 0 || numindex > 2) goto label_parse;
                            datetime[numindex++] = number;
                            break;
                        case ' ':
                            if (number >= 0)
                            {
                                datetime[numindex++] = number;
                            }
                            break;
                        case ':':
                            if (numindex < 3) numindex = 3;
                            datetime[numindex++] = number;
                            break;
                        case '.':
                            if (numindex != 6) goto label_parse;
                            datetime[6] = number;
                            break;
                        case 'a':
                        case 'A':
                            MoveNext();
                            if (_Current != 'm' && _Current != 'M') goto label_parse;
                            if (datetime[8] >= 0) goto label_parse;
                            datetime[8] = 0;
                            break;
                        case 'p':
                        case 'P':
                            MoveNext();
                            if (_Current != 'm' && _Current != 'M') goto label_parse;
                            if (datetime[8] >= 0) goto label_parse;
                            datetime[8] = 12;
                            break;
                        case '年':
                            if (datetime[0] >= 0) goto label_parse;
                            datetime[0] = number;
                            break;
                        case '月':
                            if (datetime[1] >= 0) goto label_parse;
                            datetime[1] = number;
                            break;
                        case '日':
                            if (datetime[2] >= 0) goto label_parse;
                            datetime[2] = number;
                            break;
                        case '时':
                            if (datetime[3] >= 0) goto label_parse;
                            datetime[3] = number;
                            break;
                        case '分':
                            if (datetime[4] >= 0) goto label_parse;
                            datetime[4] = number;
                            break;
                        case '秒':
                            if (datetime[5] >= 0) goto label_parse;
                            datetime[5] = number;
                            break;
                        case '上':
                            MoveNext();
                            if (_Current != '午') goto label_parse;
                            if (datetime[8] >= 0) goto label_parse;
                            datetime[8] = 0;
                            break;
                        case '下':
                            MoveNext();
                            if (_Current != '午') goto label_parse;
                            if (datetime[8] >= 0) goto label_parse;
                            datetime[8] = 12;
                            break;
                        case '星':
                            if (datetime[7] >= 0) goto label_parse;
                            datetime[7] = 1;
                            MoveNext();
                            if (_Current != '期') goto label_parse;
                            MoveNext();
                            MoveNext();
                            break;
                        case '周':
                            if (datetime[7] >= 0) goto label_parse;
                            datetime[7] = 1;
                            MoveNext();
                            MoveNext();
                            break;
                        case ',':
                            break;
                        default:
                            goto label_parse;
                            break;
                    }
                    number = -1;
                    MoveNext();
                }
            } while (_Current != quot);//是否是结束字符

            if (datetime[2] == -1 && datetime[10] >= 0)
            {
                datetime[2] = datetime[1];
                datetime[1] = datetime[10];
            }
            if (datetime[2] > 31)
            {
                datetime[10] = datetime[2];
                datetime[2] = datetime[0];
                datetime[0] = datetime[10];
            }

            MoveNext();
            if (datetime[0] > 9999 || datetime[1] > 12 || datetime[2] > 31 ||
                datetime[3] > 24 || datetime[4] > 59 || datetime[5] > 59 || datetime[6] > 999)
            {
                goto label_parse;
            }

            if (datetime[0] <= 0) datetime[0] = 1990;
            else if (datetime[0] < 100) datetime[0] += 1900;
            if (datetime[1] <= 0) datetime[1] = 1;
            if (datetime[2] <= 0) datetime[2] = 1;
            if (datetime[3] <= 0) datetime[3] = 0;
            if (datetime[4] <= 0) datetime[4] = 0;
            if (datetime[5] <= 0) datetime[5] = 0;
            if (datetime[6] <= 0) datetime[6] = 0;
            if (datetime[8] > 0) datetime[3] = datetime[3] + datetime[8];
            var td = new DateTime(datetime[0], datetime[1], datetime[2], datetime[3], datetime[4], datetime[5], datetime[6]);
            if (datetime[9] >= 0)
            {
                td = td.AddHours(datetime[9]);
            }
            return td;
        label_parse: ;

            while (_Current != quot)
            {
                MoveNext();
            }
            var str = new string(_P, index, _Position - index);
            return DateTime.Parse(str);
        }

        /// <summary> 获取时间中的英文字符,返回127 = GMT, 大于0 表示月份, 小于0 表示星期
        /// </summary>
        /// <returns></returns>
        private int GetDateTimeWord()
        {
            char[] c = new char[3];
            for (int i = 0; i < 3; i++)
            {
                if (_Current >= 'a' && _Current <= 'z')
                {
                    c[i] = (char)(_Current - 'a');
                }
                else if (_Current >= 'A' && _Current <= 'Z')
                {
                    c[i] = (char)(_Current - 'A');
                }
                else
                {
                    return 0;
                }
                MoveNext();
            }
            return DateTimeWords[c[0], c[1], c[2]];
        }
        /// <summary> 读取正整数,在ReadDateTime函数中使用
        /// </summary>
        /// <returns></returns>
        private int ReadPositiveInteger()
        {
            if (_Current < '0' || _Current > '9')
            {
                return -1;
            }
            int num = 0;
            do
            {
                num = num * 10 + (_Current - '0');
                MoveNext();
            } while (_Current >= '0' && _Current <= '9');
            return num;
        }

        /// <summary> 读取常量
        /// </summary>
        /// <returns></returns>
        public object ReadConsts()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }
            switch (_Current)
            {
                case 'f'://false
                    MoveNext();
                    if (_Current != 'a') ThrowException();
                    MoveNext();
                    if (_Current != 'l') ThrowException();
                    MoveNext();
                    if (_Current != 's') ThrowException();
                    MoveNext();
                    if (_Current != 'e') ThrowException();
                    MoveNext();
                    return false;
                case 't'://true
                    MoveNext();
                    if (_Current != 'r') ThrowException();
                    MoveNext();
                    if (_Current != 'u') ThrowException();
                    MoveNext();
                    if (_Current != 'e') ThrowException();
                    MoveNext();
                    return true;
                case 'n'://null
                    MoveNext();
                    if (_Current != 'u') ThrowException();
                    MoveNext();
                    if (_Current != 'l') ThrowException();
                    MoveNext();
                    if (_Current != 'l') ThrowException();
                    MoveNext();
                    return null;
                case 'u'://undefined
                    MoveNext();
                    if (_Current != 'n') ThrowException();
                    MoveNext();
                    if (_Current != 'd') ThrowException();
                    MoveNext();
                    if (_Current != 'e') ThrowException();
                    MoveNext();
                    if (_Current != 'f') ThrowException();
                    MoveNext();
                    if (_Current != 'i') ThrowException();
                    MoveNext();
                    if (_Current != 'n') ThrowException();
                    MoveNext();
                    if (_Current != 'e') ThrowException();
                    MoveNext();
                    if (_Current != 'd') ThrowException();
                    MoveNext();
                    return null;
                case 'N'://NaN
                    MoveNext();
                    if (_Current != 'a') ThrowException();
                    MoveNext();
                    if (_Current != 'N') ThrowException();
                    MoveNext();
                    return double.NaN;
                case 'I'://Infinity
                    MoveNext();
                    if (_Current != 'n') ThrowException();
                    MoveNext();
                    if (_Current != 'f') ThrowException();
                    MoveNext();
                    if (_Current != 'i') ThrowException();
                    MoveNext();
                    if (_Current != 'n') ThrowException();
                    MoveNext();
                    if (_Current != 'i') ThrowException();
                    MoveNext();
                    if (_Current != 't') ThrowException();
                    MoveNext();
                    if (_Current != 'y') ThrowException();
                    MoveNext();
                    return double.PositiveInfinity;
                case '-'://-Infinity
                    MoveNext();
                    if ((WordChars[_Current] & 4) > 0)
                    {
                        _Position--;
                        _Current = _P[_Position];
                        return ReadNumber();
                    }
                    if (_Current != 'I') ThrowException();
                    MoveNext();
                    if (_Current != 'n') ThrowException();
                    MoveNext();
                    if (_Current != 'f') ThrowException();
                    MoveNext();
                    if (_Current != 'i') ThrowException();
                    MoveNext();
                    if (_Current != 'n') ThrowException();
                    MoveNext();
                    if (_Current != 'i') ThrowException();
                    MoveNext();
                    if (_Current != 't') ThrowException();
                    MoveNext();
                    if (_Current != 'y') ThrowException();
                    MoveNext();
                    return double.NegativeInfinity;
                default:
                    if ((WordChars[_Current] & 4) > 0)
                    {
                        return ReadNumber();
                    }
                    ThrowException();
                    return null;
            }
        }

        /// <summary> 读取单词
        /// </summary>
        /// <returns></returns>
        public string ReadWord()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }

            //单词起始字符只能是1+2
            if (WordChars[_Current] != 3)
            {
                return null;
            }

            var index = _Position;
            while ((WordChars[_Current] & 6) != 0)//2或者4都可以
            {
                MoveNext();//读取下一个字符
            }
            return new string(_P, index, _Position - index);
        }

        /// <summary> 读取数字
        /// </summary>
        /// <returns></returns>
        private object ReadNumber()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }

            int pot = -1;
            bool neg = false;
            //if ((WordChars[_Current] & 4) == 0)
            //{
            //    return ReadConsts();
            //}

            switch (_Current)
            {
                case '.':
                    pot = 0;
                    break;
                case '+':
                    MoveNext();//读取下一个字符
                    break;
                case '-':
                    MoveNext();//读取下一个字符
                    //if ((WordChars[_Current] & 4) == 0)
                    //{
                    //    _Position--;
                    //    _Current = _p[_Position];
                    //    return ReadConsts();
                    //}
                    neg = true;
                    break;
                default:
                    break;
            }
            int index = _Position;


            while (true)
            {
                switch ((WordChars[_Current] & 6))
                {
                    case 0:
                        if (neg)
                        {
                            if (pot >= 0)
                            {
                                return -ReadDecimal(index, _Position);
                            }
                            return -ReadInteger(index, _Position);
                        }
                        else if (pot >= 0)
                        {
                            return ReadDecimal(index, _Position);
                        }
                        return ReadInteger(index, _Position);
                    case 4:
                        break;
                    case 6:
                        if (pot < 0)
                        {
                            pot = _Position;
                        }
                        else if (_Current == '.')
                        {
                            ThrowException();
                        }

                        if (_Current != '.')
                        {
                            if (neg)
                            {
                                index--;
                            }
                            string str = null;
                            if (_Current == 'e' || _Current == 'E')
                            {
                                //如果是用科学计数法计的,那么小数点后面最多保存5位
                                //不然有可能会报错
                                if (_Position - pot > 6)
                                {
                                    str = new string(_P, index, pot + 6 - index);
                                    index = _Position;
                                }
                            }
                            MoveNext();
                            while ((WordChars[_Current] & 4) != 0)
                            {
                                MoveNext();//读取下一个字符
                            }

                            str += new string(_P, index, _Position - index);
                            double d;
                            if (double.TryParse(str, out d))
                            {
                                return d;
                            }
                            else
                            {
                                ThrowException();
                                return null;
                            }
                        }
                        break;
                    default:
                        ThrowException();
                        break;
                }
                MoveNext();//读取下一个字符
            }
        }

        /// <summary> 读取小数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private double ReadDecimal(int index, int end)
        {
            double d1 = 0d;
            for (; _P[index] != '.'; index++)
            {
                d1 = d1 * 10 + ((double)(_P[index]) - (double)'0');
            }
            index++;
            end--;
            double d2 = 0d;
            for (; index <= end; end--)
            {
                d2 = d2 * 0.1 + ((long)(_P[end]) - (long)'0');
            }
            return d1 + d2 * 0.1;
        }

        /// <summary> 读取整数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private long ReadInteger(int index, int end)
        {
            long l = 0L;
            for (; index < end; index++)
            {
                l = l * 10 + ((long)(_P[index]) - (long)'0');
            }
            return l;
        }

        /// <summary> 读取字符串
        /// </summary>
        public string ReadString()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }

            Char quot = _Current;
            if (quot != '"' && quot != '\'')
            {
                ThrowException();
            }
            MoveNext();
            if (_Current == quot)
            {
                MoveNext();
                return "";
            }

            var index = _Position;
            MiniBuffer buff = null;

            do
            {
                if (_Current == '\\')//是否是转义符
                {
                    if ((WordChars[_Current] & 16) == 0)
                    {
                        ThrowException();
                    }
                    if (buff == null)
                    {
                        //锁定指针
                        char* p = stackalloc char[255];
                        buff = new MiniBuffer(p);
                    }
                    buff.AddString(_P, index, _Position - index);
                    MoveNext();
                    switch (_Current)
                    {
                        case 't':
                            buff.AddChar('\t');
                            index = _Position + 1;
                            break;
                        case 'n':
                            buff.AddChar('\n');
                            index = _Position + 1;
                            break;
                        case 'r':
                            buff.AddChar('\r');
                            index = _Position + 1;
                            break;
                        case '0':
                            buff.AddChar('\0');
                            index = _Position + 1;
                            break;
                        case 'f':
                            buff.AddChar('\f');
                            index = _Position + 1;
                            break;
                        default:
                            index = _Position;
                            break;
                    }
                }
                MoveNext();
            } while (_Current != quot);//是否是结束字符
            string str;
            if (buff == null)
            {
                str = new string(_P, index, _Position - index);
            }
            else
            {
                buff.AddString(_P, index, _Position - index);
                str = buff.ToString();
            }
            MoveNext();
            return str;
        }

        bool _IsDisposed;

        public void Dispose()
        {
            _P = null;
            _End = 0;
            _IsDisposed = true;
            _Current = '\0';
        }

        private void ThrowException()
        {
            if (_IsDisposed)
            {
                throw new ObjectDisposedException("UnsafeJsonReader", "不能访问已释放的对象!");
            }
            if (this.IsEnd())
            {
                Dispose();
                throw new Exception("遇到意外的字符串结尾,解析失败!");
            }
            else
            {
                int i = Math.Max(_Position - 20, 0);
                int j = Math.Max(_Position + 20, _End);
                string pos = _Position.ToString();
                string ch = _Current.ToString();
                string view = new string(_P, i, j - i);
                Dispose();
                throw new Exception(string.Format(ERRMESSAGE, pos, ch, view));
            }
        }

        const string ERRMESSAGE = "位置{0}遇到意外的字符{2},解析失败! \n\r 截取: {3}";
    }


}
