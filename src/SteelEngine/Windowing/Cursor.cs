using SDL3;
using SteelEngine.Utils;

namespace SteelEngine.Windowing
{
    public class Cursor
    {
        public nint CursorPtr  { get; }
        public nint ParentWindowPtr { get; }

        public bool IsHidden { get; private set; }
        public bool IsLocked { get; private set; }
        public bool IsGrabbed { get; private set; }

        public Cursor(nint windowPtr)
        {
            CursorPtr = SDL.GetCursor();
            ParentWindowPtr = windowPtr;

            SEDebug.Log(SEDebugState.Log, CursorPtr);
        }

        public void Hide()
        {
            SDL.HideCursor();
            IsHidden = true;
        }

        public void Show()
        {
            SDL.ShowCursor();
            IsHidden = false;
        }

        public void Lock()
        {
            SDL.SetWindowMouseGrab(ParentWindowPtr, true);
            IsLocked = true;
        }

        public void Unlock()
        {
            SDL.SetWindowMouseGrab(ParentWindowPtr, false);
            IsLocked = false;
        }

        public void Grab()
        {
            SDL.SetWindowRelativeMouseMode(ParentWindowPtr, true);
            IsGrabbed = true;
        }

        public void Release()
        {
            SDL.SetWindowRelativeMouseMode(ParentWindowPtr, false);
            IsGrabbed = false;
        }
    }
}
