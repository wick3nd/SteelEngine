using OpenTK.Mathematics;

namespace SteelMotion.SteelEngine.Math
{
    public enum AxisOrder
    {
        XYZ,
        YXZ,
        ZYX,
        YX,
        XY
    }

    public static class SEMath
    {
        /// <summary>
        /// Calculates a <see cref="Quaternion"/> rotation in form of a <see cref="Matrix4"/> from 3 given <see cref="float"/> values.
        /// </summary>
        /// <param name="xRot">Rotation around the X axis, in radians.</param>
        /// <param name="yRot">Rotation around the Y axis, in radians.</param>
        /// <param name="zRot">Rotation around the Z axis, in radians.</param>
        /// <param name="rotAxes">Affects the order of axis multiplication.</param>
        /// <returns>A <see cref="Quaternion"/> rotation represented as a <see cref="Matrix4"/>.</returns>
        public static Matrix4 CreateMat4QuatRotation(float xRot = 0, float yRot = 0, float zRot = 0, AxisOrder rotAxes = AxisOrder.YXZ)
        {
            Quaternion XQuat = Quaternion.FromAxisAngle(Vector3.UnitX, xRot);
            Quaternion YQuat = Quaternion.FromAxisAngle(Vector3.UnitY, yRot);
            Quaternion ZQuat = Quaternion.FromAxisAngle(Vector3.UnitZ, zRot);

            Quaternion rotationQuat = Quaternion.Identity;  // Temporary assignation

            switch (rotAxes)
            {
                case AxisOrder.XYZ: rotationQuat = XQuat * YQuat * ZQuat;
                    break;

                case AxisOrder.YXZ: rotationQuat = YQuat * XQuat * ZQuat;
                    break;

                case AxisOrder.ZYX: rotationQuat = ZQuat * YQuat * XQuat;
                    break;

                case AxisOrder.YX: rotationQuat = YQuat * XQuat;
                    break;

                case AxisOrder.XY: rotationQuat = XQuat * YQuat;
                    break;

                default: break;
            }

            Quaternion normalizedQuat = Quaternion.Normalize(rotationQuat);
            Matrix4 quatRotationMatrix = Matrix4.CreateFromQuaternion(normalizedQuat);

            return quatRotationMatrix;
        }

        /// <summary>
        /// Calculates a <see cref="Quaternion"/> rotation in form of a <see cref="Matrix4"/> from a <see cref="Vector3"/> (in radians).
        /// </summary>
        /// <param name="rotation">Rotation vector, angles represented in radians.</param>
        /// <param name="rotAxes">Affects the order of axis multiplication.</param>
        /// <returns>A <see cref="Quaternion"/> rotation represented as a <see cref="Matrix4"/>.</returns>
        public static Matrix4 CreateMat4QuatRotation(Vector3 rotation, AxisOrder rotAxes = AxisOrder.YXZ)
        {
            Quaternion XQuat = Quaternion.FromAxisAngle(Vector3.UnitX, rotation.X);
            Quaternion YQuat = Quaternion.FromAxisAngle(Vector3.UnitY, rotation.Y);
            Quaternion ZQuat = Quaternion.FromAxisAngle(Vector3.UnitZ, rotation.Z);

            Quaternion rotationQuat = Quaternion.Identity;  // Temporary assignation

            switch (rotAxes)
            {
                case AxisOrder.XYZ: rotationQuat = XQuat * YQuat * ZQuat;
                    break;

                case AxisOrder.YXZ: rotationQuat = YQuat * XQuat * ZQuat;
                    break;

                case AxisOrder.ZYX: rotationQuat = ZQuat * YQuat * XQuat;
                    break;

                case AxisOrder.YX: rotationQuat = YQuat * XQuat;
                    break;

                case AxisOrder.XY: rotationQuat = XQuat * YQuat;
                    break;

                default: break;
            }

            Quaternion normalizedQuat = Quaternion.Normalize(rotationQuat);
            Matrix4 quatRotationMatrix = Matrix4.CreateFromQuaternion(normalizedQuat);

            return quatRotationMatrix;
        }
    }
}
