namespace SteelEngine
{
    public class AdvancedMeshSettings
    {
        public bool UseReferencedTextures { internal get; set; } = true;
        public bool LoadTexCoords { internal get; set; } = true;
        public bool LoadNormals { internal get; set; } = true;
        public bool LoadVertexColors { internal get; set; } = true;
    }
}
