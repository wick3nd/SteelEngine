using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Utils;

namespace SteelEngine.Base
{
    public class Mesh : IDisposable
    {
        private readonly int _vertexBufferObject;
        private readonly int _vertexArrayObject;
        private readonly int _elementBufferObject;
        private readonly int _instanceVerexBufferObject;
        private readonly int _offset;

        private readonly string _path;
        private readonly uint[] _indices;
        private readonly float[] _vertices;

        public Mesh(string path, int offset = 0, BufferUsageHint buh = BufferUsageHint.StaticDraw)
        {
            _path = path;
            _offset = offset;

            ModelImporter _ = new(path, offset, out _vertices, out _indices);

            _vertexBufferObject = GL.GenBuffer();
            _vertexArrayObject = GL.GenVertexArray();
            _elementBufferObject = GL.GenBuffer();
            _instanceVerexBufferObject = GL.GenBuffer();

            if (_vertexArrayObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VAO of mesh at path {path} and offset {offset}.");
                throw new Exception($"Failed to create VAO of mesh at path {path} and offset {offset}.");
            }
            if (_vertexBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create VBO of mesh at path {path} and offset {offset}.");
                throw new Exception($"Failed to create VBO of mesh at path {path} and offset {offset}.");
            }
            if (_elementBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create EBO of mesh at path {path} and offset {offset}.");
                throw new Exception($"Failed to create EBO of mesh at path {path} and offset {offset}.");
            }

            GL.BindVertexArray(_vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceVerexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, buh);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, buh);

            //GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, buh);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            SEDebug.Log(SEDebugState.Info, $"Created a mesh {path} from offset {offset}");
        }

        public void Draw(PrimitiveType type = PrimitiveType.Triangles)
        {
            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceVerexBufferObject);
            GL.DrawElementsInstanced(type, _indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, 1);
        }

        public void InstancingDraw(Matrix4[] instanceData, int attributeLocation, int instanceCount = 1, PrimitiveType type = PrimitiveType.Triangles)
        {
            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceVerexBufferObject);

            int size = instanceData.Length * Marshal.SizeOf<Matrix4>();
            GL.BufferData(BufferTarget.ArrayBuffer, size, instanceData, BufferUsageHint.StaticDraw);

            for (int i = 0; i < 4; i++)
            {
                int loc = attributeLocation + i;
                GL.EnableVertexAttribArray(loc);
                GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false,
                    64, (IntPtr)(i * 16));
                GL.VertexAttribDivisor(loc, 1);
            }

            GL.DrawElementsInstanced(type, _indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, instanceCount);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing mesh {_path} from offset {_offset}");
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
