using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteelEngine.Utils;
using SteelEngine.Base.Buffers;
using System.Runtime.InteropServices;

namespace SteelEngine.Base
{
    public class Mesh : EngineScript, IDisposable
    {
        private readonly VAO _vertexArrayObject;  // VAO
        private readonly VBO _vertexBufferObject;  // VBO
        private readonly EBO _elementBufferObject;  // EBO
        private readonly int _instanceVertexBufferObject;

        private readonly string _path;
        private readonly uint[] _indices;
        private readonly float[] _vertices;

        private bool drawn;


        public Mesh(string path, bool instanced = false)
        {
            _path = path;

            ModelImporter _ = new(path, out _vertices, out _indices);

            _vertexArrayObject = new();
            _vertexBufferObject = new();
            _elementBufferObject = new();
            if (instanced) _instanceVertexBufferObject = GL.GenBuffer();


            if (instanced && _instanceVertexBufferObject == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create instance EBO of mesh at path {path}.");
                throw new Exception($"Failed to create instance EBO of mesh at path {path}.");
            }

            _vertexBufferObject.Bind();

            _elementBufferObject.Bind();
            _elementBufferObject.Data(_indices.Length * sizeof(uint), _indices);

            _vertexArrayObject.Enable();
            _vertexArrayObject.Data(_vertices);

            _vertices = [];  // Clear the array

            SEDebug.Log(SEDebugState.Info, $"Created a new mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");
        }


        public void Draw(PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArrayObject.Bind();
            GL.DrawElementsInstanced(type, _indices.Length, DrawElementsType.UnsignedInt, 0, 1);

            if (!drawn)
            {
                SEDebug.Log(SEDebugState.Debug, $"Drawn a Mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");
                drawn = true;
            }
        }


        public void DrawInstanced(Matrix4[] instanceData, PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArrayObject.Bind();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceVertexBufferObject);

            int size = instanceData.Length * Marshal.SizeOf<Matrix4>();
            GL.BufferData(BufferTarget.ArrayBuffer, size, instanceData, BufferUsageHint.StaticDraw);

            for (int i = 0; i < 4; i++)
            {
                int loc = 2 + i;
                GL.EnableVertexAttribArray(loc);
                GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 64, i * 16);
                GL.VertexAttribDivisor(loc, 1);
            }

            GL.DrawElementsInstanced(type, _indices.Length, DrawElementsType.UnsignedInt, 0, instanceData.Length);

            if (!drawn)
            {
                SEDebug.Log(SEDebugState.Debug, $"Drawn an instanced Mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");
                drawn = true;
            }
        }


        public override void OnExit() => Dispose();


        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing Mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");
                GL.DeleteBuffer(_instanceVertexBufferObject);
                _vertexBufferObject.Destroy();
                _vertexArrayObject.Destroy();
                _elementBufferObject.Destroy();

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
            drawn = false;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}