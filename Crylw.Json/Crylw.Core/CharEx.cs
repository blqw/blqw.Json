using System;
using System.Runtime.CompilerServices;
namespace Crylw.Core
{
	internal static class CharEx
	{
		public const byte _WhiteSpace = 1;
		public const byte _Number = 2;
		public const byte _JsonName = 4;
		public const byte _JsonEncode = 8;
		public const byte _JsonDecode = 16;
		public const byte _StartJsonName = 32;
		internal unsafe static readonly byte* P65536;
		internal unsafe static readonly char* PJsonEncode;
		internal unsafe static readonly int* PInt;
		internal unsafe static readonly uint* PUInt;
		internal unsafe static readonly long* PLong;
		internal unsafe static readonly ulong* PULong;
		internal unsafe static readonly char* PDigit;
		internal unsafe static readonly char* PJsonDecode;
		internal unsafe static readonly int* PLine;
		internal unsafe static readonly long* PNull;
		internal unsafe static readonly long* PDate;
		internal unsafe static readonly long* PTrue;
		internal unsafe static readonly long* PFalse;
		internal unsafe static readonly long* PSpace;
		internal unsafe static readonly int* PNameEnd;
		internal unsafe static readonly long* PUndefined;
		public static readonly string CString;
		unsafe static CharEx()
		{
			CharEx.P65536 = Memory.Offset(0);
			CharEx.PJsonEncode = (char*)Memory.Offset(65536);
			CharEx.PInt = (int*)Memory.Offset(65372);
			CharEx.PUInt = (uint*)Memory.Offset(65372);
			CharEx.PLong = (long*)Memory.Offset(65232);
			CharEx.PULong = (ulong*)Memory.Offset(65232);
			CharEx.PDigit = (char*)Memory.Offset(65700);
			CharEx.PJsonDecode = (char*)Memory.Offset(65722);
			CharEx.PLine = (int*)Memory.Offset(65820);
			CharEx.PNull = (long*)Memory.Offset(65824);
			CharEx.PDate = (long*)Memory.Offset(65832);
			CharEx.PTrue = (long*)Memory.Offset(65850);
			CharEx.PFalse = (long*)Memory.Offset(65858);
			CharEx.PSpace = (long*)Memory.Offset(65868);
			CharEx.PNameEnd = (int*)Memory.Offset(65876);
			CharEx.PUndefined = (long*)Memory.Offset(65880);
			CharEx.CString = "\r\nnullnew Date(truefalse    \":undefined";
			for (int i = 1; i < 19968; i++)
			{
				if (char.IsWhiteSpace((char)i) || char.IsControl((char)i))
				{
					CharEx.P65536[i / 1] = 1;
				}
				else
				{
					CharEx.P65536[i / 1] = 0;
				}
			}
			for (int j = 40870; j < 65536; j++)
			{
				if (char.IsWhiteSpace((char)j) || char.IsControl((char)j))
				{
					CharEx.P65536[j / 1] = 1;
				}
				else
				{
					CharEx.P65536[j / 1] = 0;
				}
			}
			for (int k = 48; k <= 57; k++)
			{
				CharEx.P65536[k / 1] = 6;
			}
			for (int l = 65; l <= 90; l++)
			{
				CharEx.P65536[l / 1] = 36;
			}
			for (int m = 97; m <= 122; m++)
			{
				CharEx.P65536[m / 1] = 36;
			}
			for (int n = 19968; n <= 40869; n++)
			{
				CharEx.P65536[n / 1] = 36;
			}
			*CharEx.P65536 = 0;
			CharEx.P65536[46] = 2;
			CharEx.P65536[36] = 36;
			CharEx.P65536[95] = 36;
			CharEx.P65536[101] = 38;
			CharEx.P65536[69] = 38;
			int num = 0;
			while (num <= 92)
			{
				if (num <= 34)
				{
					switch (num)
					{
					case 0:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = '0';
						break;
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
						goto IL_42D;
					case 7:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = 'a';
						break;
					case 8:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = 'b';
						break;
					case 9:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = 't';
						break;
					case 10:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = 'n';
						break;
					case 11:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = 'v';
						break;
					case 12:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = 'f';
						break;
					case 13:
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = 'r';
						break;
					default:
						if (num != 34)
						{
							goto IL_42D;
						}
						CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
						CharEx.PJsonEncode[num] = '"';
						break;
					}
				}
				else if (num != 39)
				{
					if (num != 92)
					{
						goto IL_42D;
					}
					CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
					CharEx.PJsonEncode[num] = '\\';
				}
				else
				{
					CharEx.P65536[num / 1] = (*(CharEx.P65536 + num / 1) | 8);
					CharEx.PJsonEncode[num] = '\'';
				}
				IL_43B:
				num++;
				continue;
				IL_42D:
				CharEx.PJsonEncode[num] = ' ';
				goto IL_43B;
			}
			int num2 = 0;
			while (num2 <= 118)
			{
				if (num2 <= 92)
				{
					if (num2 <= 39)
					{
						if (num2 != 34)
						{
							if (num2 != 39)
							{
								goto IL_692;
							}
							CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
							CharEx.PJsonDecode[num2] = '\'';
						}
						else
						{
							CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
							CharEx.PJsonDecode[num2] = '"';
						}
					}
					else if (num2 != 48)
					{
						if (num2 != 92)
						{
							goto IL_692;
						}
						CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
						CharEx.PJsonDecode[num2] = '\\';
					}
					else
					{
						CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
						CharEx.PJsonDecode[num2] = '\0';
					}
				}
				else if (num2 <= 98)
				{
					if (num2 != 97)
					{
						if (num2 != 98)
						{
							goto IL_692;
						}
						CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
						CharEx.PJsonDecode[num2] = '\b';
					}
					else
					{
						CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
						CharEx.PJsonDecode[num2] = '\a';
					}
				}
				else if (num2 != 102)
				{
					if (num2 != 110)
					{
						switch (num2)
						{
						case 114:
							CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
							CharEx.PJsonDecode[num2] = '\r';
							break;
						case 115:
						case 117:
							goto IL_692;
						case 116:
							CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
							CharEx.PJsonDecode[num2] = '\t';
							break;
						case 118:
							CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
							CharEx.PJsonDecode[num2] = '\v';
							break;
						default:
							goto IL_692;
						}
					}
					else
					{
						CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
						CharEx.PJsonDecode[num2] = '\n';
					}
				}
				else
				{
					CharEx.P65536[num2 / 1] = (*(CharEx.P65536 + num2 / 1) | 16);
					CharEx.PJsonDecode[num2] = '\f';
				}
				IL_6A0:
				num2++;
				continue;
				IL_692:
				CharEx.PJsonDecode[num2] = ' ';
				goto IL_6A0;
			}
			for (int num3 = 0; num3 < 10; num3++)
			{
				CharEx.PDigit[num3] = (char)(num3 + 48);
			}
			for (int num4 = 48; num4 <= 57; num4++)
			{
				CharEx.PLong[num4] = (long)(*(CharEx.PInt + num4) = num4 - 48);
			}
			fixed (string cString = CharEx.CString)
			{
				char* ptr = cString;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				SBuffer.wstrcpy((int*)ptr, CharEx.PLine, CharEx.CString.Length);
			}
		}
	}
}
