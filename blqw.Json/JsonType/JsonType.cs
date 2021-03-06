﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using blqw.Converts;
using blqw.IOC;

namespace blqw.Serializable
{
    /// <summary>
    /// 指示可以被序列化json的对象类型信息
    /// </summary>
    public sealed class JsonType : IEnumerable<JsonMember>
    {
        #region _mataTypes

        private static readonly HashSet<Type> _mataTypes = new HashSet<Type>
        {
            typeof(Guid),
            typeof(TimeSpan),
            typeof(DateTimeOffset),
            typeof(DateTime),
            typeof(DBNull),
            typeof(UIntPtr),
            typeof(IntPtr),
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(float),
            typeof(string),
            typeof(ushort),
            typeof(uint),
            typeof(ulong)
        };

        #endregion

        internal readonly Action<object, object, object> AddKeyValue;

        /// <summary>
        /// 用于操作
        /// </summary>
        internal readonly Action<object, object> AddValue;

        /// <summary>
        /// 转换器
        /// </summary>
        public readonly IConvertor Convertor;

        /// <summary>
        /// 如果是集合,则表示内部元素的类型
        /// <para> 字典 <see cref="Dictionary{TKey, TValue}" /> 表示TValue的类型 </para>
        /// </summary>
        public readonly JsonType ElementType;

        /// <summary>
        /// 是否是匿名类
        /// </summary>
        public readonly bool IsAnonymous;

        /// <summary>
        /// 是否是键值对集合相关的类型
        /// <para> </para>
        /// 包括继承 <see cref="NameValueCollection" /> ,实现 <see cref="IDictionary" /> ,实现<see cref="IDictionary{TKey, TValue}" />
        /// </summary>
        public readonly bool IsDictionary;

        /// <summary>
        /// 是否是索引集合相关的类型
        /// <para> </para>
        /// 类型是数组或实现 <see cref="IList" />,或实现<see cref="ICollection{T}" />
        /// </summary>
        public readonly bool IsList;

        /// <summary>
        /// 获取一个值，通过该值指示类型是否属于元类型
        /// <para> </para>
        /// 除了 Type.IsPrimitive = true 的类型以外,还包括
        /// <see cref="Guid" />,
        /// <see cref="TimeSpan" />,
        /// <see cref="DateTimeOffset" />,
        /// <see cref="DateTime" />,
        /// <see cref="DBNull" />,
        /// <see cref="IntPtr" />,
        /// <see cref="UIntPtr" />,
        /// <see cref="string" />,
        /// 以及这些类型的可空值类型(<see cref="Nullable{T}" />)
        /// </summary>
        public readonly bool IsMataType;

        /// <summary>
        /// 是否是数字类
        /// </summary>
        public readonly bool IsNumber;

        /// <summary>
        /// 是否是 <see cref="object" />的实例
        /// </summary>
        public readonly bool IsObject;

        /// <summary>
        /// 如果 <see cref="IsDictionary" /> 为true 则表示Key的类型,否则为null
        /// </summary>
        public readonly JsonType KeyType;

        /// <summary>
        /// 属性和字段集合
        /// </summary>
        public readonly JsonMember[] Members;

        /// <summary>
        /// 需要序列化的属性和字段的个数
        /// </summary>
        public readonly int PropertyCount;

        /// <summary>
        /// 对象类型
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// 获取基础类型代码
        /// </summary>
        public readonly TypeCode TypeCode;

        private readonly Dictionary<string, JsonMember> _members;

        /// <summary>
        /// 从指定的 Type 创建新的 JsonType 对象
        /// </summary>
        public JsonType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type;
            DisplayText = ComponentServices.Converter.ToString(type);
            _members = new Dictionary<string, JsonMember>(StringComparer.OrdinalIgnoreCase);
            var list = new List<JsonMember>();

            TypeCode = Type.GetTypeCode(type);
            IsMataType = EqualMataType(type);
            IsAnonymous = Type.IsGenericType && Type.Name.StartsWith("<>f__AnonymousType");
            IsObject = type == typeof(object);
            Convertor = ConvertorServices.Container.GetConvertor(type);
            IsNumber = TypeCode >= TypeCode.SByte && TypeCode <= TypeCode.Decimal;

            //兼容IList,IDictionary,IList<T>,IDictionary<TKey, TValue>
            //判断接口
            var iType = GetInterface(type, typeof(IDictionary<,>));
            if (iType != null)
            {
                IsDictionary = true;
                var args = iType.GetGenericArguments();
                if (type.IsInterface) //兼容 IDictionary<TKey, TValue>
                {
                    Type = typeof(Dictionary<,>).MakeGenericType(args);
                }
                KeyType = Get(args[0]);
                ElementType = Get(args[1]);
                AddKeyValue =
                    ((IAddOrSet)
                        Activator.CreateInstance(typeof(AddOrSet<,>).MakeGenericType(KeyType.Type, ElementType.Type)))
                        .IDictionaryT;
                return;
            }

            iType = GetInterface(type, typeof(IDictionary));
            if (iType != null)
            {
                IsDictionary = true;
                if (type.IsInterface) //兼容 IDictionary
                {
                    Type = typeof(Hashtable);
                }
                KeyType = Get<object>();
                ElementType = KeyType;
                AddKeyValue =
                    ((IAddOrSet)
                        Activator.CreateInstance(typeof(AddOrSet<,>).MakeGenericType(KeyType.Type, ElementType.Type)))
                        .IDictionary;
                return;
            }


            if (typeof(NameValueCollection).IsAssignableFrom(type))
            {
                IsDictionary = true;
                KeyType = Get<string>();
                ElementType = Get<string>();
                AddKeyValue = (o, k, v) => ((NameValueCollection) o).Add((string) k, (string) v);
                return;
            }

