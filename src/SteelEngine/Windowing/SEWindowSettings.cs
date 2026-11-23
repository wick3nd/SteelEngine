using OpenTK.Mathematics;

namespace SteelEngine.Windowing
{
    public class SEWindowSettings
    {
        public string Title { get; set; } = "SteelEngine window";
        public string Icon { internal get; set; } = "";
        public Vector2i WindowSize { get; set; } = new();
        public Vector2i MinimumWindowSize { get; set; } = new();
        public Vector2i MaximumWindowSize { get; set; } = new();
        public GLProfile Profile { get; set; } = GLProfile.Compatibility;
        public ContextAPI Context { get; set; } = ContextAPI.OpenGL;
        public WindowFlags WindowFlags { get; set; }
    }
}