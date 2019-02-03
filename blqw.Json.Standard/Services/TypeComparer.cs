using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;

namespace blqw.JsonServices
{
    public sealed class TypeComparer : IComparer<Type>
    {
        public static TypeComparer Instance { get; } = new TypeComparer();

        private TypeComparer()
        {

        }

        /// <summary>
        /// 比较两个对象并返回一个值，该值指示一个对象小于、等于还是大于另一个对象。
        /// </summary>
        /// <returns>
        /// 一个有符号整数，指示 <paramref name="x" /> 与 <paramref name="y" /> 的相对值，如下表所示。值含义小于零<paramref name="x" /> 小于
        /// <paramref name="y" />。零<paramref name="x" /> 等于 <paramref name="y" />。大于零<paramref name="x" /> 大于 <paramref name="y" />
        /// 。
        /// </returns>
        /// <param name="x"> 要比较的第一个对象。 </param>
        /// <param name="y"> 要比较的第二个对象。 </param>
        public int Compare(Type x, Type y)
        {
            var a = GetPriority(x);
            var b = GetPriority(y);
            return a.CompareTo(b);
        }



        /// <summary>
        /// 部分类型优先级调整
        /// </summary>
        private static readonly Dictionary<Type, int> _priorities = new Dictionary<Type, int>
        {
            [typeof(IObjectReference)] = 400,
            [typeof(IFormatProvider)] = 300,
            [typeof(IDictionary<,>)] = 200,
            [typeof(IDictionary)] = 199,
            [typeof(IEnumerable<>)] = 99,
            [typeof(IEnumerable)] = 98,
            [typeof(IEnumerator<>)] = 97,
            [typeof(IEnumerator)] = 96,
            [typeof(DynamicObject)] = 95
        };

        /// <summary>
        /// 获取类型的优先级
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        private static int GetPriority(Type type)
        {
            if (type == null)
            {
                return 0;
            }
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition() ?? type;
            }
            return _priorities.TryGetValue(type, out var i) ? i : 100;
        }


    }
}
