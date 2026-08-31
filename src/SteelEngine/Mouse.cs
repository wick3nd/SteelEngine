using OpenTK.Mathematics;
using SDL3;
using SteelEngine.Common;
using SteelEngine.Utils;
using System.Collections;

namespace SteelEngine
{
    public class Mouse
    {
        BitArray mouseButtons = new(16);
        BitArray previousMouseButtons = new(16);

        int[] mouseClickCount = new int[16];
        int[] previousMouseClickCount = new int[16];

        public bool IsMouseInsideWindow { get; private set; }

        //public static Vector2 GlobalPosition { get; private set; }
        //public static float GlobalX => GlobalPosition.X;
        //public static float GlobalY => GlobalPosition.Y;

        public Vector2 RelativePosition { get; private set; }
        public float RelX => RelativePosition.X;
        public float RelY => RelativePosition.Y;

        public Vector2 DeltaPosition { get; set; }
        public float DeltaX => DeltaPosition.X;
        public float DeltaY => DeltaPosition.Y;

        internal void PrepareNewInputFrame()
        {
            //string miceNames = "";
            //for (int i = 0; i < mice.Length; i++)
            //{
            //    miceNames += SDL.GetMouseNameForID(mice[i]) + " ";
            //}
            //SEDebug.Log(SEDebugState.Debug, miceNames);
            
            previousMouseButtons = mouseButtons;
            previousMouseClickCount = mouseClickCount;

            mouseButtons.SetAll(false);
            mouseClickCount = new int[16];

            DeltaPosition = Vector2.Zero;
        }

        internal void ProcessEvents(SDL.Event @event)
        {
            if (@event.Type == (uint)SDL.EventType.MouseAdded) {  }
            if (@event.Type == (uint)SDL.EventType.MouseRemoved) {  }

            if (@event.Type == (uint)SDL.EventType.MouseMotion) ProcessMotion(@event.Motion);
            
            if (@event.Type == (uint)SDL.EventType.MouseButtonDown || @event.Type == (uint)SDL.EventType.MouseButtonUp) { ProcessButtonClick(@event.Button); }

            if (@event.Type == (uint)SDL.EventType.MouseWheel) {  }

            if (@event.Type == (uint)SDL.EventType.WindowMouseEnter) { IsMouseInsideWindow = true; }
            if (@event.Type == (uint)SDL.EventType.WindowMouseLeave) { IsMouseInsideWindow = false; }
        }

        void ProcessMotion(SDL.MouseMotionEvent @event)
        {
           // SEDebug.Log(SEDebugState.Log, SDL.GetMouseNameForID(@event.Which));

            RelativePosition = (@event.X, @event.Y);
            DeltaPosition = (@event.XRel, @event.YRel);
        }

        void ProcessButtonClick(SDL.MouseButtonEvent @event)
        {
            for (int i = 0; i < mouseButtons.Length; i++)
            {
                if (@event.Button == i && @event.Type == SDL.EventType.MouseButtonDown)
                {
                    mouseButtons[i] = true;
                    mouseClickCount[i] = @event.Clicks;
                }
            }
        }

        public bool IsButtonDown(MouseButton button) => mouseButtons[(int)button];
        public bool WasButtonDown(MouseButton button) => previousMouseButtons[(int)button];

        public bool IsButtonHeld(MouseButton button) => WasButtonDown(button) && IsButtonDown(button);

        public bool WasButtonHeld(MouseButton button)
        {
            throw new NotImplementedException();
        }

        public bool IsDoubleClick(MouseButton button) => mouseClickCount[(int)button] == 2;

        public bool DidDoubleClick(MouseButton button) => previousMouseClickCount[(int)button] == 2;

        public bool IsMultiClick(MouseButton button, int clickCount) => mouseClickCount[(int)button] == clickCount;

        public bool DidMultiClick(MouseButton button, int clickCount) => previousMouseClickCount[(int)button] == clickCount;
    }
}