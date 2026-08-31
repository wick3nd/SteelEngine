using SDL3;
using SteelEngine.Utils;

namespace SteelEngine.Windowing
{
    public static class Monitors
    {
        public static Display[] displays;
        public static Display primaryDisplay => displays[0];

        internal static void Init()
        {
            SDLControl.EnsureSubSystemInit(SDL.InitFlags.Video);

            uint[]? displayID = SDL.GetDisplays(out int displayCount);
            if (displayID == null) SEDebug.Log(SEDebugState.Error, "No displays have been found", throwException: true);

            displays = new Display[displayCount];
            for (int i = 0; i < displayCount; i++)
            {
                displays[i] = new(displayID[i]);
            }
        }
    }
}