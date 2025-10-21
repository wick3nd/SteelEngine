using OpenTK.Graphics.OpenGL4;
using SteelEngine.Utils;

namespace SteelEngine.Base.Buffers
{
    internal class VBO
    {
        private readonly int _Handle;

        public VBO()
        {
            _Handle = GL.GenBuffer();

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VBO {_Handle}");
                throw new Exception($"Failed to create VBO {_Handle}");
            }
        }

        public override string ToString() => $"{_Handle}";
        internal int GetHandle() => _Handle;
        internal void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, _Handle);
        internal void UnBind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        internal void Destroy() => GL.DeleteBuffer(_Handle);
    }
}
