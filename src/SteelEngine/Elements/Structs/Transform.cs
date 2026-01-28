using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    public struct Transform
    {
        public Vector3 Pos { get; set; } = new(0f, 0f, 0f);
        public Quaternion QuatRot { get; set; } = new(0f, 0f, 0f, 1f);
        public Vector3 Scale { get; set; } = new(1f, 1f, 1f);
        internal Matrix4 modelMatrix;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Transform(Vector3 pos, Vector3 scale, Quaternion rot)
        {
            Pos = pos;
            Scale = scale;
            QuatRot = rot;

            modelMatrix = RecalculatedModelMatrix();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Matrix4 RecalculatedModelMatrix() => Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(QuatRot) * Matrix4.CreateTranslation(Pos);
    }
}