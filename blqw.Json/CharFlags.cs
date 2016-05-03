using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    [Flags]
    public enum CharFlags : byte
    {
        /// <summary>
        /// 没有
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 字母
        /// </summary>
        Letter = 1 << 0,//1
        /// <summary>
        /// 变量符号
        /// </summary>
        VariableSymbol = 1 << 1,//2
        /// <summary>
        /// 十进制数字 0-9
        /// </summary>
        Digit = 1 << 2,//4
        /// <summary>
        /// 空白字符
        /// </summary>
        WhiteSpace = 1 << 3,//8
        /// <summary>
        /// 转义字符码
        /// </summary>
        EscapeCode = 1 << 4,//16
        /// <summary>
        /// 无效字符
        /// </summary>
        IllegalChar = 1 << 5,//32
        /// <summary>
        /// 数字符号 +/-/./e/E
        /// </summary>
        NumberSymbol = 1 << 6,//64


        /// <summary>
        /// 变量首字母
        /// </summary>
        VariableNameInitial = Letter | VariableSymbol,
        /// <summary>
        /// 有效的变量名字符
        /// </summary>
        ValidVariableName = Letter | VariableSymbol | Digit,
        /// <summary>
        /// 数字 0-9 +/-/.
        /// </summary>
        Number = Digit | NumberSymbol,
    }

}
