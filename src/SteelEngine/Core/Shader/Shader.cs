using OpenTK.Graphics.OpenGL;
using SteelEngine.Core.EngineBehaviour;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

#pragma warning disable CA1816, CA1822
namespace SteelEngine.Core
{
    public partial class Shader : IDisposable
    {
        //===============================================
        //
        //              ADD
        //              A
        //              FALLBACK
        //              SHADER
        //              OPTION
        //
        // also add shader caching,
        // async compilation(if i can),
        // separable programs,      OGL 4.1
        // pipeline objects,        OGL 4.1
        // hot reload without relink and loading shaders from a file(add a bool if a shader is from a file/sep)
        //
        //===============================================

        private int m_Shader;
        private static int _currentBound;
        private readonly string? _debugName;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Shader(string debugName, ShaderBuilder[] shaders)
        {
            m_Shader = GL.CreateProgram();
            if (m_Shader == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create the Shader", true);

            int shadersCount = shaders.Length;
            for (int i = 0; i < shadersCount; i++)
            {
                ref var shader = ref shaders[i];
                CompileShader(shader.ShaderType, shader.ShaderCode, out shader.shaderHandle);
            }

            for (int i = 0; i < shadersCount; i++) GL.AttachShader(m_Shader, shaders[i].shaderHandle);

            GL.LinkProgram(m_Shader);
            for (int i = 0; i < shadersCount; i++) DeleteShader(shaders[i].shaderHandle);

            #if DEBUG
            CheckLinkStatus();
            #endif
            
            _debugName = debugName;
            SEDebug.Log(SEDebugState.Info, $"Created shader handle \"{debugName}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CompileShader(ShaderType type, string source, out int handle)
        {
            handle = GL.CreateShader(type);
            GL.ShaderSource(handle, source);
            GL.CompileShader(handle);

            #if DEBUG
            GL.GetShaderi(handle, ShaderParameterName.CompileStatus, out int compileStatus);
            if (compileStatus == 0)
            {
                GL.GetShaderInfoLog(handle, out string infoLog);
                SEDebug.Log(SEDebugState.Error, $"{type.ToString().Replace("Shader", " shader")} error: {infoLog}");
            }
            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CheckLinkStatus()
        {
            GL.GetProgrami(m_Shader, ProgramProperty.LinkStatus, out int shaderLinkStatus);
            if (shaderLinkStatus == 0)
            {
                GL.GetProgramInfoLog(m_Shader, out string infoLog);
                SEDebug.Log(SEDebugState.Error, $"Shader linking error: {infoLog}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void DeleteShader(int shaderHandle)
        {
            GL.DetachShader(m_Shader, shaderHandle);
            GL.DeleteShader(shaderHandle);
        }


        public override string ToString() => _debugName ?? $"{m_Shader}";
        public int GetHandle() => m_Shader;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_Shader)
            {
                _currentBound = m_Shader;
                GL.UseProgram(m_Shader);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_Shader && _currentBound != 0)
            {
                _currentBound = 0;
                GL.UseProgram(0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_Shader != 0)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing shader handle {m_Shader}");
                GL.DeleteProgram(m_Shader);

                _currentBound = 0;
                m_Shader = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}