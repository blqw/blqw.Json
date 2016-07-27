using blqw.IOC;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace blqw.Serializable
{
    /// <summary>
    /// <see cref="IJsonWriter" /> 的容器 
    /// </summary>
    public static class JsonWriterContainer
    {
        /// <summary>
        /// <see cref="IJsonWriterWrap" /> 的缓存 
        /// </summary>
        private static TypeCache<IJsonWriterWrap> _Items;

        /// <summary>
        /// typeof(Type) 
        /// </summary>
        private static Type _ObjectType = typeof(Type);

        /// <summary>
        /// 通过IOC加载的所有 <see cref="IJsonWriter" /> 的集合 
        /// </summary>
        [ImportMany()]
        private static List<IJsonWriter> _Writers;

        static JsonWriterContainer()
        {
            Reload();
        }

        /// <summary>
        /// <see cref="bool" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter BooleanWriter { get { return null; } }

        /// <summary>
        /// <see cref="byte" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter ByteWriter { get { return null; } }

        /// <summary>
        /// <see cref="char" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter CharWriter { get { return null; } }

        /// <summary>
        /// <see cref="DateTime" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter DateTimeWriter { get { return null; } }

        /// <summary>
        /// <see cref="decimal" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter DecimalWriter { get { return null; } }

        /// <summary>
        /// <see cref="double" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter DoubleWriter { get { return null; } }

        /// <summary>
        /// <see cref="Enum" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter EnumWriter { get { return null; } }

        /// <summary>
        /// <see cref="Guid" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter GuidWriter { get { return null; } }

        /// <summary>
        /// <see cref="IConvertible" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter IConvertibleWriter { get { return null; } }

        /// <summary>
        /// <see cref="short" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter Int16Writer { get { return null; } }

        /// <summary>
        /// <see cref="int" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter Int32Writer { get { return null; } }

        /// <summary>
        /// <see cref="long" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter Int64Writer { get { return null; } }

        /// <summary>
        /// <see cref="Nullable{T}" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter NullableWriter { get { return null; } }

        /// <summary>
        /// <see cref="object" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter ObjectWriter { get { return null; } }

        /// <summary>
        /// <see cref="sbyte" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter SByteWriter { get { return null; } }

        /// <summary>
        /// <see cref="float" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter SingleWriter { get { return null; } }

        /// <summary>
        /// <see cref="string" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter StringWriter { get { return null; } }

        /// <summary>
        /// <see cref="TimeSpan" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter TimeSpanWriter { get { return null; } }

        /// <summary>
        /// <see cref="Type" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter TypeWriter { get { return null; } }

        /// <summary>
        /// <see cref="ushort" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UInt16Writer { get { return null; } }

        /// <summary>
        /// <see cref="uint" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UInt32Writer { get { return null; } }

        /// <summary>
        /// <see cref="ulong" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UInt64Writer { get { return null; } }

        /// <summary>
        /// <see cref="Uri" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UriWriter { get { return null; } }

        /// <summary>
        /// <see cref="Version" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter VersionWriter { get { return null; } }

        /// <summary>
        /// 获取所有 <see cref="IJsonWriter" /> 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IJsonWriter> GetAll => _Writers?.AsReadOnly();

        /// <summary>
        /// 获取一个匹配度最高的 <see cref="IJsonWriter" /> 
        /// </summary>
        /// <param name="type"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns></returns>
        public static IJsonWriter Get(Type type)
        {
            return GetWrap(type)?.Writer;
        }

        /// <summary>
        /// 获取容器中 <see cref="IJsonWriter" /> 的个数 
        /// </summary>
        public static int GetWriterCount()
        {
            return _Writers?.Count ?? 0;
        }

        /// <summary>
        /// 重新加载所有 <see cref="IJsonWriter" /> 
        /// </summary>
        public static void Reload()
        {
            MEF.Import(typeof(JsonWriterContainer));
            _Items = new TypeCache<IJsonWriterWrap>();
            foreach (var w in _Writers)
            {
                _Items.Set(w.Type, new IJsonWriterWrap(w));
            }
        }

        /// <summary>
        /// 设置 <see cref="IJsonWriter" />,如果已经存在则替换 
        /// </summary>
        /// <param name="writer"> <see cref="IJsonWriter" /> 对象 </param>
        public static void Set(IJsonWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            var w = _Items.GetOrCreate(writer.Type, (t) => new IJsonWriterWrap(writer));
            if (w.Writer.Type == writer.Type)
            {
                w.Writer = writer;
            }
            else
            {
                _Items.Set(writer.Type, new IJsonWriterWrap(writer));
            }
        }

        /// <summary>
        /// 获取匹配 <paramref name="type" /> 的 <see cref="IJsonWriterWrap" /> 
        /// </summary>
        /// <param name="type"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns></returns>
        internal static IJsonWriterWrap GetWrap(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return _Items.GetOrCreate(type, Select);
        }

        /// <summary>
        /// 从 <see cref="IJsonWriter" /> 集合 <seealso cref="_Items" /> 中选择一个匹配
        /// <paramref name="type" /> 的 <see cref="IJsonWriterWrap" />
        /// </summary>
        /// <param name="type"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns></returns>
        private static IJsonWriterWrap Select(Type type)
        {
            //精确匹配当前类 或 泛型定义
            var wrap = _Items.Get(type) ?? SelectByGenericDefinition(type);
            if (wrap != null)
            {
                return wrap;
            }
            var baseType = type.BaseType;
            while (baseType != _ObjectType)
            {
                //匹配父类 或 父类泛型定义.除了Object以外
                wrap = _Items.Get(baseType) ?? SelectByGenericDefinition(baseType);
                if (wrap != null)
                {
                    return wrap;
                }
                baseType = type.BaseType;
            }
            //匹配接口
            var interfaces = type.GetInterfaces();
            foreach (var interfaceType in interfaces)
            {
                wrap = _Items.Get(interfaceType);
                if (wrap != null)
                {
                    return wrap;
                }
            }
            //匹配接口泛型定义
            foreach (var interfaceType in interfaces)
            {
                wrap = SelectByGenericDefinition(interfaceType);
                if (wrap != null)
                {
                    return new IJsonWriterWrap(wrap, type);
                }
            }
            //匹配object定义
            return new IJsonWriterWrap(_Items.Get(_ObjectType), type);
        }

        /// <summary>
        /// 获取与 <paramref name="type" /> 的泛型定义类型匹配的 <see cref="IJsonWriterWrap" />,如果
        /// <paramref name="type" /> 不是泛型,返回 <seealso cref="null" />
        /// </summary>
        /// <param name="type"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns></returns>
        private static IJsonWriterWrap SelectByGenericDefinition(Type type)
        {
            if (type.IsGenericType && type.IsGenericTypeDefinition == false)
            {
                var wrap = _Items.Get(type.GetGenericTypeDefinition());
                if (wrap != null)
                {
                    return new IJsonWriterWrap(wrap, type);
                }
            }
            return null;
        }

        /// <summary>
        /// <see cref="IJsonWriter" /> 的包装类型 
        /// </summary>
        internal class IJsonWriterWrap
        {
            /// <summary>
            /// <seealso cref="_writer"/>的生成源的引用
            /// </summary>
            private IJsonWriter _originReference;
            /// <summary>
            /// <seealso cref="_writer"/>的类型
            /// </summary>
            private Type _type;
            /// <summary>
            /// 包装对象
            /// </summary>
            private IJsonWriterWrap _wrap;
            /// <summary>
            /// <see cref="IJsonWriter"/>对象
            /// </summary>
            private IJsonWriter _writer;

            /// <summary>
            /// 包装一个 <see cref="IJsonWriter"/> 
            /// </summary>
            /// <param name="writer"><see cref="IJsonWriter"/>对象 </param>
            public IJsonWriterWrap(IJsonWriter writer)
            {
                _writer = writer;
                _type = writer.Type;
            }

            /// <summary>
            /// 生成一个新的 <see cref="IJsonWriter"/> 并包装起来
            /// </summary>
            /// <param name="wrap">原始<see cref="IJsonWriterWrap"/> </param>
            /// <param name="type">待生成的<see cref="IJsonWriter"/>的类型 </param>
            public IJsonWriterWrap(IJsonWriterWrap wrap, Type type)
            {
                _wrap = wrap;
                _type = type;
                Create();
            }

            /// <summary>
            /// 被包装的 <see cref="IJsonWriter"/>
            /// </summary>
            public IJsonWriter Writer
            {
                get
                {
                    if (_wrap == null)
                    {
                        return _writer;
                    }
                    if (ReferenceEquals(_wrap?.Writer, _originReference))
                    {
                        return _writer;
                    }
                    Create();
                    return _writer;
                }
                set
                {
                    _writer = value;
                }
            }

            /// <summary>
            /// 生成新的 <see cref="IJsonWriter"/>
            /// </summary>
            private void Create()
            {
                if (_wrap == null)
                {
                    throw new ArgumentNullException(nameof(_wrap));
                }
                if (_type == null)
                {
                    throw new ArgumentNullException(nameof(_type));
                }
                _originReference = _wrap.Writer;
                _writer = (_wrap.Writer as IGenericJsonWriter)?.MakeType(_type) ?? _wrap.Writer;
            }
        }
    }
}