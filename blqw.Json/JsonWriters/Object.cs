using System;
using System.Runtime.Serialization;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class ObjectWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(object);

        public void Write(object obj, JsonWriterArgs args)
        {
            var writer = args.Writer;
            writer.Write('{');

            var jtype = JsonType.Get(obj.GetType());
            var ms = jtype.Members;
            var comma = new CommaHelper(writer);

            if (args.SerializableType)
            {
                writer.Write("\"$Type$\":");
                JsonWriterContainer.StringWriter.Write(jtype.Type.AssemblyQualifiedName, args);
                comma.AppendCommaIgnoreFirst();
            }

            var length = args.SerializableField ? ms.Length : jtype.PropertyCount;
            for (var i = 0; i < length; i++)
            {
                var member = ms[i];
                if (member.NonSerialized && args.FormatAllMember == false)
                {
                    continue;
                }
                if (member.CanRead == false)
                {
                    continue;
                }


                var value = member.GetValue(obj);
                if (value == null || value is DBNull)
                {
                    if (args.IgnoreNullMember)
                    {
                        continue;
                    }
                    comma.AppendCommaIgnoreFirst();
                    writer.Write(member.EncodedJsonName);
                    writer.Write(':');
                    JsonWriterContainer.NullWriter.Write(null, args);
                }
                else if (member.MustFormat)
                {
                    comma.AppendCommaIgnoreFirst();
                    writer.Write(member.EncodedJsonName);
                    writer.Write(':');
                    JsonWriterContainer.StringWriter.Write(((IFormattable) value).ToString(member.FormatString, member.FormatProvider), args);
                }
                else
                {
                    Write(ref comma, member, value, args);
                }
            }
            writer.Write('}');
        }

        private static void Write(ref CommaHelper comma, JsonMember member, object value, JsonWriterArgs args)
        {
            if (value == null || value is DBNull)
            {
                if (args.IgnoreNullMember)
                {
                    return;
                }
                comma.AppendCommaIgnoreFirst();
                args.Writer.Write(member.EncodedJsonName);
                args.Writer.Write(':');
                JsonWriterContainer.NullWriter.Write(null, args);
            }
            else
            {
                comma.AppendCommaIgnoreFirst();
                args.Writer.Write(member.EncodedJsonName);
                args.Writer.Write(':');
                args.WriteCheckLoop(value, member.JsonWriter);
            }
        }
    }
}