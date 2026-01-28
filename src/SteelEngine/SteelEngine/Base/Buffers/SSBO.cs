using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.InteropServices;

namespace SteelEngine.Base.Buffers
{
#pragma warning disable IDE0079, CA1822
    internal class SSBO
    {
        private readonly int _Handle;
        private readonly bool _arb;
        private readonly bool _ext;

        internal SSBO()
        {
            if (!( GLControl.SupportsExt(GLExtension.ARB_direct_state_access, out _arb) || GLControl.SupportsExt(GLExtension.EXT_direct_state_access, out _ext) || GLControl.GLVerGEqual(4, 5) ))
            {
#if DEBUG
                SEDebug.Log(SEDebugState.Info, $"SSBO is not supported on this GPU");
#endif
                return;
            }

            GL.GenBuffers(1, ref _Handle);

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create a SSBO.");
                throw new Exception($"Failed to create a SSBO.");
            }
        }

        public override string ToString() => $"{_Handle}";
        internal int GetHandle() => _Handle;
        internal void StaticData<T>(T[] data) where T : unmanaged
        {
            if (_ext) GL.EXT.NamedBufferDataEXT(_Handle, data.Length * Marshal.SizeOf<T>(), data.AsSpan(), BufferUsage.StaticDraw); // EXT needed
            else if (_arb || GLControl.GLVerGEqual(4, 5)) GL.NamedBufferData(_Handle, data.Length * Marshal.SizeOf<T>(), data.AsSpan(), BufferUsage.StaticDraw); // ARB 4.5 or GL 4.5 needed
        }
        internal void DynamicData<T>(T[] data, int byteOffset = 0) where T : unmanaged
        {
            if (_ext) GL.EXT.NamedBufferSubDataEXT(_Handle, byteOffset, data.Length * Marshal.SizeOf<T>(), data.AsSpan());  // EXT needed
            else if (_arb || GLControl.GLVerGEqual(4, 5)) GL.NamedBufferSubData(_Handle, byteOffset, data.Length * Marshal.SizeOf<T>(), data.AsSpan());  // ARB 4.5 or GL 4.5 needed
        }
        internal void Enable() => GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _Handle);
        internal void BindBase(uint bindingIndex) => GL.BindBufferBase(BufferTarget.ShaderStorageBuffer, bindingIndex, _Handle);
        internal void Disable() => GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        internal void Destroy() => GL.DeleteBuffer(_Handle);
    }
}