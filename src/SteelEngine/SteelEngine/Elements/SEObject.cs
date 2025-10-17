using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Base;

namespace SteelEngine.Elements
{
    internal class SEObject
    {
        public Mesh? Mesh { get; set; }
        public Material? Material { get; set; }

        public void Draw(Camera camera, Matrix4 transform, PrimitiveType type = PrimitiveType.Triangles)
        {
            Material!.Use();

            Material?.Shader?.SetMatrix4("model", transform);
            Material?.Shader?.SetMatrix4("view", camera.view);
            Material?.Shader?.SetMatrix4("projection", camera.projection);

            Mesh!.Draw(type);
        }

        public void DrawInstanced(Camera camera, Matrix4[] instanceMatrix, PrimitiveType type = PrimitiveType.Triangles)
        {
            Material!.Use();

            Material?.Shader?.SetMatrix4("view", camera.view);
            Material?.Shader?.SetMatrix4("projection", camera.projection);

            Mesh!.DrawInstanced(instanceMatrix, type);
        }
    }
}

