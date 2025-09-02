using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteelEngine
{
    public class EngineScript
    {
        protected EngineScript()
        {
            BehaviourManager.Add(this);
        }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public double DeltaTime { get; set; }
        public bool IsWindowFocused { get; set; }

        public KeyboardState? KeyboardState { get; set; }
        public MouseState? MouseState { get; set; }
        public CursorState CursorState { get; set; }

        public virtual void OnStart() { }
        public virtual void OnExit() { }
        public virtual void OnResize(ResizeEventArgs e) { }
        public virtual void OnFrameBufferResize(FramebufferResizeEventArgs e) { }
        public virtual void OnFrameUpdate(FrameEventArgs e) { }
        public virtual void Update(FrameEventArgs e) { }
        public virtual void LateUpdate(FrameEventArgs e) { }
        public virtual void FixedUpdate(FrameEventArgs e) { }

        public virtual void IsMousePressed(MouseButtonEventArgs e) { }
    }
}
