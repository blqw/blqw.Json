﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class DynamicObjectWrite : IJsonWriter
    {
        public Type Type { get; } = typeof(DynamicObject);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (DynamicObject)obj;

            var names = value.GetDynamicMemberNames();
            var comma = new CommaHelper(writer);
            writer.Write('{');
            foreach (var name in names)
            {
                object val;
                if (value.TryGetMember(new MyGetMemberBinder(name), out val) == false
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
                JsonWriterContainer.StringWriter.Write(name, args);
                JsonWriterContainer.Write(val, args);
            }
            writer.Write('}');
        }

        class MyGetIndexBinder : GetIndexBinder
        {
            public MyGetIndexBinder(string name)
                : base(new CallInfo(1, name))
            {

            }
            public override DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion)
            {
                return target;
            }
        }

        class MyGetMemberBinder : GetMemberBinder
        {
            public MyGetMemberBinder(string name)
                : base(name, false)
            {

            }

            public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
            {
                return target;
            }
        }
    }
}