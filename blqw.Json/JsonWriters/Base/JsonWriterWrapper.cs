using System;
using System.Diagnostics;

namespace blqw.Serializable
{
    /// <summary>
    /// <see cref="IJsonWriter" /> 的包装类型
    /// </summary>
    [DebuggerDisplay("Type = {_type}, Writer = {_writer}")]
    public class JsonWriterWrapper
    {
        /// <summary>
        /// <seealso cref="_writer" />的类型
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// 包装对象
        /// </summary>
        private readonly JsonWriterWrapper _wrapper;

        /// <summary>
        /// <seealso cref="_writer" />的生成源的引用
        /// </summary>
        private IJsonWriter _originReference;

        /// <summary>
        /// <see cref="IJsonWriter" />对象
        /// </summary>
        private IJsonWriter _writer;

        /// <summary>
        /// 包装一个 <see cref="IJsonWriter" />
        /// </summary>
        /// <param name="writer"> <see cref="IJsonWriter" />对象 </param>
        public JsonWriterWrapper(IJsonWriter writer)
        {
            _writer = writer;
            _type = writer.Type;
        }

        /// <summary>
        /// 生成一个新的 <see cref="IJsonWriter" /> 并包装起来
        /// </summary>
        /// <param name="wrapper"> 原始<see cref="JsonWriterWrapper" /> </param>
        /// <param name="type"> 待生成的<see cref="IJsonWriter" />的类型 </param>
        public JsonWriterWrapper(JsonWriterWrapper wrapper, Type type)
        {
            _wrapper = wrapper;
            _type = type;
            Create();
        }

        /// <summary>
        /// 被包装的 <see cref="IJsonWriter" />
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
            set { _writer = value; }
        }

        /// <summary>
        /// 生成新的 <see cref="IJsonWriter" />
        /// </summary>
        /// <exception cref="NotImplementedException"> </exception>
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
            _originReference = _writer = _wrapper.Writer;
            var writer = _wrapper.Writer as IGenericJsonWriter;
            if (writer == null) return;
            _writer = (IJsonWriter)writer.GetService(_type);
            if (_writer == null)
            {
                throw new NotImplementedException(
                    $"无法从`{_wrapper.Writer.GetType()}`类型中获取`{_type}`的{nameof(IJsonWriter)}");
            }
        }
    }
}