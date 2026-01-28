using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace SteelEngine.Core
{
    public readonly struct UniformLayout140
    {
        internal readonly int size;
        internal readonly byte[] data;

        internal readonly int blockBinding;
        internal readonly string blockName;

        public UniformLayout140(string bindingName, int bindingPoint, int value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, uint value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, float value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, double value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector2i value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector2 value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector2d value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector3i value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[32];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector3 value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[32];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector3d value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[32];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector4i value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector4 value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Vector4d value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[32];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Matrix2 value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[16];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Matrix2d value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[32];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Matrix3 value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[64];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Matrix3d value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[128];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Matrix4 value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[64];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }

        public UniformLayout140(string bindingName, int bindingPoint, Matrix4d value)
        {
            blockName = bindingName;
            blockBinding = bindingPoint;
            
            data = new byte[128];
            MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref value, 1)).CopyTo(data.AsSpan());
        }
    }
}