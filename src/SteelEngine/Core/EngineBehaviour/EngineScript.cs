using OpenTK.Windowing.Common;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public abstract class EngineScript
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected EngineScript()
        {
            OnInit();
            BehaviourManager.Add(this);
        }

        public int windowWidth;
        public int windowHeight;

        public float deltaTime;
        public double deltaTimeD;

        public virtual void OnStart() { }
        public virtual void OnInit() { }
        public virtual void OnExit() { }
        public virtual void OnResize(ResizeEventArgs e) { }
        public virtual void OnFrameBufferResize(FramebufferResizeEventArgs e) { }
        public virtual void OnFrameUpdate(FrameEventArgs e) { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate(FrameEventArgs e) { }

       // public virtual void IsMousePressed(MouseButtonEventArgs e) { }
    }
}