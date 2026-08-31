using SDL3;

namespace SteelEngine.Utils
{
    public static class SDLDebug
    {
        public static bool GetError()
        {
            string error = SDL.GetError();
            if (error != null) return true;
            return false;
        }
    }
}