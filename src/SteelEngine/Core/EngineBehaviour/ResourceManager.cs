using SteelEngine.Core.Buffers;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core.EngineBehaviour
{
    internal static class ResourceManager  // Add some debug things later on
    {
        static readonly Dictionary<MeshPrimitives, VertexArray> VAOCache = new(EngineCoreSettings.StartUpVAOEntryCount);
        static readonly Dictionary<string, ShaderContainer> shaderCache = new(EngineCoreSettings.StartUpShaderEntryCount);
        static readonly Dictionary<string, Texture2D> texture2DCache = new(EngineCoreSettings.StartUpTextureEntryCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CacheVAO(VertexArray vao)
        {
            bool wasCached = !VAOCache.TryAdd(vao.PrimitiveFlags, vao);

            if (wasCached) SEDebug.Log(SEDebugState.Debug, $"Cached VAO[{vao.GetHandle()}] [{vao.PrimitiveFlags}]");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static VertexArray TryGetOrCreateVAO(MeshPrimitives primitives)
        {
            if (VAOCache.TryGetValue(primitives, out VertexArray? vao))
            {
                SEDebug.Log(SEDebugState.Debug, $"Parsed VAO[{vao.GetHandle()}] [{primitives}]");
                return vao;
            }
            
            return new();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void RemoveVAO(VertexArray vao)
        {
            SEDebug.Log(SEDebugState.Debug, $"Disposing VAO[{vao.GetHandle()}] [{vao.PrimitiveFlags}]");  // No, it has not be removed >:)

            VAOCache.Remove(vao.PrimitiveFlags);  // Just kidding :)
        }

       // The fuck

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CacheTexture2D(Texture2D texture2D)
        {
            bool wasCached = !texture2DCache.TryAdd(texture2D.ToString(), texture2D);

            if (wasCached) SEDebug.Log(SEDebugState.Debug, $"Cached Texture2D[{texture2D}]");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Texture2D TryGetOrCreateTexture2D(string x)  // Works only for normal textuers, not RBO
        {
            if (texture2DCache.TryGetValue(x, out Texture2D? texture))
            {
                SEDebug.Log(SEDebugState.Debug, $"Parsed Texture2D[{texture}]");
                return texture;
            }
            
            Texture2D texture2D = new(x);
            CacheTexture2D(texture2D);
            return texture2D;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void RemoveTexture2D(Texture2D texture2D)
        {
            SEDebug.Log(SEDebugState.Debug, $"Disposing Texture2D[{texture2D}]");

            texture2DCache.Remove(texture2D.ToString());
        }
    }
}