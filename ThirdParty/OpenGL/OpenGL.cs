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
using System.Text;

public unsafe static partial class OpenGL
{
    static GetProcAddress getProcAddress = null;
    static string[] stringArray = new string[1];
    static uint[] uintArray = new uint[1];
    static int[] intArray = new int[1];
    static bool init = false;

    public static void LoadFunctions()
    {
        if (init)
            return;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            getProcAddress = wglGetProcAddress;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            getProcAddress = glxGetProcAddress;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            getProcAddress = osxGetProcAddress;
        else
            return;

        var methods = NativeCalls.Methods;
        var delegates = typeof(Delegates).GetFields(BindingFlags.Static | BindingFlags.Public);
        for (int i = 0; i < delegates.Length; i++)
        {
            var m = delegates[i];
            var r = GetProcDelegate(m.Name, m.FieldType);

            // try NativeCalls to create the delegate
            if (r == null)
            {
                MethodInfo v;
                if (methods.TryGetValue(m.Name.Substring(2), out v))
                {
                    r = v.CreateDelegate(m.FieldType);
                }
            }

            if (r != null)
                m.SetValue(null, r);
        }

        init = true;
    }

    internal static Delegate GetProcDelegate(string proc, Type type)
    {
        var address = getProcAddress(proc);
        if (address.ToInt64() < 3)
            return null;
        else
            return Marshal.GetDelegateForFunctionPointer(address, type);
    }

    [DllImport("opengl32.dll")]
    static extern IntPtr wglGetProcAddress(string proc);

    [DllImport("libGL.so.1")]
    static extern IntPtr glxGetProcAddress(string proc);

    [DllImport("libdl.dylib")]
    static extern bool NSIsSymbolNameDefined(string s);

    [DllImport("libdl.dylib")]
    static extern IntPtr NSLookupAndBindSymbol(string s);

    [DllImport("libdl.dylib")]
    static extern IntPtr NSAddressOfSymbol(IntPtr symbol);

    static IntPtr osxGetProcAddress(string proc)
    {
        var symbolName = "_" + proc;
        if (NSIsSymbolNameDefined(symbolName))
        {
            IntPtr symbol = NSLookupAndBindSymbol(symbolName);
            if (symbol != IntPtr.Zero)
                return NSAddressOfSymbol(symbol);
        }
        return IntPtr.Zero;
    }

