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

        class MyClass
        {
            public string name { get; set; }
            public int? id { get; set; }
        }
    }
}
