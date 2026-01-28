using OpenTK.Graphics.OpenGL;
using SteelEngine.Utils;
using System.Runtime.CompilerServices;

#pragma warning disable CA1816, IDE0079, CA1822
namespace SteelEngine.Core.Buffers
{
    public class Framebuffer : IBufferObject
    {
        private int m_FrameBuffer;
        private static int _currentBound;
        private readonly string? _debugName;

        private readonly int _width;
        private readonly int _height;
        private ClearBufferMask _bufferMasks;
        private int colorBufferIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Framebuffer(int width, int height)
        {
            GL.GenFramebuffers(1, ref m_FrameBuffer);
            if (m_FrameBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a FBO", true);
            SEDebug.Log(SEDebugState.Debug, $"Created a new FBO {m_FrameBuffer}");

            _width = width;
            _height = height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Framebuffer(string debugName, int width, int height)
        {
            GL.GenFramebuffers(1, ref m_FrameBuffer);
            if (m_FrameBuffer == 0) SEDebug.Log(SEDebugState.Error, $"Failed to create a FBO \"{debugName}\"", true);

            _debugName = debugName;
            SEDebug.Log(SEDebugState.Debug, $"Created a new FBO \"{debugName}\"");

            _width = width;
            _height = height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachColor(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.ColorBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + colorBufferIndex++), TextureTarget.Texture2d, texture.GetHandle(), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachColor(Renderbuffer renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.ColorBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + colorBufferIndex++), TextureTarget.Texture2d, renderBuffer.GetHandle(), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachDepth(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2d, texture.GetHandle(), 0);

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachDepth(Renderbuffer renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit;
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderBuffer.GetHandle());

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachStencil(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.StencilBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, TextureTarget.Texture2d, texture.GetHandle(), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachStencil(Renderbuffer renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.StencilBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, TextureTarget.Texture2d, renderBuffer.GetHandle(), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachDepthStencil(Texture2D texture)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2d, texture.GetHandle(), 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachDepthStencil(Renderbuffer renderBuffer)
        {
            _bufferMasks |= ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit;
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, renderBuffer.GetHandle());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Validate()
        {
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferStatus.FramebufferComplete) throw new Exception($"FBO incomplete: {status}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateAttachments(InvalidateFramebufferAttachment[] attachmentsToInvalidate)
        {
            for (int i = 0; i < attachmentsToInvalidate.Length; i++) _bufferMasks &= ~AttachmentToMask(attachmentsToInvalidate[i]);

            GL.InvalidateFramebuffer(FramebufferTarget.Framebuffer, attachmentsToInvalidate.Length, attachmentsToInvalidate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(int dstFBOHandle, int srcStartPosX, int srcStartPosY, int srcEndPosX, int srcEndPosY, int dstStartPosX, int dstStartPosY, int dstEndPosX, int dstEndPosY, BlitFramebufferFilter bufferFilter)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, m_FrameBuffer);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, dstFBOHandle);

            GL.BlitFramebuffer(srcStartPosX, srcStartPosY,
                               srcEndPosX, srcEndPosY,

                               dstStartPosX, dstStartPosY,
                               dstEndPosX, dstEndPosY,
                               _bufferMasks,
                               bufferFilter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(Framebuffer dstFBO, int srcStartPosX, int srcStartPosY, int srcEndPosX, int srcEndPosY, int dstStartPosX, int dstStartPosY, int dstEndPosX, int dstEndPosY, BlitFramebufferFilter bufferFilter)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, m_FrameBuffer);
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

        public override string ToString() => _debugName ?? $"{m_FrameBuffer}";
        public int GetHandle() => m_FrameBuffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => GL.Clear(_bufferMasks);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            if (_currentBound != m_FrameBuffer)
            {
                _currentBound = m_FrameBuffer;
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_FrameBuffer);
                GL.Viewport(0, 0, _width, _height);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            if (_currentBound == m_FrameBuffer && _currentBound != 0)
            {
                _currentBound = 0;
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            if (m_FrameBuffer != 0)
            {
                GL.DeleteFramebuffers(1, in m_FrameBuffer);

                _currentBound = 0;
                m_FrameBuffer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Destroy();
    }
}