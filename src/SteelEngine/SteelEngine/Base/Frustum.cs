using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SteelEngine.Base.Structs;

namespace SteelEngine.SteelEngine.Base
{
    public class Frustum : EngineScript
    {
        public Plane[] planes = new Plane[6];

        public override void Update(FrameEventArgs e)
        {
            base.Update(e);
            
            Matrix4 _viewProjection = Camera.view * Camera.projection;

            // left plane
            planes[0] = new Plane(new Vector4(
                _viewProjection.M14 + _viewProjection.M11,
                _viewProjection.M24 + _viewProjection.M21,
                _viewProjection.M34 + _viewProjection.M31,
                _viewProjection.M44 + _viewProjection.M41
            )).Normalize();

            // right plane
            planes[1] = new Plane(new Vector4(
                _viewProjection.M14 - _viewProjection.M11,
                _viewProjection.M24 - _viewProjection.M21,
                _viewProjection.M34 - _viewProjection.M31,
                _viewProjection.M44 - _viewProjection.M41
            )).Normalize();

            // bottom plane
            planes[2] = new Plane(new Vector4(
                _viewProjection.M14 + _viewProjection.M12,
                _viewProjection.M24 + _viewProjection.M22,
                _viewProjection.M34 + _viewProjection.M32,
                _viewProjection.M44 + _viewProjection.M42
            )).Normalize();

            // upper plane
            planes[3] = new Plane(new Vector4(
                _viewProjection.M14 - _viewProjection.M12,
                _viewProjection.M24 - _viewProjection.M22,
                _viewProjection.M34 - _viewProjection.M32,
                _viewProjection.M44 - _viewProjection.M42
            )).Normalize();

            // near plane
            planes[4] = new Plane(new Vector4(
                _viewProjection.M14 + _viewProjection.M13,
                _viewProjection.M24 + _viewProjection.M23,
                _viewProjection.M34 + _viewProjection.M33,
                _viewProjection.M44 + _viewProjection.M43
            )).Normalize();

            // far plane
            planes[5] = new Plane(new Vector4(
                _viewProjection.M14 - _viewProjection.M13,
                _viewProjection.M24 - _viewProjection.M23,
                _viewProjection.M34 - _viewProjection.M33,
                _viewProjection.M44 - _viewProjection.M43
            )).Normalize();
        }
    }
}
