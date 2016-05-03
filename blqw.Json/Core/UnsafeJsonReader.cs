using System;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;

namespace blqw.Serializable
{
    [DebuggerDisplay("当前字符: {Current}")]
    unsafe class UnsafeJsonReader : IDisposable
    {
        Char* _p;
        int _position;
        readonly int _length;
        int _end;
        /// <summary>
        /// 原始json
        /// </summary>
        public readonly string RawJson;
        public UnsafeJsonReader(Char* origin, string str)
        {
            if (origin == null)
            {
                throw new ArgumentNullException("origin");
            }
            if (str.Length <= 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            _p = origin;
            _length = str.Length;
            _end = str.Length - 1;
            _position = 0;
            Current = *origin;
            RawJson = str;
        }

        public char Current { get; private set; }

        /// <summary> 当前位置
        /// </summary>
        public int Position
        {
            get { return _position; }
            set
            {
                if (_position >= _length)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (_isDisposed == false)
                {
                    _position = value;
                    Current = _p[_position];
                }
            }
        }

        /// <summary> 是否已经到结尾,忽略空白
        /// </summary>
        public bool IsEnd()
        {
            if (_position > _end)
            {
                return true;
            }
            if (Current.IsWhiteSpace())
            {
                while (_position < _end)
                {
                    _position++;
                    Current = _p[_position];
                    if (Current.IsWhiteSpace() == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary> 检查是否已经到结尾,忽略空白和回车,如果已达结尾,则抛出异常
        /// </summary>
        public void CheckEnd()
        {
            if (IsEnd())
            {
                ThrowException();
            }
        }


        /// <summary> 移动到下一个字符,如果已经是结尾则抛出异常
        /// </summary>
        public void MoveNext()
        {
            if (_position < _end)
            {
                _position++;
                Current = _p[_position];
            }
            else if (_position == _end)
            {
                _position++;
                Current = '\0';
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

            if (Current.IsVariableNameInitial())
            {
                ThrowException();
            }
            MoveNext();
            while (Current.IsValidVariableName())//读取下一个字符 可是是单词
            {
                MoveNext();
            }
        }

        /// <summary> 跳过一个指定字符,忽略空白和回车,如果字符串意外结束抛出异常
        /// </summary>
        /// <param name="c">需要判断和跳过的字符</param>
        /// <param name="throwOnError">失败是否抛出异常</param>
        public bool SkipChar(char c, bool throwOnError)
        {
            if (IsEnd())
            {
                ThrowMissCharException(c);
            }
            if (Current == c)
            {
                if (_position > _end)
                {
                    _position++;
                    Current = '\0';
                }
                else
                {
                    _position++;
                    Current = _p[_position];
                }
                return true;
            }
            if (throwOnError)
            {
                ThrowMissCharException(c);
            }
            return false;
        }

        /// <summary> 跳过一个字符串
        /// </summary>
        public void SkipString()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }
            Char quot = Current;
            if (quot != '"' && quot != '\'')
            {
                ThrowException();
            }
            MoveNext();
            while (Current != quot)//是否是结束字符
            {
                if (Current == '\\')//是否是转义符
                {
                    if (Current.IsEscapeCode() == false)
                    {
                        ThrowException();
                    }
                    MoveNext();
                }
                MoveNext();
            }
            MoveNext();
        }
        //袖珍版字符串处理类
        sealed class MiniBuffer : IDisposable
        {
            [ThreadStatic]
            static MiniBuffer Instance;

            public static MiniBuffer Create(char* p)
            {
                if (Instance == null)
                {
                    Instance = new MiniBuffer();
                }
                Instance._p = p;
                Instance._index = 0;
                Instance._position = 0;
                return Instance;
            }

            char* _p;
            string[] _str;
            int _index;
            int _position;
            private MiniBuffer()
            {

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
                            var s = new string(point, offset, length - 1);
                            if (_index == 3)
                            {
                                _str[0] = string.Concat(_str[0], _str[1], _str[2], s);
                                _index = 1;
                            }
                            else
                            {
                                _str[_index++] = s;
                            }
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
                        (*p1) = *(p2);
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
                if (_index == 2)
                {
                    return string.Concat(_str[0], _str[1], new string(_p, 0, _position));
                }
                return string.Concat(_str[0], _str[1], _str[2], new string(_p, 0, _position));
            }

            public void Dispose()
            {
                _p = null;
                _str = null;
            }
        }

        /// <summary> 读取时间类型的对象
        /// </summary>
        /// <returns></returns>
        public object ReadDateTime()
        {
            var text = ReadString();
            return DateTime.Parse(text);
        }
        
        /// <summary> 读取正整数,在ReadDateTime函数中使用
        /// </summary>
        /// <returns></returns>
        private int ReadPositiveInteger()
        {
            if (Current.IsDigit() == false)
            {
                return -1;
            }
            int num = 0;
            do
            {
                num = num * 10 + (Current - '0');
                MoveNext();
            } while (Current.IsDigit());
            return num;
        }

        /// <summary> 读取常量
        /// </summary>
        /// <returns></returns>
        public object ReadConsts(bool warp)
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }
            switch (Current)
            {
                case '\'':
                case '"':
                    if (warp == false)
                    {
                        ThrowException();
                    }
                    return string.Empty;
                case 'f'://false
                    MoveNext();
                    if (Current != 'a') ThrowException();
                    MoveNext();
                    if (Current != 'l') ThrowException();
                    MoveNext();
                    if (Current != 's') ThrowException();
                    MoveNext();
                    if (Current != 'e') ThrowException();
                    MoveNext();
                    return false;
                case 't'://true
                    MoveNext();
                    if (Current != 'r') ThrowException();
                    MoveNext();
                    if (Current != 'u') ThrowException();
                    MoveNext();
                    if (Current != 'e') ThrowException();
                    MoveNext();
                    return true;
                case 'n'://null
                    if (warp)
                        ThrowException();
                    MoveNext();
                    if (Current != 'u') ThrowException();
                    MoveNext();
                    if (Current != 'l') ThrowException();
                    MoveNext();
                    if (Current != 'l') ThrowException();
                    MoveNext();
                    return null;
                case 'u'://undefined
                    if (warp)
                        ThrowException();
                    MoveNext();
                    if (Current != 'n') ThrowException();
                    MoveNext();
                    if (Current != 'd') ThrowException();
                    MoveNext();
                    if (Current != 'e') ThrowException();
                    MoveNext();
                    if (Current != 'f') ThrowException();
                    MoveNext();
                    if (Current != 'i') ThrowException();
                    MoveNext();
                    if (Current != 'n') ThrowException();
                    MoveNext();
                    if (Current != 'e') ThrowException();
                    MoveNext();
                    if (Current != 'd') ThrowException();
                    MoveNext();
                    return null;
                case 'N'://NaN
                    if (warp)
                        ThrowException();
                    MoveNext();
                    if (Current != 'a') ThrowException();
                    MoveNext();
                    if (Current != 'N') ThrowException();
                    MoveNext();
                    return double.NaN;
                case 'I'://Infinity
                    if (warp)
                        ThrowException();
                    MoveNext();
                    if (Current != 'n') ThrowException();
                    MoveNext();
                    if (Current != 'f') ThrowException();
                    MoveNext();
                    if (Current != 'i') ThrowException();
                    MoveNext();
                    if (Current != 'n') ThrowException();
                    MoveNext();
                    if (Current != 'i') ThrowException();
                    MoveNext();
                    if (Current != 't') ThrowException();
                    MoveNext();
                    if (Current != 'y') ThrowException();
                    MoveNext();
                    return double.PositiveInfinity;
                case '-'://-Infinity
                    if (warp)
                        ThrowException();
                    MoveNext();
                    if (Current.IsNumber())
                    {
                        Position--;
                        var number = ReadNumber();
                        if (number % 1 == 0
                            && number > int.MinValue
                            && number < int.MaxValue)
                        {
                            return (int)number;
                        }
                        return number;
                    }
                    if (Current != 'I') ThrowException();
                    MoveNext();
                    if (Current != 'n') ThrowException();
                    MoveNext();
                    if (Current != 'f') ThrowException();
                    MoveNext();
                    if (Current != 'i') ThrowException();
                    MoveNext();
                    if (Current != 'n') ThrowException();
                    MoveNext();
                    if (Current != 'i') ThrowException();
                    MoveNext();
                    if (Current != 't') ThrowException();
                    MoveNext();
                    if (Current != 'y') ThrowException();
                    MoveNext();
                    return double.NegativeInfinity;
                default:
                    if (Current.IsNumber())
                    {
                        var number = ReadNumber();
                        if (number % 1 == 0
                            && number > int.MinValue
                            && number < int.MaxValue)
                        {
                            return (int)number;
                        }
                        return number;
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

            if (Current.IsVariableNameInitial() == false)
            {
                return null;
            }

            var index = _position;
            while (Current.IsValidVariableName())
            {
                MoveNext();//读取下一个字符
            }
            return new string(_p, index, _position - index);
        }

        /// <summary> 读取数字
        /// </summary>
        /// <returns></returns>
        private double ReadNumber()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }


            var minus = false; //是否是负数
            var number = 0d;   //数字
            var rate = 10d;    //当前转化倍率,大于0为整数 小于0为小数

            switch (Current)
            {
                case '.':
                    rate = 0.1d;
                    break;
                case '+':
                    MoveNext();//读取下一个字符
                    break;
                case '-':
                    MoveNext();//读取下一个字符
                    minus = true;
                    break;
                default:
                    break;
            }
            var index = _position;

            while (true)
            {
                var result = Current.ContainsFlag(CharFlags.Digit | CharFlags.NumberSymbol);

                switch (result)
                {
                    case CharFlags.Nothing:
                        return minus ? -number : number;
                    case CharFlags.Digit:
                        if (rate < 1)
                        {
                            number += (Current - '0') * rate;
                            rate /= 10;
                        }
                        else
                        {
                            number = number * 10 + (Current - '0');
                        }
                        MoveNext();//读取下一个字符
                        break;
                    case CharFlags.NumberSymbol:
                        switch (Current)
                        {
                            case '+':
                            case '-':
                                ThrowException();
                                break;
                            case '.':
                                if (rate < 1)
                                {
                                    ThrowException();
                                }
                                rate = 0.1;
                                MoveNext();//读取下一个字符
                                break;
                            case 'e':
                            case 'E':
                                MoveNext();
                                var power = ReadNumber();
                                if (power < -308d)
                                {
                                    number *= Math.Pow(10d, -308d);
                                    number *= Math.Pow(10d, power + 308d);
                                }
                                else
                                {
                                    number *= Math.Pow(10, power);
                                }
                                continue;
                            default:
                                ThrowException();
                                break;
                        }
                        break;
                    default:
                        ThrowException();
                        break;
                }

            }
        }

        /// <summary> 读取字符串
        /// </summary>
        public string ReadString()
        {
            if (IsEnd())//已到结尾
            {
                ThrowException();
            }

            Char quot = Current;
            if (quot != '"' && quot != '\'')
            {
                ThrowException();
            }
            MoveNext();
            if (Current == quot)
            {
                MoveNext();
                return "";
            }

            var index = _position;

            do
            {
                if (Current == '\\')//是否是转义符
                {
                    char* p = stackalloc char[255];
                    return ReadString(index, quot, MiniBuffer.Create(p));
                }
                MoveNext();
            } while (Current != quot);//是否是结束字符
            string str = new string(_p, index, _position - index);
            MoveNext();
            return str;
        }

        private string ReadString(int index, char quot, MiniBuffer buff)
        {
            do
            {
                if (Current == '\\')//是否是转义符
                {
                    buff.AddString(_p, index, _position - index);
                    MoveNext();
                    var escape = Current.Escape();
                    if (escape == null)
                    {
                        if (Current != 'u')
                        {
                            ThrowException();
                        }

                        MoveNext();
                        var a = Current;
                        MoveNext();
                        var b = Current;
                        MoveNext();
                        var c = Current;
                        MoveNext();
                        var d = Current;
                        var unicode = CharExtensions.UnicodeToChar(a, b, c, d);
                        if (unicode == null)
                        {
                            ThrowException($"Unicode转义串解析失败!\\u{a}{b}{c}{d}");
                        }
                        buff.AddChar(unicode.Value);
                    }
                    else
                    {
                        buff.AddChar(escape.Value);
                    }
                    index = _position + 1;
                }
                MoveNext();
            } while (Current != quot);//是否是结束字符
            string str;
            buff.AddString(_p, index, _position - index);
            str = buff.ToString();
            buff.Dispose();
            MoveNext();
            return str;
        }


        bool _isDisposed;

        public void Dispose()
        {
            _p = null;
            _end = 0;
            _isDisposed = true;
            Current = '\0';
        }

        private void ThrowMissCharException(char c)
        {
            if (c == '{' || c == '}')
            {
                ThrowException("缺少字符:" + c + c + " 当前字符:{0}");
            }
            ThrowException("缺少字符:" + c + " 当前字符:{0}");
        }

        private void ThrowException(string title = "遇到意外的字符:{0}")
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("UnsafeJsonReader", "不能访问已释放的对象!");
            }
            if (IsEnd())
            {
                Dispose();
                throw new JsonParseException("遇到意外的字符串结尾,解析失败!", RawJson);
            }
            int i = Math.Max(_position - 20, 0);
            int j = Math.Min(_position + 20, _length);
            string pos = _position.ToString(CultureInfo.InvariantCulture);
            string ch = Current.ToString(CultureInfo.InvariantCulture);
            string view = new string(_p, i, j - i);
            Dispose();
            throw new JsonParseException($"解析失败!{string.Format(title, ch)}\n截取: {view}\n位置{pos}", RawJson);
        }

    }


}
