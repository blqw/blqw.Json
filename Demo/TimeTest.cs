using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using blqw;
using System.Diagnostics;
using fastJSON;
using blqw.Serializable;

namespace Demo
{
    class TimeTest
    {
        public static int TestCount { get; set; }

        public static Object TestObject { get; set; }
        public static String TestJsonString { get; set; }

        private static string N(string title)
        {
            return string.Join(" ", title, TestObject.GetType().ToString());
        }

        //测试QuickJsonBuilder性能
        public static void TestQuickJsonBuilder()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            obj.ToJsonString();
            CodeTimer.Time(N("QuickJsonBuilder序列化"), TestCount, () => {
                obj.ToJsonString();
            });

        }
        //测试FastJson性能
        public static void TestFastJson()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            JSONParameters p = new JSONParameters();
            p.EnableAnonymousTypes
                = p.IgnoreCaseOnDeserialize
                = p.ShowReadOnlyProperties
                = p.UseEscapedUnicode
                = p.UseExtensions
                = p.UseFastGuid
                = p.UseOptimizedDatasetSchema
                = p.UseUTCDateTime
                = p.UsingGlobalTypes
                = false;
            fastJSON.JSON.Instance.ToJSON(obj, p);
            CodeTimer.Time(N("FastJson序列化"), TestCount, () => {
                fastJSON.JSON.Instance.ToJSON(obj, p);
            });
        }

        //测试Crylw.Json性能
        public static void TestCrylwJson()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            Crylw.Json.Json.ToString(obj);
            CodeTimer.Time(N("Crylw.Json序列化"), TestCount, () => {
                Crylw.Json.Json.ToString(obj);
            });
        }
        //测试JayrockJson性能
        public static void TestJayrockJson()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            {
                var writer = new Jayrock.Json.JsonTextWriter();
                Jayrock.Json.Conversion.JsonConvert.Export(obj, writer);
                writer.ToString();
            }
            CodeTimer.Time(N("JayrockJson序列化"), TestCount, () => {
                var writer = new Jayrock.Json.JsonTextWriter();
                Jayrock.Json.Conversion.JsonConvert.Export(obj, writer);
                writer.ToString();
            });
        }

        public static void TestNewtonsoftJson()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            CodeTimer.Time(N("NewtonsoftJson序列化"), TestCount, () => {
                Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            });
        }

        public static void TestJavaScriptSerializer()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
            CodeTimer.Time(N("JavaScriptSerializer序列化"), TestCount, () => {
                new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
            });
        }


        //测试QuickJsonBuilder性能
        public static void TestQuickJsonBuilder<T>()
        {
            TestObject = Activator.CreateInstance<T>();
            CodeTimer.Initialize();
            var jsonString = TestJsonString;
            var obj = new JsonParser().ToObject(typeof(T), jsonString);
            CodeTimer.Time(N("QuickJsonBuilder反序列化"), TestCount, () => {
                new JsonParser().ToObject(typeof(T), jsonString);
            });

        }
        //测试FastJson性能
        public static void TestFastJson<T>()
        {
            TestObject = Activator.CreateInstance<T>();
            CodeTimer.Initialize();
            CodeTimer.Initialize();
            var jsonString = TestJsonString;
            fastJSON.JSON.Instance.ToObject<T>(jsonString);
            CodeTimer.Time(N("FastJson反序列化"), TestCount, () => {
                fastJSON.JSON.Instance.ToObject<T>(jsonString);
            });
        }

        public static void SerializeTest(int count, object obj)
        {
            TimeTest.TestCount = count;
            TimeTest.TestObject = obj;

            TimeTest.TestQuickJsonBuilder();
            TimeTest.TestFastJson();
            TimeTest.TestJavaScriptSerializer();
            TimeTest.TestJayrockJson();
            TimeTest.TestNewtonsoftJson();
        }

    }
}
