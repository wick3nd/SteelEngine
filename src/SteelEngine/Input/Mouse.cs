using OpenTK.Mathematics;
using SDL3;
using SteelEngine.Common;

namespace SteelEngine
{
    public static class Mouse
    {
        static SDL.MouseButtonFlags mouseButtons;
        static SDL.MouseButtonFlags previousMouseButtons;

        static int[] mouseClickCount = new int[16];
        static int[] previousMouseClickCount = new int[16];

        public static bool IsMouseInsideWindow { get; private set; }

        //public static Vector2 GlobalPosition { get; private set; }
        //public static float GlobalX => GlobalPosition.X;
        //public static float GlobalY => GlobalPosition.Y;

        public static Vector2 RelativePosition { get; private set; }
        public static float RelX => RelativePosition.X;
        public static float RelY => RelativePosition.Y;

        public static Vector2 DeltaPosition { get; set; }
        public static float DeltaX => DeltaPosition.X;
        public static float DeltaY => DeltaPosition.Y;

        internal static void PrepareNewInputFrame()
        {
            //string miceNames = "";
            //for (int i = 0; i < mice.Length; i++)
            //{
            //    miceNames += SDL.GetMouseNameForID(mice[i]) + " ";
            //}
            //SEDebug.Log(SEDebugState.Debug, miceNames);
            var state = SDL.GetMouseState(out float x, out float y);
            
            previousMouseButtons = mouseButtons;
            previousMouseClickCount = mouseClickCount;

            mouseButtons = state;
            mouseClickCount = new int[16];

            RelativePosition = (x, y);
            DeltaPosition = Vector2.Zero;
        }

        internal static void ProcessEvents(SDL.Event @event)
        {
            if (@event.Type == (uint)SDL.EventType.MouseAdded) {  }
            if (@event.Type == (uint)SDL.EventType.MouseRemoved) {  }

            if (@event.Type == (uint)SDL.EventType.MouseMotion) ProcessMotion(@event.Motion);
            
            if (@event.Type == (uint)SDL.EventType.MouseButtonDown || @event.Type == (uint)SDL.EventType.MouseButtonUp) { ProcessButtonClick(@event.Button); }

            if (@event.Type == (uint)SDL.EventType.MouseWheel) {  }

            if (@event.Type == (uint)SDL.EventType.WindowMouseEnter) { IsMouseInsideWindow = true; }
            if (@event.Type == (uint)SDL.EventType.WindowMouseLeave) { IsMouseInsideWindow = false; }
        }

        static void ProcessMotion(SDL.MouseMotionEvent @event)
        {
            RelativePosition = (@event.X, @event.Y);
            DeltaPosition = (@event.XRel, @event.YRel);
        }

        static void ProcessButtonClick(SDL.MouseButtonEvent @event)
        {
            for (int i = 1; i < 8; i++)
            {
                if (@event.Button == i && @event.Type == SDL.EventType.MouseButtonDown)
                {
                    mouseClickCount[i] = @event.Clicks;
                }
            }
        }

        public static bool IsButtonDown(MouseButton button) => (MouseButton)mouseButtons == button;
        public static bool WasButtonDown(MouseButton button) => (MouseButton)previousMouseButtons == button;

        public static bool IsButtonHeld(MouseButton button) => WasButtonDown(button) && IsButtonDown(button);

        public static bool WasButtonHeld(MouseButton button)
        {
            throw new NotImplementedException();
        }

        public static bool IsDoubleClick(MouseButton button) => mouseClickCount[(int)button] == 2;
        public static bool DidDoubleClick(MouseButton button) => previousMouseClickCount[(int)button] == 2;
    }
}