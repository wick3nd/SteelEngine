using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public class ShaderPipeline : IDisposable
    {
        int m_ShaderPipeline;
        static int _currentBound;
        readonly string? _debugName;

        Dictionary<string, ShaderBuilder[]> presetMap = [];
        Dictionary<string, int> shaderMap = [];

        public ShaderPipeline(string debugName, PipelineBuilder[] pipelinePresets)
        {
            GL.GenProgramPipelines(1, ref m_ShaderPipeline);
            if (m_ShaderPipeline == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create the Shader", true);

            presetMap = new(pipelinePresets.Length);
            for (int i = 0; i < presetMap.Count; i++)
            {
                presetMap.Add(pipelinePresets[i].presetName, pipelinePresets[i].shaders);
                for (int j = 0; j < pipelinePresets[i].presetName.Length; j++)
                {
                    string shaderCode = pipelinePresets[i].shaders[j].ShaderCode;
                    CreateSeparableShader(pipelinePresets[i].shaders[j].ShaderType, shaderCode, out int handle);
                    
                    // ??????????????
                    if (pipelinePresets[i].shaders[j].ShaderPath != null) shaderMap.TryAdd(pipelinePresets[i].shaders[j].ShaderPath!, handle);
                    else shaderMap.TryAdd(shaderCode.GetHashCode().ToString(), handle);  // it doesnt meet its purpose
                    // ??????????????
                }
            }

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Info, $"Created shader handle \"{debugName}\"");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CreateSeparableShader(ShaderType type, string source, out int handle)
        {
            handle = GL.CreateProgram();
            if (handle == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create the Shader", true);
            GL.ProgramParameteri(handle, ProgramParameterPName.ProgramSeparable, 1);

            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);
            GL.AttachShader(handle, shader);

           // GL.BindFragDataLocation(handle, 0, "FragColor");

            GL.LinkProgram(handle);

            #if DEBUG
            CheckLinkStatus();
            #endif

            GL.DetachShader(handle, shader);
            GL.DeleteShader(shader);

           // ResourceManager.CacheShader("", new(handle, type));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CheckLinkStatus()
        {
            GL.GetProgrami(m_ShaderPipeline, ProgramProperty.LinkStatus, out int shaderLinkStatus);
            if (shaderLinkStatus == 0)
            {
                GL.GetProgramInfoLog(m_ShaderPipeline, out string infoLog);
                SEDebug.Log(SEDebugState.Error, $"Shader linking error: {infoLog}");
            }
        }

        public override string ToString() => _debugName ?? $"{m_ShaderPipeline}";
        public int GetHandle() => m_ShaderPipeline;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_ShaderPipeline)
            {
                _currentBound = m_ShaderPipeline;
                GL.BindProgramPipeline(m_ShaderPipeline);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_ShaderPipeline && _currentBound != 0)
            {
                _currentBound = 0;
                GL.UseProgram(0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_ShaderPipeline != 0)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing shader pipeline handle {m_ShaderPipeline}");
                GL.DeleteProgramPipeline(m_ShaderPipeline);

                _currentBound = 0;
                m_ShaderPipeline = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}
