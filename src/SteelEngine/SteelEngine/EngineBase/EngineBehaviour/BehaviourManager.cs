using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace SteelEngine.EngineBase.EngineBehaviour
{
    internal static class BehaviourManager
    {
        private static readonly List<EngineScript> behaviours = [];
        internal static void Add(EngineScript script) => behaviours.Add(script);    // EngineScript methods to run


        internal static void ExposeWindow(NativeWindow window)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].windowReference = window;
        }
        internal static void ExposeResolution(int width, int height)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].windowWidth = width;
                behaviours[i].windowHeight = height;
            }
        }
        internal static void ExposeTime(double deltaTime)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].deltaTime = (float)deltaTime;
                behaviours[i].deltaTimeD = deltaTime;
            }
        }
        internal static void StartCall()
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnStart();
        }
        internal static void ExitCall()
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnExit();
        }
        internal static void ResizeCall(ResizeEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnResize(e);
        }
        internal static void FrameBufferResizeCall(FramebufferResizeEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnFrameBufferResize(e);
        }
        internal static void FrameUpdateCall(FrameEventArgs args)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnFrameUpdate(args);
        }
        internal static void UpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].Update(e);
        }
        internal static void LateUpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].LateUpdate(e);
        }
        internal static void FixedUpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].FixedUpdate(e);
        }
    }
}