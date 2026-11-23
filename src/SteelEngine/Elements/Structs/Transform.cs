using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace SteelEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Transform()
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;  // store as 3 angles, later use in euler/quaternion
        public Quaternion QRotation { get; set; } = Quaternion.Zero;  // delete later
        public Vector3 Scale { get; set; } = Vector3.One;

        public Transform(Vector3 pos, Vector3 scale, Vector3 rot) : this()
        {
            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
    }
}