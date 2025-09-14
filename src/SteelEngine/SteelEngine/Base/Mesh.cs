using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteelEngine.Utils;

namespace SteelEngine.Base
{
    public class Mesh : IDisposable
    {
        private readonly int _vertexBufferObject;
        private readonly int _vertexArrayObject;
        private readonly int _elementBufferObject;
        private readonly int _instanceVertexBufferObject;

        private readonly string _path;
        private readonly uint[] _indices;
        private readonly float[] _vertices;

        internal enum VAOAttribPointer
        {
            aPosition,
            aTexCoord
        }

        public Mesh(string path)
        {
            _path = path;

            ModelImporter _ = new(path, out _vertices, out _indices);

            _vertexBufferObject = GL.GenBuffer();
            _vertexArrayObject = GL.GenVertexArray();
            _elementBufferObject = GL.GenBuffer();
            _instanceVertexBufferObject = GL.GenBuffer();

            if (_vertexArrayObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VAO of mesh at path {path}.");
                throw new Exception($"Failed to create VAO of mesh at path {path}.");
            }
            if (_vertexBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VBO of mesh at path {path}.");
                throw new Exception($"Failed to create VBO of mesh at path {path}.");
            }
            if (_elementBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create EBO of mesh at path {path}.");
                throw new Exception($"Failed to create EBO of mesh at path {path}.");
            }
            if (_instanceVertexBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create instance EBO of mesh at path {path}.");
                throw new Exception($"Failed to create instance EBO of mesh at path {path}.");
            }

            GL.BindVertexArray(_vertexArrayObject);    // Binds the VAO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);    // Binds the VBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);    // Binds the EBO
            // GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceVertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);    // Adds data to VAO
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);    // Adds data to EBO

            GL.VertexAttribPointer((int)VAOAttribPointer.aPosition, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer((int)VAOAttribPointer.aTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray((int)VAOAttribPointer.aPosition);
            GL.EnableVertexAttribArray((int)VAOAttribPointer.aTexCoord);

            _vertices = [];

            SEDebug.Log(SEDebugState.Info, $"Created a mesh {path}.");
        }

        public Mesh(float[] vertices, uint[] indices)
        {
            _vertexBufferObject = GL.GenBuffer();
            _vertexArrayObject = GL.GenVertexArray();
            _elementBufferObject = GL.GenBuffer();
            _instanceVertexBufferObject = GL.GenBuffer();

            if (_vertexArrayObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VAO of mesh at path .");
                throw new Exception($"Failed to create VAO of mesh at path .");
            }
            if (_vertexBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VBO of mesh at path.");
                throw new Exception($"Failed to create VBO of mesh at path .");
            }
            if (_elementBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create EBO of mesh at path.");
                throw new Exception($"Failed to create EBO of mesh at path .");
            }
            if (_instanceVertexBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create instance EBO of mesh at path .");
                throw new Exception($"Failed to create instance EBO of mesh at path .");
            }

            _path = "";
            _indices = indices;

            GL.BindVertexArray(_vertexArrayObject);    // Binds the VAO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);    // Binds the VBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);    // Binds the EBO
            // GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceVertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);    // Adds data to VAO
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);    // Adds data to EBO

            GL.VertexAttribPointer((int)VAOAttribPointer.aPosition, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer((int)VAOAttribPointer.aTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray((int)VAOAttribPointer.aPosition);
            GL.EnableVertexAttribArray((int)VAOAttribPointer.aTexCoord);

            _vertices = [];

            SEDebug.Log(SEDebugState.Info, $"Created a mesh.");
        }

        public void Draw(PrimitiveType type = PrimitiveType.Triangles)
        {
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElementsInstanced(type, _indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, 1);
        }

        public void DrawInstanced(Matrix4[] instanceData, PrimitiveType type = PrimitiveType.Triangles)
        {
            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceVertexBufferObject);

            int size = instanceData.Length * Marshal.SizeOf<Matrix4>();
            GL.BufferData(BufferTarget.ArrayBuffer, size, instanceData, BufferUsageHint.StaticDraw);

            for (int i = 0; i < 4; i++)
            {
                int loc = 2 + i;
                GL.EnableVertexAttribArray(loc);
                GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 64, (IntPtr)(i * 16));
                GL.VertexAttribDivisor(loc, 1);
            }

            GL.DrawElementsInstanced(type, _indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, instanceData.Length);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing mesh {_path}");
                GL.DeleteBuffer(_instanceVertexBufferObject);
                GL.DeleteBuffer(_vertexBufferObject);
                GL.DeleteVertexArray(_vertexArrayObject);
                GL.DeleteBuffer(_elementBufferObject);

                disposedValue = true;
            }
        }

        ~Mesh()
        {
            if (disposedValue == false)
            {
                SEDebug.Log(SEDebugState.Warning, "GPU Resource leak, did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
