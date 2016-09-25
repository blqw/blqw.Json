using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using blqw;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void 自定义序列化测试()
        {
            var m = new MyClass() { ID = 123, Name = "zzj" };
            var str = Json.ToJsonString(m);
            //"x":{"ID":0},"y":100
            Assert.AreEqual("{\"x\":{\"ID\":123,\"Name\":\"zzj\"},\"y\":100}", str);
        }

        [TestMethod]
        public void 自定义序列化测试2()
        {
            var m = new MyClass2(1, "zzj");
            var str = m.ToJsonString();
            //"x":{"ID":0},"y":100
            Assert.AreEqual("[1,\"zzj\"]", str);
        }


        class MyClass : IFormatProvider
        {
            public int ID { get; set; }
            public string Name { get; set; }

            #region IFormatProvider 成员

            public object GetFormat(Type formatType)
            {
                return new { x = new { ID, Name }, y = 100 };
            }

            #endregion
        }

        class MyClass2 : System.Collections.IEnumerable
        {
            public MyClass2(params object[] args)
            {
                arr = args;
            }

            object[] arr;

            public System.Collections.IEnumerator GetEnumerator()
            {
                return arr.GetEnumerator();
            }

        }
    }

}
