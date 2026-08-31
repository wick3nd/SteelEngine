using SDL3;

namespace SteelEngine.Utils
{
    internal static class SDLControl
    {
        private static SDL.InitFlags activeFlags;

        internal static void EnsureSubSystemInit(SDL.InitFlags flags)
        {
            if (activeFlags - flags == 0)
            {
               // SEDebug.Log(SEDebugState.Log, $"{flags} has been previously initialized");
                return;
            }
            
            activeFlags |= flags;
            SDL.Init(flags);
            SDLDebug.GetError();

           // SEDebug.Log(SEDebugState.Log, $"Initialized {flags}");
        }
    }
}
