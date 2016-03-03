using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    class JsonList : List<object>
    {
        public override string ToString()
        {
            return Json.ToJsonString(this);
        }
    }
}
