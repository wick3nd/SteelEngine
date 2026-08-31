using OpenTK.Mathematics;
using SteelEngine.Windowing.Common;

namespace SteelEngine.Windowing
{
    public class SEWindowSettings
    {
        public string Title { get; set; } = "SteelEngine window";
        public Vector2i WindowSize { get; set; } = new();
        public Vector2i MinimumWindowSize { get; set; } = new();
        public Vector2i MaximumWindowSize { get; set; } = new();
        public GLProfile Profile { get; set; } = GLProfile.Compatibility;
        public ContextAPI Context { get; set; }
        public WindowFlags WindowFlags { get; set; }
    }
}