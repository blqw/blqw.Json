using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest5
    {
        [TestMethod]
        public void 测试反序列化特殊字符()
        {
            var json = File.ReadAllText("json3.txt");
            var obj = Json.ToDynamic(json);
            Assert.AreEqual("\b\f\0", obj.a);
            var json2 = Json.ToJsonString(obj);

            Assert.AreEqual(json, json2);
        }
    }
}
