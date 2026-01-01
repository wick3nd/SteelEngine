using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;

namespace SteelEngine.Base.Buffers
{
#pragma warning disable IDE0079, CA1822
    internal class RBO
    {
        private readonly int _Handle;
        private readonly int _width;
        private readonly int _height;

        public RBO(int width, int height, InternalFormat pixelInternalFormat, int sampleCount = 1)
        {
            GL.GenRenderbuffers(1, ref _Handle);

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create a RBO.");
                throw new Exception($"Failed to create a RBO.");
            }

            _width = width;
            _height = height;

            Enable();

            if (sampleCount > GLControl.MaxFramebufferSamples)
            {
                SEDebug.Log(SEDebugState.Warning, $"The sample count of RBO {_Handle} was set too high, clamping");
                sampleCount = Math.Clamp(sampleCount, 1, GLControl.MaxFramebufferSamples);
            }
            if (sampleCount == 1)
            {
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, pixelInternalFormat, _width, _height);
                return;
            }
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, sampleCount, pixelInternalFormat, _width, _height);
        }

        public override string ToString() => $"{_Handle}";
        internal int GetHandle() => _Handle;
        internal void Enable() => GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _Handle);
        internal void Disable() => GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        internal void Destroy() => GL.DeleteRenderbuffer(_Handle);
    }
}