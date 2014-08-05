using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public static partial class ExtendMethod
    {
        /// <summary> 检查当前参数所表示的类型是否为数字
        /// </summary>
        public static bool IsNumber(
#if !NF2
this
#endif
         Type t)
        {
            if (t == null)
            {
                return false;
            }
            var code = Type.GetTypeCode(t);
            return (code >= TypeCode.SByte && code <= TypeCode.Decimal) || t.IsEnum;
        }

        /// <summary> 检查一个类型是否为可空值类型
        /// </summary>
        public static bool IsNullable(
#if !NF2
this
#endif
         Type t)
        {
            return (t.IsValueType && t.IsGenericType && !t.IsGenericTypeDefinition && object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(Nullable<>)));
        }

        /// <summary> 检查指定类型是否是当前类型的子类
        /// </summary>
        /// <param name="parent">当前类型(父类)</param>
        /// <param name="child">指定类型(子类)</param>
        public static bool IsChild(
#if !NF2
this
#endif
         Type parent, Type child)
        {
            return parent != null && parent.IsAssignableFrom(child);
        }

        /// <summary> 检查指定对象是否是当前类型(或其子类类型)的实例
        /// </summary>
        /// <param name="parent">当前类型(父类)</param>
        /// <param name="obj">指定对象</param>
        /// <returns>存在继承关系返回true,否则返回false</returns>
        public static bool IsChild(
#if !NF2
this
#endif
         Type parent, object obj)
        {
            return parent != null && parent.IsInstanceOfType(obj);
        }

        ///<summary> 获取一个类型名称的友好展现形式
        /// </summary>
        /// <param name="t"></param>
        public static string DisplayName(
#if !NF2
this
#endif
         Type t)
        {
            if (t == null)
            {
                return "null";
            }

            string name;
            switch (t.Namespace)
            {
                case "System":
                case "System.Collections":
                case "System.Collections.Generic":
                case "System.Data":
                    name = t.Name;
                    break;
                default:
                    name = t.Namespace + "." + t.Name;
                    break;
            }
            if (name.Length > 2)
            {
                if (name[name.Length - 2] == '`')
                {
                    name = name.Remove(name.Length - 2);
                }
                else if (name[name.Length - 3] == '`')
                {
                    name = name.Remove(name.Length - 3);
                }
            }

            if (t.IsGenericType)
            {
                if (object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(Nullable<>)))
                {
                    return t.GetGenericArguments()[0].Name + "?";
                }
                var arr = t.GetGenericArguments();
                if (arr.Length == 1)
                {
                    return name + "<" + DisplayName(arr[0]) + ">";
                }
                StringBuilder sb = new StringBuilder(name);
                sb.Append("<");
                var length = arr.Length;
                sb.Append(DisplayName(arr[0]));
                for (int i = 1; i < length; i++)
                {
                    sb.Append(',');
                    sb.Append(DisplayName(arr[i]));
                }
                sb.Append('>');
                return sb.ToString();
            }

            return name;
        }

        public static bool IsPrimitive(
#if !NF2
            this
#endif
        Type type)
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
    }
}
