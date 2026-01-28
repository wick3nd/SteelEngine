using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

#pragma warning disable CA1816
namespace SteelEngine.Core.Buffers
{
    public class UniformBuffer : IBufferObject
    {
        internal static byte[] sharedUBOBuffer = new byte[GLControl.MaxUniformBlockSize];
        internal byte[] _internalUBOBuffer = [];

        private int m_UniformBuffer;
        private static int _currentBound;
        private readonly string? _debugName;

        private int _size;

#pragma warning disable CS0649, IDE0044
        private bool _isFixedSize;
#pragma warning restore CS0649, IDE0044

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniformBuffer(string debugName, int size)
        {
            if (GLControl.GLVerGEqual(4, 5)) GL.CreateBuffers(1, ref m_UniformBuffer);
            else if (GLControl.SupportsExt(GLExtension.ARB_direct_state_access)) GL.ARB.CreateBuffers(1, ref m_UniformBuffer);
            else GL.GenBuffers(1, ref m_UniformBuffer);

            if (m_UniformBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a UBO \"{debugName}\"", true);

            _debugName = debugName;
            _size = size;

            Init();
            SEDebug.Log(SEDebugState.Debug, $"Created a new UBO \"{debugName}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniformBuffer(int size)
        {
            if (GLControl.GLVerGEqual(4, 5)) GL.CreateBuffers(1, ref m_UniformBuffer);
            else if (GLControl.SupportsExt(GLExtension.ARB_direct_state_access)) GL.ARB.CreateBuffers(1, ref m_UniformBuffer);
            else GL.GenBuffers(1, ref m_UniformBuffer);

            if (m_UniformBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a UBO", true);

            _size = size;
            Init();
            SEDebug.Log(SEDebugState.Debug, $"Created a new UBO {m_UniformBuffer}");
        }

       // private static int ValueToStd140<T>() where T : unmanaged => (Unsafe.SizeOf<T>() + 15) & ~15;

       // Binds the data to the shaders binding point
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferBase(uint binding) => GL.BindBufferBase(BufferTarget.UniformBuffer, binding, m_UniformBuffer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferBase(Shader shader, string blockName)
        {
            int programHandle = shader.GetHandle();
            uint binding = GL.GetUniformBlockIndex(programHandle, blockName);
            GL.UniformBlockBinding(programHandle, binding, 0);
            BufferBase(binding);
        }

       // 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferRange(uint binding, int offset, int size) => GL.BindBufferRange(BufferTarget.UniformBuffer, binding, m_UniformBuffer, offset, size);

       // Data init
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InitStorage(BufferStorageMask bufferUsage = BufferStorageMask.MapWriteBit | BufferStorageMask.MapPersistentBit | BufferStorageMask.MapCoherentBit | BufferStorageMask.DynamicStorageBit)
        {
            if (GLControl.GLVerEqual(4, 4))
            {
                GL.BufferStorage(BufferStorageTarget.UniformBuffer, _size, IntPtr.Zero, bufferUsage);
                return;
            }
            if (GLControl.GLVerGEqual(4, 5))
            {
                GL.NamedBufferStorage(m_UniformBuffer, _size, IntPtr.Zero, bufferUsage);
                return;
            }
            if (GLControl.SupportsExt(GLExtension.ARB_direct_state_access))
            {
                GL.ARB.NamedBufferStorage(m_UniformBuffer, _size, IntPtr.Zero, bufferUsage);
                return;
            }
            
            if (_currentBound != m_UniformBuffer)
            {
                Enable();
                _currentBound = m_UniformBuffer;
            }
            GL.BufferStorage(BufferStorageTarget.UniformBuffer, _size, IntPtr.Zero, bufferUsage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InitStorage(ref byte[] data, BufferStorageMask bufferUsage = BufferStorageMask.MapWriteBit | BufferStorageMask.MapPersistentBit | BufferStorageMask.MapCoherentBit | BufferStorageMask.DynamicStorageBit)
        {
            if (GLControl.GLVerEqual(4, 4))
            {
                GL.BufferStorage(BufferStorageTarget.UniformBuffer, _size, data, bufferUsage);
                return;
            }
            if (GLControl.GLVerGEqual(4, 5))
            {
                GL.NamedBufferStorage(m_UniformBuffer, _size, data, bufferUsage);
                return;
            }
            if (GLControl.SupportsExt(GLExtension.ARB_direct_state_access))
            {
                GL.ARB.NamedBufferStorage(m_UniformBuffer, _size, data, bufferUsage);
                return;
            }

            if (_currentBound != m_UniformBuffer)
            {
                Enable();
                _currentBound = m_UniformBuffer;
            }
            GL.BufferStorage(BufferStorageTarget.UniformBuffer, _size, data, bufferUsage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init()
        {
            if (GLControl.GLVerEqual(4, 4))
            {
                GL.NamedBufferData(m_UniformBuffer, _size, IntPtr.Zero, BufferUsage.DynamicDraw);
                return;
            }
            if (GLControl.GLVerGEqual(4, 5))
            {
                GL.NamedBufferData(m_UniformBuffer, _size, IntPtr.Zero, BufferUsage.DynamicDraw);
                return;
            }
            if (GLControl.SupportsExt(GLExtension.ARB_direct_state_access))
            {
                GL.ARB.NamedBufferData(m_UniformBuffer, _size, IntPtr.Zero, BufferUsage.DynamicDraw);
                return;
            }
            
            if (_currentBound != m_UniformBuffer)
            {
                Enable();
                _currentBound = m_UniformBuffer;
            }
            GL.BufferData(BufferTarget.UniformBuffer, _size, IntPtr.Zero, BufferUsage.DynamicDraw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init(ref byte[] data)
        {
            if (GLControl.GLVerEqual(4, 4))
            {
                GL.NamedBufferData(m_UniformBuffer, _size, data, BufferUsage.DynamicDraw);
                return;
            }
            if (GLControl.GLVerGEqual(4, 5))
            {
                GL.NamedBufferData(m_UniformBuffer, _size, data, BufferUsage.DynamicDraw);
                return;
            }
            if (GLControl.SupportsExt(GLExtension.ARB_direct_state_access))
            {
                GL.ARB.NamedBufferData(m_UniformBuffer, _size, data, BufferUsage.DynamicDraw);
                return;
            }
            
            if (_currentBound != m_UniformBuffer)
            {
                Enable();
                _currentBound = m_UniformBuffer;
            }
            GL.BufferData(BufferTarget.UniformBuffer, _size, data, BufferUsage.DynamicDraw);
        }

       // Data update
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Data(ref byte[] data)
        {
            if (GLControl.GLVerGEqual(4, 5))
            {
                GL.NamedBufferSubData(m_UniformBuffer, 0, data.Length, data);
                return;
            }
            if (GLControl.SupportsExt(GLExtension.ARB_direct_state_access))
            {
                GL.ARB.NamedBufferSubData(m_UniformBuffer, 0, data.Length, data);
                return;
            }

            if (_currentBound != m_UniformBuffer)
            {
                Enable();
                _currentBound = m_UniformBuffer;
            }
            GL.BufferSubData(BufferTarget.UniformBuffer, 0, data.Length, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(int size)
        {
            if (_size != size && (GLControl.GLVerGEqual(4, 4) || GLControl.SupportsExt(GLExtension.ARB_direct_state_access)))  // Refine it later
            {
                _size = size;
                Init();
            }
        }

        public override string ToString() => _debugName ?? $"{m_UniformBuffer}";
        public int GetHandle() => m_UniformBuffer;

       // Binds the buffer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_UniformBuffer)
            {
                _currentBound = m_UniformBuffer;
                GL.BindBuffer(BufferTarget.UniformBuffer, m_UniformBuffer);
            }
        }

       // Unbinds the buffer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_UniformBuffer && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            }
        }

        // Deletes the buffer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_UniformBuffer != 0)
            {
                GL.DeleteBuffers(1, ref m_UniformBuffer);

                _currentBound = 0;
                m_UniformBuffer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}