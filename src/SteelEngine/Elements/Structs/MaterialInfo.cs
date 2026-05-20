using SharpGLTF.Schema2;

namespace SteelEngine.Core
{
    public struct MaterialInfo
    {
        public MaterialChannel[] Material { get; internal set; }

        public AlphaMode AlphaMode { get; internal set; }
        public float AlphaCutoff { get; internal set; }
        public float Dispersion { get; internal set; }
        public bool DoubleSided { get; internal set; }
    }
}