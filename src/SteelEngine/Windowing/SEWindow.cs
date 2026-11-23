using Hardware.Info;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SDL3;
using SteelEngine.AssetLoader;
using SteelEngine.Core;
using SteelEngine.Utils;
using SteelMotion.SteelMotion_Data.scripts;
using SteelWorks;
using System.Diagnostics;

namespace SteelEngine.Windowing
{
    public class SEWindow
    {
       // public readonly Window window;
        internal nint WindowPtr;
        internal uint WindowID;
        internal nint Renderer;

        public Cursor cursor { get; }

        public string Title { get; internal set; }
        public string Identificator { get; }
        public bool Fullscreen { get; internal set; }
        public bool Maximized { get; internal set; }
        public bool Minimized { get; internal set; }
        public bool Borderless { get; internal set; }
        public ContextAPI ContextAPI { get; }
        SDL.WindowFlags WindowFlags { get; set; }

        public Vector2i WindowPosition { get; internal set; }
        public int PositionX => WindowPosition.X;
        public int PositionY => WindowPosition.Y;

        public Vector2i WindowSize { get; internal set; }
        public int Width => WindowSize.X;
        public int Height => WindowSize.Y;

        public Vector2i MinWindowSize { get; internal set; }
        public int MinWidth => MinWindowSize.X;
        public int MinHeight => MinWindowSize.Y;

        public Vector2i MaxWindowSize { get; internal set; }
        public int MaxWidth => MaxWindowSize.X;
        public int MaxHeight => MaxWindowSize.Y;
        
       // private readonly Stopwatch _FrameTimeStopwatch = new();
        readonly FPSCounter CPU_FPS = new();
        public SEWindow(string identificator, SEWindowSettings settings)
        {
            OpenTK.Graphics.GLLoader.LoadBindings(new SDLBindingContext());
            SEDebug.Init();
            Monitors.Init();
            
            Title = settings.Title;
            Identificator = identificator;
            WindowSize = settings.WindowSize;
            ContextAPI = settings.Context;
            WindowFlags = (SDL.WindowFlags)settings.WindowFlags;

            Display display = Monitors.primaryDisplay;
            if (settings.WindowSize == new Vector2i()) WindowSize = new(display.Width >> 1, display.Height >> 1);
            if (settings.MinimumWindowSize == new Vector2i()) MinWindowSize = new(display.Width >> 3, display.Height >> 3);
            if (settings.MaximumWindowSize == new Vector2i()) MaxWindowSize = new(display.Width, display.Height);

            if (!SDL.GLSetAttribute(SDL.GLAttr.DoubleBuffer, 1)) SDLDebug.GetError();
            if (!SDL.GLSetAttribute(SDL.GLAttr.DepthSize, 24)) SDLDebug.GetError();

            WindowPtr = SDL.CreateWindow(Title, Width, Height, (SDL.WindowFlags)ContextAPI | (SDL.WindowFlags)WindowFlags);
            WindowID = SDL.GetWindowID(WindowPtr);

            if (!SDL.SetWindowMinimumSize(WindowPtr, MinWidth, MinHeight)) SDLDebug.GetError();
            if (!SDL.SetWindowMaximumSize(WindowPtr, MaxWidth, MaxHeight)) SDLDebug.GetError();
            if (!SDL.SetHint(SDL.Hints.MouseRelativeModeCenter, "0")) SDLDebug.GetError();
           // cursor = new(WindowPtr);

            WindowManager.RegisterWindow(this);
        }

