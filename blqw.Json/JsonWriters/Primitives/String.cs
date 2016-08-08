using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class StringWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(string);

        [ThreadStatic]
        private static char[] _CharBuffer;

        private const int BUFFER_SIZE = 4096;

        public static void Write(TextWriter writer, string source, int start, int count)
        {
            if (count == 0)
            {
                return;
            }
            if (_CharBuffer == null)
            {
                _CharBuffer = new char[BUFFER_SIZE];
            }
            if (count < BUFFER_SIZE)
            {
                source.CopyTo(start, _CharBuffer, 0, count);
                writer.Write(_CharBuffer, 0, count);
            }
            else
            {
                var index = start;
                do
                {
                    source.CopyTo(index, _CharBuffer, 0, BUFFER_SIZE);
                    writer.Write(_CharBuffer, 0, BUFFER_SIZE);
                    index += BUFFER_SIZE;
                    count -= BUFFER_SIZE;
                } while (count > BUFFER_SIZE);
                source.CopyTo(index, _CharBuffer, 0, count);
                writer.Write(_CharBuffer, 0, count);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (string)obj;
            var writer = args.Writer;
            writer.Write('"');
            if (string.IsNullOrEmpty(value))
            {
                writer.Write('"');
                return;
            }
            unsafe
            {
                var length = value.Length;
                fixed (char* p = value)
                {
                    var index = 0;
                    var saved = 0;
                    while (index < length)
                    {
                        var c = p[index];
                        if (c < 256)
                        {
                            var escape = CharWriter.SpecialCharacters[c];
                            if (escape != null)
                            {
                                Write(writer, value, saved, index - saved);
                                if (args.FilterSpecialCharacter == false)
                                {
                                    writer.Write(escape);
                                }
                                saved = index + 1;
                            }
                        }
                        else if (args.CastUnicode)
                        {
                            Write(writer, value, saved, index - saved);
                            writer.Write(CharWriter.SpecialCharacters[c]);
                            saved = index + 1;
                        }
                        index++;
                    }
                    if (saved == 0)
                    {
                        writer.Write(value);
                    }
                    else if (index > saved)
                    {
                        Write(writer, value, saved, index - saved);
                    }
                }
            }
            writer.Write('"');
        }

        public static void Write(TextWriter writer, string value, bool castUnicode, bool filterSpecialCharacter)
        {
            unsafe
            {
                var length = value.Length;
                fixed (char* p = value)
                {
                    var index = 0;
                    var saved = 0;
                    while (index < length)
                    {
                        var c = p[index];
                        if (c < 256)
                        {
                            var escape = CharWriter.SpecialCharacters[c];
                            if (escape != null)
                            {
                                Write(writer, value, saved, index - saved);
                                if (filterSpecialCharacter == false)
                                {
                                    writer.Write(escape);
                                }
                                saved = index + 1;
                            }
                        }
                        else if (castUnicode)
                        {
                            Write(writer, value, saved, index - saved);
                            writer.Write(CharWriter.SpecialCharacters[c]);
                            saved = index + 1;
                        }
                        index++;
                    }
                    if (saved == 0)
                    {
                        writer.Write(value);
                    }
                    else if (index > saved)
                    {
                        Write(writer, value, saved, index - saved);
                    }
                }
            }
        }
    }
}