            if (type.IsArray)
            {
                IsList = true;
                ElementType = Get(type.GetElementType());
                AddValue = (o, v) => ((ArrayList) o).Add(v);
                return;
            }

            iType = GetInterface(type, typeof(ICollection<>));
            if (iType != null)
            {
                IsList = true;
                var args = iType.GetGenericArguments();
                if (type.IsInterface) //兼容 ICollection<T>
                {
                    Type = typeof(List<>).MakeGenericType(args);
                }
                ElementType = Get(args[0]);
                AddValue =
                    ((IAddOrSet)
                        Activator.CreateInstance(typeof(AddOrSet<,>).MakeGenericType(typeof(object), ElementType.Type)))
                        .ICollectionT;
                return;
            }

            iType = GetInterface(type, typeof(IList));
            if (iType != null)
            {
                if (type.IsInterface) //兼容 ICollection<T>
                {
                    Type = typeof(ArrayList);
                }
                IsList = true;
                ElementType = Get<object>();
                AddValue =((IAddOrSet)Activator.CreateInstance(typeof(AddOrSet<,>).MakeGenericType(typeof(object), ElementType.Type))).IList;

                return;
            }

            if (IsAnonymous == false)
            {
                //过滤基本类型
                if (type.IsPrimitive || type == typeof(string) || type.IsEnum
                    ||
                    (type.Namespace == "System" && type.Module == typeof(int).Module && type.IsValueType &&
                     typeof(IFormattable).IsAssignableFrom(type)))
                {
                    PropertyCount = 0;
                    return;
                }
            }
            var flags = BindingFlags.Instance | BindingFlags.Public;
            //枚举属性
            foreach (var p in Type.GetProperties(flags))
            {
                //获取索引器
                if (p.GetIndexParameters()?.Length > 0)
                {
                    continue;
                }
                var jm = JsonMember.Create(p);
                if (jm != null)
                {
                    if (_members.ContainsKey(jm.JsonName))
                    {
                        throw new ArgumentException($"JsonName重复:{jm.JsonName}");
                    }
                    _members[jm.JsonName] = jm;
                    list.Add(jm);
                }
            }
            PropertyCount = list.Count;
            //枚举字段
            foreach (var f in Type.GetFields(flags))
            {
                var jm = JsonMember.Create(f);
                if (jm != null)
                {
                    if (_members.ContainsKey(jm.JsonName))
                    {
                        throw new ArgumentException($"JsonName重复:{jm.JsonName}");
                    }
                    _members[jm.JsonName] = jm;
                    list.Add(jm);
                }
            }
            Members = list.ToArray();
        }


        /// <summary>
        /// 根据 Json成员名称查找相关属性,并指定是否区分大小写,未找到返回null
        /// </summary>
        /// <param name="jsonName"> Json成员名称 </param>
        /// <param name="ignoreCase"> 是否忽略大小写 </param>
        public JsonMember this[string jsonName, bool ignoreCase]
        {
            get
            {
                JsonMember jmember;
                if (_members.TryGetValue(jsonName, out jmember))
                {
                    if (ignoreCase || string.Equals(jmember.JsonName, jsonName, StringComparison.Ordinal))
                    {
                        return jmember;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 用于显示的文本
        /// </summary>
        public string DisplayText { get; private set; }

        /// <summary>
        /// 枚举所有成员
        /// </summary>
        public IEnumerator<JsonMember> GetEnumerator()
        {
            return _members.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _members.Values.GetEnumerator();
        }

        /// <summary>
        /// 获取指定类型的接口,如果不存在返回null
        /// </summary>
        /// <param name="type"> 需要获取接口的对象类型 </param>
        /// <param name="interfaceType"> 需要的接口类型 </param>
        /// <returns> </returns>
        private static Type GetInterface(Type type, Type interfaceType)
        {
            if (type == interfaceType)
            {
                return type;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType)
            {
                return type;
            }
            if (interfaceType.IsGenericTypeDefinition)
            {
                return type
                    .GetInterfaces()
                    .Where(it => it.IsGenericType)
                    .FirstOrDefault(it => it.GetGenericTypeDefinition() == interfaceType);
            }

            if (type.GetInterfaces().Contains(interfaceType))
            {
                return interfaceType;
            }
            return null;
        }

        private static bool EqualMataType(Type type)
        {
            var t = Nullable.GetUnderlyingType(type);
            if (t != null)
            {
                return EqualMataType(t);
            }
            return _mataTypes.Contains(type);
        }

        public object Convert(string str, Type type)
        {
            using (var context = new ConvertContext())
            {
                bool b;
                var obj = Convertor.ChangeType(context, str, type, out b);
                if (b == false)
                {
                    context.ThrowIfHaveError();
                }
                return obj;
            }
        }

        public object Convert(object val, Type type)
        {
            using (var context = new ConvertContext())
            {
                bool b;
                var obj = Convertor.ChangeType(context, val, type, out b);
                if (b == false)
                {
                    context.ThrowIfHaveError();
                }
                return obj;
            }
        }


        public override bool Equals(object obj)
        {
            var jtype = obj as JsonType;
            if (jtype == null)
            {
                return false;
            }
            return Type == jtype.Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        #region 缓存

        private static readonly TypeCache<JsonType> _cache = new TypeCache<JsonType>();

        public static JsonType Get(Type type)
        {
            return _cache.GetOrCreate(type, t => new JsonType(t));
        }

        public static JsonType Get<T>()
        {
            return _cache.GetOrCreate<T>(t => new JsonType(t));
        }

        #endregion
    }
}