using blqw;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Demo
{
    class Program
    {
        class MyClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }


        public class JsonObject : NameValueCollection
        {
            object _Data;
            public JsonObject(string json)
            {
                _Data = Json.ToObject(json);
            }

            private object Get(object obj, string name, int? index)
            {
                var map = obj as IDictionary;
                if (map != null)
                {
                    if (name != null)
                    {
                        return Get(map, name);
                    }
                    return Get(map, index.Value);
                }
                var list = obj as IList;
                if (list != null)
                {
                    if (name != null)
                    {
                        return Get(list, name);
                    }
                    return Get(list, index.Value);
                }
                return null;
            }



            private object Get(IList list, int index)
            {
                if (index < list.Count)
                {
                    return list[index];
                }
                return null;
            }

            private object Get(IList list, string name)
            {
                int index;
                if (int.TryParse(name, out index))
                {
                    return Get(list, index);
                }
                return null;
            }

            private object Get(IDictionary map, int index)
            {
                return map[index.ToString()];
            }

            private object Get(IDictionary map, string name)
            {
                return map[name];
            }

            private string ToString(object obj)
            {
                if (obj is IDictionary || obj is IList)
                {
                    return Json.ToJsonString(obj);
                }
                return obj?.ToString();
            }

            private IEnumerable<string> ParseNames(string name)
            {
                if (name == null)
                {
                    yield return null;
                    yield break;
                }
                var chars = name.ToCharArray();
                var start = 0;
                for (int i = 0, length = chars.Length; i < length; i++)
                {
                    var c = chars[i];
                    switch (c)
                    {
                        case '[':
                            yield return new string(chars, start, i - start);
                            i++;
                            start = i;
                            while (i < length)
                            {
                                if (chars[i] == ']')
                                {
                                    break;
                                }
                                i++;
                            }
                            if (i == length)
                            {
                                yield return null;
                                yield break;
                            }
                            int index;
                            var str = new string(chars, start, i - start);
                            if (int.TryParse(str, out index))
                            {
                                yield return str;
                            }
                            else
                            {
                                yield return null;
                                yield break;
                            }
                            start = i + 1;
                            break;
                        case '.':
                            yield return new string(chars, start, i - start);
                            start = i + 1;
                            break;
                        default:
                            break;
                    }
                }
                if (start != 0 && start != chars.Length)
                {
                    yield return new string(chars, start, chars.Length - start);
                }
            }

            public override string Get(string name)
            {
                var obj = _Data;
                foreach (var n in ParseNames(name))
                {
                    if (n == null)
                    {
                        return ToString(Get(_Data, name, null));
                    }
                    obj = Get(obj, n, null);
                    if (obj == null)
                    {
                        return ToString(Get(_Data, name, null));
                    }
                }
                if (ReferenceEquals(obj, _Data))
                {
                    return ToString(Get(_Data, name, null));
                }
                return ToString(obj);
            }
        }


        static void Main(string[] args)
        {
            var body = "{a:{b:1},c:'xxx',d:{e:[1]}}";
            var form = new JsonObject(body);

            Console.WriteLine(form["a"] == "{\"b\":1}");
            Console.WriteLine(form["b"] == null);
            Console.WriteLine(form["c"] == "xxx");
            Console.WriteLine(form["a.b"] == "1");
            Console.WriteLine(form["d.e[0]"] == "1");
            Console.WriteLine(form["d.e[2]"] == null);








            //Console.WriteLine(Json.ToJsonString(new object[]{null}));
            Test1(true);
            Test2();
            //
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
