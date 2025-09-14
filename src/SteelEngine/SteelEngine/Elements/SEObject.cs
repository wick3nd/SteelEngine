using OpenTK.Mathematics;
using SteelEngine;
using SteelEngine.Base;

namespace SteelMotion.SteelEngine.Elements
{
    internal class SEObject : EngineScript
    {
        public Mesh? Mesh { get; set; }
        public Material? Material { get; set; }

        public void DrawInstanced(Matrix4[] instanceMatrix)
        {
            Material!.Use();
            Mesh!.DrawInstanced(instanceMatrix);
        }

        public override void OnExit()
        {
            Mesh?.Dispose();
        }
    }
}
