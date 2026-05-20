using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace SteelEngine.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderBuilder
    {
        public readonly ShaderType ShaderType;
        public readonly string ShaderCode;
        public readonly string? ShaderPath;
        internal int shaderHandle;

        public ShaderBuilder(ShaderHint type, string source)
        {
            ShaderType = (ShaderType)type;

            if (source[0] != '#') // that'll work for now
            {
                ShaderCode = File.ReadAllText(source);
                ShaderPath = source;
            }
            else ShaderCode = source;
        }

        public ShaderBuilder(ShaderHint type)
        {
            ShaderType = (ShaderType)type;
            ShaderCode = type switch
            {
                ShaderHint.FragmentShader => Shader.default_frag,
                ShaderHint.VertexShader => Shader.default_vert,
                _ => throw new NotImplementedException(),
            };
        }
    }
}