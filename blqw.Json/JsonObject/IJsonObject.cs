using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public interface IJsonObject
    {
        IJsonObject this[string key] { get; }
        IJsonObject this[int index] { get; }
        ICollection<string> Keys { get; }
        JsonTypeCode TypeCode { get; }
        bool IsUndefined { get; }
    }
}
