using OpenTK.Graphics.OpenGL;
using SteelEngine.Core.EngineBehaviour;
using SteelEngine.Elements.Interfaces;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core.Buffers
{
    public class VertexArray : IBufferObject, IEngineDisposable
    {
        internal bool generated = false;

        int m_VertexArray;
        static int _currentBound;

        public MeshPrimitives PrimitiveFlags { get; internal set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VertexArray()
        {
            GL.GenVertexArrays(1, ref m_VertexArray);  // GL 3.0
            if (m_VertexArray == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a VAO", true);

            SEDebug.Log(SEDebugState.Debug, $"Created a new VAO[{m_VertexArray}]");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(MeshPrimitives flags)
        {
            PrimitiveFlags = flags;
            int stride = 0;
            int offset = 0;

            if ((PrimitiveFlags & MeshPrimitives.Position) != 0) stride += 3;
            if ((PrimitiveFlags & MeshPrimitives.TexCoord) != 0) stride += 2;
            if ((PrimitiveFlags & MeshPrimitives.Normal) != 0) stride += 3;
            if ((PrimitiveFlags & MeshPrimitives.Color) != 0) stride += 4;
            stride *= sizeof(float);

            if ((PrimitiveFlags & MeshPrimitives.Position) != 0)
            {
                GL.VertexAttribPointer((int)ShaderLayoutLocation.aPosition, 3, VertexAttribPointerType.Float, false, stride, offset);  // GL 2.0
                GL.EnableVertexAttribArray((int)ShaderLayoutLocation.aPosition);  // GL 2.0
                offset += 3 * sizeof(float);
            }
           // else GL.DisableVertexAttribArray((int)ShaderLayoutLocation.aPosition);

            if ((PrimitiveFlags & MeshPrimitives.TexCoord) != 0)
            {
                GL.VertexAttribPointer((int)ShaderLayoutLocation.aTexCoord, 2, VertexAttribPointerType.Float, false, stride, offset);  // GL 2.0
                GL.EnableVertexAttribArray((int)ShaderLayoutLocation.aTexCoord);  // GL 2.0
                offset += 2 * sizeof(float);
            }
           // else GL.DisableVertexAttribArray((int)ShaderLayoutLocation.aTexCoord);

            if ((PrimitiveFlags & MeshPrimitives.Normal) != 0)
            {
                GL.VertexAttribPointer((int)ShaderLayoutLocation.aNormal, 3, VertexAttribPointerType.Float, false, stride, offset);  // GL 2.0
                GL.EnableVertexAttribArray((int)ShaderLayoutLocation.aNormal);  // GL 2.0
                offset += 3 * sizeof(float);
            }
           // else GL.DisableVertexAttribArray((int)ShaderLayoutLocation.aNormal);

            if ((PrimitiveFlags & MeshPrimitives.Color) != 0)
            {
                GL.VertexAttribPointer((int)ShaderLayoutLocation.aColor, 4, VertexAttribPointerType.Float, false, stride, offset);  // GL 2.0
                GL.EnableVertexAttribArray((int)ShaderLayoutLocation.aColor);  // GL 2.0
            }
           // else GL.DisableVertexAttribArray((int)ShaderLayoutLocation.aColor);

            SEDebug.Log(SEDebugState.Debug, $"Set the VAO[{m_VertexArray}] to contain [{PrimitiveFlags}]");
        }

        public override string ToString() => $"VAO[{m_VertexArray}] [{PrimitiveFlags}]";
        public int GetHandle() => m_VertexArray;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_VertexArray)
            {
                _currentBound = m_VertexArray;
                GL.BindVertexArray(m_VertexArray);  // GL 3.0
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_VertexArray && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindVertexArray(0);  // GL 3.0
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_VertexArray != 0)
            {
                ResourceManager.RemoveVAO(this);
                GL.DeleteVertexArray(m_VertexArray);  // GL 3.0

                _currentBound = 0;
                m_VertexArray = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}