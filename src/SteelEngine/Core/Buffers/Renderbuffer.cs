using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

#pragma warning disable CA1816
namespace SteelEngine.Core.Buffers
{
    public class Renderbuffer : IBufferObject
    {
        private int m_RenderBuffer;
        private static int _currentBound;
        private readonly string? _debugName;

        private readonly int _width;
        private readonly int _height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Renderbuffer(int width, int height, InternalFormat pixelInternalFormat, int sampleCount = 1)
        {
            GL.GenRenderbuffers(1, ref m_RenderBuffer);

            if (m_RenderBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a RBO", true);
            SEDebug.Log(SEDebugState.Debug, $"Created a new RBO {m_RenderBuffer}");

            _width = width;
            _height = height;

            Enable();

            if (sampleCount == 1)
            {
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, pixelInternalFormat, _width, _height);
                return;
            }
            if (sampleCount > GLControl.MaxFramebufferSamples)
            {
                SEDebug.Log(SEDebugState.Warning, $"The sample count of RBO {m_RenderBuffer} was set too high, clamping to {GLControl.MaxFramebufferSamples}");
                sampleCount = Math.Clamp(sampleCount, 1, GLControl.MaxFramebufferSamples);
            }
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, sampleCount, pixelInternalFormat, _width, _height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Renderbuffer(string debugName, int width, int height, InternalFormat pixelInternalFormat, int sampleCount = 1)
        {
            GL.GenRenderbuffers(1, ref m_RenderBuffer);
            if (m_RenderBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a RBO \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Debug, $"Created a new RBO \"{debugName}\"");
            
            _width = width;
            _height = height;

            Enable();

            if (sampleCount == 1)
            {
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, pixelInternalFormat, _width, _height);
                return;
            }
            if (sampleCount > GLControl.MaxFramebufferSamples)
            {
                SEDebug.Log(SEDebugState.Warning, $"The sample count of RBO \"{debugName}\" was set too high, clamping to {GLControl.MaxFramebufferSamples}");
                sampleCount = Math.Clamp(sampleCount, 1, GLControl.MaxFramebufferSamples);
            }
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, sampleCount, pixelInternalFormat, _width, _height);
        }

        public override string ToString() => _debugName ?? $"{m_RenderBuffer}";
        public int GetHandle() => m_RenderBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_RenderBuffer)
            {
                _currentBound = m_RenderBuffer;
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, m_RenderBuffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_RenderBuffer && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_RenderBuffer != 0)
            {
                GL.DeleteRenderbuffer(m_RenderBuffer);

                _currentBound = 0;
                m_RenderBuffer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}