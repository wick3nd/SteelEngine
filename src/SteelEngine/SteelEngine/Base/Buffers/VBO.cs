using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.InteropServices;

namespace SteelEngine.Base.Buffers
{
#pragma warning disable IDE0079, CA1822
    internal class VBO
    {
        private readonly int _Handle;

        internal VBO()
        {
            GL.GenBuffers(1, ref _Handle);

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create a VBO.");
                throw new Exception($"Failed to create a VBO.");
            }
        }


        internal int GetHandle() => _Handle;
        internal void Data<T>(T[] data) where T : unmanaged
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Marshal.SizeOf<T>(), data.AsSpan(), BufferUsage.StaticDraw);
        }
        internal void UpdateData<T>(T[] data, int offset = 0) where T : unmanaged
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _Handle);
            GL.BufferSubData(BufferTarget.ArrayBuffer, offset, data.Length * Marshal.SizeOf<T>(), data.AsSpan());
        }
        public override string ToString() => $"{_Handle}";
        internal void Enable() => GL.BindBuffer(BufferTarget.ArrayBuffer, _Handle);
        internal void Disable() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        internal void Destroy() => GL.DeleteBuffer(_Handle);
    }
}