        public void Run()
        {
            bool loop = true;
            double timeSinceLastFixedUpdate = 0;
            ulong runtime, lastTime = 0;

            try
            {
                Renderer = SDL.GLCreateContext(WindowPtr);
                SDL.GLSetSwapInterval(0);
            
                Input.PrepareNewInputFrame();
                while (SDL.PollEvent(out SDL.Event @event))
                {
                    if (@event.Type == (uint)SDL.EventType.Quit) OnUnload();
                    if (@event.Type == (uint)SDL.EventType.WindowFocusGained) WindowManager.currentWindowFocus = this;
                    if (@event.Type == (uint)SDL.EventType.WindowResized) OnResize();
                    if (@event.Type == (uint)SDL.EventType.WindowEnterFullscreen) Fullscreen = true;
                    if (@event.Type == (uint)SDL.EventType.WindowLeaveFullscreen) Fullscreen = false;
                    if (@event.Type == (uint)SDL.EventType.WindowMaximized) Maximized = true;
                    if (@event.Type == (uint)SDL.EventType.WindowMaximized) Maximized = true;
                    else Maximized = false;
                    if (@event.Type == (uint)SDL.EventType.WindowMinimized) Minimized = true;
                    else Minimized = false;

                    Input.ProcessEvents(@event);
                }
                WindowFlags = SDL.GetWindowFlags(WindowPtr);
                
                OnLoad();

                while (loop)
                {
                    runtime = SDL.GetTicks();
                    Time.DeltaTimeD = (float)((runtime-lastTime)/1000d);
                    timeSinceLastFixedUpdate += Time.DeltaTimeD;

                    Input.PrepareNewInputFrame();

                    while (SDL.PollEvent(out SDL.Event @event))
                    {
                        if (@event.Type == (uint)SDL.EventType.Quit) loop = false;
                        if (@event.Type == (uint)SDL.EventType.WindowResized) OnResize();
                        if (@event.Type == (uint)SDL.EventType.WindowEnterFullscreen) Fullscreen = true;
                        if (@event.Type == (uint)SDL.EventType.WindowLeaveFullscreen) Fullscreen = false;
                        if (@event.Type == (uint)SDL.EventType.WindowMinimized) Maximized = false;
                        if (@event.Type == (uint)SDL.EventType.WindowMaximized) Maximized = true;
                        if (@event.Type == (uint)SDL.EventType.WindowMoved) OnWindowPosChange();

                        Input.ProcessEvents(@event);
                    }

                   // Renderer.PrepGBuffer();
                    OnRenderFrame();
                    if (timeSinceLastFixedUpdate >= Physics.updateTimestep)
                    {
                        FixedUpdate();
                        timeSinceLastFixedUpdate -= Physics.updateTimestep;
                    }
                    
                    SDL.GLSwapWindow(WindowPtr);  // End of the frame

                   // Prepare things for the next frame
                    lastTime = runtime;
                    CPU_FPS.Count();
                    SEDebug.Flush();
                }
        
                OnUnload();
            }
            catch (Exception ex)
            {
                SEDebug.Log(SEDebugState.Error, $"Exception: {ex.Message}\n{ex.StackTrace}\n{ex.Data}");
                Close();
            }
        }

        internal void OnLoad()
        {
            GetHardwareInfo();
            GLControl.GetExtensions();
            SEDebug.Log(SEDebugState.Log, $"Created new SDL3 window[W{WindowPtr} | R{Renderer}]");

            GL.DebugMessageCallback(SEDebug.DebugMessageDelegate, IntPtr.Zero);
           // GL.Enable(EnableCap.DebugOutput);  // causes an error???
            GL.Enable(EnableCap.DebugOutputSynchronous);

            GL.Enable(EnableCap.DepthTest);  // GL 1.0
            GL.Enable(EnableCap.CullFace);  // GL 1.0
            GL.Enable(EnableCap.Blend);  // GL 1.0

           // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);  // GL 1.0
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);  // GL 1.0
           // GL.BlendFunc(0, BlendingFactorSrc.One, BlendingFactorDest.One);  // GL 1.0
           // GL.BlendFunc(1, BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcAlpha);  // GL 1.0
            GL.BlendEquation(BlendEquationMode.FuncAdd);  // GL 1.4

           // Renderer.InitGBuffer();
           // GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            OnWindowPosChange();
            BehaviourManager.StartCall();
            OnResize();
            SEDebug.Flush();
        }

