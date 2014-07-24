using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 计数器值改变事件的参数
    /// </summary>
    public class CounterChangedEventArgs : EventArgs
    {

        internal CounterChangedEventArgs(int value,int oldValue)
        {
            Value = value;
            OldValue = oldValue;
        }
        /// <summary> 当前值
        /// </summary>
        public int Value { get; private set; }
        /// <summary> 原值
        /// </summary>
        public int OldValue { get; private set; }
    }
}
