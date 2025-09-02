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

        public bool isWindowFocused;

        public WindowLoop(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title /*, Flags = ContextFlags.Default, WindowBorder = WindowBorder.Fixed */})
        {
            CenterWindow(new Vector2i(width, height));
            
            BehaviourManager.ExposeWidth(width);
            BehaviourManager.ExposeHeight(height);

            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");

            SEDebug.Log(SEDebugState.Log, "Created new window");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquation(BlendEquationMode.FuncAdd);

            BehaviourManager.InitializeES();
            BehaviourManager.StartCall();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            BehaviourManager.ExitCall();

            SEDebug.Log(SEDebugState.Info, "Closing the window");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            isWindowFocused = IsFocused;

            fixedTime += e.Time;

            if (fixedTime >= fixedTimeStep)
            {
                BehaviourManager.FixedUpdateCall(e);
                fixedTime = 0;
            }

            BehaviourManager.UpdateCall(e);
            BehaviourManager.LateUpdateCall(e);



            Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            BehaviourManager.ExposeWidth(e.Width);
            BehaviourManager.ExposeHeight(e.Height);

            BehaviourManager.ResizeCall(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SEDebug.Log(SEDebugState.Info, $"resized -- Width: {e.Width} Height: {e.Height}");
            GL.Viewport(0, 0, e.Width, e.Height);

            BehaviourManager.FrameBufferResizeCall(e);
        }
    }
}
