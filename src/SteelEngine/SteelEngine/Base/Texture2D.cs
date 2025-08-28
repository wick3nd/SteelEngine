using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using SteelEngine.Utils;

namespace SteelEngine.Base
{
    class Texture2D : IDisposable
    {
        private readonly int _Handle;

        private readonly TextureUnit _unit;


        public Texture2D(string path = "", TextureUnit unit = TextureUnit.Texture0, bool generateMipmap = true, PixelInternalFormat pixelFormat = PixelInternalFormat.Rgb, PixelType pixelType = PixelType.UnsignedByte)
        {
            _unit = unit;

            _Handle = GL.GenTexture();

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, "Failed to generate Texture2D");
                throw new Exception();
            }

            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _Handle);

            StbImage.stbi_set_flip_vertically_on_load(1);

            if (path == "" || !File.Exists(path))
            {
                byte[] imgData = [ 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF ];        // Additional 2 bytes after the first row because of OpenGL fuckery
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 2, 2, 0, PixelFormat.Rgb, PixelType.UnsignedByte, imgData);

                SEDebug.Log(SEDebugState.Error, $"Created a missing Texture2D handle {_Handle}");
            }
            else
            {
                using var stream = File.OpenRead(path);
                var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2D, 0, pixelFormat, image.Width, image.Height, 0, PixelFormat.Rgba, pixelType, image.Data);

                SEDebug.Log(SEDebugState.Info, $"Created Texture2D handle {_Handle}");
            }

            if (generateMipmap == true) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public void ChangeWrapMode(TextureWrapMode wrapModeS, TextureWrapMode wrapModeT)
        {
            GL.ActiveTexture(_unit);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapModeS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapModeT);
        }

        public void ChangeFilterMode(TextureMinFilter filterMin = TextureMinFilter.NearestMipmapNearest, TextureMagFilter FilterMag = TextureMagFilter.Nearest)
        {
            GL.ActiveTexture(_unit);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filterMin);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)FilterMag);
        }

        public int Handle => _Handle;

        public void Use()
        {
            GL.ActiveTexture(_unit);
            GL.BindTexture(TextureTarget.Texture2D, _Handle);
        }

        public void UnBind()
        {
            GL.ActiveTexture(_unit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing Texture2D handle {_Handle}");
                GL.DeleteTexture(_Handle);

                disposedValue = true;
            }
        }

        ~Texture2D()
        {
            if (disposedValue == false)
            {
                SEDebug.Log(SEDebugState.Warning, "GPU Resource leak, did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
