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

        [TestMethod]
        public void 测试Unicode编码()
        {
            var obj = new { c = '我', s = "a哈ffff哈看1" };
            var json = Json.ToJsonString(obj, JsonBuilderSettings.CastUnicode);
            Assert.AreEqual(@"{""c"":""\u6211"",""s"":""a\u54c8ffff\u54c8\u770b1""}", json);
            var obj2 = Json.ToDynamic(json);
            Assert.AreEqual(obj.c.ToString() , obj2.c.ToString());
            Assert.AreEqual(obj.s.ToString(), obj2.s.ToString());

        }
    }
}
