using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    public unsafe static class CharExtensions
    {

        #region private

        static CharExtensions()
        {
            var size = char.MaxValue + 1;
            _intPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            _memoryArray = (CharFlags*)_intPtr.ToPointer();
            for (int i = 0; i < size; i++)
            {
                _memoryArray[i] = CharFlags.Nothing;
            }
            var map = new Dictionary<CharFlags, Func<IEnumerable<char>>>()
            {
                [CharFlags.Letter] = GetLetterChars,
                [CharFlags.VariableSymbol] = GetVariableSymbolChars,
                [CharFlags.Digit] = GetDigitChars,
                [CharFlags.WhiteSpace] = GetWhiteSpaceChars,
                [CharFlags.EscapeCode] = GetEscapeCode,
                [CharFlags.IllegalChar] = GetIllegalChars,
                [CharFlags.NumberSymbol] = GetNumberSymbolChars,
            };

            foreach (var item in map)
            {
                foreach (var c in item.Value())
                {
                    _memoryArray[(int)c] |= item.Key;
                }
            }
        }

        private static IEnumerable<char> GetNumberSymbolChars()
        {
            yield return '+';
            yield return '-';
            yield return '.';
            yield return 'e';
            yield return 'E';
        }

        private static IEnumerable<char> GetIllegalChars()
        {
            for (int c = char.MinValue; c <= char.MaxValue; c++)
            {
                if (c == '\b' || c == '\f' || c == '\n' || c == '\r' || c == '\t')
                {
                    continue;
                }
                if (char.IsControl((char)c))
                {
                    yield return (char)c;
                }
            }
        }

        private static IEnumerable<char> GetEscapeCode()
        {
            // 标准转义符
            yield return '"';
            yield return '\\';
            yield return '/';
            yield return 'b';
            yield return 'f';
            yield return 'n';
            yield return 'r';
            yield return 't';
            yield return 'u';

            // 非标准 但兼容转义符
            yield return '\'';
            yield return '0';
            yield return 'a';
            yield return 'v';
        }

        private static IEnumerable<char> GetWhiteSpaceChars()
        {
            for (int c = char.MinValue; c <= char.MaxValue; c++)
            {
                if (char.IsWhiteSpace((char)c))
                {
                    yield return (char)c;
                }
            }
        }

        private static IEnumerable<char> GetDigitChars()
        {
            for (char c = '0'; c <= '9'; c++)
            {
                yield return c;
            }
        }

        private static IEnumerable<char> GetVariableSymbolChars()
        {
            yield return '$';
            yield return '_';
        }


        private static IEnumerable<char> GetLetterChars()
        {
            for (char c = 'a'; c <= 'z'; c++)
            {
                yield return c;
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                yield return c;
            }
        }


        private static CharFlags* _memoryArray;
        private static IntPtr _intPtr;

        private static sbyte CastNumber(char c)
        {
            switch (c)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                case 'a': return 10;
                case 'b': return 11;
                case 'c': return 12;
                case 'd': return 13;
                case 'e': return 14;
                case 'f': return 15;
                case 'g': return 16;
                case 'h': return 17;
                case 'i': return 18;
                case 'j': return 19;
                case 'k': return 20;
                case 'l': return 21;
                case 'm': return 22;
                case 'n': return 23;
                case 'o': return 24;
                case 'p': return 25;
                case 'q': return 26;
                case 'r': return 27;
                case 's': return 28;
                case 't': return 29;
                case 'u': return 30;
                case 'v': return 31;
                case 'w': return 32;
                case 'x': return 33;
                case 'y': return 34;
                case 'z': return 35;
                case 'A': return 10;
                case 'B': return 11;
                case 'C': return 12;
                case 'D': return 13;
                case 'E': return 14;
                case 'F': return 15;
                case 'G': return 16;
                case 'H': return 17;
                case 'I': return 18;
                case 'J': return 19;
                case 'K': return 20;
                case 'L': return 21;
                case 'M': return 22;
                case 'N': return 23;
                case 'O': return 24;
                case 'P': return 25;
                case 'Q': return 26;
                case 'R': return 27;
                case 'S': return 28;
                case 'T': return 29;
                case 'U': return 30;
                case 'V': return 31;
                case 'W': return 32;
                case 'X': return 33;
                case 'Y': return 34;
                case 'Z': return 35;
                default: return -1;
            }
        }

        #endregion

        /// <summary>
        /// 将\u****中的****4个字符转为一个char
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="n3"></param>
        /// <param name="n4"></param>
        /// <returns></returns>
        public static char? UnicodeToChar(char n1, char n2, char n3, char n4)
        {
            var a = CastNumber(n1);
            var b = CastNumber(n2);
            var c = CastNumber(n3);
            var d = CastNumber(n4);

            if (a == -1 || b == -1 || c == -1 || d == -1)
            {
                return null;
            }
            return (char)(a * 0x1000 + b * 0x100 + c * 0x10 + d);
        }

        /// <summary>
        /// 获取当前字符对应的转义符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char? Escape(this char c)
        {
            switch (c)
            {
                case '\\': return '\\';
                case '\'': return '\'';
                case '"': return '"';
                case '/': return '/';
                case 'b': return '\b';
                case 'f': return '\f';
                case 'n': return '\n';
                case 'r': return '\r';
                case 't': return '\t';
                case '0': return '\0';
                case 'a': return '\a';
                case 'v': return '\v';
                default: return null;
            }
        }

        /// <summary>
        /// 获取当前转义符对应的字符码
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char? EscapeCode(this char c)
        {
            switch (c)
            {
                case '\\': return '\\';
                case '\'': return '\'';
                case '"': return '"';
                case '/': return '/';
                case '\b': return 'b';
                case '\f': return 'f';
                case '\n': return 'n';
                case '\r': return 'r';
                case '\t': return 't';
                case '\0': return '0';
                case '\a': return 'a';
                case '\v': return 'v';
                default: return null;
            }
        }

        /// <summary>
        /// 判断当前字符是否是是可以作为变量起始的字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsVariableNameInitial(this char c)
        {
            const CharFlags flag = CharFlags.VariableNameInitial;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 判断当前字符是否是有效的变量名
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsValidVariableName(this char c)
        {
            const CharFlags flag = CharFlags.ValidVariableName;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 判断当前字符是否是数字,比系统更快,包括 -/+/.符号
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsNumber(this char c)
        {
            const CharFlags flag = CharFlags.Number;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 判断当前字符是否是空白字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsWhiteSpace(this char c)
        {
            const CharFlags flag = CharFlags.WhiteSpace;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 判断当前字符是否属于转移码
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsEscapeCode(this char c)
        {
            const CharFlags flag = CharFlags.EscapeCode;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 判断当前字符对于Json协议是否是无效的
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsJsonIllegal(this char c)
        {
            const CharFlags flag = CharFlags.IllegalChar;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 判断当前字符是否是 '0' ~ '9'中的任意一个
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDigit(this char c)
        {
            const CharFlags flag = CharFlags.Digit;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 判断当前字符是否是一个字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetter(this char c)
        {
            const CharFlags flag = CharFlags.Letter;
            return (_memoryArray[(int)c] & flag) != 0;
        }

        /// <summary>
        /// 获取当前字符包含的标识
        /// </summary>
        /// <param name="c"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static CharFlags ContainsFlag(this char c, CharFlags flag)
        {
            return _memoryArray[(int)c] & flag;
        }
    }
}