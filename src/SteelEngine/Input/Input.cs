using SDL3;

namespace SteelEngine
{
    public static class Input
    {
        public static uint WindowFocusID { get; internal set; }

        internal static void ProcessEvents(SDL.Event @event)
        {
            Mouse.ProcessEvents(@event);
            Keyboard.ProcessEvents(@event);
        }

        internal static void PrepareNewInputFrame()
        {
            if (SDL.HasMouse())
            {
                Mouse.PrepareNewInputFrame();
            }
           
            if (SDL.HasKeyboard())
            {
                Keyboard.PrepareNewInputFrame();
            }
        }
    }
}