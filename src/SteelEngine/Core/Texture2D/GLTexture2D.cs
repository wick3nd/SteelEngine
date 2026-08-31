using OpenTK.Graphics.OpenGL;
using SteelEngine.AssetLoader;
using SteelEngine.Elements.Interfaces;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public class GLTexture2D : IEngineDisposable
    {
        private int m_Tex2D = -1;
        private static int _currentBound;
        private readonly string _debugName;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GLTexture2D(string path, TextureLoaderSettings settings)
        {
            _debugName = Path.GetRelativePath(Environment.CurrentDirectory, path);

            GL.GenTextures(1, ref m_Tex2D);  // GL 1.1
            if (m_Tex2D == 0) SEDebug.Log(SEDebugState.Error, "Failed to create a Texture2D handle", throwException: true);
            Bind();

            if (TextureLoader.IsTextureCompressed(path)) CreateTexture2D(TextureLoader.LoadCompressedTexture(path), settings.LoadMipMaps);
            else CreateTexture2D(TextureLoader.LoadTexture(path), settings.LoadMipMaps);

            SEDebug.Log(SEDebugState.Info, $"Created a Texture2D[{this}]");
        }

        void CreateTexture2D(CompressedTextureContainer compressedTexture, bool loadMips)
        {
            int mipsToProcess = loadMips ? compressedTexture.imageBytes.Length : 1;

            for (int i = 0; i < mipsToProcess; i++)
            {
                int mipWidth = compressedTexture.width >> i;
                int mipHeight = compressedTexture.height >> i;
                var mipmap = compressedTexture.imageBytes[i].AsSpan();

                GL.CompressedTexImage2D(TextureTarget.Texture2D, i, compressedTexture.internalPixelFormat, mipWidth, mipHeight, 0, mipmap.Length, mipmap);

                WrapMode(compressedTexture.wrapModeS, compressedTexture.wrapModeT);
                FilterMode(compressedTexture.minFilter, compressedTexture.magFilter);
            }
        }

        void CreateTexture2D(TextureContainer texture, bool loadMips)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, (InternalFormat)texture.sourceComposition, texture.width, texture.height, 0, texture.sourceComposition, PixelType.UnsignedByte, texture.imageBytes);

            if (loadMips) GL.GenerateMipmap(TextureTarget.Texture2D);

            WrapMode(texture.wrapModeS, texture.wrapModeT);
            FilterMode(texture.minFilter, texture.magFilter);
        }

        public void WrapMode(TextureWrapMode wrapModeS = TextureWrapMode.Repeat, TextureWrapMode wrapModeT = TextureWrapMode.Repeat)
        {
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapModeS);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapModeT);
        }
        public void FilterMode(TextureMinFilter filterMin = TextureMinFilter.Nearest, TextureMagFilter FilterMag = TextureMagFilter.Nearest)
        {
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filterMin);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)FilterMag);
        }
        internal void SetUnit(TextureUnit unit) => GL.ActiveTexture(unit);

        public override string ToString() => _debugName ?? $"{m_Tex2D}";
        public int GetHandle() => m_Tex2D;

        public void Bind()
        {
            if (_currentBound != m_Tex2D)
            {
                _currentBound = m_Tex2D;
                GL.BindTexture(TextureTarget.Texture2D, m_Tex2D);
            }
        }
        public void Unbind()
        {
            if (_currentBound == m_Tex2D)
            {
                _currentBound = 0;
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        private void RemoveBase()
        {
            if (m_Tex2D != 0)
            {
                GL.DeleteTexture(m_Tex2D);

                _currentBound = 0;
                m_Tex2D = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            RemoveBase();
            ResourceManager.RemoveTextureInfo(_debugName);
            SEDebug.Log(SEDebugState.Info, $"Destroyed Texture2D \"{this}\"");
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    RemoveBase();
                    SEDebug.Log(SEDebugState.Info, $"Disposed Texture2D \"{this}\"");
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}