using System.Collections.Generic;

namespace blqw.JsonServices
{
    internal class JsonList : List<object>
    {
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}