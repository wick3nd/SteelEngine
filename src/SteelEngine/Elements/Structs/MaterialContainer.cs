using OpenTK.Mathematics;
using SharpGLTF.Schema2;

namespace SteelEngine.Core
{
    public struct MaterialContainer
    {
        public string Name { get; internal set; }
        public Vector4 ColorTexture { get; internal set; }
        public AlphaMode AlphaMode { get; internal set; }
        public float AlphaCutoff { get; internal set; }
        public float Dispersion { get; internal set; }
        public bool DoubleSided { get; internal set; }
        public bool Unlit { get; internal set; }
    }
}