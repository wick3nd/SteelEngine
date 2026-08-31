using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SDL3;
using StbImageSharp;
using SteelEngine.SteelWorks;
using SteelEngine.Core;
using SteelEngine.Core.Buffers;
using SteelEngine.Utils;
using SteelEngine.Windowing.Common;
using System.Diagnostics;

namespace SteelEngine.Windowing
{
    public class SEWindow
    {
       // public readonly Window window;
        public nint WindowPtr { get; }
        public uint WindowID { get; }
        private nint _RendererContext;

        public Cursor cursor { get; }

        public string Title;
        public ContextAPI Renderer { get; }
        public WindowFlags WindowFlags { get; }

        public Vector2i WindowSize { get; }
        public int Width => WindowSize.X;
        public int Height => WindowSize.Y;

        public Vector2i MinWindowSize { get; }
        public int MinWidth => MinWindowSize.X;
        public int MinHeight => MinWindowSize.Y;

        public Vector2i MaxWindowSize { get; }
        public int MaxWidth => MaxWindowSize.X;
        public int MaxHeight => MaxWindowSize.Y;
        
        private readonly Stopwatch _FrameTimeStopwatch = new();

        public SEWindow(SEWindowSettings settings)
        {
            OpenTK.Graphics.GLLoader.LoadBindings(new SDLBindingContext());
            SEDebug.Init();
            Monitors.Init();
            
            Title = settings.Title;
            WindowSize = settings.WindowSize;
            Renderer = settings.Context;
            WindowFlags = settings.WindowFlags;

            Display display = Monitors.primaryDisplay;
            if (settings.WindowSize == new Vector2i()) WindowSize = new(display.Width >> 1, display.Height >> 1);
            if (settings.MinimumWindowSize == new Vector2i()) MinWindowSize = new(display.Width >> 3, display.Height >> 3);
            if (settings.MaximumWindowSize == new Vector2i()) MaxWindowSize = new(display.Width, display.Height);

            SDL.GLSetAttribute(SDL.GLAttr.DoubleBuffer, 1);
            SDL.GLSetAttribute(SDL.GLAttr.DepthSize, 24);

            WindowPtr = SDL.CreateWindow(Title, Width, Height, (SDL.WindowFlags)Renderer | (SDL.WindowFlags)WindowFlags);
            WindowID = SDL.GetWindowID(WindowPtr);
            if (!SDL.SetWindowMinimumSize(WindowPtr, MinWidth, MinHeight)) SDLDebug.GetError();
            if (!SDL.SetWindowMaximumSize(WindowPtr, MaxWidth, MaxHeight)) SDLDebug.GetError();
           // if (!SDL.CaptureMouse(((ulong)WindowFlags & (1UL << 33)) != 0)) SDLDebug.GetError();
            //SDL.SetHint(SDL.Hints.MouseRelativeCursorVisible, "1");
            //SDL.SetHint(SDL.Hints.MouseRelativeModeCenter, "0");
            cursor = new(WindowPtr);
        }

        public void Run()
        {
            _RendererContext = SDL.GLCreateContext(WindowPtr);
            SDL.GLSetSwapInterval(0);
            
            Input.PrepareNewInputFrame();
            while (SDL.PollEvent(out SDL.Event @event))
            {
                if (@event.Type == (uint)SDL.EventType.Quit) OnUnload();
                if (@event.Type == (uint)SDL.EventType.WindowResized) OnResize();
                Input.ProcessEvents(@event);
            }

            OnLoad();

            _FrameTimeStopwatch.Start();
            bool loop = true;
            double timeSinceLastFixedUpdate = 0;
            
            while (loop)
            {
                Time.DeltaTimeD = _FrameTimeStopwatch.Elapsed.TotalSeconds;
                timeSinceLastFixedUpdate += Time.DeltaTimeD;
                _FrameTimeStopwatch.Restart();

                Input.PrepareNewInputFrame();

                while (SDL.PollEvent(out SDL.Event @event))
                {
                    if (@event.Type == (uint)SDL.EventType.Quit) loop = false;
                    if (@event.Type == (uint)SDL.EventType.WindowResized) OnResize();
                    Input.ProcessEvents(@event);
                }
                
                OnRenderFrame();  // arg is temporary
                if (timeSinceLastFixedUpdate >= Physics.updateTimestep)
                {
                    FixedUpdate();  // arg is temporary
                    timeSinceLastFixedUpdate -= Physics.updateTimestep;
                }
                
                SDL.GLSwapWindow(WindowPtr);
                SEDebug.Flush();
            }
        
            OnUnload();
        }

        internal void OnLoad()
        {
            GLControl.GetExtensions();
            SEDebug.Log(SEDebugState.Log, $"Created new SDL3 window[W{WindowPtr} | R{_RendererContext}]");
            SEDebug.Log(SEDebugState.Info, $"{GL.GetString(StringName.Version)} {GL.GetString(StringName.Renderer)} GL{GL.GetInteger(GetPName.MajorVersion)}.{GL.GetInteger(GetPName.MinorVersion)}");

            //GL.DebugMessageCallback(SEDebug.DebugMessageDelegate, IntPtr.Zero);
            //GL.Enable(EnableCap.DebugOutput);
            //GL.Enable(EnableCap.DebugOutputSynchronous);

            GL.Enable(EnableCap.DepthTest);  // GL 1.0
            GL.Enable(EnableCap.CullFace);  // GL 1.0
            GL.Enable(EnableCap.Blend);  // GL 1.0

           // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);  // GL 1.0
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);  // GL 1.0
           // GL.BlendFunc(0, BlendingFactorSrc.One, BlendingFactorDest.One);  // GL 1.0
           // GL.BlendFunc(1, BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcAlpha);  // GL 1.0
            GL.BlendEquation(BlendEquationMode.FuncAdd);  // GL 1.4
                
           // Move it somewhere else
            GL.VertexAttrib4f((int)ShaderLayoutLocation.aColor, 0.5f, 0.5f, 0.0f, 1f);  // GL 2.0
            GL.VertexAttrib3f((int)ShaderLayoutLocation.aNormal, 1f, 1f, 1f);  // GL 2.0
            GL.VertexAttrib2f((int)ShaderLayoutLocation.aTexCoord, 0f, 0f);  // GL 2.0

            StbImage.stbi_set_flip_vertically_on_load(1); // Automatic texture flip
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            SEDebug.Flush();

            BehaviourManager.StartCall();
            OnResize();
        }

        internal void OnRenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);  // GL 1.0
            BehaviourManager.FrameLogicCall();
        }

        internal void FixedUpdate()
        {
            BehaviourManager.FixedUpdateCall(new OpenTK.Windowing.Common.FrameEventArgs(Time.DeltaTimeD));
        }

        internal void OnResize()
        {
            SDL.GetWindowSize(WindowPtr, out int width, out int height);

            GL.Viewport(0, 0, width, height);
            BehaviourManager.ExposeResolution(width, height);
            BehaviourManager.ResizeCall(new OpenTK.Windowing.Common.ResizeEventArgs(width, height));

            SEDebug.Log(SEDebugState.Info, $"Window resized -- {width}x{height}");
        }

        internal void OnUnload()
        {
            BehaviourManager.ExitCall();

            SDL.GLDestroyContext(_RendererContext);
            SDL.DestroyWindow(WindowPtr);
            SDL.Quit();
        }
    }
}