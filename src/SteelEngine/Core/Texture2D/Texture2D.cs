using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
#pragma warning disable IDE0079, CA1816, CA1822
    public class Texture2D : IDisposable
    {
        private int m_Tex2D;
        private static int _currentBound;
        private readonly string? _debugName;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Texture2D(string path = "", bool generateMipmap = true, PixelFormat pixelFormat = PixelFormat.Rgb, InternalFormat pixelInternalFormat = InternalFormat.Rgb, PixelType pixelType = PixelType.UnsignedByte)
        {
            GL.GenTextures(1, ref m_Tex2D);
            if (m_Tex2D == 0) SEDebug.Log(SEDebugState.Error, "Failed to create a Texture2D", true);

            _debugName = Path.GetFileName(path);
            Enable();
            if (path == "" || !File.Exists(path))
            {
                byte[] imgData = [ 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF ];
                GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb, 2, 2, 0, PixelFormat.Rgb, PixelType.UnsignedByte, imgData);

                SEDebug.Log(SEDebugState.Error, $"Created a Texture2D \"{_debugName}\" with a missing texture");
            }
            else
            {
                StbImage.stbi_set_flip_vertically_on_load(1);

                using var stream = File.OpenRead(path);
                var image = ImageResult.FromStream(stream);
                
                GL.TexImage2D(TextureTarget.Texture2d, 0, pixelInternalFormat, image.Width, image.Height, 0, pixelFormat, pixelType, image.Data);
                SEDebug.Log(SEDebugState.Info, $"Created a Texture2D \"{_debugName}\"");
            }

            if (generateMipmap) GL.GenerateMipmap(TextureTarget.Texture2d);

            ChangeWrapMode();
            ChangeFilterMode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Texture2D(int width, int height, bool generateMipmap = true, PixelFormat pixelFormat = PixelFormat.Rgb, InternalFormat pixelInternalFormat = InternalFormat.Rgb, PixelType pixelType = PixelType.UnsignedByte)
        {
            GL.GenTextures(1, ref m_Tex2D);
            if (m_Tex2D == 0) SEDebug.Log(SEDebugState.Error, "Failed to create a Texture2D", true);
            SEDebug.Log(SEDebugState.Info, $"Created a Texture2D {m_Tex2D}");

            Enable();
            GL.TexImage2D(TextureTarget.Texture2d, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);

            if (generateMipmap) GL.GenerateMipmap(TextureTarget.Texture2d);

            ChangeWrapMode();
            ChangeFilterMode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Texture2D(string debugName, int width, int height, bool generateMipmap = true, PixelFormat pixelFormat = PixelFormat.Rgb, InternalFormat pixelInternalFormat = InternalFormat.Rgb, PixelType pixelType = PixelType.UnsignedByte)
        {
            GL.GenTextures(1, ref m_Tex2D);
            if (m_Tex2D == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a Texture2D \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Info, $"Created a Texture2D \"{debugName}\"");

            Enable();
            GL.TexImage2D(TextureTarget.Texture2d, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);

            if (generateMipmap) GL.GenerateMipmap(TextureTarget.Texture2d);

            ChangeWrapMode();
            ChangeFilterMode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeWrapMode(TextureWrapMode wrapModeS = TextureWrapMode.Repeat, TextureWrapMode wrapModeT = TextureWrapMode.Repeat)
        {
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)wrapModeS);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)wrapModeT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeFilterMode(TextureMinFilter filterMin = TextureMinFilter.NearestMipmapNearest, TextureMagFilter FilterMag = TextureMagFilter.Nearest)
        {
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)filterMin);
            GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)FilterMag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUnit(TextureUnit unit) => GL.ActiveTexture(unit);

        public override string ToString() => _debugName ?? $"{m_Tex2D}";
        public int GetHandle() => m_Tex2D;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_Tex2D)
            {
                _currentBound = m_Tex2D;
                GL.BindTexture(TextureTarget.Texture2d, m_Tex2D);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_Tex2D)
            {
                _currentBound = 0;
                GL.BindTexture(TextureTarget.Texture2d, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_Tex2D != 0)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing Texture2D handle {m_Tex2D}");
                GL.DeleteTexture(m_Tex2D);

                _currentBound = 0;
                m_Tex2D = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}