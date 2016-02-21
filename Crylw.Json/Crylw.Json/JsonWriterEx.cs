using Crylw.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace Crylw.Json
{
	internal class JsonWriterEx
	{
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			public static readonly JsonWriterEx.<>c <>9 = new JsonWriterEx.<>c();
			public static Action<JsonWriter, object> <>9__10_0;
			public static Action<JsonWriter, object> <>9__10_1;
			public static Action<JsonWriter, object> <>9__10_2;
			public static Action<JsonWriter, object> <>9__10_3;
			public static Action<JsonWriter, object> <>9__10_4;
			public static Action<JsonWriter, object> <>9__10_5;
			public static Action<JsonWriter, object> <>9__10_6;
			public static Action<JsonWriter, object> <>9__12_0;
			public static Action<JsonWriter, object> <>9__12_1;
			public static Action<JsonWriter, object> <>9__12_2;
			public static Action<JsonWriter, object> <>9__12_3;
			public static Action<JsonWriter, object> <>9__12_4;
			public static Action<JsonWriter, object> <>9__13_0;
			public static Action<JsonWriter, object> <>9__13_1;
			public static Action<JsonWriter, object> <>9__13_2;
			public static Action<JsonWriter, object> <>9__13_3;
			public static Action<JsonWriter, object> <>9__14_0;
			public static Action<JsonWriter, object> <>9__14_1;
			public static Action<JsonWriter, object> <>9__14_2;
			public static Action<JsonWriter, object> <>9__14_3;
			public static Action<JsonWriter, object> <>9__14_4;
			public static Action<JsonWriter, object> <>9__14_5;
			public static Action<JsonWriter, object> <>9__14_6;
			public static Action<JsonWriter, object> <>9__15_0;
			public static Action<JsonWriter, object> <>9__15_1;
			public static Action<JsonWriter, object> <>9__15_2;
			public static Action<JsonWriter, object> <>9__15_3;
			public static Action<JsonWriter, object> <>9__15_4;
			public static Action<JsonWriter, object> <>9__15_5;
			public static Action<JsonWriter, object> <>9__15_6;
			public static Action<JsonWriter, object> <>9__15_7;
			internal void cctor>b__9_0(JsonWriter jw, object value)
			{
				jw.Value((byte[])value);
			}
			internal void cctor>b__9_1(JsonWriter jw, object value)
			{
				jw.PreValue().Append((long)value);
			}
			internal void cctor>b__9_2(JsonWriter jw, object value)
			{
				jw.PreValue().Append((ulong)value);
			}
			internal void cctor>b__9_3(JsonWriter jw, object value)
			{
				jw.PreValue().Append('"').Append(value.ToString()).Append('"');
			}
			internal void <GetKnownEx>b__10_0(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as DataSet).ObjectEnd();
			}
			internal void <GetKnownEx>b__10_1(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as DataRow).ObjectEnd();
			}
			internal void <GetKnownEx>b__10_2(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as DataRowView).ObjectEnd();
			}
			internal void <GetKnownEx>b__10_3(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as NameValueCollection).ObjectEnd();
			}
			internal void <GetKnownEx>b__10_4(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as DataView).ArrayEnd();
			}
			internal void <GetKnownEx>b__10_5(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as DataTable).ArrayEnd();
			}
			internal void <GetKnownEx>b__10_6(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IDataReader).ArrayEnd();
			}
			internal void <GetArrayEx>b__12_0(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as int[]).ArrayEnd();
			}
			internal void <GetArrayEx>b__12_1(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as char[]).ArrayEnd();
			}
			internal void <GetArrayEx>b__12_2(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as string[]).ArrayEnd();
			}
			internal void <GetArrayEx>b__12_3(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as DateTime[]).ArrayEnd();
			}
			internal void <GetArrayEx>b__12_4(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerable).ArrayEnd();
			}
			internal void <GetIDictionaryEx>b__13_0(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerable<KeyValuePair<string, int>>).ObjectEnd();
			}
			internal void <GetIDictionaryEx>b__13_1(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerable<KeyValuePair<string, bool>>).ObjectEnd();
			}
			internal void <GetIDictionaryEx>b__13_2(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerable<KeyValuePair<string, string>>).ObjectEnd();
			}
			internal void <GetIDictionaryEx>b__13_3(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IDictionary).ObjectEnd();
			}
			internal void <GetIEnumerableEx>b__14_0(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerable<KeyValuePair<string, int>>).ObjectEnd();
			}
			internal void <GetIEnumerableEx>b__14_1(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerable<KeyValuePair<string, bool>>).ObjectEnd();
			}
			internal void <GetIEnumerableEx>b__14_2(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerable<KeyValuePair<string, string>>).ObjectEnd();
			}
			internal void <GetIEnumerableEx>b__14_3(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerable<int>).ArrayEnd();
			}
			internal void <GetIEnumerableEx>b__14_4(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerable<string>).ArrayEnd();
			}
			internal void <GetIEnumerableEx>b__14_5(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerable<DateTime>).ArrayEnd();
			}
			internal void <GetIEnumerableEx>b__14_6(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerable).ArrayEnd();
			}
			internal void <GetIEnumeratorEx>b__15_0(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerator<KeyValuePair<string, int>>).ObjectEnd();
			}
			internal void <GetIEnumeratorEx>b__15_1(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerator<KeyValuePair<string, bool>>).ObjectEnd();
			}
			internal void <GetIEnumeratorEx>b__15_2(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IEnumerator<KeyValuePair<string, string>>).ObjectEnd();
			}
			internal void <GetIEnumeratorEx>b__15_3(JsonWriter jw, object value)
			{
				jw.ObjectStart().Extend(value as IDictionaryEnumerator).ObjectEnd();
			}
			internal void <GetIEnumeratorEx>b__15_4(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerator<int>).ArrayEnd();
			}
			internal void <GetIEnumeratorEx>b__15_5(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerator<string>).ArrayEnd();
			}
			internal void <GetIEnumeratorEx>b__15_6(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerator<DateTime>).ArrayEnd();
			}
			internal void <GetIEnumeratorEx>b__15_7(JsonWriter jw, object value)
			{
				jw.ArrayStart().Concat(value as IEnumerator).ArrayEnd();
			}
		}
		public readonly int PropertyCount;
		public readonly bool IsSimpleType;
		public readonly bool IsCustomType;
		public readonly JsonWriterMember[] Members;
		public readonly Action<JsonWriter, object> Invoke;
		private static Dictionary<Type, JsonWriterEx> Cached;
		private JsonWriterEx(Type type)
		{
			this.IsCustomType = true;
			Dictionary<string, JsonWriterMember> dictionary = new Dictionary<string, JsonWriterMember>();
			PropertyInfo[] properties = type.GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() == null && propertyInfo.GetGetMethod(true) != null)
				{
					JsonWriterMember jsonWriterMember = new JsonWriterMember(null, propertyInfo);
					if (dictionary.ContainsKey(jsonWriterMember.JsonName))
					{
						Throw.Error("JsonName重复: " + jsonWriterMember.JsonName);
					}
					dictionary[jsonWriterMember.JsonName] = jsonWriterMember;
				}
			}
			this.PropertyCount = dictionary.Count;
			FieldInfo[] fields = type.GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				if (fieldInfo.GetCustomAttribute<JsonIgnoreAttribute>() == null)
				{
					JsonWriterMember jsonWriterMember2 = new JsonWriterMember(fieldInfo, null);
					if (dictionary.ContainsKey(jsonWriterMember2.JsonName))
					{
						Throw.Error("JsonName重复: " + jsonWriterMember2.JsonName);
					}
					dictionary[jsonWriterMember2.JsonName] = jsonWriterMember2;
				}
			}
			this.Members = new JsonWriterMember[dictionary.Count];
			dictionary.Values.CopyTo(this.Members, 0);
		}
		private JsonWriterEx(bool isSimpleType, Action<JsonWriter, object> invoke)
		{
			this.Invoke = invoke;
			this.IsSimpleType = isSimpleType;
		}
		public void WriteTo(JsonWriter jw, object instance)
		{
			jw.ObjectStart();
			int num = jw._SerializableField ? this.Members.Length : this.PropertyCount;
			for (int i = 0; i < num; i++)
			{
				JsonWriterMember jsonWriterMember = this.Members[i];
				jw.PreName().Append('"').Append(jsonWriterMember.JsonName).Append2(CharEx.PNameEnd);
				if (jsonWriterMember.FormatString == null)
				{
					jw.Value(jsonWriterMember.GetValue(instance));
				}
				else
				{
					jw.Value(jsonWriterMember.GetValue(instance) as IFormattable, jsonWriterMember.FormatString, jsonWriterMember.FormatProvider);
				}
			}
			jw.ObjectEnd();
		}
		static JsonWriterEx()
		{
			JsonWriterEx.Cached = new Dictionary<Type, JsonWriterEx>();
			JsonWriterEx.Cached.Add(typeof(byte[]), new JsonWriterEx(true, new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<.cctor>b__9_0)));
			JsonWriterEx.Cached.Add(typeof(IntPtr), new JsonWriterEx(true, new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<.cctor>b__9_1)));
			JsonWriterEx.Cached.Add(typeof(UIntPtr), new JsonWriterEx(true, new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<.cctor>b__9_2)));
			JsonWriterEx.Cached.Add(typeof(Guid), new JsonWriterEx(true, new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<.cctor>b__9_3)));
		}
		private static JsonWriterEx GetKnownEx(Type type)
		{
			bool isSimpleType;
			Action<JsonWriter, object> invoke;
			if (type.IsArray && JsonWriterEx.GetArrayEx(type, out isSimpleType, out invoke))
			{
				return new JsonWriterEx(isSimpleType, invoke);
			}
			if (typeof(DataSet).IsAssignableFrom(type))
			{
				bool arg_4E_0 = false;
				Action<JsonWriter, object> arg_4E_1;
				if ((arg_4E_1 = JsonWriterEx.<>c.<>9__10_0) == null)
				{
					arg_4E_1 = (JsonWriterEx.<>c.<>9__10_0 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetKnownEx>b__10_0));
				}
				return new JsonWriterEx(arg_4E_0, arg_4E_1);
			}
			if (typeof(DataRow).IsAssignableFrom(type))
			{
				bool arg_86_0 = false;
				Action<JsonWriter, object> arg_86_1;
				if ((arg_86_1 = JsonWriterEx.<>c.<>9__10_1) == null)
				{
					arg_86_1 = (JsonWriterEx.<>c.<>9__10_1 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetKnownEx>b__10_1));
				}
				return new JsonWriterEx(arg_86_0, arg_86_1);
			}
			if (typeof(DataRowView).IsAssignableFrom(type))
			{
				bool arg_BE_0 = false;
				Action<JsonWriter, object> arg_BE_1;
				if ((arg_BE_1 = JsonWriterEx.<>c.<>9__10_2) == null)
				{
					arg_BE_1 = (JsonWriterEx.<>c.<>9__10_2 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetKnownEx>b__10_2));
				}
				return new JsonWriterEx(arg_BE_0, arg_BE_1);
			}
			if (typeof(NameValueCollection).IsAssignableFrom(type))
			{
				bool arg_F6_0 = true;
				Action<JsonWriter, object> arg_F6_1;
				if ((arg_F6_1 = JsonWriterEx.<>c.<>9__10_3) == null)
				{
					arg_F6_1 = (JsonWriterEx.<>c.<>9__10_3 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetKnownEx>b__10_3));
				}
				return new JsonWriterEx(arg_F6_0, arg_F6_1);
			}
			if (typeof(DataView).IsAssignableFrom(type))
			{
				bool arg_12E_0 = false;
				Action<JsonWriter, object> arg_12E_1;
				if ((arg_12E_1 = JsonWriterEx.<>c.<>9__10_4) == null)
				{
					arg_12E_1 = (JsonWriterEx.<>c.<>9__10_4 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetKnownEx>b__10_4));
				}
				return new JsonWriterEx(arg_12E_0, arg_12E_1);
			}
			if (typeof(DataTable).IsAssignableFrom(type))
			{
				bool arg_166_0 = false;
				Action<JsonWriter, object> arg_166_1;
				if ((arg_166_1 = JsonWriterEx.<>c.<>9__10_5) == null)
				{
					arg_166_1 = (JsonWriterEx.<>c.<>9__10_5 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetKnownEx>b__10_5));
				}
				return new JsonWriterEx(arg_166_0, arg_166_1);
			}
			if (typeof(IDataReader).IsAssignableFrom(type))
			{
				bool arg_19E_0 = false;
				Action<JsonWriter, object> arg_19E_1;
				if ((arg_19E_1 = JsonWriterEx.<>c.<>9__10_6) == null)
				{
					arg_19E_1 = (JsonWriterEx.<>c.<>9__10_6 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetKnownEx>b__10_6));
				}
				return new JsonWriterEx(arg_19E_0, arg_19E_1);
			}
			if (typeof(IDictionary).IsAssignableFrom(type) && JsonWriterEx.GetIDictionaryEx(type, out isSimpleType, out invoke))
			{
				return new JsonWriterEx(isSimpleType, invoke);
			}
			if (typeof(IEnumerable).IsAssignableFrom(type) && JsonWriterEx.GetIEnumerableEx(type, out isSimpleType, out invoke))
			{
				return new JsonWriterEx(isSimpleType, invoke);
			}
			if (typeof(IEnumerator).IsAssignableFrom(type) && JsonWriterEx.GetIEnumerableEx(type, out isSimpleType, out invoke))
			{
				return new JsonWriterEx(isSimpleType, invoke);
			}
			return null;
		}
		public static JsonWriterEx GetEx(Type type)
		{
			JsonWriterEx jsonWriterEx;
			if (JsonWriterEx.Cached.TryGetValue(type, out jsonWriterEx))
			{
				return jsonWriterEx;
			}
			Dictionary<Type, JsonWriterEx> cached = JsonWriterEx.Cached;
			JsonWriterEx result;
			lock (cached)
			{
				if (JsonWriterEx.Cached.TryGetValue(type, out jsonWriterEx))
				{
					result = jsonWriterEx;
				}
				else
				{
					jsonWriterEx = (JsonWriterEx.GetKnownEx(type) ?? new JsonWriterEx(type));
					JsonWriterEx.Cached[type] = jsonWriterEx;
					result = jsonWriterEx;
				}
			}
			return result;
		}
		private static bool GetArrayEx(Type type, out bool simple, out Action<JsonWriter, object> invoke)
		{
			if (typeof(int[]).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_32_1;
				if ((arg_32_1 = JsonWriterEx.<>c.<>9__12_0) == null)
				{
					arg_32_1 = (JsonWriterEx.<>c.<>9__12_0 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetArrayEx>b__12_0));
				}
				invoke = arg_32_1;
				return simple = true;
			}
			if (typeof(char[]).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_6C_1;
				if ((arg_6C_1 = JsonWriterEx.<>c.<>9__12_1) == null)
				{
					arg_6C_1 = (JsonWriterEx.<>c.<>9__12_1 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetArrayEx>b__12_1));
				}
				invoke = arg_6C_1;
				return simple = true;
			}
			if (typeof(string[]).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_A6_1;
				if ((arg_A6_1 = JsonWriterEx.<>c.<>9__12_2) == null)
				{
					arg_A6_1 = (JsonWriterEx.<>c.<>9__12_2 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetArrayEx>b__12_2));
				}
				invoke = arg_A6_1;
				return simple = true;
			}
			if (typeof(DateTime[]).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_E0_1;
				if ((arg_E0_1 = JsonWriterEx.<>c.<>9__12_3) == null)
				{
					arg_E0_1 = (JsonWriterEx.<>c.<>9__12_3 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetArrayEx>b__12_3));
				}
				invoke = arg_E0_1;
				return simple = true;
			}
			Action<JsonWriter, object> arg_108_1;
			if ((arg_108_1 = JsonWriterEx.<>c.<>9__12_4) == null)
			{
				arg_108_1 = (JsonWriterEx.<>c.<>9__12_4 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetArrayEx>b__12_4));
			}
			invoke = arg_108_1;
			simple = false;
			return true;
		}
		private static bool GetIDictionaryEx(Type type, out bool simple, out Action<JsonWriter, object> invoke)
		{
			if (typeof(IEnumerable<KeyValuePair<string, int>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_32_1;
				if ((arg_32_1 = JsonWriterEx.<>c.<>9__13_0) == null)
				{
					arg_32_1 = (JsonWriterEx.<>c.<>9__13_0 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIDictionaryEx>b__13_0));
				}
				invoke = arg_32_1;
				return simple = true;
			}
			if (typeof(IEnumerable<KeyValuePair<string, bool>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_6C_1;
				if ((arg_6C_1 = JsonWriterEx.<>c.<>9__13_1) == null)
				{
					arg_6C_1 = (JsonWriterEx.<>c.<>9__13_1 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIDictionaryEx>b__13_1));
				}
				invoke = arg_6C_1;
				return simple = true;
			}
			if (typeof(IEnumerable<KeyValuePair<string, string>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_A6_1;
				if ((arg_A6_1 = JsonWriterEx.<>c.<>9__13_2) == null)
				{
					arg_A6_1 = (JsonWriterEx.<>c.<>9__13_2 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIDictionaryEx>b__13_2));
				}
				invoke = arg_A6_1;
				return simple = true;
			}
			Action<JsonWriter, object> arg_CE_1;
			if ((arg_CE_1 = JsonWriterEx.<>c.<>9__13_3) == null)
			{
				arg_CE_1 = (JsonWriterEx.<>c.<>9__13_3 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIDictionaryEx>b__13_3));
			}
			invoke = arg_CE_1;
			simple = false;
			return true;
		}
		private static bool GetIEnumerableEx(Type type, out bool simple, out Action<JsonWriter, object> invoke)
		{
			if (typeof(IEnumerable<KeyValuePair<string, int>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_32_1;
				if ((arg_32_1 = JsonWriterEx.<>c.<>9__14_0) == null)
				{
					arg_32_1 = (JsonWriterEx.<>c.<>9__14_0 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumerableEx>b__14_0));
				}
				invoke = arg_32_1;
				return simple = true;
			}
			if (typeof(IEnumerable<KeyValuePair<string, bool>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_6C_1;
				if ((arg_6C_1 = JsonWriterEx.<>c.<>9__14_1) == null)
				{
					arg_6C_1 = (JsonWriterEx.<>c.<>9__14_1 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumerableEx>b__14_1));
				}
				invoke = arg_6C_1;
				return simple = true;
			}
			if (typeof(IEnumerable<KeyValuePair<string, string>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_A6_1;
				if ((arg_A6_1 = JsonWriterEx.<>c.<>9__14_2) == null)
				{
					arg_A6_1 = (JsonWriterEx.<>c.<>9__14_2 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumerableEx>b__14_2));
				}
				invoke = arg_A6_1;
				return simple = true;
			}
			if (typeof(IEnumerable<int>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_E0_1;
				if ((arg_E0_1 = JsonWriterEx.<>c.<>9__14_3) == null)
				{
					arg_E0_1 = (JsonWriterEx.<>c.<>9__14_3 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumerableEx>b__14_3));
				}
				invoke = arg_E0_1;
				return simple = true;
			}
			if (typeof(IEnumerable<string>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_11A_1;
				if ((arg_11A_1 = JsonWriterEx.<>c.<>9__14_4) == null)
				{
					arg_11A_1 = (JsonWriterEx.<>c.<>9__14_4 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumerableEx>b__14_4));
				}
				invoke = arg_11A_1;
				return simple = true;
			}
			if (typeof(IEnumerable<DateTime>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_154_1;
				if ((arg_154_1 = JsonWriterEx.<>c.<>9__14_5) == null)
				{
					arg_154_1 = (JsonWriterEx.<>c.<>9__14_5 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumerableEx>b__14_5));
				}
				invoke = arg_154_1;
				return simple = true;
			}
			Action<JsonWriter, object> arg_17C_1;
			if ((arg_17C_1 = JsonWriterEx.<>c.<>9__14_6) == null)
			{
				arg_17C_1 = (JsonWriterEx.<>c.<>9__14_6 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumerableEx>b__14_6));
			}
			invoke = arg_17C_1;
			simple = false;
			return true;
		}
		private static bool GetIEnumeratorEx(Type type, out bool simple, out Action<JsonWriter, object> invoke)
		{
			if (typeof(IEnumerator<KeyValuePair<string, int>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_32_1;
				if ((arg_32_1 = JsonWriterEx.<>c.<>9__15_0) == null)
				{
					arg_32_1 = (JsonWriterEx.<>c.<>9__15_0 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_0));
				}
				invoke = arg_32_1;
				return simple = true;
			}
			if (typeof(IEnumerator<KeyValuePair<string, bool>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_6C_1;
				if ((arg_6C_1 = JsonWriterEx.<>c.<>9__15_1) == null)
				{
					arg_6C_1 = (JsonWriterEx.<>c.<>9__15_1 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_1));
				}
				invoke = arg_6C_1;
				return simple = true;
			}
			if (typeof(IEnumerator<KeyValuePair<string, string>>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_A6_1;
				if ((arg_A6_1 = JsonWriterEx.<>c.<>9__15_2) == null)
				{
					arg_A6_1 = (JsonWriterEx.<>c.<>9__15_2 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_2));
				}
				invoke = arg_A6_1;
				return simple = true;
			}
			if (typeof(IDictionaryEnumerator).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_E0_1;
				if ((arg_E0_1 = JsonWriterEx.<>c.<>9__15_3) == null)
				{
					arg_E0_1 = (JsonWriterEx.<>c.<>9__15_3 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_3));
				}
				invoke = arg_E0_1;
				simple = false;
				return true;
			}
			if (typeof(IEnumerator<int>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_118_1;
				if ((arg_118_1 = JsonWriterEx.<>c.<>9__15_4) == null)
				{
					arg_118_1 = (JsonWriterEx.<>c.<>9__15_4 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_4));
				}
				invoke = arg_118_1;
				return simple = true;
			}
			if (typeof(IEnumerator<string>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_152_1;
				if ((arg_152_1 = JsonWriterEx.<>c.<>9__15_5) == null)
				{
					arg_152_1 = (JsonWriterEx.<>c.<>9__15_5 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_5));
				}
				invoke = arg_152_1;
				return simple = true;
			}
			if (typeof(IEnumerator<DateTime>).IsAssignableFrom(type))
			{
				Action<JsonWriter, object> arg_18C_1;
				if ((arg_18C_1 = JsonWriterEx.<>c.<>9__15_6) == null)
				{
					arg_18C_1 = (JsonWriterEx.<>c.<>9__15_6 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_6));
				}
				invoke = arg_18C_1;
				return simple = true;
			}
			Action<JsonWriter, object> arg_1B4_1;
			if ((arg_1B4_1 = JsonWriterEx.<>c.<>9__15_7) == null)
			{
				arg_1B4_1 = (JsonWriterEx.<>c.<>9__15_7 = new Action<JsonWriter, object>(JsonWriterEx.<>c.<>9.<GetIEnumeratorEx>b__15_7));
			}
			invoke = arg_1B4_1;
			simple = false;
			return true;
		}
	}
}
