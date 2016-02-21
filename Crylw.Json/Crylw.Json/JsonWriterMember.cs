using Crylw.Core;
using System;
using System.Reflection;
namespace Crylw.Json
{
	internal class JsonWriterMember
	{
		public readonly string FormatString;
		public readonly IFormatProvider FormatProvider;
		public readonly Func<object, object> GetValue;
		public readonly string JsonName;
		public JsonWriterMember(FieldInfo f, PropertyInfo p)
		{
			MemberInfo memberInfo;
			Type type;
			if (f != null)
			{
				memberInfo = f;
				type = f.FieldType;
				this.GetValue = EmitEx.CreateGet(f, null);
			}
			else
			{
				memberInfo = p;
				type = p.PropertyType;
				this.GetValue = EmitEx.CreateGet(p, null);
			}
			JsonNameAttribute customAttribute = memberInfo.GetCustomAttribute<JsonNameAttribute>();
			this.JsonName = ((customAttribute != null) ? Json.Encode(customAttribute.Name) : memberInfo.Name);
			JsonFormatAttribute customAttribute2 = memberInfo.GetCustomAttribute<JsonFormatAttribute>();
			if (customAttribute2 != null && typeof(IFormattable).IsAssignableFrom(Nullable.GetUnderlyingType(type) ?? type))
			{
				this.FormatString = customAttribute2.Format;
				this.FormatProvider = customAttribute2.Provider;
			}
		}
	}
}
