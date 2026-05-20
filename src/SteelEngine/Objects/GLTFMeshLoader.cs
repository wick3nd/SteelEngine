using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using SteelEngine.Core;
using SteelEngine.Core.Buffers;
using SteelEngine.Utils;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

using GLTFMaterial = SharpGLTF.Schema2.Material;
using SEMaterial = SteelEngine.Core.Material;

namespace SteelEngine.Objects
{
    public class GLTFMeshLoader
    {
        public List<int> objectOffsets = [];
        public List<float> verticeData = [];
        public List<uint> indices = [];

        public List<SEMaterial> materials = [];
        public Dictionary<string, Transform> primitiveTransform = [];

        readonly LoaderSettings loaderSettings;
        MeshPrimitives primitiveFlags = MeshPrimitives.Position;
        uint indexOffset = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GLTFMeshLoader(string path, out MeshPrimitives primitives, LoaderSettings settings)
        {
            var sw = new Stopwatch();
            sw.Start();

            loaderSettings = settings;
            ModelRoot model = ModelRoot.Load(path);
            Scene scene = model.DefaultScene;

            foreach (Node node in scene.VisualChildren) TraverseNode(node);

            primitives = primitiveFlags;

            sw.Stop();
            SEDebug.Log(SEDebugState.Info, $"Model loading took {sw.ElapsedMilliseconds}ms");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TraverseNode(Node node)
        {
            if (node.Mesh != null)
            {
                foreach (MeshPrimitive primitive in node.Mesh.Primitives) ParsePrimitives(primitive, node);
            }
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
            
            var color = primitive.GetVertexAccessor("COLOR_0")?.AsVector4Array();
            var index = primitive.GetIndexAccessor().AsIndexArray();
            
            for (int i = 0; i < index.Count; i++) indices.Add(index[i] + indexOffset);
            for (int i = 0; i < position.Count; i++)
            {
                var transformPos = Vector3.Transform(position[i], node.WorldMatrix);  //temporary

                verticeData.Capacity += 3;
                verticeData.Add(transformPos.X);
                verticeData.Add(transformPos.Y);
                verticeData.Add(transformPos.Z);

                if (uv != null && loaderSettings.ExtractTexCoords)
                {
                    verticeData.Capacity += 2;
                    verticeData.Add(uv[i].X);
                    verticeData.Add(-uv[i].Y);
                    
                    primitiveFlags |= MeshPrimitives.TexCoord;
                }

                if (normal != null && loaderSettings.ExtractNormals)
                {
                    verticeData.Capacity += 3;
                    verticeData.Add(normal[i].X);
                    verticeData.Add(normal[i].Y);
                    verticeData.Add(normal[i].Z);

                    primitiveFlags |= MeshPrimitives.Normal;
                }

                if (color != null && loaderSettings.ExtractVertexColors)
                {
                    verticeData.Capacity += 4;
                    verticeData.Add(color[i].X);
                    verticeData.Add(color[i].Y);
                    verticeData.Add(color[i].Z);
                    verticeData.Add(color[i].W);

                    primitiveFlags |= MeshPrimitives.Color;
                }
            }
            indexOffset += (uint)position.Count;
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

        private void GetMaterial(GLTFMaterial material)
        {
           // node.Mesh.Primitives[i].Material
            var textureStream = material.Channels.ToArray()[0].Texture.PrimaryImage.Content.Open();
            materials.Add(new()
            {
                Name = material.Name,
               // ColorTexture = ReadAllBytes(textureStream),

                AlphaMode = material.Alpha,
                AlphaCutoff = material.AlphaCutoff,
                Dispersion = material.Dispersion,
                DoubleSided = material.DoubleSided,
                Unlit = material.Unlit,
            });
        }
    }
}