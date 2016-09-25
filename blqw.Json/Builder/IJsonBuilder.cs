using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace blqw.Serializable
{
    /// <summary>
    /// Json字符串构造器
    /// </summary>
    public interface IJsonBuilder : IDisposable
    {

        /// <summary>
        /// 构造Json字符串的必要选项
        /// </summary>
        JsonBuilderSettings Settings { get; }
        /// <summary>
        /// 当前字符串长度
        /// </summary>
        int Length { get; }
        /// <summary>
        /// 追加一个 <see cref="char"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(char value);
        /// <summary>
        /// 追加一个 <see cref="byte"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(byte value);
        /// <summary>
        /// 追加一个 <see cref="DateTime"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(DateTime value);
        /// <summary>
        /// 追加一个 <see cref="double"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(double value);
        /// <summary>
        /// 追加一个 <see cref="float"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(float value);
        /// <summary>
        /// 追加一个 <see cref="string"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(IEnumerable<string> value);
        /// <summary>
        /// 追加一个 <see cref="long"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(long value);
        /// <summary>
        /// 追加一个 <see cref="short"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(short value);
        /// <summary>
        /// 追加一个 <see cref="uint"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(uint value);
        /// <summary>
        /// 追加一个 <see cref="ushort"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(ushort value);
        /// <summary>
        /// 追加一个 <see cref="ulong"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(ulong value);
        /// <summary>
        /// 追加一个 <see cref="string"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="escape">是否需要转码</param>
        /// <param name="wrapper">是否需要用双引号包裹</param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(string value, bool escape = true, bool wrapper = true);
        /// <summary>
        /// 追加一个 <see cref="sbyte"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(sbyte value);
        /// <summary>
        /// 追加一个 <see cref="int"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(int value);
        /// <summary>
        /// 追加一个 <see cref="decimal"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(decimal value);
        /// <summary>
        /// 追加一个 <see cref="bool"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(bool value);
        /// <summary>
        /// 追加一个 <see cref="Guid"/> 到当前对象
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(Guid value);

        /// <summary>
        /// 追加一个 <see cref="char"/> 数组到当前对象
        /// </summary>
        /// <param name="charArray"> 字符数组 </param>
        /// <param name="offset"> 初始偏移量 </param>
        /// <param name="length"> 长度 </param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(char[] charArray, int offset, int length);

        /// <summary>
        /// 追加一个可格式化对象 <see cref="IFormattable"/> 到当前对象
        /// </summary>
        /// <param name="value"> 可格式化对象 </param>
        /// <param name="format"> 格式化参数 </param>
        /// <param name="provider"> 格式化提供程序 </param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Append(IFormattable value, string format, IFormatProvider provider);

        /// <summary>
        /// 追加 内存数据 到当前对象
        /// </summary>
        /// <param name="point"> 指针 </param>
        /// <param name="offset"> 起始偏移量 </param>
        /// <param name="length"> 长度 </param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        unsafe void Append(char* point, int offset, int length);

        /// <summary>
        /// 使用指定的格式信息将指定的对象数组的文本表示形式写入 追加到当前对象
        /// </summary>
        /// <param name="format"> 复合格式字符串 </param>
        /// <param name="args"> 要使用 format 写入的对象的数组。 </param>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void AppendFormat(string format, params object[] args);

        /// <summary>
        /// 将当前行终止符写入标准
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void AppendLine();

        /// <summary>
        /// 返回当前已构造的字符串
        /// </summary>
        string ToString();

        /// <summary>
        /// 开始一个数据
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void BeginArray();
        /// <summary>
        /// 关闭一个数组
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void CloseArray();

        /// <summary>
        /// 逗号
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Common();

        /// <summary>
        /// 开始一个对象
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void BeginObject();

        /// <summary>
        /// 关闭一个对象
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void CloseObject();

        /// <summary>
        /// 逗号
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已经释放</exception>
        void Colon();
    }
}