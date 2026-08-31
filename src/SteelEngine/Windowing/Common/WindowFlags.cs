namespace SteelEngine.Windowing.Common
{
    [Flags]
    public enum WindowFlags : ulong
    {
        Fullscreen        = 1 << 0,
        Occluded          = 1 << 2,
        Hidden            = 1 << 3,
        Borderless        = 1 << 4,
        Resizable         = 1 << 5,
        Minimized         = 1 << 6,
        Maximized         = 1 << 7,
        MouseGrabbed      = 1 << 8,
        InputFocus        = 1 << 9,
        MouseFocus        = 1 << 10,
        Modal             = 1 << 11,
        HighPixelDensity  = 1 << 12,
        MouseCapture      = 1 << 13,
        MouseRelativeMode = 1 << 14,
        AlwaysOnTop       = 1 << 15,
        Utility           = 1 << 16,
        Tooltip           = 1 << 17,
        PopupMenu         = 1 << 18,
        KeyboardGrabbed   = 1 << 19,
        FillDocument      = 1 << 20,
        Transparent       = 1 << 23,
        NotFocusable      = 1 << 24,

       // Engine Specific Flags
        MultiMouse = 1UL << 33,
        MultiKeyboard = 1UL << 34,
    }
}
