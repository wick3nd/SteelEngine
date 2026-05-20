using System.Runtime.InteropServices;

namespace SteelEngine.EngineBase.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Mesh
    {
        string path;
        internal Vertex[] vertices;
        internal uint[] indices;
    }
}