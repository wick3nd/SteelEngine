using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0079, CA1822, CA1816
namespace SteelEngine.Core.Buffers
{
    public class ElementBuffer : IBufferObject
    {
        private int m_ElementBuffer;
        private static int _currentBound;
        private readonly string? _debugName;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ElementBuffer()
        {
            GL.GenBuffers(1, ref m_ElementBuffer);
            if (m_ElementBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a EBO", true);
            SEDebug.Log(SEDebugState.Debug, $"Created a new EBO {m_ElementBuffer}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ElementBuffer(string debugName)
        {
            GL.GenBuffers(1, ref m_ElementBuffer);
            if (m_ElementBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a EBO \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Debug, $"Created a new EBO \"{debugName}\"");
        }

        public override string ToString() => _debugName  ?? $"{m_ElementBuffer}";
        public int GetHandle() => m_ElementBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Data(int[] data) => GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(int), data, BufferUsage.StaticDraw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Data(uint[] data) => GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data, BufferUsage.StaticDraw);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_ElementBuffer)
            {
                _currentBound = m_ElementBuffer;
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_ElementBuffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_ElementBuffer && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_ElementBuffer != 0)
            {
                GL.DeleteBuffers(1, ref m_ElementBuffer);

                _currentBound = 0;
                m_ElementBuffer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}