using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CA1816, IDE0079, CA1822
namespace SteelEngine.Core.Buffers
{
    public class VertexBuffer : IBufferObject
    {
        private int m_VertexBuffer;
        private static int _currentBound;
        private readonly string? _debugName;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexBuffer()
        {
            GL.GenBuffers(1, ref m_VertexBuffer);
            if (m_VertexBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a VBO", true);
            SEDebug.Log(SEDebugState.Debug, $"Created a new VBO {m_VertexBuffer}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexBuffer(string debugName)
        {
            GL.GenBuffers(1, ref m_VertexBuffer);
            if (m_VertexBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a VBO \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Debug, $"Created a new VBO \"{debugName}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Data<T>(T[] data) where T : unmanaged
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Marshal.SizeOf<T>(), data.AsSpan(), BufferUsage.StaticDraw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateData<T>(T[] data, int offset = 0) where T : unmanaged
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VertexBuffer);
            GL.BufferSubData(BufferTarget.ArrayBuffer, offset, data.Length * Marshal.SizeOf<T>(), data.AsSpan());
        }

        public override string ToString() => _debugName ?? $"{m_VertexBuffer}";
        public int GetHandle() => m_VertexBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_VertexBuffer)
            {
                _currentBound = m_VertexBuffer;
                GL.BindBuffer(BufferTarget.ArrayBuffer, m_VertexBuffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_VertexBuffer && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_VertexBuffer != 0)
            {
                GL.DeleteBuffer(m_VertexBuffer);

                _currentBound = 0;
                m_VertexBuffer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}
