using System;

namespace blqw.Serializable
{
    public interface IGenericJsonWriter : IJsonWriter
    {
        IJsonWriter MakeType(Type type);
    }
}