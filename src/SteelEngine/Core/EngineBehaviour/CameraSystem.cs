using SteelEngine.Objects;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Core
{
#pragma warning disable CS8618
    internal static class CameraSystem
    {
        internal static Dictionary<string, Camera> savedCameras = [];
        internal static Camera chosenCamera;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Add(string name, Camera camera) => savedCameras!.Add(name, camera);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Use(string name)
        {
            if (savedCameras.TryGetValue(name, out chosenCamera!)) return;
            
            SEDebug.Log(SEDebugState.Error, $"No camera with name \"{name}\" exists.");
        }
    }
}