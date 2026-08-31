using OpenTK.Graphics.OpenGL;
using SteelEngine.Core;

namespace SteelEngine.AssetLoader
{
    public class MeshLoaderSettings
    {
        /// <summary>
        /// Decides if the model uses textures that were exported alongside it
        /// </summary>
        public bool UseReferencedTextures { internal get; set; } = true;
        public bool ExtractTexCoords { internal get; set; } = true;
        public bool ExtractNormals { internal get; set; } = true;
        public bool ExtractVertexColors { internal get; set; } = true;
    }

    public class TextureLoaderSettings
    {
        public bool LoadMipMaps { internal get; set; } = false;
        public InternalFormat GPUPixelFormat { internal get; set; } = InternalFormat.Rgb;
        public PixelType PixelFormat { internal get; set; } = PixelType.UnsignedByte;
        public ColorChannel Channels { internal get; set; } = ColorChannel.exported;
        public TextureWrapMode DefaultWrapModeS { internal get; set; } = TextureWrapMode.Repeat;
        public TextureWrapMode DefaultWrapModeT { internal get; set; } = TextureWrapMode.Repeat;
        public TextureMinFilter DefaultMinFilter { internal get; set; } = TextureMinFilter.NearestMipmapNearest;
        public TextureMagFilter DefaultMagFilter { internal get; set; } = TextureMagFilter.Nearest;
    }

    public class MaterialLoaderSettings
    {

    }
}