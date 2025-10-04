using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SteelEngine.SteelEngine.Base;
using SteelEngine.Utils;

namespace SteelEngine
{
    public class WindowLoop : GameWindow
    {
        private static double fixedUpdateTimer;
        public double fixedTimeStep = .016;

        public NativeWindow? PublicWindowReference;

        public WindowLoop(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), MinimumClientSize = (256, 144), Title = title, Vsync = VSyncMode.Off, Flags = ContextFlags.Default/*, WindowBorder = WindowBorder.Fixed*/})
        {
            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");

            CenterWindow(new Vector2i(width, height));

            SEDebug.Log(SEDebugState.Log, "Created new window");

            PublicWindowReference = this;
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);

            // GL.DepthFunc(DepthFunction.Notequal);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendEquation(BlendEquationMode.FuncAdd);

            BehaviourManager.InitializeES();
            BehaviourManager.StartCall();
            BehaviourManager.ExposeWindow(PublicWindowReference!);
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

            BehaviourManager.ExposeWindow(PublicWindowReference!);
            BehaviourManager.ExposeTime(e.Time);

            fixedUpdateTimer += e.Time;

            if (fixedUpdateTimer >= fixedTimeStep)
            {
                BehaviourManager.FixedUpdateCall(e);
                fixedUpdateTimer = 0f;
            }

            BehaviourManager.UpdateCall(e);
            BehaviourManager.LateUpdateCall(e);

            Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            SEDebug.Log(SEDebugState.Info, $"Window resized -- {e.Width}x{e.Height}");

            BehaviourManager.ExposeWidth(e.Width);
            BehaviourManager.ExposeHeight(e.Height);
            BehaviourManager.ResizeCall(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            BehaviourManager.FrameBufferResizeCall(e);
        }
    }
}
