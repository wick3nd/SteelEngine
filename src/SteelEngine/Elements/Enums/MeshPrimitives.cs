namespace SteelEngine.Core.Buffers
{
    [Flags]
    public enum MeshPrimitives
    {
        None = 0,
        Position = 1,
        TexCoord = 2,
        Normal = 4,
        Color = 8
    }
}
