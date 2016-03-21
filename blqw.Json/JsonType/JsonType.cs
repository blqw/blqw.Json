using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

using blqw.Serializable;
using blqw.JsonComponent;

namespace blqw.Serializable
{
    /// <summary> 
    /// 指示可以被序列化json的对象类型信息
    /// </summary>
    public sealed class JsonType : IEnumerable<JsonMember>
    {
        #region 缓存
        private readonly static TypeCache<JsonType> _cache = new TypeCache<JsonType>();

        public static JsonType Get(Type type)
        {
            return _cache.GetOrCreate(type, () => new JsonType(type));
        }

        public static JsonType Get<T>()
        {
            return _cache.GetOrCreate<T>(() => new JsonType(typeof(T)));
        }

        #endregion

        private Dictionary<string, JsonMember> _members;

        /// <summary> 
        /// 对象类型
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// 获取基础类型代码
        /// </summary>
        public readonly TypeCode TypeCode;

        /// <summary> 
        /// 如果 <see cref="IsDictionary"/> 为true 则表示Key的类型,否则为null
        /// </summary>
        public readonly JsonType KeyType;

        /// <summary> 
        /// 如果是集合,则表示内部元素的类型
        /// <para>字典 <see cref="Dictionary{TKey, TValue}"/> 表示TValue的类型</para>
        /// </summary>
        public readonly JsonType ElementType;

        /// <summary> 
        /// 需要序列化的属性和字段的个数
        /// </summary>
        public readonly int PropertyCount;

        /// <summary> 
        /// 是否是索引集合相关的类型
        /// <para></para>
        /// 类型是数组或实现 <see cref="IList"/>,或实现<see cref="ICollection{T}"/>
        /// </summary>
        public readonly bool IsList;

        /// <summary> 
        /// 是否是键值对集合相关的类型
        /// <para></para>
        /// 包括继承 <see cref="NameValueCollection"/> ,实现 <see cref="IDictionary"/> ,实现<see cref="IDictionary{TKey, TValue}"/>
        /// </summary>
        public readonly bool IsDictionary;

        /// <summary> 获取一个值，通过该值指示类型是否属于元类型
        /// <para></para>
        /// 除了基元类型(<see cref="Type.IsPrimitive"/>)以外,还包括
        /// <see cref="Guid"/>,
        /// <see cref="TimeSpan"/>,
        /// <see cref="DateTimeOffset"/>,
        /// <see cref="DateTime"/>,
        /// <see cref="DBNull"/>,
        /// <see cref="IntPtr"/>,
        /// <see cref="UIntPtr"/>,
        /// <see cref="String"/>,
        /// 以及这些类型的可空值类型(<see cref="Nullable{T}"/>)
        /// </summary>
        public readonly bool IsMataType;

        /// <summary> 
        /// 是否是 <see cref="object"/>的实例
        /// </summary>
        public readonly bool IsObject;

        /// <summary> 是否是匿名类
        /// </summary>
        public readonly bool IsAnonymous;

        /// <summary> 是否是数字类
        /// </summary>
        public readonly bool IsNumber;
        
        /// <summary> 
        /// 转换器
        /// </summary>
        public readonly IFormatterConverter Convertor;

        /// <summary> 
        /// 属性和字段集合
        /// </summary>
        public readonly JsonMember[] Members;

        /// <summary>
        /// 用于操作
        /// </summary>
        internal readonly Action<object, object> AddValue;

        internal readonly Action<object, object, object> AddKeyValue;
        
