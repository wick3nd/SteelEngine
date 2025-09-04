using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SteelEngine.Base.Structs;

namespace SteelEngine.SteelEngine.Base
{
    public class Frustum : EngineScript
    {
        public Plane[] planes = new Plane[6];
        public Matrix4 viewProjection;

        public override void Update(FrameEventArgs e)
        {
            base.Update(e);

            // left plane
            planes[0] = new Plane(new Vector4(
                viewProjection.M14 + viewProjection.M11,
                viewProjection.M24 + viewProjection.M21,
                viewProjection.M34 + viewProjection.M31,
                viewProjection.M44 + viewProjection.M41
            )).Normalize();

            // right plane
            planes[1] = new Plane(new Vector4(
                viewProjection.M14 - viewProjection.M11,
                viewProjection.M24 - viewProjection.M21,
                viewProjection.M34 - viewProjection.M31,
                viewProjection.M44 - viewProjection.M41
            )).Normalize();

            // bottom plane
            planes[2] = new Plane(new Vector4(
                viewProjection.M14 + viewProjection.M12,
                viewProjection.M24 + viewProjection.M22,
                viewProjection.M34 + viewProjection.M32,
                viewProjection.M44 + viewProjection.M42
            )).Normalize();

            // upper plane
            planes[3] = new Plane(new Vector4(
                viewProjection.M14 - viewProjection.M12,
                viewProjection.M24 - viewProjection.M22,
                viewProjection.M34 - viewProjection.M32,
                viewProjection.M44 - viewProjection.M42
            )).Normalize();

            // near plane
            planes[4] = new Plane(new Vector4(
                viewProjection.M14 + viewProjection.M13,
                viewProjection.M24 + viewProjection.M23,
                viewProjection.M34 + viewProjection.M33,
                viewProjection.M44 + viewProjection.M43
            )).Normalize();

            // far plane
            planes[5] = new Plane(new Vector4(
                viewProjection.M14 - viewProjection.M13,
                viewProjection.M24 - viewProjection.M23,
                viewProjection.M34 - viewProjection.M33,
                viewProjection.M44 - viewProjection.M43
            )).Normalize();
        }
    }
}
