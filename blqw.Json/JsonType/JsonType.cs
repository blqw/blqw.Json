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
    /// <summary> 指示可以被序列化json的对象类型信息
    /// </summary>
    public sealed class JsonType : IEnumerable<JsonMember>
    {
        private static void AreNull(object value, string argName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argName);
            }
        }
        private static void AreTrue(bool condition, string message)
        {
            if (condition)
            {
                throw new ArgumentException(message);
            }
        }
        private readonly static TypeCache<JsonType> _cache = new TypeCache<JsonType>();

        public static JsonType Get(Type type)
        {
            return _cache.GetOrCreate(type, () => new JsonType(type));
        }

        public static JsonType Get<T>()
        {
            return _cache.GetOrCreate<T>(() => new JsonType(typeof(T)));
        }

        private Dictionary<string, JsonMember> _members;

        /// <summary> 对象类型
        /// </summary>
        public readonly Type Type;

        public readonly TypeCode TypeCode;

        /// <summary> 如果是IDictionary 表示Key的类型,否则为null
        /// </summary>
        public readonly JsonType KeyType;

        /// <summary> 如果是IDictionary 表示Value的类型,IList表示Value的类型,Array表示数组元素的类型,否则为null
        /// </summary>
        public readonly JsonType ElementType;
        /// <summary> 属性的个数,
        /// </summary>
        public readonly int PropertyCount;

        /// <summary> TypeCodes 是否是 IList 或 IListT
        /// </summary>
        public readonly bool IsListType;

        /// <summary> TypeCodes 是否是 IDictionary 或 IDictionaryT
        /// </summary>
        public readonly bool IsDictionaryType;

        /// <summary> 获取一个值，通过该值指示类型是否属于单值类型
        /// 除了基元类型以外,Guid,TimeSpan,DateTimeOffset,DateTime,DBNull,所有指针,以及这些类型的可空值类型,都属于特殊类型
        /// </summary>
        public readonly bool IsSoleType;
        /// <summary> 是否是匿名类
        /// </summary>
        public readonly bool IsAnonymousType;

        /// <summary> 是否是Object类
        /// </summary>
        public readonly bool IsObjectType;
        /// <summary> 是否是数字类
        /// </summary>
        public readonly bool IsNumberType;

        /// <summary> 转换器
        /// </summary>
        public readonly IFormatterConverter Convertor;

        /// <summary> 属性和字段集合
        /// </summary>
        public readonly JsonMember[] Members;

        internal readonly Func<object, object[], object> AddValue;

        internal readonly Func<object, object[], object> AddKeyValue;

        /// <summary> 从指定的 Type 创建新的 JsonType 对象
        /// </summary>
        public JsonType(Type type)
        {
            AreNull(type, "type");
            Type = type;
            DisplayText = type.FullName;
            _members = new Dictionary<string, JsonMember>(StringComparer.OrdinalIgnoreCase);
            var list = new List<JsonMember>();

            TypeCode = Type.GetTypeCode(type);
            IsSoleType = IsMataType(type);
            IsAnonymousType = Type.IsGenericType && Type.Name.StartsWith("<>f__AnonymousType");
            IsObjectType = type == typeof(object);
            Convertor = Component.GetConverter(type, true);
            IsNumberType = (TypeCode >= TypeCode.SByte && TypeCode <= TypeCode.Decimal);

            //兼容IList,IDictionary,IList<T>,IDictionary<TKey, TValue>
            //判断接口
            var iType = GetInterface(type, typeof(IDictionary<,>));
            if (iType != null)
            {
                IsDictionaryType = true;
                var args = iType.GetGenericArguments();
                if (type.IsInterface)//兼容 IDictionary<TKey, TValue>
                {
                    Type = typeof(Dictionary<,>).MakeGenericType(args);
                }
                KeyType = JsonType.Get(args[0]);
                ElementType = JsonType.Get(args[1]);
                AddKeyValue = ((MethodInfo)Component.Wrapper(iType.GetMethod("Add", args) ?? type.GetMethod("Add", args))).Invoke;
                return;
            }

            iType = GetInterface(type, typeof(IDictionary));
            if (iType != null)
            {
                IsDictionaryType = true;
                var args = new Type[] { typeof(object), typeof(object) };
                if (type.IsInterface)//兼容 IDictionary
                {
                    Type = typeof(Hashtable);
                }
                KeyType = JsonType.Get<object>();
                ElementType = KeyType;
                AddKeyValue = (o, v) => {
                    ((System.Collections.IDictionary)o).Add(v[0], v[1]);
                    return null;
                };
                return;
            }


            if (typeof(NameValueCollection).IsAssignableFrom(type))
            {
                var args = new Type[] { typeof(string), typeof(string) };
                IsDictionaryType = true;
                KeyType = JsonType.Get<string>();
                ElementType = JsonType.Get<string>();
                AddKeyValue = (o, v) => {
                    ((NameValueCollection)o).Add((string)v[0], (string)v[1]);
                    return null;
                };
                return;
            }

            if (type.IsArray)
            {
                IsListType = true;
                var args = new Type[] { typeof(object) };
                ElementType = JsonType.Get(type.GetElementType());
                //AddValue = Literacy.CreateCaller(typeof(ArrayList).GetMethod("Add", args), typeof(ArrayList));
                AddValue = (o, v) => ((System.Collections.ArrayList)o).Add(v[0]);

                return;
            }
            iType = GetInterface(type, typeof(ICollection<>));
            if (iType != null)
            {
                IsListType = true;
                var args = iType.GetGenericArguments();
                if (type.IsInterface)//兼容 ICollection<T>
                {
                    Type = typeof(List<>).MakeGenericType(args);
                }
                ElementType = JsonType.Get(args[0]);

                AddValue = ((MethodInfo)Component.Wrapper(iType.GetMethod("Add", args) ?? type.GetMethod("Add", args))).Invoke;
                return;
            }

            iType = GetInterface(type, typeof(IList));
            if (iType != null)
            {
                var args = new Type[] { typeof(object) };
                if (type.IsInterface)//兼容 ICollection<T>
                {
                    Type = typeof(ArrayList);
                }
                IsListType = true;
                ElementType = JsonType.Get<object>();
                //AddValue = Literacy.CreateCaller(typeof(ArrayList).GetMethod("Add", args) ?? type.GetMethod("Add", args), type);
                AddValue = (o, v) => ((System.Collections.IList)o).Add(v[0]);

                return;
            }

            var flags = BindingFlags.Instance | BindingFlags.Public;
            //枚举属性
            foreach (var p in Type.GetProperties(flags))
            {
                
                var jm = JsonMember.Create(p);
                if (jm != null)
                {
                    AreTrue(_members.ContainsKey(jm.JsonName), "JsonName重复:" + jm.JsonName);
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
                    AreTrue(_members.ContainsKey(jm.JsonName), "JsonName重复:" + jm.JsonName);
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

        private static bool IsMataType(Type type)
        {
            var t = Nullable.GetUnderlyingType(type);
            if (t != null)
            {
                return IsMataType(t);
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
