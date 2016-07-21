using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class CharWriter : IJsonWriter
    {
        /// <summary>
        /// 特殊字符表
        /// </summary>
        internal static readonly string[] SpecialCharacters = InitSpecialCharacters();
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

        public Type Type
        {
            get
            {
                return typeof(char);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (char)obj;
            var writer = args.Writer;
            writer.Write('"');
            if (value < 256)
            {
                var escape = SpecialCharacters[value];
                if (escape == null)
                {
                    writer.Write(value);
                }
                else if (args.FilterSpecialCharacter == false)
                {
                    writer.Write(escape);
                }
            }
            else if (args.CastUnicode)
            {
                writer.Write(SpecialCharacters[value]);
            }
            else
            {
                writer.Write(value);
            }
            writer.Write('"');
        }
    }
}
