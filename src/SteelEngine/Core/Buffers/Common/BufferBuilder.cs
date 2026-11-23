namespace SteelEngine.Core.Buffers
{
    public struct BufferBuilder
    {
        internal int size;
        internal byte[] data;
        public BufferBuilder(DataType dataType)
        {
            size = (int)dataType;
        }
    }
}
