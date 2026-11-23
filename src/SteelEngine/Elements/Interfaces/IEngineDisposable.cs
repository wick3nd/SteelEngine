namespace SteelEngine.Elements.Interfaces
{
    internal interface IEngineDisposable : IDisposable
    {
        public void Destroy() { }
        internal void Dispose() { }
    }
}