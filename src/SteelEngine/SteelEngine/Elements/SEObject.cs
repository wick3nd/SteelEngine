using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Base;
using SteelEngine.EngineBase.EngineBehaviour;
using SteelEngine.Utils;

namespace SteelEngine.Elements
{
    internal class SEObject : EngineScript
    {
        public Mesh? Mesh { get; set; }
        public Material? Material { get; set; }
        public Vector3 Pos { get; set; } = (0f, 0f, 0f);
        public Vector3 Rot { get; set; } = (0f, 0f, 0f);
        public Vector3 Scale { get; set; } = (1f, 1f, 1f);

        public void Draw(PrimitiveType type = PrimitiveType.Triangles)
        {
            Material!.Use();

            Material?.Shader?.SetMatrix4("model", RecalculatedModelMatrix());
            Material?.Shader?.SetMatrix4("view", CameraSystem.chosenCamera!.view);
            Material?.Shader?.SetMatrix4("projection", CameraSystem.chosenCamera!.projection);

            Mesh!.Draw(type);
        }

        public void DrawInstanced(Matrix4[] instanceMatrix, PrimitiveType type = PrimitiveType.Triangles)
        {
            Material!.Use();

            Material?.Shader?.SetMatrix4("view", CameraSystem.chosenCamera!.view);
            Material?.Shader?.SetMatrix4("projection", CameraSystem.chosenCamera!.projection);

            Mesh!.DrawInstanced(instanceMatrix, type);
        }

        public Matrix4 RecalculatedModelMatrix() => Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(SEMath.CreateQuatRotation(Rot.X, Rot.Y, Rot.Z)) * Matrix4.CreateTranslation(Pos);
    }
}