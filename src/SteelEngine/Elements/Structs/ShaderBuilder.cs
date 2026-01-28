using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace SteelEngine.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderBuilder(ShaderHint type, string source)
    {
        public readonly ShaderType ShaderType = (ShaderType)type;
        public readonly string ShaderSource = source;
        internal int shaderHandle;
    }
}