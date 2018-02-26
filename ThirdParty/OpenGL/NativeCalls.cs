/* 
 * The MIT License (MIT)
 * https://opensource.org/licenses/MIT
 *
 * Copyright (c) 2006 - 2010 The Open Toolkit library.
 * Copyright (c) 2010 - 2017 Giawa (http://www.giawa.com) 
 */
 
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public static partial class OpenGL
{
    public unsafe static class NativeCalls
    {
        const string lib = "opengl32";
        
        public static Dictionary<string, MethodInfo> Methods;

        static NativeCalls()
        {
            var m = typeof(NativeCalls).GetMethods(BindingFlags.Static | BindingFlags.Public);
            Methods = new Dictionary<string, MethodInfo>(m.Length);
            for (int i = 0; i < m.Length; i++)
            {
                Methods.Add(m[i].Name, m[i]);
            }
        }

        [DllImport(lib, EntryPoint = "glActiveShaderProgram")]
        public extern static void ActiveShaderProgram(uint pipeline, uint program);
        [DllImport(lib, EntryPoint = "glActiveTexture")]
        public extern static void ActiveTexture(TextureUnit texture);
        [DllImport(lib, EntryPoint = "glAttachShader")]
        public extern static void AttachShader(uint program, uint shader);
        [DllImport(lib, EntryPoint = "glBeginConditionalRender")]
        public extern static void BeginConditionalRender(uint id, ConditionalRenderType mode);
        [DllImport(lib, EntryPoint = "glEndConditionalRender")]
        public extern static void EndConditionalRender();
        [DllImport(lib, EntryPoint = "glBeginQuery")]
        public extern static void BeginQuery(QueryTarget target, uint id);
        [DllImport(lib, EntryPoint = "glEndQuery")]
        public extern static void EndQuery(QueryTarget target);
        [DllImport(lib, EntryPoint = "glBeginQueryIndexed")]
        public extern static void BeginQueryIndexed(uint target, uint index, uint id);
        [DllImport(lib, EntryPoint = "glEndQueryIndexed")]
        public extern static void EndQueryIndexed(QueryTarget target, uint index);
        [DllImport(lib, EntryPoint = "glBeginTransformFeedback")]
        public extern static void BeginTransformFeedback(BeginFeedbackMode primitiveMode);
        [DllImport(lib, EntryPoint = "glEndTransformFeedback")]
        public extern static void EndTransformFeedback();
        [DllImport(lib, EntryPoint = "glBindAttribLocation")]
        public extern static void BindAttribLocation(uint program, uint index, string name);
        [DllImport(lib, EntryPoint = "glBindBuffer")]
        public extern static void BindBuffer(BufferTarget target, uint buffer);
        [DllImport(lib, EntryPoint = "glBindBufferBase")]
        public extern static void BindBufferBase(BufferTarget target, uint index, uint buffer);
        [DllImport(lib, EntryPoint = "glBindBufferRange")]
        public extern static void BindBufferRange(BufferTarget target, uint index, uint buffer, IntPtr offset, IntPtr size);
        [DllImport(lib, EntryPoint = "glBindBuffersBase")]
        public extern static void BindBuffersBase(BufferTarget target, uint first, int count, uint[] buffers);
        [DllImport(lib, EntryPoint = "glBindBuffersRange")]
        public extern static void BindBuffersRange(BufferTarget target, uint first, int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes);
        [DllImport(lib, EntryPoint = "glBindFragDataLocation")]
        public extern static void BindFragDataLocation(uint program, uint colorNumber, string name);
        [DllImport(lib, EntryPoint = "glBindFragDataLocationIndexed")]
        public extern static void BindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name);
        [DllImport(lib, EntryPoint = "glBindFramebuffer")]
        public extern static void BindFramebuffer(FramebufferTarget target, uint framebuffer);
        [DllImport(lib, EntryPoint = "glBindImageTexture")]
        public extern static void BindImageTexture(uint unit, uint texture, int level, bool layered, int layer, BufferAccess access, PixelInternalFormat format);
        [DllImport(lib, EntryPoint = "glBindImageTextures")]
        public extern static void BindImageTextures(uint first, int count, uint[] textures);
        [DllImport(lib, EntryPoint = "glBindProgramPipeline")]
        public extern static void BindProgramPipeline(uint pipeline);
        [DllImport(lib, EntryPoint = "glBindRenderbuffer")]
        public extern static void BindRenderbuffer(RenderbufferTarget target, uint renderbuffer);
        [DllImport(lib, EntryPoint = "glBindSampler")]
        public extern static void BindSampler(uint unit, uint sampler);
        [DllImport(lib, EntryPoint = "glBindSamplers")]
        public extern static void BindSamplers(uint first, int count, uint[] samplers);
        [DllImport(lib, EntryPoint = "glBindTexture")]
        public extern static void BindTexture(TextureTarget target, uint texture);
        [DllImport(lib, EntryPoint = "glBindTextures")]
        public extern static void BindTextures(uint first, int count, uint[] textures);
        [DllImport(lib, EntryPoint = "glBindTextureUnit")]
        public extern static void BindTextureUnit(uint unit, uint texture);
        [DllImport(lib, EntryPoint = "glBindTransformFeedback")]
        public extern static void BindTransformFeedback(NvTransformFeedback2 target, uint id);
        [DllImport(lib, EntryPoint = "glBindVertexArray")]
        public extern static void BindVertexArray(uint array);
        [DllImport(lib, EntryPoint = "glBindVertexBuffer")]
        public extern static void BindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, IntPtr stride);
        [DllImport(lib, EntryPoint = "glVertexArrayVertexBuffer")]
        public extern static void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride);
        [DllImport(lib, EntryPoint = "glBindVertexBuffers")]
        public extern static void BindVertexBuffers(uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides);
        [DllImport(lib, EntryPoint = "glVertexArrayVertexBuffers")]
        public extern static void VertexArrayVertexBuffers(uint vaobj, uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides);
        [DllImport(lib, EntryPoint = "glBlendColor")]
        public extern static void BlendColor(float red, float green, float blue, float alpha);
        [DllImport(lib, EntryPoint = "glBlendEquation")]
        public extern static void BlendEquation(BlendEquationMode mode);
        [DllImport(lib, EntryPoint = "glBlendEquationi")]
        public extern static void BlendEquationi(uint buf, BlendEquationMode mode);
        [DllImport(lib, EntryPoint = "glBlendEquationSeparate")]
        public extern static void BlendEquationSeparate(BlendEquationMode modeRGB, BlendEquationMode modeAlpha);
        [DllImport(lib, EntryPoint = "glBlendEquationSeparatei")]
        public extern static void BlendEquationSeparatei(uint buf, BlendEquationMode modeRGB, BlendEquationMode modeAlpha);
        [DllImport(lib, EntryPoint = "glBlendFunc")]
        public extern static void BlendFunc(BlendingFactorSrc sfactor, BlendingFactorDest dfactor);
        [DllImport(lib, EntryPoint = "glBlendFunci")]
        public extern static void BlendFunci(uint buf, BlendingFactorSrc sfactor, BlendingFactorDest dfactor);
        [DllImport(lib, EntryPoint = "glBlendFuncSeparate")]
        public extern static void BlendFuncSeparate(BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha);
        [DllImport(lib, EntryPoint = "glBlendFuncSeparatei")]
        public extern static void BlendFuncSeparatei(uint buf, BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha);
        [DllImport(lib, EntryPoint = "glBlitFramebuffer")]
        public extern static void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter);
        [DllImport(lib, EntryPoint = "glBlitNamedFramebuffer")]
        public extern static void BlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter);
        [DllImport(lib, EntryPoint = "glBufferData")]
        public extern static void BufferData(BufferTarget target, int size, IntPtr data, BufferUsageHint usage);
        [DllImport(lib, EntryPoint = "glNamedBufferData")]
        public extern static void NamedBufferData(uint buffer, int size, IntPtr data, BufferUsageHint usage);
        [DllImport(lib, EntryPoint = "glBufferStorage")]
        public extern static void BufferStorage(BufferTarget target, IntPtr size, IntPtr data, uint flags);
        [DllImport(lib, EntryPoint = "glNamedBufferStorage")]
        public extern static void NamedBufferStorage(uint buffer, int size, IntPtr data, uint flags);
        [DllImport(lib, EntryPoint = "glBufferSubData")]
        public extern static void BufferSubData(BufferTarget target, IntPtr offset, IntPtr size, IntPtr data);
        [DllImport(lib, EntryPoint = "glNamedBufferSubData")]
        public extern static void NamedBufferSubData(uint buffer, IntPtr offset, int size, IntPtr data);
        [DllImport(lib, EntryPoint = "glCheckFramebufferStatus")]
        public extern static FramebufferErrorCode CheckFramebufferStatus(FramebufferTarget target);
        [DllImport(lib, EntryPoint = "glCheckNamedFramebufferStatus")]
        public extern static FramebufferErrorCode CheckNamedFramebufferStatus(uint framebuffer, FramebufferTarget target);
        [DllImport(lib, EntryPoint = "glClampColor")]
        public extern static void ClampColor(ClampColorTarget target, ClampColorMode clamp);
        [DllImport(lib, EntryPoint = "glClear")]
        public extern static void Clear(ClearBufferMask mask);
        [DllImport(lib, EntryPoint = "glClearBufferiv")]
        public extern static void ClearBufferiv(ClearBuffer buffer, int drawbuffer, int[] value);
        [DllImport(lib, EntryPoint = "glClearBufferuiv")]
        public extern static void ClearBufferuiv(ClearBuffer buffer, int drawbuffer, uint[] value);
        [DllImport(lib, EntryPoint = "glClearBufferfv")]
        public extern static void ClearBufferfv(ClearBuffer buffer, int drawbuffer, float[] value);
        [DllImport(lib, EntryPoint = "glClearBufferfi")]
        public extern static void ClearBufferfi(ClearBuffer buffer, int drawbuffer, float depth, int stencil);
        [DllImport(lib, EntryPoint = "glClearNamedFramebufferiv")]
        public extern static void ClearNamedFramebufferiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, int[] value);
        [DllImport(lib, EntryPoint = "glClearNamedFramebufferuiv")]
        public extern static void ClearNamedFramebufferuiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, uint[] value);
        [DllImport(lib, EntryPoint = "glClearNamedFramebufferfv")]
        public extern static void ClearNamedFramebufferfv(uint framebuffer, ClearBuffer buffer, int drawbuffer, float[] value);
        [DllImport(lib, EntryPoint = "glClearNamedFramebufferfi")]
        public extern static void ClearNamedFramebufferfi(uint framebuffer, ClearBuffer buffer, float depth, int stencil);
        [DllImport(lib, EntryPoint = "glClearBufferData")]
        public extern static void ClearBufferData(BufferTarget target, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glClearNamedBufferData")]
        public extern static void ClearNamedBufferData(uint buffer, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glClearBufferSubData")]
        public extern static void ClearBufferSubData(BufferTarget target, SizedInternalFormat internalFormat, IntPtr offset, IntPtr size, PixelInternalFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glClearNamedBufferSubData")]
        public extern static void ClearNamedBufferSubData(uint buffer, SizedInternalFormat internalFormat, IntPtr offset, int size, PixelInternalFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glClearColor")]
        public extern static void ClearColor(float red, float green, float blue, float alpha);
        [DllImport(lib, EntryPoint = "glClearDepth")]
        public extern static void ClearDepth(double depth);
        [DllImport(lib, EntryPoint = "glClearDepthf")]
        public extern static void ClearDepthf(float depth);
        [DllImport(lib, EntryPoint = "glClearStencil")]
        public extern static void ClearStencil(int s);
        [DllImport(lib, EntryPoint = "glClearTexImage")]
        public extern static void ClearTexImage(uint texture, int level, PixelInternalFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glClearTexSubImage")]
        public extern static void ClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glClientWaitSync")]
        public extern static ArbSync ClientWaitSync(IntPtr sync, uint flags, ulong timeout);
        [DllImport(lib, EntryPoint = "glClipControl")]
        public extern static void ClipControl(ClipControlOrigin origin, ClipControlDepth depth);
        [DllImport(lib, EntryPoint = "glColorMask")]
        public extern static void ColorMask(bool red, bool green, bool blue, bool alpha);
        [DllImport(lib, EntryPoint = "glColorMaski")]
        public extern static void ColorMaski(uint buf, bool red, bool green, bool blue, bool alpha);
        [DllImport(lib, EntryPoint = "glCompileShader")]
        public extern static void CompileShader(uint shader);
        [DllImport(lib, EntryPoint = "glCompressedTexImage1D")]
        public extern static void CompressedTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTexImage2D")]
        public extern static void CompressedTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTexImage3D")]
        public extern static void CompressedTexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTexSubImage1D")]
        public extern static void CompressedTexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTextureSubImage1D")]
        public extern static void CompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, PixelInternalFormat format, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTexSubImage2D")]
        public extern static void CompressedTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTextureSubImage2D")]
        public extern static void CompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelInternalFormat format, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTexSubImage3D")]
        public extern static void CompressedTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCompressedTextureSubImage3D")]
        public extern static void CompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, int imageSize, IntPtr data);
        [DllImport(lib, EntryPoint = "glCopyBufferSubData")]
        public extern static void CopyBufferSubData(BufferTarget readTarget, BufferTarget writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size);
        [DllImport(lib, EntryPoint = "glCopyNamedBufferSubData")]
        public extern static void CopyNamedBufferSubData(uint readBuffer, uint writeBuffer, IntPtr readOffset, IntPtr writeOffset, int size);
        [DllImport(lib, EntryPoint = "glCopyImageSubData")]
        public extern static void CopyImageSubData(uint srcName, BufferTarget srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, BufferTarget dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);
        [DllImport(lib, EntryPoint = "glCopyTexImage1D")]
        public extern static void CopyTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int border);
        [DllImport(lib, EntryPoint = "glCopyTexImage2D")]
        public extern static void CopyTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int height, int border);
        [DllImport(lib, EntryPoint = "glCopyTexSubImage1D")]
        public extern static void CopyTexSubImage1D(TextureTarget target, int level, int xoffset, int x, int y, int width);
        [DllImport(lib, EntryPoint = "glCopyTextureSubImage1D")]
        public extern static void CopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width);
        [DllImport(lib, EntryPoint = "glCopyTexSubImage2D")]
        public extern static void CopyTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glCopyTextureSubImage2D")]
        public extern static void CopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glCopyTexSubImage3D")]
        public extern static void CopyTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glCopyTextureSubImage3D")]
        public extern static void CopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glCreateBuffers")]
        public extern static void CreateBuffers(int n, uint[] buffers);
        [DllImport(lib, EntryPoint = "glCreateFramebuffers")]
        public extern static void CreateFramebuffers(int n, uint[] ids);
        [DllImport(lib, EntryPoint = "glCreateProgram")]
        public extern static uint CreateProgram();
        [DllImport(lib, EntryPoint = "glCreateProgramPipelines")]
        public extern static void CreateProgramPipelines(int n, uint[] pipelines);
        [DllImport(lib, EntryPoint = "glCreateQueries")]
        public extern static void CreateQueries(QueryTarget target, int n, uint[] ids);
        [DllImport(lib, EntryPoint = "glCreateRenderbuffers")]
        public extern static void CreateRenderbuffers(int n, uint[] renderbuffers);
        [DllImport(lib, EntryPoint = "glCreateSamplers")]
        public extern static void CreateSamplers(int n, uint[] samplers);
        [DllImport(lib, EntryPoint = "glCreateShader")]
        public extern static uint CreateShader(ShaderType shaderType);
        [DllImport(lib, EntryPoint = "glCreateShaderProgramv")]
        public extern static uint CreateShaderProgramv(ShaderType type, int count, string strings);
        [DllImport(lib, EntryPoint = "glCreateTextures")]
        public extern static void CreateTextures(TextureTarget target, int n, uint[] textures);
        [DllImport(lib, EntryPoint = "glCreateTransformFeedbacks")]
        public extern static void CreateTransformFeedbacks(int n, uint[] ids);
        [DllImport(lib, EntryPoint = "glCreateVertexArrays")]
        public extern static void CreateVertexArrays(int n, uint[] arrays);
        [DllImport(lib, EntryPoint = "glCullFace")]
        public extern static void CullFace(CullFaceMode mode);
        [DllImport(lib, EntryPoint = "glDeleteBuffers")]
        public extern static void DeleteBuffers(int n, uint[] buffers);
        [DllImport(lib, EntryPoint = "glDeleteFramebuffers")]
        public extern static void DeleteFramebuffers(int n, uint[] framebuffers);
        [DllImport(lib, EntryPoint = "glDeleteProgram")]
        public extern static void DeleteProgram(uint program);
        [DllImport(lib, EntryPoint = "glDeleteProgramPipelines")]
        public extern static void DeleteProgramPipelines(int n, uint[] pipelines);
        [DllImport(lib, EntryPoint = "glDeleteQueries")]
        public extern static void DeleteQueries(int n, uint[] ids);
        [DllImport(lib, EntryPoint = "glDeleteRenderbuffers")]
        public extern static void DeleteRenderbuffers(int n, uint[] renderbuffers);
        [DllImport(lib, EntryPoint = "glDeleteSamplers")]
        public extern static void DeleteSamplers(int n, uint[] samplers);
        [DllImport(lib, EntryPoint = "glDeleteShader")]
        public extern static void DeleteShader(uint shader);
        [DllImport(lib, EntryPoint = "glDeleteSync")]
        public extern static void DeleteSync(IntPtr sync);
        [DllImport(lib, EntryPoint = "glDeleteTextures")]
        public extern static void DeleteTextures(int n, uint[] textures);
        [DllImport(lib, EntryPoint = "glDeleteTransformFeedbacks")]
        public extern static void DeleteTransformFeedbacks(int n, uint[] ids);
        [DllImport(lib, EntryPoint = "glDeleteVertexArrays")]
        public extern static void DeleteVertexArrays(int n, uint[] arrays);
        [DllImport(lib, EntryPoint = "glDepthFunc")]
        public extern static void DepthFunc(DepthFunction func);
        [DllImport(lib, EntryPoint = "glDepthMask")]
        public extern static void DepthMask(bool flag);
        [DllImport(lib, EntryPoint = "glDepthRange")]
        public extern static void DepthRange(double nearVal, double farVal);
        [DllImport(lib, EntryPoint = "glDepthRangef")]
        public extern static void DepthRangef(float nearVal, float farVal);
        [DllImport(lib, EntryPoint = "glDepthRangeArrayv")]
        public extern static void DepthRangeArrayv(uint first, int count, double[] v);
        [DllImport(lib, EntryPoint = "glDepthRangeIndexed")]
        public extern static void DepthRangeIndexed(uint index, double nearVal, double farVal);
        [DllImport(lib, EntryPoint = "glDetachShader")]
        public extern static void DetachShader(uint program, uint shader);
        [DllImport(lib, EntryPoint = "glDispatchCompute")]
        public extern static void DispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z);
        [DllImport(lib, EntryPoint = "glDispatchComputeIndirect")]
        public extern static void DispatchComputeIndirect(IntPtr indirect);
        [DllImport(lib, EntryPoint = "glDrawArrays")]
        public extern static void DrawArrays(BeginMode mode, int first, int count);
        [DllImport(lib, EntryPoint = "glDrawArraysIndirect")]
        public extern static void DrawArraysIndirect(BeginMode mode, IntPtr indirect);
        [DllImport(lib, EntryPoint = "glDrawArraysInstanced")]
        public extern static void DrawArraysInstanced(BeginMode mode, int first, int count, int primcount);
        [DllImport(lib, EntryPoint = "glDrawArraysInstancedBaseInstance")]
        public extern static void DrawArraysInstancedBaseInstance(BeginMode mode, int first, int count, int primcount, uint baseinstance);
        [DllImport(lib, EntryPoint = "glDrawBuffer")]
        public extern static void DrawBuffer(DrawBufferMode buf);
        [DllImport(lib, EntryPoint = "glNamedFramebufferDrawBuffer")]
        public extern static void NamedFramebufferDrawBuffer(uint framebuffer, DrawBufferMode buf);
        [DllImport(lib, EntryPoint = "glDrawBuffers")]
        public extern static void DrawBuffers(int n, DrawBuffersEnum[] bufs);
        [DllImport(lib, EntryPoint = "glNamedFramebufferDrawBuffers")]
        public extern static void NamedFramebufferDrawBuffers(uint framebuffer, int n, DrawBufferMode[] bufs);
        [DllImport(lib, EntryPoint = "glDrawElements")]
        public extern static void DrawElements(BeginMode mode, int count, DrawElementsType type, IntPtr indices);
        [DllImport(lib, EntryPoint = "glDrawElementsBaseVertex")]
        public extern static void DrawElementsBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int basevertex);
        [DllImport(lib, EntryPoint = "glDrawElementsIndirect")]
        public extern static void DrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect);
        [DllImport(lib, EntryPoint = "glDrawElementsInstanced")]
        public extern static void DrawElementsInstanced(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount);
        [DllImport(lib, EntryPoint = "glDrawElementsInstancedBaseInstance")]
        public extern static void DrawElementsInstancedBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, uint baseinstance);
        [DllImport(lib, EntryPoint = "glDrawElementsInstancedBaseVertex")]
        public extern static void DrawElementsInstancedBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex);
        [DllImport(lib, EntryPoint = "glDrawElementsInstancedBaseVertexBaseInstance")]
        public extern static void DrawElementsInstancedBaseVertexBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex, uint baseinstance);
        [DllImport(lib, EntryPoint = "glDrawRangeElements")]
        public extern static void DrawRangeElements(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices);
        [DllImport(lib, EntryPoint = "glDrawRangeElementsBaseVertex")]
        public extern static void DrawRangeElementsBaseVertex(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices, int basevertex);
        [DllImport(lib, EntryPoint = "glDrawTransformFeedback")]
        public extern static void DrawTransformFeedback(NvTransformFeedback2 mode, uint id);
        [DllImport(lib, EntryPoint = "glDrawTransformFeedbackInstanced")]
        public extern static void DrawTransformFeedbackInstanced(BeginMode mode, uint id, int primcount);
        [DllImport(lib, EntryPoint = "glDrawTransformFeedbackStream")]
        public extern static void DrawTransformFeedbackStream(NvTransformFeedback2 mode, uint id, uint stream);
        [DllImport(lib, EntryPoint = "glDrawTransformFeedbackStreamInstanced")]
        public extern static void DrawTransformFeedbackStreamInstanced(BeginMode mode, uint id, uint stream, int primcount);
        [DllImport(lib, EntryPoint = "glEnable")]
        public extern static void Enable(EnableCap cap);
        [DllImport(lib, EntryPoint = "glDisable")]
        public extern static void Disable(EnableCap cap);
        [DllImport(lib, EntryPoint = "glEnablei")]
        public extern static void Enablei(EnableCap cap, uint index);
        [DllImport(lib, EntryPoint = "glDisablei")]
        public extern static void Disablei(EnableCap cap, uint index);
        [DllImport(lib, EntryPoint = "glEnableVertexAttribArray")]
        public extern static void EnableVertexAttribArray(uint index);
        [DllImport(lib, EntryPoint = "glDisableVertexAttribArray")]
        public extern static void DisableVertexAttribArray(uint index);
        [DllImport(lib, EntryPoint = "glEnableVertexArrayAttrib")]
        public extern static void EnableVertexArrayAttrib(uint vaobj, uint index);
        [DllImport(lib, EntryPoint = "glDisableVertexArrayAttrib")]
        public extern static void DisableVertexArrayAttrib(uint vaobj, uint index);
        [DllImport(lib, EntryPoint = "glFenceSync")]
        public extern static IntPtr FenceSync(ArbSync condition, uint flags);
        [DllImport(lib, EntryPoint = "glFinish")]
        public extern static void Finish();
        [DllImport(lib, EntryPoint = "glFlush")]
        public extern static void Flush();
        [DllImport(lib, EntryPoint = "glFlushMappedBufferRange")]
        public extern static void FlushMappedBufferRange(BufferTarget target, IntPtr offset, IntPtr length);
        [DllImport(lib, EntryPoint = "glFlushMappedNamedBufferRange")]
        public extern static void FlushMappedNamedBufferRange(uint buffer, IntPtr offset, int length);
        [DllImport(lib, EntryPoint = "glFramebufferParameteri")]
        public extern static void FramebufferParameteri(FramebufferTarget target, FramebufferPName pname, int param);
        [DllImport(lib, EntryPoint = "glNamedFramebufferParameteri")]
        public extern static void NamedFramebufferParameteri(uint framebuffer, FramebufferPName pname, int param);
        [DllImport(lib, EntryPoint = "glFramebufferRenderbuffer")]
        public extern static void FramebufferRenderbuffer(FramebufferTarget target, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer);
        [DllImport(lib, EntryPoint = "glNamedFramebufferRenderbuffer")]
        public extern static void NamedFramebufferRenderbuffer(uint framebuffer, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer);
        [DllImport(lib, EntryPoint = "glFramebufferTexture")]
        public extern static void FramebufferTexture(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level);
        [DllImport(lib, EntryPoint = "glFramebufferTexture1D")]
        public extern static void FramebufferTexture1D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level);
        [DllImport(lib, EntryPoint = "glFramebufferTexture2D")]
        public extern static void FramebufferTexture2D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level);
        [DllImport(lib, EntryPoint = "glFramebufferTexture3D")]
        public extern static void FramebufferTexture3D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level, int layer);
        [DllImport(lib, EntryPoint = "glNamedFramebufferTexture")]
        public extern static void NamedFramebufferTexture(uint framebuffer, FramebufferAttachment attachment, uint texture, int level);
        [DllImport(lib, EntryPoint = "glFramebufferTextureLayer")]
        public extern static void FramebufferTextureLayer(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level, int layer);
        [DllImport(lib, EntryPoint = "glNamedFramebufferTextureLayer")]
        public extern static void NamedFramebufferTextureLayer(uint framebuffer, FramebufferAttachment attachment, uint texture, int level, int layer);
        [DllImport(lib, EntryPoint = "glFrontFace")]
        public extern static void FrontFace(FrontFaceDirection mode);
        [DllImport(lib, EntryPoint = "glGenBuffers")]
        public extern static void GenBuffers(int n, [Out] uint[] buffers);
        [DllImport(lib, EntryPoint = "glGenerateMipmap")]
        public extern static void GenerateMipmap(TextureTarget target);
        [DllImport(lib, EntryPoint = "glGenerateTextureMipmap")]
        public extern static void GenerateTextureMipmap(uint texture);
        [DllImport(lib, EntryPoint = "glGenFramebuffers")]
        public extern static void GenFramebuffers(int n, [Out] uint[] ids);
        [DllImport(lib, EntryPoint = "glGenProgramPipelines")]
        public extern static void GenProgramPipelines(int n, [Out] uint[] pipelines);
        [DllImport(lib, EntryPoint = "glGenQueries")]
        public extern static void GenQueries(int n, [Out] uint[] ids);
        [DllImport(lib, EntryPoint = "glGenRenderbuffers")]
        public extern static void GenRenderbuffers(int n, [Out] uint[] renderbuffers);
        [DllImport(lib, EntryPoint = "glGenSamplers")]
        public extern static void GenSamplers(int n, [Out] uint[] samplers);
        [DllImport(lib, EntryPoint = "glGenTextures")]
        public extern static void GenTextures(int n, [Out] uint[] textures);
        [DllImport(lib, EntryPoint = "glGenTransformFeedbacks")]
        public extern static void GenTransformFeedbacks(int n, [Out] uint[] ids);
        [DllImport(lib, EntryPoint = "glGenVertexArrays")]
        public extern static void GenVertexArrays(int n, [Out] uint[] arrays);
        [DllImport(lib, EntryPoint = "glGetboolv")]
        public extern static void Getboolv(GetPName pname, [Out] bool[] data);
        [DllImport(lib, EntryPoint = "glGetdoublev")]
        public extern static void Getdoublev(GetPName pname, [Out] double[] data);
        [DllImport(lib, EntryPoint = "glGetFloatv")]
        public extern static void GetFloatv(GetPName pname, [Out] float[] data);
        [DllImport(lib, EntryPoint = "glGetIntegerv")]
        public extern static void GetIntegerv(GetPName pname, [Out] int[] data);
        [DllImport(lib, EntryPoint = "glGetInteger64v")]
        public extern static void GetInteger64v(ArbSync pname, [Out] long[] data);
        [DllImport(lib, EntryPoint = "glGetbooli_v")]
        public extern static void Getbooli_v(GetPName target, uint index, [Out] bool[] data);
        [DllImport(lib, EntryPoint = "glGetIntegeri_v")]
        public extern static void GetIntegeri_v(GetPName target, uint index, [Out] int[] data);
        [DllImport(lib, EntryPoint = "glGetFloati_v")]
        public extern static void GetFloati_v(GetPName target, uint index, [Out] float[] data);
        [DllImport(lib, EntryPoint = "glGetdoublei_v")]
        public extern static void Getdoublei_v(GetPName target, uint index, [Out] double[] data);
        [DllImport(lib, EntryPoint = "glGetInteger64i_v")]
        public extern static void GetInteger64i_v(GetPName target, uint index, [Out] long[] data);
        [DllImport(lib, EntryPoint = "glGetActiveAtomicCounterBufferiv")]
        public extern static void GetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, AtomicCounterParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetActiveAttrib")]
        public extern static void GetActiveAttrib(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] System.Text.StringBuilder name);
        [DllImport(lib, EntryPoint = "glGetActiveSubroutineName")]
        public extern static void GetActiveSubroutineName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] System.Text.StringBuilder name);
        [DllImport(lib, EntryPoint = "glGetActiveSubroutineUniformiv")]
        public extern static void GetActiveSubroutineUniformiv(uint program, ShaderType shadertype, uint index, SubroutineParameterName pname, [Out] int[] values);
        [DllImport(lib, EntryPoint = "glGetActiveSubroutineUniformName")]
        public extern static void GetActiveSubroutineUniformName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] System.Text.StringBuilder name);
        [DllImport(lib, EntryPoint = "glGetActiveUniform")]
        public extern static void GetActiveUniform(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveUniformType[] type, [Out] System.Text.StringBuilder name);
        [DllImport(lib, EntryPoint = "glGetActiveUniformBlockiv")]
        public extern static void GetActiveUniformBlockiv(uint program, uint uniformBlockIndex, ActiveUniformBlockParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetActiveUniformBlockName")]
        public extern static void GetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder uniformBlockName);
        [DllImport(lib, EntryPoint = "glGetActiveUniformName")]
        public extern static void GetActiveUniformName(uint program, uint uniformIndex, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder uniformName);
        [DllImport(lib, EntryPoint = "glGetActiveUniformsiv")]
        public extern static void GetActiveUniformsiv(uint program, int uniformCount, [Out] uint[] uniformIndices, ActiveUniformType pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetAttachedShaders")]
        public extern static void GetAttachedShaders(uint program, int maxCount, [Out] int[] count, [Out] uint[] shaders);
        [DllImport(lib, EntryPoint = "glGetAttribLocation")]
        public extern static int GetAttribLocation(uint program, string name);
        [DllImport(lib, EntryPoint = "glGetBufferParameteriv")]
        public extern static void GetBufferParameteriv(BufferTarget target, BufferParameterName value, [Out] int[] data);
        [DllImport(lib, EntryPoint = "glGetBufferParameteri64v")]
        public extern static void GetBufferParameteri64v(BufferTarget target, BufferParameterName value, [Out] long[] data);
        [DllImport(lib, EntryPoint = "glGetNamedBufferParameteriv")]
        public extern static void GetNamedBufferParameteriv(uint buffer, BufferParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetNamedBufferParameteri64v")]
        public extern static void GetNamedBufferParameteri64v(uint buffer, BufferParameterName pname, [Out] long[] @params);
        [DllImport(lib, EntryPoint = "glGetBufferPointerv")]
        public extern static void GetBufferPointerv(BufferTarget target, BufferPointer pname, [Out] IntPtr @params);
        [DllImport(lib, EntryPoint = "glGetNamedBufferPointerv")]
        public extern static void GetNamedBufferPointerv(uint buffer, BufferPointer pname, [Out] IntPtr @params);
        [DllImport(lib, EntryPoint = "glGetBufferSubData")]
        public extern static void GetBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, [Out] IntPtr data);
        [DllImport(lib, EntryPoint = "glGetNamedBufferSubData")]
        public extern static void GetNamedBufferSubData(uint buffer, IntPtr offset, int size, [Out] IntPtr data);
        [DllImport(lib, EntryPoint = "glGetCompressedTexImage")]
        public extern static void GetCompressedTexImage(TextureTarget target, int level, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetnCompressedTexImage")]
        public extern static void GetnCompressedTexImage(TextureTarget target, int level, int bufSize, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetCompressedTextureImage")]
        public extern static void GetCompressedTextureImage(uint texture, int level, int bufSize, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetCompressedTextureSubImage")]
        public extern static void GetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetError")]
        public extern static ErrorCode GetError();
        [DllImport(lib, EntryPoint = "glGetFragDataIndex")]
        public extern static int GetFragDataIndex(uint program, string name);
        [DllImport(lib, EntryPoint = "glGetFragDataLocation")]
        public extern static int GetFragDataLocation(uint program, string name);
        [DllImport(lib, EntryPoint = "glGetFramebufferAttachmentParameteriv")]
        public extern static void GetFramebufferAttachmentParameteriv(FramebufferTarget target, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetNamedFramebufferAttachmentParameteriv")]
        public extern static void GetNamedFramebufferAttachmentParameteriv(uint framebuffer, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetFramebufferParameteriv")]
        public extern static void GetFramebufferParameteriv(FramebufferTarget target, FramebufferPName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetNamedFramebufferParameteriv")]
        public extern static void GetNamedFramebufferParameteriv(uint framebuffer, FramebufferPName pname, [Out] int[] param);
        [DllImport(lib, EntryPoint = "glGetGraphicsResetStatus")]
        public extern static GraphicResetStatus GetGraphicsResetStatus();
        [DllImport(lib, EntryPoint = "glGetInternalformativ")]
        public extern static void GetInternalformativ(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetInternalformati64v")]
        public extern static void GetInternalformati64v(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] long[] @params);
        [DllImport(lib, EntryPoint = "glGetMultisamplefv")]
        public extern static void GetMultisamplefv(GetMultisamplePName pname, uint index, [Out] float[] val);
        [DllImport(lib, EntryPoint = "glGetObjectLabel")]
        public extern static void GetObjectLabel(ObjectLabelEnum identifier, uint name, int bifSize, [Out] int[] length, [Out] System.Text.StringBuilder label);
        [DllImport(lib, EntryPoint = "glGetObjectPtrLabel")]
        public extern static void GetObjectPtrLabel([Out] IntPtr ptr, int bifSize, [Out] int[] length, [Out] System.Text.StringBuilder label);
        [DllImport(lib, EntryPoint = "glGetPointerv")]
        public extern static void GetPointerv(GetPointerParameter pname, [Out] IntPtr @params);
        [DllImport(lib, EntryPoint = "glGetProgramiv")]
        public extern static void GetProgramiv(uint program, ProgramParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetProgramBinary")]
        public extern static void GetProgramBinary(uint program, int bufsize, [Out] int[] length, [Out] int[] binaryFormat, [Out] IntPtr binary);
        [DllImport(lib, EntryPoint = "glGetProgramInfoLog")]
        public extern static void GetProgramInfoLog(uint program, int maxLength, [Out] int[] length, [Out] System.Text.StringBuilder infoLog);
        [DllImport(lib, EntryPoint = "glGetProgramInterfaceiv")]
        public extern static void GetProgramInterfaceiv(uint program, ProgramInterface programInterface, ProgramInterfaceParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetProgramPipelineiv")]
        public extern static void GetProgramPipelineiv(uint pipeline, int pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetProgramPipelineInfoLog")]
        public extern static void GetProgramPipelineInfoLog(uint pipeline, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder infoLog);
        [DllImport(lib, EntryPoint = "glGetProgramResourceiv")]
        public extern static void GetProgramResourceiv(uint program, ProgramInterface programInterface, uint index, int propCount, [Out] ProgramResourceParameterName[] props, int bufSize, [Out] int[] length, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetProgramResourceIndex")]
        public extern static uint GetProgramResourceIndex(uint program, ProgramInterface programInterface, string name);
        [DllImport(lib, EntryPoint = "glGetProgramResourceLocation")]
        public extern static int GetProgramResourceLocation(uint program, ProgramInterface programInterface, string name);
        [DllImport(lib, EntryPoint = "glGetProgramResourceLocationIndex")]
        public extern static int GetProgramResourceLocationIndex(uint program, ProgramInterface programInterface, string name);
        [DllImport(lib, EntryPoint = "glGetProgramResourceName")]
        public extern static void GetProgramResourceName(uint program, ProgramInterface programInterface, uint index, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder name);
        [DllImport(lib, EntryPoint = "glGetProgramStageiv")]
        public extern static void GetProgramStageiv(uint program, ShaderType shadertype, ProgramStageParameterName pname, [Out] int[] values);
        [DllImport(lib, EntryPoint = "glGetQueryIndexediv")]
        public extern static void GetQueryIndexediv(QueryTarget target, uint index, GetQueryParam pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetQueryiv")]
        public extern static void GetQueryiv(QueryTarget target, GetQueryParam pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetQueryObjectiv")]
        public extern static void GetQueryObjectiv(uint id, GetQueryObjectParam pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetQueryObjectuiv")]
        public extern static void GetQueryObjectuiv(uint id, GetQueryObjectParam pname, [Out] uint[] @params);
        [DllImport(lib, EntryPoint = "glGetQueryObjecti64v")]
        public extern static void GetQueryObjecti64v(uint id, GetQueryObjectParam pname, [Out] long[] @params);
        [DllImport(lib, EntryPoint = "glGetQueryObjectui64v")]
        public extern static void GetQueryObjectui64v(uint id, GetQueryObjectParam pname, [Out] ulong[] @params);
        [DllImport(lib, EntryPoint = "glGetRenderbufferParameteriv")]
        public extern static void GetRenderbufferParameteriv(RenderbufferTarget target, RenderbufferParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetNamedRenderbufferParameteriv")]
        public extern static void GetNamedRenderbufferParameteriv(uint renderbuffer, RenderbufferParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetSamplerParameterfv")]
        public extern static void GetSamplerParameterfv(uint sampler, int pname, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetSamplerParameteriv")]
        public extern static void GetSamplerParameteriv(uint sampler, int pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetSamplerParameterIiv")]
        public extern static void GetSamplerParameterIiv(uint sampler, TextureParameterName pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetSamplerParameterIuiv")]
        public extern static void GetSamplerParameterIuiv(uint sampler, TextureParameterName pname, [Out] uint[] @params);
        [DllImport(lib, EntryPoint = "glGetShaderiv")]
        public extern static void GetShaderiv(uint shader, ShaderParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetShaderInfoLog")]
        public extern static void GetShaderInfoLog(uint shader, int maxLength, [Out] int[] length, [Out] System.Text.StringBuilder infoLog);
        [DllImport(lib, EntryPoint = "glGetShaderPrecisionFormat")]
        public extern static void GetShaderPrecisionFormat(ShaderType shaderType, int precisionType, [Out] int[] range, [Out] int[] precision);
        [DllImport(lib, EntryPoint = "glGetShaderSource")]
        public extern static void GetShaderSource(uint shader, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder source);
        [DllImport(lib, EntryPoint = "glGetString")]
        public extern static IntPtr GetString(StringName name);
        [DllImport(lib, EntryPoint = "glGetStringi")]
        public extern static IntPtr GetStringi(StringName name, uint index);
        [DllImport(lib, EntryPoint = "glGetSubroutineIndex")]
        public extern static uint GetSubroutineIndex(uint program, ShaderType shadertype, string name);
        [DllImport(lib, EntryPoint = "glGetSubroutineUniformLocation")]
        public extern static int GetSubroutineUniformLocation(uint program, ShaderType shadertype, string name);
        [DllImport(lib, EntryPoint = "glGetSynciv")]
        public extern static void GetSynciv(IntPtr sync, ArbSync pname, int bufSize, [Out] int[] length, [Out] int[] values);
        [DllImport(lib, EntryPoint = "glGetTexImage")]
        public extern static void GetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetnTexImage")]
        public extern static void GetnTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetTextureImage")]
        public extern static void GetTextureImage(uint texture, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetTexLevelParameterfv")]
        public extern static void GetTexLevelParameterfv(GetPName target, int level, GetTextureLevelParameter pname, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetTexLevelParameteriv")]
        public extern static void GetTexLevelParameteriv(GetPName target, int level, GetTextureLevelParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetTextureLevelParameterfv")]
        public extern static void GetTextureLevelParameterfv(uint texture, int level, GetTextureLevelParameter pname, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetTextureLevelParameteriv")]
        public extern static void GetTextureLevelParameteriv(uint texture, int level, GetTextureLevelParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetTexParameterfv")]
        public extern static void GetTexParameterfv(TextureTarget target, GetTextureParameter pname, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetTexParameteriv")]
        public extern static void GetTexParameteriv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetTexParameterIiv")]
        public extern static void GetTexParameterIiv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetTexParameterIuiv")]
        public extern static void GetTexParameterIuiv(TextureTarget target, GetTextureParameter pname, [Out] uint[] @params);
        [DllImport(lib, EntryPoint = "glGetTextureParameterfv")]
        public extern static void GetTextureParameterfv(uint texture, GetTextureParameter pname, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetTextureParameteriv")]
        public extern static void GetTextureParameteriv(uint texture, GetTextureParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetTextureParameterIiv")]
        public extern static void GetTextureParameterIiv(uint texture, GetTextureParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetTextureParameterIuiv")]
        public extern static void GetTextureParameterIuiv(uint texture, GetTextureParameter pname, [Out] uint[] @params);
        [DllImport(lib, EntryPoint = "glGetTextureSubImage")]
        public extern static void GetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels);
        [DllImport(lib, EntryPoint = "glGetTransformFeedbackiv")]
        public extern static void GetTransformFeedbackiv(uint xfb, TransformFeedbackParameterName pname, [Out] int[] param);
        [DllImport(lib, EntryPoint = "glGetTransformFeedbacki_v")]
        public extern static void GetTransformFeedbacki_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] int[] param);
        [DllImport(lib, EntryPoint = "glGetTransformFeedbacki64_v")]
        public extern static void GetTransformFeedbacki64_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] long[] param);
        [DllImport(lib, EntryPoint = "glGetTransformFeedbackVarying")]
        public extern static void GetTransformFeedbackVarying(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] System.Text.StringBuilder name);
        [DllImport(lib, EntryPoint = "glGetUniformfv")]
        public extern static void GetUniformfv(uint program, int location, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetUniformiv")]
        public extern static void GetUniformiv(uint program, int location, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetUniformuiv")]
        public extern static void GetUniformuiv(uint program, int location, [Out] uint[] @params);
        [DllImport(lib, EntryPoint = "glGetUniformdv")]
        public extern static void GetUniformdv(uint program, int location, [Out] double[] @params);
        [DllImport(lib, EntryPoint = "glGetnUniformfv")]
        public extern static void GetnUniformfv(uint program, int location, int bufSize, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetnUniformiv")]
        public extern static void GetnUniformiv(uint program, int location, int bufSize, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetnUniformuiv")]
        public extern static void GetnUniformuiv(uint program, int location, int bufSize, [Out] uint[] @params);
        [DllImport(lib, EntryPoint = "glGetnUniformdv")]
        public extern static void GetnUniformdv(uint program, int location, int bufSize, [Out] double[] @params);
        [DllImport(lib, EntryPoint = "glGetUniformBlockIndex")]
        public extern static uint GetUniformBlockIndex(uint program, string uniformBlockName);
        [DllImport(lib, EntryPoint = "glGetUniformIndices")]
        public extern static void GetUniformIndices(uint program, int uniformCount, string uniformNames, [Out] uint[] uniformIndices);
        [DllImport(lib, EntryPoint = "glGetUniformLocation")]
        public extern static int GetUniformLocation(uint program, string name);
        [DllImport(lib, EntryPoint = "glGetUniformSubroutineuiv")]
        public extern static void GetUniformSubroutineuiv(ShaderType shadertype, int location, [Out] uint[] values);
        [DllImport(lib, EntryPoint = "glGetVertexArrayIndexed64iv")]
        public extern static void GetVertexArrayIndexed64iv(uint vaobj, uint index, VertexAttribParameter pname, [Out] long[] param);
        [DllImport(lib, EntryPoint = "glGetVertexArrayIndexediv")]
        public extern static void GetVertexArrayIndexediv(uint vaobj, uint index, VertexAttribParameter pname, [Out] int[] param);
        [DllImport(lib, EntryPoint = "glGetVertexArrayiv")]
        public extern static void GetVertexArrayiv(uint vaobj, VertexAttribParameter pname, [Out] int[] param);
        [DllImport(lib, EntryPoint = "glGetVertexAttribdv")]
        public extern static void GetVertexAttribdv(uint index, VertexAttribParameter pname, [Out] double[] @params);
        [DllImport(lib, EntryPoint = "glGetVertexAttribfv")]
        public extern static void GetVertexAttribfv(uint index, VertexAttribParameter pname, [Out] float[] @params);
        [DllImport(lib, EntryPoint = "glGetVertexAttribiv")]
        public extern static void GetVertexAttribiv(uint index, VertexAttribParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetVertexAttribIiv")]
        public extern static void GetVertexAttribIiv(uint index, VertexAttribParameter pname, [Out] int[] @params);
        [DllImport(lib, EntryPoint = "glGetVertexAttribIuiv")]
        public extern static void GetVertexAttribIuiv(uint index, VertexAttribParameter pname, [Out] uint[] @params);
        [DllImport(lib, EntryPoint = "glGetVertexAttribLdv")]
        public extern static void GetVertexAttribLdv(uint index, VertexAttribParameter pname, [Out] double[] @params);
        [DllImport(lib, EntryPoint = "glGetVertexAttribPointerv")]
        public extern static void GetVertexAttribPointerv(uint index, VertexAttribPointerParameter pname, [Out] IntPtr pointer);
        [DllImport(lib, EntryPoint = "glHint")]
        public extern static void Hint(HintTarget target, HintMode mode);
        [DllImport(lib, EntryPoint = "glInvalidateBufferData")]
        public extern static void InvalidateBufferData(uint buffer);
        [DllImport(lib, EntryPoint = "glInvalidateBufferSubData")]
        public extern static void InvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length);
        [DllImport(lib, EntryPoint = "glInvalidateFramebuffer")]
        public extern static void InvalidateFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments);
        [DllImport(lib, EntryPoint = "glInvalidateNamedFramebufferData")]
        public extern static void InvalidateNamedFramebufferData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments);
        [DllImport(lib, EntryPoint = "glInvalidateSubFramebuffer")]
        public extern static void InvalidateSubFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glInvalidateNamedFramebufferSubData")]
        public extern static void InvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glInvalidateTexImage")]
        public extern static void InvalidateTexImage(uint texture, int level);
        [DllImport(lib, EntryPoint = "glInvalidateTexSubImage")]
        public extern static void InvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth);
        [DllImport(lib, EntryPoint = "glIsBuffer")]
        public extern static bool IsBuffer(uint buffer);
        [DllImport(lib, EntryPoint = "glIsEnabled")]
        public extern static bool IsEnabled(EnableCap cap);
        [DllImport(lib, EntryPoint = "glIsEnabledi")]
        public extern static bool IsEnabledi(EnableCap cap, uint index);
        [DllImport(lib, EntryPoint = "glIsFramebuffer")]
        public extern static bool IsFramebuffer(uint framebuffer);
        [DllImport(lib, EntryPoint = "glIsProgram")]
        public extern static bool IsProgram(uint program);
        [DllImport(lib, EntryPoint = "glIsProgramPipeline")]
        public extern static bool IsProgramPipeline(uint pipeline);
        [DllImport(lib, EntryPoint = "glIsQuery")]
        public extern static bool IsQuery(uint id);
        [DllImport(lib, EntryPoint = "glIsRenderbuffer")]
        public extern static bool IsRenderbuffer(uint renderbuffer);
        [DllImport(lib, EntryPoint = "glIsSampler")]
        public extern static bool IsSampler(uint id);
        [DllImport(lib, EntryPoint = "glIsShader")]
        public extern static bool IsShader(uint shader);
        [DllImport(lib, EntryPoint = "glIsSync")]
        public extern static bool IsSync(IntPtr sync);
        [DllImport(lib, EntryPoint = "glIsTexture")]
        public extern static bool IsTexture(uint texture);
        [DllImport(lib, EntryPoint = "glIsTransformFeedback")]
        public extern static bool IsTransformFeedback(uint id);
        [DllImport(lib, EntryPoint = "glIsVertexArray")]
        public extern static bool IsVertexArray(uint array);
        [DllImport(lib, EntryPoint = "glLineWidth")]
        public extern static void LineWidth(float width);
        [DllImport(lib, EntryPoint = "glLinkProgram")]
        public extern static void LinkProgram(uint program);
        [DllImport(lib, EntryPoint = "glLogicOp")]
        public extern static void LogicOp(LogicOpEnum opcode);
        [DllImport(lib, EntryPoint = "glMapBuffer")]
        public extern static IntPtr MapBuffer(BufferTarget target, BufferAccess access);
        [DllImport(lib, EntryPoint = "glMapNamedBuffer")]
        public extern static IntPtr MapNamedBuffer(uint buffer, BufferAccess access);
        [DllImport(lib, EntryPoint = "glMapBufferRange")]
        public extern static IntPtr MapBufferRange(BufferTarget target, IntPtr offset, IntPtr length, BufferAccessMask access);
        [DllImport(lib, EntryPoint = "glMapNamedBufferRange")]
        public extern static IntPtr MapNamedBufferRange(uint buffer, IntPtr offset, int length, uint access);
        [DllImport(lib, EntryPoint = "glMemoryBarrier")]
        public extern static void MemoryBarrier(uint barriers);
        [DllImport(lib, EntryPoint = "glMemoryBarrierByRegion")]
        public extern static void MemoryBarrierByRegion(uint barriers);
        [DllImport(lib, EntryPoint = "glMinSampleShading")]
        public extern static void MinSampleShading(float value);
        [DllImport(lib, EntryPoint = "glMultiDrawArrays")]
        public extern static void MultiDrawArrays(BeginMode mode, int[] first, int[] count, int drawcount);
        [DllImport(lib, EntryPoint = "glMultiDrawArraysIndirect")]
        public extern static void MultiDrawArraysIndirect(BeginMode mode, IntPtr indirect, int drawcount, int stride);
        [DllImport(lib, EntryPoint = "glMultiDrawElements")]
        public extern static void MultiDrawElements(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount);
        [DllImport(lib, EntryPoint = "glMultiDrawElementsBaseVertex")]
        public extern static void MultiDrawElementsBaseVertex(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount, int[] basevertex);
        [DllImport(lib, EntryPoint = "glMultiDrawElementsIndirect")]
        public extern static void MultiDrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect, int drawcount, int stride);
        [DllImport(lib, EntryPoint = "glObjectLabel")]
        public extern static void ObjectLabel(ObjectLabelEnum identifier, uint name, int length, string label);
        [DllImport(lib, EntryPoint = "glObjectPtrLabel")]
        public extern static void ObjectPtrLabel(IntPtr ptr, int length, string label);
        [DllImport(lib, EntryPoint = "glPatchParameteri")]
        public extern static void PatchParameteri(int pname, int value);
        [DllImport(lib, EntryPoint = "glPatchParameterfv")]
        public extern static void PatchParameterfv(int pname, float[] values);
        [DllImport(lib, EntryPoint = "glPixelStoref")]
        public extern static void PixelStoref(PixelStoreParameter pname, float param);
        [DllImport(lib, EntryPoint = "glPixelStorei")]
        public extern static void PixelStorei(PixelStoreParameter pname, int param);
        [DllImport(lib, EntryPoint = "glPointParameterf")]
        public extern static void PointParameterf(PointParameterName pname, float param);
        [DllImport(lib, EntryPoint = "glPointParameteri")]
        public extern static void PointParameteri(PointParameterName pname, int param);
        [DllImport(lib, EntryPoint = "glPointParameterfv")]
        public extern static void PointParameterfv(PointParameterName pname, float[] @params);
        [DllImport(lib, EntryPoint = "glPointParameteriv")]
        public extern static void PointParameteriv(PointParameterName pname, int[] @params);
        [DllImport(lib, EntryPoint = "glPointSize")]
        public extern static void PointSize(float size);
        [DllImport(lib, EntryPoint = "glPolygonMode")]
        public extern static void PolygonMode(MaterialFace face, PolygonModeEnum mode);
        [DllImport(lib, EntryPoint = "glPolygonOffset")]
        public extern static void PolygonOffset(float factor, float units);
        [DllImport(lib, EntryPoint = "glPrimitiveRestartIndex")]
        public extern static void PrimitiveRestartIndex(uint index);
        [DllImport(lib, EntryPoint = "glProgramBinary")]
        public extern static void ProgramBinary(uint program, int binaryFormat, IntPtr binary, int length);
        [DllImport(lib, EntryPoint = "glProgramParameteri")]
        public extern static void ProgramParameteri(uint program, Version32 pname, int value);
        [DllImport(lib, EntryPoint = "glProgramUniform1f")]
        public extern static void ProgramUniform1f(uint program, int location, float v0);
        [DllImport(lib, EntryPoint = "glProgramUniform2f")]
        public extern static void ProgramUniform2f(uint program, int location, float v0, float v1);
        [DllImport(lib, EntryPoint = "glProgramUniform3f")]
        public extern static void ProgramUniform3f(uint program, int location, float v0, float v1, float v2);
        [DllImport(lib, EntryPoint = "glProgramUniform4f")]
        public extern static void ProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3);
        [DllImport(lib, EntryPoint = "glProgramUniform1i")]
        public extern static void ProgramUniform1i(uint program, int location, int v0);
        [DllImport(lib, EntryPoint = "glProgramUniform2i")]
        public extern static void ProgramUniform2i(uint program, int location, int v0, int v1);
        [DllImport(lib, EntryPoint = "glProgramUniform3i")]
        public extern static void ProgramUniform3i(uint program, int location, int v0, int v1, int v2);
        [DllImport(lib, EntryPoint = "glProgramUniform4i")]
        public extern static void ProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3);
        [DllImport(lib, EntryPoint = "glProgramUniform1ui")]
        public extern static void ProgramUniform1ui(uint program, int location, uint v0);
        [DllImport(lib, EntryPoint = "glProgramUniform2ui")]
        public extern static void ProgramUniform2ui(uint program, int location, int v0, uint v1);
        [DllImport(lib, EntryPoint = "glProgramUniform3ui")]
        public extern static void ProgramUniform3ui(uint program, int location, int v0, int v1, uint v2);
        [DllImport(lib, EntryPoint = "glProgramUniform4ui")]
        public extern static void ProgramUniform4ui(uint program, int location, int v0, int v1, int v2, uint v3);
        [DllImport(lib, EntryPoint = "glProgramUniform1fv")]
        public extern static void ProgramUniform1fv(uint program, int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniform2fv")]
        public extern static void ProgramUniform2fv(uint program, int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniform3fv")]
        public extern static void ProgramUniform3fv(uint program, int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniform4fv")]
        public extern static void ProgramUniform4fv(uint program, int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniform1iv")]
        public extern static void ProgramUniform1iv(uint program, int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glProgramUniform2iv")]
        public extern static void ProgramUniform2iv(uint program, int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glProgramUniform3iv")]
        public extern static void ProgramUniform3iv(uint program, int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glProgramUniform4iv")]
        public extern static void ProgramUniform4iv(uint program, int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glProgramUniform1uiv")]
        public extern static void ProgramUniform1uiv(uint program, int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glProgramUniform2uiv")]
        public extern static void ProgramUniform2uiv(uint program, int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glProgramUniform3uiv")]
        public extern static void ProgramUniform3uiv(uint program, int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glProgramUniform4uiv")]
        public extern static void ProgramUniform4uiv(uint program, int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix2fv")]
        public extern static void ProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix3fv")]
        public extern static void ProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix4fv")]
        public extern static void ProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix2x3fv")]
        public extern static void ProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix3x2fv")]
        public extern static void ProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix2x4fv")]
        public extern static void ProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix4x2fv")]
        public extern static void ProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix3x4fv")]
        public extern static void ProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProgramUniformMatrix4x3fv")]
        public extern static void ProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glProvokingVertex")]
        public extern static void ProvokingVertex(ProvokingVertexMode provokeMode);
        [DllImport(lib, EntryPoint = "glQueryCounter")]
        public extern static void QueryCounter(uint id, int target);
        [DllImport(lib, EntryPoint = "glReadBuffer")]
        public extern static void ReadBuffer(ReadBufferMode mode);
        [DllImport(lib, EntryPoint = "glNamedFramebufferReadBuffer")]
        public extern static void NamedFramebufferReadBuffer(uint framebuffer, BeginMode mode);
        [DllImport(lib, EntryPoint = "glReadPixels")]
        public extern static void ReadPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int[] data);
        [DllImport(lib, EntryPoint = "glReadnPixels")]
        public extern static void ReadnPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int bufSize, int[] data);
        [DllImport(lib, EntryPoint = "glRenderbufferStorage")]
        public extern static void RenderbufferStorage(RenderbufferTarget target, RenderbufferStorageEnum internalFormat, int width, int height);
        [DllImport(lib, EntryPoint = "glNamedRenderbufferStorage")]
        public extern static void NamedRenderbufferStorage(uint renderbuffer, RenderbufferStorageEnum internalFormat, int width, int height);
        [DllImport(lib, EntryPoint = "glRenderbufferStorageMultisample")]
        public extern static void RenderbufferStorageMultisample(RenderbufferTarget target, int samples, RenderbufferStorageEnum internalFormat, int width, int height);
        [DllImport(lib, EntryPoint = "glNamedRenderbufferStorageMultisample")]
        public extern static void NamedRenderbufferStorageMultisample(uint renderbuffer, int samples, RenderbufferStorageEnum internalFormat, int width, int height);
        [DllImport(lib, EntryPoint = "glSampleCoverage")]
        public extern static void SampleCoverage(float value, bool invert);
        [DllImport(lib, EntryPoint = "glSampleMaski")]
        public extern static void SampleMaski(uint maskNumber, uint mask);
        [DllImport(lib, EntryPoint = "glSamplerParameterf")]
        public extern static void SamplerParameterf(uint sampler, int pname, float param);
        [DllImport(lib, EntryPoint = "glSamplerParameteri")]
        public extern static void SamplerParameteri(uint sampler, int pname, int param);
        [DllImport(lib, EntryPoint = "glSamplerParameterfv")]
        public extern static void SamplerParameterfv(uint sampler, int pname, float[] @params);
        [DllImport(lib, EntryPoint = "glSamplerParameteriv")]
        public extern static void SamplerParameteriv(uint sampler, int pname, int[] @params);
        [DllImport(lib, EntryPoint = "glSamplerParameterIiv")]
        public extern static void SamplerParameterIiv(uint sampler, TextureParameterName pname, int[] @params);
        [DllImport(lib, EntryPoint = "glSamplerParameterIuiv")]
        public extern static void SamplerParameterIuiv(uint sampler, TextureParameterName pname, uint[] @params);
        [DllImport(lib, EntryPoint = "glScissor")]
        public extern static void Scissor(int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glScissorArrayv")]
        public extern static void ScissorArrayv(uint first, int count, int[] v);
        [DllImport(lib, EntryPoint = "glScissorIndexed")]
        public extern static void ScissorIndexed(uint index, int left, int bottom, int width, int height);
        [DllImport(lib, EntryPoint = "glScissorIndexedv")]
        public extern static void ScissorIndexedv(uint index, int[] v);
        [DllImport(lib, EntryPoint = "glShaderBinary")]
        public extern static void ShaderBinary(int count, uint[] shaders, int binaryFormat, IntPtr binary, int length);
        [DllImport(lib, EntryPoint = "glShaderSource")]
        public extern static void ShaderSource(uint shader, int count, string[] @string, int[] length);
        [DllImport(lib, EntryPoint = "glShaderStorageBlockBinding")]
        public extern static void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding);
        [DllImport(lib, EntryPoint = "glStencilFunc")]
        public extern static void StencilFunc(StencilFunction func, int @ref, uint mask);
        [DllImport(lib, EntryPoint = "glStencilFuncSeparate")]
        public extern static void StencilFuncSeparate(StencilFace face, StencilFunction func, int @ref, uint mask);
        [DllImport(lib, EntryPoint = "glStencilMask")]
        public extern static void StencilMask(uint mask);
        [DllImport(lib, EntryPoint = "glStencilMaskSeparate")]
        public extern static void StencilMaskSeparate(StencilFace face, uint mask);
        [DllImport(lib, EntryPoint = "glStencilOp")]
        public extern static void StencilOp(StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass);
        [DllImport(lib, EntryPoint = "glStencilOpSeparate")]
        public extern static void StencilOpSeparate(StencilFace face, StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass);
        [DllImport(lib, EntryPoint = "glTexBuffer")]
        public extern static void TexBuffer(TextureBufferTarget target, SizedInternalFormat internalFormat, uint buffer);
        [DllImport(lib, EntryPoint = "glTextureBuffer")]
        public extern static void TextureBuffer(uint texture, SizedInternalFormat internalFormat, uint buffer);
        [DllImport(lib, EntryPoint = "glTexBufferRange")]
        public extern static void TexBufferRange(BufferTarget target, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, IntPtr size);
        [DllImport(lib, EntryPoint = "glTextureBufferRange")]
        public extern static void TextureBufferRange(uint texture, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, int size);
        [DllImport(lib, EntryPoint = "glTexImage1D")]
        public extern static void TexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, PixelFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glTexImage2D")]
        public extern static void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glTexImage2DMultisample")]
        public extern static void TexImage2DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, bool fixedsamplelocations);
        [DllImport(lib, EntryPoint = "glTexImage3D")]
        public extern static void TexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, PixelFormat format, PixelType type, IntPtr data);
        [DllImport(lib, EntryPoint = "glTexImage3DMultisample")]
        public extern static void TexImage3DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations);
        [DllImport(lib, EntryPoint = "glTexParameterf")]
        public extern static void TexParameterf(TextureTarget target, TextureParameterName pname, float param);
        [DllImport(lib, EntryPoint = "glTexParameteri")]
        public extern static void TexParameteri(TextureTarget target, TextureParameterName pname, int param);
        [DllImport(lib, EntryPoint = "glTextureParameterf")]
        public extern static void TextureParameterf(uint texture, TextureParameter pname, float param);
        [DllImport(lib, EntryPoint = "glTextureParameteri")]
        public extern static void TextureParameteri(uint texture, TextureParameter pname, int param);
        [DllImport(lib, EntryPoint = "glTexParameterfv")]
        public extern static void TexParameterfv(TextureTarget target, TextureParameterName pname, float[] @params);
        [DllImport(lib, EntryPoint = "glTexParameteriv")]
        public extern static void TexParameteriv(TextureTarget target, TextureParameterName pname, int[] @params);
        [DllImport(lib, EntryPoint = "glTexParameterIiv")]
        public extern static void TexParameterIiv(TextureTarget target, TextureParameterName pname, int[] @params);
        [DllImport(lib, EntryPoint = "glTexParameterIuiv")]
        public extern static void TexParameterIuiv(TextureTarget target, TextureParameterName pname, uint[] @params);
        [DllImport(lib, EntryPoint = "glTextureParameterfv")]
        public extern static void TextureParameterfv(uint texture, TextureParameter pname, float[] paramtexture);
        [DllImport(lib, EntryPoint = "glTextureParameteriv")]
        public extern static void TextureParameteriv(uint texture, TextureParameter pname, int[] param);
        [DllImport(lib, EntryPoint = "glTextureParameterIiv")]
        public extern static void TextureParameterIiv(uint texture, TextureParameter pname, int[] @params);
        [DllImport(lib, EntryPoint = "glTextureParameterIuiv")]
        public extern static void TextureParameterIuiv(uint texture, TextureParameter pname, uint[] @params);
        [DllImport(lib, EntryPoint = "glTexStorage1D")]
        public extern static void TexStorage1D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width);
        [DllImport(lib, EntryPoint = "glTextureStorage1D")]
        public extern static void TextureStorage1D(uint texture, int levels, SizedInternalFormat internalFormat, int width);
        [DllImport(lib, EntryPoint = "glTexStorage2D")]
        public extern static void TexStorage2D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height);
        [DllImport(lib, EntryPoint = "glTextureStorage2D")]
        public extern static void TextureStorage2D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height);
        [DllImport(lib, EntryPoint = "glTexStorage2DMultisample")]
        public extern static void TexStorage2DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations);
        [DllImport(lib, EntryPoint = "glTextureStorage2DMultisample")]
        public extern static void TextureStorage2DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations);
        [DllImport(lib, EntryPoint = "glTexStorage3D")]
        public extern static void TexStorage3D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height, int depth);
        [DllImport(lib, EntryPoint = "glTextureStorage3D")]
        public extern static void TextureStorage3D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height, int depth);
        [DllImport(lib, EntryPoint = "glTexStorage3DMultisample")]
        public extern static void TexStorage3DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations);
        [DllImport(lib, EntryPoint = "glTextureStorage3DMultisample")]
        public extern static void TextureStorage3DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations);
        [DllImport(lib, EntryPoint = "glTexSubImage1D")]
        public extern static void TexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels);
        [DllImport(lib, EntryPoint = "glTextureSubImage1D")]
        public extern static void TextureSubImage1D(uint texture, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels);
        [DllImport(lib, EntryPoint = "glTexSubImage2D")]
        public extern static void TexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels);
        [DllImport(lib, EntryPoint = "glTextureSubImage2D")]
        public extern static void TextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels);
        [DllImport(lib, EntryPoint = "glTexSubImage3D")]
        public extern static void TexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels);
        [DllImport(lib, EntryPoint = "glTextureSubImage3D")]
        public extern static void TextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels);
        [DllImport(lib, EntryPoint = "glTextureBarrier")]
        public extern static void TextureBarrier();
        [DllImport(lib, EntryPoint = "glTextureView")]
        public extern static void TextureView(uint texture, TextureTarget target, uint origtexture, PixelInternalFormat internalFormat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);
        [DllImport(lib, EntryPoint = "glTransformFeedbackBufferBase")]
        public extern static void TransformFeedbackBufferBase(uint xfb, uint index, uint buffer);
        [DllImport(lib, EntryPoint = "glTransformFeedbackBufferRange")]
        public extern static void TransformFeedbackBufferRange(uint xfb, uint index, uint buffer, IntPtr offset, int size);
        [DllImport(lib, EntryPoint = "glTransformFeedbackVaryings")]
        public extern static void TransformFeedbackVaryings(uint program, int count, string[] varyings, TransformFeedbackMode bufferMode);
        [DllImport(lib, EntryPoint = "glUniform1f")]
        public extern static void Uniform1f(int location, float v0);
        [DllImport(lib, EntryPoint = "glUniform2f")]
        public extern static void Uniform2f(int location, float v0, float v1);
        [DllImport(lib, EntryPoint = "glUniform3f")]
        public extern static void Uniform3f(int location, float v0, float v1, float v2);
        [DllImport(lib, EntryPoint = "glUniform4f")]
        public extern static void Uniform4f(int location, float v0, float v1, float v2, float v3);
        [DllImport(lib, EntryPoint = "glUniform1i")]
        public extern static void Uniform1i(int location, int v0);
        [DllImport(lib, EntryPoint = "glUniform2i")]
        public extern static void Uniform2i(int location, int v0, int v1);
        [DllImport(lib, EntryPoint = "glUniform3i")]
        public extern static void Uniform3i(int location, int v0, int v1, int v2);
        [DllImport(lib, EntryPoint = "glUniform4i")]
        public extern static void Uniform4i(int location, int v0, int v1, int v2, int v3);
        [DllImport(lib, EntryPoint = "glUniform1ui")]
        public extern static void Uniform1ui(int location, uint v0);
        [DllImport(lib, EntryPoint = "glUniform2ui")]
        public extern static void Uniform2ui(int location, uint v0, uint v1);
        [DllImport(lib, EntryPoint = "glUniform3ui")]
        public extern static void Uniform3ui(int location, uint v0, uint v1, uint v2);
        [DllImport(lib, EntryPoint = "glUniform4ui")]
        public extern static void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3);
        [DllImport(lib, EntryPoint = "glUniform1fv")]
        public extern static void Uniform1fv(int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glUniform2fv")]
        public extern static void Uniform2fv(int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glUniform3fv")]
        public extern static void Uniform3fv(int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glUniform4fv")]
        public extern static void Uniform4fv(int location, int count, float* value);
        [DllImport(lib, EntryPoint = "glUniform1iv")]
        public extern static void Uniform1iv(int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glUniform2iv")]
        public extern static void Uniform2iv(int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glUniform3iv")]
        public extern static void Uniform3iv(int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glUniform4iv")]
        public extern static void Uniform4iv(int location, int count, int* value);
        [DllImport(lib, EntryPoint = "glUniform1uiv")]
        public extern static void Uniform1uiv(int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glUniform2uiv")]
        public extern static void Uniform2uiv(int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glUniform3uiv")]
        public extern static void Uniform3uiv(int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glUniform4uiv")]
        public extern static void Uniform4uiv(int location, int count, uint* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix2fv")]
        public extern static void UniformMatrix2fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix3fv")]
        public extern static void UniformMatrix3fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix4fv")]
        public extern static void UniformMatrix4fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix2x3fv")]
        public extern static void UniformMatrix2x3fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix3x2fv")]
        public extern static void UniformMatrix3x2fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix2x4fv")]
        public extern static void UniformMatrix2x4fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix4x2fv")]
        public extern static void UniformMatrix4x2fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix3x4fv")]
        public extern static void UniformMatrix3x4fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformMatrix4x3fv")]
        public extern static void UniformMatrix4x3fv(int location, int count, bool transpose, float* value);
        [DllImport(lib, EntryPoint = "glUniformBlockBinding")]
        public extern static void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
        [DllImport(lib, EntryPoint = "glUniformSubroutinesuiv")]
        public extern static void UniformSubroutinesuiv(ShaderType shadertype, int count, uint[] indices);
        [DllImport(lib, EntryPoint = "glUnmapBuffer")]
        public extern static bool UnmapBuffer(BufferTarget target);
        [DllImport(lib, EntryPoint = "glUnmapNamedBuffer")]
        public extern static bool UnmapNamedBuffer(uint buffer);
        [DllImport(lib, EntryPoint = "glUseProgram")]
        public extern static void UseProgram(uint program);
        [DllImport(lib, EntryPoint = "glUseProgramStages")]
        public extern static void UseProgramStages(uint pipeline, uint stages, uint program);
        [DllImport(lib, EntryPoint = "glValidateProgram")]
        public extern static void ValidateProgram(uint program);
        [DllImport(lib, EntryPoint = "glValidateProgramPipeline")]
        public extern static void ValidateProgramPipeline(uint pipeline);
        [DllImport(lib, EntryPoint = "glVertexArrayElementBuffer")]
        public extern static void VertexArrayElementBuffer(uint vaobj, uint buffer);
        [DllImport(lib, EntryPoint = "glVertexAttrib1f")]
        public extern static void VertexAttrib1f(uint index, float v0);
        [DllImport(lib, EntryPoint = "glVertexAttrib1s")]
        public extern static void VertexAttrib1s(uint index, short v0);
        [DllImport(lib, EntryPoint = "glVertexAttrib1d")]
        public extern static void VertexAttrib1d(uint index, double v0);
        [DllImport(lib, EntryPoint = "glVertexAttribI1i")]
        public extern static void VertexAttribI1i(uint index, int v0);
        [DllImport(lib, EntryPoint = "glVertexAttribI1ui")]
        public extern static void VertexAttribI1ui(uint index, uint v0);
        [DllImport(lib, EntryPoint = "glVertexAttrib2f")]
        public extern static void VertexAttrib2f(uint index, float v0, float v1);
        [DllImport(lib, EntryPoint = "glVertexAttrib2s")]
        public extern static void VertexAttrib2s(uint index, short v0, short v1);
        [DllImport(lib, EntryPoint = "glVertexAttrib2d")]
        public extern static void VertexAttrib2d(uint index, double v0, double v1);
        [DllImport(lib, EntryPoint = "glVertexAttribI2i")]
        public extern static void VertexAttribI2i(uint index, int v0, int v1);
        [DllImport(lib, EntryPoint = "glVertexAttribI2ui")]
        public extern static void VertexAttribI2ui(uint index, uint v0, uint v1);
        [DllImport(lib, EntryPoint = "glVertexAttrib3f")]
        public extern static void VertexAttrib3f(uint index, float v0, float v1, float v2);
        [DllImport(lib, EntryPoint = "glVertexAttrib3s")]
        public extern static void VertexAttrib3s(uint index, short v0, short v1, short v2);
        [DllImport(lib, EntryPoint = "glVertexAttrib3d")]
        public extern static void VertexAttrib3d(uint index, double v0, double v1, double v2);
        [DllImport(lib, EntryPoint = "glVertexAttribI3i")]
        public extern static void VertexAttribI3i(uint index, int v0, int v1, int v2);
        [DllImport(lib, EntryPoint = "glVertexAttribI3ui")]
        public extern static void VertexAttribI3ui(uint index, uint v0, uint v1, uint v2);
        [DllImport(lib, EntryPoint = "glVertexAttrib4f")]
        public extern static void VertexAttrib4f(uint index, float v0, float v1, float v2, float v3);
        [DllImport(lib, EntryPoint = "glVertexAttrib4s")]
        public extern static void VertexAttrib4s(uint index, short v0, short v1, short v2, short v3);
        [DllImport(lib, EntryPoint = "glVertexAttrib4d")]
        public extern static void VertexAttrib4d(uint index, double v0, double v1, double v2, double v3);
        [DllImport(lib, EntryPoint = "glVertexAttrib4Nub")]
        public extern static void VertexAttrib4Nub(uint index, byte v0, byte v1, byte v2, byte v3);
        [DllImport(lib, EntryPoint = "glVertexAttribI4i")]
        public extern static void VertexAttribI4i(uint index, int v0, int v1, int v2, int v3);
        [DllImport(lib, EntryPoint = "glVertexAttribI4ui")]
        public extern static void VertexAttribI4ui(uint index, uint v0, uint v1, uint v2, uint v3);
        [DllImport(lib, EntryPoint = "glVertexAttribL1d")]
        public extern static void VertexAttribL1d(uint index, double v0);
        [DllImport(lib, EntryPoint = "glVertexAttribL2d")]
        public extern static void VertexAttribL2d(uint index, double v0, double v1);
        [DllImport(lib, EntryPoint = "glVertexAttribL3d")]
        public extern static void VertexAttribL3d(uint index, double v0, double v1, double v2);
        [DllImport(lib, EntryPoint = "glVertexAttribL4d")]
        public extern static void VertexAttribL4d(uint index, double v0, double v1, double v2, double v3);
        [DllImport(lib, EntryPoint = "glVertexAttrib1fv")]
        public extern static void VertexAttrib1fv(uint index, float[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib1sv")]
        public extern static void VertexAttrib1sv(uint index, short[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib1dv")]
        public extern static void VertexAttrib1dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI1iv")]
        public extern static void VertexAttribI1iv(uint index, int[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI1uiv")]
        public extern static void VertexAttribI1uiv(uint index, uint[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib2fv")]
        public extern static void VertexAttrib2fv(uint index, float[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib2sv")]
        public extern static void VertexAttrib2sv(uint index, short[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib2dv")]
        public extern static void VertexAttrib2dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI2iv")]
        public extern static void VertexAttribI2iv(uint index, int[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI2uiv")]
        public extern static void VertexAttribI2uiv(uint index, uint[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib3fv")]
        public extern static void VertexAttrib3fv(uint index, float[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib3sv")]
        public extern static void VertexAttrib3sv(uint index, short[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib3dv")]
        public extern static void VertexAttrib3dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI3iv")]
        public extern static void VertexAttribI3iv(uint index, int[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI3uiv")]
        public extern static void VertexAttribI3uiv(uint index, uint[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4fv")]
        public extern static void VertexAttrib4fv(uint index, float[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4sv")]
        public extern static void VertexAttrib4sv(uint index, short[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4dv")]
        public extern static void VertexAttrib4dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4iv")]
        public extern static void VertexAttrib4iv(uint index, int[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4bv")]
        public extern static void VertexAttrib4bv(uint index, sbyte[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4ubv")]
        public extern static void VertexAttrib4ubv(uint index, byte[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4usv")]
        public extern static void VertexAttrib4usv(uint index, ushort[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4uiv")]
        public extern static void VertexAttrib4uiv(uint index, uint[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4Nbv")]
        public extern static void VertexAttrib4Nbv(uint index, sbyte[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4Nsv")]
        public extern static void VertexAttrib4Nsv(uint index, short[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4Niv")]
        public extern static void VertexAttrib4Niv(uint index, int[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4Nubv")]
        public extern static void VertexAttrib4Nubv(uint index, byte[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4Nusv")]
        public extern static void VertexAttrib4Nusv(uint index, ushort[] v);
        [DllImport(lib, EntryPoint = "glVertexAttrib4Nuiv")]
        public extern static void VertexAttrib4Nuiv(uint index, uint[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI4bv")]
        public extern static void VertexAttribI4bv(uint index, sbyte[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI4ubv")]
        public extern static void VertexAttribI4ubv(uint index, byte[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI4sv")]
        public extern static void VertexAttribI4sv(uint index, short[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI4usv")]
        public extern static void VertexAttribI4usv(uint index, ushort[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI4iv")]
        public extern static void VertexAttribI4iv(uint index, int[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribI4uiv")]
        public extern static void VertexAttribI4uiv(uint index, uint[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribL1dv")]
        public extern static void VertexAttribL1dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribL2dv")]
        public extern static void VertexAttribL2dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribL3dv")]
        public extern static void VertexAttribL3dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribL4dv")]
        public extern static void VertexAttribL4dv(uint index, double[] v);
        [DllImport(lib, EntryPoint = "glVertexAttribP1ui")]
        public extern static void VertexAttribP1ui(uint index, VertexAttribPType type, bool normalized, uint value);
        [DllImport(lib, EntryPoint = "glVertexAttribP2ui")]
        public extern static void VertexAttribP2ui(uint index, VertexAttribPType type, bool normalized, uint value);
        [DllImport(lib, EntryPoint = "glVertexAttribP3ui")]
        public extern static void VertexAttribP3ui(uint index, VertexAttribPType type, bool normalized, uint value);
        [DllImport(lib, EntryPoint = "glVertexAttribP4ui")]
        public extern static void VertexAttribP4ui(uint index, VertexAttribPType type, bool normalized, uint value);
        [DllImport(lib, EntryPoint = "glVertexAttribBinding")]
        public extern static void VertexAttribBinding(uint attribindex, uint bindingindex);
        [DllImport(lib, EntryPoint = "glVertexArrayAttribBinding")]
        public extern static void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex);
        [DllImport(lib, EntryPoint = "glVertexAttribDivisor")]
        public extern static void VertexAttribDivisor(uint index, uint divisor);
        [DllImport(lib, EntryPoint = "glVertexAttribFormat")]
        public extern static void VertexAttribFormat(uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset);
        [DllImport(lib, EntryPoint = "glVertexAttribIFormat")]
        public extern static void VertexAttribIFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
        [DllImport(lib, EntryPoint = "glVertexAttribLFormat")]
        public extern static void VertexAttribLFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
        [DllImport(lib, EntryPoint = "glVertexArrayAttribFormat")]
        public extern static void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset);
        [DllImport(lib, EntryPoint = "glVertexArrayAttribIFormat")]
        public extern static void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
        [DllImport(lib, EntryPoint = "glVertexArrayAttribLFormat")]
        public extern static void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
        [DllImport(lib, EntryPoint = "glVertexAttribPointer")]
        public extern static void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, IntPtr pointer);
        [DllImport(lib, EntryPoint = "glVertexAttribIPointer")]
        public extern static void VertexAttribIPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer);
        [DllImport(lib, EntryPoint = "glVertexAttribLPointer")]
        public extern static void VertexAttribLPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer);
        [DllImport(lib, EntryPoint = "glVertexBindingDivisor")]
        public extern static void VertexBindingDivisor(uint bindingindex, uint divisor);
        [DllImport(lib, EntryPoint = "glVertexArrayBindingDivisor")]
        public extern static void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor);
        [DllImport(lib, EntryPoint = "glViewport")]
        public extern static void Viewport(int x, int y, int width, int height);
        [DllImport(lib, EntryPoint = "glViewportArrayv")]
        public extern static void ViewportArrayv(uint first, int count, float[] v);
        [DllImport(lib, EntryPoint = "glViewportIndexedf")]
        public extern static void ViewportIndexedf(uint index, float x, float y, float w, float h);
        [DllImport(lib, EntryPoint = "glViewportIndexedfv")]
        public extern static void ViewportIndexedfv(uint index, float[] v);
        [DllImport(lib, EntryPoint = "glWaitSync")]
        public extern static void WaitSync(IntPtr sync, uint flags, ulong timeout);
    }
}