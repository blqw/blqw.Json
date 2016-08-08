using System.IO;

namespace blqw.Serializable
{
    /// <summary>
    /// 逗号帮助类
    /// </summary>
    internal struct CommaHelper
    {
        private const char COMMA = ',';
        private readonly TextWriter _writer;
        private bool _first;
        public CommaHelper(TextWriter writer)
        {
            _writer = writer;
            _first = true;
        }

        /// <summary>
        /// 追加一个逗号,但忽略这个方法的第一次执行
        /// </summary>
        public void AppendCommaIgnoreFirst()
        {
            if (_first)
                _first = false;
            else
                _writer.Write(COMMA);
        }
    }
}
