using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace SteelEngine.Core.EngineBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ShaderContainer(int handle, ShaderType type)
    {
        int shaderID = handle;
        ShaderType type = type;
    }
}