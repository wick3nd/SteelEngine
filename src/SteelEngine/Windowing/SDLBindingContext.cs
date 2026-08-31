using OpenTK;
using SDL3;

namespace SteelEngine.Windowing
{
    internal class SDLBindingContext : IBindingsContext
    {
        public nint GetProcAddress(string procName)
        {
            return SDL.GLGetProcAddress(procName);
        }
    }
}