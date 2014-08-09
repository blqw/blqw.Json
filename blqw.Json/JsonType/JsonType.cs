using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace blqw
{
    /// <summary> 指示可以被序列化json的对象类型信息
    /// </summary>
    public sealed class JsonType : IEnumerable<JsonMember>
    {
        private static OrderlyList<int, JsonType> _Cache = new OrderlyList<int, JsonType>();

        public static JsonType Get(Type type)
        {
            var hashCode = type.GUID.GetHashCode();
            var jtype = _Cache[hashCode];
            if (jtype == null)
            {
                jtype = new JsonType(type);
                _Cache.Add(hashCode, jtype);
            }
            return jtype;
        }

        private DictionaryEx<string, JsonMember> _members;

        /// <summary> 对象构造函数委托
        /// </summary>
        private LiteracyNewObject _ctor;

        /// <summary> 对象类型
        /// </summary>
        public readonly Type Type;

        /// <summary> 类型枚举
        /// </summary>
        public readonly TypeCodeEx TypeCodeEx;

        /// <summary> 如果是IDictionary 表示Key的类型,否则为null
        /// </summary>
        public readonly Type KeyType;

        /// <summary> 如果是IDictionary 表示Value的类型,IList表示Value的类型,Array表示数组元素的类型,否则为null
        /// </summary>
        public readonly Type ElementType;
        /// <summary> 属性的个数,
        /// </summary>
        public readonly int PropertyCount;

        /// <summary> TypeCodeEx 是否是 IList 或 IListT
        /// </summary>
        public readonly bool IsList;

        /// <summary> TypeCodeEx 是否是 IDictionary 或 IDictionaryT
        /// </summary>
        public readonly bool IsDictionary;

        /// <summary> 属性和字段集合
        /// </summary>
        public readonly JsonMember[] Members;

        public readonly LiteracyCaller AddValue;

        public readonly LiteracyCaller AddKeyValue;

        private JsonType(Type type, int i)
        {
            Assertor.AreNull(type, "type");
            Type = type;
            _members = new DictionaryEx<string, JsonMember>(StringComparer.OrdinalIgnoreCase);
            var list = new List<JsonMember>();
            //枚举属性
            foreach (var p in Type.GetProperties())
            {
                var jm = JsonMember.Create(p);
                if (jm != null)
                {
                    Assertor.AreTrue(_members.ContainsKey(jm.JsonName), "JsonName重复:" + jm.JsonName);
                    _members[jm.JsonName] = jm;
                    list.Add(jm);
                }
            }
            PropertyCount = list.Count;
            //枚举字段
            foreach (var p in Type.GetFields())
            {
                var jm = JsonMember.Create(p);
                if (jm != null)
                {
                    Assertor.AreTrue(_members.ContainsKey(jm.JsonName), "JsonName重复:" + jm.JsonName);
                    _members[jm.JsonName] = jm;
                    list.Add(jm);
                }
            }
            Members = list.ToArray();
            //设置 TypeCodeEx ,ElementType ,KeyType
            TypeCodeEx = Literacy.GetTypeCodeEx(type);
            switch (TypeCodeEx)
            {
                case TypeCodeEx.IListT:
                    IsList = true;
                    var args = type.GetGenericArguments();
                    ElementType = args[0];
                    AddValue = Literacy.CreateCaller(type.GetMethod("Add", args));
                    break;
                case TypeCodeEx.IList:
                    IsList = true;
                    if (type.IsArray)
                    {
                        ElementType = type.GetElementType();
                        AddValue = (o, v) => ((System.Collections.ArrayList)o).Add(v[0]);
                    }
                    else
                    {
                        ElementType = typeof(object);
                        AddValue = (o, v) => ((System.Collections.IList)o).Add(v[0]);
                    }
                    break;
                case TypeCodeEx.IDictionary:
                    IsDictionary = true;
                    KeyType = typeof(object);
                    ElementType = typeof(object);
                    AddKeyValue = (o, v) => { ((System.Collections.IDictionary)o).Add(v[0], v[1]); return null; };
                    break;
                case TypeCodeEx.IDictionaryT:
                    IsDictionary = true;
                    args = type.GetGenericArguments();
                    KeyType = args[0];
                    ElementType = args[1];
                    AddKeyValue = Literacy.CreateCaller(type.GetMethod("Add", args));
                    break;
                default:
                    break;
            }
            //处理可空值类型
            if (KeyType != null)
            {
                KeyType = Nullable.GetUnderlyingType(KeyType) ?? KeyType;
            }
            if (ElementType != null)
            {
                ElementType = Nullable.GetUnderlyingType(ElementType) ?? ElementType;
            }
        }

        /// <summary> 从指定的 Type 创建新的 JsonType 对象,该方法必须保证类型公开的构造函数有且只有一个
        /// </summary>
        public JsonType(Type type)
            : this(type, 0)
        {
            _ctor = Literacy.CreateNewObject(type);
            if (_ctor == null)
            {
                var ctors = type.GetConstructors();
                Assertor.AreTrue<ArgumentException>(ctors.Length == 0, "没有找到构造函数");
                Assertor.AreTrue<ArgumentException>(ctors.Length > 1, "构造函数调用不明确");
                _ctor = Literacy.CreateNewObject(ctors[0]);
            }
        }

        /// <summary> 从指定的 Type 创建新的 JsonType 对象,并指定构造函数
        /// </summary>
        public JsonType(Type type, ConstructorInfo ctor)
            : this(type, 1)
        {
            Assertor.AreNull(ctor, "ctor");
            Assertor.AreTrue<ArgumentException>(type == ctor.ReflectedType, "ctor不属于当前类型");
            _ctor = Literacy.CreateNewObject(ctor);
        }

        /// <summary> 从指定的 Type 创建新的 JsonType 对象,并指定构造函数的参数
        /// </summary>
        public JsonType(Type type, Type[] ctorArgsType)
            : this(type, 2)
        {
            _ctor = Literacy.CreateNewObject(type, ctorArgsType);
            Assertor.AreTrue<ArgumentException>(_ctor == null, "没有找到符合条件的构造函数");
        }


        /// <summary> 根据 Json成员名称查找相关属性,并指定是否区分大小写,未找到返回null
        /// </summary>
        /// <param name="jsonName">Json成员名称</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        public JsonMember this[string jsonName, bool ignoreCase]
        {
            get
            {
                var jmember = _members[jsonName];
                if (ignoreCase || string.Equals(jmember.JsonName, jsonName, StringComparison.Ordinal))
                {
                    return jmember;
                }
                return null;
            }
        }

        /// <summary> 创建当前类型的对象实例
        /// </summary>
        /// <param name="args">构造函数参数</param>
        public object CreateInstance(params object[] args)
        {
            return _ctor(args);
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
