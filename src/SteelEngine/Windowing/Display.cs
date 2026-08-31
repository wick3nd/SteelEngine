using SDL3;
using SteelEngine.Utils;

namespace SteelEngine.Windowing
{
    public class Display
    {
        public readonly uint DisplayID;

        public int Width;
        public int Height;
        public float RefreshRate;

        public Display(uint displayID)
        {
            DisplayID = displayID;
            var displayMode = SDL.GetCurrentDisplayMode(DisplayID);

            Width = displayMode.Value.W;
            Height = displayMode.Value.H;
            RefreshRate = displayMode.Value.RefreshRate;
        }
    }
}