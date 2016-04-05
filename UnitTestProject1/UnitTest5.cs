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
            var json = File.ReadAllLines("json3.txt");
            var obj = Json.ToDynamic(json[0]);
            Assert.AreEqual("/\b\f\0\a\v/", obj.a);
            var json2 = Json.ToJsonString(obj, 0);
            Assert.AreEqual(json[1], json2);
        }
    }
}
