using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
namespace Crylw.Core
{
	internal class Memory : SafeHandleZeroOrMinusOneIsInvalid
	{
		private static Memory Fixed;
		private unsafe static readonly byte* Pointer;
		public const int Size = 65960;
		public Memory(int size) : base(true)
		{
			base.SetHandle(Marshal.AllocHGlobal(size));
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			Marshal.FreeHGlobal(this.handle);
			return true;
		}
		unsafe static Memory()
		{
			Memory.Fixed = new Memory(65960);
			Memory.Pointer = (byte*)Memory.Fixed.DangerousGetHandle().ToPointer();
		}
		internal unsafe static byte* Offset(int offset)
		{
			return Memory.Pointer + offset / 1;
		}
	}
}
