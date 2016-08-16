using blqw;
using blqw.Serializable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using ServiceStack.Text;

namespace Demo
{
    public class Program
    {
        class TestClass1 : IObjectReference
        {
            public int ID { get; set; }
            public string Name { get; set; }
            
            public object GetRealObject(StreamingContext context)
            {
                if (context.Context is JsonWriterArgs)
                {
                    return new {id = this.ID, name = this.Name};
                }
                return this;
            }
        }

        class TestClass2
        {
            public Regex Regex { get; set; }
        }

        class TestClass2Writer : IJsonWriter
        {
            public Type Type => typeof(TestClass2);
            public void Write(object obj, JsonWriterArgs args)
            {
                var o = (TestClass2)obj;
                args.Writer.Write($"\"Regex\":/{o.Regex.ToString()}/g");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Guid.NewGuid().ToJsonString());
            var t1 = new TestClass1()
            {
                ID = 1,
                Name = "blqw",
            };
            var t2 = new TestClass2()
            {
               Regex = new Regex("^[0-9]{4,6}$") 
            };

            var str1 = t1.ToJsonString(); //{ "id":1,"name":"blqw"}
            Console.WriteLine(str1);
            var str2 = t2.ToJsonString();  //{"Regex":/^[0-9]{4,6}$/g}
            Console.WriteLine(str2);


            //var obj1 = User.TestUser();
            //TimeTest.TestCount = 10000;
            //TimeTest.TestObject = obj1;
            //TimeTest.TestQuickJsonBuilder();

            //Test1(true);
            //Test2();
            //
            //dynamic json = Json.ToDynamic(str);
            //Console.WriteLine(json.Name);
            //Console.WriteLine(json.Age);
            //Console.WriteLine(((DateTime)json.Array[0]).ToShortDateString());
            //Console.WriteLine(((bool)json.Array[1]) == false);
            //Console.WriteLine(json.Array[2].a);
            //Console.WriteLine(json.Array[2].b);

        }

        public static void Test1(bool fastjson)
        {
            var obj1 = User.TestUser();
            var obj2 = Json.ToObject<ResultDTO>(File.ReadAllText("json1.txt"));
            var obj3 = Json.ToObject<List<Object2>>(File.ReadAllText("json2.txt"));

            TimeTest.TestCount = 100000 * 5;
            TimeTest.TestObject = obj1;
            TimeTest.TestQuickJsonBuilder();
            TimeTest.TestCrylwJson();
            if (fastjson) TimeTest.TestFastJson();

            Console.WriteLine("========================");
            TimeTest.TestCount = 2000 * 5;
            TimeTest.TestObject = obj2;
            TimeTest.TestQuickJsonBuilder();
            TimeTest.TestCrylwJson();
            if (fastjson) TimeTest.TestFastJson();

            Console.WriteLine("========================"); ;
            TimeTest.TestCount = 250 * 5;
            TimeTest.TestObject = obj3;
            TimeTest.TestQuickJsonBuilder();
            TimeTest.TestCrylwJson();
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
            TimeTest.TestCount = 500;
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
