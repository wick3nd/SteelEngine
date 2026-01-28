using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using SteelEngine.EngineBase.EngineBehaviour;
using SteelEngine.Utils;

namespace SteelEngine.Base
{
    class Texture2D : EngineScript, IDisposable
    {
        private readonly int _Handle;
        private readonly TextureUnit _unit;

        public Texture2D(string path = "", TextureUnit unit = TextureUnit.Texture0, bool generateMipmap = true, PixelFormat pixelFormat = PixelFormat.Rgba, InternalFormat pixelInternalFormat = InternalFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
        {
            _unit = unit;
            _Handle = GL.GenTexture();

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, "Failed to generate Texture2D");
                throw new Exception();
            }
            GL.BindTexture(TextureTarget.Texture2d, _Handle);

            if (path == "" || !File.Exists(path))
            {
                byte[] imgData = [ 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF ];
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb, 2, 2, 0, PixelFormat.Rgb, PixelType.UnsignedByte, imgData);

                SEDebug.Log(SEDebugState.Error, $"Created Texture2D handle {_Handle} with a missing texture");
            }
            else
            {
                StbImage.stbi_set_flip_vertically_on_load(1);

                using var stream = File.OpenRead(path);
                var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                
                GL.TexImage2D(TextureTarget.Texture2d, 0, pixelInternalFormat, image.Width, image.Height, 0, pixelFormat, pixelType, image.Data);

                SEDebug.Log(SEDebugState.Info, $"Created Texture2D handle {_Handle}");
            }

            if (generateMipmap == true) GL.GenerateMipmap(TextureTarget.Texture2d);

            ChangeWrapMode();
            ChangeFilterMode();
        }

        public Texture2D(int width, int height, TextureUnit unit = TextureUnit.Texture0, PixelFormat pixelFormat = PixelFormat.Rgba, InternalFormat pixelInternalFormat = InternalFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
        {
            _unit = unit;
            _Handle = GL.GenTexture();

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, "Failed to generate Texture2D");
                throw new Exception();
            }

            GL.BindTexture(TextureTarget.Texture2d, _Handle);
            GL.TexImage2D(TextureTarget.Texture2d, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);

            ChangeWrapMode();
            ChangeFilterMode();
        }

        public void ChangeWrapMode(TextureWrapMode wrapModeS = TextureWrapMode.Repeat, TextureWrapMode wrapModeT = TextureWrapMode.Repeat)
        {
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)wrapModeS);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)wrapModeT);
        }

        public void ChangeFilterMode(TextureMinFilter filterMin = TextureMinFilter.NearestMipmapNearest, TextureMagFilter FilterMag = TextureMagFilter.Nearest)
        {
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)filterMin);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)FilterMag);
        }

        public int GetHandle() => _Handle;

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2d, _Handle);
        }

        public void UnBind()
        {
            GL.ActiveTexture(_unit);
            GL.BindTexture(TextureTarget.Texture2d, 0);
        }

        public override void OnExit() => Dispose();
        public void Dispose()
        {
                SEDebug.Log(SEDebugState.Info, $"Disposing Texture2D handle {_Handle}");
                GL.DeleteTexture(_Handle);
        }
    }
}
