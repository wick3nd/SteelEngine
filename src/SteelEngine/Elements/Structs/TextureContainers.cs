using OpenTK.Graphics.OpenGL;

namespace SteelEngine.Core
{
    internal struct TextureContainer
    {
        internal string path;

        internal byte[] imageBytes;
        internal int width;
        internal int height;

        internal PixelFormat sourceComposition;
        internal TextureWrapMode wrapModeS;
        internal TextureWrapMode wrapModeT;
        internal TextureMinFilter minFilter;
        internal TextureMagFilter magFilter;

        public override readonly string ToString() => path;
        public TextureContainer()
        {
            path = "";

            imageBytes = [ 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00,    0x00, 0x00,    0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF ];
            width = 2;
            height = 2;

            sourceComposition = PixelFormat.Rgb;

            wrapModeS = TextureWrapMode.Repeat;
            wrapModeT = TextureWrapMode.Repeat;
            minFilter = TextureMinFilter.Nearest;
            magFilter = TextureMagFilter.Nearest;
        }
    }

    internal struct CompressedTextureContainer
    {
        internal string path;

        internal byte[][] imageBytes;
        internal int width;
        internal int height;

        internal InternalFormat internalPixelFormat;

        internal TextureWrapMode wrapModeS;
        internal TextureWrapMode wrapModeT;
        internal TextureMinFilter minFilter;
        internal TextureMagFilter magFilter;

        public override readonly string ToString() => path;
        public CompressedTextureContainer()
        {
            path = "";
        }
    }
}
