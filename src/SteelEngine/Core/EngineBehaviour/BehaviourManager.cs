using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
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
        internal static void ExposeResolution(int width, int height)
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].windowSize = (width, height);
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
        internal static void ResizeCall()
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnResize();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FrameBufferResizeCall()
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnFrameBufferResize();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FrameUpdateCall()
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].OnFrameUpdate();
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
        internal static void WindowPosChange(Vector2i pos)
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].windowPosition = pos;
                _behaviours[i].OnWindowPosChange();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FixedUpdateCall()
        {
            for (int i = 0; i < _behaviours.Count; i++) _behaviours[i].FixedUpdate();
        }
    }
}