using OpenTK.Graphics.OpenGL4;
using SteelEngine;
using SteelEngine.Base;
using SteelEngine.SteelEngine.Base;

namespace SteelMotion.SteelEngine.Elements
{
    internal class Material : EngineScript
    {
        public Shader? Shader { get; set; }
        public Texture2D? Tex1 { get; set; }
        public Texture2D? Tex2 { get; set; }

        public void Use()
        {
            Shader!.Use();

            Shader.SetMatrix4("projection", Camera.projection);
            Shader.SetMatrix4("view", Camera.view);

            int unit = 0;
            if (Tex1 != null)
            {
                Tex1.Use(TextureUnit.Texture0 + unit);
                Shader.SetInt("texture0", unit);
                unit++;
            }
            if (Tex2 != null)
            {
                Tex2.Use(TextureUnit.Texture0 + unit);
                Shader.SetInt("texture1", unit);
                unit++;
            }
        }

        public override void OnExit()
        {
            Shader!.Dispose();
            Tex1?.Dispose();
            Tex2?.Dispose();
        }
    }

}
