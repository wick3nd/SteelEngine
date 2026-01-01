using OpenTK.Graphics.OpenGL;

namespace SteelEngine.Base
{
    internal class Material
    {
        public Shader? Shader { get; set; }
        public Texture2D? Texture1 { get; set; }
        public Texture2D? Texture2 { get; set; }

        public void Use()
        {
            Shader!.Enable();

            int unit = 0;
            if (Texture1 != null)
            {
                Texture1.Bind((TextureUnit)((int)TextureUnit.Texture0 + unit));
                Shader.SetInt("texture0", unit);
                unit++;
            }
            if (Texture2 != null)
            {
                Texture2.Bind((TextureUnit)((int)TextureUnit.Texture0 + unit));
                Shader.SetInt("texture1", unit);
               // unit++;
            }
        }
    }
}