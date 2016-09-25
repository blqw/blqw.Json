using System;

namespace blqw.Serializable
{
    public interface IServiceProvider : IJsonWriter
    {
        IJsonWriter GetService(Type serviceType); 
    }
}