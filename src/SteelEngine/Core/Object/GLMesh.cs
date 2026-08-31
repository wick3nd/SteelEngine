using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.AssetLoader;
using SteelEngine.Core.Buffers;
using SteelEngine.Elements.Interfaces;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public class GLMesh //: IEngineDisposable
    {
        readonly string _name;
        bool _drawn;
        readonly GLTFMeshLoader _meshLoader;

        readonly VertexArray[] _vertexArrayObject;
        readonly VertexBuffer[] _vertexBufferObject;
        readonly ElementBuffer[] _elementBufferObject;
        readonly VertexBuffer[] _instanceVertexBufferObject;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GLMesh(string path, string debugName = "")
        {
            _meshLoader = new(path, new() {  });
            _name = debugName == "" ? Path.GetRelativePath(Environment.CurrentDirectory, path) : debugName;

            int submeshCount = _meshLoader.mesh.submeshes.Length;
            _vertexArrayObject = new VertexArray[submeshCount];
            _vertexBufferObject = new VertexBuffer[submeshCount];
            _elementBufferObject = new ElementBuffer[submeshCount];
            _instanceVertexBufferObject = new VertexBuffer[submeshCount];
            
            for (int i = 0; i < submeshCount; i++)
            {
                _vertexArrayObject[i] = new();
                _vertexBufferObject[i] = new(_name);
                _elementBufferObject[i] = new(_name);
                _instanceVertexBufferObject[i] = new(_name);

                _vertexArrayObject[i].Enable();

                _vertexBufferObject[i].Enable();
                _vertexBufferObject[i].Data(_meshLoader.mesh.submeshes[i].vertices);

                _elementBufferObject[i].Enable();
                _elementBufferObject[i].Data(_meshLoader.mesh.submeshes[i].indices);

                _vertexArrayObject[i].Set(_meshLoader.mesh.submeshes[i].primitiveFlags);
            }

            SEDebug.Log(SEDebugState.Info, $"Created a new Mesh \"{this}\"");
        }

       // THIS DOES NOT IN WORK IN PURE 3.2
       // CHANGE IT
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(Matrix4[] instanceData)
        {
            nint size = instanceData.Length * 64;  // multiplied by size of Matrix4

            for (int j = 0; j < _vertexBufferObject.Length; j++)
            {
                _vertexArrayObject[j].Enable();
                _instanceVertexBufferObject[j].Enable();

                for (uint i = 0; i < 4; i++)
                {
                    uint loc = (uint)ShaderLayoutLocation.iModel + i;
                    GL.EnableVertexAttribArray(loc);  // GL 2.0
                    GL.VertexAttribPointer(loc, 4, VertexAttribPointerType.Float, false, 64, (nint)(i * 16));  // GL 2.0

                    if (GLControl.GLVerGEqual(3, 3))
                    {
                        GL.VertexAttribDivisor(loc, 1);  // GL 3.3!!!
                        continue;
                    }

                    else if (GLControl.SupportsExt(GLExtension.ARB_instanced_arrays))
                    {
                        GL.ARB.VertexAttribDivisorARB(loc, 1);  // GL_ARB_instanced_arrays
                        continue;
                    }

                    else throw new NotSupportedException($"Your GPU does not have a {nameof(GLExtension.ARB_instanced_arrays)} extension or doesn't have a opengl 3.3 driver");
                }

                GL.BufferData(BufferTarget.ArrayBuffer, size, instanceData.AsSpan(), BufferUsage.DynamicDraw);
                GL.DrawElementsInstanced(_meshLoader.mesh.submeshes[j].topologyType, _elementBufferObject[j].Size(), DrawElementsType.UnsignedInt, 0, instanceData.Length);  // GL 3.1
            }

            if (!_drawn)
            {
                SEDebug.Log(SEDebugState.Debug, $"Drawn mesh \"{this}\"[{_meshLoader.mesh.submeshes.Length}] in {instanceData.Length} location(s)");
                _drawn = true;
            } 
        }

        public override string ToString() => _name;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void Destroy()  // wtf?
        //{
        //    SEDebug.Log(SEDebugState.Info, $"Disposing Mesh \"{this}\"");

        //    for (int i = 0; i < _vertexArrayObject!.Length; i++)
        //    {
        //        _instanceVertexBufferObject?[i].Destroy();
        //        _vertexBufferObject?[i].Destroy();
        //        _vertexArrayObject?[i].Destroy();
        //        _elementBufferObject?[i].Destroy();
        //    }

        //    _drawn = false;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void Dispose() => Destroy();
    }
}