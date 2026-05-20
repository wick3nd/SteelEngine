using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public partial class Shader
    {
        private readonly Dictionary<string, int> _attribCache = [];
        private readonly Dictionary<string, int> _uniformCache = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetAttribLoc(string attribute)
        {
            if (_attribCache.TryGetValue(attribute, out int value)) return value;

            int attrib = GL.GetAttribLocation(m_Shader, attribute);
            _attribCache.Add(attribute, attrib);

            return attrib;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLoc(string uniform)
        {
            if (_uniformCache.TryGetValue(uniform, out int value)) return value;

            int attrib = GL.GetUniformLocation(m_Shader, uniform);
            _uniformCache.Add(uniform, attrib);

            return attrib;
        }
    }
}
