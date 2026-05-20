using SharpGLTF.Schema2;
using SteelEngine.Utils;
using System.Text.Json;

namespace SteelEngine.Core
{
    public class Material
    {
        public string Name { get; internal set; }
        public byte[] ColorTexture { get; internal set; } = [255, 255, 255, 255];

        public AlphaMode AlphaMode { get; internal set; } = AlphaMode.OPAQUE;
        public float AlphaCutoff { get; internal set; } = 0f;
        public float Dispersion { get; internal set; } = 0f;
        public bool DoubleSided { get; internal set; } = false;
        public bool Unlit { get; internal set; } = false;

        public Material(string path)
        {
            ParseMaterialFile(path);
        }

        public Material() { }

        public override string ToString() => Name;

        void ParseMaterialFile(string path)
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;

            if (root.TryGetProperty("materialName", out var element1)) Name = element1.GetString() ?? "EMPTY \"materialName\" FIELD";
            if (root.TryGetProperty("doubleSided", out var element2)) DoubleSided = element2.GetBoolean();
            if (root.TryGetProperty("unlit", out var element3)) DoubleSided = element3.GetBoolean();
            if (root.TryGetProperty("alphaCutOff", out var element4)) AlphaCutoff = Math.Clamp(element4.GetSingle(), 0f, 1f);

            #if DEBUG
            SEDebug.Log(SEDebugState.Warning, this);
            SEDebug.Log(SEDebugState.Warning, DoubleSided);
            SEDebug.Log(SEDebugState.Warning, Unlit);
            SEDebug.Log(SEDebugState.Warning, AlphaCutoff);
            #endif

            ParseAlphaMode(root);
            ParseTextureField(root, "colorTexture");
        }

        void ParseTextureField(JsonElement root, string fieldName)
        {
            if (root.TryGetProperty(fieldName, out var element))
            {
                if (element.ValueKind == JsonValueKind.Array)  // parses texture as a solid color
                {
                    byte[] values = [.. element.EnumerateArray().Select(x => Math.Clamp(x.GetByte(), (byte)0x00, (byte)0xFF))];
                    ColorTexture = [.. values];

                    SEDebug.Log(SEDebugState.Error, $"{values[0]} {values[1]} {values[2]} {values[3]}");
                    return;
                }
                
                var path2 = element.GetString();

                #if DEBUG
                SEDebug.Log(SEDebugState.Warning, path2);
                #endif

                return;
            }

            SEDebug.Log(SEDebugState.Error, $"Missing \"{fieldName}\" field");
        }

        void ParseAlphaMode(JsonElement root)
        {
            string alphaMode = null!;
            if (root.TryGetProperty("alphaMode", out var element)) alphaMode = element.GetString()!;

            AlphaMode = alphaMode.ToLower() switch
            {
                "opaque" => AlphaMode.OPAQUE,
                "blend" => AlphaMode.BLEND,
                "mask" => AlphaMode.MASK,
                _ => AlphaMode.OPAQUE
            };

            #if DEBUG
            SEDebug.Log(SEDebugState.Warning, alphaMode);
            #endif
        }
    }
}