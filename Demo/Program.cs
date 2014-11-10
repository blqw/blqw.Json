using blqw;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Demo
{
    class Program
    {

        static void Main(string[] args)
        {

            Test1(true);
            Test2();
//var str = "{ Name : \"blqw\", Age : 11 ,Array : [\"2014-1-1 1:00:00\",false,{ a:1,b:2 }] }";
//dynamic json = Json.ToDynamic(str);
//Console.WriteLine(json.Name);
//Console.WriteLine(json.Age);
//Console.WriteLine(((DateTime)json.Array[0]).ToShortDateString());
//Console.WriteLine(((bool)json.Array[1]) == false);
//Console.WriteLine(json.Array[2].a);
//Console.WriteLine(json.Array[2].b);

        }

        static void Test1(bool fastjson)
        {
            var obj1 = User.TestUser();
            var obj2 = blqw.Json.ToObject<ResultDTO>(File.ReadAllText("json1.txt"));
            var obj3 = blqw.Json.ToObject<List<Object2>>(File.ReadAllText("json2.txt"));

            TimeTest.TestCount = 100000 * 5;
            TimeTest.TestObject = obj1;
            TimeTest.TestQuickJsonBuilder();
            if (fastjson) TimeTest.TestFastJson();

            Console.WriteLine("========================");
            TimeTest.TestCount = 2000 * 5;
            TimeTest.TestObject = obj2;
            TimeTest.TestQuickJsonBuilder();
            if (fastjson) TimeTest.TestFastJson();

            Console.WriteLine("========================"); ;
            TimeTest.TestCount = 250 * 5;
            TimeTest.TestObject = obj3;
            TimeTest.TestQuickJsonBuilder();
            if (fastjson) TimeTest.TestFastJson();

            //var obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultDTO>(jsonStr);
            //var jsonStr1 = blqw.Json.ToJsonString(obj1);

            //Class1.AssertEquals(obj1, obj2);
        }

        static void Test2()
        {
            //var json1 = blqw.Json.ToJsonString(User.TestUser());
            var json2 = File.ReadAllText("json1.txt");
            var json3 = File.ReadAllText("json2.txt");

            //TimeTest.TestCount = 1000;
            //TimeTest.TestJsonString = json1;
            //TimeTest.TestQuickJsonBuilder<User>();
            //TimeTest.TestFastJson<User>();

            //Console.WriteLine("========================");
            //TimeTest.TestCount = 200;
            //TimeTest.TestJsonString = json2;
            //TimeTest.TestQuickJsonBuilder<ResultDTO>();
            //TimeTest.TestFastJson<ResultDTO>();

            Console.WriteLine("========================"); ;
            TimeTest.TestCount = 25;
            TimeTest.TestJsonString = json3;
            TimeTest.TestQuickJsonBuilder<List<Object2>>();
            TimeTest.TestFastJson<List<Object2>>();

            //var obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultDTO>(jsonStr);
            //var jsonStr1 = blqw.Json.ToJsonString(obj1);

            //Class1.AssertEquals(obj1, obj2);
        }


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
