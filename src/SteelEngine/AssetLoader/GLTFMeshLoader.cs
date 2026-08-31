using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using SteelEngine.Core;
using SteelEngine.Core.Buffers;
using SteelEngine.EngineBase.Structs;
using SteelEngine.Utils;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SteelEngine.AssetLoader
{
    public class GLTFMeshLoader
    {
        internal MeshContainer mesh;
        private readonly List<SubMesh> _submeshes = [];
        public Dictionary<string, Transform> primitiveTransform = [];

        readonly MeshLoaderSettings loaderSettings;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GLTFMeshLoader(string path, MeshLoaderSettings settings)
        {
            var sw = new Stopwatch();
            sw.Start();

            loaderSettings = settings;
            ModelRoot model = ModelRoot.Load(path);
            Scene scene = model.DefaultScene;

            foreach (Node node in scene.VisualChildren) TraverseNode(node);

            mesh = new()
            {
                meshPath = path,
                submeshes = [.. _submeshes]
            };

            sw.Stop();
            SEDebug.Log(SEDebugState.Info, $"Model loading took {sw.ElapsedMilliseconds}ms");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TraverseNode(Node node)
        {
            if (node.Mesh != null) foreach (MeshPrimitive primitive in node.Mesh.Primitives) ParsePrimitives(primitive, node);
            if (node.Name != null) primitiveTransform.Add(node.Name, ParseNodeTransform(node));
            
            foreach (Node childNode in node.VisualChildren) TraverseNode(childNode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ParsePrimitives(MeshPrimitive primitive, Node node)
        {
           // Add handling for potentially empty vectors to not accidentally add flags
            var position = primitive.GetVertexAccessor("POSITION").AsVector3Array();
            var normal = primitive.GetVertexAccessor("NORMAL")?.AsVector3Array();
            var uv = primitive.GetVertexAccessor("TEXCOORD_0")?.AsVector2Array();
            var index = primitive.GetIndexAccessor().AsIndexArray().ToArray();

            MeshPrimitives primitiveFlags = MeshPrimitives.Position;
            bool extractUV = uv != null && uv.Count > 0 && loaderSettings.ExtractTexCoords;
            bool extractNormal = normal != null && normal.Count > 0 && loaderSettings.ExtractNormals;

            int vertexArrayLength = position.Count * 3;
            int strideLength = 3;

            if (extractUV)
            {
                primitiveFlags |= MeshPrimitives.TexCoord;
                vertexArrayLength += position.Count * 2;
                strideLength += 2;
            }
            if (extractNormal)
            {
                primitiveFlags |= MeshPrimitives.Normal;
                vertexArrayLength += position.Count * 3;
                strideLength += 3;
            }

            float[] vertex = new float[vertexArrayLength];

            for (int i = 0; i < position.Count; i++)
            {
                int arrayOffset = i * strideLength;

                var transformedPos = System.Numerics.Vector3.Transform(position[i], node.WorldMatrix);
                vertex[arrayOffset++] = transformedPos.X;
                vertex[arrayOffset++] = transformedPos.Y;
                vertex[arrayOffset++] = transformedPos.Z;

                if (extractUV)
                {
                    vertex[arrayOffset++] = uv![i].X;
                    vertex[arrayOffset++] = -uv[i].Y;
                }
                if (extractNormal)
                {
                    var transformedNormal = System.Numerics.Vector3.TransformNormal(normal![i], node.WorldMatrix);
                    vertex[arrayOffset++] = transformedNormal.X;
                    vertex[arrayOffset++] = transformedNormal.Y;
                    vertex[arrayOffset++] = transformedNormal.Z;
                }
            }

            _submeshes.Add(new()
            {
                topologyType = (OpenTK.Graphics.OpenGL.PrimitiveType)primitive.DrawPrimitiveType,
                primitiveFlags = primitiveFlags,
                vertices = [.. vertex],
                indices = index,
                material = GetMaterial(primitive.Material)
            });
        }

        private static Transform ParseNodeTransform(Node node)
        {
            AffineTransform transform;
            if (node.LocalTransform.IsSRT) transform = node.LocalTransform;
            else transform = node.LocalTransform.GetDecomposed();

            return new() {
                Pos = (OpenTK.Mathematics.Vector3)transform.Translation,
                Scale = (OpenTK.Mathematics.Vector3)transform.Scale,
                QuatRot = (OpenTK.Mathematics.Quaternion)transform.Rotation
            };
        }

        private MaterialContainer GetMaterial(SharpGLTF.Schema2.Material material)
        {
            return new()
            {
                Name = material.Name,
                AlphaCutoff = material.AlphaCutoff,
                AlphaMode = material.Alpha,
                Dispersion = material.Dispersion,
                DoubleSided = material.DoubleSided,
                Unlit = material.Unlit
            };
        }
    }
}