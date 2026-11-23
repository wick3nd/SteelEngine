using OpenTK.Mathematics;
using SDL3;
using SteelEngine.AssetLoader;
using SteelEngine.Windowing;

namespace SteelEngine
{
    public static class Window
    {
        public static SEWindow Get(string internalName) => WindowManager.QueryWindow(internalName);

        public static void CursorState(CursorState state)
        {
            nint window = WindowManager.GetCurrentWindowPtr();

            SDL.SetWindowRelativeMouseMode(window, state!=0);

            SDL.SetHint(SDL.Hints.MouseRelativeCursorVisible, (state!=Windowing.CursorState.Grabbed) ? "1" : "0");
            SDL.SetHint(SDL.Hints.MouseRelativeModeCenter, (state==Windowing.CursorState.Grabbed) ? "1" : "0");
        }

        public static void ChangeState(WindowState state)
        {
            nint window = WindowManager.GetCurrentWindowPtr();

            bool borderless = (state & WindowState.Borderless) == WindowState.Borderless;
            bool minimized = (state & WindowState.Minimized) == WindowState.Minimized;
            bool fullscreen = (state & WindowState.Fullscreen) == WindowState.Fullscreen;

            SDL.SetWindowBordered(window, !borderless);
            SDL.SetWindowFullscreen(window, fullscreen);
            if (minimized) SDL.MinimizeWindow(window);
        }

        public static bool IsFullscreen() => WindowManager.currentWindowFocus.Fullscreen;

        //public static void SetVSync(bool enabled)
        //{
        //    nint renderer = WindowManager.GetCurrentWindowRenderer();

        //    SDL.GLSetSwapInterval(enabled ? 1 : 0);
        //}

        public static void SetIcon(string path)
        {
            nint window = WindowManager.GetCurrentWindowPtr();

            var icon = TextureLoader.LoadTexture(path);
            
            fixed (byte* p = icon.imageBytes)
            {
                var windowIcon = SDL.CreateSurfaceFrom(icon.width, icon.height, SDL.PixelFormat.RGBA8888, (nint)p, icon.width*3);
                SDL.SetWindowIcon(window, windowIcon);
            }
        }

        public static void SetTitle(string title)
        {
            SDL.SetWindowTitle(WindowManager.GetCurrentWindowPtr(), title);
        }

        public static void MoveBy(Vector2i moveVector)
        {
            var window = WindowManager.currentWindowFocus;
            SDL.SetWindowPosition(window!.WindowPtr, window.PositionX + moveVector.X, window.PositionY + moveVector.Y);
        }

        public static void MoveTo(Vector2i newPosition)
        {
            nint window = WindowManager.GetCurrentWindowPtr();

            SDL.SetWindowPosition(window, newPosition.X, newPosition.Y);
        }

        //public static void X()
        //{
        //    nint window = WindowManager.GetCurrentWindowPtr();

        //    bool resizable;

        //    SDL.SetWindowResizable(window, resizable);
        //}

        //internal static NativeWindow nativeWindow = null!;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static CursorState GetState() => nativeWindow.CursorState;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static void ChangeCursor(CursorState state) => nativeWindow.CursorState = state;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static CursorState GetCursor() => nativeWindow.CursorState;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static void ChangeBorder(WindowBorder border) => nativeWindow.WindowBorder = border;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static WindowBorder GetBorder() => nativeWindow.WindowBorder;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static void SetVSync(VSyncMode mode) => nativeWindow.VSync = mode;

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static VSyncMode GetVSync() => nativeWindow.VSync;
    }
}
