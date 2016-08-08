using System.Collections.Generic;
using System.IO;
using blqw;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest5
    {
        [TestMethod]
        public void 测试反序列化特殊字符()
        {
            // {"a":"/\b\f\u0000\a\v\/"}
            // {"a":"\/\b\f\u0000\u0007\u000b\/"}
            var json = File.ReadAllLines("json3.txt");
            var obj = Json.ToDynamic(json[0]);
            Assert.AreEqual("/\b\f\0\a\v/", (string)obj.a);
            var json2 = Json.ToJsonString(obj,0);
            Assert.AreEqual(json[1], json2);
        }

        [TestMethod]
        public void 测试Unicode编码()
        {
            var str = "a哈ffff哈看1";
            var json = str.ToJsonString(JsonBuilderSettings.CastUnicode);
            Assert.AreEqual(@"""a\u54c8ffff\u54c8\u770b1""", json);

            var obj = new {c = '我', s = "a哈ffff哈看1"};
            json = obj.ToJsonString(JsonBuilderSettings.CastUnicode);
            Assert.AreEqual(@"{""c"":""\u6211"",""s"":""a\u54c8ffff\u54c8\u770b1""}", json);
            var obj2 = Json.ToDynamic(json);
            Assert.AreEqual(obj.c.ToString(), obj2.c.ToString());
            Assert.AreEqual(obj.s, obj2.s.ToString());
        }

        [TestMethod]
        public void 支持键值枚举()
        {
            //var list = new List<KeyValuePair<string, object>>
            //{
            //    new KeyValuePair<string, object>("name", "blqw"),
            //    new KeyValuePair<string, object>("age", 30),
            //    new KeyValuePair<string, object>("sex", true)
            //};
            //var json = list.ToJsonString();
            //Assert.AreEqual(@"{""name"":""blqw"",""age"":30,""sex"":true}", json);
        }

        [TestMethod]
        public void 反斜杠序列化失败的问题()
        {
            var str = "\\abc";
            var json = str.ToJsonString(0);
            Assert.AreEqual("\"\\\\abc\"", json);
            var obj = new {s = "\\abc"};
             json = obj.ToJsonString(0);
            Assert.AreEqual("{\"s\":\"\\\\abc\"}", json);
        }
    }
}