        internal void OnRenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);  // GL 1.0
            BehaviourManager.FrameLogicCall();
        }

        internal void OnWindowPosChange()
        {
            SDL.GetWindowPosition(WindowPtr, out int x, out int y);
            WindowPosition = (x, y);

            BehaviourManager.WindowPosChange(WindowPosition);
        }

        internal void FixedUpdate()
        {
            BehaviourManager.FixedUpdateCall();
        }

        internal void OnResize()
        {
            SDL.GetWindowSize(WindowPtr, out int width, out int height);

            GL.Viewport(0, 0, width, height);
            BehaviourManager.ExposeResolution(width, height);
            BehaviourManager.ResizeCall();

            SEDebug.Log(SEDebugState.Info, $"Window resized to {width}x{height}");
        }

        internal void OnUnload()
        {
            BehaviourManager.ExitCall();
            Close();
        }

        public void Close()
        {
            SDL.GLDestroyContext(Renderer);
            SDL.DestroyWindow(WindowPtr);
            SDL.Quit();
        }

        public void CursorState(CursorState state)
        {
            SDL.SetWindowRelativeMouseMode(WindowPtr, state!=0);

            SDL.SetHint(SDL.Hints.MouseRelativeCursorVisible, (state!=Windowing.CursorState.Grabbed) ? "1" : "0");
            SDL.SetHint(SDL.Hints.MouseRelativeModeCenter, (state==Windowing.CursorState.Grabbed) ? "1" : "0");
        }

        public bool IsFullscreen() => ((int)WindowFlags & (1 << 0)) == 1;
        public bool IsMaximized() => ((int)WindowFlags & (1 << 7)) == 1;
        public bool IsMinimized() => ((int)WindowFlags & (1 << 6)) == 1;
        public bool CheckState(WindowFlags flag) => ((int)WindowFlags & (int)flag) == 1;
        public void ChangeState(WindowState state)
        {
            bool borderless = (state & WindowState.Borderless) == WindowState.Borderless;
            bool minimized = (state & WindowState.Minimized) == WindowState.Minimized;
            bool fullscreen = (state & WindowState.Fullscreen) == WindowState.Fullscreen;

            SDL.SetWindowBordered(WindowPtr, !borderless);
            SDL.SetWindowFullscreen(WindowPtr, fullscreen);
            if (minimized) SDL.MinimizeWindow(WindowPtr);
        }

        public void SetIcon(string path)
        {
            var icon = TextureLoader.LoadTexture(path);
            
            fixed (byte* p = icon.imageBytes)
            {
                var windowIcon = SDL.CreateSurfaceFrom(icon.width, icon.height, SDL.PixelFormat.RGBA8888, (nint)p, icon.width*3);
                SDL.SetWindowIcon(WindowPtr, windowIcon);
            }
        }

        public void SetTitle(string title)
        {
            Title = title;
            SDL.SetWindowTitle(WindowPtr, title);
        }

        public string GetTitle() => Title;

        public void MoveBy(Vector2i moveVector)
        {
            SDL.SetWindowPosition(WindowPtr, PositionX + moveVector.X, PositionY + moveVector.Y);
        }

        public void MoveTo(Vector2i newPosition)
        {
            SDL.SetWindowPosition(WindowPtr, newPosition.X, newPosition.Y);
        }

        void GetHardwareInfo()  // move somewhere else, it will launch every time new SEWindow is ran
        {
            var hwInfo = new HardwareInfo();
            hwInfo.RefreshCPUList(includePercentProcessorTime: false, includePerformanceCounter: false);
            hwInfo.RefreshVideoControllerList(refreshMonitorList: false);
            hwInfo.RefreshMemoryList();
            hwInfo.RefreshOperatingSystem();
            hwInfo.RefreshSoundDeviceList();

            var OSinfo = hwInfo.OperatingSystem;
            var CPUInfo = hwInfo.CpuList;
            var GPUInfo = hwInfo.VideoControllerList;
            var MemInfo = hwInfo.MemoryList;
            var SDInfo = hwInfo.SoundDeviceList;

            string allCPUs = "";
            string allGPUs = "";
            string allRAM = "";
            string allSD = "";

            if (CPUInfo.Count == 1)  allCPUs = CPUInfo[0].Name;
            else for (int i = 0; i < CPUInfo.Count; i++)  allCPUs += $"\n    - {CPUInfo[i].Name}";

            if (GPUInfo.Count == 1)  allGPUs = $"{GPUInfo[0].Name} {float.Round((float)GPUInfo[0].AdapterRAM / (1 << 30))}GB";
            else for (int i = 0; i < GPUInfo.Count; i++)  allGPUs += $"\n    - {GPUInfo[i].Name} {float.Round((float)GPUInfo[i].AdapterRAM / (1 << 30))}";

            if (MemInfo.Count == 1)  allRAM = $"{MemInfo[0].MemoryType} {MemInfo[0].Speed*1000000} {MemInfo[0].Capacity / (1 << 30)}";
            else for (int i = 0; i < MemInfo.Count; i++)  allRAM += $"\n    - {MemInfo[i].MemoryType} {(float)MemInfo[i].Speed/1000}0Mhz {MemInfo[i].Capacity / (1 << 30)}GB {MemInfo[i].BankLabel}";
            
            if (SDInfo.Count == 1)  allSD = $"{SDInfo[0].Manufacturer} - {SDInfo[0].ProductName}";
            else for (int i = 0; i < SDInfo.Count; i++)  allSD += $"\n    - {SDInfo[i].ProductName}";

            SEDebug.Log(SEDebugState.Info, $"""
                
                ========================= System Info =========================
                  OS: {OSinfo.Name} {OSinfo.VersionString}
                  CPU: {allCPUs}
                  GPU: {allGPUs}
                  RAM: {allRAM}
                  Sound: {allSD}
                  OpenGL:
                    - GL{GL.GetInteger(GetPName.MajorVersion)}.{GL.GetInteger(GetPName.MinorVersion)}
                    - GLSL{GL.GetString(StringName.ShadingLanguageVersion)}
                ===============================================================

                """);
        }
    }
}