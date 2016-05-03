using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    static class TypeName
    {
        private static readonly NameValueCollection _typeNames = new NameValueCollection();
        ///<summary> 获取类型名称的友好展现形式
        /// </summary>
        public static string Get(Type t)
        {
            var s = _typeNames[t.GetHashCode().ToString()];
            if (s != null)
            {
                return s;
            }
            lock (_typeNames)
            {
                var t2 = Nullable.GetUnderlyingType(t);
                if (t2 != null)
                {
                    return _typeNames[t.GetHashCode().ToString()] = Get(t2) + "?";

                }
                if (t.IsGenericType)
                {
                    string[] generic;
                    if (t.IsGenericTypeDefinition) //泛型定义
                    {
                        var args = t.GetGenericArguments();
                        generic = new string[args.Length];
                    }
                    else
                    {
                        var infos = t.GetGenericArguments();
                        generic = new string[infos.Length];
                        for (int i = 0; i < infos.Length; i++)
                        {
                            generic[i] = Get(infos[i]);
                        }
                    }
                    return _typeNames[t.GetHashCode().ToString()] = GetSimpleName(t) + "<" + string.Join(", ", generic) + ">";
                }
                else
                {
                    return _typeNames[t.GetHashCode().ToString()] = GetSimpleName(t);
                }
            }
        }

        private static string GetSimpleName(Type t)
        {
            string name;
            switch (t.Namespace)
            {
                case "System":
                    switch (t.Name)
                    {
                        case "Boolean": return "bool";
                        case "Byte": return "byte";
                        case "Char": return "char";
                        case "Decimal": return "decimal";
                        case "Double": return "double";
                        case "Int16": return "short";
                        case "Int32": return "int";
                        case "Int64": return "long";
                        case "SByte": return "sbyte";
                        case "Single": return "float";
                        case "String": return "string";
                        case "Object": return "object";
                        case "UInt16": return "ushort";
                        case "UInt32": return "uint";
                        case "UInt64": return "ulong";
                        case "Guid": return "Guid";
                        default:
                            name = t.Name;
                            break;
                    }
                    break;
                case null:
                case "System.Collections":
                case "System.Collections.Generic":
                case "System.Data":
                case "System.Text":
                case "System.IO":
                case "System.Collections.Specialized":
                    name = t.Name;
                    break;
                default:
                    name = t.Namespace + "." + t.Name;
                    break;
            }
            var index = name.LastIndexOf('`');
            if (index > -1)
            {
                name = name.Remove(index);
            }
            return name;
        }
    }
}
