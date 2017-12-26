using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Engine
{
    unsafe static partial class OpenGL
    {
        static getProcAddress get_proc_address = null;
        static string[] str_arr = new string[1];
        static uint[] uint_arr = new uint[1];
        static int[] int_arr = new int[1];
        static bool init = false;

        public static void LoadFunctions()
        {
            if (init)
                return;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                get_proc_address = wglGetProcAddress;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                get_proc_address = glxGetProcAddress;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                get_proc_address = osxGetProcAddress;
            else
                return;

            var methods = NativeCalls.Methods;
            var delegates = typeof(Delegates).GetFields(BindingFlags.Static | BindingFlags.NonPublic);
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

                // if it is still null means extension is not available on target system
                if (r != null)
                {
                    m.SetValue(null, r);
                }
            }

            init = true;
        }

        internal static Delegate GetProcDelegate(string proc, Type type)
        {
            var address = get_proc_address(proc);
            if (address.ToInt64() < 3)
            {
                return null;
            }
            else
            {
                return Marshal.GetDelegateForFunctionPointer(address, type);
            }
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

        internal static void glActiveShaderProgram(uint pipeline, uint program) { Delegates.glActiveShaderProgram(pipeline, program); }
        internal static void glActiveTexture(TextureUnit texture) { Delegates.glActiveTexture(texture); }
        internal static void glAttachShader(uint program, uint shader) { Delegates.glAttachShader(program, shader); }
        internal static void glBeginConditionalRender(uint id, ConditionalRenderType mode) { Delegates.glBeginConditionalRender(id, mode); }
        internal static void glEndConditionalRender() { Delegates.glEndConditionalRender(); }
        internal static void glBeginQuery(QueryTarget target, uint id) { Delegates.glBeginQuery(target, id); }
        internal static void glEndQuery(QueryTarget target) { Delegates.glEndQuery(target); }
        internal static void glBeginQueryIndexed(uint target, uint index, uint id) { Delegates.glBeginQueryIndexed(target, index, id); }
        internal static void glEndQueryIndexed(QueryTarget target, uint index) { Delegates.glEndQueryIndexed(target, index); }
        internal static void glBeginTransformFeedback(BeginFeedbackMode primitiveMode) { Delegates.glBeginTransformFeedback(primitiveMode); }
        internal static void glEndTransformFeedback() { Delegates.glEndTransformFeedback(); }
        internal static void glBindAttribLocation(uint program, uint index, string name) { Delegates.glBindAttribLocation(program, index, name); }
        internal static void glBindBuffer(BufferTarget target, uint buffer) { Delegates.glBindBuffer(target, buffer); }
        internal static void glBindBufferBase(BufferTarget target, uint index, uint buffer) { Delegates.glBindBufferBase(target, index, buffer); }
        internal static void glBindBufferRange(BufferTarget target, uint index, uint buffer, IntPtr offset, IntPtr size) { Delegates.glBindBufferRange(target, index, buffer, offset, size); }
        internal static void glBindBuffersBase(BufferTarget target, uint first, int count, uint[] buffers) { Delegates.glBindBuffersBase(target, first, count, buffers); }
        internal static void glBindBuffersRange(BufferTarget target, uint first, int count, uint[] buffers, IntPtr[] offsets, IntPtr[] sizes) { Delegates.glBindBuffersRange(target, first, count, buffers, offsets, sizes); }
        internal static void glBindFragDataLocation(uint program, uint colorNumber, string name) { Delegates.glBindFragDataLocation(program, colorNumber, name); }
        internal static void glBindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name) { Delegates.glBindFragDataLocationIndexed(program, colorNumber, index, name); }
        internal static void glBindFramebuffer(FramebufferTarget target, uint framebuffer) { Delegates.glBindFramebuffer(target, framebuffer); }
        internal static void glBindImageTexture(uint unit, uint texture, int level, bool layered, int layer, BufferAccess access, PixelInternalFormat format) { Delegates.glBindImageTexture(unit, texture, level, layered, layer, access, format); }
        internal static void glBindImageTextures(uint first, int count, uint[] textures) { Delegates.glBindImageTextures(first, count, textures); }
        internal static void glBindProgramPipeline(uint pipeline) { Delegates.glBindProgramPipeline(pipeline); }
        internal static void glBindRenderbuffer(RenderbufferTarget target, uint renderbuffer) { Delegates.glBindRenderbuffer(target, renderbuffer); }
        internal static void glBindSampler(uint unit, uint sampler) { Delegates.glBindSampler(unit, sampler); }
        internal static void glBindSamplers(uint first, int count, uint[] samplers) { Delegates.glBindSamplers(first, count, samplers); }
        internal static void glBindTexture(TextureTarget target, uint texture) { Delegates.glBindTexture(target, texture); }
        internal static void glBindTextures(uint first, int count, uint[] textures) { Delegates.glBindTextures(first, count, textures); }
        internal static void glBindTextureUnit(uint unit, uint texture) { Delegates.glBindTextureUnit(unit, texture); }
        internal static void glBindTransformFeedback(NvTransformFeedback2 target, uint id) { Delegates.glBindTransformFeedback(target, id); }
        internal static void glBindVertexArray(uint array) { Delegates.glBindVertexArray(array); }
        internal static void glBindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, IntPtr stride) { Delegates.glBindVertexBuffer(bindingindex, buffer, offset, stride); }
        internal static void glVertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride) { Delegates.glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride); }
        internal static void glBindVertexBuffers(uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides) { Delegates.glBindVertexBuffers(first, count, buffers, offsets, strides); }
        internal static void glVertexArrayVertexBuffers(uint vaobj, uint first, int count, uint[] buffers, IntPtr[] offsets, int[] strides) { Delegates.glVertexArrayVertexBuffers(vaobj, first, count, buffers, offsets, strides); }
        internal static void glBlendColor(float red, float green, float blue, float alpha) { Delegates.glBlendColor(red, green, blue, alpha); }
        internal static void glBlendEquation(BlendEquationMode mode) { Delegates.glBlendEquation(mode); }
        internal static void glBlendEquationi(uint buf, BlendEquationMode mode) { Delegates.glBlendEquationi(buf, mode); }
        internal static void glBlendEquationSeparate(BlendEquationMode modeRGB, BlendEquationMode modeAlpha) { Delegates.glBlendEquationSeparate(modeRGB, modeAlpha); }
        internal static void glBlendEquationSeparatei(uint buf, BlendEquationMode modeRGB, BlendEquationMode modeAlpha) { Delegates.glBlendEquationSeparatei(buf, modeRGB, modeAlpha); }
        internal static void glBlendFunc(BlendingFactorSrc sfactor, BlendingFactorDest dfactor) { Delegates.glBlendFunc(sfactor, dfactor); }
        internal static void glBlendFunci(uint buf, BlendingFactorSrc sfactor, BlendingFactorDest dfactor) { Delegates.glBlendFunci(buf, sfactor, dfactor); }
        internal static void glBlendFuncSeparate(BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha) { Delegates.glBlendFuncSeparate(srcRGB, dstRGB, srcAlpha, dstAlpha); }
        internal static void glBlendFuncSeparatei(uint buf, BlendingFactorSrc srcRGB, BlendingFactorDest dstRGB, BlendingFactorSrc srcAlpha, BlendingFactorDest dstAlpha) { Delegates.glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha); }
        internal static void glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter) { Delegates.glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter); }
        internal static void glBlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter) { Delegates.glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter); }
        internal static void glBufferData(BufferTarget target, int size, IntPtr data, BufferUsageHint usage) { Delegates.glBufferData(target, size, data, usage); }
        internal static void glNamedBufferData(uint buffer, int size, IntPtr data, BufferUsageHint usage) { Delegates.glNamedBufferData(buffer, size, data, usage); }
        internal static void glBufferStorage(BufferTarget target, IntPtr size, IntPtr data, uint flags) { Delegates.glBufferStorage(target, size, data, flags); }
        internal static void glNamedBufferStorage(uint buffer, int size, IntPtr data, uint flags) { Delegates.glNamedBufferStorage(buffer, size, data, flags); }
        internal static void glBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, IntPtr data) { Delegates.glBufferSubData(target, offset, size, data); }
        internal static void glNamedBufferSubData(uint buffer, IntPtr offset, int size, IntPtr data) { Delegates.glNamedBufferSubData(buffer, offset, size, data); }
        internal static FramebufferErrorCode glCheckFramebufferStatus(FramebufferTarget target) { return Delegates.glCheckFramebufferStatus(target); }
        internal static FramebufferErrorCode glCheckNamedFramebufferStatus(uint framebuffer, FramebufferTarget target) { return Delegates.glCheckNamedFramebufferStatus(framebuffer, target); }
        internal static void glClampColor(ClampColorTarget target, ClampColorMode clamp) { Delegates.glClampColor(target, clamp); }
        internal static void glClear(ClearBufferMask mask) { Delegates.glClear(mask); }
        internal static void glClearBufferiv(ClearBuffer buffer, int drawbuffer, int[] value) { Delegates.glClearBufferiv(buffer, drawbuffer, value); }
        internal static void glClearBufferuiv(ClearBuffer buffer, int drawbuffer, uint[] value) { Delegates.glClearBufferuiv(buffer, drawbuffer, value); }
        internal static void glClearBufferfv(ClearBuffer buffer, int drawbuffer, float[] value) { Delegates.glClearBufferfv(buffer, drawbuffer, value); }
        internal static void glClearBufferfi(ClearBuffer buffer, int drawbuffer, float depth, int stencil) { Delegates.glClearBufferfi(buffer, drawbuffer, depth, stencil); }
        internal static void glClearNamedFramebufferiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, int[] value) { Delegates.glClearNamedFramebufferiv(framebuffer, buffer, drawbuffer, value); }
        internal static void glClearNamedFramebufferuiv(uint framebuffer, ClearBuffer buffer, int drawbuffer, uint[] value) { Delegates.glClearNamedFramebufferuiv(framebuffer, buffer, drawbuffer, value); }
        internal static void glClearNamedFramebufferfv(uint framebuffer, ClearBuffer buffer, int drawbuffer, float[] value) { Delegates.glClearNamedFramebufferfv(framebuffer, buffer, drawbuffer, value); }
        internal static void glClearNamedFramebufferfi(uint framebuffer, ClearBuffer buffer, float depth, int stencil) { Delegates.glClearNamedFramebufferfi(framebuffer, buffer, depth, stencil); }
        internal static void glClearBufferData(BufferTarget target, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearBufferData(target, internalFormat, format, type, data); }
        internal static void glClearNamedBufferData(uint buffer, SizedInternalFormat internalFormat, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearNamedBufferData(buffer, internalFormat, format, type, data); }
        internal static void glClearBufferSubData(BufferTarget target, SizedInternalFormat internalFormat, IntPtr offset, IntPtr size, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearBufferSubData(target, internalFormat, offset, size, format, type, data); }
        internal static void glClearNamedBufferSubData(uint buffer, SizedInternalFormat internalFormat, IntPtr offset, int size, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearNamedBufferSubData(buffer, internalFormat, offset, size, format, type, data); }
        internal static void glClearColor(float red, float green, float blue, float alpha) { Delegates.glClearColor(red, green, blue, alpha); }
        internal static void glClearDepth(double depth) { Delegates.glClearDepth(depth); }
        internal static void glClearDepthf(float depth) { Delegates.glClearDepthf(depth); }
        internal static void glClearStencil(int s) { Delegates.glClearStencil(s); }
        internal static void glClearTexImage(uint texture, int level, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearTexImage(texture, level, format, type, data); }
        internal static void glClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, PixelType type, IntPtr data) { Delegates.glClearTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data); }
        internal static ArbSync glClientWaitSync(IntPtr sync, uint flags, ulong timeout) { return Delegates.glClientWaitSync(sync, flags, timeout); }
        internal static void glClipControl(ClipControlOrigin origin, ClipControlDepth depth) { Delegates.glClipControl(origin, depth); }
        internal static void glColorMask(bool red, bool green, bool blue, bool alpha) { Delegates.glColorMask(red, green, blue, alpha); }
        internal static void glColorMaski(uint buf, bool red, bool green, bool blue, bool alpha) { Delegates.glColorMaski(buf, red, green, blue, alpha); }
        internal static void glCompileShader(uint shader) { Delegates.glCompileShader(shader); }
        internal static void glCompressedTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, int imageSize, IntPtr data) { Delegates.glCompressedTexImage1D(target, level, internalFormat, width, border, imageSize, data); }
        internal static void glCompressedTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, int imageSize, IntPtr data) { Delegates.glCompressedTexImage2D(target, level, internalFormat, width, height, border, imageSize, data); }
        internal static void glCompressedTexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data) { Delegates.glCompressedTexImage3D(target, level, internalFormat, width, height, depth, border, imageSize, data); }
        internal static void glCompressedTexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data); }
        internal static void glCompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, PixelInternalFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data); }
        internal static void glCompressedTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data); }
        internal static void glCompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelInternalFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data); }
        internal static void glCompressedTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data); }
        internal static void glCompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelInternalFormat format, int imageSize, IntPtr data) { Delegates.glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data); }
        internal static void glCopyBufferSubData(BufferTarget readTarget, BufferTarget writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size) { Delegates.glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size); }
        internal static void glCopyNamedBufferSubData(uint readBuffer, uint writeBuffer, IntPtr readOffset, IntPtr writeOffset, int size) { Delegates.glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size); }
        internal static void glCopyImageSubData(uint srcName, BufferTarget srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, BufferTarget dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) { Delegates.glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth); }
        internal static void glCopyTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int border) { Delegates.glCopyTexImage1D(target, level, internalFormat, x, y, width, border); }
        internal static void glCopyTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int x, int y, int width, int height, int border) { Delegates.glCopyTexImage2D(target, level, internalFormat, x, y, width, height, border); }
        internal static void glCopyTexSubImage1D(TextureTarget target, int level, int xoffset, int x, int y, int width) { Delegates.glCopyTexSubImage1D(target, level, xoffset, x, y, width); }
        internal static void glCopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) { Delegates.glCopyTextureSubImage1D(texture, level, xoffset, x, y, width); }
        internal static void glCopyTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height) { Delegates.glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height); }
        internal static void glCopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) { Delegates.glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height); }
        internal static void glCopyTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) { Delegates.glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height); }
        internal static void glCopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) { Delegates.glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height); }
        internal static void glCreateBuffers(int n, uint[] buffers) { Delegates.glCreateBuffers(n, buffers); }
        internal static void glCreateFramebuffers(int n, uint[] ids) { Delegates.glCreateFramebuffers(n, ids); }
        internal static uint glCreateProgram() { return Delegates.glCreateProgram(); }
        internal static void glCreateProgramPipelines(int n, uint[] pipelines) { Delegates.glCreateProgramPipelines(n, pipelines); }
        internal static void glCreateQueries(QueryTarget target, int n, uint[] ids) { Delegates.glCreateQueries(target, n, ids); }
        internal static void glCreateRenderbuffers(int n, uint[] renderbuffers) { Delegates.glCreateRenderbuffers(n, renderbuffers); }
        internal static void glCreateSamplers(int n, uint[] samplers) { Delegates.glCreateSamplers(n, samplers); }
        internal static uint glCreateShader(ShaderType shaderType) { return Delegates.glCreateShader(shaderType); }
        internal static uint glCreateShaderProgramv(ShaderType type, int count, string strings) { return Delegates.glCreateShaderProgramv(type, count, strings); }
        internal static void glCreateTextures(TextureTarget target, int n, uint[] textures) { Delegates.glCreateTextures(target, n, textures); }
        internal static void glCreateTransformFeedbacks(int n, uint[] ids) { Delegates.glCreateTransformFeedbacks(n, ids); }
        internal static void glCreateVertexArrays(int n, uint[] arrays) { Delegates.glCreateVertexArrays(n, arrays); }
        internal static void glCullFace(CullFaceMode mode) { Delegates.glCullFace(mode); }
        internal static void glDeleteBuffers(int n, uint[] buffers) { Delegates.glDeleteBuffers(n, buffers); }
        internal static void glDeleteBuffer(uint buffer)
        {
            uint_arr[0] = buffer;
            Delegates.glDeleteBuffers(1, uint_arr);
        }
        internal static void glDeleteFramebuffers(int n, uint[] framebuffers) { Delegates.glDeleteFramebuffers(n, framebuffers); }
        internal static void glDeleteFramebuffer(uint framebuffer)
        {
            uint_arr[0] = framebuffer;
            Delegates.glDeleteFramebuffers(1, uint_arr);
        }
        internal static void glDeleteProgram(uint program) { Delegates.glDeleteProgram(program); }
        internal static void glDeleteProgramPipelines(int n, uint[] pipelines) { Delegates.glDeleteProgramPipelines(n, pipelines); }
        internal static void glDeleteQueries(int n, uint[] ids) { Delegates.glDeleteQueries(n, ids); }
        internal static void glDeleteRenderbuffers(int n, uint[] renderbuffers) { Delegates.glDeleteRenderbuffers(n, renderbuffers); }
        internal static void glDeleteSamplers(int n, uint[] samplers) { Delegates.glDeleteSamplers(n, samplers); }
        internal static void glDeleteShader(uint shader) { Delegates.glDeleteShader(shader); }
        internal static void glDeleteSync(IntPtr sync) { Delegates.glDeleteSync(sync); }
        internal static void glDeleteTextures(int n, uint[] textures) { Delegates.glDeleteTextures(n, textures); }
        internal static void glDeleteTexture(uint texture)
        {
            uint_arr[0] = texture;
            Delegates.glDeleteTextures(1, uint_arr);
        }
        internal static void glDeleteTransformFeedbacks(int n, uint[] ids) { Delegates.glDeleteTransformFeedbacks(n, ids); }
        internal static void glDeleteVertexArrays(int n, uint[] arrays) { Delegates.glDeleteVertexArrays(n, arrays); }
        internal static void glDeleteVertexArray(uint array)
        {
            uint_arr[0] = array;
            Delegates.glDeleteVertexArrays(1, uint_arr);
        }
        internal static void glDepthFunc(DepthFunction func) { Delegates.glDepthFunc(func); }
        internal static void glDepthMask(bool flag) { Delegates.glDepthMask(flag); }
        internal static void glDepthRange(double nearVal, double farVal) { Delegates.glDepthRange(nearVal, farVal); }
        internal static void glDepthRangef(float nearVal, float farVal) { Delegates.glDepthRangef(nearVal, farVal); }
        internal static void glDepthRangeArrayv(uint first, int count, double[] v) { Delegates.glDepthRangeArrayv(first, count, v); }
        internal static void glDepthRangeIndexed(uint index, double nearVal, double farVal) { Delegates.glDepthRangeIndexed(index, nearVal, farVal); }
        internal static void glDetachShader(uint program, uint shader) { Delegates.glDetachShader(program, shader); }
        internal static void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z) { Delegates.glDispatchCompute(num_groups_x, num_groups_y, num_groups_z); }
        internal static void glDispatchComputeIndirect(IntPtr indirect) { Delegates.glDispatchComputeIndirect(indirect); }
        internal static void glDrawArrays(BeginMode mode, int first, int count) { Delegates.glDrawArrays(mode, first, count); }
        internal static void glDrawArraysIndirect(BeginMode mode, IntPtr indirect) { Delegates.glDrawArraysIndirect(mode, indirect); }
        internal static void glDrawArraysInstanced(BeginMode mode, int first, int count, int primcount) { Delegates.glDrawArraysInstanced(mode, first, count, primcount); }
        internal static void glDrawArraysInstancedBaseInstance(BeginMode mode, int first, int count, int primcount, uint baseinstance) { Delegates.glDrawArraysInstancedBaseInstance(mode, first, count, primcount, baseinstance); }
        internal static void glDrawBuffer(DrawBufferMode buf) { Delegates.glDrawBuffer(buf); }
        internal static void glNamedFramebufferDrawBuffer(uint framebuffer, DrawBufferMode buf) { Delegates.glNamedFramebufferDrawBuffer(framebuffer, buf); }
        internal static void glDrawBuffers(int n, DrawBuffersEnum[] bufs) { Delegates.glDrawBuffers(n, bufs); }
        internal static void glNamedFramebufferDrawBuffers(uint framebuffer, int n, DrawBufferMode[] bufs) { Delegates.glNamedFramebufferDrawBuffers(framebuffer, n, bufs); }
        internal static void glDrawElements(BeginMode mode, int count, DrawElementsType type, IntPtr indices) { Delegates.glDrawElements(mode, count, type, indices); }
        internal static void glDrawElementsBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int basevertex) { Delegates.glDrawElementsBaseVertex(mode, count, type, indices, basevertex); }
        internal static void glDrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect) { Delegates.glDrawElementsIndirect(mode, type, indirect); }
        internal static void glDrawElementsInstanced(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount) { Delegates.glDrawElementsInstanced(mode, count, type, indices, primcount); }
        internal static void glDrawElementsInstancedBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, uint baseinstance) { Delegates.glDrawElementsInstancedBaseInstance(mode, count, type, indices, primcount, baseinstance); }
        internal static void glDrawElementsInstancedBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex) { Delegates.glDrawElementsInstancedBaseVertex(mode, count, type, indices, primcount, basevertex); }
        internal static void glDrawElementsInstancedBaseVertexBaseInstance(BeginMode mode, int count, DrawElementsType type, IntPtr indices, int primcount, int basevertex, uint baseinstance) { Delegates.glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices, primcount, basevertex, baseinstance); }
        internal static void glDrawRangeElements(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices) { Delegates.glDrawRangeElements(mode, start, end, count, type, indices); }
        internal static void glDrawRangeElementsBaseVertex(BeginMode mode, uint start, uint end, int count, DrawElementsType type, IntPtr indices, int basevertex) { Delegates.glDrawRangeElementsBaseVertex(mode, start, end, count, type, indices, basevertex); }
        internal static void glDrawTransformFeedback(NvTransformFeedback2 mode, uint id) { Delegates.glDrawTransformFeedback(mode, id); }
        internal static void glDrawTransformFeedbackInstanced(BeginMode mode, uint id, int primcount) { Delegates.glDrawTransformFeedbackInstanced(mode, id, primcount); }
        internal static void glDrawTransformFeedbackStream(NvTransformFeedback2 mode, uint id, uint stream) { Delegates.glDrawTransformFeedbackStream(mode, id, stream); }
        internal static void glDrawTransformFeedbackStreamInstanced(BeginMode mode, uint id, uint stream, int primcount) { Delegates.glDrawTransformFeedbackStreamInstanced(mode, id, stream, primcount); }
        internal static void glEnable(EnableCap cap) { Delegates.glEnable(cap); }
        internal static void glDisable(EnableCap cap) { Delegates.glDisable(cap); }
        internal static void glEnablei(EnableCap cap, uint index) { Delegates.glEnablei(cap, index); }
        internal static void glDisablei(EnableCap cap, uint index) { Delegates.glDisablei(cap, index); }
        internal static void glEnableVertexAttribArray(uint index) { Delegates.glEnableVertexAttribArray(index); }
        internal static void glDisableVertexAttribArray(uint index) { Delegates.glDisableVertexAttribArray(index); }
        internal static void glEnableVertexArrayAttrib(uint vaobj, uint index) { Delegates.glEnableVertexArrayAttrib(vaobj, index); }
        internal static void glDisableVertexArrayAttrib(uint vaobj, uint index) { Delegates.glDisableVertexArrayAttrib(vaobj, index); }
        internal static IntPtr glFenceSync(ArbSync condition, uint flags) { return Delegates.glFenceSync(condition, flags); }
        internal static void glFinish() { Delegates.glFinish(); }
        internal static void glFlush() { Delegates.glFlush(); }
        internal static void glFlushMappedBufferRange(BufferTarget target, IntPtr offset, IntPtr length) { Delegates.glFlushMappedBufferRange(target, offset, length); }
        internal static void glFlushMappedNamedBufferRange(uint buffer, IntPtr offset, int length) { Delegates.glFlushMappedNamedBufferRange(buffer, offset, length); }
        internal static void glFramebufferParameteri(FramebufferTarget target, FramebufferPName pname, int param) { Delegates.glFramebufferParameteri(target, pname, param); }
        internal static void glNamedFramebufferParameteri(uint framebuffer, FramebufferPName pname, int param) { Delegates.glNamedFramebufferParameteri(framebuffer, pname, param); }
        internal static void glFramebufferRenderbuffer(FramebufferTarget target, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer) { Delegates.glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer); }
        internal static void glNamedFramebufferRenderbuffer(uint framebuffer, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer) { Delegates.glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer); }
        internal static void glFramebufferTexture(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level) { Delegates.glFramebufferTexture(target, attachment, texture, level); }
        internal static void glFramebufferTexture1D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level) { Delegates.glFramebufferTexture1D(target, attachment, textarget, texture, level); }
        internal static void glFramebufferTexture2D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level) { Delegates.glFramebufferTexture2D(target, attachment, textarget, texture, level); }
        internal static void glFramebufferTexture3D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level, int layer) { Delegates.glFramebufferTexture3D(target, attachment, textarget, texture, level, layer); }
        internal static void glNamedFramebufferTexture(uint framebuffer, FramebufferAttachment attachment, uint texture, int level) { Delegates.glNamedFramebufferTexture(framebuffer, attachment, texture, level); }
        internal static void glFramebufferTextureLayer(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level, int layer) { Delegates.glFramebufferTextureLayer(target, attachment, texture, level, layer); }
        internal static void glNamedFramebufferTextureLayer(uint framebuffer, FramebufferAttachment attachment, uint texture, int level, int layer) { Delegates.glNamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer); }
        internal static void glFrontFace(FrontFaceDirection mode) { Delegates.glFrontFace(mode); }
        internal static void glGenBuffers(int n, [Out] uint[] buffers) { Delegates.glGenBuffers(n, buffers); }
        internal static uint glGenBuffer()
        {
            Delegates.glGenBuffers(1, uint_arr);
            return uint_arr[0];
        }
        internal static void glGenerateMipmap(TextureTarget target) { Delegates.glGenerateMipmap(target); }
        internal static void glGenerateTextureMipmap(uint texture) { Delegates.glGenerateTextureMipmap(texture); }
        internal static void glGenFramebuffers(int n, [Out] uint[] ids) { Delegates.glGenFramebuffers(n, ids); }
        internal static uint glGenFramebuffer()
        {
            Delegates.glGenFramebuffers(1, uint_arr);
            return uint_arr[0];
        }
        internal static void glGenProgramPipelines(int n, [Out] uint[] pipelines) { Delegates.glGenProgramPipelines(n, pipelines); }
        internal static void glGenQueries(int n, [Out] uint[] ids) { Delegates.glGenQueries(n, ids); }
        internal static void glGenRenderbuffers(int n, [Out] uint[] renderbuffers) { Delegates.glGenRenderbuffers(n, renderbuffers); }
        internal static void glGenSamplers(int n, [Out] uint[] samplers) { Delegates.glGenSamplers(n, samplers); }
        internal static void glGenTextures(int n, [Out] uint[] textures) { Delegates.glGenTextures(n, textures); }
        internal static uint glGenTexture()
        {
            Delegates.glGenTextures(1, uint_arr);
            return uint_arr[0];
        }
        internal static void glGenTransformFeedbacks(int n, [Out] uint[] ids) { Delegates.glGenTransformFeedbacks(n, ids); }
        internal static void glGenVertexArrays(int n, [Out] uint[] arrays) { Delegates.glGenVertexArrays(n, arrays); }
        internal static uint glGenVertexArray()
        {
            Delegates.glGenVertexArrays(1, uint_arr);
            return uint_arr[0];
        }
        internal static void glGetboolv(GetPName pname, [Out] bool[] data) { Delegates.glGetboolv(pname, data); }
        internal static void glGetdoublev(GetPName pname, [Out] double[] data) { Delegates.glGetdoublev(pname, data); }
        internal static void glGetFloatv(GetPName pname, [Out] float[] data) { Delegates.glGetFloatv(pname, data); }
        internal static void glGetIntegerv(GetPName pname, [Out] int[] data) { Delegates.glGetIntegerv(pname, data); }
        internal static void glGetInteger64v(ArbSync pname, [Out] long[] data) { Delegates.glGetInteger64v(pname, data); }
        internal static void glGetbooli_v(GetPName target, uint index, [Out] bool[] data) { Delegates.glGetbooli_v(target, index, data); }
        internal static void glGetIntegeri_v(GetPName target, uint index, [Out] int[] data) { Delegates.glGetIntegeri_v(target, index, data); }
        internal static void glGetFloati_v(GetPName target, uint index, [Out] float[] data) { Delegates.glGetFloati_v(target, index, data); }
        internal static void glGetdoublei_v(GetPName target, uint index, [Out] double[] data) { Delegates.glGetdoublei_v(target, index, data); }
        internal static void glGetInteger64i_v(GetPName target, uint index, [Out] long[] data) { Delegates.glGetInteger64i_v(target, index, data); }
        internal static void glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, AtomicCounterParameterName pname, [Out] int[] @params) { Delegates.glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, @params); }
        internal static void glGetActiveAttrib(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] StringBuilder name) { Delegates.glGetActiveAttrib(program, index, bufSize, length, size, type, name); }
        internal static void glGetActiveSubroutineName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] StringBuilder name) { Delegates.glGetActiveSubroutineName(program, shadertype, index, bufsize, length, name); }
        internal static void glGetActiveSubroutineUniformiv(uint program, ShaderType shadertype, uint index, SubroutineParameterName pname, [Out] int[] values) { Delegates.glGetActiveSubroutineUniformiv(program, shadertype, index, pname, values); }
        internal static void glGetActiveSubroutineUniformName(uint program, ShaderType shadertype, uint index, int bufsize, [Out] int[] length, [Out] StringBuilder name) { Delegates.glGetActiveSubroutineUniformName(program, shadertype, index, bufsize, length, name); }
        internal static void glGetActiveUniform(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveUniformType[] type, [Out] StringBuilder name) { Delegates.glGetActiveUniform(program, index, bufSize, length, size, type, name); }
        internal static void glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, ActiveUniformBlockParameter pname, [Out] int[] @params) { Delegates.glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, @params); }
        internal static void glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, [Out] int[] length, [Out] StringBuilder uniformBlockName) { Delegates.glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, length, uniformBlockName); }
        internal static void glGetActiveUniformName(uint program, uint uniformIndex, int bufSize, [Out] int[] length, [Out] StringBuilder uniformName) { Delegates.glGetActiveUniformName(program, uniformIndex, bufSize, length, uniformName); }
        internal static void glGetActiveUniformsiv(uint program, int uniformCount, [Out] uint[] uniformIndices, ActiveUniformType pname, [Out] int[] @params) { Delegates.glGetActiveUniformsiv(program, uniformCount, uniformIndices, pname, @params); }
        internal static void glGetAttachedShaders(uint program, int maxCount, [Out] int[] count, [Out] uint[] shaders) { Delegates.glGetAttachedShaders(program, maxCount, count, shaders); }
        internal static int glGetAttribLocation(uint program, string name) { return Delegates.glGetAttribLocation(program, name); }
        internal static void glGetBufferParameteriv(BufferTarget target, BufferParameterName value, [Out] int[] data) { Delegates.glGetBufferParameteriv(target, value, data); }
        internal static void glGetBufferParameteri64v(BufferTarget target, BufferParameterName value, [Out] long[] data) { Delegates.glGetBufferParameteri64v(target, value, data); }
        internal static void glGetNamedBufferParameteriv(uint buffer, BufferParameterName pname, [Out] int[] @params) { Delegates.glGetNamedBufferParameteriv(buffer, pname, @params); }
        internal static void glGetNamedBufferParameteri64v(uint buffer, BufferParameterName pname, [Out] long[] @params) { Delegates.glGetNamedBufferParameteri64v(buffer, pname, @params); }
        internal static void glGetBufferPointerv(BufferTarget target, BufferPointer pname, [Out] IntPtr @params) { Delegates.glGetBufferPointerv(target, pname, @params); }
        internal static void glGetNamedBufferPointerv(uint buffer, BufferPointer pname, [Out] IntPtr @params) { Delegates.glGetNamedBufferPointerv(buffer, pname, @params); }
        internal static void glGetBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, [Out] IntPtr data) { Delegates.glGetBufferSubData(target, offset, size, data); }
        internal static void glGetNamedBufferSubData(uint buffer, IntPtr offset, int size, [Out] IntPtr data) { Delegates.glGetNamedBufferSubData(buffer, offset, size, data); }
        internal static void glGetCompressedTexImage(TextureTarget target, int level, [Out] IntPtr pixels) { Delegates.glGetCompressedTexImage(target, level, pixels); }
        internal static void glGetnCompressedTexImage(TextureTarget target, int level, int bufSize, [Out] IntPtr pixels) { Delegates.glGetnCompressedTexImage(target, level, bufSize, pixels); }
        internal static void glGetCompressedTextureImage(uint texture, int level, int bufSize, [Out] IntPtr pixels) { Delegates.glGetCompressedTextureImage(texture, level, bufSize, pixels); }
        internal static void glGetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, [Out] IntPtr pixels) { Delegates.glGetCompressedTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels); }
        internal static ErrorCode glGetError() { return Delegates.glGetError(); }
        internal static int glGetFragDataIndex(uint program, string name) { return Delegates.glGetFragDataIndex(program, name); }
        internal static int glGetFragDataLocation(uint program, string name) { return Delegates.glGetFragDataLocation(program, name); }
        internal static void glGetFramebufferAttachmentParameteriv(FramebufferTarget target, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params) { Delegates.glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params); }
        internal static void glGetNamedFramebufferAttachmentParameteriv(uint framebuffer, FramebufferAttachment attachment, FramebufferParameterName pname, [Out] int[] @params) { Delegates.glGetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, @params); }
        internal static void glGetFramebufferParameteriv(FramebufferTarget target, FramebufferPName pname, [Out] int[] @params) { Delegates.glGetFramebufferParameteriv(target, pname, @params); }
        internal static void glGetNamedFramebufferParameteriv(uint framebuffer, FramebufferPName pname, [Out] int[] param) { Delegates.glGetNamedFramebufferParameteriv(framebuffer, pname, param); }
        internal static GraphicResetStatus glGetGraphicsResetStatus() { return Delegates.glGetGraphicsResetStatus(); }
        internal static void glGetInternalformativ(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] int[] @params) { Delegates.glGetInternalformativ(target, internalFormat, pname, bufSize, @params); }
        internal static void glGetInternalformati64v(TextureTarget target, PixelInternalFormat internalFormat, GetPName pname, int bufSize, [Out] long[] @params) { Delegates.glGetInternalformati64v(target, internalFormat, pname, bufSize, @params); }
        internal static void glGetMultisamplefv(GetMultisamplePName pname, uint index, [Out] float[] val) { Delegates.glGetMultisamplefv(pname, index, val); }
        internal static void glGetObjectLabel(ObjectLabelEnum identifier, uint name, int bifSize, [Out] int[] length, [Out] StringBuilder label) { Delegates.glGetObjectLabel(identifier, name, bifSize, length, label); }
        internal static void glGetObjectPtrLabel([Out] IntPtr ptr, int bifSize, [Out] int[] length, [Out] StringBuilder label) { Delegates.glGetObjectPtrLabel(ptr, bifSize, length, label); }
        internal static void glGetPointerv(GetPointerParameter pname, [Out] IntPtr @params) { Delegates.glGetPointerv(pname, @params); }
        internal static void glGetProgramiv(uint program, ProgramParameter pname, [Out] int[] @params) { Delegates.glGetProgramiv(program, pname, @params); }
        internal static int glGetProgramiv(uint program, ProgramParameter pname)
        {
            Delegates.glGetProgramiv(program, pname, int_arr);
            return int_arr[0];
        }
        internal static void glGetProgramBinary(uint program, int bufsize, [Out] int[] length, [Out] int[] binaryFormat, [Out] IntPtr binary) { Delegates.glGetProgramBinary(program, bufsize, length, binaryFormat, binary); }
        internal static void glGetProgramInfoLog(uint program, int maxLength, [Out] int[] length, [Out] StringBuilder infoLog) { Delegates.glGetProgramInfoLog(program, maxLength, length, infoLog); }
        internal static string glGetProgramInfoLog(uint program)
        {
            Delegates.glGetProgramiv(program, ProgramParameter.InfoLogLength, int_arr);
            if (int_arr[0] == 0)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder sb = new StringBuilder(int_arr[0] * 2);
                Delegates.glGetProgramInfoLog(program, sb.Capacity, int_arr, sb);
                return sb.ToString();
            }
        }
        internal static void glGetProgramInterfaceiv(uint program, ProgramInterface programInterface, ProgramInterfaceParameterName pname, [Out] int[] @params) { Delegates.glGetProgramInterfaceiv(program, programInterface, pname, @params); }
        internal static void glGetProgramPipelineiv(uint pipeline, int pname, [Out] int[] @params) { Delegates.glGetProgramPipelineiv(pipeline, pname, @params); }
        internal static void glGetProgramPipelineInfoLog(uint pipeline, int bufSize, [Out] int[] length, [Out] StringBuilder infoLog) { Delegates.glGetProgramPipelineInfoLog(pipeline, bufSize, length, infoLog); }
        internal static void glGetProgramResourceiv(uint program, ProgramInterface programInterface, uint index, int propCount, [Out] ProgramResourceParameterName[] props, int bufSize, [Out] int[] length, [Out] int[] @params) { Delegates.glGetProgramResourceiv(program, programInterface, index, propCount, props, bufSize, length, @params); }
        internal static uint glGetProgramResourceIndex(uint program, ProgramInterface programInterface, string name) { return Delegates.glGetProgramResourceIndex(program, programInterface, name); }
        internal static int glGetProgramResourceLocation(uint program, ProgramInterface programInterface, string name) { return Delegates.glGetProgramResourceLocation(program, programInterface, name); }
        internal static int glGetProgramResourceLocationIndex(uint program, ProgramInterface programInterface, string name) { return Delegates.glGetProgramResourceLocationIndex(program, programInterface, name); }
        internal static void glGetProgramResourceName(uint program, ProgramInterface programInterface, uint index, int bufSize, [Out] int[] length, [Out] StringBuilder name) { Delegates.glGetProgramResourceName(program, programInterface, index, bufSize, length, name); }
        internal static void glGetProgramStageiv(uint program, ShaderType shadertype, ProgramStageParameterName pname, [Out] int[] values) { Delegates.glGetProgramStageiv(program, shadertype, pname, values); }
        internal static void glGetQueryIndexediv(QueryTarget target, uint index, GetQueryParam pname, [Out] int[] @params) { Delegates.glGetQueryIndexediv(target, index, pname, @params); }
        internal static void glGetQueryiv(QueryTarget target, GetQueryParam pname, [Out] int[] @params) { Delegates.glGetQueryiv(target, pname, @params); }
        internal static void glGetQueryObjectiv(uint id, GetQueryObjectParam pname, [Out] int[] @params) { Delegates.glGetQueryObjectiv(id, pname, @params); }
        internal static void glGetQueryObjectuiv(uint id, GetQueryObjectParam pname, [Out] uint[] @params) { Delegates.glGetQueryObjectuiv(id, pname, @params); }
        internal static void glGetQueryObjecti64v(uint id, GetQueryObjectParam pname, [Out] long[] @params) { Delegates.glGetQueryObjecti64v(id, pname, @params); }
        internal static void glGetQueryObjectui64v(uint id, GetQueryObjectParam pname, [Out] ulong[] @params) { Delegates.glGetQueryObjectui64v(id, pname, @params); }
        internal static void glGetRenderbufferParameteriv(RenderbufferTarget target, RenderbufferParameterName pname, [Out] int[] @params) { Delegates.glGetRenderbufferParameteriv(target, pname, @params); }
        internal static void glGetNamedRenderbufferParameteriv(uint renderbuffer, RenderbufferParameterName pname, [Out] int[] @params) { Delegates.glGetNamedRenderbufferParameteriv(renderbuffer, pname, @params); }
        internal static void glGetSamplerParameterfv(uint sampler, int pname, [Out] float[] @params) { Delegates.glGetSamplerParameterfv(sampler, pname, @params); }
        internal static void glGetSamplerParameteriv(uint sampler, int pname, [Out] int[] @params) { Delegates.glGetSamplerParameteriv(sampler, pname, @params); }
        internal static void glGetSamplerParameterIiv(uint sampler, TextureParameterName pname, [Out] int[] @params) { Delegates.glGetSamplerParameterIiv(sampler, pname, @params); }
        internal static void glGetSamplerParameterIuiv(uint sampler, TextureParameterName pname, [Out] uint[] @params) { Delegates.glGetSamplerParameterIuiv(sampler, pname, @params); }
        internal static void glGetShaderiv(uint shader, ShaderParameter pname, [Out] int[] @params) { Delegates.glGetShaderiv(shader, pname, @params); }
        internal static int glGetShaderiv(uint shader, ShaderParameter pname)
        {
            Delegates.glGetShaderiv(shader, pname, int_arr);
            return int_arr[0];
        }
        internal static void glGetShaderInfoLog(uint shader, int maxLength, [Out] int[] length, [Out] StringBuilder infoLog) { Delegates.glGetShaderInfoLog(shader, maxLength, length, infoLog); }
        internal static string glGetShaderInfoLog(uint shader)
        {
            Delegates.glGetShaderiv(shader, ShaderParameter.InfoLogLength, int_arr);
            if (int_arr[0] == 0)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder sb = new StringBuilder(int_arr[0] * 2);
                Delegates.glGetShaderInfoLog(shader, sb.Capacity, int_arr, sb);
                return sb.ToString();
            }
        }
        internal static void glGetShaderPrecisionFormat(ShaderType shaderType, int precisionType, [Out] int[] range, [Out] int[] precision) { Delegates.glGetShaderPrecisionFormat(shaderType, precisionType, range, precision); }
        internal static void glGetShaderSource(uint shader, int bufSize, [Out] int[] length, [Out] StringBuilder source) { Delegates.glGetShaderSource(shader, bufSize, length, source); }
        internal static string glGetString(StringName name) { return Marshal.PtrToStringAnsi(Delegates.glGetString(name)); }
        internal static string glGetStringi(StringName name, uint index) { return Marshal.PtrToStringAnsi(Delegates.glGetStringi(name, index)); }
        internal static uint glGetSubroutineIndex(uint program, ShaderType shadertype, string name) { return Delegates.glGetSubroutineIndex(program, shadertype, name); }
        internal static int glGetSubroutineUniformLocation(uint program, ShaderType shadertype, string name) { return Delegates.glGetSubroutineUniformLocation(program, shadertype, name); }
        internal static void glGetSynciv(IntPtr sync, ArbSync pname, int bufSize, [Out] int[] length, [Out] int[] values) { Delegates.glGetSynciv(sync, pname, bufSize, length, values); }
        internal static void glGetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, [Out] IntPtr pixels) { Delegates.glGetTexImage(target, level, format, type, pixels); }
        internal static void glGetnTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels) { Delegates.glGetnTexImage(target, level, format, type, bufSize, pixels); }
        internal static void glGetTextureImage(uint texture, int level, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels) { Delegates.glGetTextureImage(texture, level, format, type, bufSize, pixels); }
        internal static void glGetTexLevelParameterfv(GetPName target, int level, GetTextureLevelParameter pname, [Out] float[] @params) { Delegates.glGetTexLevelParameterfv(target, level, pname, @params); }
        internal static void glGetTexLevelParameteriv(GetPName target, int level, GetTextureLevelParameter pname, [Out] int[] @params) { Delegates.glGetTexLevelParameteriv(target, level, pname, @params); }
        internal static void glGetTextureLevelParameterfv(uint texture, int level, GetTextureLevelParameter pname, [Out] float[] @params) { Delegates.glGetTextureLevelParameterfv(texture, level, pname, @params); }
        internal static void glGetTextureLevelParameteriv(uint texture, int level, GetTextureLevelParameter pname, [Out] int[] @params) { Delegates.glGetTextureLevelParameteriv(texture, level, pname, @params); }
        internal static void glGetTexParameterfv(TextureTarget target, GetTextureParameter pname, [Out] float[] @params) { Delegates.glGetTexParameterfv(target, pname, @params); }
        internal static void glGetTexParameteriv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTexParameteriv(target, pname, @params); }
        internal static void glGetTexParameterIiv(TextureTarget target, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTexParameterIiv(target, pname, @params); }
        internal static void glGetTexParameterIuiv(TextureTarget target, GetTextureParameter pname, [Out] uint[] @params) { Delegates.glGetTexParameterIuiv(target, pname, @params); }
        internal static void glGetTextureParameterfv(uint texture, GetTextureParameter pname, [Out] float[] @params) { Delegates.glGetTextureParameterfv(texture, pname, @params); }
        internal static void glGetTextureParameteriv(uint texture, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTextureParameteriv(texture, pname, @params); }
        internal static void glGetTextureParameterIiv(uint texture, GetTextureParameter pname, [Out] int[] @params) { Delegates.glGetTextureParameterIiv(texture, pname, @params); }
        internal static void glGetTextureParameterIuiv(uint texture, GetTextureParameter pname, [Out] uint[] @params) { Delegates.glGetTextureParameterIuiv(texture, pname, @params); }
        internal static void glGetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, int bufSize, [Out] IntPtr pixels) { Delegates.glGetTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels); }
        internal static void glGetTransformFeedbackiv(uint xfb, TransformFeedbackParameterName pname, [Out] int[] param) { Delegates.glGetTransformFeedbackiv(xfb, pname, param); }
        internal static void glGetTransformFeedbacki_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] int[] param) { Delegates.glGetTransformFeedbacki_v(xfb, pname, index, param); }
        internal static void glGetTransformFeedbacki64_v(uint xfb, TransformFeedbackParameterName pname, uint index, [Out] long[] param) { Delegates.glGetTransformFeedbacki64_v(xfb, pname, index, param); }
        internal static void glGetTransformFeedbackVarying(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size, [Out] ActiveAttribType[] type, [Out] StringBuilder name) { Delegates.glGetTransformFeedbackVarying(program, index, bufSize, length, size, type, name); }
        internal static void glGetUniformfv(uint program, int location, [Out] float[] @params) { Delegates.glGetUniformfv(program, location, @params); }
        internal static void glGetUniformiv(uint program, int location, [Out] int[] @params) { Delegates.glGetUniformiv(program, location, @params); }
        internal static void glGetUniformuiv(uint program, int location, [Out] uint[] @params) { Delegates.glGetUniformuiv(program, location, @params); }
        internal static void glGetUniformdv(uint program, int location, [Out] double[] @params) { Delegates.glGetUniformdv(program, location, @params); }
        internal static void glGetnUniformfv(uint program, int location, int bufSize, [Out] float[] @params) { Delegates.glGetnUniformfv(program, location, bufSize, @params); }
        internal static void glGetnUniformiv(uint program, int location, int bufSize, [Out] int[] @params) { Delegates.glGetnUniformiv(program, location, bufSize, @params); }
        internal static void glGetnUniformuiv(uint program, int location, int bufSize, [Out] uint[] @params) { Delegates.glGetnUniformuiv(program, location, bufSize, @params); }
        internal static void glGetnUniformdv(uint program, int location, int bufSize, [Out] double[] @params) { Delegates.glGetnUniformdv(program, location, bufSize, @params); }
        internal static uint glGetUniformBlockIndex(uint program, string uniformBlockName) { return Delegates.glGetUniformBlockIndex(program, uniformBlockName); }
        internal static void glGetUniformIndices(uint program, int uniformCount, string uniformNames, [Out] uint[] uniformIndices) { Delegates.glGetUniformIndices(program, uniformCount, uniformNames, uniformIndices); }
        internal static int glGetUniformLocation(uint program, string name) { return Delegates.glGetUniformLocation(program, name); }
        internal static void glGetUniformSubroutineuiv(ShaderType shadertype, int location, [Out] uint[] values) { Delegates.glGetUniformSubroutineuiv(shadertype, location, values); }
        internal static void glGetVertexArrayIndexed64iv(uint vaobj, uint index, VertexAttribParameter pname, [Out] long[] param) { Delegates.glGetVertexArrayIndexed64iv(vaobj, index, pname, param); }
        internal static void glGetVertexArrayIndexediv(uint vaobj, uint index, VertexAttribParameter pname, [Out] int[] param) { Delegates.glGetVertexArrayIndexediv(vaobj, index, pname, param); }
        internal static void glGetVertexArrayiv(uint vaobj, VertexAttribParameter pname, [Out] int[] param) { Delegates.glGetVertexArrayiv(vaobj, pname, param); }
        internal static void glGetVertexAttribdv(uint index, VertexAttribParameter pname, [Out] double[] @params) { Delegates.glGetVertexAttribdv(index, pname, @params); }
        internal static void glGetVertexAttribfv(uint index, VertexAttribParameter pname, [Out] float[] @params) { Delegates.glGetVertexAttribfv(index, pname, @params); }
        internal static void glGetVertexAttribiv(uint index, VertexAttribParameter pname, [Out] int[] @params) { Delegates.glGetVertexAttribiv(index, pname, @params); }
        internal static void glGetVertexAttribIiv(uint index, VertexAttribParameter pname, [Out] int[] @params) { Delegates.glGetVertexAttribIiv(index, pname, @params); }
        internal static void glGetVertexAttribIuiv(uint index, VertexAttribParameter pname, [Out] uint[] @params) { Delegates.glGetVertexAttribIuiv(index, pname, @params); }
        internal static void glGetVertexAttribLdv(uint index, VertexAttribParameter pname, [Out] double[] @params) { Delegates.glGetVertexAttribLdv(index, pname, @params); }
        internal static void glGetVertexAttribPointerv(uint index, VertexAttribPointerParameter pname, [Out] IntPtr pointer) { Delegates.glGetVertexAttribPointerv(index, pname, pointer); }
        internal static void glHint(HintTarget target, HintMode mode) { Delegates.glHint(target, mode); }
        internal static void glInvalidateBufferData(uint buffer) { Delegates.glInvalidateBufferData(buffer); }
        internal static void glInvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length) { Delegates.glInvalidateBufferSubData(buffer, offset, length); }
        internal static void glInvalidateFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments) { Delegates.glInvalidateFramebuffer(target, numAttachments, attachments); }
        internal static void glInvalidateNamedFramebufferData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments) { Delegates.glInvalidateNamedFramebufferData(framebuffer, numAttachments, attachments); }
        internal static void glInvalidateSubFramebuffer(FramebufferTarget target, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height) { Delegates.glInvalidateSubFramebuffer(target, numAttachments, attachments, x, y, width, height); }
        internal static void glInvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, FramebufferAttachment[] attachments, int x, int y, int width, int height) { Delegates.glInvalidateNamedFramebufferSubData(framebuffer, numAttachments, attachments, x, y, width, height); }
        internal static void glInvalidateTexImage(uint texture, int level) { Delegates.glInvalidateTexImage(texture, level); }
        internal static void glInvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth) { Delegates.glInvalidateTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth); }
        internal static bool glIsBuffer(uint buffer) { return Delegates.glIsBuffer(buffer); }
        internal static bool glIsEnabled(EnableCap cap) { return Delegates.glIsEnabled(cap); }
        internal static bool glIsEnabledi(EnableCap cap, uint index) { return Delegates.glIsEnabledi(cap, index); }
        internal static bool glIsFramebuffer(uint framebuffer) { return Delegates.glIsFramebuffer(framebuffer); }
        internal static bool glIsProgram(uint program) { return Delegates.glIsProgram(program); }
        internal static bool glIsProgramPipeline(uint pipeline) { return Delegates.glIsProgramPipeline(pipeline); }
        internal static bool glIsQuery(uint id) { return Delegates.glIsQuery(id); }
        internal static bool glIsRenderbuffer(uint renderbuffer) { return Delegates.glIsRenderbuffer(renderbuffer); }
        internal static bool glIsSampler(uint id) { return Delegates.glIsSampler(id); }
        internal static bool glIsShader(uint shader) { return Delegates.glIsShader(shader); }
        internal static bool glIsSync(IntPtr sync) { return Delegates.glIsSync(sync); }
        internal static bool glIsTexture(uint texture) { return Delegates.glIsTexture(texture); }
        internal static bool glIsTransformFeedback(uint id) { return Delegates.glIsTransformFeedback(id); }
        internal static bool glIsVertexArray(uint array) { return Delegates.glIsVertexArray(array); }
        internal static void glLineWidth(float width) { Delegates.glLineWidth(width); }
        internal static void glLinkProgram(uint program) { Delegates.glLinkProgram(program); }
        internal static void glLogicOp(LogicOpEnum opcode) { Delegates.glLogicOp(opcode); }
        internal static IntPtr glMapBuffer(BufferTarget target, BufferAccess access) { return Delegates.glMapBuffer(target, access); }
        internal static IntPtr glMapNamedBuffer(uint buffer, BufferAccess access) { return Delegates.glMapNamedBuffer(buffer, access); }
        internal static IntPtr glMapBufferRange(BufferTarget target, IntPtr offset, IntPtr length, BufferAccessMask access) { return Delegates.glMapBufferRange(target, offset, length, access); }
        internal static IntPtr glMapNamedBufferRange(uint buffer, IntPtr offset, int length, uint access) { return Delegates.glMapNamedBufferRange(buffer, offset, length, access); }
        internal static void glMemoryBarrier(uint barriers) { Delegates.glMemoryBarrier(barriers); }
        internal static void glMemoryBarrierByRegion(uint barriers) { Delegates.glMemoryBarrierByRegion(barriers); }
        internal static void glMinSampleShading(float value) { Delegates.glMinSampleShading(value); }
        internal static void glMultiDrawArrays(BeginMode mode, int[] first, int[] count, int drawcount) { Delegates.glMultiDrawArrays(mode, first, count, drawcount); }
        internal static void glMultiDrawArraysIndirect(BeginMode mode, IntPtr indirect, int drawcount, int stride) { Delegates.glMultiDrawArraysIndirect(mode, indirect, drawcount, stride); }
        internal static void glMultiDrawElements(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount) { Delegates.glMultiDrawElements(mode, count, type, indices, drawcount); }
        internal static void glMultiDrawElementsBaseVertex(BeginMode mode, int[] count, DrawElementsType type, IntPtr indices, int drawcount, int[] basevertex) { Delegates.glMultiDrawElementsBaseVertex(mode, count, type, indices, drawcount, basevertex); }
        internal static void glMultiDrawElementsIndirect(BeginMode mode, DrawElementsType type, IntPtr indirect, int drawcount, int stride) { Delegates.glMultiDrawElementsIndirect(mode, type, indirect, drawcount, stride); }
        internal static void glObjectLabel(ObjectLabelEnum identifier, uint name, int length, string label) { Delegates.glObjectLabel(identifier, name, length, label); }
        internal static void glObjectPtrLabel(IntPtr ptr, int length, string label) { Delegates.glObjectPtrLabel(ptr, length, label); }
        internal static void glPatchParameteri(int pname, int value) { Delegates.glPatchParameteri(pname, value); }
        internal static void glPatchParameterfv(int pname, float[] values) { Delegates.glPatchParameterfv(pname, values); }
        internal static void glPixelStoref(PixelStoreParameter pname, float param) { Delegates.glPixelStoref(pname, param); }
        internal static void glPixelStorei(PixelStoreParameter pname, int param) { Delegates.glPixelStorei(pname, param); }
        internal static void glPointParameterf(PointParameterName pname, float param) { Delegates.glPointParameterf(pname, param); }
        internal static void glPointParameteri(PointParameterName pname, int param) { Delegates.glPointParameteri(pname, param); }
        internal static void glPointParameterfv(PointParameterName pname, float[] @params) { Delegates.glPointParameterfv(pname, @params); }
        internal static void glPointParameteriv(PointParameterName pname, int[] @params) { Delegates.glPointParameteriv(pname, @params); }
        internal static void glPointSize(float size) { Delegates.glPointSize(size); }
        internal static void glPolygonMode(MaterialFace face, PolygonModeEnum mode) { Delegates.glPolygonMode(face, mode); }
        internal static void glPolygonOffset(float factor, float units) { Delegates.glPolygonOffset(factor, units); }
        internal static void glPrimitiveRestartIndex(uint index) { Delegates.glPrimitiveRestartIndex(index); }
        internal static void glProgramBinary(uint program, int binaryFormat, IntPtr binary, int length) { Delegates.glProgramBinary(program, binaryFormat, binary, length); }
        internal static void glProgramParameteri(uint program, Version32 pname, int value) { Delegates.glProgramParameteri(program, pname, value); }
        internal static void glProgramUniform1f(uint program, int location, float v0) { Delegates.glProgramUniform1f(program, location, v0); }
        internal static void glProgramUniform2f(uint program, int location, float v0, float v1) { Delegates.glProgramUniform2f(program, location, v0, v1); }
        internal static void glProgramUniform3f(uint program, int location, float v0, float v1, float v2) { Delegates.glProgramUniform3f(program, location, v0, v1, v2); }
        internal static void glProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3) { Delegates.glProgramUniform4f(program, location, v0, v1, v2, v3); }
        internal static void glProgramUniform1i(uint program, int location, int v0) { Delegates.glProgramUniform1i(program, location, v0); }
        internal static void glProgramUniform2i(uint program, int location, int v0, int v1) { Delegates.glProgramUniform2i(program, location, v0, v1); }
        internal static void glProgramUniform3i(uint program, int location, int v0, int v1, int v2) { Delegates.glProgramUniform3i(program, location, v0, v1, v2); }
        internal static void glProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3) { Delegates.glProgramUniform4i(program, location, v0, v1, v2, v3); }
        internal static void glProgramUniform1ui(uint program, int location, uint v0) { Delegates.glProgramUniform1ui(program, location, v0); }
        internal static void glProgramUniform2ui(uint program, int location, int v0, uint v1) { Delegates.glProgramUniform2ui(program, location, v0, v1); }
        internal static void glProgramUniform3ui(uint program, int location, int v0, int v1, uint v2) { Delegates.glProgramUniform3ui(program, location, v0, v1, v2); }
        internal static void glProgramUniform4ui(uint program, int location, int v0, int v1, int v2, uint v3) { Delegates.glProgramUniform4ui(program, location, v0, v1, v2, v3); }
        internal static void glProgramUniform1fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform1fv(program, location, count, value); }
        internal static void glProgramUniform2fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform2fv(program, location, count, value); }
        internal static void glProgramUniform3fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform3fv(program, location, count, value); }
        internal static void glProgramUniform4fv(uint program, int location, int count, float* value) { Delegates.glProgramUniform4fv(program, location, count, value); }
        internal static void glProgramUniform1iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform1iv(program, location, count, value); }
        internal static void glProgramUniform2iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform2iv(program, location, count, value); }
        internal static void glProgramUniform3iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform3iv(program, location, count, value); }
        internal static void glProgramUniform4iv(uint program, int location, int count, int* value) { Delegates.glProgramUniform4iv(program, location, count, value); }
        internal static void glProgramUniform1uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform1uiv(program, location, count, value); }
        internal static void glProgramUniform2uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform2uiv(program, location, count, value); }
        internal static void glProgramUniform3uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform3uiv(program, location, count, value); }
        internal static void glProgramUniform4uiv(uint program, int location, int count, uint* value) { Delegates.glProgramUniform4uiv(program, location, count, value); }
        internal static void glProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix2fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix3fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix4fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix2x3fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix3x2fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix2x4fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix4x2fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix3x4fv(program, location, count, transpose, value); }
        internal static void glProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, float* value) { Delegates.glProgramUniformMatrix4x3fv(program, location, count, transpose, value); }
        internal static void glProvokingVertex(ProvokingVertexMode provokeMode) { Delegates.glProvokingVertex(provokeMode); }
        internal static void glQueryCounter(uint id, int target) { Delegates.glQueryCounter(id, target); }
        internal static void glReadBuffer(ReadBufferMode mode) { Delegates.glReadBuffer(mode); }
        internal static void glNamedFramebufferReadBuffer(uint framebuffer, BeginMode mode) { Delegates.glNamedFramebufferReadBuffer(framebuffer, mode); }
        internal static void glReadPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int[] data) { Delegates.glReadPixels(x, y, width, height, format, type, data); }
        internal static void glReadnPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, int bufSize, int[] data) { Delegates.glReadnPixels(x, y, width, height, format, type, bufSize, data); }
        internal static void glRenderbufferStorage(RenderbufferTarget target, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glRenderbufferStorage(target, internalFormat, width, height); }
        internal static void glNamedRenderbufferStorage(uint renderbuffer, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glNamedRenderbufferStorage(renderbuffer, internalFormat, width, height); }
        internal static void glRenderbufferStorageMultisample(RenderbufferTarget target, int samples, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glRenderbufferStorageMultisample(target, samples, internalFormat, width, height); }
        internal static void glNamedRenderbufferStorageMultisample(uint renderbuffer, int samples, RenderbufferStorageEnum internalFormat, int width, int height) { Delegates.glNamedRenderbufferStorageMultisample(renderbuffer, samples, internalFormat, width, height); }
        internal static void glSampleCoverage(float value, bool invert) { Delegates.glSampleCoverage(value, invert); }
        internal static void glSampleMaski(uint maskNumber, uint mask) { Delegates.glSampleMaski(maskNumber, mask); }
        internal static void glSamplerParameterf(uint sampler, int pname, float param) { Delegates.glSamplerParameterf(sampler, pname, param); }
        internal static void glSamplerParameteri(uint sampler, int pname, int param) { Delegates.glSamplerParameteri(sampler, pname, param); }
        internal static void glSamplerParameterfv(uint sampler, int pname, float[] @params) { Delegates.glSamplerParameterfv(sampler, pname, @params); }
        internal static void glSamplerParameteriv(uint sampler, int pname, int[] @params) { Delegates.glSamplerParameteriv(sampler, pname, @params); }
        internal static void glSamplerParameterIiv(uint sampler, TextureParameterName pname, int[] @params) { Delegates.glSamplerParameterIiv(sampler, pname, @params); }
        internal static void glSamplerParameterIuiv(uint sampler, TextureParameterName pname, uint[] @params) { Delegates.glSamplerParameterIuiv(sampler, pname, @params); }
        internal static void glScissor(int x, int y, int width, int height) { Delegates.glScissor(x, y, width, height); }
        internal static void glScissorArrayv(uint first, int count, int[] v) { Delegates.glScissorArrayv(first, count, v); }
        internal static void glScissorIndexed(uint index, int left, int bottom, int width, int height) { Delegates.glScissorIndexed(index, left, bottom, width, height); }
        internal static void glScissorIndexedv(uint index, int[] v) { Delegates.glScissorIndexedv(index, v); }
        internal static void glShaderBinary(int count, uint[] shaders, int binaryFormat, IntPtr binary, int length) { Delegates.glShaderBinary(count, shaders, binaryFormat, binary, length); }
        internal static void glShaderSource(uint shader, int count, string[] @string, int[] length) { Delegates.glShaderSource(shader, count, @string, length); }
        internal static void glShaderSource(uint shader, string src)
        {
            str_arr[0] = src;
            int_arr[0] = src.Length;
            Delegates.glShaderSource(shader, 1, str_arr, int_arr);
        }
        internal static void glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) { Delegates.glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding); }
        internal static void glStencilFunc(StencilFunction func, int @ref, uint mask) { Delegates.glStencilFunc(func, @ref, mask); }
        internal static void glStencilFuncSeparate(StencilFace face, StencilFunction func, int @ref, uint mask) { Delegates.glStencilFuncSeparate(face, func, @ref, mask); }
        internal static void glStencilMask(uint mask) { Delegates.glStencilMask(mask); }
        internal static void glStencilMaskSeparate(StencilFace face, uint mask) { Delegates.glStencilMaskSeparate(face, mask); }
        internal static void glStencilOp(StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass) { Delegates.glStencilOp(sfail, dpfail, dppass); }
        internal static void glStencilOpSeparate(StencilFace face, StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass) { Delegates.glStencilOpSeparate(face, sfail, dpfail, dppass); }
        internal static void glTexBuffer(TextureBufferTarget target, SizedInternalFormat internalFormat, uint buffer) { Delegates.glTexBuffer(target, internalFormat, buffer); }
        internal static void glTextureBuffer(uint texture, SizedInternalFormat internalFormat, uint buffer) { Delegates.glTextureBuffer(texture, internalFormat, buffer); }
        internal static void glTexBufferRange(BufferTarget target, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, IntPtr size) { Delegates.glTexBufferRange(target, internalFormat, buffer, offset, size); }
        internal static void glTextureBufferRange(uint texture, SizedInternalFormat internalFormat, uint buffer, IntPtr offset, int size) { Delegates.glTextureBufferRange(texture, internalFormat, buffer, offset, size); }
        internal static void glTexImage1D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int border, PixelFormat format, PixelType type, IntPtr data) { Delegates.glTexImage1D(target, level, internalFormat, width, border, format, type, data); }
        internal static void glTexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr data) { Delegates.glTexImage2D(target, level, internalFormat, width, height, border, format, type, data); }
        internal static void glTexImage2DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, bool fixedsamplelocations) { Delegates.glTexImage2DMultisample(target, samples, internalFormat, width, height, fixedsamplelocations); }
        internal static void glTexImage3D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int depth, int border, PixelFormat format, PixelType type, IntPtr data) { Delegates.glTexImage3D(target, level, internalFormat, width, height, depth, border, format, type, data); }
        internal static void glTexImage3DMultisample(TextureTargetMultisample target, int samples, PixelInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations) { Delegates.glTexImage3DMultisample(target, samples, internalFormat, width, height, depth, fixedsamplelocations); }
        internal static void glTexParameterf(TextureTarget target, TextureParameterName pname, float param) { Delegates.glTexParameterf(target, pname, param); }
        internal static void glTexParameteri(TextureTarget target, TextureParameterName pname, int param) { Delegates.glTexParameteri(target, pname, param); }
        internal static void glTextureParameterf(uint texture, TextureParameter pname, float param) { Delegates.glTextureParameterf(texture, pname, param); }
        internal static void glTextureParameteri(uint texture, TextureParameter pname, int param) { Delegates.glTextureParameteri(texture, pname, param); }
        internal static void glTexParameterfv(TextureTarget target, TextureParameterName pname, float[] @params) { Delegates.glTexParameterfv(target, pname, @params); }
        internal static void glTexParameteriv(TextureTarget target, TextureParameterName pname, int[] @params) { Delegates.glTexParameteriv(target, pname, @params); }
        internal static void glTexParameterIiv(TextureTarget target, TextureParameterName pname, int[] @params) { Delegates.glTexParameterIiv(target, pname, @params); }
        internal static void glTexParameterIuiv(TextureTarget target, TextureParameterName pname, uint[] @params) { Delegates.glTexParameterIuiv(target, pname, @params); }
        internal static void glTextureParameterfv(uint texture, TextureParameter pname, float[] paramtexture) { Delegates.glTextureParameterfv(texture, pname, paramtexture); }
        internal static void glTextureParameteriv(uint texture, TextureParameter pname, int[] param) { Delegates.glTextureParameteriv(texture, pname, param); }
        internal static void glTextureParameterIiv(uint texture, TextureParameter pname, int[] @params) { Delegates.glTextureParameterIiv(texture, pname, @params); }
        internal static void glTextureParameterIuiv(uint texture, TextureParameter pname, uint[] @params) { Delegates.glTextureParameterIuiv(texture, pname, @params); }
        internal static void glTexStorage1D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width) { Delegates.glTexStorage1D(target, levels, internalFormat, width); }
        internal static void glTextureStorage1D(uint texture, int levels, SizedInternalFormat internalFormat, int width) { Delegates.glTextureStorage1D(texture, levels, internalFormat, width); }
        internal static void glTexStorage2D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height) { Delegates.glTexStorage2D(target, levels, internalFormat, width, height); }
        internal static void glTextureStorage2D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height) { Delegates.glTextureStorage2D(texture, levels, internalFormat, width, height); }
        internal static void glTexStorage2DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations) { Delegates.glTexStorage2DMultisample(target, samples, internalFormat, width, height, fixedsamplelocations); }
        internal static void glTextureStorage2DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, bool fixedsamplelocations) { Delegates.glTextureStorage2DMultisample(texture, samples, internalFormat, width, height, fixedsamplelocations); }
        internal static void glTexStorage3D(TextureTarget target, int levels, SizedInternalFormat internalFormat, int width, int height, int depth) { Delegates.glTexStorage3D(target, levels, internalFormat, width, height, depth); }
        internal static void glTextureStorage3D(uint texture, int levels, SizedInternalFormat internalFormat, int width, int height, int depth) { Delegates.glTextureStorage3D(texture, levels, internalFormat, width, height, depth); }
        internal static void glTexStorage3DMultisample(TextureTarget target, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations) { Delegates.glTexStorage3DMultisample(target, samples, internalFormat, width, height, depth, fixedsamplelocations); }
        internal static void glTextureStorage3DMultisample(uint texture, int samples, SizedInternalFormat internalFormat, int width, int height, int depth, bool fixedsamplelocations) { Delegates.glTextureStorage3DMultisample(texture, samples, internalFormat, width, height, depth, fixedsamplelocations); }
        internal static void glTexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTexSubImage1D(target, level, xoffset, width, format, type, pixels); }
        internal static void glTextureSubImage1D(uint texture, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTextureSubImage1D(texture, level, xoffset, width, format, type, pixels); }
        internal static void glTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels); }
        internal static void glTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels); }
        internal static void glTexSubImage3D(TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels); }
        internal static void glTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels) { Delegates.glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels); }
        internal static void glTextureBarrier() { Delegates.glTextureBarrier(); }
        internal static void glTextureView(uint texture, TextureTarget target, uint origtexture, PixelInternalFormat internalFormat, uint minlevel, uint numlevels, uint minlayer, uint numlayers) { Delegates.glTextureView(texture, target, origtexture, internalFormat, minlevel, numlevels, minlayer, numlayers); }
        internal static void glTransformFeedbackBufferBase(uint xfb, uint index, uint buffer) { Delegates.glTransformFeedbackBufferBase(xfb, index, buffer); }
        internal static void glTransformFeedbackBufferRange(uint xfb, uint index, uint buffer, IntPtr offset, int size) { Delegates.glTransformFeedbackBufferRange(xfb, index, buffer, offset, size); }
        internal static void glTransformFeedbackVaryings(uint program, int count, string[] varyings, TransformFeedbackMode bufferMode) { Delegates.glTransformFeedbackVaryings(program, count, varyings, bufferMode); }
        internal static void glUniform1f(int location, float v0) { Delegates.glUniform1f(location, v0); }
        internal static void glUniform2f(int location, float v0, float v1) { Delegates.glUniform2f(location, v0, v1); }
        internal static void glUniform3f(int location, float v0, float v1, float v2) { Delegates.glUniform3f(location, v0, v1, v2); }
        internal static void glUniform4f(int location, float v0, float v1, float v2, float v3) { Delegates.glUniform4f(location, v0, v1, v2, v3); }
        internal static void glUniform1i(int location, int v0) { Delegates.glUniform1i(location, v0); }
        internal static void glUniform2i(int location, int v0, int v1) { Delegates.glUniform2i(location, v0, v1); }
        internal static void glUniform3i(int location, int v0, int v1, int v2) { Delegates.glUniform3i(location, v0, v1, v2); }
        internal static void glUniform4i(int location, int v0, int v1, int v2, int v3) { Delegates.glUniform4i(location, v0, v1, v2, v3); }
        internal static void glUniform1ui(int location, uint v0) { Delegates.glUniform1ui(location, v0); }
        internal static void glUniform2ui(int location, uint v0, uint v1) { Delegates.glUniform2ui(location, v0, v1); }
        internal static void glUniform3ui(int location, uint v0, uint v1, uint v2) { Delegates.glUniform3ui(location, v0, v1, v2); }
        internal static void glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3) { Delegates.glUniform4ui(location, v0, v1, v2, v3); }
        internal static void glUniform1fv(int location, int count, float* value) { Delegates.glUniform1fv(location, count, value); }
        internal static void glUniform2fv(int location, int count, float* value) { Delegates.glUniform2fv(location, count, value); }
        internal static void glUniform3fv(int location, int count, float* value) { Delegates.glUniform3fv(location, count, value); }
        internal static void glUniform4fv(int location, int count, float* value) { Delegates.glUniform4fv(location, count, value); }
        internal static void glUniform1iv(int location, int count, int* value) { Delegates.glUniform1iv(location, count, value); }
        internal static void glUniform2iv(int location, int count, int* value) { Delegates.glUniform2iv(location, count, value); }
        internal static void glUniform3iv(int location, int count, int* value) { Delegates.glUniform3iv(location, count, value); }
        internal static void glUniform4iv(int location, int count, int* value) { Delegates.glUniform4iv(location, count, value); }
        internal static void glUniform1uiv(int location, int count, uint* value) { Delegates.glUniform1uiv(location, count, value); }
        internal static void glUniform2uiv(int location, int count, uint* value) { Delegates.glUniform2uiv(location, count, value); }
        internal static void glUniform3uiv(int location, int count, uint* value) { Delegates.glUniform3uiv(location, count, value); }
        internal static void glUniform4uiv(int location, int count, uint* value) { Delegates.glUniform4uiv(location, count, value); }
        internal static void glUniformMatrix2fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix2fv(location, count, transpose, value); }
        internal static void glUniformMatrix3fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix3fv(location, count, transpose, value); }
        internal static void glUniformMatrix4fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix4fv(location, count, transpose, value); }
        internal static void glUniformMatrix2x3fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix2x3fv(location, count, transpose, value); }
        internal static void glUniformMatrix3x2fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix3x2fv(location, count, transpose, value); }
        internal static void glUniformMatrix2x4fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix2x4fv(location, count, transpose, value); }
        internal static void glUniformMatrix4x2fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix4x2fv(location, count, transpose, value); }
        internal static void glUniformMatrix3x4fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix3x4fv(location, count, transpose, value); }
        internal static void glUniformMatrix4x3fv(int location, int count, bool transpose, float* value) { Delegates.glUniformMatrix4x3fv(location, count, transpose, value); }
        internal static void glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) { Delegates.glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding); }
        internal static void glUniformSubroutinesuiv(ShaderType shadertype, int count, uint[] indices) { Delegates.glUniformSubroutinesuiv(shadertype, count, indices); }
        internal static bool glUnmapBuffer(BufferTarget target) { return Delegates.glUnmapBuffer(target); }
        internal static bool glUnmapNamedBuffer(uint buffer) { return Delegates.glUnmapNamedBuffer(buffer); }
        internal static void glUseProgram(uint program) { Delegates.glUseProgram(program); }
        internal static void glUseProgramStages(uint pipeline, uint stages, uint program) { Delegates.glUseProgramStages(pipeline, stages, program); }
        internal static void glValidateProgram(uint program) { Delegates.glValidateProgram(program); }
        internal static void glValidateProgramPipeline(uint pipeline) { Delegates.glValidateProgramPipeline(pipeline); }
        internal static void glVertexArrayElementBuffer(uint vaobj, uint buffer) { Delegates.glVertexArrayElementBuffer(vaobj, buffer); }
        internal static void glVertexAttrib1f(uint index, float v0) { Delegates.glVertexAttrib1f(index, v0); }
        internal static void glVertexAttrib1s(uint index, short v0) { Delegates.glVertexAttrib1s(index, v0); }
        internal static void glVertexAttrib1d(uint index, double v0) { Delegates.glVertexAttrib1d(index, v0); }
        internal static void glVertexAttribI1i(uint index, int v0) { Delegates.glVertexAttribI1i(index, v0); }
        internal static void glVertexAttribI1ui(uint index, uint v0) { Delegates.glVertexAttribI1ui(index, v0); }
        internal static void glVertexAttrib2f(uint index, float v0, float v1) { Delegates.glVertexAttrib2f(index, v0, v1); }
        internal static void glVertexAttrib2s(uint index, short v0, short v1) { Delegates.glVertexAttrib2s(index, v0, v1); }
        internal static void glVertexAttrib2d(uint index, double v0, double v1) { Delegates.glVertexAttrib2d(index, v0, v1); }
        internal static void glVertexAttribI2i(uint index, int v0, int v1) { Delegates.glVertexAttribI2i(index, v0, v1); }
        internal static void glVertexAttribI2ui(uint index, uint v0, uint v1) { Delegates.glVertexAttribI2ui(index, v0, v1); }
        internal static void glVertexAttrib3f(uint index, float v0, float v1, float v2) { Delegates.glVertexAttrib3f(index, v0, v1, v2); }
        internal static void glVertexAttrib3s(uint index, short v0, short v1, short v2) { Delegates.glVertexAttrib3s(index, v0, v1, v2); }
        internal static void glVertexAttrib3d(uint index, double v0, double v1, double v2) { Delegates.glVertexAttrib3d(index, v0, v1, v2); }
        internal static void glVertexAttribI3i(uint index, int v0, int v1, int v2) { Delegates.glVertexAttribI3i(index, v0, v1, v2); }
        internal static void glVertexAttribI3ui(uint index, uint v0, uint v1, uint v2) { Delegates.glVertexAttribI3ui(index, v0, v1, v2); }
        internal static void glVertexAttrib4f(uint index, float v0, float v1, float v2, float v3) { Delegates.glVertexAttrib4f(index, v0, v1, v2, v3); }
        internal static void glVertexAttrib4s(uint index, short v0, short v1, short v2, short v3) { Delegates.glVertexAttrib4s(index, v0, v1, v2, v3); }
        internal static void glVertexAttrib4d(uint index, double v0, double v1, double v2, double v3) { Delegates.glVertexAttrib4d(index, v0, v1, v2, v3); }
        internal static void glVertexAttrib4Nub(uint index, byte v0, byte v1, byte v2, byte v3) { Delegates.glVertexAttrib4Nub(index, v0, v1, v2, v3); }
        internal static void glVertexAttribI4i(uint index, int v0, int v1, int v2, int v3) { Delegates.glVertexAttribI4i(index, v0, v1, v2, v3); }
        internal static void glVertexAttribI4ui(uint index, uint v0, uint v1, uint v2, uint v3) { Delegates.glVertexAttribI4ui(index, v0, v1, v2, v3); }
        internal static void glVertexAttribL1d(uint index, double v0) { Delegates.glVertexAttribL1d(index, v0); }
        internal static void glVertexAttribL2d(uint index, double v0, double v1) { Delegates.glVertexAttribL2d(index, v0, v1); }
        internal static void glVertexAttribL3d(uint index, double v0, double v1, double v2) { Delegates.glVertexAttribL3d(index, v0, v1, v2); }
        internal static void glVertexAttribL4d(uint index, double v0, double v1, double v2, double v3) { Delegates.glVertexAttribL4d(index, v0, v1, v2, v3); }
        internal static void glVertexAttrib1fv(uint index, float[] v) { Delegates.glVertexAttrib1fv(index, v); }
        internal static void glVertexAttrib1sv(uint index, short[] v) { Delegates.glVertexAttrib1sv(index, v); }
        internal static void glVertexAttrib1dv(uint index, double[] v) { Delegates.glVertexAttrib1dv(index, v); }
        internal static void glVertexAttribI1iv(uint index, int[] v) { Delegates.glVertexAttribI1iv(index, v); }
        internal static void glVertexAttribI1uiv(uint index, uint[] v) { Delegates.glVertexAttribI1uiv(index, v); }
        internal static void glVertexAttrib2fv(uint index, float[] v) { Delegates.glVertexAttrib2fv(index, v); }
        internal static void glVertexAttrib2sv(uint index, short[] v) { Delegates.glVertexAttrib2sv(index, v); }
        internal static void glVertexAttrib2dv(uint index, double[] v) { Delegates.glVertexAttrib2dv(index, v); }
        internal static void glVertexAttribI2iv(uint index, int[] v) { Delegates.glVertexAttribI2iv(index, v); }
        internal static void glVertexAttribI2uiv(uint index, uint[] v) { Delegates.glVertexAttribI2uiv(index, v); }
        internal static void glVertexAttrib3fv(uint index, float[] v) { Delegates.glVertexAttrib3fv(index, v); }
        internal static void glVertexAttrib3sv(uint index, short[] v) { Delegates.glVertexAttrib3sv(index, v); }
        internal static void glVertexAttrib3dv(uint index, double[] v) { Delegates.glVertexAttrib3dv(index, v); }
        internal static void glVertexAttribI3iv(uint index, int[] v) { Delegates.glVertexAttribI3iv(index, v); }
        internal static void glVertexAttribI3uiv(uint index, uint[] v) { Delegates.glVertexAttribI3uiv(index, v); }
        internal static void glVertexAttrib4fv(uint index, float[] v) { Delegates.glVertexAttrib4fv(index, v); }
        internal static void glVertexAttrib4sv(uint index, short[] v) { Delegates.glVertexAttrib4sv(index, v); }
        internal static void glVertexAttrib4dv(uint index, double[] v) { Delegates.glVertexAttrib4dv(index, v); }
        internal static void glVertexAttrib4iv(uint index, int[] v) { Delegates.glVertexAttrib4iv(index, v); }
        internal static void glVertexAttrib4bv(uint index, sbyte[] v) { Delegates.glVertexAttrib4bv(index, v); }
        internal static void glVertexAttrib4ubv(uint index, byte[] v) { Delegates.glVertexAttrib4ubv(index, v); }
        internal static void glVertexAttrib4usv(uint index, ushort[] v) { Delegates.glVertexAttrib4usv(index, v); }
        internal static void glVertexAttrib4uiv(uint index, uint[] v) { Delegates.glVertexAttrib4uiv(index, v); }
        internal static void glVertexAttrib4Nbv(uint index, sbyte[] v) { Delegates.glVertexAttrib4Nbv(index, v); }
        internal static void glVertexAttrib4Nsv(uint index, short[] v) { Delegates.glVertexAttrib4Nsv(index, v); }
        internal static void glVertexAttrib4Niv(uint index, int[] v) { Delegates.glVertexAttrib4Niv(index, v); }
        internal static void glVertexAttrib4Nubv(uint index, byte[] v) { Delegates.glVertexAttrib4Nubv(index, v); }
        internal static void glVertexAttrib4Nusv(uint index, ushort[] v) { Delegates.glVertexAttrib4Nusv(index, v); }
        internal static void glVertexAttrib4Nuiv(uint index, uint[] v) { Delegates.glVertexAttrib4Nuiv(index, v); }
        internal static void glVertexAttribI4bv(uint index, sbyte[] v) { Delegates.glVertexAttribI4bv(index, v); }
        internal static void glVertexAttribI4ubv(uint index, byte[] v) { Delegates.glVertexAttribI4ubv(index, v); }
        internal static void glVertexAttribI4sv(uint index, short[] v) { Delegates.glVertexAttribI4sv(index, v); }
        internal static void glVertexAttribI4usv(uint index, ushort[] v) { Delegates.glVertexAttribI4usv(index, v); }
        internal static void glVertexAttribI4iv(uint index, int[] v) { Delegates.glVertexAttribI4iv(index, v); }
        internal static void glVertexAttribI4uiv(uint index, uint[] v) { Delegates.glVertexAttribI4uiv(index, v); }
        internal static void glVertexAttribL1dv(uint index, double[] v) { Delegates.glVertexAttribL1dv(index, v); }
        internal static void glVertexAttribL2dv(uint index, double[] v) { Delegates.glVertexAttribL2dv(index, v); }
        internal static void glVertexAttribL3dv(uint index, double[] v) { Delegates.glVertexAttribL3dv(index, v); }
        internal static void glVertexAttribL4dv(uint index, double[] v) { Delegates.glVertexAttribL4dv(index, v); }
        internal static void glVertexAttribP1ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP1ui(index, type, normalized, value); }
        internal static void glVertexAttribP2ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP2ui(index, type, normalized, value); }
        internal static void glVertexAttribP3ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP3ui(index, type, normalized, value); }
        internal static void glVertexAttribP4ui(uint index, VertexAttribPType type, bool normalized, uint value) { Delegates.glVertexAttribP4ui(index, type, normalized, value); }
        internal static void glVertexAttribBinding(uint attribindex, uint bindingindex) { Delegates.glVertexAttribBinding(attribindex, bindingindex); }
        internal static void glVertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex) { Delegates.glVertexArrayAttribBinding(vaobj, attribindex, bindingindex); }
        internal static void glVertexAttribDivisor(uint index, uint divisor) { Delegates.glVertexAttribDivisor(index, divisor); }
        internal static void glVertexAttribFormat(uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset) { Delegates.glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset); }
        internal static void glVertexAttribIFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexAttribIFormat(attribindex, size, type, relativeoffset); }
        internal static void glVertexAttribLFormat(uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexAttribLFormat(attribindex, size, type, relativeoffset); }
        internal static void glVertexArrayAttribFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, bool normalized, uint relativeoffset) { Delegates.glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativeoffset); }
        internal static void glVertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativeoffset); }
        internal static void glVertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, VertexAttribFormatEnum type, uint relativeoffset) { Delegates.glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativeoffset); }
        internal static void glVertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, IntPtr pointer) { Delegates.glVertexAttribPointer(index, size, type, normalized, stride, pointer); }
        internal static void glVertexAttribIPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer) { Delegates.glVertexAttribIPointer(index, size, type, stride, pointer); }
        internal static void glVertexAttribLPointer(uint index, int size, VertexAttribPointerType type, int stride, IntPtr pointer) { Delegates.glVertexAttribLPointer(index, size, type, stride, pointer); }
        internal static void glVertexBindingDivisor(uint bindingindex, uint divisor) { Delegates.glVertexBindingDivisor(bindingindex, divisor); }
        internal static void glVertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor) { Delegates.glVertexArrayBindingDivisor(vaobj, bindingindex, divisor); }
        internal static void glViewport(int x, int y, int width, int height) { Delegates.glViewport(x, y, width, height); }
        internal static void glViewportArrayv(uint first, int count, float[] v) { Delegates.glViewportArrayv(first, count, v); }
        internal static void glViewportIndexedf(uint index, float x, float y, float w, float h) { Delegates.glViewportIndexedf(index, x, y, w, h); }
        internal static void glViewportIndexedfv(uint index, float[] v) { Delegates.glViewportIndexedfv(index, v); }
        internal static void glWaitSync(IntPtr sync, uint flags, ulong timeout) { Delegates.glWaitSync(sync, flags, timeout); }
    }

    delegate IntPtr getProcAddress(string proc);
}