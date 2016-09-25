using System;
using System.Dynamic;
using System.Linq;

namespace blqw.Serializable.JsonWriters
{
    internal class DynamicObjectWrite : IJsonWriter
    {
        public Type Type => typeof(DynamicObject);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (DynamicObject) obj;

            var memberNames = value.GetDynamicMemberNames();
            var names = memberNames as string[] ?? memberNames.ToArray();
            if (names.Length == 0)
            {
                args.WriterContainer.GetWriter<string>().Write(obj, args);
                return;
            }
            var comma = new CommaHelper(writer);
            args.BeginObject();
            foreach (var name in names)
            {
                object val;
                if (value.TryGetMember(new MyGetMemberBinder(name), out val) == false
                    && value.TryGetIndex(new MyGetIndexBinder(name), new object[] {name}, out val) == false)
                {
                    continue;
                }

                if (args.IgnoreNullMember)
                {
                    if (val == null || val is DBNull)
                    {
                        continue;
                    }
                }

                comma.AppendCommaIgnoreFirst();
                args.WriterContainer.GetWriter<string>().Write(name, args);
                args.Colon();
                args.WriteCheckLoop(val, null);
            }
            args.EndObject();
        }

        private class MyGetIndexBinder : GetIndexBinder
        {
            public MyGetIndexBinder(string name)
                : base(new CallInfo(1, name))
            {
            }

            public override DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes,
                DynamicMetaObject errorSuggestion)
            {
                return target;
            }
        }

        private class MyGetMemberBinder : GetMemberBinder
        {
            public MyGetMemberBinder(string name)
                : base(name, false)
            {
            }

            public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target,
                DynamicMetaObject errorSuggestion)
            {
                return target;
            }
        }
    }
}