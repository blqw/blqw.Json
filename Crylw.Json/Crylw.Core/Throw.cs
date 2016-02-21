using System;
namespace Crylw.Core
{
	internal static class Throw
	{
		public static readonly Func<string, string> GetResourceString;
		static Throw()
		{
			Throw.GetResourceString = (Delegate.CreateDelegate(typeof(Func<string, string>), typeof(Environment), "GetResourceFromDefault", false, true) as Func<string, string>);
		}
		public static void Error(Exception error)
		{
			throw error;
		}
		public static void Error(string message = null)
		{
			throw new Exception(message);
		}
		public static void ArgumentNull(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}
		public static void ArgumentNull(string paramName, string message, bool isResource = false)
		{
			throw new ArgumentNullException(paramName, isResource ? Throw.GetResourceString(message) : message);
		}
		public static void ArgumentOutOfRange(string paramName)
		{
			throw new ArgumentOutOfRangeException(paramName);
		}
		public static void ArgumentOutOfRange(string paramName, string message, bool isResource = false)
		{
			throw new ArgumentOutOfRangeException(paramName, isResource ? Throw.GetResourceString(message) : message);
		}
		public static void NotSupported(string message, bool isResource = false)
		{
			throw new NotSupportedException(isResource ? Throw.GetResourceString(message) : message);
		}
	}
}
