using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    using static SDL2.SDL;

    sealed class SDLWindow : Window
    {
        string title;
        IntPtr ptr;
        IntPtr ctx; // gl context
        int width;
        int height;
        bool isFocused = true;
        Array<string> droppedFiles;

        public override string Title
        {
            get => title;
            set
            {
                title = value;
                SDL_SetWindowTitle(ptr, value);
            }
        }

        public override int Width => width;

        public override int Height => height;

        public override bool IsFocused => isFocused;

        public SDLWindow(GameConfig cfg)
        {
            SDL_SetMainReady();
            SDL_Init(SDL_INIT_GAMECONTROLLER | SDL_INIT_VIDEO);
            SDL_SetHint("SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING", "1");

            SDL_WindowFlags flags = 0x00;
            if (cfg.Resizable)
                flags |= SDL_WindowFlags.SDL_WINDOW_RESIZABLE;

            flags |= SDL_WindowFlags.SDL_WINDOW_OPENGL;

            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_RED_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_BLUE_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_ALPHA_SIZE, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DEPTH_SIZE, 24);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_STENCIL_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, 0);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);

            ptr = SDL_CreateWindow(Game.Current.Name, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, cfg.Width, cfg.Height, flags);
            width = cfg.Width;
            height = cfg.Height;
            title = Game.Current.Name;

            if (ptr == IntPtr.Zero)
                throw new Exception($"Failed to create sdl window.\n{SDL_GetError()}");

            ctx = SDL_GL_CreateContext(ptr);
            if (ctx == IntPtr.Zero)
                throw new Exception($"Failed to create opengl context.\n{SDL_GetError()}");

            if (SDL_GL_MakeCurrent(ptr, ctx) != 0)
                throw new Exception($"Failed to make opengl context current.\n{SDL_GetError()}");

            SDL_GL_SetSwapInterval(cfg.Vsync ? 1 : 0);

            droppedFiles = new Array<string>();
            SetSize(cfg.Width, cfg.Height, cfg.Fullscreen);
        }

        public override void SetSize(int width, int height, bool fullscreen)
        {
            SDL_SetWindowSize(ptr, width, height);
            SDL_SetWindowFullscreen(ptr, fullscreen ? (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN : 0);
        }

        public override Vector2 GetMousePosition()
        {
            SDL_GetMouseState(out int x, out int y);
            return new Vector2(x, y);
        }

        public override void SetMousePosition(Vector2 value)
        {
            SDL_WarpMouseInWindow(ptr, (int)value.X, (int)value.Y);
        }

        public override void SetIcon(Image image)
        {
            var surface = createSdlSurface(image);
            SDL_SetWindowIcon(ptr, surface);
            SDL_FreeSurface(surface);
        }

        public override IntPtr CreateCursor(Image image, Vector2 hotpos)
        {
            var surface = createSdlSurface(image);
            var r = SDL_CreateColorCursor(surface, (int)hotpos.X, (int)hotpos.Y);
            SDL_FreeSurface(surface);
            return r;
        }

        public override void SetCursor(IntPtr cursor) => SDL_SetCursor(cursor);

        public override void DestroyCursor(IntPtr cursor) => SDL_FreeCursor(cursor);

        public override void SwapBuffers() => SDL_GL_SwapWindow(ptr);

        public override void DoEvents()
        {
            droppedFiles.Clear();
            SDL_Event e;
            while (SDL_PollEvent(out e) != 0)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        Game.Current.Quit();
                        break;
                    case SDL_EventType.SDL_WINDOWEVENT:
                        switch (e.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                                this.width = e.window.data1;
                                this.height = e.window.data2;
                                RaiseOnResize(e.window.data1, e.window.data2);
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                                RaiseOnFocusChanged(true);
                                isFocused = false;
                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                                RaiseOnFocusChanged(false);
                                isFocused = true;
                                break;
                        }
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        {
                            if (e.key.repeat == 0)
                                RaiseOnKeyPressed((KeyCode)e.key.keysym.scancode);
                        }
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        {
                            RaiseOnKeyReleased((KeyCode)e.key.keysym.scancode);
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        {
                            int button = e.button.button - 1;
                            if ((uint)button < 3)
                                RaiseOnMouseButtonPressed((MouseButton)button);
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        {
                            int button = e.button.button - 1;
                            if ((uint)button < 3)
                                RaiseOnMouseButtonReleased((MouseButton)button);
                        }
                        break;
                    case SDL_EventType.SDL_TEXTINPUT:
                        unsafe
                        {
                            var str = UTF8_ToManaged(new IntPtr(e.text.text));
                            RaiseOnTextInput(str[str.Length - 1]);
                        }
                        break;
                    case SDL_EventType.SDL_MOUSEWHEEL:
                        RaiseOnMouseScroll(new Vector2(e.wheel.x, e.wheel.y));
                        break;
                    case SDL_EventType.SDL_MOUSEMOTION:
                        RaiseOnMouseMove(new Vector2(e.motion.x, e.motion.y));
                        break;
                    case SDL_EventType.SDL_DROPFILE:
                        var dropfile = UTF8_ToManaged(e.drop.file, true);
                        droppedFiles.Push(dropfile);
                        break;
                    case SDL_EventType.SDL_DROPTEXT:
                        if (e.drop.file != IntPtr.Zero)
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

            if (droppedFiles.Count > 0)
                RaiseOnFileDrop(droppedFiles.ToArray());
        }

        protected override void OnDestroy()
        {
            SDL_GL_DeleteContext(ctx);
            SDL_DestroyWindow(ptr);
            SDL_Quit();

            ptr = IntPtr.Zero;
        }

        IntPtr createSdlSurface(Image value)
        {
            IntPtr surface;
            if (BitConverter.IsLittleEndian)
                surface = SDL_CreateRGBSurfaceFrom(value.PixelData, value.Width, value.Height, 32, value.Width * 4, 0x000000ffu, 0x0000ff00u, 0x00ff0000u, 0xff000000u);
            else
                surface = SDL_CreateRGBSurfaceFrom(value.PixelData, value.Width, value.Height, 32, value.Width * 4, 0xff000000u, 0x00ff0000u, 0x0000ff00u, 0x000000ffu);

            return surface;
        }
    }
}