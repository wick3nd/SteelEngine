using SteelEngine.EngineBase.Structs;
using System.Numerics;
using System.Text;

namespace SteelEngine.Objects
{
    class ModelImporter
    {
        public ModelImporter(string path, out MeshStr meshStruct)
        {
            MeshStr meshStr = new();

            using (FileStream fs = new(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new(fs, Encoding.UTF8, false))
            {
                ulong vertexCount = br.ReadUInt64() >> 3;
                ulong indiceCount = br.ReadUInt64();

                meshStr.vertices = new Vertex[vertexCount];
                meshStr.indices = new uint[indiceCount];

                for (ulong i = 0; i != vertexCount; i++)
                {
                    meshStr.vertices[i].position = new Vector3(
                        br.ReadSingle(),
                        br.ReadSingle(),
                        br.ReadSingle()
                    );

                    meshStr.vertices[i].normals = new Vector3(
                        br.ReadSingle(),
                        br.ReadSingle(),
                        br.ReadSingle()
                    );

                    meshStr.vertices[i].UV      = new Vector2(
                        br.ReadSingle(),
                        br.ReadSingle()
                    );
                }

                for (ulong i = 0; i != indiceCount; i += 3)
                {
                    meshStr.indices[i]     = br.ReadUInt16();
                    meshStr.indices[i + 1] = br.ReadUInt16();
                    meshStr.indices[i + 2] = br.ReadUInt16();
                }
            }

            meshStruct = meshStr;
        }
    }
}