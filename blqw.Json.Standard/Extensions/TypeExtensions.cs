using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace blqw.JsonServices
{
    /// <summary>
    /// 关于类型 <seealso cref="Type"/> 的扩展方法
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// 安全的获取程序集中可以被导出的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> SafeGetTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                return Type.EmptyTypes;
            }
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types;
            }
            catch
            {
                return Type.EmptyTypes;
            }
        }

        /// <summary>
        /// 判断类型是否可被实例化 <para/>
        /// 是值类型, 或 至少有一个公开构造函数的非抽象类/非静态类/非泛型定义类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Instantiable(this Type type) =>
            type.IsValueType
            || (type.IsClass && !type.IsAbstract && !type.IsGenericTypeDefinition && type.GetConstructors().Length > 0);

        /// <summary>
        /// 获取当前类型根据指定泛型定义类型约束导出的泛型参数
        /// </summary>
        /// <param name="type"> 测试类型 </param>
        /// <param name="defineType"> 泛型定义类型 </param>
        /// <param name="inherit"> 是否检测被测试类型的父类和接口 </param>
        /// <returns></returns>
        public static Type[] GetGenericArguments(this Type type, Type defineType, bool inherit = true)
        {
            if (defineType.IsAssignableFrom(type)) //2个类本身存在继承关系
            {
                return type.GetGenericArguments();
            }
            if (defineType.IsGenericType == false)
            {
                return null; //否则如果definer不是泛型类,则不存在兼容的可能性
            }
            if (defineType.IsGenericTypeDefinition == false)
            {
                defineType = defineType.GetGenericTypeDefinition(); //获取定义类型的泛型定义
            }
            if (type.IsGenericType)
            {
                if (type.IsGenericTypeDefinition)
                {
                    return null; //tester是泛型定义类型,无法兼容
                }
                //获取2个类的泛型参数
                var arg1 = ((TypeInfo)defineType).GenericTypeParameters;
                var arg2 = type.GetGenericArguments();
                //判断2个类型的泛型参数个数
                if (arg1.Length == arg2.Length)
                {
                    //判断definer 应用 tester泛型参数 后的继承关系
                    if (defineType.MakeGenericType(arg2).IsAssignableFrom(type))
                    {
                        return arg2;
                    }
                }
            }
            if (inherit == false)
            {
                return null;
            }
            //测试tester的父类是否被definer兼容
            var baseType = type.BaseType;
            while ((baseType != typeof(object)) && (baseType != null))
            {
                var result = GetGenericArguments(baseType, defineType, false);
                if (result != null)
                {
                    return result;
                }
                baseType = baseType.BaseType;
            }
            //测试tester的接口是否被definer兼容
            foreach (var @interface in type.GetInterfaces())
            {
                var result = GetGenericArguments(@interface, defineType, false);
                if (result != null)
                {
                    return result;
                }
            }
            return null;

        }

        /// <summary>
        /// 枚举指定类型的所有父类, 不包括 <seealso cref="object"/>
        /// </summary>
        /// <param name="type"> 需要枚举父类的类型 </param>
        /// <returns> </returns>
        public static IEnumerable<Type> EnumerateBaseTypes(this Type type)
        {
            var baseType = type?.BaseType ?? typeof(object);
            while (baseType != typeof(object))
            {
                yield return baseType;
                baseType = baseType.BaseType ?? typeof(object);
            }
        }

        /// <summary>
        /// 类型名称缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, string> _typeNames = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// 元组类型的接口,通过这个接口判断类型是否是元组类型
        /// </summary>
        private static readonly Type _valueTupleType = Type.GetType("System.IValueTupleInternal", false, false);

        /// <summary>
        /// 获取类型名称的友好展现形式
        /// </summary>
        public static string GetFriendlyName(this Type type)
        {
            if (type == null)
            {
                return "`null`";
            }

            return _typeNames.GetOrAdd(type, t =>
            {
                var t2 = Nullable.GetUnderlyingType(t);
                if (t2 != null)
                {
                    return GetFriendlyName(t2) + "?";
                }
                if (t.IsPointer)
                {
                    return GetFriendlyName(t.GetElementType()) + "*";
                }
                if (t.IsByRef)
                {
                    return GetFriendlyName(t.GetElementType()) + "&";
                }
                if (t.IsArray)
                {
                    return GetFriendlyName(t.GetElementType()) + "[]";
                }
                if (!t.IsGenericType)
                {
                    return GetSimpleName(t);
                }
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
                        generic[i] = GetFriendlyName(infos[i]);
                    }

                    //这个表示元组类型
                    if ((_valueTupleType?.IsAssignableFrom(t) ?? false))
                    {
                        return "(" + string.Join(", ", generic) + ")";
                    }
                }
                return GetSimpleName(t) + "<" + string.Join(", ", generic) + ">";
            });
        }

        /// <summary>
        /// 获取指定类型的简单名称
        /// </summary>
        /// <param name="t">指定类型</param>
        /// <returns></returns>
        private static string GetSimpleName(Type t)
        {
            string name;
            if (t.ReflectedType == null)
            {
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
                            case "Void":
                                return "void";
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
                        name = $"{t.Namespace}.{t.Name}";
                        break;
                }
            }
            else
            {
                name = $"{GetSimpleName(t.ReflectedType)}.{t.Name}";
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
