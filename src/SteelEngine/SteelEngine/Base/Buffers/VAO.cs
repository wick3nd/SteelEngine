using OpenTK.Graphics.OpenGL;
using SteelEngine.EngineBase.Structs;
using SteelEngine.Utils;

namespace SteelEngine.Base.Buffers
{
#pragma warning disable IDE0079, CA1822
    internal class VAO
    {
        private readonly int _Handle;

        internal VAO()
        {
            GL.GenVertexArrays(1, ref _Handle);

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create a VAO.");
                throw new Exception($"Failed to create a VAO.");
            }
        }

        internal int GetHandle() => _Handle;
        internal void Set()
        {
            GL.VertexAttribPointer((int)VAOAttribPointer.aPosition, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer((int)VAOAttribPointer.aTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray((int)VAOAttribPointer.aPosition);
            GL.EnableVertexAttribArray((int)VAOAttribPointer.aTexCoord);
        }
        public override string ToString() => $"{_Handle}";
        internal void Enable() => GL.BindVertexArray(_Handle);
        internal void Disable() => GL.BindVertexArray(0);
        internal void Destroy() => GL.DeleteVertexArray(_Handle);
    }
}
