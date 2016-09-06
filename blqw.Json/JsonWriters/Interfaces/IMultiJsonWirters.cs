namespace blqw.Serializable.JsonWriters
{
    internal interface IMultiJsonWriters
    {
        void Add(JsonWriterWrapper writer);
    }
}