using System;
using System.Runtime.InteropServices;
using System.Security;

namespace blqw
{
    /// <summary> 用于存放所有Windows API的NativeMethods类
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class WinApi
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();
    }
}
