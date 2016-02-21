using System;
namespace Crylw.Json
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class JsonNameAttribute : Attribute
	{
		public string Name
		{
			get;
			private set;
		}
		public JsonNameAttribute(string name)
		{
			this.Name = name;
		}
	}
}
