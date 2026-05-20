using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using SteelEngine.Core.EngineBehaviour;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
#pragma warning disable IDE0079, CA1816, CA1822
    public class Texture2D : IDisposable
    {
        private int m_Tex2D;
        private static int _currentBound;
        private readonly string _debugName;

        public readonly PixelFormat pixelFormat;

        private readonly Dictionary<ColorComponents, PixelFormat> srcComp2PixType = new()
        {
            { ColorComponents.RedGreenBlue, PixelFormat.Rgb },
            { ColorComponents.RedGreenBlueAlpha, PixelFormat.Rgba }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Texture2D(string path = "", bool generateMipmap = true, InternalFormat GPUPixelFormat = InternalFormat.Rgb, PixelType pixelType = PixelType.UnsignedByte)
        {
            GL.GenTextures(1, ref m_Tex2D);  // GL 1.1
            if (m_Tex2D == 0) SEDebug.Log(SEDebugState.Error, "Failed to create a Texture2D", throwException: true);

            _debugName = Path.GetRelativePath(Environment.CurrentDirectory, path);
            Enable();
            if (path == "" || !File.Exists(path))
            {
                byte[] imgData = [ 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF ];
                GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, 2, 2, 0, PixelFormat.Rgb, PixelType.UnsignedByte, imgData);  // GL 1.0

                SEDebug.Log(SEDebugState.Error, $"Created a Texture2D[{_debugName}] with a missing texture");
            }
            else
            {
                using var stream = File.OpenRead(path);
                var image = ImageResult.FromStream(stream);
                pixelFormat = srcComp2PixType.GetValueOrDefault(image.SourceComp, PixelFormat.Rgba);

                GL.TexImage2D(TextureTarget.Texture2D, 0, GPUPixelFormat, image.Width, image.Height, 0, pixelFormat, pixelType, image.Data);  // GL 1.0
                SEDebug.Log(SEDebugState.Info, $"Created a Texture2D[{_debugName}]");
            }

            if (generateMipmap) GL.GenerateMipmap(TextureTarget.Texture2D);  // GL 3.0

            WrapMode();
            FilterMode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Texture2D(string debugName, int width, int height, bool generateMipmap = true, PixelFormat texturePixelFormat = PixelFormat.Rgb, InternalFormat GPUPixelFormat = InternalFormat.Rgb, PixelType pixelType = PixelType.UnsignedByte)
        {
            GL.GenTextures(1, ref m_Tex2D);  // GL1.1
            if (m_Tex2D == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a Texture2D \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Info, $"Created a Texture2D \"{debugName}\"");

            Enable();
            GL.TexImage2D(TextureTarget.Texture2D, 0, GPUPixelFormat, width, height, 0, pixelFormat = texturePixelFormat, pixelType, IntPtr.Zero);  // GL1.0

            if (generateMipmap) GL.GenerateMipmap(TextureTarget.Texture2D);  // GL3.0

            WrapMode();
            FilterMode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WrapMode(TextureWrapMode wrapModeS = TextureWrapMode.Repeat, TextureWrapMode wrapModeT = TextureWrapMode.Repeat)
        {
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapModeS);  // GL 1.0
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapModeT);  // GL 1.0
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FilterMode(TextureMinFilter filterMin = TextureMinFilter.NearestMipmapNearest, TextureMagFilter FilterMag = TextureMagFilter.Nearest)
        {
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filterMin);  // GL 1.0
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)FilterMag);  // GL 1.0
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetUnit(TextureUnit unit) => GL.ActiveTexture(unit);  // GL 

        public override string ToString() => _debugName ?? $"{m_Tex2D}";
        public int GetHandle() => m_Tex2D;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_Tex2D)
            {
                _currentBound = m_Tex2D;
                GL.BindTexture(TextureTarget.Texture2D, m_Tex2D);  // GL 1.1
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_Tex2D)
            {
                _currentBound = 0;
                GL.BindTexture(TextureTarget.Texture2D, 0);  // GL 1.1
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_Tex2D != 0)
            {
                ResourceManager.RemoveTexture2D(this);
                GL.DeleteTexture(m_Tex2D);  // GL 1.1

                _currentBound = 0;
                m_Tex2D = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}