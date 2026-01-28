using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Core.Buffers;
using SteelEngine.EngineBase.Structs;
using SteelEngine.Objects;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    internal class Mesh : IDisposable
    {
        private readonly VertexArray _vertexArrayObject;
        private readonly VertexBuffer _vertexBufferObject;
        private readonly ElementBuffer _elementBufferObject;
        private readonly VertexBuffer _instanceVertexBufferObject;

        private readonly string _name;

        private MeshStr meshStr;
        private bool drawn;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Mesh(string path, bool instanced = false)
        {
            ModelImporter _ = new(path, out meshStr);
            _name = Path.GetFileName(path);

            _vertexArrayObject = new(_name + " VAO");
            _vertexBufferObject = new(_name + " VBO");
            _elementBufferObject = new(_name + " EBO");
            _instanceVertexBufferObject = instanced ? new(_name + " iVBO") : null!;

            _vertexArrayObject.Enable();

            _vertexBufferObject.Enable();
            _vertexBufferObject.Data(meshStr.vertices);

            _elementBufferObject.Enable();
            _elementBufferObject.Data(meshStr.indices);

            _vertexArrayObject.Set();
            _vertexArrayObject.Disable();

            SEDebug.Log(SEDebugState.Info, $"Created a new Mesh \"{_name}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArrayObject.Enable();
            GL.DrawArrays(type, 0, meshStr.indices.Length);

            if (drawn) return;

            SEDebug.Log(SEDebugState.Debug, $"Drawn a Mesh \"{_name}\"");
            drawn = true;
        }

       // THIS DOES NOT IN WORK IN PURE 3.2
       // REDO IT
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawInstanced(Matrix4[] instanceData, PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArrayObject.Enable();
            _instanceVertexBufferObject.Enable();

            nint size = instanceData.Length * 64;  // * size of Matrix4 (float)
            GL.BufferData(BufferTarget.ArrayBuffer, size, instanceData.AsSpan(), BufferUsage.StaticDraw);

            for (uint i = 0; i < 4; i++)
            {
                uint loc = (uint)ShaderLayoutLocation.iModel + i;
                GL.EnableVertexAttribArray(loc);
                GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 64, (nint)(i * 16));

                if (GLControl.GLVerGEqual(3, 3))
                {
                    GL.VertexAttribDivisor(loc, 1);
                    continue;
                }

                else if (GLControl.SupportsExt(GLExtension.ARB_instanced_arrays))
                {
                    GL.ARB.VertexAttribDivisorARB(loc, 1);
                    continue;
                }

                else throw new NotSupportedException("Your GPU does not have a ARB_instanced_arrays extension or doesn't have a opengl 3.3 driver");
            }
            GL.DrawElementsInstanced(type, meshStr.indices.Length, DrawElementsType.UnsignedInt, 0, instanceData.Length);
            // GL.MultiDrawElementsIndirect()  // 4.3

            if (drawn) return;
            
            SEDebug.Log(SEDebugState.Debug, $"Drawn an instanced Mesh \"{_name}\"");
            drawn = true;
        }

        public override string ToString() => _name;
        public int[] GetHandles() => [_vertexArrayObject.GetHandle(), _vertexBufferObject.GetHandle(), _elementBufferObject.GetHandle(), _instanceVertexBufferObject.GetHandle()];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            SEDebug.Log(SEDebugState.Info, $"Disposing Mesh \"{_name}\"");

            _instanceVertexBufferObject?.Destroy();
            _vertexBufferObject?.Destroy();
            _vertexArrayObject?.Destroy();
            _elementBufferObject?.Destroy();

            drawn = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}