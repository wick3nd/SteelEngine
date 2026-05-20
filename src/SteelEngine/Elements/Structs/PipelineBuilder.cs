using System.Runtime.InteropServices;

namespace SteelEngine.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PipelineBuilder(string presetName, ShaderBuilder[] shaders)
    {
        public string presetName = presetName;
        public ShaderBuilder[] shaders = shaders;
    }
}
