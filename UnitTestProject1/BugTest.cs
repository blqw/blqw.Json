using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;
using blqw;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class BugTest
    {
        #region model
        public class BaiduTQ
        {
            public int error { get; set; }
            public string status { get; set; }
            public string date { get; set; }
            public List<BaiduResult> results { get; set; }
        }

        public class BaiduResult
        {
            public string currentCity { get; set; }
            public string pm25 { get; set; }
            public List<BaiduIndex> index { get; set; }
            public List<BaiDuWeaterData> weather_data { get; set; }
        }

        public class BaiduIndex
        {
            public string title { get; set; }
            public string zs { get; set; }
            public string tipt { get; set; }
            public string des { get; set; }
        }

        public class BaiDuWeaterData
        {
            public string date { get; set; }
            public string dayPictureUrl { get; set; }
            public string nightPictureUrl { get; set; }
            public string weather { get; set; }
            public string wind { get; set; }
            public string temperature { get; set; }
        } 
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                var url = "http://api.map.baidu.com/telematics/v3/weather?location=北京&output=json&ak=hXWAgbsCC9UTkBO5V5Qg1WZ9";
                var json = client.DownloadString(url);
                var tq = Json.ToObject<BaiduTQ>(json);
                Console.WriteLine("{0} {1}", tq.results[0].currentCity, tq.results[0].weather_data[0].date);
            }
        }
    }
}
