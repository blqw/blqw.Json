using System;
namespace Crylw.Json
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class JsonIgnoreAttribute : Attribute
	{
		public bool NonDeserialize
		{
			get;
			private set;
		}
		public JsonIgnoreAttribute() : this(false)
		{
		}
		public JsonIgnoreAttribute(bool nonDeserialize)
		{
			this.NonDeserialize = nonDeserialize;
		}
	}
}
