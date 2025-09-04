using OpenTK.Mathematics;

namespace SteelEngine.SteelEngine.Base
{
    public class Frustum
    {
        public Plane[] planes = new Plane[6];

        public void Update(Matrix4 viewProjection)    // use EngineScript update
        {
            // left plane
            planes[0] = new Plane(
                viewProjection.M14 + viewProjection.M11,
                viewProjection.M24 + viewProjection.M21,
                viewProjection.M34 + viewProjection.M31,
                viewProjection.M44 + viewProjection.M41
            ).Normalize();

            // right plane
            planes[1] = new Plane(
                viewProjection.M14 - viewProjection.M11,
                viewProjection.M24 - viewProjection.M21,
                viewProjection.M34 - viewProjection.M31,
                viewProjection.M44 - viewProjection.M41
            ).Normalize();

            // lower plane
            planes[2] = new Plane(
                viewProjection.M14 + viewProjection.M12,
                viewProjection.M24 + viewProjection.M22,
                viewProjection.M34 + viewProjection.M32,
                viewProjection.M44 + viewProjection.M42
            ).Normalize();

            // upper plane
            planes[3] = new Plane(
                viewProjection.M14 - viewProjection.M12,
                viewProjection.M24 - viewProjection.M22,
                viewProjection.M34 - viewProjection.M32,
                viewProjection.M44 - viewProjection.M42
            ).Normalize();

            // near plane
            planes[4] = new Plane(
                viewProjection.M14 + viewProjection.M13,
                viewProjection.M24 + viewProjection.M23,
                viewProjection.M34 + viewProjection.M33,
                viewProjection.M44 + viewProjection.M43
            ).Normalize();

            // far plane
            planes[5] = new Plane(
                viewProjection.M14 - viewProjection.M13,
                viewProjection.M24 - viewProjection.M23,
                viewProjection.M34 - viewProjection.M33,
                viewProjection.M44 - viewProjection.M43
            ).Normalize();
        }
    }

    public struct Plane(float a, float b, float c, float d)    // use more informative names perhaps?
    {
        public Vector3 Normal = new(a, b, c);
        float D = d;

        public void Normalize()
        {
            float length = Normal.Length;    // its a fixed size - put it in struct to not make new one evey time normalize is called
            Normal /= length;
            D /= length;
        }

        public readonly float DistanceToPoint(Vector3 point) => Vector3.Dot(Normal, point) + D;
    }
}
