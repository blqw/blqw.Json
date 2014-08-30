using System;
using System.Threading;

namespace blqw
{
    /// <summary> 使用using代替lock操作的对象,可指定写入和读取锁定模式
    /// </summary>
    public class UsingLock2 : Disposable, IDisposable
    {
        #region 内部类

        /// <summary> 利用IDisposable的using语法糖方便的释放锁定操作
        /// <para>内部类</para>
        /// </summary>
        private struct Lock : IDisposable
        {
            /// <summary> 读写锁对象
            /// </summary>
            private ReaderWriterLock _Lock;
            /// <summary> 是否为写入模式
            /// </summary>
            private bool _IsWrite;
            /// <summary> 利用IDisposable的using语法糖方便的释放锁定操作
            /// <para>构造函数</para>
            /// </summary>
            /// <param name="rwl">读写锁</param>
            /// <param name="isWrite">写入模式为true,读取模式为false</param>
            public Lock(ReaderWriterLock rwl, bool isWrite)
            {
                _Lock = rwl;
                _IsWrite = isWrite;
            }
            /// <summary> 释放对象时退出指定锁定模式
            /// </summary>
            public void Dispose()
            {
                if (_IsWrite)
                {
                    if (_Lock.IsWriterLockHeld)
                    {
                        _Lock.ReleaseWriterLock();
                    }
                }
                else
                {
                    if (_Lock.IsReaderLockHeld)
                    {
                        _Lock.ReleaseReaderLock();
                    }
                }
            }
        }

        /// <summary> 空的可释放对象,免去了调用时需要判断是否为null的问题
        /// <para>内部类</para>
        /// </summary>
        private class Disposable : IDisposable
        {
            /// <summary> 空的可释放对象
            /// </summary>
            public static readonly Disposable Empty = new Disposable();
            /// <summary> 空的释放方法
            /// </summary>
            public void Dispose() { }
        }

        #endregion

        /// <summary> 读写锁
        /// </summary>
        private ReaderWriterLock _lockSlim = new ReaderWriterLock();

        /// <summary> 使用using代替lock操作的对象,可指定写入和读取锁定模式
        /// <para>构造函数</para>
        /// </summary>
        public UsingLock2()
        {
            Enabled = true;
        }

        public bool IsReadLocked { get { return _lockSlim.IsReaderLockHeld; } }
        public bool IsWriteLocked { get { return _lockSlim.IsWriterLockHeld; } }

        /// <summary> 是否启用,当该值为false时,Read()和Write()方法将返回 Disposable.Empty
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary> 进入读取锁定模式,该模式下允许多个读操作同时进行
        /// <para>退出读锁请将返回对象释放,建议使用using语块</para>
        /// <para>Enabled为false时,返回Disposable.Empty;</para>
        /// <para>在读取或写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        public IDisposable Read()
        {
            if (Enabled == false || _lockSlim.IsReaderLockHeld || _lockSlim.IsWriterLockHeld)
            {
                return Disposable.Empty;
            }
            else
            {
                _lockSlim.AcquireReaderLock(-1);
                return new Lock(_lockSlim, false);
            }
        }
        
        /// <summary> 进入写入锁定模式,该模式下只允许同时执行一个读操作
        /// <para>退出读锁请将返回对象释放,建议使用using语块</para>
        /// <para>Enabled为false时,返回Disposable.Empty;</para>
        /// <para>在写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        /// <exception cref="NotImplementedException">读取模式下不能进入写入锁定状态</exception>
        public IDisposable Write()
        {
            if (Enabled == false || _lockSlim.IsWriterLockHeld)
            {
                return Disposable.Empty;
            }
            else if (_lockSlim.IsReaderLockHeld)
            {
                throw new NotImplementedException("读取模式下不能进入写入锁定状态");
            }
            else
            {
                _lockSlim.AcquireWriterLock(-1);
                return new Lock(_lockSlim, true);
            }
        }
    }

    /// <summary> 使用using代替lock操作的对象,可指定写入和读取锁定模式
    /// </summary>
    public sealed class UsingLock2<T> : UsingLock2
    {
        /// <summary> 保存数据
        /// </summary>
        private T _Data;

        /// <summary> 使用using代替lock操作的对象,可指定写入和读取锁定模式
        /// <para>构造函数</para>
        /// <param name="data">为Data属性设置初始值</param>
        public UsingLock2(T data)
        {
            Enabled = true;
            _Data = data;
        }

        /// <summary> 获取或设置当前对象中保存数据的值
        /// </summary>
        /// <exception cref="MemberAccessException">获取数据时未进入读取或写入锁定模式</exception>
        /// <exception cref="MemberAccessException">设置数据时未进入写入锁定模式</exception>
        public T Data
        {
            get
            {
                if (IsReadLocked || IsWriteLocked)
                {
                    return _Data;
                }
                throw new MemberAccessException("请先进入读取或写入锁定模式再进行操作");
            }
            set
            {
                if (IsWriteLocked == false)
                {
                    throw new MemberAccessException("只有写入锁定模式中才能改变Data的值");
                }
                _Data = value;
            }
        }
    }
}