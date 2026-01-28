using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CA1816
namespace SteelEngine.Core.Buffers
{
    public class ShaderStorageBuffer : IBufferObject
    {
        private int m_ShaderStorageBuffer;
        private static int _currentBound;
        private readonly string? _debugName;

        private readonly bool _arb;
        private readonly bool _ext;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ShaderStorageBuffer()
        {
            if (!( GLControl.SupportsExt(GLExtension.ARB_direct_state_access, out _arb) || GLControl.SupportsExt(GLExtension.EXT_direct_state_access, out _ext) || GLControl.GLVerGEqual(4, 5) ))
            {
#if DEBUG
                SEDebug.Log(SEDebugState.Info, $"SSBO is not supported on this GPU");
#endif
                return;
            }

            GL.GenBuffers(1, ref m_ShaderStorageBuffer);
            if (m_ShaderStorageBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a SSBO", true);
            SEDebug.Log(SEDebugState.Debug, $"Created a new SSBO {m_ShaderStorageBuffer}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ShaderStorageBuffer(string debugName)
        {
            if (!( GLControl.SupportsExt(GLExtension.ARB_direct_state_access, out _arb) || GLControl.SupportsExt(GLExtension.EXT_direct_state_access, out _ext) || GLControl.GLVerGEqual(4, 5) ))
            {
#if DEBUG
                SEDebug.Log(SEDebugState.Info, $"SSBO is not supported on this GPU");
#endif
                return;
            }

            GL.GenBuffers(1, ref m_ShaderStorageBuffer);
            if (m_ShaderStorageBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a SSBO \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Debug, $"Created a new SSBO \"{debugName}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StaticData<T>(T[] data) where T : unmanaged
        {
            if (_ext) GL.EXT.NamedBufferDataEXT(m_ShaderStorageBuffer, data.Length * Marshal.SizeOf<T>(), data.AsSpan(), BufferUsage.StaticDraw); // EXT needed
            else if (_arb || GLControl.GLVerGEqual(4, 5)) GL.NamedBufferData(m_ShaderStorageBuffer, data.Length * Marshal.SizeOf<T>(), data.AsSpan(), BufferUsage.StaticDraw); // ARB 4.5 or GL 4.5 needed
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DynamicData<T>(T[] data, int byteOffset = 0) where T : unmanaged
        {
            if (_ext) GL.EXT.NamedBufferSubDataEXT(m_ShaderStorageBuffer, byteOffset, data.Length * Marshal.SizeOf<T>(), data.AsSpan());  // EXT needed
            else if (_arb || GLControl.GLVerGEqual(4, 5)) GL.NamedBufferSubData(m_ShaderStorageBuffer, byteOffset, data.Length * Marshal.SizeOf<T>(), data.AsSpan());  // ARB 4.5 or GL 4.5 needed
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBase(uint bindingIndex) => GL.BindBufferBase(BufferTarget.ShaderStorageBuffer, bindingIndex, m_ShaderStorageBuffer);

        public override string ToString() => _debugName ?? $"{m_ShaderStorageBuffer}";
        public int GetHandle() => m_ShaderStorageBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_ShaderStorageBuffer)
            {
                _currentBound = m_ShaderStorageBuffer;
                GL.BindBuffer(BufferTarget.ShaderStorageBuffer, m_ShaderStorageBuffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_ShaderStorageBuffer && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_ShaderStorageBuffer != 0)
            {
                GL.DeleteBuffer(m_ShaderStorageBuffer);

                _currentBound = 0;
                m_ShaderStorageBuffer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}