    public static void glActiveShaderProgram(uint pipeline, uint program) { Delegates.glActiveShaderProgram(pipeline, program); }
    public static void glActiveTexture(TextureUnit texture) { Delegates.glActiveTexture(texture); }
    public static void glAttachShader(uint program, uint shader) { Delegates.glAttachShader(program, shader); }
    public static void glBeginConditionalRender(uint id, ConditionalRenderType mode) { Delegates.glBeginConditionalRender(id, mode); }
    public static void glEndConditionalRender() { Delegates.glEndConditionalRender(); }
    public static void glBeginQuery(QueryTarget target, uint id) { Delegates.glBeginQuery(target, id); }
    public static void glEndQuery(QueryTarget target) { Delegates.glEndQuery(target); }
    public static void glBeginQueryIndexed(uint target, uint index, uint id) { Delegates.glBeginQueryIndexed(target, index, id); }
    public static void glEndQueryIndexed(QueryTarget target, uint index) { Delegates.glEndQueryIndexed(target, index); }
    public static void glBeginTransformFeedback(BeginFeedbackMode primitiveMode) { Delegates.glBeginTransformFeedback(primitiveMode); }
    public static void glEndTransformFeedback() { Delegates.glEndTransformFeedback(); }
    public static void glBindAttribLocation(uint program, uint index, string name) { Delegates.glBindAttribLocation(program, index, name); }
    public static void glBindBuffer(BufferTarget target, uint buffer) { Delegates.glBindBuffer(target, buffer); }
    public static void glBindBufferBase(BufferTarget target, uint index, uint buffer) { Delegates.glBindBufferBase(target, index, buffer); }
    public static void glBindBufferRange(BufferTarget target, uint index, uint buffer, IntPtr offset, IntPtr size) { Delegates.glBindBufferRange(target, index, buffer, offset, size); }
    public static void glBindBuffersBase(BufferTarget target, uint first, int count, uint[] buffers) { Delegates.glBindBuffersBase(target, first, count, buffers); }
    public static void glBindBuffersRange(BufferTarget target, uint first, int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes) { Delegates.glBindBuffersRange(target, first, count, buffers, offsets, sizes); }
    public static void glBindFragDataLocation(uint program, uint colorNumber, string name) { Delegates.glBindFragDataLocation(program, colorNumber, name); }
    public static void glBindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name) { Delegates.glBindFragDataLocationIndexed(program, colorNumber, index, name); }
    public static void glBindFramebuffer(FramebufferTarget target, uint framebuffer) { Delegates.glBindFramebuffer(target, framebuffer); }
    public static void glBindImageTexture(uint unit, uint texture, int level, bool layered, int layer, BufferAccess access, PixelInternalFormat format) { Delegates.glBindImageTexture(unit, texture, level, layered, layer, access, format); }
    public static void glBindImageTextures(uint first, int count, uint[] textures) { Delegates.glBindImageTextures(first, count, textures); }
    public static void glBindProgramPipeline(uint pipeline) { Delegates.glBindProgramPipeline(pipeline); }
    public static void glBindRenderbuffer(RenderbufferTarget target, uint renderbuffer) { Delegates.glBindRenderbuffer(target, renderbuffer); }
    public static void glBindSampler(uint unit, uint sampler) { Delegates.glBindSampler(unit, sampler); }
    public static void glBindSamplers(uint first, int count, uint[] samplers) { Delegates.glBindSamplers(first, count, samplers); }
    public static void glBindTexture(TextureTarget target, uint texture) { Delegates.glBindTexture(target, texture); }
    public static void glBindTextures(uint first, int count, uint[] textures) { Delegates.glBindTextures(first, count, textures); }
    public static void glBindTextureUnit(uint unit, uint texture) { Delegates.glBindTextureUnit(unit, texture); }
    public static void glBindTransformFeedback(NvTransformFeedback2 target, uint id) { Delegates.glBindTransformFeedback(target, id); }
    public static void glBindVertexArray(uint array) { Delegates.glBindVertexArray(array); }
    public static void glBindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, IntPtr stride) { Delegates.glBindVertexBuffer(bindingindex, buffer, offset, stride); }
    public static void glVertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride) { Delegates.glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride); }
    public static void glBindVertexBuffers(uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides) { Delegates.glBindVertexBuffers(first, count, buffers, offsets, strides); }
    public static void glVertexArrayVertexBuffers(uint vaobj, uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides) { Delegates.glVertexArrayVertexBuffers(vaobj, first, count, buffers, offsets, strides); }
    public static void glBlendColor(float red, float green, float blue, float alpha) { Delegates.glBlendColor(red, green, blue, alpha); }
    public static void glBlendEquation(BlendEquationMode mode) { Delegates.glBlendEquation(mode); }
    public static void glBlendEquationi(uint buf, BlendEquationMode mode) { Delegates.glBlendEquationi(buf, mode); }
    public static void glBlendEquationSeparate(BlendEquationMode modeRGB, BlendEquationMode modeAlpha) { Delegates.glBlendEquationSeparate(modeRGB, modeAlpha); }
    public static void glBlendEquationSeparatei(uint buf, BlendEquationMode modeRGB, BlendEquationMode modeAlpha) { Delegates.glBlendEquationSeparatei(buf, modeRGB, modeAlpha); }
    public static void glBlendFunc(BlendingFactorSrc sfactor, BlendingFactorDest dfactor) { Delegates.glBlendFunc(sfactor, dfactor); }
    public static void glBlendFunci(uint buf, BlendingFactorSrc sfactor, BlendingFactorDest dfactor) { Delegates.glBlendFunci(buf, sfactor, dfactor); }
    public static void glBlendFuncSeparate(BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha) { Delegates.glBlendFuncSeparate(srcRGB, dstRGB, srcAlpha, dstAlpha); }
    public static void glBlendFuncSeparatei(uint buf, BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha) { Delegates.glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha); }
    public static void glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter) { Delegates.glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter); }
    public static void glBlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter) { Delegates.glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter); }
    public static void glBufferData(BufferTarget target, int size, IntPtr data, BufferUsageHint usage) { Delegates.glBufferData(target, size, data, usage); }
    public static void glNamedBufferData(uint buffer, int size, IntPtr data, BufferUsageHint usage) { Delegates.glNamedBufferData(buffer, size, data, usage); }
    public static void glBufferStorage(BufferTarget target, IntPtr size, IntPtr data, uint flags) { Delegates.glBufferStorage(target, size, data, flags); }
    public static void glNamedBufferStorage(uint buffer, int size, IntPtr data, uint flags) { Delegates.glNamedBufferStorage(buffer, size, data, flags); }
    public static void glBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, IntPtr data) { Delegates.glBufferSubData(target, offset, size, data); }
    public static void glNamedBufferSubData(uint buffer, IntPtr offset, int size, IntPtr data) { Delegates.glNamedBufferSubData(buffer, offset, size, data); }
    public static FramebufferErrorCode glCheckFramebufferStatus(FramebufferTarget target) { return Delegates.glCheckFramebufferStatus(target); }
    public static FramebufferErrorCode glCheckNamedFramebufferStatus(uint framebuffer, FramebufferTarget target) { return Delegates.glCheckNamedFramebufferStatus(framebuffer, target); }
    public static void glClampColor(ClampColorTarget target, ClampColorMode clamp) { Delegates.glClampColor(target, clamp); }
    public static void glClear(ClearBufferMask mask) { Delegates.glClear(mask); }
    public static void glClearBufferiv(ClearBuffer buffer, int drawbuffer, int* value) { Delegates.glClearBufferiv(buffer, drawbuffer, value); }
    public static void glClearBufferuiv(ClearBuffer buffer, int drawbuffer, uint* value) { Delegates.glClearBufferuiv(buffer, drawbuffer, value); }
    public static void glClearBufferfv(ClearBuffer buffer, int drawbuffer, float* value) { Delegates.glClearBufferfv(buffer, drawbuffer, value); }
    public static void glClearBufferfi(ClearBuffer buffer, int drawbuffer, float depth, int stencil) { Delegates.glClearBufferfi(buffer, drawbuffer, depth, stencil); }
    public static void glClearNamedFramebufferiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, int[] value) { Delegates.glClearNamedFramebufferiv(framebuffer, buffer, drawbuffer, value); }
    public static void glClearNamedFramebufferuiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, uint[] value) { Delegates.glClearNamedFramebufferuiv(framebuffer, buffer, drawbuffer, value); }
    public static void glClearNamedFramebufferfv(uint framebuffer, ClearBuffer buffer, int drawbuffer, float[] value) { Delegates.glClearNamedFramebufferfv(framebuffer, buffer, drawbuffer, value); }
    public static void glClearNamedFramebufferfi(uint framebuffer, ClearBuffer buffer, float depth, int stencil) { Delegates.glClearNamedFramebufferfi(framebuffer, buffer, depth, stencil); }
    public static void glClearBufferData(BufferTarget target, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearBufferData(target, internalFormat, format, type, data); }
    public static void glClearNamedBufferData(uint buffer, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearNamedBufferData(buffer, internalFormat, format, type, data); }
    public static void glClearBufferSubData(BufferTarget target, SizedInternalFormat internalFormat, IntPtr offset, IntPtr size, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearBufferSubData(target, internalFormat, offset, size, format, type, data); }
    public static void glClearNamedBufferSubData(uint buffer, SizedInternalFormat internalFormat, IntPtr offset, int size, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearNamedBufferSubData(buffer, internalFormat, offset, size, format, type, data); }
    public static void glClearColor(float red, float green, float blue, float alpha) { Delegates.glClearColor(red, green, blue, alpha); }
    public static void glClearDepth(double depth) { Delegates.glClearDepth(depth); }
    public static void glClearDepthf(float depth) { Delegates.glClearDepthf(depth); }
    public static void glClearStencil(int s) { Delegates.glClearStencil(s); }
    public static void glClearTexImage(uint texture, int level, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearTexImage(texture, level, format, type, data); }
    public static void glClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data); }
    public static ArbSync glClientWaitSync(IntPtr sync, uint flags, ulong timeout) { return Delegates.glClientWaitSync(sync, flags, timeout); }
    public static void glClipControl(ClipControlOrigin origin, ClipControlDepth depth) { Delegates.glClipControl(origin, depth); }
    public static void glColorMask(bool red, bool green, bool blue, bool alpha) { Delegates.glColorMask(red, green, blue, alpha); }
    public static void glColorMaski(uint buf, bool red, bool green, bool blue, bool alpha) { Delegates.glColorMaski(buf, red, green, blue, alpha); }
    public static void glCompileShader(uint shader) { Delegates.glCompileShader(shader); }
    public static void glCompressedTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, int imageSize, IntPtr data) { Delegates.glCompressedTexImage1D(target, level, internalFormat, width, border, imageSize, data); }
    public static void glCompressedTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, int imageSize, IntPtr data) { Delegates.glCompressedTexImage2D(target, level, internalFormat, width, height, border, imageSize, data); }
    public static void glCompressedTexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data) { Delegates.glCompressedTexImage3D(target, level, internalFormat, width, height, depth, border, imageSize, data); }
    public static void glCompressedTexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data); }
    public static void glCompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, PixelInternalFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data); }
    public static void glCompressedTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data); }
    public static void glCompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelInternalFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data); }
    public static void glCompressedTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data); }
    public static void glCompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data); }
    public static void glCopyBufferSubData(BufferTarget readTarget, BufferTarget writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size) { Delegates.glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size); }
    public static void glCopyNamedBufferSubData(uint readBuffer, uint writeBuffer, IntPtr readOffset, IntPtr writeOffset, int size) { Delegates.glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size); }
    public static void glCopyImageSubData(uint srcName, BufferTarget srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, BufferTarget dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) { Delegates.glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth); }
    public static void glCopyTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int border) { Delegates.glCopyTexImage1D(target, level, internalFormat, x, y, width, border); }
    public static void glCopyTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int height, int border) { Delegates.glCopyTexImage2D(target, level, internalFormat, x, y, width, height, border); }
    public static void glCopyTexSubImage1D(TextureTarget target, int level, int xoffset, int x, int y, int width) { Delegates.glCopyTexSubImage1D(target, level, xoffset, x, y, width); }
    public static void glCopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) { Delegates.glCopyTextureSubImage1D(texture, level, xoffset, x, y, width); }
    public static void glCopyTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height) { Delegates.glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height); }
    public static void glCopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) { Delegates.glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height); }
    public static void glCopyTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) { Delegates.glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height); }
    public static void glCopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) { Delegates.glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height); }
    public static void glCreateBuffers(int n, uint[] buffers) { Delegates.glCreateBuffers(n, buffers); }
    public static void glCreateFramebuffers(int n, uint[] ids) { Delegates.glCreateFramebuffers(n, ids); }
    public static uint glCreateProgram() { return Delegates.glCreateProgram(); }
    public static void glCreateProgramPipelines(int n, uint[] pipelines) { Delegates.glCreateProgramPipelines(n, pipelines); }
    public static void glCreateQueries(QueryTarget target, int n, uint[] ids) { Delegates.glCreateQueries(target, n, ids); }
    public static void glCreateRenderbuffers(int n, uint[] renderbuffers) { Delegates.glCreateRenderbuffers(n, renderbuffers); }
    public static void glCreateSamplers(int n, uint[] samplers) { Delegates.glCreateSamplers(n, samplers); }
    public static uint glCreateShader(ShaderType shaderType) { return Delegates.glCreateShader(shaderType); }
    public static uint glCreateShaderProgramv(ShaderType type, int count, string strings) { return Delegates.glCreateShaderProgramv(type, count, strings); }
    public static void glCreateTextures(TextureTarget target, int n, uint[] textures) { Delegates.glCreateTextures(target, n, textures); }
    public static void glCreateTransformFeedbacks(int n, uint[] ids) { Delegates.glCreateTransformFeedbacks(n, ids); }
    public static void glCreateVertexArrays(int n, uint[] arrays) { Delegates.glCreateVertexArrays(n, arrays); }
    public static void glCullFace(CullFaceMode mode) { Delegates.glCullFace(mode); }
    public static void glDeleteBuffers(int n, uint[] buffers) { Delegates.glDeleteBuffers(n, buffers); }
    public static void glDeleteBuffer(uint buffer)
    {
        uintArray[0] = buffer;
        Delegates.glDeleteBuffers(1, uintArray);
    }
    public static void glDeleteFramebuffers(int n, uint[] framebuffers) { Delegates.glDeleteFramebuffers(n, framebuffers); }
    public static void glDeleteFramebuffer(uint framebuffer)
    {
        uintArray[0] = framebuffer;
        Delegates.glDeleteFramebuffers(1, uintArray);
    }
    public static void glDeleteProgram(uint program) { Delegates.glDeleteProgram(program); }
    public static void glDeleteProgramPipelines(int n, uint[] pipelines) { Delegates.glDeleteProgramPipelines(n, pipelines); }
    public static void glDeleteQueries(int n, uint[] ids) { Delegates.glDeleteQueries(n, ids); }
    public static void glDeleteRenderbuffers(int n, uint[] renderbuffers) { Delegates.glDeleteRenderbuffers(n, renderbuffers); }
    public static void glDeleteRenderbuffer(uint renderbuffer)
    {
        uintArray[0] = renderbuffer;
        Delegates.glDeleteRenderbuffers(1, uintArray);
    }
    public static void glDeleteSamplers(int n, uint[] samplers) { Delegates.glDeleteSamplers(n, samplers); }
    public static void glDeleteShader(uint shader) { Delegates.glDeleteShader(shader); }
    public static void glDeleteSync(IntPtr sync) { Delegates.glDeleteSync(sync); }
    public static void glDeleteTextures(int n, uint[] textures) { Delegates.glDeleteTextures(n, textures); }
    public static void glDeleteTexture(uint texture)
    {
        uintArray[0] = texture;
        Delegates.glDeleteTextures(1, uintArray);
    }
    public static void glDeleteTransformFeedbacks(int n, uint[] ids) { Delegates.glDeleteTransformFeedbacks(n, ids); }
    public static void glDeleteVertexArrays(int n, uint[] arrays) { Delegates.glDeleteVertexArrays(n, arrays); }
    public static void glDeleteVertexArray(uint array)
    {
        uintArray[0] = array;
        Delegates.glDeleteVertexArrays(1, uintArray);
    }
    public static void glDepthFunc(DepthFunction func) { Delegates.glDepthFunc(func); }
    public static void glDepthMask(bool flag) { Delegates.glDepthMask(flag); }
    public static void glDepthRange(double nearVal, double farVal) { Delegates.glDepthRange(nearVal, farVal); }
    public static void glDepthRangef(float nearVal, float farVal) { Delegates.glDepthRangef(nearVal, farVal); }
    public static void glDepthRangeArrayv(uint first, int count, double[] v) { Delegates.glDepthRangeArrayv(first, count, v); }
    public static void glDepthRangeIndexed(uint index, double nearVal, double farVal) { Delegates.glDepthRangeIndexed(index, nearVal, farVal); }
    public static void glDetachShader(uint program, uint shader) { Delegates.glDetachShader(program, shader); }
    public static void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z) { Delegates.glDispatchCompute(num_groups_x, num_groups_y, num_groups_z); }
    public static void glDispatchComputeIndirect(IntPtr indirect) { Delegates.glDispatchComputeIndirect(indirect); }
    public static void glDrawArrays(BeginMode mode, int first, int count) { Delegates.glDrawArrays(mode, first, count); }
    public static void glDrawArraysIndirect(BeginMode mode, IntPtr indirect) { Delegates.glDrawArraysIndirect(mode, indirect); }
    public static void glDrawArraysInstanced(BeginMode mode, int first, int count, int primcount) { Delegates.glDrawArraysInstanced(mode, first, count, primcount); }
    public static void glDrawArraysInstancedBaseInstance(BeginMode mode, int first, int count, int primcount, uint baseinstance) { Delegates.glDrawArraysInstancedBaseInstance(mode, first, count, primcount, baseinstance); }
    public static void glDrawBuffer(DrawBufferMode buf) { Delegates.glDrawBuffer(buf); }
    public static void glNamedFramebufferDrawBuffer(uint framebuffer, DrawBufferMode buf) { Delegates.glNamedFramebufferDrawBuffer(framebuffer, buf); }
    public static void glDrawBuffers(int n, DrawBuffersEnum[] bufs) { Delegates.glDrawBuffers(n, bufs); }
    public static void glNamedFramebufferDrawBuffers(uint framebuffer, int n, DrawBufferMode[] bufs) { Delegates.glNamedFramebufferDrawBuffers(framebuffer, n, bufs); }
    public static void glDrawElements(BeginMode mode, int count, DrawElementsType type, IntPtr indices) { Delegates.glDrawElements(mode, count, type, indices); }
    public static void glDrawElementsBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int basevertex) { Delegates.glDrawElementsBaseVertex(mode, count, type, indices, basevertex); }
    public static void glDrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect) { Delegates.glDrawElementsIndirect(mode, type, indirect); }
    public static void glDrawElementsInstanced(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount) { Delegates.glDrawElementsInstanced(mode, count, type, indices, primcount); }
    public static void glDrawElementsInstancedBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, uint baseinstance) { Delegates.glDrawElementsInstancedBaseInstance(mode, count, type, indices, primcount, baseinstance); }
    public static void glDrawElementsInstancedBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex) { Delegates.glDrawElementsInstancedBaseVertex(mode, count, type, indices, primcount, basevertex); }
    public static void glDrawElementsInstancedBaseVertexBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex, uint baseinstance) { Delegates.glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices, primcount, basevertex, baseinstance); }
    public static void glDrawRangeElements(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices) { Delegates.glDrawRangeElements(mode, start, end, count, type, indices); }
    public static void glDrawRangeElementsBaseVertex(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices, int basevertex) { Delegates.glDrawRangeElementsBaseVertex(mode, start, end, count, type, indices, basevertex); }
    public static void glDrawTransformFeedback(NvTransformFeedback2 mode, uint id) { Delegates.glDrawTransformFeedback(mode, id); }
    public static void glDrawTransformFeedbackInstanced(BeginMode mode, uint id, int primcount) { Delegates.glDrawTransformFeedbackInstanced(mode, id, primcount); }
    public static void glDrawTransformFeedbackStream(NvTransformFeedback2 mode, uint id, uint stream) { Delegates.glDrawTransformFeedbackStream(mode, id, stream); }
    public static void glDrawTransformFeedbackStreamInstanced(BeginMode mode, uint id, uint stream, int primcount) { Delegates.glDrawTransformFeedbackStreamInstanced(mode, id, stream, primcount); }
    public static void glEnable(EnableCap cap) { Delegates.glEnable(cap); }
    public static void glDisable(EnableCap cap) { Delegates.glDisable(cap); }
    public static void glEnablei(EnableCap cap, uint index) { Delegates.glEnablei(cap, index); }
    public static void glDisablei(EnableCap cap, uint index) { Delegates.glDisablei(cap, index); }
    public static void glEnableVertexAttribArray(uint index) { Delegates.glEnableVertexAttribArray(index); }
    public static void glDisableVertexAttribArray(uint index) { Delegates.glDisableVertexAttribArray(index); }
    public static void glEnableVertexArrayAttrib(uint vaobj, uint index) { Delegates.glEnableVertexArrayAttrib(vaobj, index); }
    public static void glDisableVertexArrayAttrib(uint vaobj, uint index) { Delegates.glDisableVertexArrayAttrib(vaobj, index); }
    public static IntPtr glFenceSync(ArbSync condition, uint flags) { return Delegates.glFenceSync(condition, flags); }
    public static void glFinish() { Delegates.glFinish(); }
    public static void glFlush() { Delegates.glFlush(); }
    public static void glFlushMappedBufferRange(BufferTarget target, IntPtr offset, IntPtr length) { Delegates.glFlushMappedBufferRange(target, offset, length); }
    public static void glFlushMappedNamedBufferRange(uint buffer, IntPtr offset, int length) { Delegates.glFlushMappedNamedBufferRange(buffer, offset, length); }
    public static void glFramebufferParameteri(FramebufferTarget target, FramebufferPName pname, int param) { Delegates.glFramebufferParameteri(target, pname, param); }
    public static void glNamedFramebufferParameteri(uint framebuffer, FramebufferPName pname, int param) { Delegates.glNamedFramebufferParameteri(framebuffer, pname, param); }
    public static void glFramebufferRenderbuffer(FramebufferTarget target, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer) { Delegates.glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer); }
    public static void glNamedFramebufferRenderbuffer(uint framebuffer, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer) { Delegates.glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer); }
    public static void glFramebufferTexture(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level) { Delegates.glFramebufferTexture(target, attachment, texture, level); }
    public static void glFramebufferTexture1D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level) { Delegates.glFramebufferTexture1D(target, attachment, textarget, texture, level); }
    public static void glFramebufferTexture2D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level) { Delegates.glFramebufferTexture2D(target, attachment, textarget, texture, level); }
    public static void glFramebufferTexture3D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level, int layer) { Delegates.glFramebufferTexture3D(target, attachment, textarget, texture, level, layer); }
    public static void glNamedFramebufferTexture(uint framebuffer, FramebufferAttachment attachment, uint texture, int level) { Delegates.glNamedFramebufferTexture(framebuffer, attachment, texture, level); }
    public static void glFramebufferTextureLayer(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level, int layer) { Delegates.glFramebufferTextureLayer(target, attachment, texture, level, layer); }
    public static void glNamedFramebufferTextureLayer(uint framebuffer, FramebufferAttachment attachment, uint texture, int level, int layer) { Delegates.glNamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer); }
    public static void glFrontFace(FrontFaceDirection mode) { Delegates.glFrontFace(mode); }
    public static void glGenBuffers(int n, [Out] uint[] buffers) { Delegates.glGenBuffers(n, buffers); }
    public static uint glGenBuffer()
    {
        Delegates.glGenBuffers(1, uintArray);
        return uintArray[0];
    }
    public static void glGenerateMipmap(TextureTarget target) { Delegates.glGenerateMipmap(target); }
    public static void glGenerateTextureMipmap(uint texture) { Delegates.glGenerateTextureMipmap(texture); }
    public static void glGenFramebuffers(int n, [Out] uint[] ids) { Delegates.glGenFramebuffers(n, ids); }
    public static uint glGenFramebuffer()
    {
        Delegates.glGenFramebuffers(1, uintArray);
        return uintArray[0];
    }
    public static void glGenProgramPipelines(int n, [Out] uint[] pipelines) { Delegates.glGenProgramPipelines(n, pipelines); }
    public static void glGenQueries(int n, [Out] uint[] ids) { Delegates.glGenQueries(n, ids); }
    public static void glGenRenderbuffers(int n, [Out] uint[] renderbuffers) { Delegates.glGenRenderbuffers(n, renderbuffers); }
    public static uint glGenRenderbuffer()
    {
        Delegates.glGenRenderbuffers(1, uintArray);
        return uintArray[0];
    }
    public static void glGenSamplers(int n, [Out] uint[] samplers) { Delegates.glGenSamplers(n, samplers); }
    public static void glGenTextures(int n, [Out] uint[] textures) { Delegates.glGenTextures(n, textures); }
    public static uint glGenTexture()
    {
        Delegates.glGenTextures(1, uintArray);
        return uintArray[0];
    }
    public static void glGenTransformFeedbacks(int n, [Out] uint[] ids) { Delegates.glGenTransformFeedbacks(n, ids); }
    public static void glGenVertexArrays(int n, [Out] uint[] arrays) { Delegates.glGenVertexArrays(n, arrays); }
    public static uint glGenVertexArray()
    {
        Delegates.glGenVertexArrays(1, uintArray);
        return uintArray[0];
    }
    public static void glGetboolv(GetPName pname, [Out] bool[] data) { Delegates.glGetboolv(pname, data); }
    public static void glGetdoublev(GetPName pname, [Out] double[] data) { Delegates.glGetdoublev(pname, data); }
    public static void glGetFloatv(GetPName pname, [Out] float[] data) { Delegates.glGetFloatv(pname, data); }
    public static void glGetIntegerv(GetPName pname, [Out] int[] data) { Delegates.glGetIntegerv(pname, data); }
    public static int glGetIntegerv(GetPName pname)
    {
        Delegates.glGetIntegerv(pname, intArray);
        return intArray[0];
    }
    public static void glGetInteger64v(ArbSync pname, [Out] long[] data) { Delegates.glGetInteger64v(pname, data); }
    public static void glGetbooli_v(GetPName target, uint index, [Out] bool[] data) { Delegates.glGetbooli_v(target, index, data); }
    public static void glGetIntegeri_v(GetPName target, uint index, [Out] int[] data) { Delegates.glGetIntegeri_v(target, index, data); }
    public static void glGetFloati_v(GetPName target, uint index, [Out] float[] data) { Delegates.glGetFloati_v(target, index, data); }
    public static void glGetdoublei_v(GetPName target, uint index, [Out] double[] data) { Delegates.glGetdoublei_v(target, index, data); }
    public static void glGetInteger64i_v(GetPName target, uint index, [Out] long[] data) { Delegates.glGetInteger64i_v(target, index, data); }
    public static void glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, AtomicCounterParameterName pname, [Out] int[] @params) { Delegates.glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, @params); }
    public static void glGetActiveAttrib(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] StringBuilder name) { Delegates.glGetActiveAttrib(program, index, bufSize, length, size, type, name); }
    public static void glGetActiveSubroutineName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] StringBuilder name) { Delegates.glGetActiveSubroutineName(program, shadertype, index, bufsize, length, name); }
    public static void glGetActiveSubroutineUniformiv(uint program, ShaderType shadertype, uint index, SubroutineParameterName pname, [Out] int[] values) { Delegates.glGetActiveSubroutineUniformiv(program, shadertype, index, pname, values); }
    public static void glGetActiveSubroutineUniformName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] StringBuilder name) { Delegates.glGetActiveSubroutineUniformName(program, shadertype, index, bufsize, length, name); }
    public static void glGetActiveUniform(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveUniformType[] type, [Out] StringBuilder name) { Delegates.glGetActiveUniform(program, index, bufSize, length, size, type, name); }
    public static void glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, ActiveUniformBlockParameter pname, [Out] int[] @params) { Delegates.glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, @params); }
    public static void glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, [Out] int[] length, [Out] StringBuilder uniformBlockName) { Delegates.glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, length, uniformBlockName); }
    public static void glGetActiveUniformName(uint program, uint uniformIndex, int bufSize, [Out] int[] length, [Out] StringBuilder uniformName) { Delegates.glGetActiveUniformName(program, uniformIndex, bufSize, length, uniformName); }
    public static void glGetActiveUniformsiv(uint program, int uniformCount, [Out] uint[] uniformIndices, ActiveUniformType pname, [Out] int[] @params) { Delegates.glGetActiveUniformsiv(program, uniformCount, uniformIndices, pname, @params); }
    public static void glGetAttachedShaders(uint program, int maxCount, [Out] int[] count, [Out] uint[] shaders) { Delegates.glGetAttachedShaders(program, maxCount, count, shaders); }
    public static int glGetAttribLocation(uint program, string name) { return Delegates.glGetAttribLocation(program, name); }
    public static void glGetBufferParameteriv(BufferTarget target, BufferParameterName value, [Out] int[] data) { Delegates.glGetBufferParameteriv(target, value, data); }
    public static void glGetBufferParameteri64v(BufferTarget target, BufferParameterName value, [Out] long[] data) { Delegates.glGetBufferParameteri64v(target, value, data); }
    public static void glGetNamedBufferParameteriv(uint buffer, BufferParameterName pname, [Out] int[] @params) { Delegates.glGetNamedBufferParameteriv(buffer, pname, @params); }
    public static void glGetNamedBufferParameteri64v(uint buffer, BufferParameterName pname, [Out] long[] @params) { Delegates.glGetNamedBufferParameteri64v(buffer, pname, @params); }
    public static void glGetBufferPointerv(BufferTarget target, BufferPointer pname, [Out] IntPtr @params) { Delegates.glGetBufferPointerv(target, pname, @params); }
    public static void glGetNamedBufferPointerv(uint buffer, BufferPointer pname, [Out] IntPtr @params) { Delegates.glGetNamedBufferPointerv(buffer, pname, @params); }
    public static void glGetBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, [Out] IntPtr data) { Delegates.glGetBufferSubData(target, offset, size, data); }
    public static void glGetNamedBufferSubData(uint buffer, IntPtr offset, int size, [Out] IntPtr data) { Delegates.glGetNamedBufferSubData(buffer, offset, size, data); }
    public static void glGetCompressedTexImage(TextureTarget target, int level, [Out] IntPtr pixels) { Delegates.glGetCompressedTexImage(target, level, pixels); }
    public static void glGetnCompressedTexImage(TextureTarget target, int level, int bufSize, [Out] IntPtr pixels) { Delegates.glGetnCompressedTexImage(target, level, bufSize, pixels); }
    public static void glGetCompressedTextureImage(uint texture, int level, int bufSize, [Out] IntPtr pixels) { Delegates.glGetCompressedTextureImage(texture, level, bufSize, pixels); }
    public static void glGetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, [Out] IntPtr pixels) { Delegates.glGetCompressedTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels); }
    public static ErrorCode glGetError() { return Delegates.glGetError(); }
    public static int glGetFragDataIndex(uint program, string name) { return Delegates.glGetFragDataIndex(program, name); }
    public static int glGetFragDataLocation(uint program, string name) { return Delegates.glGetFragDataLocation(program, name); }
    public static void glGetFramebufferAttachmentParameteriv(FramebufferTarget target, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params) { Delegates.glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params); }
    public static void glGetNamedFramebufferAttachmentParameteriv(uint framebuffer, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params) { Delegates.glGetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, @params); }
    public static void glGetFramebufferParameteriv(FramebufferTarget target, FramebufferPName pname, [Out] int[] @params) { Delegates.glGetFramebufferParameteriv(target, pname, @params); }
    public static void glGetNamedFramebufferParameteriv(uint framebuffer, FramebufferPName pname, [Out] int[] param) { Delegates.glGetNamedFramebufferParameteriv(framebuffer, pname, param); }
    public static GraphicResetStatus glGetGraphicsResetStatus() { return Delegates.glGetGraphicsResetStatus(); }
    public static void glGetInternalformativ(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] int[] @params) { Delegates.glGetInternalformativ(target, internalFormat, pname, bufSize, @params); }
    public static void glGetInternalformati64v(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] long[] @params) { Delegates.glGetInternalformati64v(target, internalFormat, pname, bufSize, @params); }
    public static void glGetMultisamplefv(GetMultisamplePName pname, uint index, [Out] float[] val) { Delegates.glGetMultisamplefv(pname, index, val); }
    public static void glGetObjectLabel(ObjectLabelEnum identifier, uint name, int bifSize, [Out] int[] length, [Out] StringBuilder label) { Delegates.glGetObjectLabel(identifier, name, bifSize, length, label); }
    public static void glGetObjectPtrLabel([Out] IntPtr ptr, int bifSize, [Out] int[] length, [Out] StringBuilder label) { Delegates.glGetObjectPtrLabel(ptr, bifSize, length, label); }
    public static void glGetPointerv(GetPointerParameter pname, [Out] IntPtr @params) { Delegates.glGetPointerv(pname, @params); }
    public static void glGetProgramiv(uint program, ProgramParameter pname, [Out] int[] @params) { Delegates.glGetProgramiv(program, pname, @params); }
    public static int glGetProgramiv(uint program, ProgramParameter pname)
    {
        Delegates.glGetProgramiv(program, pname, intArray);
        return intArray[0];
    }
    public static void glGetProgramBinary(uint program, int bufsize, [Out] int[] length, [Out] int[] binaryFormat, [Out] IntPtr binary) { Delegates.glGetProgramBinary(program, bufsize, length, binaryFormat, binary); }
    public static void glGetProgramInfoLog(uint program, int maxLength, [Out] int[] length, [Out] StringBuilder infoLog) { Delegates.glGetProgramInfoLog(program, maxLength, length, infoLog); }
    public static string glGetProgramInfoLog(uint program)
    {
        Delegates.glGetProgramiv(program, ProgramParameter.InfoLogLength, intArray);
        if (intArray[0] == 0)
        {
            return string.Empty;
        }
        else
        {
            StringBuilder sb = new StringBuilder(intArray[0] * 2);
            Delegates.glGetProgramInfoLog(program, sb.Capacity, intArray, sb);
            return sb.ToString();
        }
    }
    public static void glGetProgramInterfaceiv(uint program, ProgramInterface programInterface, ProgramInterfaceParameterName pname, [Out] int[] @params) { Delegates.glGetProgramInterfaceiv(program, programInterface, pname, @params); }
    public static void glGetProgramPipelineiv(uint pipeline, int pname, [Out] int[] @params) { Delegates.glGetProgramPipelineiv(pipeline, pname, @params); }
    public static void glGetProgramPipelineInfoLog(uint pipeline, int bufSize, [Out] int[] length, [Out] StringBuilder infoLog) { Delegates.glGetProgramPipelineInfoLog(pipeline, bufSize, length, infoLog); }
    public static void glGetProgramResourceiv(uint program, ProgramInterface programInterface, uint index, int propCount, [Out] ProgramResourceParameterName[] props, int bufSize, [Out] int[] length, [Out] int[] @params) { Delegates.glGetProgramResourceiv(program, programInterface, index, propCount, props, bufSize, length, @params); }
    public static uint glGetProgramResourceIndex(uint program, ProgramInterface programInterface, string name) { return Delegates.glGetProgramResourceIndex(program, programInterface, name); }
    public static int glGetProgramResourceLocation(uint program, ProgramInterface programInterface, string name) { return Delegates.glGetProgramResourceLocation(program, programInterface, name); }
    public static int glGetProgramResourceLocationIndex(uint program, ProgramInterface programInterface, string name) { return Delegates.glGetProgramResourceLocationIndex(program, programInterface, name); }
    public static void glGetProgramResourceName(uint program, ProgramInterface programInterface, uint index, int bufSize, [Out] int[] length, [Out] StringBuilder name) { Delegates.glGetProgramResourceName(program, programInterface, index, bufSize, length, name); }
    public static void glGetProgramStageiv(uint program, ShaderType shadertype, ProgramStageParameterName pname, [Out] int[] values) { Delegates.glGetProgramStageiv(program, shadertype, pname, values); }
    public static void glGetQueryIndexediv(QueryTarget target, uint index, GetQueryParam pname, [Out] int[] @params) { Delegates.glGetQueryIndexediv(target, index, pname, @params); }
    public static void glGetQueryiv(QueryTarget target, GetQueryParam pname, [Out] int[] @params) { Delegates.glGetQueryiv(target, pname, @params); }
    public static void glGetQueryObjectiv(uint id, GetQueryObjectParam pname, [Out] int[] @params) { Delegates.glGetQueryObjectiv(id, pname, @params); }
    public static void glGetQueryObjectuiv(uint id, GetQueryObjectParam pname, [Out] uint[] @params) { Delegates.glGetQueryObjectuiv(id, pname, @params); }
    public static void glGetQueryObjecti64v(uint id, GetQueryObjectParam pname, [Out] long[] @params) { Delegates.glGetQueryObjecti64v(id, pname, @params); }
    public static void glGetQueryObjectui64v(uint id, GetQueryObjectParam pname, [Out] ulong[] @params) { Delegates.glGetQueryObjectui64v(id, pname, @params); }
    public static void glGetRenderbufferParameteriv(RenderbufferTarget target, RenderbufferParameterName pname, [Out] int[] @params) { Delegates.glGetRenderbufferParameteriv(target, pname, @params); }
    public static void glGetNamedRenderbufferParameteriv(uint renderbuffer, RenderbufferParameterName pname, [Out] int[] @params) { Delegates.glGetNamedRenderbufferParameteriv(renderbuffer, pname, @params); }
    public static void glGetSamplerParameterfv(uint sampler, int pname, [Out] float[] @params) { Delegates.glGetSamplerParameterfv(sampler, pname, @params); }
    public static void glGetSamplerParameteriv(uint sampler, int pname, [Out] int[] @params) { Delegates.glGetSamplerParameteriv(sampler, pname, @params); }
    public static void glGetSamplerParameterIiv(uint sampler, TextureParameterName pname, [Out] int[] @params) { Delegates.glGetSamplerParameterIiv(sampler, pname, @params); }
    public static void glGetSamplerParameterIuiv(uint sampler, TextureParameterName pname, [Out] uint[] @params) { Delegates.glGetSamplerParameterIuiv(sampler, pname, @params); }
    public static void glGetShaderiv(uint shader, ShaderParameter pname, [Out] int[] @params) { Delegates.glGetShaderiv(shader, pname, @params); }
    public static int glGetShaderiv(uint shader, ShaderParameter pname)
    {
        Delegates.glGetShaderiv(shader, pname, intArray);
        return intArray[0];
    }
    public static void glGetShaderInfoLog(uint shader, int maxLength, [Out] int[] length, [Out] StringBuilder infoLog) { Delegates.glGetShaderInfoLog(shader, maxLength, length, infoLog); }
    public static string glGetShaderInfoLog(uint shader)
    {
        Delegates.glGetShaderiv(shader, ShaderParameter.InfoLogLength, intArray);
        if (intArray[0] == 0)
        {
            return string.Empty;
        }
        else
        {
            StringBuilder sb = new StringBuilder(intArray[0] * 2);
            Delegates.glGetShaderInfoLog(shader, sb.Capacity, intArray, sb);
            return sb.ToString();
        }
    }
    public static void glGetShaderPrecisionFormat(ShaderType shaderType, int precisionType, [Out] int[] range, [Out] int[] precision) { Delegates.glGetShaderPrecisionFormat(shaderType, precisionType, range, precision); }
    public static void glGetShaderSource(uint shader, int bufSize, [Out] int[] length, [Out] StringBuilder source) { Delegates.glGetShaderSource(shader, bufSize, length, source); }
    public static string glGetString(StringName name) { return Marshal.PtrToStringAnsi(Delegates.glGetString(name)); }
    public static string glGetStringi(StringName name, uint index) { return Marshal.PtrToStringAnsi(Delegates.glGetStringi(name, index)); }
    public static uint glGetSubroutineIndex(uint program, ShaderType shadertype, string name) { return Delegates.glGetSubroutineIndex(program, shadertype, name); }
    public static int glGetSubroutineUniformLocation(uint program, ShaderType shadertype, string name) { return Delegates.glGetSubroutineUniformLocation(program, shadertype, name); }
    public static void glGetSynciv(IntPtr sync, ArbSync pname, int bufSize, [Out] int[] length, [Out] int[] values) { Delegates.glGetSynciv(sync, pname, bufSize, length, values); }
    public static void glGetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, [Out] IntPtr pixels) { Delegates.glGetTexImage(target, level, format, type, pixels); }
    public static void glGetnTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels) { Delegates.glGetnTexImage(target, level, format, type, bufSize, pixels); }
    public static void glGetTextureImage(uint texture, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels) { Delegates.glGetTextureImage(texture, level, format, type, bufSize, pixels); }
    public static void glGetTexLevelParameterfv(GetPName target, int level, GetTextureLevelParameter pname, [Out] float[] @params) { Delegates.glGetTexLevelParameterfv(target, level, pname, @params); }
    public static void glGetTexLevelParameteriv(GetPName target, int level, GetTextureLevelParameter pname, [Out] int[] @params) { Delegates.glGetTexLevelParameteriv(target, level, pname, @params); }
    public static void glGetTextureLevelParameterfv(uint texture, int level, GetTextureLevelParameter pname, [Out] float[] @params) { Delegates.glGetTextureLevelParameterfv(texture, level, pname, @params); }
    public static void glGetTextureLevelParameteriv(uint texture, int level, GetTextureLevelParameter pname, [Out] int[] @params) { Delegates.glGetTextureLevelParameteriv(texture, level, pname, @params); }
    public static void glGetTexParameterfv(TextureTarget target, GetTextureParameter pname, [Out] float[] @params) { Delegates.glGetTexParameterfv(target, pname, @params); }
    public static void glGetTexParameteriv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTexParameteriv(target, pname, @params); }
    public static void glGetTexParameterIiv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTexParameterIiv(target, pname, @params); }
    public static void glGetTexParameterIuiv(TextureTarget target, GetTextureParameter pname, [Out] uint[] @params) { Delegates.glGetTexParameterIuiv(target, pname, @params); }
    public static void glGetTextureParameterfv(uint texture, GetTextureParameter pname, [Out] float[] @params) { Delegates.glGetTextureParameterfv(texture, pname, @params); }
    public static void glGetTextureParameteriv(uint texture, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTextureParameteriv(texture, pname, @params); }
    public static void glGetTextureParameterIiv(uint texture, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTextureParameterIiv(texture, pname, @params); }
    public static void glGetTextureParameterIuiv(uint texture, GetTextureParameter pname, [Out] uint[] @params) { Delegates.glGetTextureParameterIuiv(texture, pname, @params); }
    public static void glGetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels) { Delegates.glGetTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels); }
    public static void glGetTransformFeedbackiv(uint xfb, TransformFeedbackParameterName pname, [Out] int[] param) { Delegates.glGetTransformFeedbackiv(xfb, pname, param); }
    public static void glGetTransformFeedbacki_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] int[] param) { Delegates.glGetTransformFeedbacki_v(xfb, pname, index, param); }
    public static void glGetTransformFeedbacki64_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] long[] param) { Delegates.glGetTransformFeedbacki64_v(xfb, pname, index, param); }
    public static void glGetTransformFeedbackVarying(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] StringBuilder name) { Delegates.glGetTransformFeedbackVarying(program, index, bufSize, length, size, type, name); }
    public static void glGetUniformfv(uint program, int location, [Out] float[] @params) { Delegates.glGetUniformfv(program, location, @params); }
    public static void glGetUniformiv(uint program, int location, [Out] int[] @params) { Delegates.glGetUniformiv(program, location, @params); }
    public static void glGetUniformuiv(uint program, int location, [Out] uint[] @params) { Delegates.glGetUniformuiv(program, location, @params); }
    public static void glGetUniformdv(uint program, int location, [Out] double[] @params) { Delegates.glGetUniformdv(program, location, @params); }
    public static void glGetnUniformfv(uint program, int location, int bufSize, [Out] float[] @params) { Delegates.glGetnUniformfv(program, location, bufSize, @params); }
    public static void glGetnUniformiv(uint program, int location, int bufSize, [Out] int[] @params) { Delegates.glGetnUniformiv(program, location, bufSize, @params); }
    public static void glGetnUniformuiv(uint program, int location, int bufSize, [Out] uint[] @params) { Delegates.glGetnUniformuiv(program, location, bufSize, @params); }
    public static void glGetnUniformdv(uint program, int location, int bufSize, [Out] double[] @params) { Delegates.glGetnUniformdv(program, location, bufSize, @params); }
    public static uint glGetUniformBlockIndex(uint program, string uniformBlockName) { return Delegates.glGetUniformBlockIndex(program, uniformBlockName); }
    public static void glGetUniformIndices(uint program, int uniformCount, string uniformNames, [Out] uint[] uniformIndices) { Delegates.glGetUniformIndices(program, uniformCount, uniformNames, uniformIndices); }
    public static int glGetUniformLocation(uint program, string name) { return Delegates.glGetUniformLocation(program, name); }
    public static void glGetUniformSubroutineuiv(ShaderType shadertype, int location, [Out] uint[] values) { Delegates.glGetUniformSubroutineuiv(shadertype, location, values); }
    public static void glGetVertexArrayIndexed64iv(uint vaobj, uint index, VertexAttribParameter pname, [Out] long[] param) { Delegates.glGetVertexArrayIndexed64iv(vaobj, index, pname, param); }
    public static void glGetVertexArrayIndexediv(uint vaobj, uint index, VertexAttribParameter pname, [Out] int[] param) { Delegates.glGetVertexArrayIndexediv(vaobj, index, pname, param); }
    public static void glGetVertexArrayiv(uint vaobj, VertexAttribParameter pname, [Out] int[] param) { Delegates.glGetVertexArrayiv(vaobj, pname, param); }
    public static void glGetVertexAttribdv(uint index, VertexAttribParameter pname, [Out] double[] @params) { Delegates.glGetVertexAttribdv(index, pname, @params); }
    public static void glGetVertexAttribfv(uint index, VertexAttribParameter pname, [Out] float[] @params) { Delegates.glGetVertexAttribfv(index, pname, @params); }
    public static void glGetVertexAttribiv(uint index, VertexAttribParameter pname, [Out] int[] @params) { Delegates.glGetVertexAttribiv(index, pname, @params); }
    public static void glGetVertexAttribIiv(uint index, VertexAttribParameter pname, [Out] int[] @params) { Delegates.glGetVertexAttribIiv(index, pname, @params); }
    public static void glGetVertexAttribIuiv(uint index, VertexAttribParameter pname, [Out] uint[] @params) { Delegates.glGetVertexAttribIuiv(index, pname, @params); }
    public static void glGetVertexAttribLdv(uint index, VertexAttribParameter pname, [Out] double[] @params) { Delegates.glGetVertexAttribLdv(index, pname, @params); }
    public static void glGetVertexAttribPointerv(uint index, VertexAttribPointerParameter pname, [Out] IntPtr pointer) { Delegates.glGetVertexAttribPointerv(index, pname, pointer); }
    public static void glHint(HintTarget target, HintMode mode) { Delegates.glHint(target, mode); }
    public static void glInvalidateBufferData(uint buffer) { Delegates.glInvalidateBufferData(buffer); }
    public static void glInvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length) { Delegates.glInvalidateBufferSubData(buffer, offset, length); }
    public static void glInvalidateFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments) { Delegates.glInvalidateFramebuffer(target, numAttachments, attachments); }
    public static void glInvalidateNamedFramebufferData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments) { Delegates.glInvalidateNamedFramebufferData(framebuffer, numAttachments, attachments); }
    public static void glInvalidateSubFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height) { Delegates.glInvalidateSubFramebuffer(target, numAttachments, attachments, x, y, width, height); }
    public static void glInvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height) { Delegates.glInvalidateNamedFramebufferSubData(framebuffer, numAttachments, attachments, x, y, width, height); }
    public static void glInvalidateTexImage(uint texture, int level) { Delegates.glInvalidateTexImage(texture, level); }
    public static void glInvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth) { Delegates.glInvalidateTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth); }
    public static bool glIsBuffer(uint buffer) { return Delegates.glIsBuffer(buffer); }
    public static bool glIsEnabled(EnableCap cap) { return Delegates.glIsEnabled(cap); }
    public static bool glIsEnabledi(EnableCap cap, uint index) { return Delegates.glIsEnabledi(cap, index); }
    public static bool glIsFramebuffer(uint framebuffer) { return Delegates.glIsFramebuffer(framebuffer); }
    public static bool glIsProgram(uint program) { return Delegates.glIsProgram(program); }
    public static bool glIsProgramPipeline(uint pipeline) { return Delegates.glIsProgramPipeline(pipeline); }
    public static bool glIsQuery(uint id) { return Delegates.glIsQuery(id); }
    public static bool glIsRenderbuffer(uint renderbuffer) { return Delegates.glIsRenderbuffer(renderbuffer); }
    public static bool glIsSampler(uint id) { return Delegates.glIsSampler(id); }
    public static bool glIsShader(uint shader) { return Delegates.glIsShader(shader); }
    public static bool glIsSync(IntPtr sync) { return Delegates.glIsSync(sync); }
    public static bool glIsTexture(uint texture) { return Delegates.glIsTexture(texture); }
    public static bool glIsTransformFeedback(uint id) { return Delegates.glIsTransformFeedback(id); }
    public static bool glIsVertexArray(uint array) { return Delegates.glIsVertexArray(array); }
    public static void glLineWidth(float width) { Delegates.glLineWidth(width); }
    public static void glLinkProgram(uint program) { Delegates.glLinkProgram(program); }
    public static void glLogicOp(LogicOpEnum opcode) { Delegates.glLogicOp(opcode); }
    public static IntPtr glMapBuffer(BufferTarget target, BufferAccess access) { return Delegates.glMapBuffer(target, access); }
    public static IntPtr glMapNamedBuffer(uint buffer, BufferAccess access) { return Delegates.glMapNamedBuffer(buffer, access); }
    public static IntPtr glMapBufferRange(BufferTarget target, IntPtr offset, IntPtr length, BufferAccessMask access) { return Delegates.glMapBufferRange(target, offset, length, access); }
    public static IntPtr glMapNamedBufferRange(uint buffer, IntPtr offset, int length, uint access) { return Delegates.glMapNamedBufferRange(buffer, offset, length, access); }
    public static void glMemoryBarrier(uint barriers) { Delegates.glMemoryBarrier(barriers); }
    public static void glMemoryBarrierByRegion(uint barriers) { Delegates.glMemoryBarrierByRegion(barriers); }
    public static void glMinSampleShading(float value) { Delegates.glMinSampleShading(value); }
    public static void glMultiDrawArrays(BeginMode mode, int[] first, int[] count, int drawcount) { Delegates.glMultiDrawArrays(mode, first, count, drawcount); }
    public static void glMultiDrawArraysIndirect(BeginMode mode, IntPtr indirect, int drawcount, int stride) { Delegates.glMultiDrawArraysIndirect(mode, indirect, drawcount, stride); }
    public static void glMultiDrawElements(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount) { Delegates.glMultiDrawElements(mode, count, type, indices, drawcount); }
    public static void glMultiDrawElementsBaseVertex(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount, int[] basevertex) { Delegates.glMultiDrawElementsBaseVertex(mode, count, type, indices, drawcount, basevertex); }
    public static void glMultiDrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect, int drawcount, int stride) { Delegates.glMultiDrawElementsIndirect(mode, type, indirect, drawcount, stride); }
    public static void glObjectLabel(ObjectLabelEnum identifier, uint name, int length, string label) { Delegates.glObjectLabel(identifier, name, length, label); }
    public static void glObjectPtrLabel(IntPtr ptr, int length, string label) { Delegates.glObjectPtrLabel(ptr, length, label); }
    public static void glPatchParameteri(int pname, int value) { Delegates.glPatchParameteri(pname, value); }
    public static void glPatchParameterfv(int pname, float[] values) { Delegates.glPatchParameterfv(pname, values); }
    public static void glPixelStoref(PixelStoreParameter pname, float param) { Delegates.glPixelStoref(pname, param); }
    public static void glPixelStorei(PixelStoreParameter pname, int param) { Delegates.glPixelStorei(pname, param); }
    public static void glPointParameterf(PointParameterName pname, float param) { Delegates.glPointParameterf(pname, param); }
    public static void glPointParameteri(PointParameterName pname, int param) { Delegates.glPointParameteri(pname, param); }
    public static void glPointParameterfv(PointParameterName pname, float[] @params) { Delegates.glPointParameterfv(pname, @params); }
    public static void glPointParameteriv(PointParameterName pname, int[] @params) { Delegates.glPointParameteriv(pname, @params); }
    public static void glPointSize(float size) { Delegates.glPointSize(size); }
    public static void glPolygonMode(MaterialFace face, PolygonModeEnum mode) { Delegates.glPolygonMode(face, mode); }
    public static void glPolygonOffset(float factor, float units) { Delegates.glPolygonOffset(factor, units); }
    public static void glPrimitiveRestartIndex(uint index) { Delegates.glPrimitiveRestartIndex(index); }
    public static void glProgramBinary(uint program, int binaryFormat, IntPtr binary, int length) { Delegates.glProgramBinary(program, binaryFormat, binary, length); }
    public static void glProgramParameteri(uint program, Version32 pname, int value) { Delegates.glProgramParameteri(program, pname, value); }
    public static void glProgramUniform1f(uint program, int location, float v0) { Delegates.glProgramUniform1f(program, location, v0); }
    public static void glProgramUniform2f(uint program, int location, float v0, float v1) { Delegates.glProgramUniform2f(program, location, v0, v1); }
    public static void glProgramUniform3f(uint program, int location, float v0, float v1, float v2) { Delegates.glProgramUniform3f(program, location, v0, v1, v2); }
    public static void glProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3) { Delegates.glProgramUniform4f(program, location, v0, v1, v2, v3); }
    public static void glProgramUniform1i(uint program, int location, int v0) { Delegates.glProgramUniform1i(program, location, v0); }
    public static void glProgramUniform2i(uint program, int location, int v0, int v1) { Delegates.glProgramUniform2i(program, location, v0, v1); }
    public static void glProgramUniform3i(uint program, int location, int v0, int v1, int v2) { Delegates.glProgramUniform3i(program, location, v0, v1, v2); }
    public static void glProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3) { Delegates.glProgramUniform4i(program, location, v0, v1, v2, v3); }
    public static void glProgramUniform1ui(uint program, int location, uint v0) { Delegates.glProgramUniform1ui(program, location, v0); }
    public static void glProgramUniform2ui(uint program, int location, int v0, uint v1) { Delegates.glProgramUniform2ui(program, location, v0, v1); }
    public static void glProgramUniform3ui(uint program, int location, int v0, int v1, uint v2) { Delegates.glProgramUniform3ui(program, location, v0, v1, v2); }
    public static void glProgramUniform4ui(uint program, int location, int v0, int v1, int v2, uint v3) { Delegates.glProgramUniform4ui(program, location, v0, v1, v2, v3); }
    public static void glProgramUniform1fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform1fv(program, location, count, value); }
    public static void glProgramUniform2fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform2fv(program, location, count, value); }
    public static void glProgramUniform3fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform3fv(program, location, count, value); }
    public static void glProgramUniform4fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform4fv(program, location, count, value); }
    public static void glProgramUniform1iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform1iv(program, location, count, value); }
    public static void glProgramUniform2iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform2iv(program, location, count, value); }
    public static void glProgramUniform3iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform3iv(program, location, count, value); }
    public static void glProgramUniform4iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform4iv(program, location, count, value); }
    public static void glProgramUniform1uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform1uiv(program, location, count, value); }
    public static void glProgramUniform2uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform2uiv(program, location, count, value); }
    public static void glProgramUniform3uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform3uiv(program, location, count, value); }
    public static void glProgramUniform4uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform4uiv(program, location, count, value); }
    public static void glProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix2fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix3fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix4fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix2x3fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix3x2fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix2x4fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix4x2fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix3x4fv(program, location, count, transpose, value); }
    public static void glProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix4x3fv(program, location, count, transpose, value); }
    public static void glProvokingVertex(ProvokingVertexMode provokeMode) { Delegates.glProvokingVertex(provokeMode); }
    public static void glQueryCounter(uint id, int target) { Delegates.glQueryCounter(id, target); }
    public static void glReadBuffer(ReadBufferMode mode) { Delegates.glReadBuffer(mode); }
    public static void glNamedFramebufferReadBuffer(uint framebuffer, BeginMode mode) { Delegates.glNamedFramebufferReadBuffer(framebuffer, mode); }
    public static void glReadPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int[] data) { Delegates.glReadPixels(x, y, width, height, format, type, data); }
    public static void glReadnPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int bufSize, int[] data) { Delegates.glReadnPixels(x, y, width, height, format, type, bufSize, data); }
    public static void glRenderbufferStorage(RenderbufferTarget target, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glRenderbufferStorage(target, internalFormat, width, height); }
    public static void glNamedRenderbufferStorage(uint renderbuffer, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glNamedRenderbufferStorage(renderbuffer, internalFormat, width, height); }
    public static void glRenderbufferStorageMultisample(RenderbufferTarget target, int samples, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glRenderbufferStorageMultisample(target, samples, internalFormat, width, height); }
    public static void glNamedRenderbufferStorageMultisample(uint renderbuffer, int samples, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glNamedRenderbufferStorageMultisample(renderbuffer, samples, internalFormat, width, height); }
    public static void glSampleCoverage(float value, bool invert) { Delegates.glSampleCoverage(value, invert); }
    public static void glSampleMaski(uint maskNumber, uint mask) { Delegates.glSampleMaski(maskNumber, mask); }
    public static void glSamplerParameterf(uint sampler, int pname, float param) { Delegates.glSamplerParameterf(sampler, pname, param); }
    public static void glSamplerParameteri(uint sampler, int pname, int param) { Delegates.glSamplerParameteri(sampler, pname, param); }
    public static void glSamplerParameterfv(uint sampler, int pname, float[] @params) { Delegates.glSamplerParameterfv(sampler, pname, @params); }
    public static void glSamplerParameteriv(uint sampler, int pname, int[] @params) { Delegates.glSamplerParameteriv(sampler, pname, @params); }
    public static void glSamplerParameterIiv(uint sampler, TextureParameterName pname, int[] @params) { Delegates.glSamplerParameterIiv(sampler, pname, @params); }
    public static void glSamplerParameterIuiv(uint sampler, TextureParameterName pname, uint[] @params) { Delegates.glSamplerParameterIuiv(sampler, pname, @params); }
    public static void glScissor(int x, int y, int width, int height) { Delegates.glScissor(x, y, width, height); }
    public static void glScissorArrayv(uint first, int count, int[] v) { Delegates.glScissorArrayv(first, count, v); }
    public static void glScissorIndexed(uint index, int left, int bottom, int width, int height) { Delegates.glScissorIndexed(index, left, bottom, width, height); }
    public static void glScissorIndexedv(uint index, int[] v) { Delegates.glScissorIndexedv(index, v); }
    public static void glShaderBinary(int count, uint[] shaders, int binaryFormat, IntPtr binary, int length) { Delegates.glShaderBinary(count, shaders, binaryFormat, binary, length); }
    public static void glShaderSource(uint shader, int count, string[] @string, int[] length) { Delegates.glShaderSource(shader, count, @string, length); }
    public static void glShaderSource(uint shader, string src)
    {
        stringArray[0] = src;
        intArray[0] = src.Length;
        Delegates.glShaderSource(shader, 1, stringArray, intArray);
    }
    public static void glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) { Delegates.glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding); }
    public static void glStencilFunc(StencilFunction func, int @ref, uint mask) { Delegates.glStencilFunc(func, @ref, mask); }
    public static void glStencilFuncSeparate(StencilFace face, StencilFunction func, int @ref, uint mask) { Delegates.glStencilFuncSeparate(face, func, @ref, mask); }
    public static void glStencilMask(uint mask) { Delegates.glStencilMask(mask); }
    public static void glStencilMaskSeparate(StencilFace face, uint mask) { Delegates.glStencilMaskSeparate(face, mask); }
    public static void glStencilOp(StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass) { Delegates.glStencilOp(sfail, dpfail, dppass); }
    public static void glStencilOpSeparate(StencilFace face, StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass) { Delegates.glStencilOpSeparate(face, sfail, dpfail, dppass); }
    public static void glTexBuffer(TextureBufferTarget target, SizedInternalFormat internalFormat, uint buffer) { Delegates.glTexBuffer(target, internalFormat, buffer); }
    public static void glTextureBuffer(uint texture, SizedInternalFormat internalFormat, uint buffer) { Delegates.glTextureBuffer(texture, internalFormat, buffer); }
    public static void glTexBufferRange(BufferTarget target, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, IntPtr size) { Delegates.glTexBufferRange(target, internalFormat, buffer, offset, size); }
    public static void glTextureBufferRange(uint texture, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, int size) { Delegates.glTextureBufferRange(texture, internalFormat, buffer, offset, size); }
    public static void glTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, PixelFormat format, PixelType type, IntPtr data) { Delegates.glTexImage1D(target, level, internalFormat, width, border, format, type, data); }
    public static void glTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr data) { Delegates.glTexImage2D(target, level, internalFormat, width, height, border, format, type, data); }
    public static void glTexImage2DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, bool fixedsamplelocations) { Delegates.glTexImage2DMultisample(target, samples, internalFormat, width, height, fixedsamplelocations); }
    public static void glTexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, PixelFormat format, PixelType type, IntPtr data) { Delegates.glTexImage3D(target, level, internalFormat, width, height, depth, border, format, type, data); }
    public static void glTexImage3DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations) { Delegates.glTexImage3DMultisample(target, samples, internalFormat, width, height, depth, fixedsamplelocations); }
    public static void glTexParameterf(TextureTarget target, TextureParameterName pname, float param) { Delegates.glTexParameterf(target, pname, param); }
    public static void glTexParameteri(TextureTarget target, TextureParameterName pname, int param) { Delegates.glTexParameteri(target, pname, param); }
    public static void glTextureParameterf(uint texture, TextureParameter pname, float param) { Delegates.glTextureParameterf(texture, pname, param); }
    public static void glTextureParameteri(uint texture, TextureParameter pname, int param) { Delegates.glTextureParameteri(texture, pname, param); }
    public static void glTexParameterfv(TextureTarget target, TextureParameterName pname, float[] @params) { Delegates.glTexParameterfv(target, pname, @params); }
    public static void glTexParameteriv(TextureTarget target, TextureParameterName pname, int[] @params) { Delegates.glTexParameteriv(target, pname, @params); }
    public static void glTexParameterIiv(TextureTarget target, TextureParameterName pname, int[] @params) { Delegates.glTexParameterIiv(target, pname, @params); }
    public static void glTexParameterIuiv(TextureTarget target, TextureParameterName pname, uint[] @params) { Delegates.glTexParameterIuiv(target, pname, @params); }
    public static void glTextureParameterfv(uint texture, TextureParameter pname, float[] paramtexture) { Delegates.glTextureParameterfv(texture, pname, paramtexture); }
    public static void glTextureParameteriv(uint texture, TextureParameter pname, int[] param) { Delegates.glTextureParameteriv(texture, pname, param); }
    public static void glTextureParameterIiv(uint texture, TextureParameter pname, int[] @params) { Delegates.glTextureParameterIiv(texture, pname, @params); }
    public static void glTextureParameterIuiv(uint texture, TextureParameter pname, uint[] @params) { Delegates.glTextureParameterIuiv(texture, pname, @params); }
    public static void glTexStorage1D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width) { Delegates.glTexStorage1D(target, levels, internalFormat, width); }
    public static void glTextureStorage1D(uint texture, int levels, SizedInternalFormat internalFormat, int width) { Delegates.glTextureStorage1D(texture, levels, internalFormat, width); }
    public static void glTexStorage2D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height) { Delegates.glTexStorage2D(target, levels, internalFormat, width, height); }
    public static void glTextureStorage2D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height) { Delegates.glTextureStorage2D(texture, levels, internalFormat, width, height); }
    public static void glTexStorage2DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations) { Delegates.glTexStorage2DMultisample(target, samples, internalFormat, width, height, fixedsamplelocations); }
    public static void glTextureStorage2DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations) { Delegates.glTextureStorage2DMultisample(texture, samples, internalFormat, width, height, fixedsamplelocations); }
    public static void glTexStorage3D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height, int depth) { Delegates.glTexStorage3D(target, levels, internalFormat, width, height, depth); }
    public static void glTextureStorage3D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height, int depth) { Delegates.glTextureStorage3D(texture, levels, internalFormat, width, height, depth); }
    public static void glTexStorage3DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations) { Delegates.glTexStorage3DMultisample(target, samples, internalFormat, width, height, depth, fixedsamplelocations); }
    public static void glTextureStorage3DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations) { Delegates.glTextureStorage3DMultisample(texture, samples, internalFormat, width, height, depth, fixedsamplelocations); }
    public static void glTexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTexSubImage1D(target, level, xoffset, width, format, type, pixels); }
    public static void glTextureSubImage1D(uint texture, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTextureSubImage1D(texture, level, xoffset, width, format, type, pixels); }
    public static void glTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels); }
    public static void glTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels); }
    public static void glTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels); }
    public static void glTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels); }
    public static void glTextureBarrier() { Delegates.glTextureBarrier(); }
    public static void glTextureView(uint texture, TextureTarget target, uint origtexture, PixelInternalFormat internalFormat, uint minlevel, uint numlevels, uint minlayer, uint numlayers) { Delegates.glTextureView(texture, target, origtexture, internalFormat, minlevel, numlevels, minlayer, numlayers); }
    public static void glTransformFeedbackBufferBase(uint xfb, uint index, uint buffer) { Delegates.glTransformFeedbackBufferBase(xfb, index, buffer); }
    public static void glTransformFeedbackBufferRange(uint xfb, uint index, uint buffer, IntPtr offset, int size) { Delegates.glTransformFeedbackBufferRange(xfb, index, buffer, offset, size); }
    public static void glTransformFeedbackVaryings(uint program, int count, string[] varyings, TransformFeedbackMode bufferMode) { Delegates.glTransformFeedbackVaryings(program, count, varyings, bufferMode); }
    public static void glUniform1f(int location, float v0) { Delegates.glUniform1f(location, v0); }
    public static void glUniform2f(int location, float v0, float v1) { Delegates.glUniform2f(location, v0, v1); }
    public static void glUniform3f(int location, float v0, float v1, float v2) { Delegates.glUniform3f(location, v0, v1, v2); }
    public static void glUniform4f(int location, float v0, float v1, float v2, float v3) { Delegates.glUniform4f(location, v0, v1, v2, v3); }
    public static void glUniform1i(int location, int v0) { Delegates.glUniform1i(location, v0); }
    public static void glUniform2i(int location, int v0, int v1) { Delegates.glUniform2i(location, v0, v1); }
    public static void glUniform3i(int location, int v0, int v1, int v2) { Delegates.glUniform3i(location, v0, v1, v2); }
    public static void glUniform4i(int location, int v0, int v1, int v2, int v3) { Delegates.glUniform4i(location, v0, v1, v2, v3); }
    public static void glUniform1ui(int location, uint v0) { Delegates.glUniform1ui(location, v0); }
    public static void glUniform2ui(int location, uint v0, uint v1) { Delegates.glUniform2ui(location, v0, v1); }
    public static void glUniform3ui(int location, uint v0, uint v1, uint v2) { Delegates.glUniform3ui(location, v0, v1, v2); }
    public static void glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3) { Delegates.glUniform4ui(location, v0, v1, v2, v3); }
    public static void glUniform1fv(int location, int count, float* value) { Delegates.glUniform1fv(location, count, value); }
    public static void glUniform2fv(int location, int count, float* value) { Delegates.glUniform2fv(location, count, value); }
    public static void glUniform3fv(int location, int count, float* value) { Delegates.glUniform3fv(location, count, value); }
    public static void glUniform4fv(int location, int count, float* value) { Delegates.glUniform4fv(location, count, value); }
    public static void glUniform1iv(int location, int count, int* value) { Delegates.glUniform1iv(location, count, value); }
    public static void glUniform2iv(int location, int count, int* value) { Delegates.glUniform2iv(location, count, value); }
    public static void glUniform3iv(int location, int count, int* value) { Delegates.glUniform3iv(location, count, value); }
    public static void glUniform4iv(int location, int count, int* value) { Delegates.glUniform4iv(location, count, value); }
    public static void glUniform1uiv(int location, int count, uint* value) { Delegates.glUniform1uiv(location, count, value); }
    public static void glUniform2uiv(int location, int count, uint* value) { Delegates.glUniform2uiv(location, count, value); }
    public static void glUniform3uiv(int location, int count, uint* value) { Delegates.glUniform3uiv(location, count, value); }
    public static void glUniform4uiv(int location, int count, uint* value) { Delegates.glUniform4uiv(location, count, value); }
    public static void glUniformMatrix2fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix2fv(location, count, transpose, value); }
    public static void glUniformMatrix3fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix3fv(location, count, transpose, value); }
    public static void glUniformMatrix4fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix4fv(location, count, transpose, value); }
    public static void glUniformMatrix2x3fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix2x3fv(location, count, transpose, value); }
    public static void glUniformMatrix3x2fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix3x2fv(location, count, transpose, value); }
    public static void glUniformMatrix2x4fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix2x4fv(location, count, transpose, value); }
    public static void glUniformMatrix4x2fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix4x2fv(location, count, transpose, value); }
    public static void glUniformMatrix3x4fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix3x4fv(location, count, transpose, value); }
    public static void glUniformMatrix4x3fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix4x3fv(location, count, transpose, value); }
    public static void glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) { Delegates.glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding); }
    public static void glUniformSubroutinesuiv(ShaderType shadertype, int count, uint[] indices) { Delegates.glUniformSubroutinesuiv(shadertype, count, indices); }
    public static bool glUnmapBuffer(BufferTarget target) { return Delegates.glUnmapBuffer(target); }
    public static bool glUnmapNamedBuffer(uint buffer) { return Delegates.glUnmapNamedBuffer(buffer); }
    public static void glUseProgram(uint program) { Delegates.glUseProgram(program); }
    public static void glUseProgramStages(uint pipeline, uint stages, uint program) { Delegates.glUseProgramStages(pipeline, stages, program); }
    public static void glValidateProgram(uint program) { Delegates.glValidateProgram(program); }
    public static void glValidateProgramPipeline(uint pipeline) { Delegates.glValidateProgramPipeline(pipeline); }
    public static void glVertexArrayElementBuffer(uint vaobj, uint buffer) { Delegates.glVertexArrayElementBuffer(vaobj, buffer); }
    public static void glVertexAttrib1f(uint index, float v0) { Delegates.glVertexAttrib1f(index, v0); }
    public static void glVertexAttrib1s(uint index, short v0) { Delegates.glVertexAttrib1s(index, v0); }
    public static void glVertexAttrib1d(uint index, double v0) { Delegates.glVertexAttrib1d(index, v0); }
    public static void glVertexAttribI1i(uint index, int v0) { Delegates.glVertexAttribI1i(index, v0); }
    public static void glVertexAttribI1ui(uint index, uint v0) { Delegates.glVertexAttribI1ui(index, v0); }
    public static void glVertexAttrib2f(uint index, float v0, float v1) { Delegates.glVertexAttrib2f(index, v0, v1); }
    public static void glVertexAttrib2s(uint index, short v0, short v1) { Delegates.glVertexAttrib2s(index, v0, v1); }
    public static void glVertexAttrib2d(uint index, double v0, double v1) { Delegates.glVertexAttrib2d(index, v0, v1); }
    public static void glVertexAttribI2i(uint index, int v0, int v1) { Delegates.glVertexAttribI2i(index, v0, v1); }
    public static void glVertexAttribI2ui(uint index, uint v0, uint v1) { Delegates.glVertexAttribI2ui(index, v0, v1); }
    public static void glVertexAttrib3f(uint index, float v0, float v1, float v2) { Delegates.glVertexAttrib3f(index, v0, v1, v2); }
    public static void glVertexAttrib3s(uint index, short v0, short v1, short v2) { Delegates.glVertexAttrib3s(index, v0, v1, v2); }
    public static void glVertexAttrib3d(uint index, double v0, double v1, double v2) { Delegates.glVertexAttrib3d(index, v0, v1, v2); }
    public static void glVertexAttribI3i(uint index, int v0, int v1, int v2) { Delegates.glVertexAttribI3i(index, v0, v1, v2); }
    public static void glVertexAttribI3ui(uint index, uint v0, uint v1, uint v2) { Delegates.glVertexAttribI3ui(index, v0, v1, v2); }
    public static void glVertexAttrib4f(uint index, float v0, float v1, float v2, float v3) { Delegates.glVertexAttrib4f(index, v0, v1, v2, v3); }
    public static void glVertexAttrib4s(uint index, short v0, short v1, short v2, short v3) { Delegates.glVertexAttrib4s(index, v0, v1, v2, v3); }
    public static void glVertexAttrib4d(uint index, double v0, double v1, double v2, double v3) { Delegates.glVertexAttrib4d(index, v0, v1, v2, v3); }
    public static void glVertexAttrib4Nub(uint index, byte v0, byte v1, byte v2, byte v3) { Delegates.glVertexAttrib4Nub(index, v0, v1, v2, v3); }
    public static void glVertexAttribI4i(uint index, int v0, int v1, int v2, int v3) { Delegates.glVertexAttribI4i(index, v0, v1, v2, v3); }
    public static void glVertexAttribI4ui(uint index, uint v0, uint v1, uint v2, uint v3) { Delegates.glVertexAttribI4ui(index, v0, v1, v2, v3); }
    public static void glVertexAttribL1d(uint index, double v0) { Delegates.glVertexAttribL1d(index, v0); }
    public static void glVertexAttribL2d(uint index, double v0, double v1) { Delegates.glVertexAttribL2d(index, v0, v1); }
    public static void glVertexAttribL3d(uint index, double v0, double v1, double v2) { Delegates.glVertexAttribL3d(index, v0, v1, v2); }
    public static void glVertexAttribL4d(uint index, double v0, double v1, double v2, double v3) { Delegates.glVertexAttribL4d(index, v0, v1, v2, v3); }
    public static void glVertexAttrib1fv(uint index, float[] v) { Delegates.glVertexAttrib1fv(index, v); }
    public static void glVertexAttrib1sv(uint index, short[] v) { Delegates.glVertexAttrib1sv(index, v); }
    public static void glVertexAttrib1dv(uint index, double[] v) { Delegates.glVertexAttrib1dv(index, v); }
    public static void glVertexAttribI1iv(uint index, int[] v) { Delegates.glVertexAttribI1iv(index, v); }
    public static void glVertexAttribI1uiv(uint index, uint[] v) { Delegates.glVertexAttribI1uiv(index, v); }
    public static void glVertexAttrib2fv(uint index, float[] v) { Delegates.glVertexAttrib2fv(index, v); }
    public static void glVertexAttrib2sv(uint index, short[] v) { Delegates.glVertexAttrib2sv(index, v); }
    public static void glVertexAttrib2dv(uint index, double[] v) { Delegates.glVertexAttrib2dv(index, v); }
    public static void glVertexAttribI2iv(uint index, int[] v) { Delegates.glVertexAttribI2iv(index, v); }
    public static void glVertexAttribI2uiv(uint index, uint[] v) { Delegates.glVertexAttribI2uiv(index, v); }
    public static void glVertexAttrib3fv(uint index, float[] v) { Delegates.glVertexAttrib3fv(index, v); }
    public static void glVertexAttrib3sv(uint index, short[] v) { Delegates.glVertexAttrib3sv(index, v); }
    public static void glVertexAttrib3dv(uint index, double[] v) { Delegates.glVertexAttrib3dv(index, v); }
    public static void glVertexAttribI3iv(uint index, int[] v) { Delegates.glVertexAttribI3iv(index, v); }
    public static void glVertexAttribI3uiv(uint index, uint[] v) { Delegates.glVertexAttribI3uiv(index, v); }
    public static void glVertexAttrib4fv(uint index, float[] v) { Delegates.glVertexAttrib4fv(index, v); }
    public static void glVertexAttrib4sv(uint index, short[] v) { Delegates.glVertexAttrib4sv(index, v); }
    public static void glVertexAttrib4dv(uint index, double[] v) { Delegates.glVertexAttrib4dv(index, v); }
    public static void glVertexAttrib4iv(uint index, int[] v) { Delegates.glVertexAttrib4iv(index, v); }
    public static void glVertexAttrib4bv(uint index, sbyte[] v) { Delegates.glVertexAttrib4bv(index, v); }
    public static void glVertexAttrib4ubv(uint index, byte[] v) { Delegates.glVertexAttrib4ubv(index, v); }
    public static void glVertexAttrib4usv(uint index, ushort[] v) { Delegates.glVertexAttrib4usv(index, v); }
    public static void glVertexAttrib4uiv(uint index, uint[] v) { Delegates.glVertexAttrib4uiv(index, v); }
    public static void glVertexAttrib4Nbv(uint index, sbyte[] v) { Delegates.glVertexAttrib4Nbv(index, v); }
    public static void glVertexAttrib4Nsv(uint index, short[] v) { Delegates.glVertexAttrib4Nsv(index, v); }
    public static void glVertexAttrib4Niv(uint index, int[] v) { Delegates.glVertexAttrib4Niv(index, v); }
    public static void glVertexAttrib4Nubv(uint index, byte[] v) { Delegates.glVertexAttrib4Nubv(index, v); }
    public static void glVertexAttrib4Nusv(uint index, ushort[] v) { Delegates.glVertexAttrib4Nusv(index, v); }
    public static void glVertexAttrib4Nuiv(uint index, uint[] v) { Delegates.glVertexAttrib4Nuiv(index, v); }
    public static void glVertexAttribI4bv(uint index, sbyte[] v) { Delegates.glVertexAttribI4bv(index, v); }
    public static void glVertexAttribI4ubv(uint index, byte[] v) { Delegates.glVertexAttribI4ubv(index, v); }
    public static void glVertexAttribI4sv(uint index, short[] v) { Delegates.glVertexAttribI4sv(index, v); }
    public static void glVertexAttribI4usv(uint index, ushort[] v) { Delegates.glVertexAttribI4usv(index, v); }
    public static void glVertexAttribI4iv(uint index, int[] v) { Delegates.glVertexAttribI4iv(index, v); }
    public static void glVertexAttribI4uiv(uint index, uint[] v) { Delegates.glVertexAttribI4uiv(index, v); }
    public static void glVertexAttribL1dv(uint index, double[] v) { Delegates.glVertexAttribL1dv(index, v); }
    public static void glVertexAttribL2dv(uint index, double[] v) { Delegates.glVertexAttribL2dv(index, v); }
    public static void glVertexAttribL3dv(uint index, double[] v) { Delegates.glVertexAttribL3dv(index, v); }
    public static void glVertexAttribL4dv(uint index, double[] v) { Delegates.glVertexAttribL4dv(index, v); }
    public static void glVertexAttribP1ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP1ui(index, type, normalized, value); }
    public static void glVertexAttribP2ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP2ui(index, type, normalized, value); }
    public static void glVertexAttribP3ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP3ui(index, type, normalized, value); }
    public static void glVertexAttribP4ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP4ui(index, type, normalized, value); }
    public static void glVertexAttribBinding(uint attribindex, uint bindingindex) { Delegates.glVertexAttribBinding(attribindex, bindingindex); }
    public static void glVertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex) { Delegates.glVertexArrayAttribBinding(vaobj, attribindex, bindingindex); }
    public static void glVertexAttribDivisor(uint index, uint divisor) { Delegates.glVertexAttribDivisor(index, divisor); }
    public static void glVertexAttribFormat(uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset) { Delegates.glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset); }
    public static void glVertexAttribIFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexAttribIFormat(attribindex, size, type, relativeoffset); }
    public static void glVertexAttribLFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexAttribLFormat(attribindex, size, type, relativeoffset); }
    public static void glVertexArrayAttribFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset) { Delegates.glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativeoffset); }
    public static void glVertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativeoffset); }
    public static void glVertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativeoffset); }
    public static void glVertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, IntPtr pointer) { Delegates.glVertexAttribPointer(index, size, type, normalized, stride, pointer); }
    public static void glVertexAttribIPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer) { Delegates.glVertexAttribIPointer(index, size, type, stride, pointer); }
    public static void glVertexAttribLPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer) { Delegates.glVertexAttribLPointer(index, size, type, stride, pointer); }
    public static void glVertexBindingDivisor(uint bindingindex, uint divisor) { Delegates.glVertexBindingDivisor(bindingindex, divisor); }
    public static void glVertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor) { Delegates.glVertexArrayBindingDivisor(vaobj, bindingindex, divisor); }
    public static void glViewport(int x, int y, int width, int height) { Delegates.glViewport(x, y, width, height); }
    public static void glViewportArrayv(uint first, int count, float[] v) { Delegates.glViewportArrayv(first, count, v); }
    public static void glViewportIndexedf(uint index, float x, float y, float w, float h) { Delegates.glViewportIndexedf(index, x, y, w, h); }
    public static void glViewportIndexedfv(uint index, float[] v) { Delegates.glViewportIndexedfv(index, v); }
    public static void glWaitSync(IntPtr sync, uint flags, ulong timeout) { Delegates.glWaitSync(sync, flags, timeout); }
}

delegate IntPtr GetProcAddress(string proc);
