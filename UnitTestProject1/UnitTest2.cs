using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using blqw;
using System.Collections.Specialized;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        class MyList : List<string>,  IFormatProvider
        {
            public int TotalCount { get; set; }

            public object GetFormat(Type formatType)
            {
                if (formatType != null && formatType.Name == "Json")
                    return new { Items = GetEnumerator(), TotalCount };
                return null;
            }
        }

        class User
        {
            [JsonName("UserID")]
            public int ID { get; set; }

            [JsonFormat("d2")]
            public int Money { get; set; }

            [JsonIgnore]
            public bool Sex { get; set; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var list = new MyList();
            list.Add("a");
            list.TotalCount = 100;
            var json = list.ToJsonString();
            Assert.AreEqual("{\"Items\":[\"a\"],\"TotalCount\":100}", json);
        }
        

        [TestMethod]
        public void 动态类型()
        {
            var str = "{ Name : \"blqw\", Age : 11 ,Array : [\"2014-1-1 1:00:00\",false,{ a:1,b:2 }] }";
            dynamic obj = Json.ToDynamic(str);


            Assert.AreEqual("blqw", (string)obj.Name);
            Assert.AreEqual(11, (int)obj.Age);
            Assert.AreEqual(3, (int)obj.Array.Count);
            Assert.AreEqual(1, (int)obj.Array[2].a);
            Assert.AreEqual(2, (int)obj.Array[2].b);
            //Assert.AreEqual(new DateTime(2014,1,1,1,0,0), (DateTime)obj.Array[0]);
            //Assert.AreEqual(false, (bool)obj.Array[1]);


        }


        [TestMethod]
        public void 支持NameValueCollection()
        {
            var nv = new NameValueCollection();
            nv["Name"] = "blqw";
            nv["Age"] = "11";

            var str = Json.ToJsonString(nv);

            Assert.AreEqual("{\"Name\":\"blqw\",\"Age\":\"11\"}", str);

            var nv1 = Json.ToObject<NameValueCollection>(str);
            Assert.AreEqual(2, nv1.Count);
            Assert.AreEqual("blqw", nv1["Name"]);
            Assert.AreEqual("11", nv1["Age"]);

        }
    }
}
