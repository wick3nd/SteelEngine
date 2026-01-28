using System.Runtime.InteropServices;

namespace SteelEngine.EngineBase.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MeshStr
    {
        internal Vertex[] vertices;
        internal uint[] indices;
    }
}