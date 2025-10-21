using OpenTK.Graphics.OpenGL4;
using SteelEngine.Base.Structs;
using SteelEngine.Utils;

namespace SteelEngine.Base.Buffers
{
    internal class VAO
    {
        private readonly int _Handle;

        public VAO()
        {
            _Handle = GL.GenVertexArray();

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VAO {_Handle}");
                throw new Exception($"Failed to create VAO {_Handle}");
            }

            GL.BindVertexArray(_Handle);
        }

        internal int GetHandle() => _Handle;
        internal void Enable()
        {
            GL.VertexAttribPointer((int)VAOAttribPointer.aPosition, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer((int)VAOAttribPointer.aTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray((int)VAOAttribPointer.aPosition);
            GL.EnableVertexAttribArray((int)VAOAttribPointer.aTexCoord);
        }

        public override string ToString() => $"{_Handle}";
        internal void Data(float[] vertices) => GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        internal void Bind() => GL.BindVertexArray(_Handle);
        internal void UnBind() => GL.BindVertexArray(0);
        internal void Destroy() => GL.DeleteVertexArray(_Handle);
    }
}
