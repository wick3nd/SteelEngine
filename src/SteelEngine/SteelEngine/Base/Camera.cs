using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteelEngine.SteelEngine.Base
{
    public class Camera : EngineScript
    {
        public float fieldOfView = 60f;

        public Matrix4 view;
        public Matrix4 projection;

        public Vector3 camPosition;
        public Vector3 camTarget;
        public Vector3 camDirection;
        public Vector3 camRight;
        public Vector3 camUp;

        private Vector3 _camFront;
        private Vector3 _up;

        private float _camYaw;
        private float _camPitch;
        public float speed = 2f;
        public readonly float sensitivity = 0.2f;
        public bool isCursorLocked = true;

        public Frustum frustum = new();

        public Camera()
        {
            _up = Vector3.UnitY;
            camPosition = new Vector3(0.0f, 0.0f, 3.0f);
            camTarget = new Vector3(0.0f, 0.0f, 0.0f);
            camDirection = Vector3.Normalize(camPosition - camTarget);
            camRight = Vector3.Normalize(Vector3.Cross(_up, camDirection));
            camUp = Vector3.Cross(camDirection, camRight);

            view = Matrix4.LookAt(
                new Vector3(0.0f, 0.0f, 3.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 1.0f, 0.0f)
            );

            _camFront = new Vector3(0.0f, 0.0f, -1.0f);
        }

        public override void Update(FrameEventArgs e)
        {
            base.Update(e);

            ProcessInput(WindowReference!.KeyboardState!,WindowReference.MouseState!, (float)DeltaTime);

            float aspectRatio = WindowWidth / (float)WindowHeight;

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), aspectRatio, 0.0001f, 10000f);
            view = Matrix4.LookAt(camPosition, camPosition + _camFront, _up);

            Matrix4 viewProj = view * projection;
            frustum.Update(viewProj);
        }

        public bool IsSphereVisible(Vector3 center, float radius)
        {
            for (int i = 0; i != frustum.planes.Length; i++)
            {
                if (frustum.planes[i].DistanceToPoint(center) < -radius) return false;
            }
            return true;
        }

        private void HandleKeyboard(KeyboardState input, float deltaTime)
        {
            float acceleration = speed * deltaTime;
            if (input.IsKeyDown(Keys.LeftControl)) acceleration *= 2f;

            if (input.IsKeyDown(Keys.W)) camPosition += _camFront * acceleration;    // Forward

            if (input.IsKeyDown(Keys.S)) camPosition -= _camFront * acceleration;    // Backward

            if (input.IsKeyDown(Keys.A)) camPosition -= Vector3.Normalize(Vector3.Cross(_camFront, _up)) * acceleration;    // Left

            if (input.IsKeyDown(Keys.D)) camPosition += Vector3.Normalize(Vector3.Cross(_camFront, _up)) * acceleration;    // Right

            if (input.IsKeyDown(Keys.Space)) camPosition += _up * acceleration;    // Up

            if (input.IsKeyDown(Keys.LeftShift)) camPosition -= _up * acceleration;    // Down
        }

        private void HandleMouse(MouseState mouse)
        {
            float deltaX = mouse.Delta.X;
            float deltaY = mouse.Delta.Y;

            _camYaw += deltaX * sensitivity;
            _camPitch -= deltaY * sensitivity;

            _camPitch = Math.Clamp(_camPitch, -89.9f, 89.9f);

            _camFront.X = (float)Math.Cos(MathHelper.DegreesToRadians(_camPitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(_camYaw));
            _camFront.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_camPitch));
            _camFront.Z = (float)Math.Cos(MathHelper.DegreesToRadians(_camPitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(_camYaw));

            _camFront.Normalize();
        }

        public void ProcessInput(KeyboardState input, MouseState mouse, float deltaTime)
        {
            if (input.IsKeyPressed(Keys.Escape))
            {
                isCursorLocked = !isCursorLocked;
                WindowReference!.CursorState = CursorState.Normal;
            }

            if (isCursorLocked)
            {
                WindowReference!.CursorState = CursorState.Grabbed;
                HandleKeyboard(input, deltaTime);
                HandleMouse(mouse);
            }
        }
    }
}
