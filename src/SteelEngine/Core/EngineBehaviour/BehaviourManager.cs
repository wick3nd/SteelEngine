using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    internal static class BehaviourManager
    {
        private static readonly List<EngineScript> _behaviours = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Add(EngineScript script) => _behaviours.Add(script);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Remove(EngineScript script) => _behaviours.Remove(script);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ExposeInformation(NativeWindow window)
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].window = window;
                _behaviours[i].keyboard = window.KeyboardState;
                _behaviours[i].mouse = window.MouseState;
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ExposeResolution(int width, int height)
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].windowWidth = width;
                _behaviours[i].windowHeight = height;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ExposeTime(double deltaTime)
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].deltaTime = (float)deltaTime;
                _behaviours[i].deltaTimeD = deltaTime;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void StartCall()
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnStart();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ExitCall()
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnExit();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ResizeCall(ResizeEventArgs e)
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnResize(e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FrameBufferResizeCall(FramebufferResizeEventArgs e)
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnFrameBufferResize(e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FrameUpdateCall(FrameEventArgs args)
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnFrameUpdate(args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FrameLogicCall()
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].Update();
                _behaviours[i].LateUpdate();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FixedUpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].FixedUpdate(e);
        }
    }
}