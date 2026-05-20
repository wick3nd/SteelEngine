using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Core.Buffers;
using SteelEngine.Core.EngineBehaviour;
using SteelEngine.Elements.Interfaces;
using SteelEngine.Objects;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public class Mesh : IEngineDisposable
    {
        readonly VertexArray _vertexArrayObject;
        readonly VertexBuffer _vertexBufferObject;
        readonly ElementBuffer _elementBufferObject;
        readonly VertexBuffer _instanceVertexBufferObject;

        readonly string _name;

        readonly GLTFMeshLoader _mesh;
        bool drawn;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Mesh(string path, string debugName = "")
        {
            _mesh = new(path, out MeshPrimitives flags, new() { });
            _name = debugName == "" ? Path.GetRelativePath(Environment.CurrentDirectory, path) : debugName;
            
            _vertexArrayObject = ResourceManager.TryGetOrCreateVAO(flags);
            _vertexBufferObject = new(_name);
            _elementBufferObject = new(_name);
            _instanceVertexBufferObject = new(_name);

            _vertexArrayObject.Enable();

            _vertexBufferObject.Enable();
            _vertexBufferObject.Data(_mesh.verticeData.ToArray());

            _elementBufferObject.Enable();
            _elementBufferObject.Data(_mesh.indices.ToArray());

            if (!_vertexArrayObject.generated)
            {
                _vertexArrayObject.Set(flags);
                _vertexArrayObject.generated = true;
            }
            
            ResourceManager.CacheVAO(_vertexArrayObject);
            SEDebug.Log(SEDebugState.Info, $"Created a new Mesh \"{this}\"");
        }

       // THIS DOES NOT IN WORK IN PURE 3.2
       // CHANGE IT
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(Matrix4[] instanceData)
        {
            PrimitiveType type = PrimitiveType.Triangles;

            _vertexArrayObject.Enable();
            _instanceVertexBufferObject.Enable();

            nint size = instanceData.Length * 64;  // multiplied by size of Matrix4 (float)
            GL.BufferData(BufferTarget.ArrayBuffer, size, instanceData.AsSpan(), BufferUsage.StaticDraw);

            for (uint i = 0; i < 4; i++)
            {
                uint loc = (uint)ShaderLayoutLocation.iModel + i;
                GL.EnableVertexAttribArray(loc);  // GL 2.0
                GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 64, (nint)(i * 16));  // GL 2.0

                if (GLControl.GLVerGEqual(3, 3))
                {
                    GL.VertexAttribDivisor(loc, 1);  // GL 3.3
                    continue;
                }

                else if (GLControl.SupportsExt(GLExtension.ARB_instanced_arrays))
                {
                    GL.ARB.VertexAttribDivisorARB(loc, 1);  // GL_ARB_instanced_arrays
                    continue;
                }

                else throw new NotSupportedException($"Your GPU does not have a {nameof(GLExtension.ARB_instanced_arrays)} extension or doesn't have a opengl 3.3 driver");
            }
            GL.DrawElementsInstanced(type, _elementBufferObject.Size(), DrawElementsType.UnsignedInt, 0, instanceData.Length);  // GL 3.1
            
            if (!drawn)
            {
                SEDebug.Log(SEDebugState.Debug, $"Drawn mesh \"{this}\" in {instanceData.Length} location(s)");
                drawn = true;
            }
        }

        public override string ToString() => _name;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int GetVAOHandle() => _vertexArrayObject.GetHandle();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int GetVBOHandle() => _vertexBufferObject.GetHandle();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int GetEBOHandle() => _elementBufferObject.GetHandle();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public int GetIVBOHandle() => _instanceVertexBufferObject.GetHandle();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            SEDebug.Log(SEDebugState.Info, $"Disposing Mesh \"{this}\"");

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