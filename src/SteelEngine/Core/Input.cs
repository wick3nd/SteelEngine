using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.CompilerServices;

namespace SteelEngine
{
    public static class Input
    {
        internal static MouseState mouse = null!;
        internal static KeyboardState keyboard = null!;
        internal static JoystickState[] controller = null!;

        /// <summary>
        /// Returns true if any keyboard key is pressed
        /// </summary>
        public static bool IsAnyKeyDown {get; private set; }

        /// <summary>
        /// Returns true if any mouse button is pressed
        /// </summary>
        public static bool IsAnyMouseButtonDown {get; private set; }

        /// <summary>
        /// Returns the max double click delay
        /// </summary>
        public static float MouseDoubleClickTime { get; private set; }

        /// <summary>
        /// Returns how much the mouse has moved since the last frame
        /// </summary>
        public static Vector2 MousePosDelta { get; private set; }

        /// <summary>
        /// Returns the current mouse position
        /// </summary>
        public static Vector2 MousePosition { get; private set; }

        /// <summary>
        /// Returns the mouse position from the previous frame
        /// </summary>
        public static Vector2 PreviousMousePos { get; private set; }

        /// <summary>
        /// Returns the mouse X position
        /// </summary>
        public static float MouseX { get; private set; }

        /// <summary>
        /// Returns the mouse X position from the previous frame
        /// </summary>
        public static float PreviousMouseX { get; private set; }

        /// <summary>
        /// Returns the mouse Y position
        /// </summary>
        public static float MouseY { get; private set; }

        /// <summary>
        /// Returns the mouse Y position from the previous frame
        /// </summary>
        public static float PreviousMouseY { get; private set; }

        /// <summary>
        /// Returns how much the scroll wheel has moved since the last frame
        /// </summary>
        public static Vector2 ScrollDelta { get; private set; }
        
        /// <summary>
        /// Returns the accumulated scroll value
        /// </summary>
        public static Vector2 Scroll { get; private set; }

        /// <summary>
        /// Returns how much the scroll value has changed since the last frame
        /// </summary>
        public static Vector2 PreviousScroll { get; private set; }

        public static float Deadzone { get; private set; }

        /// <summary>
        /// Captures various input from kaybyoards, mouses and controllers every frame
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CaptureInput(ref NativeWindow window)
        {
            IsAnyMouseButtonDown = window.IsAnyMouseButtonDown;
            
            mouse = window.MouseState;
            keyboard = window.KeyboardState;

            controller = new JoystickState[window.JoystickStates.Count];
            for (int i = 0; i < window.JoystickStates.Count; i++) controller[i] = window.JoystickStates[i];

            UpdateMouseInput();
            UpdateKeyboardInput();
        }

        static void UpdateMouseInput()
        {
            MouseDoubleClickTime = 0.5f; // Temporarily

            MousePosDelta = mouse.Delta;
            MousePosition = mouse.Position;
            PreviousMousePos = mouse.PreviousPosition;
            PreviousMouseX = mouse.PreviousX;
            PreviousMouseY = mouse.PreviousY;
            MouseX = mouse.X;
            MouseY = mouse.Y;

            ScrollDelta = mouse.ScrollDelta;
            Scroll = mouse.Scroll;
            PreviousScroll = mouse.PreviousScroll;
        }

        static void UpdateKeyboardInput()
        {
            IsAnyKeyDown = keyboard.IsAnyKeyDown;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WasMouseDown(MouseButton button) => mouse.WasButtonDown(button);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OnMouseUp(MouseButton button) => mouse.IsButtonReleased(button);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OnMouseClick(MouseButton button) => mouse.IsButtonDown(button);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OnMouseHold(MouseButton button) => mouse.IsButtonPressed(button);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OnDoubleClick(MouseButton button)
        {
            _ = new NotImplementedException();
            return false;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WasKeyDown(Keys key) => keyboard.WasKeyDown(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyDown(Keys key) => keyboard.IsKeyDown(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyPressed(Keys key) => keyboard.IsKeyPressed(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsKeyUp(Keys key) => keyboard.IsKeyReleased(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsButtonDown(int index) => controller[0].IsButtonDown(index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float GetAxis(int index) => controller[0].GetAxis(index);
    }
}