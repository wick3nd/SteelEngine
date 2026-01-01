using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SteelEngine.EngineBase.EngineBehaviour;
using SteelEngine.EngineBase.Structs;

namespace SteelEngine.Elements
{
    public class Camera : EngineScript
    {
        public float FarPlaneDist = 100f;
        public float NearPlaneDist = 0.001f;
        public float FieldOfView = 60f;

        internal Matrix4 view;
        internal Matrix4 projection;

        public Vector3 CamTarget { get; } = Vector3.Zero;
        public Vector3 CamDirection { get; }

        public Vector3 Pos = Vector3.Zero;
        public Vector3 CamRight;
        public Vector3 CamUp;
        public Vector3 CamFront;

        public float CamYaw;
        public float CamPitch;

        internal Frustum frustum = new();

        public Camera(string name)
        {
            CameraSystem.Add(name, this);

            CamDirection = Vector3.Normalize(Pos - CamTarget);
            CamRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, CamDirection));
            CamUp = Vector3.Cross(CamDirection, CamRight);

            view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
                                  new Vector3(0.0f, 0.0f, 0.0f),
                                  new Vector3(0.0f, 1.0f, 0.0f));

            CamFront = new Vector3(0.0f, 0.0f, -1.0f);

            frustum.camView = view;
            frustum.camProj = projection;
        }

        public override void Update(FrameEventArgs e)
        {
            base.Update(e);

            float aspectRatio = windowWidth / (float)windowHeight;
            float camPitchRad = CamPitch * MathHelper.DegToRad;
            float camYawRad = CamYaw * MathHelper.DegToRad;

            CamFront = new Vector3(
                MathF.Cos(camPitchRad) * MathF.Cos(camYawRad),
                MathF.Sin(camPitchRad),
                MathF.Cos(camPitchRad) * MathF.Sin(camYawRad)
            ).Normalized();

            projection = Matrix4.CreatePerspectiveFieldOfView(FieldOfView * MathHelper.DegToRad, aspectRatio, NearPlaneDist, FarPlaneDist);
            view = Matrix4.LookAt(Pos, Pos + CamFront, Vector3.UnitY);
            frustum.camView = view;
            frustum.camProj = projection;
        }

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