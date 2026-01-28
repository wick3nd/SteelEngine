using OpenTK.Graphics.OpenGL;
using SteelEngine.EngineBase.EngineBehaviour;
using SteelEngine.Utils;

namespace SteelEngine.Base
{
#pragma warning disable IDE0079, CA1822
    internal partial class Shader : EngineScript, IDisposable
    {
        private readonly int _Handle;

        public Shader(string fragmentSource = defaultFrag, string vertexSource = defaultVert)
        {
           // Vertex compilation and validation
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);

            GL.GetShaderi(vertexShader, ShaderParameterName.CompileStatus, out int vertCompileStatus);
            if (vertCompileStatus == 0)
            {
                GL.GetShaderInfoLog(vertexShader, out string infoLog);
                SEDebug.Log(SEDebugState.Error, $"Vertex shader error: {infoLog}");
            }

           // Fragment compilation and validation
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);

            GL.GetShaderi(fragmentShader, ShaderParameterName.CompileStatus, out int fragCompileStatus);
            if (fragCompileStatus == 0)
            {
                GL.GetShaderInfoLog(fragmentShader, out string infoLog);
                SEDebug.Log(SEDebugState.Error, $"Fragment shader error: {infoLog}");
            }

            //============================================================
            //
            //              ADD
            //              A
            //              FALLBACK
            //              SHADER
            //              OPTION
            //              JUST
            //              IN
            //              CASE
            //              (and make it avaible to be changed)
            //
            //============================================================

            _Handle = GL.CreateProgram();

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create the Shader.");
                throw new Exception($"Failed to create a VBO.");
            }

            GL.AttachShader(_Handle, vertexShader);
            GL.AttachShader(_Handle, fragmentShader);
            GL.LinkProgram(_Handle);

            GL.GetProgrami(_Handle, ProgramProperty.LinkStatus, out int shaderCompileStatus);
            if (shaderCompileStatus == 0)
            {
                GL.GetProgramInfoLog(_Handle, out string infoLog);
                SEDebug.Log(SEDebugState.Error, $"Handle linking error: {infoLog}");
            }

            SEDebug.Log(SEDebugState.Info, $"Created shader handle {_Handle}");

            GL.DetachShader(_Handle, vertexShader);
            GL.DetachShader(_Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            Disable();
        }

        public int GetHandle() => _Handle;
        public void Enable() => GL.UseProgram(_Handle);
        public void Disable() => GL.UseProgram(0);
        public void Destroy()
        {
            SEDebug.Log(SEDebugState.Info, $"Disposing shader handle {_Handle}");
            GL.DeleteProgram(_Handle);
        }
        public override void OnExit() => Dispose();
        public void Dispose() => Destroy();
    }
}