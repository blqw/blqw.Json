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
            var jsonStr = File.ReadAllText("json1.txt");

            var obj1 = blqw.Json.ToObject<ResultDTO>(jsonStr);
            var obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultDTO>(jsonStr);
            var jsonStr1 = blqw.Json.ToJsonString(obj1);

            Class1.AssertEquals(obj1, obj2);

        }



        public class ResultDTO
        {
            public bool Result { get; set; }
            public string Message { get; set; }
            public DTOItem[] data { get; set; }
            public int total { get; set; }
        }

        public class DTOItem
        {
            public long vptNum { get; set; }
            public int state { get; set; }
            public DateTime tdate { get; set; }
            public string ntype { get; set; }
            public int abnormal { get; set; }
        }


    }
}
