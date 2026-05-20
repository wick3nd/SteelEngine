namespace SteelEngine.Elements.Interfaces
{
    internal interface IEngineDisposable : IDisposable
    {
        public void Destroy() { }
        internal new void Dispose() { }
    }
}