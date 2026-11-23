using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Core.Buffers;
using SteelEngine.Utils;
using SteelEngine.Windowing;
using System.Runtime.CompilerServices;

namespace SteelEngine
{
    public static class Renderer
    {
        private static int _geometryBuffer;
        private static int _gPosition;
        private static int _gNormal;
        private static int _gColorSpec;
        private static int _depth;

        internal static void InitGBuffer()
        {
            int width = Monitors.primaryDisplay.Width;
            int height = Monitors.primaryDisplay.Height;

            GL.GenFramebuffers(1, ref _geometryBuffer);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _geometryBuffer);

            GL.GenTextures(1, ref _gPosition);
            GL.BindTexture(TextureTarget.Texture2D, _gPosition);
            GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _gPosition, 0);

            GL.GenTextures(1, ref _gNormal);
            GL.BindTexture(TextureTarget.Texture2D, _gNormal);
            GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, _gNormal, 0);

            GL.GenTextures(1, ref _gColorSpec);
            GL.BindTexture(TextureTarget.Texture2D, _gColorSpec);
            GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, _gColorSpec, 0);

            GL.GenTextures(1, ref _depth);
            GL.BindTexture(TextureTarget.Texture2D, _depth);
            GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.DepthComponent24, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _depth, 0);

            DrawBufferMode[] attachments = [DrawBufferMode.ColorAttachment0, DrawBufferMode.ColorAttachment1, DrawBufferMode.ColorAttachment2, DrawBufferMode.ColorAttachment3];
            GL.DrawBuffers(4, attachments);

            var FBOstatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (FBOstatus != FramebufferStatus.FramebufferComplete) SEDebug.Log(SEDebugState.Error, "Couldn't initialize framebuffer", true);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
        }

        internal static void PrepGBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _geometryBuffer);
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnableFunction(EnableCap enable) => GL.Enable(enable);

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
