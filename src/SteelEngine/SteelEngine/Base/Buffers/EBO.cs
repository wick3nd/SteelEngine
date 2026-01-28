using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;

namespace SteelEngine.Base.Buffers
{
#pragma warning disable IDE0079, CA1822
    internal class EBO
    {
        private readonly int _Handle;

        internal EBO()
        {
            GL.GenBuffers(1, ref _Handle);

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create a EBO.");
                throw new Exception($"Failed to create a EBO.");
            }
        }

        public override string ToString() => $"{_Handle}";
        internal int GetHandle() => _Handle;
        internal void Data(int[] data) => GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(int), data, BufferUsage.StaticDraw);
        internal void Data(uint[] data) => GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data, BufferUsage.StaticDraw);
        internal void Enable() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, _Handle);
        internal void Disable() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        internal void Destroy() => GL.DeleteBuffer(_Handle);
    }
}