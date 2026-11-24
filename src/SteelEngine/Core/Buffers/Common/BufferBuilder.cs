namespace SteelEngine.Core.Buffers
{
    public struct BufferBuilder(DataType dataType)
    {
        internal int size = (int)dataType;
    }
}
