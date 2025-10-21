using OpenTK.Graphics.OpenGL4;
using SteelEngine.Utils;

namespace SteelEngine.Base.Buffers
{
    internal class EBO
    {
        private readonly int _Handle;

        public EBO()
        {
            _Handle = GL.GenBuffer();

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create a EBO {_Handle}");
                throw new Exception($"Failed to create a EBO {_Handle}");
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _Handle);
        }

        public override string ToString() => $"{_Handle}";
        internal int GetHandle() => _Handle;
        internal void Data(int size, uint[] data) => GL.BufferData(BufferTarget.ElementArrayBuffer, size, data, BufferUsageHint.StaticDraw);
        internal void Bind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, _Handle);
        internal void UnBind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        internal void Destroy() => GL.DeleteBuffer(_Handle);
    }
}
