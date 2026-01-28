using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
#pragma warning disable CS8618
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

        public NativeWindow window;
        public KeyboardState keyboard;
        public MouseState mouse;

        public virtual void OnStart() { }
        public virtual void OnInit() { }
        public virtual void OnExit() { }
        public virtual void OnResize(ResizeEventArgs e) { }
        public virtual void OnFrameBufferResize(FramebufferResizeEventArgs e) { }
        public virtual void OnFrameUpdate(FrameEventArgs e) { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate(FrameEventArgs e) { }
        public void Quit() => window!.Close();

       // public virtual void IsMousePressed(MouseButtonEventArgs e) { }
    }
}