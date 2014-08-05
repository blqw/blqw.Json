using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace blqw
{
    /// <summary> 指示可以被序列化json的对象类型信息
    /// </summary>
    public sealed class JsonType : IEnumerable<JsonMember>
    {
        /// <summary> 对象成员集合
        /// </summary>
        private DictionaryEx<string, JsonMember> _members;
        private JsonMember[] _memberArray;

        /// <summary> 对象构造函数委托
        /// </summary>
        private LiteracyNewObject _ctor;

        /// <summary> 对象类型
        /// </summary>
        public readonly Type Type;

        /// <summary> 从指定的 Type 创建新的 JsonType 对象,该方法必须保证类型公开的构造函数有且只有一个
        /// </summary>
        public JsonType(Type type)
        {
            Assertor.AreNull(type, "type");
            Type = type;
            _ctor = Literacy.CreateNewObject(type);
            if (_ctor == null)
            {
                var ctors = type.GetConstructors();
                Assertor.AreTrue<ArgumentException>(ctors.Length == 0, "没有找到构造函数");
                Assertor.AreTrue<ArgumentException>(ctors.Length > 1, "构造函数调用不明确");
                _ctor = Literacy.CreateNewObject(ctors[0]);
            }
            Init();
        }

        /// <summary> 从指定的 Type 创建新的 JsonType 对象,并指定构造函数
        /// </summary>
        public JsonType(Type type, ConstructorInfo ctor)
        {
            Assertor.AreNull(type, "type");
            Assertor.AreNull(ctor, "ctor");
            Type = type;
            Assertor.AreTrue<ArgumentException>(type == ctor.ReflectedType, "ctor不属于当前类型");
            _ctor = Literacy.CreateNewObject(ctor);
            Init();
        }

        /// <summary> 从指定的 Type 创建新的 JsonType 对象,并指定构造函数的参数
        /// </summary>
        public JsonType(Type type, Type[] ctorArgsType)
        {
            Assertor.AreNull(type, "type");
            Type = type;
            _ctor = Literacy.CreateNewObject(type, ctorArgsType);
            Assertor.AreTrue<ArgumentException>(_ctor == null, "没有找到符合条件的构造函数");
            Init();
        }

        private void Init()
        {
            _members = new DictionaryEx<string, JsonMember>(StringComparer.OrdinalIgnoreCase);

            foreach (var p in Type.GetProperties())
            {
                var jm = JsonMember.Create(p);
                if (jm != null)
                {
                    Assertor.AreTrue(_members.ContainsKey(jm.JsonName), "JsonName重复:" + jm.JsonName);
                    _members[jm.JsonName] = jm;
                }
            }
            foreach (var p in Type.GetFields())
            {
                var jm = JsonMember.Create(p);
                if (jm != null)
                {
                    Assertor.AreTrue(_members.ContainsKey(jm.JsonName), "JsonName重复:" + jm.JsonName);
                    _members[jm.JsonName] = jm;
                }
            }
            _memberArray = new JsonMember[ _members.Values.Count];
            _members.Values.CopyTo(_memberArray, 0);
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

        /// <summary> 枚举字段
        /// </summary>
        public IEnumerator<JsonMember> Fields
        {
            get
            {
                foreach (var item in _members.Values)
                {
                    if (item.Member.Field)
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary> 枚举属性
        /// </summary>
        public IEnumerator<JsonMember> Properties
        {
            get
            {
                foreach (var item in _members.Values)
                {
                    if (item.Member.Field == false)
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary> 枚举所有成员
        /// </summary>
        public JsonMember[] Members
        {
            get
            {
                return _memberArray;
            }
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