        /// <summary> 从指定的 Type 创建新的 JsonType 对象
        /// </summary>
        public JsonType(Type type)
        {

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type;
            DisplayText = type.FullName;
            _members = new Dictionary<string, JsonMember>(StringComparer.OrdinalIgnoreCase);
            var list = new List<JsonMember>();

            TypeCode = Type.GetTypeCode(type);
            IsMataType = EqualMataType(type);
            IsAnonymous = Type.IsGenericType && Type.Name.StartsWith("<>f__AnonymousType");
            IsObject = type == typeof(object);
            Convertor = Component.GetConverter(type, true);
            IsNumber = (TypeCode >= TypeCode.SByte && TypeCode <= TypeCode.Decimal);

            //兼容IList,IDictionary,IList<T>,IDictionary<TKey, TValue>
            //判断接口
            var iType = GetInterface(type, typeof(IDictionary<,>));
            if (iType != null)
            {
                IsDictionary = true;
                var args = iType.GetGenericArguments();
                if (type.IsInterface)//兼容 IDictionary<TKey, TValue>
                {
                    Type = typeof(Dictionary<,>).MakeGenericType(args);
                }
                KeyType = JsonType.Get(args[0]);
                ElementType = JsonType.Get(args[1]);
                AddKeyValue = ((ISetAdd)Activator.CreateInstance(typeof(SetAdd<,>).MakeGenericType(KeyType.Type, ElementType.Type))).IDictionaryT;
                return;
            }

            iType = GetInterface(type, typeof(IDictionary));
            if (iType != null)
            {
                IsDictionary = true;
                var args = new Type[] { typeof(object), typeof(object) };
                if (type.IsInterface)//兼容 IDictionary
                {
                    Type = typeof(Hashtable);
                }
                KeyType = JsonType.Get<object>();
                ElementType = KeyType;
                AddKeyValue = ((ISetAdd)Activator.CreateInstance(typeof(SetAdd<,>).MakeGenericType(KeyType.Type, ElementType.Type))).IDictionary;
                return;
            }


            if (typeof(NameValueCollection).IsAssignableFrom(type))
            {
                var args = new Type[] { typeof(string), typeof(string) };
                IsDictionary = true;
                KeyType = JsonType.Get<string>();
                ElementType = JsonType.Get<string>();
                AddKeyValue = (o, k, v) => ((NameValueCollection)o).Add((string)k, (string)v);
                return;
            }

            if (type.IsArray)
            {
                IsList = true;
                ElementType = JsonType.Get(type.GetElementType());
                AddValue = (o, v) => ((System.Collections.ArrayList)o).Add(v);
                return;
            }

            iType = GetInterface(type, typeof(ICollection<>));
            if (iType != null)
            {
                IsList = true;
                var args = iType.GetGenericArguments();
                if (type.IsInterface)//兼容 ICollection<T>
                {
                    Type = typeof(List<>).MakeGenericType(args);
                }
                ElementType = JsonType.Get(args[0]);
                AddValue = ((ISetAdd)Activator.CreateInstance(typeof(SetAdd<,>).MakeGenericType(typeof(object), ElementType.Type))).ICollectionT;
                return;
            }

            iType = GetInterface(type, typeof(IList));
            if (iType != null)
            {
                if (type.IsInterface)//兼容 ICollection<T>
                {
                    Type = typeof(ArrayList);
                }
                IsList = true;
                ElementType = JsonType.Get<object>();
                AddValue = ((ISetAdd)Activator.CreateInstance(typeof(SetAdd<,>).MakeGenericType(typeof(object), ElementType.Type))).IList;

                return;
            }

            var flags = BindingFlags.Instance | BindingFlags.Public;
            //枚举属性
            foreach (var p in Type.GetProperties(flags))
            {

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

        /// <summary> 获取指定类型的接口,如果不存在返回null
        /// </summary>
        /// <param name="type">需要获取接口的对象类型</param>
        /// <param name="interfaceType">需要的接口类型</param>
        /// <returns></returns>
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

                return type.GetInterfaces()
                    .Where(it => it.IsGenericType)
                    .Where(it => it.GetGenericTypeDefinition() == interfaceType)
                    .FirstOrDefault();
            }

            if (type.GetInterfaces().Contains(interfaceType))
            {
                return interfaceType;
            }
            return null;
        }

        #region _mataTypes
        private static HashSet<Type> _mataTypes = new HashSet<Type>() {
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
            typeof(sbyte ),
            typeof(float ),
            typeof(string),
            typeof(ushort),
            typeof(uint ),
            typeof(ulong ),
        };
        #endregion

        private static bool EqualMataType(Type type)
        {
            var t = Nullable.GetUnderlyingType(type);
            if (t != null)
            {
                return EqualMataType(t);
            }
            return _mataTypes.Contains(type);
        }


        /// <summary> 根据 Json成员名称查找相关属性,并指定是否区分大小写,未找到返回null
        /// </summary>
        /// <param name="jsonName">Json成员名称</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
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

        /// <summary> 枚举所有成员
        /// </summary>
        public IEnumerator<JsonMember> GetEnumerator()
        {
            return _members.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _members.Values.GetEnumerator();
        }

        /// <summary>
        /// 用于显示的文本
        /// </summary>
        public string DisplayText { get; private set; }


        public override bool Equals(object obj)
        {
            var jtype = obj as JsonType;
            if (jtype == null)
            {
                return false;
            }
            return this.Type.Equals(jtype.Type);
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }
    }
}
