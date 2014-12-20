using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        class MyList : List<string>, blqw.IToJson, blqw.ILoadJson
        {
            public int TotalCount { get; set; }

            public object ToJson()
            {
                return new { Items = GetEnumerator(), TotalCount };
            }

            public void LoadJson(blqw.IJsonObject jsonObject)
            {
                var items = jsonObject["Items"];
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        Add(item.ToString());
                    }
                }
                var total = jsonObject["TotalCount"] as IConvertible;
                if (total != null)
                {
                    TotalCount = total.ToInt32(null);
                }
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var list = new MyList();
            list.Add("a");
            list.TotalCount = 100;
            var json = blqw.Json.ToJsonString(list);
            Assert.AreEqual("{\"Items\":[\"a\"],\"TotalCount\":100}", json);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var json = "{\"Items\":[\"a\"],\"TotalCount\":100}";
            var list = blqw.Json.ToObject<MyList>(json);
            Assert.AreEqual(100, list.TotalCount);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("a", list[0]);

        }
    }
}
