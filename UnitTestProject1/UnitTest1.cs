using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Collections;
using blqw;
using blqw.Serializable;

namespace UnitTestProject1
{
    #region 测试用例
    public class User
    {
        public static User TestUser()
        {//这里我尽量构造一个看上去很复杂的对象,并且这个对象几乎涵盖了所有常用的类型
            User user = new User();
            user.UID = Guid.NewGuid();
            user.Birthday = new DateTime(1986, 10, 29, 18, 00, 00);
            user.IsDeleted = false;
            user.Name = "blqw";
            user.Sex = UserSex.Male;
            user.LoginHistory = new List<DateTime>();
            user.LoginHistory.Add(DateTime.Today.Add(new TimeSpan(8, 00, 00)));
            user.LoginHistory.Add(DateTime.Today.Add(new TimeSpan(10, 10, 10)));
            user.LoginHistory.Add(DateTime.Today.Add(new TimeSpan(12, 33, 56)));
            user.LoginHistory.Add(DateTime.Today.Add(new TimeSpan(17, 25, 18)));
            user.LoginHistory.Add(DateTime.Today.Add(new TimeSpan(23, 06, 59)));
            user.Info = new UserInfo();
            user.Info.Address = "广东省\n广州市";
            user.Info.ZipCode = 510000;
            user.Info.Phone = new Dictionary<string, string>();
            user.Info.Phone.Add("手机", "18688888888");
            user.Info.Phone.Add("电话", "82580000");
            user.Info.Phone.Add("短号", "10086");
            user.Info.Phone.Add("QQ", "21979018");
            user.Double = Double.NegativeInfinity;
            // user.Self = user; //这里是用来测试循环引用的解析情况
            return user;
        }

