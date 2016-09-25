using System.Collections.Generic;

namespace blqw.Serializable
{
    internal class JsonList : List<object>
    {
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}