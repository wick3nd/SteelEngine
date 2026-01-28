using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Utils;
using SteelEngine.Base.Buffers;
using SteelEngine.EngineBase.Structs;
using SteelEngine.EngineBase.EngineBehaviour;
using SteelEngine.Elements;

namespace SteelEngine.Base
{
    internal class Mesh : EngineScript, IDisposable
    {
        private readonly VAO _vertexArrayObject;
        private readonly VBO _vertexBufferObject;
        private readonly EBO _elementBufferObject;
        private readonly VBO _instanceVertexBufferObject;

        private MeshStr meshStr;
        private bool drawn;

        public Mesh(string path, bool instanced = false)
        {
            ModelImporter _ = new(path, out meshStr);

            _vertexArrayObject = new();
            _vertexBufferObject = new();
            _elementBufferObject = new();
            _instanceVertexBufferObject = instanced ? new() : null!;

            _vertexArrayObject.Enable();

            _vertexBufferObject.Enable();
            _vertexBufferObject.Data(meshStr.vertices);

            _elementBufferObject.Enable();
            _elementBufferObject.Data(meshStr.indices);

            _vertexArrayObject.Set();
            _vertexArrayObject.Disable();

            SEDebug.Log(SEDebugState.Info, $"Created a new mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");
        }

        public void Draw(PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArrayObject.Enable();
            GL.DrawArrays(type, 0, meshStr.indices.Length);

            if (drawn) return;

            SEDebug.Log(SEDebugState.Debug, $"Drawn a Mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");
            drawn = true;
        }

        public void DrawInstanced(Matrix4[] instanceData, PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArrayObject.Enable();
            _instanceVertexBufferObject.Enable();

            int size = instanceData.Length * 64;  // * size of Matrix4 (float)
            GL.BufferData(BufferTarget.ArrayBuffer, size, instanceData.AsSpan(), BufferUsage.StaticDraw);

            for (uint i = 0; i < 4; i++)
            {
                uint loc = 2 + i;
                GL.EnableVertexAttribArray(loc);
                GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 64, (nint)(i * 16));

                if (GLControl.GLVerGEqual(3, 3))
                {
                    GL.VertexAttribDivisor(loc, 1);
                    continue;
                }

                if (GLControl.SupportsExt(GLExtension.ARB_instanced_arrays))
                {
                    GL.ARB.VertexAttribDivisorARB(loc, 1);
                    continue;
                }

                else throw new NotSupportedException("Your GPU does not support the opengl 3.3 driver");
            }
            GL.DrawElementsInstanced(type, meshStr.indices.Length, DrawElementsType.UnsignedInt, 0, instanceData.Length);
            // GL.MultiDrawElementsIndirect()  // 4.3

            if (drawn) return;
            
            SEDebug.Log(SEDebugState.Debug, $"Drawn an instanced Mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");
            drawn = true;
        }

        public void Destroy() => Dispose();
        public override void OnExit() => Dispose();
        
        public void Dispose()
        {
            SEDebug.Log(SEDebugState.Info, $"Disposing Mesh {_vertexArrayObject} {_vertexBufferObject} {_elementBufferObject} {_instanceVertexBufferObject}");

            _instanceVertexBufferObject?.Destroy();
            _vertexBufferObject?.Destroy();
            _vertexArrayObject?.Destroy();
            _elementBufferObject?.Destroy();

            drawn = false;
        }
    }
}