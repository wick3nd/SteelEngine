using OpenTK.Graphics.OpenGL;
using SteelEngine.Core;
using SteelEngine.Core.Buffers;
using System.Runtime.InteropServices;

namespace SteelEngine.EngineBase.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SubMesh
    {
        internal PrimitiveType topologyType;
        internal MeshPrimitives primitiveFlags;

        internal float[] vertices;
        internal uint[] indices;
        internal MaterialContainer material;
        internal GLTexture2D texture;
    }
}