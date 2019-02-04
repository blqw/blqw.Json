using System;

namespace blqw.JsonServices
{
    public interface IGenericJsonWriter : IJsonWriter
    {
        IJsonWriter MakeGenericType(Type genericType);
    }
}