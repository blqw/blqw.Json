using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;
using blqw;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class BugTest
    {
        #region model
        public class BaiduTQ
        {
            public int error { get; set; }
            public string status { get; set; }
            public string date { get; set; }
            public List<BaiduResult> results { get; set; }
        }

        public class BaiduResult
        {
            public string currentCity { get; set; }
            public string pm25 { get; set; }
            public List<BaiduIndex> index { get; set; }
            public List<BaiDuWeaterData> weather_data { get; set; }
        }

        public class BaiduIndex
        {
            public string title { get; set; }
            public string zs { get; set; }
            public string tipt { get; set; }
            public string des { get; set; }
        }

        public class BaiDuWeaterData
        {
            public string date { get; set; }
            public string dayPictureUrl { get; set; }
            public string nightPictureUrl { get; set; }
            public string weather { get; set; }
            public string wind { get; set; }
            public string temperature { get; set; }
        } 
        #endregion

       
        [TestMethod]
        public void Test_20141128()
        {
            var a = new { a = -10 };
            var c = a.ToJsonString();
            Assert.AreEqual("{\"a\":-10}", c);
        }

        [TestMethod]
        public void Debug_20161013()
        {
            var a = new MyClass();
            var c = a.ToJsonString(JsonBuilderSettings.Default^JsonBuilderSettings.IgnoreNullMember);
            Assert.AreEqual("null", c);
        }

        class MyClass:IConvertible
        {
            /// <summary>返回此实例的 <see cref="T:System.TypeCode" />。</summary>
            /// <returns>枚举常数，它是实现该接口的类或值类型的 <see cref="T:System.TypeCode" />。</returns>
            public TypeCode GetTypeCode() => TypeCode.Object;

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 Boolean 值。</summary>
            /// <returns>与此实例的值等效的 Boolean 值。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public bool ToBoolean(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 Unicode 字符。</summary>
            /// <returns>与此实例的值等效的 Unicode 字符。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public char ToChar(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 8 位有符号整数。</summary>
            /// <returns>与此实例的值等效的 8 位有符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public sbyte ToSByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将该实例的值转换为等效的 8 位无符号整数。</summary>
            /// <returns>与该实例的值等效的 8 位无符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public byte ToByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 16 位有符号整数。</summary>
            /// <returns>与此实例的值等效的 16 位有符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public short ToInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将该实例的值转换为等效的 16 位无符号整数。</summary>
            /// <returns>与该实例的值等效的 16 位无符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public ushort ToUInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 32 位有符号整数。</summary>
            /// <returns>与此实例的值等效的 32 位有符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public int ToInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将该实例的值转换为等效的 32 位无符号整数。</summary>
            /// <returns>与该实例的值等效的 32 位无符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public uint ToUInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 64 位有符号整数。</summary>
            /// <returns>与此实例的值等效的 64 位有符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public long ToInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将该实例的值转换为等效的 64 位无符号整数。</summary>
            /// <returns>与该实例的值等效的 64 位无符号整数。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public ulong ToUInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的单精度浮点数字。</summary>
            /// <returns>与此实例的值等效的单精度浮点数字。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public float ToSingle(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的双精度浮点数字。</summary>
            /// <returns>与此实例的值等效的双精度浮点数字。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public double ToDouble(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="T:System.Decimal" /> 数字。</summary>
            /// <returns>与此实例的值等效的 <see cref="T:System.Decimal" /> 数字。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public decimal ToDecimal(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="T:System.DateTime" />。</summary>
            /// <returns>与此实例的值等效的 <see cref="T:System.DateTime" /> 实例。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public DateTime ToDateTime(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="T:System.String" />。</summary>
            /// <returns>与此实例的值等效的 <see cref="T:System.String" /> 实例。</returns>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public string ToString(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            /// <summary>使用指定的区域性特定格式设置信息将此实例的值转换为具有等效值的指定 <see cref="T:System.Type" /> 的 <see cref="T:System.Object" />。</summary>
            /// <returns>其值与此实例值等效的 <paramref name="conversionType" /> 类型的 <see cref="T:System.Object" /> 实例。</returns>
            /// <param name="conversionType">要将此实例的值转换为的 <see cref="T:System.Type" />。 </param>
            /// <param name="provider">
            /// <see cref="T:System.IFormatProvider" /> 接口实现，提供区域性特定的格式设置信息。 </param>
            public object ToType(Type conversionType, IFormatProvider provider) => null;
        }
    }
}
