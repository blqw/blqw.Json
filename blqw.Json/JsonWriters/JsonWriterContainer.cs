using blqw.IOC;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    /// <summary>
    /// <see cref="IJsonWriter"/>容器
    /// </summary>
    public static class JsonWriterContainer
    {
        [ImportMany()]
        static List<IJsonWriter> _Writers;

        class MyClass
        {
            [ImportMany()]
            public List<IJsonWriter> _Writers { get; set; }
        }
        static TypeCache<IJsonWriterPackage> _Items;

        static Type _ObjectType = typeof(Type);

        class IJsonWriterPackage
        {
            public IJsonWriterPackage(IJsonWriter writer)
            {
                this.Writer = writer;
            }

            public IJsonWriter Writer { get; set; }
        }

        static JsonWriterContainer()
        {
            Reload();
        }

        public static void Reload()
        {
            var a = new MyClass();
            MEFLite.Import(a);
            MEFLite.Import(typeof(JsonWriterContainer));
            _Items = new TypeCache<IJsonWriterPackage>();
            foreach (var w in _Writers)
            {
                _Items.Set(w.Type, new IJsonWriterPackage(w));
            }
        }

        private static IJsonWriterPackage SelectByGenericDefinition(Type type)
        {
            if (type.IsGenericType && type.IsGenericTypeDefinition == false)
            {
                var writer = _Items.Get(type.GetGenericTypeDefinition());
                if (writer != null)
                {
                    return writer;
                }
            }
            return null;
        }

        private static IJsonWriterPackage Select(Type type)
        {
            //精确匹配当前类 或 泛型定义
            var writer = _Items.Get(type) ?? SelectByGenericDefinition(type);
            if (writer != null)
            {
                return writer;
            }
            var baseType = type.BaseType;
            while (baseType != _ObjectType)
            {
                //匹配父类 或 父类泛型定义.除了Object以外
                writer = _Items.Get(baseType) ?? SelectByGenericDefinition(baseType);
                if (writer != null)
                {
                    return writer;
                }
                baseType = type.BaseType;
            }
            //匹配接口
            var interfaces = type.GetInterfaces();
            foreach (var interfaceType in interfaces)
            {
                writer = _Items.Get(interfaceType);
                if (writer != null)
                {
                    return writer;
                }
            }
            //匹配接口泛型定义
            foreach (var interfaceType in interfaces)
            {
                writer = SelectByGenericDefinition(interfaceType);
                if (writer != null)
                {
                    return writer;
                }
            }
            //匹配object定义
            return _Items.Get(_ObjectType); ;
        }

        /// <summary>
        /// 获取一个匹配度最高的<see cref="IJsonWriter"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IJsonWriter Get(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return _Items.GetOrCreate(type, Select)?.Writer;
        }

        /// <summary>
        /// 设置<see cref="IJsonWriter"/>,如果有同类型的则替换
        /// </summary>
        /// <param name="writer">Json字符编写器</param>
        public static void Set(IJsonWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            var w = _Items.GetOrCreate(writer.Type, Select);
            w.Writer = writer;
        }

        /// <summary>
        /// 容器中<see cref="IJsonWriter"/>的个数
        /// </summary>
        public static int Count
        {
            get
            {
                return _Writers?.Count ?? 0;
            }
        }

        /// <summary>
        /// 获取所有<see cref="IJsonWriter"/>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IJsonWriter> GetAll => _Writers?.AsReadOnly();
    }
}
