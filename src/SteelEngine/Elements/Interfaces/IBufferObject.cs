namespace SteelEngine.Core
{
    public interface IBufferObject : IDisposable
    {
        int GetHandle();
        void Enable() { }
        void Disable() { }
        void Destroy() { }
    }
}
