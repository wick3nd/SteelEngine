using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteelEngine.SteelEngine.Base
{
    public class Camera
    {

        public Matrix4 view;
        public Matrix4 projection;

        public Vector3 CamPosition;
        public Vector3 CamTarget;
        public Vector3 CamDirection;
        public Vector3 CamRight;
        public Vector3 CamUp;

        public Vector3 CamFront;
        public Vector3 Up;

        private float CamYaw;
        private float CamPitch;
        private float sensitivity = 0.2f;
        public const float Speed = 0.5f;

        public Frustum frustum = new Frustum();

        public Camera() 
        {
            Up = Vector3.UnitY;
            CamPosition = new Vector3(0.0f, 0.0f, 3.0f);
            CamTarget = new Vector3(0.0f, 0.0f, 0.0f);
            CamDirection = Vector3.Normalize(CamPosition - CamTarget);
            CamRight = Vector3.Normalize(Vector3.Cross(Up, CamDirection));
            CamUp = Vector3.Cross(CamDirection, CamRight);

            view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
             new Vector3(0.0f, 0.0f, 0.0f),
             new Vector3(0.0f, 1.0f, 0.0f));

            CamFront = new Vector3(0.0f, 0.0f, -1.0f);
        }

        public void Update(WindowRes windowRes)
        {
            float aspectRatio = windowRes.height > 0 ? windowRes.width / (float)windowRes.height : 1f;

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, aspectRatio, 0.1f, 100f);
            view = Matrix4.LookAt(CamPosition, CamPosition + CamFront, Up);
            Matrix4 viewProj = view * projection;

            frustum.Update(viewProj);
        }

        public bool IsSphereVisible(Vector3 center, float radius)
        {
            foreach (var plane in frustum.Planes)
            {
                if (plane.DistanceToPoint(center) < -radius)
                    return false;
            }
            return true;
        }

        private void HandleKeyboard(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.W))
            {
                CamPosition += CamFront * Speed; // pszut
            }

            if (input.IsKeyDown(Keys.S))
            {
                CamPosition -= CamFront * Speed; // tyl
            }

            if (input.IsKeyDown(Keys.A))
            {
                CamPosition -= Vector3.Normalize(Vector3.Cross(CamFront, Up)) * Speed; //lewo
            }

            if (input.IsKeyDown(Keys.D))
            {
                CamPosition += Vector3.Normalize(Vector3.Cross(CamFront, Up)) * Speed; //prawoMarcina
            }

            if (input.IsKeyDown(Keys.Space)) // gora
            {
                CamPosition += Up * Speed;
            }

            if (input.IsKeyDown(Keys.LeftShift)) // dol
            {
                CamPosition -= Up * Speed;
            }
        }

        private void HandleMouse(MouseState mouse)
        {
            float deltaX = mouse.Delta.X;
            float deltaY = mouse.Delta.Y;

            CamYaw += deltaX * sensitivity;
            CamPitch -= deltaY * sensitivity;

            CamPitch = Math.Clamp(CamPitch, -89f, 89f);

            CamFront.X = (float)Math.Cos(MathHelper.DegreesToRadians(CamPitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(CamYaw));
            CamFront.Y = (float)Math.Sin(MathHelper.DegreesToRadians(CamPitch));
            CamFront.Z = (float)Math.Cos(MathHelper.DegreesToRadians(CamPitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(CamYaw));
            CamFront.Normalize();
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
