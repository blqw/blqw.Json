using Crylw.Core;
using System;
using System.IO;
namespace Crylw.Json
{
	public static class Json
	{
		public unsafe static string Encode(string text)
		{
			char[] array;
			char[] expr_0A = array = SBuffer.Acquire(1024);
			char* ptr;
			if (expr_0A == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr = &array[0])
				{
				}
			}
			return new SBuffer(expr_0A, ptr).AppendJsonEncode(text).GetStringAndRelease();
		}
		public unsafe static void Encode(string[] names)
		{
			char[] array;
			char[] expr_0A = array = SBuffer.Acquire(1024);
			char* ptr;
			if (expr_0A == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr = &array[0])
				{
				}
			}
			SBuffer sBuffer = new SBuffer(expr_0A, ptr);
			for (int i = 0; i < names.Length; i++)
			{
				names[i] = sBuffer.AppendJsonEncode(names[i]).GetStringAndClear();
			}
			sBuffer.Release();
			ptr = null;
		}
		public unsafe static string ToString(object obj, bool format)
		{
			char[] array;
			char[] expr_0A = array = SBuffer.Acquire(1024);
			char* ptr;
			if (expr_0A == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr = &array[0])
				{
				}
			}
			return new JsonWriter(new SBuffer(expr_0A, ptr), format ? (JsonWriterSettings.FormatOutput | JsonWriterSettings.EnumToNumber | JsonWriterSettings.BooleanToNumber | JsonWriterSettings.DateTimeToNumber) : JsonWriterSettings.Default).Value(obj).sb.GetStringAndRelease();
		}
		public unsafe static string ToString(object obj, JsonWriterSettings settings = JsonWriterSettings.Default)
		{
			char[] array;
			char[] expr_0A = array = SBuffer.Acquire(1024);
			char* ptr;
			if (expr_0A == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr = &array[0])
				{
				}
			}
			return new JsonWriter(new SBuffer(expr_0A, ptr), settings).Value(obj).sb.GetStringAndRelease();
		}
		public unsafe static void WriteTo(TextWriter writer, object obj, bool format)
		{
			if (writer == null)
			{
				Throw.ArgumentNull("writer", "ArgumentNull_Buffer", true);
			}
			char[] array;
			char[] expr_1D = array = SBuffer.Acquire(1024);
			char* ptr;
			if (expr_1D == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr = &array[0])
				{
				}
			}
			new JsonWriter(new SBuffer(expr_1D, ptr, writer), format ? (JsonWriterSettings.FormatOutput | JsonWriterSettings.EnumToNumber | JsonWriterSettings.BooleanToNumber | JsonWriterSettings.DateTimeToNumber) : JsonWriterSettings.Default).Value(obj).sb.FlushAndRelease();
			ptr = null;
		}
		public unsafe static void WriteTo(TextWriter writer, object obj, JsonWriterSettings settings = JsonWriterSettings.Default)
		{
			if (writer == null)
			{
				Throw.ArgumentNull("writer", "ArgumentNull_Buffer", true);
			}
			char[] array;
			char[] expr_1D = array = SBuffer.Acquire(1024);
			char* ptr;
			if (expr_1D == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr = &array[0])
				{
				}
			}
			new JsonWriter(new SBuffer(expr_1D, ptr, writer), settings).Value(obj).sb.FlushAndRelease();
			ptr = null;
		}
	}
}
