using System;
namespace Crylw.Json
{
	[Flags]
	public enum JsonWriterSettings
	{
		None = 1,
		IgnoreNull = 2,
		FormatOutput = 4,
		EnumToNumber = 8,
		NameAllowNull = 16,
		BooleanToNumber = 32,
		DateTimeToNumber = 64,
		SerializableField = 128,
		CheckLoopReference = 256,
		Default = 104
	}
}
