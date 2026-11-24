using OpenTK.Graphics.OpenGL;
using SteelEngine.Elements.Interfaces;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CA1816
namespace SteelEngine.Core.Buffers
{
    public class UniformBuffer : IBufferObject, IEngineDisposable
    {
        private int _UniformBuffer;
        private static int _currentBound;
        private readonly string? _debugName;

        private int _size;
        private BufferBuilder[] _builderCopy;
        private byte[] _internalBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniformBuffer(string debugName, int size)
        {
            GL.GenBuffers(1, ref _UniformBuffer);  // GL 1.5
            if (_UniformBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a UBO \"{debugName}\"", true);

            _debugName = debugName;
            _size = size;

            Init();
            SEDebug.Log(SEDebugState.Debug, $"Created a new UBO \"{debugName}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniformBuffer(string debugName, BufferBuilder[] dataTypes)
        {
            GL.GenBuffers(1, ref _UniformBuffer);  // GL 1.5
            if (_UniformBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a UBO \"{debugName}\"", true);

            _debugName = debugName;
            PrepareInternalBuffer(dataTypes);

            Init();
            SEDebug.Log(SEDebugState.Debug, $"Created a new UBO \"{debugName}\"");
        }

        void PrepareInternalBuffer(BufferBuilder[] dataTypes)
        {
            for (int i = 0; i < dataTypes.Length; i++) _size += dataTypes[i].size;
            _builderCopy = dataTypes;
            _internalBuffer = new byte[_size];
        }

        int updatedVariables = 0;
        public void Update<T>(T variable) where T: unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref variable, 1));
            bytes.CopyTo(_internalBuffer.AsSpan(updatedVariables * _builderCopy[updatedVariables].size));
            
            if (++updatedVariables == _builderCopy.Length)
            {
                updatedVariables = 0;
                Data(ref _internalBuffer);
                return;
            }
        }

       // No booleans are present in GLSL
        public void Update(bool variable)
        {
            Update(variable ? 1 : 0);
        }

       // Binds the data to the shaders binding point
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferBase(Shader shader, string blockName)
        {
            int programHandle = shader.GetHandle();
            uint binding = GL.GetUniformBlockIndex(programHandle, blockName);

            GL.UniformBlockBinding(programHandle, binding, 0);
            GL.BindBufferBase(BufferTarget.UniformBuffer, binding, _UniformBuffer);
        }

       // 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferRange(uint binding, int offset, int size) => GL.BindBufferRange(BufferTarget.UniformBuffer, binding, _UniformBuffer, offset, size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init()
        {
            if (_currentBound != _UniformBuffer)
            {
                Enable();
                _currentBound = _UniformBuffer;
            }
            GL.BufferData(BufferTarget.UniformBuffer, _size, IntPtr.Zero, BufferUsage.DynamicDraw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init(ref byte[] data)
        {
            if (_currentBound != _UniformBuffer)
            {
                Enable();
                _currentBound = _UniformBuffer;
            }
            GL.BufferData(BufferTarget.UniformBuffer, _size, data, BufferUsage.DynamicDraw);
        }

       // Data update
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Data(ref byte[] data)
        {
            if (_currentBound != _UniformBuffer)
            {
                Enable();
                _currentBound = _UniformBuffer;
            }
            GL.BufferSubData(BufferTarget.UniformBuffer, 0, data.Length, data);
        }

        public override string ToString() => _debugName ?? $"{_UniformBuffer}";
        public int GetHandle() => _UniformBuffer;

       // Binds the buffer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != _UniformBuffer)
            {
                _currentBound = _UniformBuffer;
                GL.BindBuffer(BufferTarget.UniformBuffer, _UniformBuffer);
            }
        }

       // Unbinds the buffer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == _UniformBuffer && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            }
        }

        // Deletes the buffer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (_UniformBuffer != 0)
            {
                GL.DeleteBuffers(1, ref _UniformBuffer);

                _currentBound = 0;
                _UniformBuffer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}