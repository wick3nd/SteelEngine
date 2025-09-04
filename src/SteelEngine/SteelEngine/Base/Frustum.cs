using OpenTK.Mathematics;

namespace SteelEngine.SteelEngine.Base
{
    public class Frustum
    {
        public Plane[] planes = new Plane[6];

        public void Update(Matrix4 viewProjection)
        {
            // lewa
            planes[0] = new Plane(
                viewProjection.M14 + viewProjection.M11,
                viewProjection.M24 + viewProjection.M21,
                viewProjection.M34 + viewProjection.M31,
                viewProjection.M44 + viewProjection.M41
            );

            // prawa
            planes[1] = new Plane(
                viewProjection.M14 - viewProjection.M11,
                viewProjection.M24 - viewProjection.M21,
                viewProjection.M34 - viewProjection.M31,
                viewProjection.M44 - viewProjection.M41
            );

            // dolna
            planes[2] = new Plane(
                viewProjection.M14 + viewProjection.M12,
                viewProjection.M24 + viewProjection.M22,
                viewProjection.M34 + viewProjection.M32,
                viewProjection.M44 + viewProjection.M42
            );

            // gorna
            planes[3] = new Plane(
                viewProjection.M14 - viewProjection.M12,
                viewProjection.M24 - viewProjection.M22,
                viewProjection.M34 - viewProjection.M32,
                viewProjection.M44 - viewProjection.M42
            );

            // ta bliska
            planes[4] = new Plane(
                viewProjection.M14 + viewProjection.M13,
                viewProjection.M24 + viewProjection.M23,
                viewProjection.M34 + viewProjection.M33,
                viewProjection.M44 + viewProjection.M43
            );

            // ta z tylu
            planes[5] = new Plane(
                viewProjection.M14 - viewProjection.M13,
                viewProjection.M24 - viewProjection.M23,
                viewProjection.M34 - viewProjection.M33,
                viewProjection.M44 - viewProjection.M43
            );

            for (int i = 0; i < 6; i++) planes[i].Normalize();
        }
    }

    public struct Plane(float a, float b, float c, float d)
    {
        public Vector3 Normal = new(a, b, c);

        public void Normalize()
        {
            float length = Normal.Length;
            Normal /= length;
            d /= length;
        }

        public readonly float DistanceToPoint(Vector3 point) => Vector3.Dot(Normal, point) + D;
    }

}
