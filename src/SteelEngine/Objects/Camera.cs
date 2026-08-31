using OpenTK.Mathematics;
using SteelEngine.Core;
using System.Runtime.CompilerServices;

namespace SteelEngine.Objects
{
    public class Camera : EngineScript
    {
        public float FarPlaneDist = 100f;
        public float NearPlaneDist = 0.001f;
        public float FieldOfView = 60f;

        internal Matrix4 view;
        internal Matrix4 projection;

        public Vector3 Pos = Vector3.Zero;
        public Vector3 CamRight;
        public Vector3 CamUp;
        public Vector3 CamFront;

        public float CamYaw;
        public float CamPitch;

        internal Frustum frustum = new();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Camera(string name)
        {
            CameraSystem.Add(name, this);

            CamFront = new Vector3(0.0f, 0.0f, -1.0f);
            CamRight = Vector3.Normalize(Vector3.Cross(CamFront, Vector3.UnitY));
            CamUp = Vector3.Normalize(Vector3.Cross(CamRight, CamFront));

            projection = Matrix4.CreatePerspectiveFieldOfView(FieldOfView * MathHelper.DegToRad, (float)windowWidth / windowHeight, NearPlaneDist, FarPlaneDist);
            view = Matrix4.LookAt(Pos, Pos + CamFront, CamUp);

            frustum.camView = view;
            frustum.camProj = projection;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Update()
        {
            float aspectRatio = (float)windowWidth / windowHeight;
            float camPitchRad = CamPitch * MathHelper.DegToRad;
            float camYawRad = CamYaw * MathHelper.DegToRad;

            CamFront = new Vector3(
                MathF.Cos(camPitchRad) * MathF.Cos(camYawRad),
                MathF.Sin(camPitchRad),
                MathF.Cos(camPitchRad) * MathF.Sin(camYawRad)
            ).Normalized();
            CamRight = Vector3.Normalize(Vector3.Cross(CamFront, Vector3.UnitY));
            CamUp = Vector3.Normalize(Vector3.Cross(CamRight, CamFront));

            projection = Matrix4.CreatePerspectiveFieldOfView(FieldOfView * MathHelper.DegToRad, aspectRatio, NearPlaneDist, FarPlaneDist);
            view = Matrix4.LookAt(Pos, Pos + CamFront, CamUp);

            frustum.camView = view;
            frustum.camProj = projection;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RenderTo(string name) => CameraSystem.Use(name);

        public bool IsSphereVisible(Vector3 center, float radius)
        {
            for (int i = 0; i != 6; i++)
            {
                if (frustum.planes[i].DistanceToPoint(center) < -radius) return false;
            }
            return true;
        }
    }
}