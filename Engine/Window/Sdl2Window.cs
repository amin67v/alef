using System;
using System.Numerics;

namespace Engine
{
    using static Sdl2;

    class Sdl2Window : Window
    {
        string title; // window title
        IntPtr wnd; // window pointer
        IntPtr ctx; // gl context
        Vector2 size; // window size
        IntPtr[] curs; // system cursors

        public Sdl2Window(AppConfig cfg)
        {
            SDL_SetMainReady();
            SDL_Init(SDL_INIT_GAMECONTROLLER | SDL_INIT_VIDEO);
            SDL_SetHint("SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING", "1");

            SDL_WindowFlags flags = 0x00;
            if (cfg.Fullscreen)
                flags |= SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;

            if (cfg.Resizable)
                flags |= SDL_WindowFlags.SDL_WINDOW_RESIZABLE;

            flags |= SDL_WindowFlags.SDL_WINDOW_OPENGL;

            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_RED_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_BLUE_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_ALPHA_SIZE, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DEPTH_SIZE, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_STENCIL_SIZE, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);

            wnd = SDL_CreateWindow(cfg.Title, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, cfg.Width, cfg.Height, flags);
            size = new Vector2(cfg.Width, cfg.Height);
            if (wnd == IntPtr.Zero)
                throw new Exception($"Failed to create sdl window.\n{SDL_GetError()}");

            ctx = SDL_GL_CreateContext(wnd);
            if (ctx == IntPtr.Zero)
                throw new Exception($"Failed to create opengl context.\n{SDL_GetError()}");

            if (SDL_GL_MakeCurrent(wnd, ctx) != 0)
                throw new Exception($"Failed to make opengl context current.\n{SDL_GetError()}");

            SDL_GL_SetSwapInterval(cfg.Vsync ? 1 : 0);

            curs = new IntPtr[(int)SystemCursor.Count];
        }

        public override string Title
        {
            get => title;
            set
            {
                title = value;
                SDL_SetWindowTitle(wnd, value);
            }
        }

        public override Vector2 Size
        {
            get => size;
            set
            {
                size = value;
                SDL_SetWindowSize(wnd, (int)value.X, (int)value.Y);
            }
        }

        protected internal override void WrapMouse(Vector2 value)
        {
            SDL_WarpMouseInWindow(wnd, (int)value.X, (int)value.Y);
        }

        public override void SetIcon(Image value)
        {
            IntPtr surface;

            if (BitConverter.IsLittleEndian)
                surface = SDL_CreateRGBSurfaceFrom(value.PixelData, value.Width, value.Height, 32, value.Width * 4, 0x000000ffu, 0x0000ff00u, 0x00ff0000u, 0xff000000u);
            else
                surface = SDL_CreateRGBSurfaceFrom(value.PixelData, value.Width, value.Height, 32, value.Width * 4, 0xff000000u, 0x00ff0000u, 0x0000ff00u, 0x000000ffu);

            SDL_SetWindowIcon(wnd, surface);
            SDL_FreeSurface(surface);
        }

        public override void SetCursor(SystemCursor value)
        {
            if (curs[(int)value] == IntPtr.Zero)
                curs[(int)value] = SDL_CreateSystemCursor((SDL_SystemCursor)value);

            SDL_SetCursor(curs[(int)value]);
        }

        protected internal override void SwapBuffers()
        {
             SDL_GL_SwapWindow(wnd);
        }

        protected internal override void DoEvents()
        {
            SDL_Event e;
            while (SDL_PollEvent(out e) != 0)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        App.Quit();
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT:
                        switch (e.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                                size = new Vector2(e.window.data1, e.window.data2);
                                RaiseResize(e.window.data1, e.window.data2);
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                RaiseLostFocus();
                                break;
                        }
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        {
                            if (e.key.repeat == 0)
                                RaiseKeyPressed((KeyCode)e.key.keysym.scancode);
                        }
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        {
                            RaiseKeyReleased((KeyCode)e.key.keysym.scancode);
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        {
                            int button = e.button.button - 1;
                            if ((uint)button < 3)
                                RaiseMouseButtonPressed((MouseButton)button);
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        {
                            int button = e.button.button - 1;
                            if ((uint)button < 3)
                                RaiseMouseButtonReleased((MouseButton)button);
                        }
                        break;
                    case SDL_EventType.SDL_TEXTINPUT:
                        unsafe
                        {
                            RaiseTextInput(UTF8_ToManaged(new IntPtr(e.text.text)));
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEWHEEL:
                        RaiseMouseScroll(new Vector2(e.wheel.x, e.wheel.y));
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION:
                        RaiseMouseMove(new Vector2(e.motion.x, e.motion.y));
                        break;
                    case SDL_EventType.SDL_DROPFILE:
                        var dropfile = UTF8_ToManaged(e.drop.file);
                        RaiseFileDrop(dropfile);
                        SDL_free(e.drop.file);
                        break;
                    case SDL_EventType.SDL_DROPTEXT:
                        var droptext = UTF8_ToManaged(e.drop.file);
                        RaiseTextDrop(droptext);
                        SDL_free(e.drop.file);
                        break;
                    case SDL_EventType.SDL_DROPBEGIN:
                        if (e.drop.file != IntPtr.Zero)
                            SDL_free(e.drop.file);
                        break;
                    case SDL_EventType.SDL_DROPCOMPLETE:
                        if (e.drop.file != IntPtr.Zero)
                            SDL_free(e.drop.file);
                        break;
                }
            }
        }

        protected internal override void ShutDown()
        {
            for (int i = 0; i < (int)SystemCursor.Count; i++)
            {
                if (curs[i] != IntPtr.Zero)
                    SDL_FreeCursor(curs[i]);
            }
            curs = null;

            SDL_GL_DeleteContext(ctx);
            SDL_DestroyWindow(wnd);
            SDL_Quit();
            wnd = IntPtr.Zero;
        }
    }
}