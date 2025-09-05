using OpenTK.Mathematics;

namespace SteelEngine.Base.Structs
{
    public readonly struct Plane(Vector4 vec)
    {
        private readonly Vector4 _vec = vec;

        public readonly Plane Normalize()
        {
            float len = _vec.Length;
            return new Plane(_vec / len);
        }

        public readonly float DistanceToPoint(Vector3 point) => Vector3.Dot(new Vector3(_vec.X, _vec.Y, _vec.Z), point) + _vec.W;
    }
}
