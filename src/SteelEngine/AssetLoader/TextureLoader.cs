using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using SteelEngine.Core;
using SteelEngine.Utils;
using w3.CRC;
using ZstdSharp;

namespace SteelEngine.AssetLoader
{
    internal class TextureLoader
    {
        private static readonly string[] _supportedExtensionList = [ ".jpg", "jpeg", ".png", ".bmp", ".tga", ".gif" ];

        private static readonly Dictionary<ColorComponents, PixelFormat> srcComp2PixType = new()
        {
            { ColorComponents.RedGreenBlue, PixelFormat.Rgb },
            { ColorComponents.RedGreenBlueAlpha, PixelFormat.Rgba },
            { ColorComponents.Grey, PixelFormat.Rgb },
            { ColorComponents.GreyAlpha, PixelFormat.Rgba }
        };

        private static readonly TextureWrapMode[] WrapModeTable = [
            TextureWrapMode.Repeat,
            TextureWrapMode.ClampToBorder,
            TextureWrapMode.ClampToEdge,
            TextureWrapMode.MirroredRepeat
        ];

        private static readonly int[] filteringTable = [
            9728,
            9729,
            9984,
            9985,
            9986,
            9987,
        ];

        internal static TextureContainer LoadTexture(string path)
        {
            if (ResourceManager.TryGetTextureInfo(path, out TextureContainer cachedTexture)) return cachedTexture;
            TextureContainer texture = new();

            using var stream = File.OpenRead(path);
            var image = ImageResult.FromStream(stream);
            var pixelFormat = srcComp2PixType.GetValueOrDefault(image.SourceComp, PixelFormat.Rgba);

            texture.path = Path.GetRelativePath(Environment.CurrentDirectory, path);

            texture.imageBytes = image.Data;
            texture.width = image.Width;
            texture.height = image.Height;
            texture.sourceComposition = pixelFormat;

            texture.wrapModeS = TextureWrapMode.Repeat;    ;       ;  ;
            texture.wrapModeT = TextureWrapMode.Repeat;    ;       ;  ;

            texture.minFilter = TextureMinFilter.Nearest;  ;  ;    ;
            texture.magFilter = TextureMagFilter.Nearest;  ;  ;    ;  ;;;;

            SEDebug.Log(SEDebugState.Info, $"Successfully opened a texture file[{path}]");

            ResourceManager.CacheTextureInfo(texture);
            return texture;
        }

        internal static CompressedTextureContainer LoadCompressedTexture(string path)
        {
            // add caching logic
            CompressedTextureContainer texture = new();

            using (FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                // 1. File validation
                if (fs.Length < 32)
                {
                    SEDebug.Log(SEDebugState.Error, $"File \"{path}\" is not a valid texture");
                    return texture;  // this will cause failure
                }

                ReadFromStream(fs, 0, 21, out byte[] buffer);
                if (!CRC8.Validate(buffer)) return texture;  // this will cause failure

                // 2. Texture preparation
                using (MemoryStream ms = new())
                {
                    DecompressZSTD(fs, ms);
                    int mipCount = buffer[15];

                    texture.path = path;

                    texture.imageBytes = new byte[mipCount][];
                    texture.width = BitConverter.ToInt16(buffer.AsSpan(6, 2));
                    texture.height = BitConverter.ToInt16(buffer.AsSpan(8, 2));

                    texture.internalPixelFormat = (InternalFormat)BitConverter.ToInt32(buffer.AsSpan(11, 4));

                    texture.wrapModeS = WrapModeTable[buffer[16]];
                    texture.wrapModeT = WrapModeTable[buffer[17]];
                    texture.minFilter = (TextureMinFilter)filteringTable[buffer[18]];
                    texture.magFilter = (TextureMagFilter)filteringTable[buffer[19]];

                    for (int i = 0; i < mipCount; i++)
                    {
                        ReadFromStream(ms, 0, 4, out byte[] temp);
                        int mipLength = BitConverter.ToInt32(temp.AsSpan());

                        texture.imageBytes[i] = new byte[mipLength];
                        ms.ReadExactly(texture.imageBytes[i], 0, mipLength);
                    }
                }
            }

            return texture;
        }

        internal static bool IsTextureCompressed(string path)
        {
            var extension = Path.GetExtension(path);

            if (Path.Exists(path) && string.Equals(extension, ".setx", StringComparison.CurrentCultureIgnoreCase)) return true;
            return false;
        }

        static void ReadFromStream(Stream stream, int offset, int count, out byte[] buffer)
        {
            buffer = new byte[count];

            stream.ReadExactly(buffer, offset, count);
        }

        public static void DecompressZSTD(Stream inStream, Stream outStream)
        {
            using (var ZSTDecoder = new DecompressionStream(inStream))
            {
                ZSTDecoder.CopyTo(outStream);
            }
            outStream.Flush();
            outStream.Position = 0;
        }
    }
}