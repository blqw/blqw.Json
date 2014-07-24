using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace blqw
{
    public class Disposable
    {
        public Disposable(object obj)
        {
            _obj = obj;
            _disposeMark = 0;
        }

        protected Disposable()
        {
            _obj = this;
            _auto = true;
            _disposeMark = 0;
        }

        ~Disposable()
        {
            if (_auto)
            {
                Destructor();
            }
        }

        public void Destructor()
        {
            //如果尚未释放对象(标记为0),则将标记改为2,否则标记不变
            Interlocked.CompareExchange(ref _disposeMark, 2, 0);
            Dispose();
        }
        private object _obj;
        private readonly bool _auto;
        /// <summary> 释放标记,0未释放,1已释放,2执行了析构函数
        /// </summary>
        private int _disposeMark;

        /// <summary> 释放非托管资源
        /// </summary>
        public void Dispose()
        {
            //如果已释放(标记为1)则不执行任何操作
            if (_disposeMark == 1)
            {
                return;
            }
            //将标记改为1,并返回修改之前的值
            var mark = Interlocked.Exchange(ref _disposeMark, 1);
            //如果当前方法被多个线程同时执行,确保仅执行其中的一个
            if (mark == 1)
            {
                return;
            }

            //释放非托管资源
            OnEvent(_disposeUnmanaged);
            _disposeUnmanaged = null;
            if (mark == 0)
            {
                //释放托管资源
                OnEvent(_disposeManaged);
                _disposeManaged = null;
                GC.SuppressFinalize(_obj);
            }
        }

        public bool IsDisposed
        {
            get
            {
                return _disposeMark != 0;
            }
        }

        public void Assert()
        {
            if (_disposeMark != 0)
            {
                if (_obj is string)
                {
                    throw new ObjectDisposedException(_obj + "");
                }
                else
                {
                    throw new ObjectDisposedException(_obj.GetType().FullName);
                }
            }
        }

        private void OnEvent(ThreadStart start)
        {
            var eve = start;
            if (eve != null)
            {
                var dele = eve.GetInvocationList();
                var length = dele.Length;
                for (int i = 0; i < length; i++)
                {
                    try
                    {
                        ((ThreadStart)dele[i])();
                    }
                    catch { }
                }
            }
        }

        private event ThreadStart _disposeManaged;

        private event ThreadStart _disposeUnmanaged;

        public event ThreadStart DisposeManaged
        {
            add
            {
                _disposeManaged -= value;
                _disposeManaged += value;
            }
            remove
            {
                _disposeManaged += value;
            }
        }

        public event ThreadStart DisposeUnmanaged
        {
            add
            {
                _disposeUnmanaged -= value;
                _disposeUnmanaged += value;
            }
            remove
            {
                _disposeUnmanaged += value;
            }
        }
    }
}
