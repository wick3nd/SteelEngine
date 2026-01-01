using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteelEngine.Utils;
using SteelEngine.EngineBase.EngineBehaviour;

namespace SteelEngine
{
    public class WindowLoop : GameWindow
    {
#pragma warning disable CA2211, CS0108

        public static string Title = "SteelEngine window";
        public static Vector2i WindowSize = (640, 360);
        public static Vector2i MinWindowSize = (256, 144);

        public double fixedTimeStep = .016;
        static double _fixedUpdateTimer;

#pragma warning restore CA2211, CS0108

        public WindowLoop() : base(GameWindowSettings.Default, new NativeWindowSettings() { Title = Title, ClientSize = WindowSize, MinimumClientSize = MinWindowSize, Profile = ContextProfile.Compatability })
        {
            try
            {
                CenterWindow(new Vector2i(ClientSize.X, ClientSize.Y));

                if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
                OpenTK.Graphics.GLLoader.LoadBindings(new GLFWBindingsContext());    // Load the OpenGL bindings separately for trim compatibility

                SEDebug.Log(SEDebugState.Info, $"{GL.GetString(StringName.Version)} {GL.GetString(StringName.Renderer)} GL{GL.GetInteger(GetPName.MajorVersion)}.{GL.GetInteger(GetPName.MinorVersion)}");
                SEDebug.Log(SEDebugState.Log, "Created new window");
                
                GLControl.GetExtensions();
            }
            catch (Exception ex)
            {
                Crash(ex);
            }
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            try
            {
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.CullFace);
                GL.Enable(EnableCap.Blend);

               // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
               // GL.BlendFunc(0, BlendingFactorSrc.One, BlendingFactorDest.One);
               // GL.BlendFunc(1, BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcAlpha);
                GL.BlendEquation(BlendEquationMode.FuncAdd);

                BehaviourManager.ExposeWindow(this);
                BehaviourManager.ExposeResolution(ClientSize.X, ClientSize.Y);
                BehaviourManager.StartCall();
            }
            catch (Exception ex)
            {
                Crash(ex);
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            try
            {
                BehaviourManager.ExitCall();

                SEDebug.Log(SEDebugState.Info, "Closing the window");
            }
            catch (Exception ex)
            {
                Crash(ex);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            try
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                BehaviourManager.ExposeWindow(this);
                BehaviourManager.ExposeTime(e.Time);

                _fixedUpdateTimer += e.Time;

                if (_fixedUpdateTimer >= fixedTimeStep)
                {
                    BehaviourManager.FixedUpdateCall(e);
                    _fixedUpdateTimer -= fixedTimeStep;
                    // fixedUpdateTimer = 0;
                }

                BehaviourManager.UpdateCall(e);
                BehaviourManager.LateUpdateCall(e);

                Context.SwapBuffers();
            }
            catch (Exception ex)
            {
                Crash(ex);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            try
            {
                BehaviourManager.FrameUpdateCall(args);
            }
            catch (Exception ex)
            {
                Crash(ex);
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            try
            {
                SEDebug.Log(SEDebugState.Info, $"Window resized -- {e.Width}x{e.Height}");

                BehaviourManager.ExposeResolution(e.Width, e.Height);
                BehaviourManager.ResizeCall(e);
            }
            catch (Exception ex)
            {
                Crash(ex);
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            try
            {
                GL.Viewport(0, 0, e.Width, e.Height);

                BehaviourManager.FrameBufferResizeCall(e);
            }
            catch (Exception ex)
            {
                Crash(ex);
            }
        }

        private void Crash(Exception ex)
        {
            SEDebug.Log(SEDebugState.Error, $"Exception: {ex.Message}\n{ex.StackTrace}");
            Close();
        }
    }
}