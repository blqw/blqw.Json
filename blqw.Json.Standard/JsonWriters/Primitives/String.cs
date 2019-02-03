using System;
using System.IO;

namespace blqw.JsonServices.JsonWriters
{
    internal sealed class StringWriter : IJsonWriter
    {
        private const int BUFFER_SIZE = 4096;

        [ThreadStatic]
        private static char[] _CharBuffer;

        public Type Type { get; } = typeof(string);

        public void Write(object obj, JsonWriterSettings args)
        {
            var writer = args.Writer;
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var value = (string) obj;
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
                            var escape = CharWriter._specialCharacters[c];
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
                            writer.Write(CharWriter._specialCharacters[c]);
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
        
    }
}