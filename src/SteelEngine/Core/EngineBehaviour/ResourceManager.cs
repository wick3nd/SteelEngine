using SharpGLTF.Schema2;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
    internal static class ResourceManager  // Add some debug things later on
    {
       // private static readonly Dictionary<string, ShaderContainer> _shaderCache = [];
        private static readonly Dictionary<string, TextureContainer> _textureCache = [];
        private static readonly Dictionary<string, MaterialContainer> _materialCache = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CacheTextureInfo(TextureContainer texture)
        {
            bool wasCached = _textureCache.TryAdd(texture.path, texture);

            if (wasCached) SEDebug.Log(SEDebugState.Debug, $"Cached Texture2D[{texture}]");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetTextureInfo(string path, out TextureContainer texture)
        {
            if (_textureCache.TryGetValue(Path.GetRelativePath(Environment.CurrentDirectory, path), out texture)) return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void RemoveTextureInfo(string path)
        {
            SEDebug.Log(SEDebugState.Debug, $"Disposing TextureContainer[{path}]");

            _materialCache.Remove(Path.GetRelativePath(Environment.CurrentDirectory, path));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CacheMaterial(MaterialContainer material)
        {
            bool wasCached = !_materialCache.TryAdd(material.Name, material);

            if (wasCached) SEDebug.Log(SEDebugState.Debug, $"Cached MaterialInfo[{material.Name}]");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetMaterial(string name, out MaterialContainer material)
        {
            if (_materialCache.TryGetValue(name, out material)) return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void RemoveMaterial(string name)
        {
            SEDebug.Log(SEDebugState.Debug, $"Disposing MaterialInfo[{name}]");

            _materialCache.Remove(name);
        }
    }
}