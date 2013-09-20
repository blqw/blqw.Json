using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 快速的将任意对象转换为Json字符串
    /// </summary>
    public class QuickJsonBuilder : JsonBuilder
    {
        static Dictionary<Type, Literacy> _LitCache = new Dictionary<Type, Literacy>();
        /// <summary> 将未知对象按属性名和值转换为Json中的键值字符串写入Buffer
        /// </summary>
        /// <param name="obj">非null的位置对象</param>
        protected override void AppendOther(object obj)
        {
            Type type = obj.GetType();
            Literacy lit = Literacy.Cache(type, true);

            UnsafeAppend('{');
            var ee = lit.Property.GetEnumerator();

            if (ee.MoveNext())
            {
                var p = ee.Current;
                AppendKey(p.Name, false);
                AppendObject(p.GetValue(obj));
                while (ee.MoveNext())
                {
                    p = ee.Current;
                    UnsafeAppend(',');
                    AppendKey(p.Name, false);
                    AppendObject(p.GetValue(obj));
                }
            }

            UnsafeAppend('}');
        }
    }

}
