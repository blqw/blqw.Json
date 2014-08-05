using System;

namespace blqw
{
    /// <summary> 快速的将任意对象转换为Json字符串
    /// </summary>
    public class QuickJsonBuilder : JsonBuilder
    {
        private static OrderlyList<JsonType> Cache = new OrderlyList<JsonType>();

        public QuickJsonBuilder()
            : base(JsonBuilderSettings.Default)
        {

        }

        public QuickJsonBuilder(JsonBuilderSettings settings)
            : base(settings)
        {

        }

        /// <summary> 将未知对象按属性名和值转换为Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="obj">非null的位置对象</param>
        protected override void AppendOther(object obj)
        {
            UnsafeAppend('{');

            Type type = obj.GetType();
            var hashCode = type.GetHashCode();
            var jtype = Cache[hashCode];
            if (jtype == null)
            {
                jtype = new JsonType(type);
                Cache.Add(hashCode, jtype);
            }

            var ms = jtype.Members;
            bool b = false;
            for (int i = 0; i < ms.Length; i++)
            {
                var member = ms[i];
                if (member.NonSerialized == false)
                {
                    var p = member.Member;
                    if (p.CanRead)
                    {
                        var value = p.GetValue(obj);
                        if (value != null)
                        {
                            if (b) UnsafeAppend(',');
                            UnsafeAppend(member.JsonName);
                            UnsafeAppend(':');
                            //AppendKey(member.JsonName, false);
                            AppendObject(value);
                            if (!b) b = true;
                        }
                    }
                }
            }

            //Literacy lit = Literacy.Cache(type, true);
            //var ee = lit.Property.GetEnumerator();
            //var fix = "";
            //while (ee.MoveNext())
            //{
            //    var p = ee.Current;
            //    var value = p.GetValue(obj);
            //    if (value != null)
            //    {
            //        UnsafeAppend(fix);
            //        AppendKey(p.Name, false);
            //        AppendObject(value);
            //        fix = ",";
            //    }
            //}

            UnsafeAppend('}');
        }

    }

}
