using System;
namespace Crylw.Core
{
	internal static class SystemEx
	{
		public unsafe static bool isNumber(this char c)
		{
			return (CharEx.P65536[c / '\u0001'] & 2) > 0;
		}
		public unsafe static bool isJsonName(this char c)
		{
			return (CharEx.P65536[c / '\u0001'] & 4) > 0;
		}
		public unsafe static bool isJsonDecode(this char c)
		{
			return (CharEx.P65536[c / '\u0001'] & 16) > 0;
		}
		public unsafe static bool isJsonEncode(this char c)
		{
			return (CharEx.P65536[c / '\u0001'] & 8) > 0;
		}
		public unsafe static bool isWhiteSpace(this char c)
		{
			return (CharEx.P65536[c / '\u0001'] & 1) > 0;
		}
		public unsafe static bool isStartJsonName(this char c)
		{
			return (CharEx.P65536[c / '\u0001'] & 32) > 0;
		}
		public static long getTime(this DateTime self)
		{
			return (self.ToUniversalTime().Ticks - 621355968000000000L) / 10000L;
		}
	}
}
