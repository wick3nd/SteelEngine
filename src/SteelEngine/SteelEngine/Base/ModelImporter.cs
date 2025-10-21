using System.Text;

namespace SteelEngine.Base
{
    class ModelImporter
    {
        public ModelImporter(string path, out float[] vertices, out uint[] indices)
        {
            float[] vertData;
            uint[] indiceData;

            using (FileStream fs = new(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new(fs, Encoding.UTF8, false))
            {
                ulong floatCount = br.ReadUInt64();
                ulong indiceCount = br.ReadUInt64();

                vertData = new float[floatCount];
                indiceData = new uint[indiceCount];

                for (ulong i = 0; i != floatCount; i+=8)
                {
                    vertData[i] = br.ReadSingle();
                    vertData[i+1] = br.ReadSingle();
                    vertData[i+2] = br.ReadSingle();

                    vertData[i+3] = br.ReadSingle();
                    vertData[i+4] = br.ReadSingle();
                    vertData[i+5] = br.ReadSingle();

                    vertData[i+6] = br.ReadSingle();
                    vertData[i+7] = br.ReadSingle();
                }

                for (ulong i = 0; i != indiceCount; i+=3)
                {
                    indiceData[i] = br.ReadUInt16();
                    indiceData[i+1] = br.ReadUInt16();
                    indiceData[i+2] = br.ReadUInt16();
                }
            }

            vertices = vertData;
            indices = indiceData;
        }
    }
}