using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Engine
{
    static partial class OpenGL
    {
        internal unsafe static class NativeCalls
        {
            const string lib = "opengl32";
            internal static Dictionary<string, MethodInfo> Methods;

            static NativeCalls()
            {
                var m = typeof(NativeCalls).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
                Methods = new Dictionary<string, MethodInfo>(m.Length);
                for (int i = 0; i < m.Length; i++)
                {
                    Methods.Add(m[i].Name, m[i]);
                }
            }

            [DllImport(lib, EntryPoint = "glActiveShaderProgram")]
            internal extern static void ActiveShaderProgram(uint pipeline, uint program);
            [DllImport(lib, EntryPoint = "glActiveTexture")]
            internal extern static void ActiveTexture(TextureUnit texture);
            [DllImport(lib, EntryPoint = "glAttachShader")]
            internal extern static void AttachShader(uint program, uint shader);
            [DllImport(lib, EntryPoint = "glBeginConditionalRender")]
            internal extern static void BeginConditionalRender(uint id, ConditionalRenderType mode);
            [DllImport(lib, EntryPoint = "glEndConditionalRender")]
            internal extern static void EndConditionalRender();
            [DllImport(lib, EntryPoint = "glBeginQuery")]
            internal extern static void BeginQuery(QueryTarget target, uint id);
            [DllImport(lib, EntryPoint = "glEndQuery")]
            internal extern static void EndQuery(QueryTarget target);
            [DllImport(lib, EntryPoint = "glBeginQueryIndexed")]
            internal extern static void BeginQueryIndexed(uint target, uint index, uint id);
            [DllImport(lib, EntryPoint = "glEndQueryIndexed")]
            internal extern static void EndQueryIndexed(QueryTarget target, uint index);
            [DllImport(lib, EntryPoint = "glBeginTransformFeedback")]
            internal extern static void BeginTransformFeedback(BeginFeedbackMode primitiveMode);
            [DllImport(lib, EntryPoint = "glEndTransformFeedback")]
            internal extern static void EndTransformFeedback();
            [DllImport(lib, EntryPoint = "glBindAttribLocation")]
            internal extern static void BindAttribLocation(uint program, uint index, string name);
            [DllImport(lib, EntryPoint = "glBindBuffer")]
            internal extern static void BindBuffer(BufferTarget target, uint buffer);
            [DllImport(lib, EntryPoint = "glBindBufferBase")]
            internal extern static void BindBufferBase(BufferTarget target, uint index, uint buffer);
            [DllImport(lib, EntryPoint = "glBindBufferRange")]
            internal extern static void BindBufferRange(BufferTarget target, uint index, uint buffer, IntPtr offset, IntPtr size);
            [DllImport(lib, EntryPoint = "glBindBuffersBase")]
            internal extern static void BindBuffersBase(BufferTarget target, uint first, int count, uint[] buffers);
            [DllImport(lib, EntryPoint = "glBindBuffersRange")]
            internal extern static void BindBuffersRange(BufferTarget target, uint first, int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes);
            [DllImport(lib, EntryPoint = "glBindFragDataLocation")]
            internal extern static void BindFragDataLocation(uint program, uint colorNumber, string name);
            [DllImport(lib, EntryPoint = "glBindFragDataLocationIndexed")]
            internal extern static void BindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name);
            [DllImport(lib, EntryPoint = "glBindFramebuffer")]
            internal extern static void BindFramebuffer(FramebufferTarget target, uint framebuffer);
            [DllImport(lib, EntryPoint = "glBindImageTexture")]
            internal extern static void BindImageTexture(uint unit, uint texture, int level, bool layered, int layer, BufferAccess access, PixelInternalFormat format);
            [DllImport(lib, EntryPoint = "glBindImageTextures")]
            internal extern static void BindImageTextures(uint first, int count, uint[] textures);
            [DllImport(lib, EntryPoint = "glBindProgramPipeline")]
            internal extern static void BindProgramPipeline(uint pipeline);
            [DllImport(lib, EntryPoint = "glBindRenderbuffer")]
            internal extern static void BindRenderbuffer(RenderbufferTarget target, uint renderbuffer);
            [DllImport(lib, EntryPoint = "glBindSampler")]
            internal extern static void BindSampler(uint unit, uint sampler);
            [DllImport(lib, EntryPoint = "glBindSamplers")]
            internal extern static void BindSamplers(uint first, int count, uint[] samplers);
            [DllImport(lib, EntryPoint = "glBindTexture")]
            internal extern static void BindTexture(TextureTarget target, uint texture);
            [DllImport(lib, EntryPoint = "glBindTextures")]
            internal extern static void BindTextures(uint first, int count, uint[] textures);
            [DllImport(lib, EntryPoint = "glBindTextureUnit")]
            internal extern static void BindTextureUnit(uint unit, uint texture);
            [DllImport(lib, EntryPoint = "glBindTransformFeedback")]
            internal extern static void BindTransformFeedback(NvTransformFeedback2 target, uint id);
            [DllImport(lib, EntryPoint = "glBindVertexArray")]
            internal extern static void BindVertexArray(uint array);
            [DllImport(lib, EntryPoint = "glBindVertexBuffer")]
            internal extern static void BindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, IntPtr stride);
            [DllImport(lib, EntryPoint = "glVertexArrayVertexBuffer")]
            internal extern static void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride);
            [DllImport(lib, EntryPoint = "glBindVertexBuffers")]
            internal extern static void BindVertexBuffers(uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides);
            [DllImport(lib, EntryPoint = "glVertexArrayVertexBuffers")]
            internal extern static void VertexArrayVertexBuffers(uint vaobj, uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides);
            [DllImport(lib, EntryPoint = "glBlendColor")]
            internal extern static void BlendColor(float red, float green, float blue, float alpha);
            [DllImport(lib, EntryPoint = "glBlendEquation")]
            internal extern static void BlendEquation(BlendEquationMode mode);
            [DllImport(lib, EntryPoint = "glBlendEquationi")]
            internal extern static void BlendEquationi(uint buf, BlendEquationMode mode);
            [DllImport(lib, EntryPoint = "glBlendEquationSeparate")]
            internal extern static void BlendEquationSeparate(BlendEquationMode modeRGB, BlendEquationMode modeAlpha);
            [DllImport(lib, EntryPoint = "glBlendEquationSeparatei")]
            internal extern static void BlendEquationSeparatei(uint buf, BlendEquationMode modeRGB, BlendEquationMode modeAlpha);
            [DllImport(lib, EntryPoint = "glBlendFunc")]
            internal extern static void BlendFunc(BlendingFactorSrc sfactor, BlendingFactorDest dfactor);
            [DllImport(lib, EntryPoint = "glBlendFunci")]
            internal extern static void BlendFunci(uint buf, BlendingFactorSrc sfactor, BlendingFactorDest dfactor);
            [DllImport(lib, EntryPoint = "glBlendFuncSeparate")]
            internal extern static void BlendFuncSeparate(BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha);
            [DllImport(lib, EntryPoint = "glBlendFuncSeparatei")]
            internal extern static void BlendFuncSeparatei(uint buf, BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha);
            [DllImport(lib, EntryPoint = "glBlitFramebuffer")]
            internal extern static void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter);
            [DllImport(lib, EntryPoint = "glBlitNamedFramebuffer")]
            internal extern static void BlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter);
            [DllImport(lib, EntryPoint = "glBufferData")]
            internal extern static void BufferData(BufferTarget target, int size, IntPtr data, BufferUsageHint usage);
            [DllImport(lib, EntryPoint = "glNamedBufferData")]
            internal extern static void NamedBufferData(uint buffer, int size, IntPtr data, BufferUsageHint usage);
            [DllImport(lib, EntryPoint = "glBufferStorage")]
            internal extern static void BufferStorage(BufferTarget target, IntPtr size, IntPtr data, uint flags);
            [DllImport(lib, EntryPoint = "glNamedBufferStorage")]
            internal extern static void NamedBufferStorage(uint buffer, int size, IntPtr data, uint flags);
            [DllImport(lib, EntryPoint = "glBufferSubData")]
            internal extern static void BufferSubData(BufferTarget target, IntPtr offset, IntPtr size, IntPtr data);
            [DllImport(lib, EntryPoint = "glNamedBufferSubData")]
            internal extern static void NamedBufferSubData(uint buffer, IntPtr offset, int size, IntPtr data);
            [DllImport(lib, EntryPoint = "glCheckFramebufferStatus")]
            internal extern static FramebufferErrorCode CheckFramebufferStatus(FramebufferTarget target);
            [DllImport(lib, EntryPoint = "glCheckNamedFramebufferStatus")]
            internal extern static FramebufferErrorCode CheckNamedFramebufferStatus(uint framebuffer, FramebufferTarget target);
            [DllImport(lib, EntryPoint = "glClampColor")]
            internal extern static void ClampColor(ClampColorTarget target, ClampColorMode clamp);
            [DllImport(lib, EntryPoint = "glClear")]
            internal extern static void Clear(ClearBufferMask mask);
            [DllImport(lib, EntryPoint = "glClearBufferiv")]
            internal extern static void ClearBufferiv(ClearBuffer buffer, int drawbuffer, int[] value);
            [DllImport(lib, EntryPoint = "glClearBufferuiv")]
            internal extern static void ClearBufferuiv(ClearBuffer buffer, int drawbuffer, uint[] value);
            [DllImport(lib, EntryPoint = "glClearBufferfv")]
            internal extern static void ClearBufferfv(ClearBuffer buffer, int drawbuffer, float[] value);
            [DllImport(lib, EntryPoint = "glClearBufferfi")]
            internal extern static void ClearBufferfi(ClearBuffer buffer, int drawbuffer, float depth, int stencil);
            [DllImport(lib, EntryPoint = "glClearNamedFramebufferiv")]
            internal extern static void ClearNamedFramebufferiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, int[] value);
            [DllImport(lib, EntryPoint = "glClearNamedFramebufferuiv")]
            internal extern static void ClearNamedFramebufferuiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, uint[] value);
            [DllImport(lib, EntryPoint = "glClearNamedFramebufferfv")]
            internal extern static void ClearNamedFramebufferfv(uint framebuffer, ClearBuffer buffer, int drawbuffer, float[] value);
            [DllImport(lib, EntryPoint = "glClearNamedFramebufferfi")]
            internal extern static void ClearNamedFramebufferfi(uint framebuffer, ClearBuffer buffer, float depth, int stencil);
            [DllImport(lib, EntryPoint = "glClearBufferData")]
            internal extern static void ClearBufferData(BufferTarget target, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glClearNamedBufferData")]
            internal extern static void ClearNamedBufferData(uint buffer, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glClearBufferSubData")]
            internal extern static void ClearBufferSubData(BufferTarget target, SizedInternalFormat internalFormat, IntPtr offset, IntPtr size, PixelInternalFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glClearNamedBufferSubData")]
            internal extern static void ClearNamedBufferSubData(uint buffer, SizedInternalFormat internalFormat, IntPtr offset, int size, PixelInternalFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glClearColor")]
            internal extern static void ClearColor(float red, float green, float blue, float alpha);
            [DllImport(lib, EntryPoint = "glClearDepth")]
            internal extern static void ClearDepth(double depth);
            [DllImport(lib, EntryPoint = "glClearDepthf")]
            internal extern static void ClearDepthf(float depth);
            [DllImport(lib, EntryPoint = "glClearStencil")]
            internal extern static void ClearStencil(int s);
            [DllImport(lib, EntryPoint = "glClearTexImage")]
            internal extern static void ClearTexImage(uint texture, int level, PixelInternalFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glClearTexSubImage")]
            internal extern static void ClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glClientWaitSync")]
            internal extern static ArbSync ClientWaitSync(IntPtr sync, uint flags, ulong timeout);
            [DllImport(lib, EntryPoint = "glClipControl")]
            internal extern static void ClipControl(ClipControlOrigin origin, ClipControlDepth depth);
            [DllImport(lib, EntryPoint = "glColorMask")]
            internal extern static void ColorMask(bool red, bool green, bool blue, bool alpha);
            [DllImport(lib, EntryPoint = "glColorMaski")]
            internal extern static void ColorMaski(uint buf, bool red, bool green, bool blue, bool alpha);
            [DllImport(lib, EntryPoint = "glCompileShader")]
            internal extern static void CompileShader(uint shader);
            [DllImport(lib, EntryPoint = "glCompressedTexImage1D")]
            internal extern static void CompressedTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTexImage2D")]
            internal extern static void CompressedTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTexImage3D")]
            internal extern static void CompressedTexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTexSubImage1D")]
            internal extern static void CompressedTexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTextureSubImage1D")]
            internal extern static void CompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, PixelInternalFormat format, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTexSubImage2D")]
            internal extern static void CompressedTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTextureSubImage2D")]
            internal extern static void CompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelInternalFormat format, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTexSubImage3D")]
            internal extern static void CompressedTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCompressedTextureSubImage3D")]
            internal extern static void CompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, int imageSize, IntPtr data);
            [DllImport(lib, EntryPoint = "glCopyBufferSubData")]
            internal extern static void CopyBufferSubData(BufferTarget readTarget, BufferTarget writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size);
            [DllImport(lib, EntryPoint = "glCopyNamedBufferSubData")]
            internal extern static void CopyNamedBufferSubData(uint readBuffer, uint writeBuffer, IntPtr readOffset, IntPtr writeOffset, int size);
            [DllImport(lib, EntryPoint = "glCopyImageSubData")]
            internal extern static void CopyImageSubData(uint srcName, BufferTarget srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, BufferTarget dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);
            [DllImport(lib, EntryPoint = "glCopyTexImage1D")]
            internal extern static void CopyTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int border);
            [DllImport(lib, EntryPoint = "glCopyTexImage2D")]
            internal extern static void CopyTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int height, int border);
            [DllImport(lib, EntryPoint = "glCopyTexSubImage1D")]
            internal extern static void CopyTexSubImage1D(TextureTarget target, int level, int xoffset, int x, int y, int width);
            [DllImport(lib, EntryPoint = "glCopyTextureSubImage1D")]
            internal extern static void CopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width);
            [DllImport(lib, EntryPoint = "glCopyTexSubImage2D")]
            internal extern static void CopyTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glCopyTextureSubImage2D")]
            internal extern static void CopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glCopyTexSubImage3D")]
            internal extern static void CopyTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glCopyTextureSubImage3D")]
            internal extern static void CopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glCreateBuffers")]
            internal extern static void CreateBuffers(int n, uint[] buffers);
            [DllImport(lib, EntryPoint = "glCreateFramebuffers")]
            internal extern static void CreateFramebuffers(int n, uint[] ids);
            [DllImport(lib, EntryPoint = "glCreateProgram")]
            internal extern static uint CreateProgram();
            [DllImport(lib, EntryPoint = "glCreateProgramPipelines")]
            internal extern static void CreateProgramPipelines(int n, uint[] pipelines);
            [DllImport(lib, EntryPoint = "glCreateQueries")]
            internal extern static void CreateQueries(QueryTarget target, int n, uint[] ids);
            [DllImport(lib, EntryPoint = "glCreateRenderbuffers")]
            internal extern static void CreateRenderbuffers(int n, uint[] renderbuffers);
            [DllImport(lib, EntryPoint = "glCreateSamplers")]
            internal extern static void CreateSamplers(int n, uint[] samplers);
            [DllImport(lib, EntryPoint = "glCreateShader")]
            internal extern static uint CreateShader(ShaderType shaderType);
            [DllImport(lib, EntryPoint = "glCreateShaderProgramv")]
            internal extern static uint CreateShaderProgramv(ShaderType type, int count, string strings);
            [DllImport(lib, EntryPoint = "glCreateTextures")]
            internal extern static void CreateTextures(TextureTarget target, int n, uint[] textures);
            [DllImport(lib, EntryPoint = "glCreateTransformFeedbacks")]
            internal extern static void CreateTransformFeedbacks(int n, uint[] ids);
            [DllImport(lib, EntryPoint = "glCreateVertexArrays")]
            internal extern static void CreateVertexArrays(int n, uint[] arrays);
            [DllImport(lib, EntryPoint = "glCullFace")]
            internal extern static void CullFace(CullFaceMode mode);
            [DllImport(lib, EntryPoint = "glDeleteBuffers")]
            internal extern static void DeleteBuffers(int n, uint[] buffers);
            [DllImport(lib, EntryPoint = "glDeleteFramebuffers")]
            internal extern static void DeleteFramebuffers(int n, uint[] framebuffers);
            [DllImport(lib, EntryPoint = "glDeleteProgram")]
            internal extern static void DeleteProgram(uint program);
            [DllImport(lib, EntryPoint = "glDeleteProgramPipelines")]
            internal extern static void DeleteProgramPipelines(int n, uint[] pipelines);
            [DllImport(lib, EntryPoint = "glDeleteQueries")]
            internal extern static void DeleteQueries(int n, uint[] ids);
            [DllImport(lib, EntryPoint = "glDeleteRenderbuffers")]
            internal extern static void DeleteRenderbuffers(int n, uint[] renderbuffers);
            [DllImport(lib, EntryPoint = "glDeleteSamplers")]
            internal extern static void DeleteSamplers(int n, uint[] samplers);
            [DllImport(lib, EntryPoint = "glDeleteShader")]
            internal extern static void DeleteShader(uint shader);
            [DllImport(lib, EntryPoint = "glDeleteSync")]
            internal extern static void DeleteSync(IntPtr sync);
            [DllImport(lib, EntryPoint = "glDeleteTextures")]
            internal extern static void DeleteTextures(int n, uint[] textures);
            [DllImport(lib, EntryPoint = "glDeleteTransformFeedbacks")]
            internal extern static void DeleteTransformFeedbacks(int n, uint[] ids);
            [DllImport(lib, EntryPoint = "glDeleteVertexArrays")]
            internal extern static void DeleteVertexArrays(int n, uint[] arrays);
            [DllImport(lib, EntryPoint = "glDepthFunc")]
            internal extern static void DepthFunc(DepthFunction func);
            [DllImport(lib, EntryPoint = "glDepthMask")]
            internal extern static void DepthMask(bool flag);
            [DllImport(lib, EntryPoint = "glDepthRange")]
            internal extern static void DepthRange(double nearVal, double farVal);
            [DllImport(lib, EntryPoint = "glDepthRangef")]
            internal extern static void DepthRangef(float nearVal, float farVal);
            [DllImport(lib, EntryPoint = "glDepthRangeArrayv")]
            internal extern static void DepthRangeArrayv(uint first, int count, double[] v);
            [DllImport(lib, EntryPoint = "glDepthRangeIndexed")]
            internal extern static void DepthRangeIndexed(uint index, double nearVal, double farVal);
            [DllImport(lib, EntryPoint = "glDetachShader")]
            internal extern static void DetachShader(uint program, uint shader);
            [DllImport(lib, EntryPoint = "glDispatchCompute")]
            internal extern static void DispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z);
            [DllImport(lib, EntryPoint = "glDispatchComputeIndirect")]
            internal extern static void DispatchComputeIndirect(IntPtr indirect);
            [DllImport(lib, EntryPoint = "glDrawArrays")]
            internal extern static void DrawArrays(BeginMode mode, int first, int count);
            [DllImport(lib, EntryPoint = "glDrawArraysIndirect")]
            internal extern static void DrawArraysIndirect(BeginMode mode, IntPtr indirect);
            [DllImport(lib, EntryPoint = "glDrawArraysInstanced")]
            internal extern static void DrawArraysInstanced(BeginMode mode, int first, int count, int primcount);
            [DllImport(lib, EntryPoint = "glDrawArraysInstancedBaseInstance")]
            internal extern static void DrawArraysInstancedBaseInstance(BeginMode mode, int first, int count, int primcount, uint baseinstance);
            [DllImport(lib, EntryPoint = "glDrawBuffer")]
            internal extern static void DrawBuffer(DrawBufferMode buf);
            [DllImport(lib, EntryPoint = "glNamedFramebufferDrawBuffer")]
            internal extern static void NamedFramebufferDrawBuffer(uint framebuffer, DrawBufferMode buf);
            [DllImport(lib, EntryPoint = "glDrawBuffers")]
            internal extern static void DrawBuffers(int n, DrawBuffersEnum[] bufs);
            [DllImport(lib, EntryPoint = "glNamedFramebufferDrawBuffers")]
            internal extern static void NamedFramebufferDrawBuffers(uint framebuffer, int n, DrawBufferMode[] bufs);
            [DllImport(lib, EntryPoint = "glDrawElements")]
            internal extern static void DrawElements(BeginMode mode, int count, DrawElementsType type, IntPtr indices);
            [DllImport(lib, EntryPoint = "glDrawElementsBaseVertex")]
            internal extern static void DrawElementsBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int basevertex);
            [DllImport(lib, EntryPoint = "glDrawElementsIndirect")]
            internal extern static void DrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect);
            [DllImport(lib, EntryPoint = "glDrawElementsInstanced")]
            internal extern static void DrawElementsInstanced(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount);
            [DllImport(lib, EntryPoint = "glDrawElementsInstancedBaseInstance")]
            internal extern static void DrawElementsInstancedBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, uint baseinstance);
            [DllImport(lib, EntryPoint = "glDrawElementsInstancedBaseVertex")]
            internal extern static void DrawElementsInstancedBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex);
            [DllImport(lib, EntryPoint = "glDrawElementsInstancedBaseVertexBaseInstance")]
            internal extern static void DrawElementsInstancedBaseVertexBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex, uint baseinstance);
            [DllImport(lib, EntryPoint = "glDrawRangeElements")]
            internal extern static void DrawRangeElements(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices);
            [DllImport(lib, EntryPoint = "glDrawRangeElementsBaseVertex")]
            internal extern static void DrawRangeElementsBaseVertex(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices, int basevertex);
            [DllImport(lib, EntryPoint = "glDrawTransformFeedback")]
            internal extern static void DrawTransformFeedback(NvTransformFeedback2 mode, uint id);
            [DllImport(lib, EntryPoint = "glDrawTransformFeedbackInstanced")]
            internal extern static void DrawTransformFeedbackInstanced(BeginMode mode, uint id, int primcount);
            [DllImport(lib, EntryPoint = "glDrawTransformFeedbackStream")]
            internal extern static void DrawTransformFeedbackStream(NvTransformFeedback2 mode, uint id, uint stream);
            [DllImport(lib, EntryPoint = "glDrawTransformFeedbackStreamInstanced")]
            internal extern static void DrawTransformFeedbackStreamInstanced(BeginMode mode, uint id, uint stream, int primcount);
            [DllImport(lib, EntryPoint = "glEnable")]
            internal extern static void Enable(EnableCap cap);
            [DllImport(lib, EntryPoint = "glDisable")]
            internal extern static void Disable(EnableCap cap);
            [DllImport(lib, EntryPoint = "glEnablei")]
            internal extern static void Enablei(EnableCap cap, uint index);
            [DllImport(lib, EntryPoint = "glDisablei")]
            internal extern static void Disablei(EnableCap cap, uint index);
            [DllImport(lib, EntryPoint = "glEnableVertexAttribArray")]
            internal extern static void EnableVertexAttribArray(uint index);
            [DllImport(lib, EntryPoint = "glDisableVertexAttribArray")]
            internal extern static void DisableVertexAttribArray(uint index);
            [DllImport(lib, EntryPoint = "glEnableVertexArrayAttrib")]
            internal extern static void EnableVertexArrayAttrib(uint vaobj, uint index);
            [DllImport(lib, EntryPoint = "glDisableVertexArrayAttrib")]
            internal extern static void DisableVertexArrayAttrib(uint vaobj, uint index);
            [DllImport(lib, EntryPoint = "glFenceSync")]
            internal extern static IntPtr FenceSync(ArbSync condition, uint flags);
            [DllImport(lib, EntryPoint = "glFinish")]
            internal extern static void Finish();
            [DllImport(lib, EntryPoint = "glFlush")]
            internal extern static void Flush();
            [DllImport(lib, EntryPoint = "glFlushMappedBufferRange")]
            internal extern static void FlushMappedBufferRange(BufferTarget target, IntPtr offset, IntPtr length);
            [DllImport(lib, EntryPoint = "glFlushMappedNamedBufferRange")]
            internal extern static void FlushMappedNamedBufferRange(uint buffer, IntPtr offset, int length);
            [DllImport(lib, EntryPoint = "glFramebufferParameteri")]
            internal extern static void FramebufferParameteri(FramebufferTarget target, FramebufferPName pname, int param);
            [DllImport(lib, EntryPoint = "glNamedFramebufferParameteri")]
            internal extern static void NamedFramebufferParameteri(uint framebuffer, FramebufferPName pname, int param);
            [DllImport(lib, EntryPoint = "glFramebufferRenderbuffer")]
            internal extern static void FramebufferRenderbuffer(FramebufferTarget target, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer);
            [DllImport(lib, EntryPoint = "glNamedFramebufferRenderbuffer")]
            internal extern static void NamedFramebufferRenderbuffer(uint framebuffer, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer);
            [DllImport(lib, EntryPoint = "glFramebufferTexture")]
            internal extern static void FramebufferTexture(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level);
            [DllImport(lib, EntryPoint = "glFramebufferTexture1D")]
            internal extern static void FramebufferTexture1D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level);
            [DllImport(lib, EntryPoint = "glFramebufferTexture2D")]
            internal extern static void FramebufferTexture2D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level);
            [DllImport(lib, EntryPoint = "glFramebufferTexture3D")]
            internal extern static void FramebufferTexture3D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level, int layer);
            [DllImport(lib, EntryPoint = "glNamedFramebufferTexture")]
            internal extern static void NamedFramebufferTexture(uint framebuffer, FramebufferAttachment attachment, uint texture, int level);
            [DllImport(lib, EntryPoint = "glFramebufferTextureLayer")]
            internal extern static void FramebufferTextureLayer(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level, int layer);
            [DllImport(lib, EntryPoint = "glNamedFramebufferTextureLayer")]
            internal extern static void NamedFramebufferTextureLayer(uint framebuffer, FramebufferAttachment attachment, uint texture, int level, int layer);
            [DllImport(lib, EntryPoint = "glFrontFace")]
            internal extern static void FrontFace(FrontFaceDirection mode);
            [DllImport(lib, EntryPoint = "glGenBuffers")]
            internal extern static void GenBuffers(int n, [Out] uint[] buffers);
            [DllImport(lib, EntryPoint = "glGenerateMipmap")]
            internal extern static void GenerateMipmap(TextureTarget target);
            [DllImport(lib, EntryPoint = "glGenerateTextureMipmap")]
            internal extern static void GenerateTextureMipmap(uint texture);
            [DllImport(lib, EntryPoint = "glGenFramebuffers")]
            internal extern static void GenFramebuffers(int n, [Out] uint[] ids);
            [DllImport(lib, EntryPoint = "glGenProgramPipelines")]
            internal extern static void GenProgramPipelines(int n, [Out] uint[] pipelines);
            [DllImport(lib, EntryPoint = "glGenQueries")]
            internal extern static void GenQueries(int n, [Out] uint[] ids);
            [DllImport(lib, EntryPoint = "glGenRenderbuffers")]
            internal extern static void GenRenderbuffers(int n, [Out] uint[] renderbuffers);
            [DllImport(lib, EntryPoint = "glGenSamplers")]
            internal extern static void GenSamplers(int n, [Out] uint[] samplers);
            [DllImport(lib, EntryPoint = "glGenTextures")]
            internal extern static void GenTextures(int n, [Out] uint[] textures);
            [DllImport(lib, EntryPoint = "glGenTransformFeedbacks")]
            internal extern static void GenTransformFeedbacks(int n, [Out] uint[] ids);
            [DllImport(lib, EntryPoint = "glGenVertexArrays")]
            internal extern static void GenVertexArrays(int n, [Out] uint[] arrays);
            [DllImport(lib, EntryPoint = "glGetboolv")]
            internal extern static void Getboolv(GetPName pname, [Out] bool[] data);
            [DllImport(lib, EntryPoint = "glGetdoublev")]
            internal extern static void Getdoublev(GetPName pname, [Out] double[] data);
            [DllImport(lib, EntryPoint = "glGetFloatv")]
            internal extern static void GetFloatv(GetPName pname, [Out] float[] data);
            [DllImport(lib, EntryPoint = "glGetIntegerv")]
            internal extern static void GetIntegerv(GetPName pname, [Out] int[] data);
            [DllImport(lib, EntryPoint = "glGetInteger64v")]
            internal extern static void GetInteger64v(ArbSync pname, [Out] long[] data);
            [DllImport(lib, EntryPoint = "glGetbooli_v")]
            internal extern static void Getbooli_v(GetPName target, uint index, [Out] bool[] data);
            [DllImport(lib, EntryPoint = "glGetIntegeri_v")]
            internal extern static void GetIntegeri_v(GetPName target, uint index, [Out] int[] data);
            [DllImport(lib, EntryPoint = "glGetFloati_v")]
            internal extern static void GetFloati_v(GetPName target, uint index, [Out] float[] data);
            [DllImport(lib, EntryPoint = "glGetdoublei_v")]
            internal extern static void Getdoublei_v(GetPName target, uint index, [Out] double[] data);
            [DllImport(lib, EntryPoint = "glGetInteger64i_v")]
            internal extern static void GetInteger64i_v(GetPName target, uint index, [Out] long[] data);
            [DllImport(lib, EntryPoint = "glGetActiveAtomicCounterBufferiv")]
            internal extern static void GetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, AtomicCounterParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetActiveAttrib")]
            internal extern static void GetActiveAttrib(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] System.Text.StringBuilder name);
            [DllImport(lib, EntryPoint = "glGetActiveSubroutineName")]
            internal extern static void GetActiveSubroutineName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] System.Text.StringBuilder name);
            [DllImport(lib, EntryPoint = "glGetActiveSubroutineUniformiv")]
            internal extern static void GetActiveSubroutineUniformiv(uint program, ShaderType shadertype, uint index, SubroutineParameterName pname, [Out] int[] values);
            [DllImport(lib, EntryPoint = "glGetActiveSubroutineUniformName")]
            internal extern static void GetActiveSubroutineUniformName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] System.Text.StringBuilder name);
            [DllImport(lib, EntryPoint = "glGetActiveUniform")]
            internal extern static void GetActiveUniform(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveUniformType[] type, [Out] System.Text.StringBuilder name);
            [DllImport(lib, EntryPoint = "glGetActiveUniformBlockiv")]
            internal extern static void GetActiveUniformBlockiv(uint program, uint uniformBlockIndex, ActiveUniformBlockParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetActiveUniformBlockName")]
            internal extern static void GetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder uniformBlockName);
            [DllImport(lib, EntryPoint = "glGetActiveUniformName")]
            internal extern static void GetActiveUniformName(uint program, uint uniformIndex, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder uniformName);
            [DllImport(lib, EntryPoint = "glGetActiveUniformsiv")]
            internal extern static void GetActiveUniformsiv(uint program, int uniformCount, [Out] uint[] uniformIndices, ActiveUniformType pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetAttachedShaders")]
            internal extern static void GetAttachedShaders(uint program, int maxCount, [Out] int[] count, [Out] uint[] shaders);
            [DllImport(lib, EntryPoint = "glGetAttribLocation")]
            internal extern static int GetAttribLocation(uint program, string name);
            [DllImport(lib, EntryPoint = "glGetBufferParameteriv")]
            internal extern static void GetBufferParameteriv(BufferTarget target, BufferParameterName value, [Out] int[] data);
            [DllImport(lib, EntryPoint = "glGetBufferParameteri64v")]
            internal extern static void GetBufferParameteri64v(BufferTarget target, BufferParameterName value, [Out] long[] data);
            [DllImport(lib, EntryPoint = "glGetNamedBufferParameteriv")]
            internal extern static void GetNamedBufferParameteriv(uint buffer, BufferParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetNamedBufferParameteri64v")]
            internal extern static void GetNamedBufferParameteri64v(uint buffer, BufferParameterName pname, [Out] long[] @params);
            [DllImport(lib, EntryPoint = "glGetBufferPointerv")]
            internal extern static void GetBufferPointerv(BufferTarget target, BufferPointer pname, [Out] IntPtr @params);
            [DllImport(lib, EntryPoint = "glGetNamedBufferPointerv")]
            internal extern static void GetNamedBufferPointerv(uint buffer, BufferPointer pname, [Out] IntPtr @params);
            [DllImport(lib, EntryPoint = "glGetBufferSubData")]
            internal extern static void GetBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, [Out] IntPtr data);
            [DllImport(lib, EntryPoint = "glGetNamedBufferSubData")]
            internal extern static void GetNamedBufferSubData(uint buffer, IntPtr offset, int size, [Out] IntPtr data);
            [DllImport(lib, EntryPoint = "glGetCompressedTexImage")]
            internal extern static void GetCompressedTexImage(TextureTarget target, int level, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetnCompressedTexImage")]
            internal extern static void GetnCompressedTexImage(TextureTarget target, int level, int bufSize, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetCompressedTextureImage")]
            internal extern static void GetCompressedTextureImage(uint texture, int level, int bufSize, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetCompressedTextureSubImage")]
            internal extern static void GetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetError")]
            internal extern static ErrorCode GetError();
            [DllImport(lib, EntryPoint = "glGetFragDataIndex")]
            internal extern static int GetFragDataIndex(uint program, string name);
            [DllImport(lib, EntryPoint = "glGetFragDataLocation")]
            internal extern static int GetFragDataLocation(uint program, string name);
            [DllImport(lib, EntryPoint = "glGetFramebufferAttachmentParameteriv")]
            internal extern static void GetFramebufferAttachmentParameteriv(FramebufferTarget target, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetNamedFramebufferAttachmentParameteriv")]
            internal extern static void GetNamedFramebufferAttachmentParameteriv(uint framebuffer, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetFramebufferParameteriv")]
            internal extern static void GetFramebufferParameteriv(FramebufferTarget target, FramebufferPName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetNamedFramebufferParameteriv")]
            internal extern static void GetNamedFramebufferParameteriv(uint framebuffer, FramebufferPName pname, [Out] int[] param);
            [DllImport(lib, EntryPoint = "glGetGraphicsResetStatus")]
            internal extern static GraphicResetStatus GetGraphicsResetStatus();
            [DllImport(lib, EntryPoint = "glGetInternalformativ")]
            internal extern static void GetInternalformativ(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetInternalformati64v")]
            internal extern static void GetInternalformati64v(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] long[] @params);
            [DllImport(lib, EntryPoint = "glGetMultisamplefv")]
            internal extern static void GetMultisamplefv(GetMultisamplePName pname, uint index, [Out] float[] val);
            [DllImport(lib, EntryPoint = "glGetObjectLabel")]
            internal extern static void GetObjectLabel(ObjectLabelEnum identifier, uint name, int bifSize, [Out] int[] length, [Out] System.Text.StringBuilder label);
            [DllImport(lib, EntryPoint = "glGetObjectPtrLabel")]
            internal extern static void GetObjectPtrLabel([Out] IntPtr ptr, int bifSize, [Out] int[] length, [Out] System.Text.StringBuilder label);
            [DllImport(lib, EntryPoint = "glGetPointerv")]
            internal extern static void GetPointerv(GetPointerParameter pname, [Out] IntPtr @params);
            [DllImport(lib, EntryPoint = "glGetProgramiv")]
            internal extern static void GetProgramiv(uint program, ProgramParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetProgramBinary")]
            internal extern static void GetProgramBinary(uint program, int bufsize, [Out] int[] length, [Out] int[] binaryFormat, [Out] IntPtr binary);
            [DllImport(lib, EntryPoint = "glGetProgramInfoLog")]
            internal extern static void GetProgramInfoLog(uint program, int maxLength, [Out] int[] length, [Out] System.Text.StringBuilder infoLog);
            [DllImport(lib, EntryPoint = "glGetProgramInterfaceiv")]
            internal extern static void GetProgramInterfaceiv(uint program, ProgramInterface programInterface, ProgramInterfaceParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetProgramPipelineiv")]
            internal extern static void GetProgramPipelineiv(uint pipeline, int pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetProgramPipelineInfoLog")]
            internal extern static void GetProgramPipelineInfoLog(uint pipeline, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder infoLog);
            [DllImport(lib, EntryPoint = "glGetProgramResourceiv")]
            internal extern static void GetProgramResourceiv(uint program, ProgramInterface programInterface, uint index, int propCount, [Out] ProgramResourceParameterName[] props, int bufSize, [Out] int[] length, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetProgramResourceIndex")]
            internal extern static uint GetProgramResourceIndex(uint program, ProgramInterface programInterface, string name);
            [DllImport(lib, EntryPoint = "glGetProgramResourceLocation")]
            internal extern static int GetProgramResourceLocation(uint program, ProgramInterface programInterface, string name);
            [DllImport(lib, EntryPoint = "glGetProgramResourceLocationIndex")]
            internal extern static int GetProgramResourceLocationIndex(uint program, ProgramInterface programInterface, string name);
            [DllImport(lib, EntryPoint = "glGetProgramResourceName")]
            internal extern static void GetProgramResourceName(uint program, ProgramInterface programInterface, uint index, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder name);
            [DllImport(lib, EntryPoint = "glGetProgramStageiv")]
            internal extern static void GetProgramStageiv(uint program, ShaderType shadertype, ProgramStageParameterName pname, [Out] int[] values);
            [DllImport(lib, EntryPoint = "glGetQueryIndexediv")]
            internal extern static void GetQueryIndexediv(QueryTarget target, uint index, GetQueryParam pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetQueryiv")]
            internal extern static void GetQueryiv(QueryTarget target, GetQueryParam pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetQueryObjectiv")]
            internal extern static void GetQueryObjectiv(uint id, GetQueryObjectParam pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetQueryObjectuiv")]
            internal extern static void GetQueryObjectuiv(uint id, GetQueryObjectParam pname, [Out] uint[] @params);
            [DllImport(lib, EntryPoint = "glGetQueryObjecti64v")]
            internal extern static void GetQueryObjecti64v(uint id, GetQueryObjectParam pname, [Out] long[] @params);
            [DllImport(lib, EntryPoint = "glGetQueryObjectui64v")]
            internal extern static void GetQueryObjectui64v(uint id, GetQueryObjectParam pname, [Out] ulong[] @params);
            [DllImport(lib, EntryPoint = "glGetRenderbufferParameteriv")]
            internal extern static void GetRenderbufferParameteriv(RenderbufferTarget target, RenderbufferParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetNamedRenderbufferParameteriv")]
            internal extern static void GetNamedRenderbufferParameteriv(uint renderbuffer, RenderbufferParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetSamplerParameterfv")]
            internal extern static void GetSamplerParameterfv(uint sampler, int pname, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetSamplerParameteriv")]
            internal extern static void GetSamplerParameteriv(uint sampler, int pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetSamplerParameterIiv")]
            internal extern static void GetSamplerParameterIiv(uint sampler, TextureParameterName pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetSamplerParameterIuiv")]
            internal extern static void GetSamplerParameterIuiv(uint sampler, TextureParameterName pname, [Out] uint[] @params);
            [DllImport(lib, EntryPoint = "glGetShaderiv")]
            internal extern static void GetShaderiv(uint shader, ShaderParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetShaderInfoLog")]
            internal extern static void GetShaderInfoLog(uint shader, int maxLength, [Out] int[] length, [Out] System.Text.StringBuilder infoLog);
            [DllImport(lib, EntryPoint = "glGetShaderPrecisionFormat")]
            internal extern static void GetShaderPrecisionFormat(ShaderType shaderType, int precisionType, [Out] int[] range, [Out] int[] precision);
            [DllImport(lib, EntryPoint = "glGetShaderSource")]
            internal extern static void GetShaderSource(uint shader, int bufSize, [Out] int[] length, [Out] System.Text.StringBuilder source);
            [DllImport(lib, EntryPoint = "glGetString")]
            internal extern static IntPtr GetString(StringName name);
            [DllImport(lib, EntryPoint = "glGetStringi")]
            internal extern static IntPtr GetStringi(StringName name, uint index);
            [DllImport(lib, EntryPoint = "glGetSubroutineIndex")]
            internal extern static uint GetSubroutineIndex(uint program, ShaderType shadertype, string name);
            [DllImport(lib, EntryPoint = "glGetSubroutineUniformLocation")]
            internal extern static int GetSubroutineUniformLocation(uint program, ShaderType shadertype, string name);
            [DllImport(lib, EntryPoint = "glGetSynciv")]
            internal extern static void GetSynciv(IntPtr sync, ArbSync pname, int bufSize, [Out] int[] length, [Out] int[] values);
            [DllImport(lib, EntryPoint = "glGetTexImage")]
            internal extern static void GetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetnTexImage")]
            internal extern static void GetnTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetTextureImage")]
            internal extern static void GetTextureImage(uint texture, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetTexLevelParameterfv")]
            internal extern static void GetTexLevelParameterfv(GetPName target, int level, GetTextureLevelParameter pname, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetTexLevelParameteriv")]
            internal extern static void GetTexLevelParameteriv(GetPName target, int level, GetTextureLevelParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetTextureLevelParameterfv")]
            internal extern static void GetTextureLevelParameterfv(uint texture, int level, GetTextureLevelParameter pname, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetTextureLevelParameteriv")]
            internal extern static void GetTextureLevelParameteriv(uint texture, int level, GetTextureLevelParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetTexParameterfv")]
            internal extern static void GetTexParameterfv(TextureTarget target, GetTextureParameter pname, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetTexParameteriv")]
            internal extern static void GetTexParameteriv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetTexParameterIiv")]
            internal extern static void GetTexParameterIiv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetTexParameterIuiv")]
            internal extern static void GetTexParameterIuiv(TextureTarget target, GetTextureParameter pname, [Out] uint[] @params);
            [DllImport(lib, EntryPoint = "glGetTextureParameterfv")]
            internal extern static void GetTextureParameterfv(uint texture, GetTextureParameter pname, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetTextureParameteriv")]
            internal extern static void GetTextureParameteriv(uint texture, GetTextureParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetTextureParameterIiv")]
            internal extern static void GetTextureParameterIiv(uint texture, GetTextureParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetTextureParameterIuiv")]
            internal extern static void GetTextureParameterIuiv(uint texture, GetTextureParameter pname, [Out] uint[] @params);
            [DllImport(lib, EntryPoint = "glGetTextureSubImage")]
            internal extern static void GetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels);
            [DllImport(lib, EntryPoint = "glGetTransformFeedbackiv")]
            internal extern static void GetTransformFeedbackiv(uint xfb, TransformFeedbackParameterName pname, [Out] int[] param);
            [DllImport(lib, EntryPoint = "glGetTransformFeedbacki_v")]
            internal extern static void GetTransformFeedbacki_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] int[] param);
            [DllImport(lib, EntryPoint = "glGetTransformFeedbacki64_v")]
            internal extern static void GetTransformFeedbacki64_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] long[] param);
            [DllImport(lib, EntryPoint = "glGetTransformFeedbackVarying")]
            internal extern static void GetTransformFeedbackVarying(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] System.Text.StringBuilder name);
            [DllImport(lib, EntryPoint = "glGetUniformfv")]
            internal extern static void GetUniformfv(uint program, int location, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetUniformiv")]
            internal extern static void GetUniformiv(uint program, int location, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetUniformuiv")]
            internal extern static void GetUniformuiv(uint program, int location, [Out] uint[] @params);
            [DllImport(lib, EntryPoint = "glGetUniformdv")]
            internal extern static void GetUniformdv(uint program, int location, [Out] double[] @params);
            [DllImport(lib, EntryPoint = "glGetnUniformfv")]
            internal extern static void GetnUniformfv(uint program, int location, int bufSize, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetnUniformiv")]
            internal extern static void GetnUniformiv(uint program, int location, int bufSize, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetnUniformuiv")]
            internal extern static void GetnUniformuiv(uint program, int location, int bufSize, [Out] uint[] @params);
            [DllImport(lib, EntryPoint = "glGetnUniformdv")]
            internal extern static void GetnUniformdv(uint program, int location, int bufSize, [Out] double[] @params);
            [DllImport(lib, EntryPoint = "glGetUniformBlockIndex")]
            internal extern static uint GetUniformBlockIndex(uint program, string uniformBlockName);
            [DllImport(lib, EntryPoint = "glGetUniformIndices")]
            internal extern static void GetUniformIndices(uint program, int uniformCount, string uniformNames, [Out] uint[] uniformIndices);
            [DllImport(lib, EntryPoint = "glGetUniformLocation")]
            internal extern static int GetUniformLocation(uint program, string name);
            [DllImport(lib, EntryPoint = "glGetUniformSubroutineuiv")]
            internal extern static void GetUniformSubroutineuiv(ShaderType shadertype, int location, [Out] uint[] values);
            [DllImport(lib, EntryPoint = "glGetVertexArrayIndexed64iv")]
            internal extern static void GetVertexArrayIndexed64iv(uint vaobj, uint index, VertexAttribParameter pname, [Out] long[] param);
            [DllImport(lib, EntryPoint = "glGetVertexArrayIndexediv")]
            internal extern static void GetVertexArrayIndexediv(uint vaobj, uint index, VertexAttribParameter pname, [Out] int[] param);
            [DllImport(lib, EntryPoint = "glGetVertexArrayiv")]
            internal extern static void GetVertexArrayiv(uint vaobj, VertexAttribParameter pname, [Out] int[] param);
            [DllImport(lib, EntryPoint = "glGetVertexAttribdv")]
            internal extern static void GetVertexAttribdv(uint index, VertexAttribParameter pname, [Out] double[] @params);
            [DllImport(lib, EntryPoint = "glGetVertexAttribfv")]
            internal extern static void GetVertexAttribfv(uint index, VertexAttribParameter pname, [Out] float[] @params);
            [DllImport(lib, EntryPoint = "glGetVertexAttribiv")]
            internal extern static void GetVertexAttribiv(uint index, VertexAttribParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetVertexAttribIiv")]
            internal extern static void GetVertexAttribIiv(uint index, VertexAttribParameter pname, [Out] int[] @params);
            [DllImport(lib, EntryPoint = "glGetVertexAttribIuiv")]
            internal extern static void GetVertexAttribIuiv(uint index, VertexAttribParameter pname, [Out] uint[] @params);
            [DllImport(lib, EntryPoint = "glGetVertexAttribLdv")]
            internal extern static void GetVertexAttribLdv(uint index, VertexAttribParameter pname, [Out] double[] @params);
            [DllImport(lib, EntryPoint = "glGetVertexAttribPointerv")]
            internal extern static void GetVertexAttribPointerv(uint index, VertexAttribPointerParameter pname, [Out] IntPtr pointer);
            [DllImport(lib, EntryPoint = "glHint")]
            internal extern static void Hint(HintTarget target, HintMode mode);
            [DllImport(lib, EntryPoint = "glInvalidateBufferData")]
            internal extern static void InvalidateBufferData(uint buffer);
            [DllImport(lib, EntryPoint = "glInvalidateBufferSubData")]
            internal extern static void InvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length);
            [DllImport(lib, EntryPoint = "glInvalidateFramebuffer")]
            internal extern static void InvalidateFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments);
            [DllImport(lib, EntryPoint = "glInvalidateNamedFramebufferData")]
            internal extern static void InvalidateNamedFramebufferData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments);
            [DllImport(lib, EntryPoint = "glInvalidateSubFramebuffer")]
            internal extern static void InvalidateSubFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glInvalidateNamedFramebufferSubData")]
            internal extern static void InvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glInvalidateTexImage")]
            internal extern static void InvalidateTexImage(uint texture, int level);
            [DllImport(lib, EntryPoint = "glInvalidateTexSubImage")]
            internal extern static void InvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth);
            [DllImport(lib, EntryPoint = "glIsBuffer")]
            internal extern static bool IsBuffer(uint buffer);
            [DllImport(lib, EntryPoint = "glIsEnabled")]
            internal extern static bool IsEnabled(EnableCap cap);
            [DllImport(lib, EntryPoint = "glIsEnabledi")]
            internal extern static bool IsEnabledi(EnableCap cap, uint index);
            [DllImport(lib, EntryPoint = "glIsFramebuffer")]
            internal extern static bool IsFramebuffer(uint framebuffer);
            [DllImport(lib, EntryPoint = "glIsProgram")]
            internal extern static bool IsProgram(uint program);
            [DllImport(lib, EntryPoint = "glIsProgramPipeline")]
            internal extern static bool IsProgramPipeline(uint pipeline);
            [DllImport(lib, EntryPoint = "glIsQuery")]
            internal extern static bool IsQuery(uint id);
            [DllImport(lib, EntryPoint = "glIsRenderbuffer")]
            internal extern static bool IsRenderbuffer(uint renderbuffer);
            [DllImport(lib, EntryPoint = "glIsSampler")]
            internal extern static bool IsSampler(uint id);
            [DllImport(lib, EntryPoint = "glIsShader")]
            internal extern static bool IsShader(uint shader);
            [DllImport(lib, EntryPoint = "glIsSync")]
            internal extern static bool IsSync(IntPtr sync);
            [DllImport(lib, EntryPoint = "glIsTexture")]
            internal extern static bool IsTexture(uint texture);
            [DllImport(lib, EntryPoint = "glIsTransformFeedback")]
            internal extern static bool IsTransformFeedback(uint id);
            [DllImport(lib, EntryPoint = "glIsVertexArray")]
            internal extern static bool IsVertexArray(uint array);
            [DllImport(lib, EntryPoint = "glLineWidth")]
            internal extern static void LineWidth(float width);
            [DllImport(lib, EntryPoint = "glLinkProgram")]
            internal extern static void LinkProgram(uint program);
            [DllImport(lib, EntryPoint = "glLogicOp")]
            internal extern static void LogicOp(LogicOpEnum opcode);
            [DllImport(lib, EntryPoint = "glMapBuffer")]
            internal extern static IntPtr MapBuffer(BufferTarget target, BufferAccess access);
            [DllImport(lib, EntryPoint = "glMapNamedBuffer")]
            internal extern static IntPtr MapNamedBuffer(uint buffer, BufferAccess access);
            [DllImport(lib, EntryPoint = "glMapBufferRange")]
            internal extern static IntPtr MapBufferRange(BufferTarget target, IntPtr offset, IntPtr length, BufferAccessMask access);
            [DllImport(lib, EntryPoint = "glMapNamedBufferRange")]
            internal extern static IntPtr MapNamedBufferRange(uint buffer, IntPtr offset, int length, uint access);
            [DllImport(lib, EntryPoint = "glMemoryBarrier")]
            internal extern static void MemoryBarrier(uint barriers);
            [DllImport(lib, EntryPoint = "glMemoryBarrierByRegion")]
            internal extern static void MemoryBarrierByRegion(uint barriers);
            [DllImport(lib, EntryPoint = "glMinSampleShading")]
            internal extern static void MinSampleShading(float value);
            [DllImport(lib, EntryPoint = "glMultiDrawArrays")]
            internal extern static void MultiDrawArrays(BeginMode mode, int[] first, int[] count, int drawcount);
            [DllImport(lib, EntryPoint = "glMultiDrawArraysIndirect")]
            internal extern static void MultiDrawArraysIndirect(BeginMode mode, IntPtr indirect, int drawcount, int stride);
            [DllImport(lib, EntryPoint = "glMultiDrawElements")]
            internal extern static void MultiDrawElements(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount);
            [DllImport(lib, EntryPoint = "glMultiDrawElementsBaseVertex")]
            internal extern static void MultiDrawElementsBaseVertex(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount, int[] basevertex);
            [DllImport(lib, EntryPoint = "glMultiDrawElementsIndirect")]
            internal extern static void MultiDrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect, int drawcount, int stride);
            [DllImport(lib, EntryPoint = "glObjectLabel")]
            internal extern static void ObjectLabel(ObjectLabelEnum identifier, uint name, int length, string label);
            [DllImport(lib, EntryPoint = "glObjectPtrLabel")]
            internal extern static void ObjectPtrLabel(IntPtr ptr, int length, string label);
            [DllImport(lib, EntryPoint = "glPatchParameteri")]
            internal extern static void PatchParameteri(int pname, int value);
            [DllImport(lib, EntryPoint = "glPatchParameterfv")]
            internal extern static void PatchParameterfv(int pname, float[] values);
            [DllImport(lib, EntryPoint = "glPixelStoref")]
            internal extern static void PixelStoref(PixelStoreParameter pname, float param);
            [DllImport(lib, EntryPoint = "glPixelStorei")]
            internal extern static void PixelStorei(PixelStoreParameter pname, int param);
            [DllImport(lib, EntryPoint = "glPointParameterf")]
            internal extern static void PointParameterf(PointParameterName pname, float param);
            [DllImport(lib, EntryPoint = "glPointParameteri")]
            internal extern static void PointParameteri(PointParameterName pname, int param);
            [DllImport(lib, EntryPoint = "glPointParameterfv")]
            internal extern static void PointParameterfv(PointParameterName pname, float[] @params);
            [DllImport(lib, EntryPoint = "glPointParameteriv")]
            internal extern static void PointParameteriv(PointParameterName pname, int[] @params);
            [DllImport(lib, EntryPoint = "glPointSize")]
            internal extern static void PointSize(float size);
            [DllImport(lib, EntryPoint = "glPolygonMode")]
            internal extern static void PolygonMode(MaterialFace face, PolygonModeEnum mode);
            [DllImport(lib, EntryPoint = "glPolygonOffset")]
            internal extern static void PolygonOffset(float factor, float units);
            [DllImport(lib, EntryPoint = "glPrimitiveRestartIndex")]
            internal extern static void PrimitiveRestartIndex(uint index);
            [DllImport(lib, EntryPoint = "glProgramBinary")]
            internal extern static void ProgramBinary(uint program, int binaryFormat, IntPtr binary, int length);
            [DllImport(lib, EntryPoint = "glProgramParameteri")]
            internal extern static void ProgramParameteri(uint program, Version32 pname, int value);
            [DllImport(lib, EntryPoint = "glProgramUniform1f")]
            internal extern static void ProgramUniform1f(uint program, int location, float v0);
            [DllImport(lib, EntryPoint = "glProgramUniform2f")]
            internal extern static void ProgramUniform2f(uint program, int location, float v0, float v1);
            [DllImport(lib, EntryPoint = "glProgramUniform3f")]
            internal extern static void ProgramUniform3f(uint program, int location, float v0, float v1, float v2);
            [DllImport(lib, EntryPoint = "glProgramUniform4f")]
            internal extern static void ProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3);
            [DllImport(lib, EntryPoint = "glProgramUniform1i")]
            internal extern static void ProgramUniform1i(uint program, int location, int v0);
            [DllImport(lib, EntryPoint = "glProgramUniform2i")]
            internal extern static void ProgramUniform2i(uint program, int location, int v0, int v1);
            [DllImport(lib, EntryPoint = "glProgramUniform3i")]
            internal extern static void ProgramUniform3i(uint program, int location, int v0, int v1, int v2);
            [DllImport(lib, EntryPoint = "glProgramUniform4i")]
            internal extern static void ProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3);
            [DllImport(lib, EntryPoint = "glProgramUniform1ui")]
            internal extern static void ProgramUniform1ui(uint program, int location, uint v0);
            [DllImport(lib, EntryPoint = "glProgramUniform2ui")]
            internal extern static void ProgramUniform2ui(uint program, int location, int v0, uint v1);
            [DllImport(lib, EntryPoint = "glProgramUniform3ui")]
            internal extern static void ProgramUniform3ui(uint program, int location, int v0, int v1, uint v2);
            [DllImport(lib, EntryPoint = "glProgramUniform4ui")]
            internal extern static void ProgramUniform4ui(uint program, int location, int v0, int v1, int v2, uint v3);
            [DllImport(lib, EntryPoint = "glProgramUniform1fv")]
            internal extern static void ProgramUniform1fv(uint program, int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniform2fv")]
            internal extern static void ProgramUniform2fv(uint program, int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniform3fv")]
            internal extern static void ProgramUniform3fv(uint program, int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniform4fv")]
            internal extern static void ProgramUniform4fv(uint program, int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniform1iv")]
            internal extern static void ProgramUniform1iv(uint program, int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glProgramUniform2iv")]
            internal extern static void ProgramUniform2iv(uint program, int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glProgramUniform3iv")]
            internal extern static void ProgramUniform3iv(uint program, int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glProgramUniform4iv")]
            internal extern static void ProgramUniform4iv(uint program, int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glProgramUniform1uiv")]
            internal extern static void ProgramUniform1uiv(uint program, int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glProgramUniform2uiv")]
            internal extern static void ProgramUniform2uiv(uint program, int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glProgramUniform3uiv")]
            internal extern static void ProgramUniform3uiv(uint program, int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glProgramUniform4uiv")]
            internal extern static void ProgramUniform4uiv(uint program, int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix2fv")]
            internal extern static void ProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix3fv")]
            internal extern static void ProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix4fv")]
            internal extern static void ProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix2x3fv")]
            internal extern static void ProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix3x2fv")]
            internal extern static void ProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix2x4fv")]
            internal extern static void ProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix4x2fv")]
            internal extern static void ProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix3x4fv")]
            internal extern static void ProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProgramUniformMatrix4x3fv")]
            internal extern static void ProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glProvokingVertex")]
            internal extern static void ProvokingVertex(ProvokingVertexMode provokeMode);
            [DllImport(lib, EntryPoint = "glQueryCounter")]
            internal extern static void QueryCounter(uint id, int target);
            [DllImport(lib, EntryPoint = "glReadBuffer")]
            internal extern static void ReadBuffer(ReadBufferMode mode);
            [DllImport(lib, EntryPoint = "glNamedFramebufferReadBuffer")]
            internal extern static void NamedFramebufferReadBuffer(uint framebuffer, BeginMode mode);
            [DllImport(lib, EntryPoint = "glReadPixels")]
            internal extern static void ReadPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int[] data);
            [DllImport(lib, EntryPoint = "glReadnPixels")]
            internal extern static void ReadnPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int bufSize, int[] data);
            [DllImport(lib, EntryPoint = "glRenderbufferStorage")]
            internal extern static void RenderbufferStorage(RenderbufferTarget target, RenderbufferStorageEnum internalFormat, int width, int height);
            [DllImport(lib, EntryPoint = "glNamedRenderbufferStorage")]
            internal extern static void NamedRenderbufferStorage(uint renderbuffer, RenderbufferStorageEnum internalFormat, int width, int height);
            [DllImport(lib, EntryPoint = "glRenderbufferStorageMultisample")]
            internal extern static void RenderbufferStorageMultisample(RenderbufferTarget target, int samples, RenderbufferStorageEnum internalFormat, int width, int height);
            [DllImport(lib, EntryPoint = "glNamedRenderbufferStorageMultisample")]
            internal extern static void NamedRenderbufferStorageMultisample(uint renderbuffer, int samples, RenderbufferStorageEnum internalFormat, int width, int height);
            [DllImport(lib, EntryPoint = "glSampleCoverage")]
            internal extern static void SampleCoverage(float value, bool invert);
            [DllImport(lib, EntryPoint = "glSampleMaski")]
            internal extern static void SampleMaski(uint maskNumber, uint mask);
            [DllImport(lib, EntryPoint = "glSamplerParameterf")]
            internal extern static void SamplerParameterf(uint sampler, int pname, float param);
            [DllImport(lib, EntryPoint = "glSamplerParameteri")]
            internal extern static void SamplerParameteri(uint sampler, int pname, int param);
            [DllImport(lib, EntryPoint = "glSamplerParameterfv")]
            internal extern static void SamplerParameterfv(uint sampler, int pname, float[] @params);
            [DllImport(lib, EntryPoint = "glSamplerParameteriv")]
            internal extern static void SamplerParameteriv(uint sampler, int pname, int[] @params);
            [DllImport(lib, EntryPoint = "glSamplerParameterIiv")]
            internal extern static void SamplerParameterIiv(uint sampler, TextureParameterName pname, int[] @params);
            [DllImport(lib, EntryPoint = "glSamplerParameterIuiv")]
            internal extern static void SamplerParameterIuiv(uint sampler, TextureParameterName pname, uint[] @params);
            [DllImport(lib, EntryPoint = "glScissor")]
            internal extern static void Scissor(int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glScissorArrayv")]
            internal extern static void ScissorArrayv(uint first, int count, int[] v);
            [DllImport(lib, EntryPoint = "glScissorIndexed")]
            internal extern static void ScissorIndexed(uint index, int left, int bottom, int width, int height);
            [DllImport(lib, EntryPoint = "glScissorIndexedv")]
            internal extern static void ScissorIndexedv(uint index, int[] v);
            [DllImport(lib, EntryPoint = "glShaderBinary")]
            internal extern static void ShaderBinary(int count, uint[] shaders, int binaryFormat, IntPtr binary, int length);
            [DllImport(lib, EntryPoint = "glShaderSource")]
            internal extern static void ShaderSource(uint shader, int count, string[] @string, int[] length);
            [DllImport(lib, EntryPoint = "glShaderStorageBlockBinding")]
            internal extern static void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding);
            [DllImport(lib, EntryPoint = "glStencilFunc")]
            internal extern static void StencilFunc(StencilFunction func, int @ref, uint mask);
            [DllImport(lib, EntryPoint = "glStencilFuncSeparate")]
            internal extern static void StencilFuncSeparate(StencilFace face, StencilFunction func, int @ref, uint mask);
            [DllImport(lib, EntryPoint = "glStencilMask")]
            internal extern static void StencilMask(uint mask);
            [DllImport(lib, EntryPoint = "glStencilMaskSeparate")]
            internal extern static void StencilMaskSeparate(StencilFace face, uint mask);
            [DllImport(lib, EntryPoint = "glStencilOp")]
            internal extern static void StencilOp(StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass);
            [DllImport(lib, EntryPoint = "glStencilOpSeparate")]
            internal extern static void StencilOpSeparate(StencilFace face, StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass);
            [DllImport(lib, EntryPoint = "glTexBuffer")]
            internal extern static void TexBuffer(TextureBufferTarget target, SizedInternalFormat internalFormat, uint buffer);
            [DllImport(lib, EntryPoint = "glTextureBuffer")]
            internal extern static void TextureBuffer(uint texture, SizedInternalFormat internalFormat, uint buffer);
            [DllImport(lib, EntryPoint = "glTexBufferRange")]
            internal extern static void TexBufferRange(BufferTarget target, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, IntPtr size);
            [DllImport(lib, EntryPoint = "glTextureBufferRange")]
            internal extern static void TextureBufferRange(uint texture, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, int size);
            [DllImport(lib, EntryPoint = "glTexImage1D")]
            internal extern static void TexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, PixelFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glTexImage2D")]
            internal extern static void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glTexImage2DMultisample")]
            internal extern static void TexImage2DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, bool fixedsamplelocations);
            [DllImport(lib, EntryPoint = "glTexImage3D")]
            internal extern static void TexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, PixelFormat format, PixelType type, IntPtr data);
            [DllImport(lib, EntryPoint = "glTexImage3DMultisample")]
            internal extern static void TexImage3DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations);
            [DllImport(lib, EntryPoint = "glTexParameterf")]
            internal extern static void TexParameterf(TextureTarget target, TextureParameterName pname, float param);
            [DllImport(lib, EntryPoint = "glTexParameteri")]
            internal extern static void TexParameteri(TextureTarget target, TextureParameterName pname, int param);
            [DllImport(lib, EntryPoint = "glTextureParameterf")]
            internal extern static void TextureParameterf(uint texture, TextureParameter pname, float param);
            [DllImport(lib, EntryPoint = "glTextureParameteri")]
            internal extern static void TextureParameteri(uint texture, TextureParameter pname, int param);
            [DllImport(lib, EntryPoint = "glTexParameterfv")]
            internal extern static void TexParameterfv(TextureTarget target, TextureParameterName pname, float[] @params);
            [DllImport(lib, EntryPoint = "glTexParameteriv")]
            internal extern static void TexParameteriv(TextureTarget target, TextureParameterName pname, int[] @params);
            [DllImport(lib, EntryPoint = "glTexParameterIiv")]
            internal extern static void TexParameterIiv(TextureTarget target, TextureParameterName pname, int[] @params);
            [DllImport(lib, EntryPoint = "glTexParameterIuiv")]
            internal extern static void TexParameterIuiv(TextureTarget target, TextureParameterName pname, uint[] @params);
            [DllImport(lib, EntryPoint = "glTextureParameterfv")]
            internal extern static void TextureParameterfv(uint texture, TextureParameter pname, float[] paramtexture);
            [DllImport(lib, EntryPoint = "glTextureParameteriv")]
            internal extern static void TextureParameteriv(uint texture, TextureParameter pname, int[] param);
            [DllImport(lib, EntryPoint = "glTextureParameterIiv")]
            internal extern static void TextureParameterIiv(uint texture, TextureParameter pname, int[] @params);
            [DllImport(lib, EntryPoint = "glTextureParameterIuiv")]
            internal extern static void TextureParameterIuiv(uint texture, TextureParameter pname, uint[] @params);
            [DllImport(lib, EntryPoint = "glTexStorage1D")]
            internal extern static void TexStorage1D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width);
            [DllImport(lib, EntryPoint = "glTextureStorage1D")]
            internal extern static void TextureStorage1D(uint texture, int levels, SizedInternalFormat internalFormat, int width);
            [DllImport(lib, EntryPoint = "glTexStorage2D")]
            internal extern static void TexStorage2D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height);
            [DllImport(lib, EntryPoint = "glTextureStorage2D")]
            internal extern static void TextureStorage2D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height);
            [DllImport(lib, EntryPoint = "glTexStorage2DMultisample")]
            internal extern static void TexStorage2DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations);
            [DllImport(lib, EntryPoint = "glTextureStorage2DMultisample")]
            internal extern static void TextureStorage2DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations);
            [DllImport(lib, EntryPoint = "glTexStorage3D")]
            internal extern static void TexStorage3D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height, int depth);
            [DllImport(lib, EntryPoint = "glTextureStorage3D")]
            internal extern static void TextureStorage3D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height, int depth);
            [DllImport(lib, EntryPoint = "glTexStorage3DMultisample")]
            internal extern static void TexStorage3DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations);
            [DllImport(lib, EntryPoint = "glTextureStorage3DMultisample")]
            internal extern static void TextureStorage3DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations);
            [DllImport(lib, EntryPoint = "glTexSubImage1D")]
            internal extern static void TexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels);
            [DllImport(lib, EntryPoint = "glTextureSubImage1D")]
            internal extern static void TextureSubImage1D(uint texture, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels);
            [DllImport(lib, EntryPoint = "glTexSubImage2D")]
            internal extern static void TexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels);
            [DllImport(lib, EntryPoint = "glTextureSubImage2D")]
            internal extern static void TextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels);
            [DllImport(lib, EntryPoint = "glTexSubImage3D")]
            internal extern static void TexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels);
            [DllImport(lib, EntryPoint = "glTextureSubImage3D")]
            internal extern static void TextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels);
            [DllImport(lib, EntryPoint = "glTextureBarrier")]
            internal extern static void TextureBarrier();
            [DllImport(lib, EntryPoint = "glTextureView")]
            internal extern static void TextureView(uint texture, TextureTarget target, uint origtexture, PixelInternalFormat internalFormat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);
            [DllImport(lib, EntryPoint = "glTransformFeedbackBufferBase")]
            internal extern static void TransformFeedbackBufferBase(uint xfb, uint index, uint buffer);
            [DllImport(lib, EntryPoint = "glTransformFeedbackBufferRange")]
            internal extern static void TransformFeedbackBufferRange(uint xfb, uint index, uint buffer, IntPtr offset, int size);
            [DllImport(lib, EntryPoint = "glTransformFeedbackVaryings")]
            internal extern static void TransformFeedbackVaryings(uint program, int count, string[] varyings, TransformFeedbackMode bufferMode);
            [DllImport(lib, EntryPoint = "glUniform1f")]
            internal extern static void Uniform1f(int location, float v0);
            [DllImport(lib, EntryPoint = "glUniform2f")]
            internal extern static void Uniform2f(int location, float v0, float v1);
            [DllImport(lib, EntryPoint = "glUniform3f")]
            internal extern static void Uniform3f(int location, float v0, float v1, float v2);
            [DllImport(lib, EntryPoint = "glUniform4f")]
            internal extern static void Uniform4f(int location, float v0, float v1, float v2, float v3);
            [DllImport(lib, EntryPoint = "glUniform1i")]
            internal extern static void Uniform1i(int location, int v0);
            [DllImport(lib, EntryPoint = "glUniform2i")]
            internal extern static void Uniform2i(int location, int v0, int v1);
            [DllImport(lib, EntryPoint = "glUniform3i")]
            internal extern static void Uniform3i(int location, int v0, int v1, int v2);
            [DllImport(lib, EntryPoint = "glUniform4i")]
            internal extern static void Uniform4i(int location, int v0, int v1, int v2, int v3);
            [DllImport(lib, EntryPoint = "glUniform1ui")]
            internal extern static void Uniform1ui(int location, uint v0);
            [DllImport(lib, EntryPoint = "glUniform2ui")]
            internal extern static void Uniform2ui(int location, uint v0, uint v1);
            [DllImport(lib, EntryPoint = "glUniform3ui")]
            internal extern static void Uniform3ui(int location, uint v0, uint v1, uint v2);
            [DllImport(lib, EntryPoint = "glUniform4ui")]
            internal extern static void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3);
            [DllImport(lib, EntryPoint = "glUniform1fv")]
            internal extern static void Uniform1fv(int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glUniform2fv")]
            internal extern static void Uniform2fv(int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glUniform3fv")]
            internal extern static void Uniform3fv(int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glUniform4fv")]
            internal extern static void Uniform4fv(int location, int count, float* value);
            [DllImport(lib, EntryPoint = "glUniform1iv")]
            internal extern static void Uniform1iv(int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glUniform2iv")]
            internal extern static void Uniform2iv(int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glUniform3iv")]
            internal extern static void Uniform3iv(int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glUniform4iv")]
            internal extern static void Uniform4iv(int location, int count, int* value);
            [DllImport(lib, EntryPoint = "glUniform1uiv")]
            internal extern static void Uniform1uiv(int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glUniform2uiv")]
            internal extern static void Uniform2uiv(int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glUniform3uiv")]
            internal extern static void Uniform3uiv(int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glUniform4uiv")]
            internal extern static void Uniform4uiv(int location, int count, uint* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix2fv")]
            internal extern static void UniformMatrix2fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix3fv")]
            internal extern static void UniformMatrix3fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix4fv")]
            internal extern static void UniformMatrix4fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix2x3fv")]
            internal extern static void UniformMatrix2x3fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix3x2fv")]
            internal extern static void UniformMatrix3x2fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix2x4fv")]
            internal extern static void UniformMatrix2x4fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix4x2fv")]
            internal extern static void UniformMatrix4x2fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix3x4fv")]
            internal extern static void UniformMatrix3x4fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformMatrix4x3fv")]
            internal extern static void UniformMatrix4x3fv(int location, int count, bool transpose, float* value);
            [DllImport(lib, EntryPoint = "glUniformBlockBinding")]
            internal extern static void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
            [DllImport(lib, EntryPoint = "glUniformSubroutinesuiv")]
            internal extern static void UniformSubroutinesuiv(ShaderType shadertype, int count, uint[] indices);
            [DllImport(lib, EntryPoint = "glUnmapBuffer")]
            internal extern static bool UnmapBuffer(BufferTarget target);
            [DllImport(lib, EntryPoint = "glUnmapNamedBuffer")]
            internal extern static bool UnmapNamedBuffer(uint buffer);
            [DllImport(lib, EntryPoint = "glUseProgram")]
            internal extern static void UseProgram(uint program);
            [DllImport(lib, EntryPoint = "glUseProgramStages")]
            internal extern static void UseProgramStages(uint pipeline, uint stages, uint program);
            [DllImport(lib, EntryPoint = "glValidateProgram")]
            internal extern static void ValidateProgram(uint program);
            [DllImport(lib, EntryPoint = "glValidateProgramPipeline")]
            internal extern static void ValidateProgramPipeline(uint pipeline);
            [DllImport(lib, EntryPoint = "glVertexArrayElementBuffer")]
            internal extern static void VertexArrayElementBuffer(uint vaobj, uint buffer);
            [DllImport(lib, EntryPoint = "glVertexAttrib1f")]
            internal extern static void VertexAttrib1f(uint index, float v0);
            [DllImport(lib, EntryPoint = "glVertexAttrib1s")]
            internal extern static void VertexAttrib1s(uint index, short v0);
            [DllImport(lib, EntryPoint = "glVertexAttrib1d")]
            internal extern static void VertexAttrib1d(uint index, double v0);
            [DllImport(lib, EntryPoint = "glVertexAttribI1i")]
            internal extern static void VertexAttribI1i(uint index, int v0);
            [DllImport(lib, EntryPoint = "glVertexAttribI1ui")]
            internal extern static void VertexAttribI1ui(uint index, uint v0);
            [DllImport(lib, EntryPoint = "glVertexAttrib2f")]
            internal extern static void VertexAttrib2f(uint index, float v0, float v1);
            [DllImport(lib, EntryPoint = "glVertexAttrib2s")]
            internal extern static void VertexAttrib2s(uint index, short v0, short v1);
            [DllImport(lib, EntryPoint = "glVertexAttrib2d")]
            internal extern static void VertexAttrib2d(uint index, double v0, double v1);
            [DllImport(lib, EntryPoint = "glVertexAttribI2i")]
            internal extern static void VertexAttribI2i(uint index, int v0, int v1);
            [DllImport(lib, EntryPoint = "glVertexAttribI2ui")]
            internal extern static void VertexAttribI2ui(uint index, uint v0, uint v1);
            [DllImport(lib, EntryPoint = "glVertexAttrib3f")]
            internal extern static void VertexAttrib3f(uint index, float v0, float v1, float v2);
            [DllImport(lib, EntryPoint = "glVertexAttrib3s")]
            internal extern static void VertexAttrib3s(uint index, short v0, short v1, short v2);
            [DllImport(lib, EntryPoint = "glVertexAttrib3d")]
            internal extern static void VertexAttrib3d(uint index, double v0, double v1, double v2);
            [DllImport(lib, EntryPoint = "glVertexAttribI3i")]
            internal extern static void VertexAttribI3i(uint index, int v0, int v1, int v2);
            [DllImport(lib, EntryPoint = "glVertexAttribI3ui")]
            internal extern static void VertexAttribI3ui(uint index, uint v0, uint v1, uint v2);
            [DllImport(lib, EntryPoint = "glVertexAttrib4f")]
            internal extern static void VertexAttrib4f(uint index, float v0, float v1, float v2, float v3);
            [DllImport(lib, EntryPoint = "glVertexAttrib4s")]
            internal extern static void VertexAttrib4s(uint index, short v0, short v1, short v2, short v3);
            [DllImport(lib, EntryPoint = "glVertexAttrib4d")]
            internal extern static void VertexAttrib4d(uint index, double v0, double v1, double v2, double v3);
            [DllImport(lib, EntryPoint = "glVertexAttrib4Nub")]
            internal extern static void VertexAttrib4Nub(uint index, byte v0, byte v1, byte v2, byte v3);
            [DllImport(lib, EntryPoint = "glVertexAttribI4i")]
            internal extern static void VertexAttribI4i(uint index, int v0, int v1, int v2, int v3);
            [DllImport(lib, EntryPoint = "glVertexAttribI4ui")]
            internal extern static void VertexAttribI4ui(uint index, uint v0, uint v1, uint v2, uint v3);
            [DllImport(lib, EntryPoint = "glVertexAttribL1d")]
            internal extern static void VertexAttribL1d(uint index, double v0);
            [DllImport(lib, EntryPoint = "glVertexAttribL2d")]
            internal extern static void VertexAttribL2d(uint index, double v0, double v1);
            [DllImport(lib, EntryPoint = "glVertexAttribL3d")]
            internal extern static void VertexAttribL3d(uint index, double v0, double v1, double v2);
            [DllImport(lib, EntryPoint = "glVertexAttribL4d")]
            internal extern static void VertexAttribL4d(uint index, double v0, double v1, double v2, double v3);
            [DllImport(lib, EntryPoint = "glVertexAttrib1fv")]
            internal extern static void VertexAttrib1fv(uint index, float[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib1sv")]
            internal extern static void VertexAttrib1sv(uint index, short[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib1dv")]
            internal extern static void VertexAttrib1dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI1iv")]
            internal extern static void VertexAttribI1iv(uint index, int[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI1uiv")]
            internal extern static void VertexAttribI1uiv(uint index, uint[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib2fv")]
            internal extern static void VertexAttrib2fv(uint index, float[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib2sv")]
            internal extern static void VertexAttrib2sv(uint index, short[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib2dv")]
            internal extern static void VertexAttrib2dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI2iv")]
            internal extern static void VertexAttribI2iv(uint index, int[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI2uiv")]
            internal extern static void VertexAttribI2uiv(uint index, uint[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib3fv")]
            internal extern static void VertexAttrib3fv(uint index, float[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib3sv")]
            internal extern static void VertexAttrib3sv(uint index, short[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib3dv")]
            internal extern static void VertexAttrib3dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI3iv")]
            internal extern static void VertexAttribI3iv(uint index, int[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI3uiv")]
            internal extern static void VertexAttribI3uiv(uint index, uint[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4fv")]
            internal extern static void VertexAttrib4fv(uint index, float[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4sv")]
            internal extern static void VertexAttrib4sv(uint index, short[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4dv")]
            internal extern static void VertexAttrib4dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4iv")]
            internal extern static void VertexAttrib4iv(uint index, int[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4bv")]
            internal extern static void VertexAttrib4bv(uint index, sbyte[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4ubv")]
            internal extern static void VertexAttrib4ubv(uint index, byte[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4usv")]
            internal extern static void VertexAttrib4usv(uint index, ushort[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4uiv")]
            internal extern static void VertexAttrib4uiv(uint index, uint[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4Nbv")]
            internal extern static void VertexAttrib4Nbv(uint index, sbyte[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4Nsv")]
            internal extern static void VertexAttrib4Nsv(uint index, short[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4Niv")]
            internal extern static void VertexAttrib4Niv(uint index, int[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4Nubv")]
            internal extern static void VertexAttrib4Nubv(uint index, byte[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4Nusv")]
            internal extern static void VertexAttrib4Nusv(uint index, ushort[] v);
            [DllImport(lib, EntryPoint = "glVertexAttrib4Nuiv")]
            internal extern static void VertexAttrib4Nuiv(uint index, uint[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI4bv")]
            internal extern static void VertexAttribI4bv(uint index, sbyte[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI4ubv")]
            internal extern static void VertexAttribI4ubv(uint index, byte[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI4sv")]
            internal extern static void VertexAttribI4sv(uint index, short[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI4usv")]
            internal extern static void VertexAttribI4usv(uint index, ushort[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI4iv")]
            internal extern static void VertexAttribI4iv(uint index, int[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribI4uiv")]
            internal extern static void VertexAttribI4uiv(uint index, uint[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribL1dv")]
            internal extern static void VertexAttribL1dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribL2dv")]
            internal extern static void VertexAttribL2dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribL3dv")]
            internal extern static void VertexAttribL3dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribL4dv")]
            internal extern static void VertexAttribL4dv(uint index, double[] v);
            [DllImport(lib, EntryPoint = "glVertexAttribP1ui")]
            internal extern static void VertexAttribP1ui(uint index, VertexAttribPType type, bool normalized, uint value);
            [DllImport(lib, EntryPoint = "glVertexAttribP2ui")]
            internal extern static void VertexAttribP2ui(uint index, VertexAttribPType type, bool normalized, uint value);
            [DllImport(lib, EntryPoint = "glVertexAttribP3ui")]
            internal extern static void VertexAttribP3ui(uint index, VertexAttribPType type, bool normalized, uint value);
            [DllImport(lib, EntryPoint = "glVertexAttribP4ui")]
            internal extern static void VertexAttribP4ui(uint index, VertexAttribPType type, bool normalized, uint value);
            [DllImport(lib, EntryPoint = "glVertexAttribBinding")]
            internal extern static void VertexAttribBinding(uint attribindex, uint bindingindex);
            [DllImport(lib, EntryPoint = "glVertexArrayAttribBinding")]
            internal extern static void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex);
            [DllImport(lib, EntryPoint = "glVertexAttribDivisor")]
            internal extern static void VertexAttribDivisor(uint index, uint divisor);
            [DllImport(lib, EntryPoint = "glVertexAttribFormat")]
            internal extern static void VertexAttribFormat(uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset);
            [DllImport(lib, EntryPoint = "glVertexAttribIFormat")]
            internal extern static void VertexAttribIFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
            [DllImport(lib, EntryPoint = "glVertexAttribLFormat")]
            internal extern static void VertexAttribLFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
            [DllImport(lib, EntryPoint = "glVertexArrayAttribFormat")]
            internal extern static void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset);
            [DllImport(lib, EntryPoint = "glVertexArrayAttribIFormat")]
            internal extern static void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
            [DllImport(lib, EntryPoint = "glVertexArrayAttribLFormat")]
            internal extern static void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset);
            [DllImport(lib, EntryPoint = "glVertexAttribPointer")]
            internal extern static void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, IntPtr pointer);
            [DllImport(lib, EntryPoint = "glVertexAttribIPointer")]
            internal extern static void VertexAttribIPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer);
            [DllImport(lib, EntryPoint = "glVertexAttribLPointer")]
            internal extern static void VertexAttribLPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer);
            [DllImport(lib, EntryPoint = "glVertexBindingDivisor")]
            internal extern static void VertexBindingDivisor(uint bindingindex, uint divisor);
            [DllImport(lib, EntryPoint = "glVertexArrayBindingDivisor")]
            internal extern static void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor);
            [DllImport(lib, EntryPoint = "glViewport")]
            internal extern static void Viewport(int x, int y, int width, int height);
            [DllImport(lib, EntryPoint = "glViewportArrayv")]
            internal extern static void ViewportArrayv(uint first, int count, float[] v);
            [DllImport(lib, EntryPoint = "glViewportIndexedf")]
            internal extern static void ViewportIndexedf(uint index, float x, float y, float w, float h);
            [DllImport(lib, EntryPoint = "glViewportIndexedfv")]
            internal extern static void ViewportIndexedfv(uint index, float[] v);
            [DllImport(lib, EntryPoint = "glWaitSync")]
            internal extern static void WaitSync(IntPtr sync, uint flags, ulong timeout);
        }
    }
}