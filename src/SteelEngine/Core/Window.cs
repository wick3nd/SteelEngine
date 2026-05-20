using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Runtime.CompilerServices;

namespace SteelEngine
{
    public static class Window
    {
        internal static NativeWindow nativeWindow = null!;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeState(WindowState state) => nativeWindow.WindowState = state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CursorState GetState() => nativeWindow.CursorState;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeCursor(CursorState state) => nativeWindow.CursorState = state;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CursorState GetCursor() => nativeWindow.CursorState;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeBorder(WindowBorder border) => nativeWindow.WindowBorder = border;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WindowBorder GetBorder() => nativeWindow.WindowBorder;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetVSync(VSyncMode mode) => nativeWindow.VSync = mode;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VSyncMode GetVSync() => nativeWindow.VSync;
    }
}