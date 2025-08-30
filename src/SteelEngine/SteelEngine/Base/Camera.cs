using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteelEngine.SteelEngine.Base
{
    public class Camera
    {
        public float fieldOfView = 90f;

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
        public readonly float sensitivity = 0.2f;
        public float speed = 0.5f;

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

        public void Update(WindowRes windowRes)
        {
            float aspectRatio = windowRes.height > 0 ? windowRes.width / (float)windowRes.height : 1f;

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), aspectRatio, 0.1f, 100f);
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

        private void HandleKeyboard(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.W)) camPosition += _camFront * speed;    // Forward

            if (input.IsKeyDown(Keys.S)) camPosition -= _camFront * speed;    // Backward

            if (input.IsKeyDown(Keys.A)) camPosition -= Vector3.Normalize(Vector3.Cross(_camFront, _up)) * speed;    // Left

            if (input.IsKeyDown(Keys.D)) camPosition += Vector3.Normalize(Vector3.Cross(_camFront, _up)) * speed;    // Right

            if (input.IsKeyDown(Keys.Space)) camPosition += _up * speed;    // Up

            if (input.IsKeyDown(Keys.LeftShift))  camPosition -= _up * speed;    // Down
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

        public void ProcessKeyboardInput(KeyboardState input)
        {
            HandleKeyboard(input);
        }
        public void ProcessMouseInput(KeyboardState input, MouseState mouse, ref bool cursorLocked, Action setCursorNormal, Action setCursorGrabbed)
        {
            if (input.IsKeyPressed(Keys.Escape))
            {
                cursorLocked = false;
                setCursorNormal();
            }

            if (cursorLocked)
            {
                setCursorGrabbed();
                HandleMouse(mouse);
            }
        }

    }
}
