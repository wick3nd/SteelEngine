using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

#pragma warning disable CA1816, IDE0079, CA1822
namespace SteelEngine.Core.Buffers
{
    public class VertexArray : IBufferObject
    {
        private int m_VertexArray;
        private static int _currentBound;
        private readonly string? _debugName;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexArray()
        {
            GL.GenVertexArrays(1, ref m_VertexArray);
            if (m_VertexArray == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a VAO", true);
            SEDebug.Log(SEDebugState.Debug, $"Created a new VAO {m_VertexArray}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexArray(string debugName)
        {
            GL.GenVertexArrays(1, ref m_VertexArray);
            if (m_VertexArray == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a VAO \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Debug, $"Created a new VAO \"{debugName}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set()
        {
            GL.VertexAttribPointer((int)ShaderLayoutLocation.aPosition, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer((int)ShaderLayoutLocation.aTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.EnableVertexAttribArray((int)ShaderLayoutLocation.aPosition);
            GL.EnableVertexAttribArray((int)ShaderLayoutLocation.aTexCoord);
        }

        public override string ToString() => _debugName ?? $"{m_VertexArray}";
        public int GetHandle() => m_VertexArray;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_VertexArray)
            {
                _currentBound = m_VertexArray;
                GL.BindVertexArray(m_VertexArray);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_VertexArray && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindVertexArray(0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_VertexArray != 0)
            {
                GL.DeleteVertexArray(m_VertexArray);

                _currentBound = 0;
                m_VertexArray = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}
