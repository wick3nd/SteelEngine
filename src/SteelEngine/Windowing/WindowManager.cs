using SDL3;

namespace SteelEngine.Windowing
{
    internal static class WindowManager
    {
        internal static Dictionary<uint, SEWindow> registeredWindows = new(1);
        internal static Dictionary<string, SEWindow> windowIdMatch = new(1);
        internal static SEWindow? currentWindowFocus;

        internal static void RegisterWindow(this SEWindow window)
        {
            registeredWindows.Add(window.WindowID, window);
            windowIdMatch.Add(window.Identificator, window);
        }
        internal static void QueryFocusedWindow(this SEWindow window)
        {
            currentWindowFocus = window;

            nint focusedWindowPtr = SDL.GetKeyboardFocus();
            uint focusedWindowID = SDL.GetWindowID(focusedWindowPtr);

            registeredWindows.TryGetValue(focusedWindowID, out currentWindowFocus);
        }
        internal static SEWindow QueryWindow(string identifcator) => windowIdMatch[identifcator];

        public static nint GetCurrentWindowPtr() => currentWindowFocus!.WindowPtr;
        public static nint GetCurrentWindowRenderer() => currentWindowFocus!.Renderer;
    }
}
