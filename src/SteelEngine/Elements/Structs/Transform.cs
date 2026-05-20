using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace SteelEngine.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Transform(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        public Vector3 Pos { get; set; } = pos;
        public Quaternion QuatRot { get; set; } = rot;
        public Vector3 Scale { get; set; } = scale;
    }
}