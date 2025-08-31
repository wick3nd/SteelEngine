using OpenTK.Windowing.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace SteelEngine
{
    public static class BehaviourManager
    {
        private static readonly List<EngineScript> behaviours = [];

        public static void Add(EngineScript script) => behaviours.Add(script);    // EngineScript methods to run

        private static readonly Type[] _engineScriptTypes = [.. Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(EngineScript)) && !t.IsAbstract)];    // EngineScripts to run
        public static void InitializeES()
        {
            for (int i = 0; i < _engineScriptTypes.Length; i++)
            {
                var ctor = _engineScriptTypes[i].GetConstructor(Type.EmptyTypes);
                if (ctor != null)
                {
                    var newExpr = Expression.New(ctor);
                    var lambda = Expression.Lambda<Func<EngineScript>>(newExpr).Compile();
                    lambda();
                }
            }
        }

        public static void ExposeWidth(int width)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].WindowWidth = width;
        }
        public static void ExposeHeight(int height)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].WindowHeight = height;
        }
        public static void StartCall()
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnStart();
        }
        public static void ExitCall()
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnExit();
        }
        public static void ResizeCall(ResizeEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnResize(e);
        }
        public static void FrameBufferResizeCall(FramebufferResizeEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnFrameBufferResize(e);
        }
        public static void FrameUpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].OnFrameUpdate(e);
        }
        public static void UpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].Update(e);
        }
        public static void LateUpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].LateUpdate(e);
        }
        public static void FixedUpdateCall(FrameEventArgs e)
        {
            for (int i = 0; i < behaviours.Count; i++) behaviours[i].FixedUpdate(e);
        }
    }
}
