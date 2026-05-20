namespace SteelEngine.Objects
{
    public class LoaderSettings
    {
        /// <summary>
        /// Decides if the model uses textures that were exported with it
        /// </summary>
        public bool UseReferencedTextures { internal get; set; } = true;
        public bool ExtractTexCoords { internal get; set; } = true;
        public bool ExtractNormals { internal get; set; } = true;
        public bool ExtractVertexColors { internal get; set; } = true;
    }
}