        public User Self { get; set; }
        //User self
        /// <summary> 唯一ID
        /// </summary>
        public Guid UID { get; set; }
        /// <summary> 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary> 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary> 性别
        /// </summary>
        public UserSex Sex { get; set; }
        /// <summary> 是否删除标记
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary> 最近登录记录
        /// </summary>
        public List<DateTime> LoginHistory { get; set; }
        /// <summary> 联系信息
        /// </summary>
        public UserInfo Info { get; set; }
        public Double Double { get; set; }
    }
    /// <summary> 用户性别
    /// </summary>
    public enum UserSex
    {
        /// <summary> 男
        /// </summary>
        Male,
        /// <summary> 女
        /// </summary>
        Female
    }
    /// <summary> 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary> 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary> 联系方式
        /// </summary>
        public Dictionary<string, string> Phone { get; set; }
        /// <summary> 邮政编码
        /// </summary>
        public int ZipCode { get; set; }
    }
    #endregion

    public class Object1
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public Object1Item[] data { get; set; }
        public int total { get; set; }
    }

    public class Object1Item
    {
        public long vptNum { get; set; }
        public int state { get; set; }
        public DateTime tdate { get; set; }
        public string ntype { get; set; }
        public int abnormal { get; set; }
    }
    public class Object2
    {
        public uint ORGCODE { get; set; }
        public string NAME { get; set; }
        public uint VALUE { get; set; }
        public string VTYPE { get; set; }
    }


    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var user = User.TestUser();
            var userJson = Json.ToJsonString(user);
            TestNewtonsoftResult<Object1>(File.ReadAllText("json1.txt"));
            TestNewtonsoftResult<Object2[]>(File.ReadAllText("json2.txt"));
            TestNewtonsoftResult<List<Object2>>(File.ReadAllText("json2.txt"));
            TestNewtonsoftResult<User>(userJson);
            TestSafeResult<Object1>(File.ReadAllText("json1.txt"));
            TestSafeResult<Object2[]>(File.ReadAllText("json2.txt"));
            TestSafeResult<List<Object2>>(File.ReadAllText("json2.txt"));
            TestSafeResult<User>(userJson);

            TestSafeResult(File.ReadAllText("json1.txt"));
            TestSafeResult(File.ReadAllText("json2.txt"));
            TestSafeResult(File.ReadAllText("json2.txt"));
            TestSafeResult(userJson);

            //时间处理
            DateTime date = new DateTime(2014, 1, 2, 3, 4, 5, 6);
            Assert.AreEqual("{\"date\":\"2014-01-02 03:04:05\"}", Json.ToJsonString(new { date = date }));

            Assert.AreEqual(
                "{\"date\":\"2014-01-02\"}",
                Json.ToJsonString(new { date = date }, JsonBuilderSettings.FormatDate
                ));

            Assert.AreEqual("{\"date\":\"03:04:05\"}", Json.ToJsonString(new { date = date }, blqw.JsonBuilderSettings.FormatTime));
            Assert.AreEqual("{\"date\":\"2014-01-02\"}", Json.ToJsonString(new { date = date.Date }));
            Assert.AreEqual("{\"date\":\"\"}", Json.ToJsonString(new { date = date.Date }, blqw.JsonBuilderSettings.FormatTime | blqw.JsonBuilderSettings.IgnoreEmptyTime));

            //枚举
            Assert.AreEqual("{\"key\":\"Applications\"}", Json.ToJsonString(new { key = ConsoleKey.Applications }, 0));
            Assert.AreEqual("{\"key\":93}", Json.ToJsonString(new { key = ConsoleKey.Applications }, JsonBuilderSettings.EnumToNumber));

            //数字
            Assert.AreEqual("{\"number\":1}", Json.ToJsonString(new { number = 1 }));
            Assert.AreEqual("{\"number\":\"1\"}", Json.ToJsonString(new { number = 1 }, JsonBuilderSettings.QuotWrapNumber));

            //布尔
            Assert.AreEqual("{\"bool\":true}", Json.ToJsonString(new { @bool = true }));
            Assert.AreEqual("{\"bool\":false}", Json.ToJsonString(new { @bool = false }));
            Assert.AreEqual("{\"bool\":\"true\"}", Json.ToJsonString(new { @bool = true }, JsonBuilderSettings.QuotWrapBoolean));
            Assert.AreEqual("{\"bool\":\"false\"}", Json.ToJsonString(new { @bool = false }, JsonBuilderSettings.QuotWrapBoolean));
            Assert.AreEqual("{\"bool\":1}", Json.ToJsonString(new { @bool = true }, JsonBuilderSettings.BooleanToNumber));
            Assert.AreEqual("{\"bool\":0}", Json.ToJsonString(new { @bool = false }, JsonBuilderSettings.BooleanToNumber));

            //循环引用
            try
            {
                user.Self = user;
                blqw.Json.ToJsonString(user);
                Assert.Fail("循环引用测试失败1");
            }
            catch (Exception)
            {
            }
            try
            {
                user.Self = user;
                Json.ToJsonString(user, JsonBuilderSettings.CheckLoopRef);
            }
            catch (Exception)
            {
                Assert.Fail("循环引用测试失败2");
            }


            //忽略null属性
            Assert.AreEqual("{\"a\":null,\"b\":1}", Json.ToJsonString(new { a = (string)null, b = 1 }, JsonBuilderSettings.None));
            Assert.AreEqual("{\"b\":1}", Json.ToJsonString(new { a = (string)null, b = 1 }, JsonBuilderSettings.IgnoreNullMember));

            //特性测试
            var test2 = new AttrTest { ID = 1, Name = "a", Time = new DateTime(2014, 1, 2, 3, 4, 5, 6) };
            Assert.AreEqual("{\"name\":\"a\"}", Json.ToJsonString(test2, JsonBuilderSettings.None));
            Assert.AreEqual("{\"name\":\"a\",\"Time\":\"2014年1月2日\"}", Json.ToJsonString(test2, JsonBuilderSettings.SerializableField));
        }

        class AttrTest
        {
            [JsonIgnore]
            public int ID { get; set; }
            [JsonName("name")]
            public string Name { get; set; }
            [JsonFormat("yyyy年M月d日")]
            public DateTime Time;
        }

        public void TestNewtonsoftResult<T>(string jsonString)
        {
            var obj1 = blqw.Json.ToObject<T>(jsonString);
            var obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
            AssertEquals(obj1, obj2);
        }

        public void TestSafeResult<T>(string jsonString)
        {
            var obj1 = blqw.Json.ToObject<T>(jsonString);
            var jsonString1 = blqw.Json.ToJsonString(obj1);
            var obj2 = blqw.Json.ToObject<T>(jsonString1);
            var jsonString2 = blqw.Json.ToJsonString(obj2);
            AssertEquals(obj1, obj2);
            Assert.AreEqual(jsonString1, jsonString2);
        }


        public void TestSafeResult(string jsonString)
        {
            var obj1 = blqw.Json.ToObject(jsonString);
            var jsonString1 = blqw.Json.ToJsonString(obj1, JsonBuilderSettings.Default ^ JsonBuilderSettings.IgnoreNullMember);
            var obj2 = blqw.Json.ToObject(jsonString1);
            var jsonString2 = blqw.Json.ToJsonString(obj2, JsonBuilderSettings.Default ^ JsonBuilderSettings.IgnoreNullMember);
            AssertEquals(obj1, obj2);
            Assert.AreEqual(jsonString1, jsonString2);
        }

        #region MyRegion
        /// <summary> 检查一个类型是否为可空值类型
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return (t.IsValueType && t.IsGenericType && !t.IsGenericTypeDefinition && object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(Nullable<>)));
        }

        public static bool IsPrimitive(Type type)
        {
            if (Type.GetTypeCode(type) == TypeCode.Object)
            {
                if (type == typeof(Guid))
                {
                    return true;
                }
                else if (IsNullable(type))
                {
                    return IsPrimitive(type.GetGenericArguments()[0]);
                }
            }
            else
            {
                if (type.IsPrimitive)
                {
                    return true;
                }
                else if (type == typeof(DateTime))
                {
                    return true;
                }
                else if (type == typeof(string))
                {
                    return true;
                }
            }
            return false;
        }

        public static void AssertEquals<T>(T t1, T t2, string title = null)
        {
            //if (t1 == null || (t1.GetType().IsValueType && t1.GetHashCode() == 0))
            //{
            //    return;
            //}
            if (t1 == null || t2 == null)
            {
                if (t1 != null || t2 != null)
                {
                    throw new Exception(string.Format("{0} 值不同 ,值1 {1}, 值2 {2}", title, (object)t1 ?? "NULL", (object)t2 ?? "NULL"));
                }
                return;
            }

            if (IsPrimitive(t1.GetType()))
            {
                if (!object.Equals(t1, t2))
                {
                    throw new Exception(string.Format("{0} 值不同 ,值1 {1}, 值2 {2}", title, (object)t1 ?? "NULL", (object)t2 ?? "NULL"));
                }
            }
            else if (t1 is IEnumerable)
            {
                var e1 = ((IEnumerable)t1).GetEnumerator();
                var e2 = ((IEnumerable)t2).GetEnumerator();

                while (e1.MoveNext())
                {
                    if (e2.MoveNext() == false)
                    {
                        throw new Exception(string.Format("{0} 个数不同1", title));
                    }
                    AssertEquals(e1.Current, e2.Current, title);
                }
                if (e2.MoveNext())
                {
                    throw new Exception(string.Format("{0} 个数不同2", title));
                }
            }
            else
            {
                foreach (var p in t1.GetType().GetProperties())
                {
                    if (p.CanRead)
                    {
                        var val1 = p.GetValue(t1);
                        var val2 = p.GetValue(t2);
                        AssertEquals(val1, val2, "属性" + p.Name);
                    }
                }
            }
        }
        #endregion
    }


}
