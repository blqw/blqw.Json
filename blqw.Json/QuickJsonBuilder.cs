using System;

namespace blqw
{
    /// <summary> 快速的将任意对象转换为Json字符串
    /// </summary>
    public class QuickJsonBuilder : JsonBuilder
    {

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

            var jtype = JsonType.Get(obj.GetType());
            var ms = jtype.Members;
            bool b = false;
            var length = SerializableField ? ms.Length : jtype.PropertyCount;
            for (int i = 0; i < length; i++)
            {
                var member = ms[i];
                if (member.NonSerialized == false)
                {
                    var p = member.Member;
                    if (p.CanRead)
                    {
                        var value = p.GetValue(obj);
                        if (value != null || !IgnoreNullMember)
                        {
                            if (b) UnsafeAppend(',');
                            AppendKey(member.JsonName, false);
                            if (member.MustFormat)
                            {
                                AppendFormattable((IFormattable)value, member.FormatString, member.FormatProvider);
                            }
                            else
                            {
                                AppendObject(value);
                            }
                            if (!b) b = true;
                        }
                    }
                }
            }
            UnsafeAppend('}');
        }

    }

}
