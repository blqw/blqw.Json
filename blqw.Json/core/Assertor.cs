using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary> 断言器,用于判断参数值和抛出异常
    /// </summary>
    public static class Assertor
    {
        /// <summary> 如果value值是null 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name, "参数不能为null");
            }
        }

        /// <summary> 如果value字符串是null或string.Empty 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNullOrEmpty(string value, string name)
        {
            if (value == null || value.Length == 0)
            {
                throw new ArgumentNullException(name, "字符串不能为null或空");
            }
        }

        /// <summary> 如果value集合是null或者Count是0 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNullOrEmpty(ICollection value, string name)
        {
            if (value == null || value.Count == 0)
            {
                throw new ArgumentNullException(name, "集合不能为null且必须有元素");
            }
        }

        /// <summary> 如果value集合是null或者Count是0 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNullOrEmpty<T>(T[] value, string name)
        {
            if (value == null || value.Length == 0)
            {
                throw new ArgumentNullException(name, "集合不能为null且必须有元素");
            }
        }

        /// <summary> 如果value集合是null或者Count是0 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNullOrEmpty<T>(List<T> value, string name)
        {
            if (value == null || value.Count == 0)
            {
                throw new ArgumentNullException(name, "集合不能为null且必须有元素");
            }
        }

        /// <summary> 如果value集合是null或者Count是0 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNullOrEmpty<T>(ICollection<T> value, string name)
        {
            if (value == null || value.Count == 0)
            {
                throw new ArgumentNullException(name, "集合不能为null且必须有元素");
            }
        }

        /// <summary> 如果value字符串是 null、空还是仅由空白字符组成 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNullOrWhiteSpace(string value, string name)
        {
#if !NF2
            if (string.IsNullOrWhiteSpace(value))
#else
            if (value == null || value.Length == 0 ||
                (value[0] == ' ' && value.Trim().Length == 0))
#endif
            {
                throw new ArgumentNullException(name, "字符串不能为null或连续空白");
            }
        }

        /// <summary> 如果value非null,则value值超过min~max 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static void AreInRange<T>(Nullable<T> value, string name, T min, T max)
            where T : struct, IComparable
        {
            if (value.HasValue)
            {
                if (value.Value.CompareTo(min) < 0 || value.Value.CompareTo(max) > 0)
                {
                    throw new ArgumentOutOfRangeException(name, value, "值不能大于" + max + "或小于" + min);
                }
            }
        }

        /// <summary> 如果value值超过min~max 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static void AreInRange<T>(T value, string name, T min, T max)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "值不能大于" + max + "或小于" + min);
            }
        }

        /// <summary> 如果value值超过min~max 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static void AreInRange<T>(IComparable value, string name, T min, T max)
            where T : IComparable
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "值不能大于" + max + "或小于" + min);
            }
        }

        public static void AreInRange(Enum value, string name)
        {
            if (Enum.GetName(value.GetType(), value) == null)
            {
                throw new ArgumentOutOfRangeException(name, value, "值不在枚举中");
            }
        }

        /// <summary> 如果value不是T类型或其子类 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreType<T>(object value, string name)
        {
            if (value is T == false)
            {
                throw new ArgumentOutOfRangeException(name, value + " 值不是指定的类型 '" + typeof(T) + "'");
            }
        }

        /// <summary> 如果value不是Type类型或其子类 则抛出异常
        /// </summary>
        /// <param name="type">限定类型</param>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreType(Type type, object value, string name)
        {
            if (type.IsInstanceOfType(value) == false)
            {
                throw new ArgumentOutOfRangeException(name, value + " 值不是指定的类型 '" + type + "'");
            }
        }

        /// <summary> 如果value不是数字类型 则抛出异常
        /// </summary>
        /// <param name="value">参数值</param>
        /// <param name="name">参数名称</param>
        public static void AreNumberType(object value, string name)
        {
            var conv = value as IConvertible;
            if (conv == null)
            {
                var code = (int)conv.GetTypeCode();
                if (code < 5 || code > 15)
                {
                    throw new ArgumentOutOfRangeException(name, value + "不是数字类型");
                }
            }
        }

        /// <summary> 如果condition是true 则抛出异常
        /// </summary>
        /// <param name="condition">判断条件</param>
        /// <param name="message">异常消息</param>
        public static void AreTrue<T>(bool condition, string message)
            where T : Exception, new()
        {
            if (condition)
            {
                var ex = new T();
                typeof(Exception).GetField("_message", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ex, message);
                throw ex;
            }
        }

        /// <summary> 如果condition是false 则抛出异常
        /// </summary>
        /// <param name="condition">判断条件</param>
        /// <param name="message">异常消息</param>
        public static void AreFalse<T>(bool condition, string message)
            where T : Exception, new()
        {
            AreTrue<T>(!condition, message);
        }

        /// <summary> 如果condition是true 则抛出NotSupportedException异常
        /// </summary>
        /// <param name="condition">判断条件</param>
        /// <param name="message">异常消息</param>
        public static void AreTrue(bool condition, string message)
        {
            AreTrue<NotSupportedException>(condition, message);
        }

        /// <summary> 如果condition是false 则抛出NotSupportedException异常
        /// </summary>
        /// <param name="condition">判断条件</param>
        /// <param name="message">异常消息</param>
        public static void AreFalse(bool condition, string message)
        {
            AreTrue<NotSupportedException>(!condition, message);
        }
    }
}
