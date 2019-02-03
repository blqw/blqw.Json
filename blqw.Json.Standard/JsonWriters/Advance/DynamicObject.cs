using System;
using System.Dynamic;
using System.Linq;

namespace blqw.JsonServices.JsonWriters
{
    internal class DynamicObjectWrite : IJsonWriter
    {
        public Type Type => typeof(DynamicObject);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var writer = args.Writer;
            var value = (DynamicObject)obj;

            var memberNames = value.GetDynamicMemberNames();
            var names = memberNames as string[] ?? memberNames.ToArray();
            if (names.Length == 0)
            {
                args.Write(obj);
                return;
            }
            var comma = new CommaHelper(writer);
            args.BeginObject();
            foreach (var name in names)
            {
                //TODO: 这里可以封装成服务
                if (value.TryGetMember(new MyGetMemberBinder(name), out var val) == false
                    && value.TryGetIndex(new MyGetIndexBinder(name), new object[] { name }, out val) == false)
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
                args.Write(name);
                args.Colon();
                args.WriteObject(val);
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