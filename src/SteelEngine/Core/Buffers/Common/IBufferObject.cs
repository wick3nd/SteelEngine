namespace SteelEngine.Core.Buffers
{
    public interface IBufferObject : IDisposable
    {
        int GetHandle();
        void Enable() { }
        void Disable() { }
    }
}