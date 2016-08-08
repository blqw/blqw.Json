using System;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class CharWriter : IJsonWriter
    {
        /// <summary>
        /// 特殊字符表
        /// </summary>
        internal static readonly string[] SpecialCharacters = InitSpecialCharacters();

        public Type Type => typeof(char);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (char) obj;
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

        private static string[] InitSpecialCharacters()
        {
            var chars = new string[65536];
            for (var i = 0; i < 256; i++)
            {
                if (char.IsControl((char) i))
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
            chars['\\'] = @"\\";

            //unicode编码
            for (var i = 256; i < 65536; i++)
            {
                chars[i] = "\\u" + i.ToString("x4");
            }
            return chars;
        }
    }
}