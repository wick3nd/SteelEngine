using OpenTK.Mathematics;
using SteelEngine.Elements;
using SteelEngine.Utils;

namespace SteelEngine
{
    public class Mesh
    {
        private readonly MeshMetadata _metadata;
        internal GLMesh mesh;
       // internal Shader Shader;
        public Transform transform;
        public bool IsVisible { get; private set; }
        public bool IsDestroyed { get; private set; }

        private Vector3 objectCenter;
        private Box3 boundingBox;
        private float radius;

        public Mesh(string path)
        {
            _metadata = new()
            {
                path = path,
                debugName = Path.GetRelativePath(Environment.CurrentDirectory, path)
            };
            mesh = new(_metadata.path, _metadata.debugName);
            boundingBox = CalculateBoundingBox();
            objectCenter = boundingBox.Center;
        }

        public Mesh(string path, string debugName)
        {
            _metadata = new()
            {
                path = path,
                debugName = debugName
            };
            mesh = new(_metadata.path, _metadata.debugName);
            boundingBox = CalculateBoundingBox();
            objectCenter = boundingBox.Center;
        }

        public void Transform(Vector3 position, Vector3 scale, Vector3 rotation)
        {
            transform = new()
            {
                Position = position,
                Scale = scale,
                Rotation = rotation
            };
        }
        public Transform GetNodeTransform(string nodeName)
        {
            bool isValuePresent = mesh.meshLoader.primitiveTransform.TryGetValue(nodeName, out Transform value);
            if (isValuePresent) return value;

            SEDebug.Log(SEDebugState.Debug, $"Couldn't find a node named {nodeName}");
            return new Transform();
        }
        public void Hidden(bool visible)
        {
            IsVisible = visible;
            mesh.render = visible;
        }
        public void Destroy()
        {
            if (!IsDestroyed)
            {
                mesh.Destroy();
                boundingBox = Box3.Zero;
                objectCenter = Vector3.Zero;
                radius = 0;
            }
        }
        public void Recreate()
        {
            if (IsDestroyed)
            {
                mesh = new(_metadata.path, _metadata.debugName);
                boundingBox = CalculateBoundingBox();
                objectCenter = boundingBox.Center;
               // radius = null; // <--
            }
        }

        Box3 CalculateBoundingBox()
        {
            return Box3.UnitCube;
        }
    }
}
