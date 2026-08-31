using SDL3;

namespace SteelEngine
{
    public static class Input
    {
        public static uint WindowFocusID { get; internal set; }
        public static Mouse Mouse { get; internal set; }

        internal static void ProcessEvents(SDL.Event @event)
        {
            Mouse.ProcessEvents(@event);
        }

        internal static void PrepareNewInputFrame()
        {
            Mouse = new();
           // Mouse.PrepareNewInputFrame();
        }
    }
}
