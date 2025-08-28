using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteelEngine.Utils;

namespace SteelEngine.Base
{
    public class Shader : IDisposable
    {
        private readonly int _Handle;
        private readonly int _VertexShader;
        private readonly int _FragmentShader;

        // Default shaders ==================================================
        public const string defaultFrag = """
            #version 330 core
            in vec2 TexCoord;
            uniform sampler2D texture0;
            uniform sampler2D texture1;
            uniform sampler2D texture2;
            uniform float time = 0.5f;
            out vec4 FragColor;

            void main() {
                vec4 image0 = texture(texture0, TexCoord);
                vec4 image1 = texture(texture1, TexCoord);
                vec4 image2 = texture(texture2, TexCoord);

                float pulse = abs(sin(time));
                FragColor = mix(mix(image0, image1, 0.5f), image2, pulse);
            }
            """;

        public const string defaultVert = """
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;
            
            out vec2 TexCoord;
            
            uniform float tilingX = 1.0f;
            uniform float tilingY = 1.0f;
            uniform mat4 projection = mat4(1.0f);
            uniform mat4 model = mat4(1.0f);
            
            void main()
            {
                gl_Position =  projection * model * vec4(aPosition, 1.0f);
                TexCoord = vec2(aTexCoord.x * tilingX, aTexCoord.y * tilingY);
            }
            """;
        //===================================================================

        public Shader(string fragmentSource = defaultFrag, string vertexSource = defaultVert)
        {
            _VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_VertexShader, vertexSource);

            _FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_FragmentShader, fragmentSource);

            GL.CompileShader(_VertexShader);
            GL.CompileShader(_FragmentShader);

            GL.GetShader(_VertexShader, ShaderParameter.CompileStatus, out int success);
            GL.GetShader(_FragmentShader, ShaderParameter.CompileStatus, out int success2);

            if (success2 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(_FragmentShader);
                SEDebug.Log(SEDebugState.Error, $"Fragment shader error: {infoLog}");
            }

            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(_VertexShader);
                SEDebug.Log(SEDebugState.Error, $"Vertex shader error: {infoLog}");
            }

            _Handle = GL.CreateProgram();

            GL.AttachShader(_Handle, _VertexShader);
            GL.AttachShader(_Handle, _FragmentShader);
            GL.LinkProgram(_Handle);

            GL.GetProgram(_Handle, GetProgramParameterName.LinkStatus, out int success3);
            if (success3 == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_Handle);
                SEDebug.Log(SEDebugState.Error, $"Handle linking error: {infoLog}");
            }

            SEDebug.Log(SEDebugState.Info, $"Created shader handle {_Handle}");

            GL.DetachShader(_Handle, _VertexShader);
            GL.DetachShader(_Handle, _FragmentShader);
            GL.DeleteShader(_FragmentShader);
            GL.DeleteShader(_VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(_Handle);
        }

        private static readonly Dictionary<string, int> _attribCache = [];
        public int GetAttribLoc(string attribute)       // Get the location of the shader attribute
        {
            if (_attribCache.TryGetValue(attribute, out int value)) return value;

            int attrib = GL.GetAttribLocation(_Handle, attribute);

            _attribCache.Add(attribute, attrib);

            return attrib;
        }
        public void BindAttribLoc(string name, int location) => GL.BindAttribLocation(_Handle, location, name);

        public void SetBool(string name, bool value) => GL.Uniform1(GL.GetUniformLocation(_Handle, name), value ? 1 : 0);       // It's really an int. 1 or 0, take it or leave it
        public void SetUInt(string name, uint value) => GL.Uniform1(GL.GetUniformLocation(_Handle, name), value);
        public void SetInt(string name, int value) =>  GL.Uniform1(GL.GetUniformLocation(_Handle, name), value);
        public void SetLong(string name, long value) => GL.Uniform1(GL.GetUniformLocation(_Handle, name), value);
        public void SetFloat(string name, float value) => GL.Uniform1(GL.GetUniformLocation(_Handle, name), value);

        public void SetVec2(string name, Vector2 value) => GL.Uniform2(GL.GetUniformLocation(_Handle, name), value);
        public void SetVec3(string name, Vector3 value) => GL.Uniform3(GL.GetUniformLocation(_Handle, name), value);
        public void SetVec4(string name, Vector4 value) => GL.Uniform4(GL.GetUniformLocation(_Handle, name), value);

        public void SetMatrix2(string name, Matrix2 matrix) => GL.UniformMatrix2(GL.GetUniformLocation(_Handle, name), false, ref matrix);
        public void SetMatrix3(string name, Matrix3 matrix) => GL.UniformMatrix3(GL.GetUniformLocation(_Handle, name), false, ref matrix);
        public void SetMatrix4(string name, Matrix4 matrix) => GL.UniformMatrix4(GL.GetUniformLocation(_Handle, name), false, ref matrix);

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                SEDebug.Log(SEDebugState.Info, $"Disposing shader handle {_Handle}");
                GL.DeleteProgram(_Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                SEDebug.Log(SEDebugState.Warning, "GPU Resource leak, did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
