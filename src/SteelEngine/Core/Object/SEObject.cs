using OpenTK.Mathematics;

namespace SteelEngine.Core
{
    public class SEObject
    {
       // == Object Initialization ==
       // warns the user if none is set
        public Mesh? mesh;
        public string? meshPath;

       // == Texture Initialization ==
       // uses the default "error" texture if not defined
        public Texture2D[]? textures;
        public Texture2D? texture;
        public string[]? texturePaths;
        public string? texturePath;

       // == Shader Initialization ==
       // Uses default shaders if not defined
        public Shader? shader;

       // == Material Initialization ==
       // either uses shipped with gltf, set or none
        public Material[]? materials;
        public Material? material;
        public string[]? materialPaths;
        public string? materialPath;

       // == Misc ==
        public Transform[]? transforms;
        public Transform? transform;

        public SEObject()
        {

        }
    }
}