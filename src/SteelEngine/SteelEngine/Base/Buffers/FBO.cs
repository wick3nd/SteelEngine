using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

namespace SteelEngine.Base.Buffers
{
#pragma warning disable IDE0079, CA1822
    internal class FBO
    {
        private readonly int _Handle;

        private readonly int _width;
        private readonly int _height;
        private ClearBufferMask _bufferMasks;

       // private Texture2D? texture;

        private int colorBufferIndex;

        internal FBO(int width, int height)

        {
            GL.GenFramebuffers(1, ref _Handle);

            if (_Handle == 0)
            {
                SEDebug.Log(SEDebugState.Error, $"Failed to create a FBO.");
                throw new Exception($"Failed to create a FBO.");
            }

            _width = width;
            _height = height;
        }

        internal void AttachColor(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.ColorBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + colorBufferIndex++), TextureTarget.Texture2d, texture.GetHandle(), 0);
        }
        internal void AttachColor(RBO renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.ColorBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + colorBufferIndex++), TextureTarget.Texture2d, renderBuffer.GetHandle(), 0);
        }

        internal void AttachDepth(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2d, texture.GetHandle(), 0);

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
        }
        internal void AttachDepth(RBO renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit;
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderBuffer.GetHandle());

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
        }

        internal void AttachStencil(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.StencilBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, TextureTarget.Texture2d, texture.GetHandle(), 0);
        }
        internal void AttachStencil(RBO renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.StencilBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, TextureTarget.Texture2d, renderBuffer.GetHandle(), 0);
        }

        internal void AttachDepthStencil(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2d, texture.GetHandle(), 0);
        }
        internal void AttachDepthStencil(RBO renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit;
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, renderBuffer.GetHandle());
        }

        internal void Validate()
        {
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferStatus.FramebufferComplete) throw new Exception($"FBO incomplete: {status}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InvalidateAttachments(InvalidateFramebufferAttachment[] attachmentsToInvalidate)
        {
            for (int i = 0; i < attachmentsToInvalidate.Length; i++) _bufferMasks &= ~AttachmentToMask(attachmentsToInvalidate[i]);

            GL.InvalidateFramebuffer(FramebufferTarget.Framebuffer, attachmentsToInvalidate.Length, attachmentsToInvalidate);
        }

        internal void CopyTo(int dstFBOHandle, int srcStartPosX, int srcStartPosY, int srcEndPosX, int srcEndPosY, int dstStartPosX, int dstStartPosY, int dstEndPosX, int dstEndPosY, BlitFramebufferFilter bufferFilter)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _Handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, dstFBOHandle);

            GL.BlitFramebuffer(srcStartPosX, srcStartPosY,
                               srcEndPosX, srcEndPosY,

                               dstStartPosX, dstStartPosY,
                               dstEndPosX, dstEndPosY,
                               _bufferMasks,
                               bufferFilter);
        }
        internal void CopyTo(FBO dstFBO, int srcStartPosX, int srcStartPosY, int srcEndPosX, int srcEndPosY, int dstStartPosX, int dstStartPosY, int dstEndPosX, int dstEndPosY, BlitFramebufferFilter bufferFilter)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _Handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, dstFBO.GetHandle());

            GL.BlitFramebuffer(srcStartPosX, srcStartPosY,
                               srcEndPosX,   srcEndPosY,

                               dstStartPosX, dstStartPosY,
                               dstEndPosX,   dstEndPosY,

                               _bufferMasks,
                               bufferFilter);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ClearBufferMask AttachmentToMask(InvalidateFramebufferAttachment attachment)
        {
            return attachment switch
            {
                InvalidateFramebufferAttachment.DepthAttachment => ClearBufferMask.DepthBufferBit,
                InvalidateFramebufferAttachment.StencilAttachmentExt => ClearBufferMask.StencilBufferBit,
                InvalidateFramebufferAttachment.DepthStencilAttachment => ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit,

                >= InvalidateFramebufferAttachment.ColorAttachment0 and <= InvalidateFramebufferAttachment.ColorAttachment31 => ClearBufferMask.ColorBufferBit,
                _ => throw new NotImplementedException(),
            };
        }

        public override string ToString() => $"{_Handle}";
        internal int GetHandle() => _Handle;
        internal void Clear() => GL.Clear(_bufferMasks);
        internal void Enable()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _Handle);
            GL.Viewport(0, 0, _width, _height);
        }
        internal void Disable() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        internal void Destroy() => GL.DeleteFramebuffer(_Handle);
    }
}