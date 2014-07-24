using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using blqw;
using System.Diagnostics;
using fastJSON;

namespace Demo
{
    class TimeTest
    {
        public static int TestCount { get; set; }

        public static Object TestObject { get; set; }

        //测试QuickJsonBuilder性能
        public static void TestQuickJsonBuilder()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            new QuickJsonBuilder().ToJsonString(obj);
            CodeTimer.Time("QuickJsonBuilder", TestCount, () => {
                new QuickJsonBuilder().ToJsonString(obj);
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
            CodeTimer.Time("FastJson", TestCount, () => {
                fastJSON.JSON.Instance.ToJSON(obj, p);
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
            CodeTimer.Time("JayrockJson", TestCount, () => {
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
            CodeTimer.Time("NewtonsoftJson", TestCount, () => {
                Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            });
        }

        public static void TestJavaScriptSerializer()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
            CodeTimer.Time("JavaScriptSerializer", TestCount, () => {
                new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
            });
        }

        //测试JsonBuilder性能
        public static void TestJsonBuilder()
        {
            CodeTimer.Initialize();
            object obj = TestObject;
            new JsonBuilder().ToJsonString(obj);
            CodeTimer.Time("JsonBuilder", TestCount, () => {
                new JsonBuilder().ToJsonString(obj);
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
            TimeTest.TestJsonBuilder();
        }

    }
}
