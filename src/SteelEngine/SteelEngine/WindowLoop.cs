using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteelEngine.Utils;

namespace SteelEngine
{
    public class WindowLoop : GameWindow
    {
        private static double fixedUpdateTimer;
        public double fixedTimeStep = .016;

        public NativeWindow? PublicWindowReference;

        public WindowLoop(NativeWindowSettings NW) : base(GameWindowSettings.Default, NW)
        {
            GL.LoadBindings(new GLFWBindingsContext());    // Load the OpenGL bindings separately for trim compatibility

            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");

            CenterWindow(new Vector2i(ClientSize.X, ClientSize.Y));

            SEDebug.Log(SEDebugState.Log, "Created new window");

            PublicWindowReference = this;
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);

           // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
           // GL.BlendFunc(0, BlendingFactorSrc.One, BlendingFactorDest.One);
           // GL.BlendFunc(1, BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BlendEquation(BlendEquationMode.FuncAdd);

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
                fixedUpdateTimer -= fixedTimeStep;
               // fixedUpdateTimer = 0;
            }

            BehaviourManager.UpdateCall(e);
            BehaviourManager.LateUpdateCall(e);

            Context.SwapBuffers();

            BehaviourManager.AfterBufferSwap(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            BehaviourManager.FrameUpdateCall(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            SEDebug.Log(SEDebugState.Info, $"Window resized -- {e.Width}x{e.Height}");

            BehaviourManager.ExposeResolution(e.Width, e.Height);
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
