using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw;
using System.IO;
using blqw.Serializable;

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
            Assert.AreEqual(obj.c.ToString(), obj2.c.ToString());
            Assert.AreEqual(obj.s.ToString(), obj2.s.ToString());

        }


        [TestMethod]
        public void 测试Double的bug()
        {
            var str = "{\"a\":2.2}";
            var obj = Json.ToDynamic(str);
            Assert.AreEqual(2.2d, obj.a);

            str = "{\"a\":123e+123}";
            obj = Json.ToDynamic(str);
            Assert.AreEqual(123e+123d, obj.a);
            str = "{\"a\":234e-315}";
            obj = Json.ToDynamic(str);
            Assert.AreEqual(234e-315d, obj.a);
        }

        [TestMethod]
        public void 测试FillArray的Bug()
        {
            var json = "[0,1,2,3,4,5]";
            var arr = new int[6];
            new JsonParser().FillObject(arr, json);
            for (int i = 0; i < arr.Length; i++)
            {
                Assert.AreEqual(arr[i], i);
            }

            arr = new int[9];
            new JsonParser().FillObject(arr, json);
            for (int i = 0; i < arr.Length; i++)
            {
                Assert.AreEqual(arr[i], i > 5 ? 0 : i);
            }

            arr = new int[3];
            new JsonParser().FillObject(arr, json);
            for (int i = 0; i < arr.Length; i++)
            {
                Assert.AreEqual(arr[i], i);
            }

        }

    }
}
