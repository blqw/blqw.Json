using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest4
    {
        [TestMethod]
        public void 测试可空值类型()
        {
            var str = "{\"name\":\"zzj\",\"id\":null}";
            var mc = Json.ToObject<MyClass>(str);
            Assert.IsNotNull(mc);
            Assert.IsNull(mc.id);
            Assert.AreEqual("zzj", mc.name);
        }

        [TestMethod]
        public void 匿名对象的反序列化()
        {
            var a = new { id = 1, name = "blqw" };
            var json = Json.ToJsonString(a);
            dynamic b = Json.ToObject(a.GetType(), json);
            Assert.IsNotNull(b);
            Assert.IsInstanceOfType(b, a.GetType());
            Assert.AreEqual(a.id, b.id);
            Assert.AreEqual(a.name, b.name);

        }

        class MyClass
        {
            public string name { get; set; }
            public int? id { get; set; }
        }
    }
}
