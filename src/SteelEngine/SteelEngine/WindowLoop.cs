using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using SteelEngine.Utils;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;

namespace SteelEngine
{
    public class WindowLoop : GameWindow
    {
        private static double fixedTime;
        public double fixedTimeStep = .02;

        public WindowRes windowRes = new();

        public WindowLoop(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title /*, Flags = ContextFlags.Default, WindowBorder = WindowBorder.Fixed */})
        {
            CenterWindow(new Vector2i(width, height));

            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs"); 

            SEDebug.Log(SEDebugState.Log, "Created new window");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            windowRes.width = Monitors.GetPrimaryMonitor().HorizontalResolution;
            windowRes.height = Monitors.GetPrimaryMonitor().VerticalResolution;

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquation(BlendEquationMode.FuncAdd);

            Start();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            OnExit();

            SEDebug.Log(SEDebugState.Info, "Closing the window");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            fixedTime += e.Time;

            if (fixedTime >= fixedTimeStep)
            {
                FixedUpdate(e);
                fixedTime = 0;
            }

            Update(e);
            LateUpdate(e);

            Context.SwapBuffers();
        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            SEDebug.Log(SEDebugState.Info, $"resized -- Width: {e.Width} Height: {e.Height}");

            GL.Viewport(0, 0, e.Width, e.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e) => base.OnUpdateFrame(e);
        protected override void OnMouseDown(MouseButtonEventArgs e) => base.OnMouseDown(e);

        protected virtual void Start() { }
        protected virtual void OnExit() { }
        protected virtual void OnResize(FramebufferResizeEventArgs e) { }
        protected virtual void Update(FrameEventArgs e) { }
        protected virtual void LateUpdate(FrameEventArgs e) { }
        protected virtual void FixedUpdate(FrameEventArgs e) { }
    }
}

