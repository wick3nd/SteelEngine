namespace SteelEngine.Core.Buffers
{
    public enum DataType
    {
        Bool = 4,
        Int = 4,
        UInt = 4,
        Float = 4,
        Double = 8,

        BVec2 = 8,
        BVec3 = 16,
        BVec4 = 16,

        IVec2 = 8,
        IVec3 = 16,
        IVec4 = 16,

        UVec2 = 8,
        UVec3 = 16,
        UVec4 = 16,

        Vec2 = 8,
        Vec3 = 16,
        Vec4 = 16,

        DVec2 = 16,
        DVec3 = 32,
        DVec4 = 32,

        Mat2 = 32,
        Mat3 = 48,
        Mat4 = 64,
        Mat2x3 = 32,
        Mat2x4 = 32,
        Mat3x2 = 48,
        Mat3x4 = 48,
        Mat4x2 = 64,
        Mat4x3 = 64,

        DMat2 = 32,
        DMat3 = 96,
        DMat4 = 128,
        DMat2x3 = 64,
        DMat2x4 = 64,
        DMat3x2 = 96,
        DMat3x4 = 96,
        DMat4x2 = 128,
        DMat4x3 = 128,
    }
}