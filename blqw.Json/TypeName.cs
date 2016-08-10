using System;
using System.Collections.Concurrent;

namespace blqw.Serializable
{
    internal static class TypeName
    {
        private static readonly ConcurrentDictionary<string, string> _TypeNames =
            new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 获取类型名称的友好展现形式
        /// </summary>
        public static string Get(Type t)
        {
            if (_TypeNames == null) throw new ArgumentNullException(nameof(_TypeNames));
            if (t == null) throw new ArgumentNullException(nameof(t));
            return _TypeNames.GetOrAdd(t.GetHashCode().ToString(), k =>
            {
                var t2 = Nullable.GetUnderlyingType(t);
                if (t2 != null)
                {
                    return Get(t2) + "?";
                }
                if (t.IsGenericType == false)
                    return GetSimpleName(t);

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
                    for (var i = 0; i < infos.Length; i++)
                    {
                        generic[i] = Get(infos[i]);
                    }
                }
                return $"{GetSimpleName(t)}<{string.Join(", ", generic)}>";
            });
        }

        private static string GetSimpleName(Type t)
        {
            string name;
            switch (t.Namespace)
            {
                case "System":
                    switch (t.Name)
                    {
                        case "Boolean":
                            return "bool";
                        case "Byte":
                            return "byte";
                        case "Char":
                            return "char";
                        case "Decimal":
                            return "decimal";
                        case "Double":
                            return "double";
                        case "Int16":
                            return "short";
                        case "Int32":
                            return "int";
                        case "Int64":
                            return "long";
                        case "SByte":
                            return "sbyte";
                        case "Single":
                            return "float";
                        case "String":
                            return "string";
                        case "Object":
                            return "object";
                        case "UInt16":
                            return "ushort";
                        case "UInt32":
                            return "uint";
                        case "UInt64":
                            return "ulong";
                        case "Guid":
                            return "Guid";
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