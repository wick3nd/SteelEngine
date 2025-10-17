using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteelEngine
{
    public abstract class EngineScript
    {
        protected EngineScript() => BehaviourManager.Add(this);

        public int windowWidth;
        public int windowHeight;

        public float deltaTime;
        public double deltaTimeD;

        public NativeWindow? windowReference;
        public KeyboardState? keyboardState;
        public MouseState? mouseState;

        public virtual void OnStart() { }
        public virtual void OnExit() { }
        public virtual void OnResize(ResizeEventArgs e) { }
        public virtual void OnFrameBufferResize(FramebufferResizeEventArgs e) { }
        public virtual void OnFrameUpdate(FrameEventArgs e) { }
        public virtual void Update(FrameEventArgs e) { }
        public virtual void LateUpdate(FrameEventArgs e) { }
        public virtual void FixedUpdate(FrameEventArgs e) { }
        public virtual void AfterBufferSwap(FrameEventArgs e) { }

        public virtual void IsMousePressed(MouseButtonEventArgs e) { }
    }
}
