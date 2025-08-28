using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SteelEngine;
using SteelEngine.Base;
using SteelMotion.SteelMotion_Data.assets.shaders;

namespace SteelMotion.SteelMotion_Data.scripts
{
    public class Program : WindowLoop
    {
        public Program() : base(Monitors.GetPrimaryMonitor().HorizontalResolution, Monitors.GetPrimaryMonitor().VerticalResolution, "SteelMotion") { }

        float shaderTime;

        Shader? STexture;
        Shader? SOneColor;

        Texture2D? TSkull;
        Texture2D? TSkull_glow;

        Mesh? MCube;

        private static void Main()
        {
            using var game = new Program();
            game.Run();
        }

        protected override void Start()
        {
            STexture = new();
            SOneColor = new(CustomShaders.oneColor);

            TSkull = new(@"SteelMotion_Data\assets\textures\IOS_cube.png");
            TSkull_glow = new(@"SteelMotion_Data\assets\textures\IOS_cube-glow.png", TextureUnit.Texture1);
            
            MCube = new(@"SteelMotion_Data\assets\model3.semp");
        }

        protected override void OnExit()
        {
            base.OnExit();

            STexture!.Dispose();
            SOneColor!.Dispose();
            
            TSkull!.Dispose();
            TSkull_glow!.Dispose();

            MCube!.Dispose();
        }

        protected override void Update(FrameEventArgs e)
        {
            base.Update(e);

            shaderTime += (float)e.Time;

            RenderLoop();
        }

        protected override void OnResize(FramebufferResizeEventArgs e)
        {
            base.OnResize(e);

            RenderLoop();
        }

        private void RenderLoop()
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), WindowRes.width / (float)WindowRes.height, 0.0001f, 1000f);        // camera perspective
            Matrix4 model = Matrix4.CreateRotationY(shaderTime) * Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(0f, -2f, -5f);
            
            STexture!.SetMatrix4("projection", projection);
            STexture!.SetMatrix4("model", model);

            TSkull!.Use();
            TSkull_glow!.Use();

            //SOneColor.SetFloat("time", shaderTime);
            STexture.SetFloat("time", shaderTime);

            //SOneColor.Use();
            STexture.Use();

            MCube!.Draw();
        }
    }
}