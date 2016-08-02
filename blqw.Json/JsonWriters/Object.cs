using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
                    JsonWriterContainer.StringWriter.Write(member.JsonName, args);
                    writer.Write(':');
                    JsonWriterContainer.NullWriter.Write(null, args);
                }
                else if (member.MustFormat)
                {
                    comma.AppendCommaIgnoreFirst();
                    JsonWriterContainer.StringWriter.Write(member.JsonName, args);
                    writer.Write(':');
                    JsonWriterContainer.StringWriter.Write(((IFormattable)value)?.ToString(member.FormatString, member.FormatProvider), args);
                }
                else
                {
                    var obj1 = (value as IFormatProvider)?.GetFormat(typeof(Json));
                    if (obj1 != null)
                    {
                        Write(ref comma, member.JsonName, obj1, args);
                        return;
                    }

                    var objref = value as IObjectReference;
                    if (objref != null)
                    {
                        Write(ref comma, member.JsonName, objref.GetRealObject(new StreamingContext(StreamingContextStates.All, args)), args);
                    }
                    Write(ref comma, member.JsonName, value, args);
                }

            }
            writer.Write('}');
        }

        private static void Write(ref CommaHelper comma, string name, object value, JsonWriterArgs args)
        {
            if (value == null || value is DBNull)
            {
                if (args.IgnoreNullMember)
                {
                    return;
                }
                comma.AppendCommaIgnoreFirst();
                JsonWriterContainer.StringWriter.Write(name, args);
                args.Writer.Write(':');
                JsonWriterContainer.NullWriter.Write(null, args);
            }
            else
            {
                comma.AppendCommaIgnoreFirst();
                JsonWriterContainer.StringWriter.Write(name, args);
                args.Writer.Write(':');
                args.WriteCheckLoop(value);
            }
        }
    }
}
