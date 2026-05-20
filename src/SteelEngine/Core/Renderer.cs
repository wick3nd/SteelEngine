using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace SteelEngine
{
    public static class Renderer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable(EnableCap enable) => GL.Enable(enable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeBlendFunc(BlendingFactor sourceBlend, BlendingFactor destBlend) => GL.BlendFunc(sourceBlend, destBlend);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeBlendEquation(BlendEquationMode blendMode) => GL.BlendEquation(blendMode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeBlendColor(float r, float g, float b, float a) => GL.BlendColor(r, g, b, a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ChangeBlendColor(Vector4 color) => GL.BlendColor(color.X, color.Y, color.Z, color.W);
    }
}
