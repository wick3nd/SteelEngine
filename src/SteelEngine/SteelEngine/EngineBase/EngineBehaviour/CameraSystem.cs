using SteelEngine.Elements;
using SteelEngine.Utils;

namespace SteelEngine.EngineBase.EngineBehaviour
{
    internal static class CameraSystem
    {
        internal static Dictionary<string, Camera> savedCameras = [];
        internal static Camera? chosenCamera;

        internal static void Add(string name, Camera camera) => savedCameras!.Add(name, camera);
        internal static void Use(string name)
        {
            if (savedCameras.TryGetValue(name, out chosenCamera)) return;
            
            SEDebug.Log(SEDebugState.Error, $"No camera with name \"{name}\" exists.");
        }
    }
}