using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteelEngine.Utils;
using SteelEngine.Core;

namespace SteelEngine
{
    public class WindowLoop : GameWindow
    {
        double _physicsTimeStepTimer;

        public WindowLoop(NativeWindowSettings windowSettings) : base(GameWindowSettings.Default, windowSettings)
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

                BehaviourManager.ExposeInformation(this);
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

                BehaviourManager.ExposeInformation(this);
                BehaviourManager.ExposeTime(e.Time);

                _physicsTimeStepTimer += e.Time;

                while (_physicsTimeStepTimer >= EngineSettings.PhysicsTimeStep)
                {
                    BehaviourManager.FixedUpdateCall(e);
                    _physicsTimeStepTimer -= EngineSettings.PhysicsTimeStep;
                    // fixedUpdateTimer = 0;
                }

                BehaviourManager.FrameLogicCall();

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