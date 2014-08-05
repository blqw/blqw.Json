using blqw;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Demo
{
    class Program
    {

        static void Main(string[] args)
        {
            var obj1 = User.TestUser();
            var obj2 = blqw.Json.ToObject<ResultDTO>(File.ReadAllText("json1.txt"));
            var obj3 = blqw.Json.ToObject<List<Object2>>(File.ReadAllText("json2.txt"));

            TimeTest.TestCount = 100000 * 5;
            TimeTest.TestObject = obj1;
            TimeTest.TestQuickJsonBuilder();
            //TimeTest.TestFastJson();

            Console.WriteLine("========================");
            TimeTest.TestCount = 2000 * 5;
            TimeTest.TestObject = obj2;
            TimeTest.TestQuickJsonBuilder();
            //TimeTest.TestFastJson();

            Console.WriteLine("========================"); ;
            TimeTest.TestCount = 250 * 5;
            TimeTest.TestObject = obj3;
            TimeTest.TestQuickJsonBuilder();
            //TimeTest.TestFastJson();

            //var obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultDTO>(jsonStr);
            //var jsonStr1 = blqw.Json.ToJsonString(obj1);

            //Class1.AssertEquals(obj1, obj2);

        }



        public class ResultDTO
        {
            public bool Result { get; set; }
            public string Message { get; set; }
            public DTOItem[] data { get; set; }
            public int total { get; set; }

            public class DTOItem
            {
                public long vptNum { get; set; }
                public int state { get; set; }
                public DateTime tdate { get; set; }
                public string ntype { get; set; }
                public int abnormal { get; set; }
            }
        }


        public class Object2
        {
            public string ORGCODE { get; set; }
        }

    }
}
