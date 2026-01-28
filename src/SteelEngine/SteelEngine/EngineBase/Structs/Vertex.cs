using System.Numerics;
using System.Runtime.InteropServices;

namespace SteelEngine.EngineBase.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vertex
    {
        internal Vector3 position;
        internal Vector3 normals;
        internal Vector2 UV;
    }
}