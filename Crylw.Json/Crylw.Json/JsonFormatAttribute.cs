using System;
namespace Crylw.Json
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class JsonFormatAttribute : Attribute
	{
		public string Format
		{
			get;
			private set;
		}
		public IFormatProvider Provider
		{
			get;
			private set;
		}
		public JsonFormatAttribute(string format, IFormatProvider provider = null)
		{
			this.Format = format;
			this.Provider = provider;
		}
	}
}
