using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace blqw
{
    /// <summary> 计数器,具有单线程模式和多线程模式
    /// </summary>
    public sealed class Counter
    {
        /// <summary> 构造一个计数器,默认单线程模式
        /// <para>无论在任何线程中每次执行Add方法都会增加引用数,执行Remove或者token.Dispose都会减少引用数</para>
        /// </summary>
        public Counter()
            : this(false)
        {

        }
        /// <summary> 构造一个计数器,根据参数multiThreadMode确定是否使用多线程模式
        /// <para>多线程模式:在相同线程中,只有第一次执行Add方法时增加引用数,也只有此token被Remove或Dispose才会减少引用数</para>
        /// </summary>
        /// <param name="multiThreadMode"></param>
        public Counter(bool multiThreadMode)
        {
            if (multiThreadMode)
            {
                _dataSlot = Thread.AllocateDataSlot();
            }
        }

        /// <summary> 当前引用数
        /// </summary>
        private int _value;
        /// <summary> 值改变事件
        /// </summary>
        private EventHandler<CounterChangedEventArgs> _valueChanged;
        /// <summary> 用于储存多线程间的独立数据,多线程模式下有值
        /// </summary>
        private LocalDataStoreSlot _dataSlot;

        /// <summary> 增加引用,并获取用于释放引用的标记
        /// </summary>
        public IDisposable Add()
        {
            if (_dataSlot != null)
            {
                //获取当前线程中的值,此方法每个线程中获得的值都不同,不需要线程同步
                //如果已经存在,则不计数
                if (Thread.GetData(_dataSlot) != null)
                {
                    return null;
                }
                Thread.SetData(_dataSlot, string.Empty);
            }
            return new CounterToken(this);
        }

        /// <summary> 减少引用
        /// </summary>
        /// <param name="token">通过Add方法获取的标记对象</param>
        public void Remove(IDisposable token)
        {
            if (token == null)
            {
                return;
            }
            if (token is CounterToken == false)
            {
                throw new ArgumentException("参数不是一个有效的引用标记", "token");
            }
            if (token.Equals(this) == false)
            {
                throw new ArgumentOutOfRangeException("token", "此标记不属于当前计数器");
            }
            token.Dispose();
        }

        /// <summary> 获取当前计数值
        /// </summary>
        public int Value
        {
            get { return _value; }
        }

        /// <summary> 增加记数
        /// </summary>
        private void OnIncrement()
        {
            var val = Interlocked.Increment(ref _value);
            OnValueChanged(val, val - 1);
        }
        /// <summary> 减少计数
        /// </summary>
        private void OnDecrement()
        {
            if (_dataSlot != null)
            {
                Thread.SetData(_dataSlot, null);
            }
            var val = Interlocked.Decrement(ref _value);
            OnValueChanged(val, val + 1);
        }
        /// <summary> 触发ValueChaged事件
        /// </summary>
        /// <param name="value">触发Value事件时Value的值</param>
        /// <param name="oldValue">触发Value事件之前Value的值</param>
        private void OnValueChanged(int value, int oldValue)
        {
            var handler = _valueChanged;
            if (handler != null)
            {
                var e = new CounterChangedEventArgs(value, oldValue);
                handler(this, e);
            }
        }
        /// <summary> 计数器值改变事件
        /// </summary>
        public event EventHandler<CounterChangedEventArgs> ValueChanged
        {
            add
            {
                _valueChanged -= value;
                _valueChanged += value;
            }
            remove
            {
                _valueChanged -= value;
            }
        }

        /// <summary> 计数器引用标记,调用计数器的Add方法可获得该对象,释放对象时,减少计数器的计数值
        /// </summary>
        private sealed class CounterToken : Disposable
        {
            /// <summary> 宿主计数器
            /// </summary>
            private Counter _counter;
            /// <summary> 构造函数,创建引用标记并增加宿主计数器的值
            /// </summary>
            /// <param name="counter">宿主计数器</param>
            public CounterToken(Counter counter)
            {
                if (counter == null)
                {
                    throw new ArgumentNullException("counter");
                }
                _counter = counter;
                _counter.OnIncrement();
                base.DisposeManaged += _counter.OnDecrement;
            }
            /// <summary> 重新实现比较的方法
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj is Counter)
                {
                    return object.ReferenceEquals(this._counter, obj);
                }
                return object.ReferenceEquals(this, obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }

    /// <summary> 计数器值改变事件的参数
    /// </summary>
    public class CounterChangedEventArgs : EventArgs
    {
        internal CounterChangedEventArgs(int value, int oldValue)
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
