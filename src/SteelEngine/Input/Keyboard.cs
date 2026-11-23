using SDL3;

namespace SteelEngine
{
    public static class Keyboard
    {
        internal static uint keyboardID;
        static bool[] previousKeyboardKeys;
        static bool[] keyboardKeys;

        //internal static SDL.KeyboardEvent currentKeyboardFrameEvent;
        //internal static SDL.KeyboardEvent previousKeyboardFrameEvent;
        //internal static SDL.KeyboardDeviceEvent currentKeyboardFrameDeviceEvent;
        //internal static SDL.KeyboardDeviceEvent previousKeyboardFrameDeviceEvent;

        internal static void ProcessEvents(SDL.Event @event)
        {
            // if keyboardID == kdevice/key.which
            if (@event.Type == (uint)SDL.EventType.KeyboardAdded) {  }
            if (@event.Type == (uint)SDL.EventType.KeyboardRemoved) {  }
            if (@event.Type == (uint)SDL.EventType.KeyDown) {  }
            if (@event.Type == (uint)SDL.EventType.KeyUp) {  }
            if (@event.Type == (uint)SDL.EventType.KeymapChanged) {  }
            if (@event.Type == (uint)SDL.EventType.ScreenKeyboardHidden) {  }
            if (@event.Type == (uint)SDL.EventType.ScreenKeyboardShown) {  }
        }

        internal static void PrepareNewInputFrame()
        {
            previousKeyboardKeys = keyboardKeys;
            keyboardKeys = SDL.GetKeyboardState(out int numOfKeys).ToArray();

            //previousKeyboardFrameEvent = currentKeyboardFrameEvent;
            //previousKeyboardFrameDeviceEvent = currentKeyboardFrameDeviceEvent;
        }

        public static bool IsKeyDown(Scancode key) => keyboardKeys[(int)key];
        public static bool IsKeyPressed(Scancode key) => keyboardKeys[(int)key] && !previousKeyboardKeys[(int)key];
    }
}