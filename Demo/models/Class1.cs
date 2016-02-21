using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    class Class1
    {
        /// <summary> 检查一个类型是否为可空值类型
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return (t.IsValueType && t.IsGenericType && !t.IsGenericTypeDefinition && object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(Nullable<>)));
        }

        public static bool IsPrimitive(Type type)
        {
            if (Type.GetTypeCode(type) == TypeCode.Object)
            {
                if (type == typeof(Guid))
                {
                    return true;
                }
                else if (IsNullable(type))
                {
                    return IsPrimitive(type.GetGenericArguments()[0]);
                }
            }
            else
            {
                if (type.IsPrimitive)
                {
                    return true;
                }
                else if (type == typeof(DateTime))
                {
                    return true;
                }
                else if (type == typeof(string))
                {
                    return true;
                }
            }
            
            return false;
        }

        public static void AssertEquals<T>(T t1, T t2, string title = null)
        {
            if (t1 == null || (t1.GetType().IsValueType && t1.GetHashCode() == 0))
            {
                return;
            }
            if (t1 == null || t2 == null)
            {
                if (t1 != null || t2 != null)
                {
                    throw new Exception(string.Format("{0} 值不同 ,值1 {1}, 值2 {2}", title, (object)t1 ?? "NULL", (object)t2 ?? "NULL"));
                }
                return;
            }

            if (IsPrimitive(t1.GetType()))
            {
                if (!object.Equals(t1, t2))
                {
                    throw new Exception(string.Format("{0} 值不同 ,值1 {1}, 值2 {2}", title, (object)t1 ?? "NULL", (object)t2 ?? "NULL"));
                }
            }
            else if (t1 is IEnumerable)
            {
                var e1 = ((IEnumerable)t1).GetEnumerator();
                var e2 = ((IEnumerable)t2).GetEnumerator();

                while (e1.MoveNext())
                {
                    if (e2.MoveNext() == false)
                    {
                        throw new Exception(string.Format("{0} 个数不同1", title));
                    }
                    AssertEquals(e1.Current, e2.Current, title);
                }
                if (e2.MoveNext())
                {
                    throw new Exception(string.Format("{0} 个数不同2", title));
                }
            }
            else
            {
                foreach (var p in t1.GetType().GetProperties())
                {
                    if (p.CanRead)
                    {
                        var val1 = p.GetValue(t1);
                        var val2 = p.GetValue(t2);
                        AssertEquals(val1, val2, "属性" + p.Name);
                    }
                }
            }
        }

    }
}
