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
        /// <see cref="IJsonWriterWrapper" /> 的缓存 
        /// </summary>
        private static TypeCache<IJsonWriterWrapper> _Items;

        /// <summary>
        /// typeof(Type) 
        /// </summary>
        private static readonly Type _ObjectType = typeof(Type);


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
        /// 重新加载所有 <see cref="IJsonWriter" /> 
        /// </summary>
        public static void Reload()
        {
            MEF.Import(typeof(JsonWriterContainer));
            _Items = new TypeCache<IJsonWriterWrapper>();
            foreach (var w in _Writers)
            {
                _Items.Set(w.Type, new IJsonWriterWrapper(w));
            }
        }

        private static IJsonWriterWrapper _NullWapper;
        private static IJsonWriterWrapper _VersionWapper;
        private static IJsonWriterWrapper _UriWapper;
        private static IJsonWriterWrapper _UInt64Wapper;
        private static IJsonWriterWrapper _UInt32Wapper;
        private static IJsonWriterWrapper _UInt16Wapper;
        private static IJsonWriterWrapper _TypeWapper;
        private static IJsonWriterWrapper _TimeSpanWapper;
        private static IJsonWriterWrapper _StringWapper;
        private static IJsonWriterWrapper _SingleWapper;
        private static IJsonWriterWrapper _SByteWapper;
        private static IJsonWriterWrapper _Int64Wapper;
        private static IJsonWriterWrapper _Int32Wapper;
        private static IJsonWriterWrapper _Int16Wapper;
        private static IJsonWriterWrapper _ConvertibleWapper;
        private static IJsonWriterWrapper _GuidWapper;
        private static IJsonWriterWrapper _EnumWapper;
        private static IJsonWriterWrapper _DoubleWapper;
        private static IJsonWriterWrapper _DateTimeWapper;
        private static IJsonWriterWrapper _DecimalWapper;
        private static IJsonWriterWrapper _CharWapper;
        private static IJsonWriterWrapper _ByteWapper;
        private static IJsonWriterWrapper _BooleanWapper;


        /// <summary>
        /// <see cref="bool" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter BooleanWriter => (_BooleanWapper ?? (_BooleanWapper = _Items.Get<bool>())).Writer;

        /// <summary>
        /// <see cref="byte" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter ByteWriter => (_ByteWapper ?? (_ByteWapper = _Items.Get<byte>())).Writer;

        /// <summary>
        /// <see cref="char" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter CharWriter => (_CharWapper ?? (_CharWapper = _Items.Get<char>())).Writer;

        /// <summary>
        /// <see cref="DateTime" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter DateTimeWriter => (_DateTimeWapper ?? (_DateTimeWapper = _Items.Get<DateTime>())).Writer;

        /// <summary>
        /// <see cref="decimal" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter DecimalWriter => (_DecimalWapper ?? (_DecimalWapper = _Items.Get<decimal>())).Writer;

        /// <summary>
        /// <see cref="double" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter DoubleWriter => (_DoubleWapper ?? (_DoubleWapper = _Items.Get<double>())).Writer;

        /// <summary>
        /// <see cref="Enum" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter EnumWriter => (_EnumWapper ?? (_EnumWapper = _Items.Get<Enum>())).Writer;

        /// <summary>
        /// <see cref="Guid" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter GuidWriter => (_GuidWapper ?? (_GuidWapper = _Items.Get<Guid>())).Writer;

        /// <summary>
        /// <see cref="IConvertible" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter ConvertibleWriter => (_ConvertibleWapper ?? (_ConvertibleWapper = _Items.Get<IConvertible>())).Writer;

        /// <summary>
        /// <see cref="short" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter Int16Writer => (_Int16Wapper ?? (_Int16Wapper = _Items.Get<short>())).Writer;

        /// <summary>
        /// <see cref="int" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter Int32Writer => (_Int32Wapper ?? (_Int32Wapper = _Items.Get<int>())).Writer;

        /// <summary>
        /// <see cref="long" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter Int64Writer => (_Int64Wapper ?? (_Int64Wapper = _Items.Get<long>())).Writer;

        /// <summary>
        /// <see cref="sbyte" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter SByteWriter => (_SByteWapper ?? (_SByteWapper = _Items.Get<sbyte>())).Writer;

        /// <summary>
        /// <see cref="float" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter SingleWriter => (_SingleWapper ?? (_SingleWapper = _Items.Get<float>())).Writer;

        /// <summary>
        /// <see cref="string" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter StringWriter => (_StringWapper ?? (_StringWapper = _Items.Get<string>())).Writer;

        /// <summary>
        /// <see cref="TimeSpan" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter TimeSpanWriter => (_TimeSpanWapper ?? (_TimeSpanWapper = _Items.Get<TimeSpan>())).Writer;

        /// <summary>
        /// <see cref="Type" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter TypeWriter => (_TypeWapper ?? (_TypeWapper = _Items.Get<Type>())).Writer;

        /// <summary>
        /// <see cref="ushort" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UInt16Writer => (_UInt16Wapper ?? (_UInt16Wapper = _Items.Get<ushort>())).Writer;

        /// <summary>
        /// <see cref="uint" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UInt32Writer => (_UInt32Wapper ?? (_UInt32Wapper = _Items.Get<uint>())).Writer;

        /// <summary>
        /// <see cref="ulong" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UInt64Writer => (_UInt64Wapper ?? (_UInt64Wapper = _Items.Get<ulong>())).Writer;

        /// <summary>
        /// <see cref="Uri" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter UriWriter => (_UriWapper ?? (_UriWapper = _Items.Get<Uri>())).Writer;

        /// <summary>
        /// <see cref="Version" /> 类型的 <see cref="IJsonWriter" /> 
        /// </summary>
        public static IJsonWriter VersionWriter => (_VersionWapper ?? (_VersionWapper = _Items.Get<Version>())).Writer;

        /// <summary>
        /// null 类型的 <see cref="IJsonWriter"/>
        /// </summary>
        public static IJsonWriter NullWriter => (_NullWapper ?? (_NullWapper = _Items.Get(typeof(void)))).Writer;

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

        public static void Write(object value, JsonWriterArgs args)
        {
            if (value == null || value is DBNull)
            {
                NullWriter.Write(null, args);
            }
            else
            {
                Get(value.GetType()).Write(value, args);
            }
        }
        /// <summary>
        /// 获取容器中 <see cref="IJsonWriter" /> 的个数 
        /// </summary>
        public static int GetWriterCount()
        {
            return _Writers?.Count ?? 0;
        }
   /// <summary>
        /// 设置 <see cref="IJsonWriter" />,如果已经存在则替换 
        /// </summary>
        /// <param name="writer"> <see cref="IJsonWriter" /> 对象 </param>
        public static void Set(IJsonWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            var w = _Items.GetOrCreate(writer.Type, (t) => new IJsonWriterWrapper(writer));
            if (w.Writer.Type == writer.Type)
            {
                w.Writer = writer;
            }
            else
            {
                _Items.Set(writer.Type, new IJsonWriterWrapper(writer));
            }
        }

        /// <summary>
        /// 获取匹配 <paramref name="type" /> 的 <see cref="IJsonWriterWrapper" /> 
        /// </summary>
        /// <param name="type"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns></returns>
        internal static IJsonWriterWrapper GetWrap(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return _Items.GetOrCreate(type, Select);
        }

        /// <summary>
        /// 从 <see cref="IJsonWriter" /> 集合 <seealso cref="_Items" /> 中选择一个匹配
        /// <paramref name="type" /> 的 <see cref="IJsonWriterWrapper" />
        /// </summary>
        /// <param name="type"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns></returns>
        private static IJsonWriterWrapper Select(Type type)
        {
            //精确匹配当前类 或 泛型定义
            var wrap = _Items.Get(type) ?? SelectByGenericDefinition(type);
            if (wrap != null)
            {
                return wrap;
            }
            var baseType = type.BaseType ?? _ObjectType;
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
                    return new IJsonWriterWrapper(wrap, type);
                }
            }
            //匹配object定义
            return new IJsonWriterWrapper(_Items.Get(_ObjectType), type);
        }

        /// <summary>
        /// 获取与 <paramref name="type" /> 的泛型定义类型匹配的 <see cref="IJsonWriterWrapper" />,如果
        /// <paramref name="type" /> 不是泛型,返回 <seealso cref="null" />
        /// </summary>
        /// <param name="type"> 用于匹配的 <see cref="Type" /> </param>
        /// <returns></returns>
        private static IJsonWriterWrapper SelectByGenericDefinition(Type type)
        {
            if (type.IsGenericType && type.IsGenericTypeDefinition == false)
            {
                var wrap = _Items.Get(type.GetGenericTypeDefinition());
                if (wrap != null)
                {
                    return new IJsonWriterWrapper(wrap, type);
                }
            }
            return null;
        }

        /// <summary>
        /// <see cref="IJsonWriter" /> 的包装类型 
        /// </summary>
        internal class IJsonWriterWrapper
        {
            /// <summary>
            /// <seealso cref="_writer"/>的生成源的引用
            /// </summary>
            private IJsonWriter _originReference;
            /// <summary>
            /// <seealso cref="_writer"/>的类型
            /// </summary>
            private readonly Type _type;
            /// <summary>
            /// 包装对象
            /// </summary>
            private readonly IJsonWriterWrapper _wrapper;
            /// <summary>
            /// <see cref="IJsonWriter"/>对象
            /// </summary>
            private IJsonWriter _writer;

            /// <summary>
            /// 包装一个 <see cref="IJsonWriter"/> 
            /// </summary>
            /// <param name="writer"><see cref="IJsonWriter"/>对象 </param>
            public IJsonWriterWrapper(IJsonWriter writer)
            {
                _writer = writer;
                _type = writer.Type;
            }

            /// <summary>
            /// 生成一个新的 <see cref="IJsonWriter"/> 并包装起来
            /// </summary>
            /// <param name="wrapper">原始<see cref="IJsonWriterWrapper"/> </param>
            /// <param name="type">待生成的<see cref="IJsonWriter"/>的类型 </param>
            public IJsonWriterWrapper(IJsonWriterWrapper wrapper, Type type)
            {
                _wrapper = wrapper;
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
                    if (_wrapper == null)
                    {
                        return _writer;
                    }
                    if (ReferenceEquals(_wrapper?.Writer, _originReference))
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
                if (_wrapper == null)
                {
                    throw new ArgumentNullException(nameof(_wrapper));
                }
                if (_type == null)
                {
                    throw new ArgumentNullException(nameof(_type));
                }
                _originReference = _wrapper.Writer;
                _writer = (_wrapper.Writer as IGenericJsonWriter)?.MakeType(_type) ?? _wrapper.Writer;
            }
        }


    }
}