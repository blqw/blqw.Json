using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
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
}
