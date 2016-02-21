using System;
using System.Reflection;
using System.Reflection.Emit;
namespace Crylw.Core
{
	internal static class EmitEx
	{
		private static readonly Type TypeObject = typeof(object);
		private static readonly Type[] TypesObject = new Type[]
		{
			typeof(object)
		};
		private static readonly Type[] TypesObjects = new Type[]
		{
			typeof(object[])
		};
		private static readonly Type[] TypesObjectObject = new Type[]
		{
			typeof(object),
			typeof(object)
		};
		private static readonly Type[] TypesObjectObjects = new Type[]
		{
			typeof(object),
			typeof(object[])
		};
		private static void EmitCast(ILGenerator il, Type type, bool check = true)
		{
			if (type.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, type);
				if (check && Nullable.GetUnderlyingType(type) == null)
				{
					LocalBuilder local = il.DeclareLocal(type);
					il.Emit(OpCodes.Stloc, local);
					il.Emit(OpCodes.Ldloca_S, local);
					return;
				}
			}
			else
			{
				il.Emit(OpCodes.Castclass, type);
			}
		}
		public static Func<object, object> CreateGet(FieldInfo f, Type owner = null)
		{
			if (f == null)
			{
				return null;
			}
			DynamicMethod expr_2B = new DynamicMethod("", EmitEx.TypeObject, EmitEx.TypesObject, owner ?? f.ReflectedType, true);
			ILGenerator iLGenerator = expr_2B.GetILGenerator();
			if (f.IsStatic)
			{
				iLGenerator.Emit(OpCodes.Ldsfld, f);
			}
			else
			{
				iLGenerator.Emit(OpCodes.Ldarg_0);
				EmitEx.EmitCast(iLGenerator, f.DeclaringType, true);
				iLGenerator.Emit(OpCodes.Ldfld, f);
			}
			if (f.FieldType.IsValueType)
			{
				iLGenerator.Emit(OpCodes.Box, f.FieldType);
			}
			iLGenerator.Emit(OpCodes.Ret);
			return (Func<object, object>)expr_2B.CreateDelegate(typeof(Func<object, object>));
		}
		public static Func<object, object> CreateGet(PropertyInfo p, Type owner = null)
		{
			if (p == null)
			{
				return null;
			}
			DynamicMethod dynamicMethod = new DynamicMethod("", EmitEx.TypeObject, EmitEx.TypesObject, owner ?? p.ReflectedType, true);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			MethodInfo getMethod = p.GetGetMethod(true);
			if (getMethod == null)
			{
				return null;
			}
			if (getMethod.IsStatic)
			{
				iLGenerator.Emit(OpCodes.Call, getMethod);
			}
			else
			{
				iLGenerator.Emit(OpCodes.Ldarg_0);
				EmitEx.EmitCast(iLGenerator, p.DeclaringType, true);
				if (p.DeclaringType.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Call, getMethod);
				}
				else
				{
					iLGenerator.Emit(OpCodes.Callvirt, getMethod);
				}
			}
			if (p.PropertyType.IsValueType)
			{
				iLGenerator.Emit(OpCodes.Box, p.PropertyType);
			}
			iLGenerator.Emit(OpCodes.Ret);
			return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
		}
	}
}
