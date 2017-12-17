using System;
using System.Numerics;

using static CSFML;

namespace Engine
{
    public sealed class Window
    {
        const uint KeyCount = (uint)Keys.KeyCount;
        string title;
        IntPtr wnd_ptr;
        bool[] keys_state;

        internal Window(AppConfig cfg)
        {
            var mode = new VideoMode();
            mode.Width = (uint)cfg.Width;
            mode.Height = (uint)cfg.Height;
            mode.BitsPerPixel = 24;
            ContextSettings ctx = new ContextSettings();
            ctx.AntialiasingLevel = 0;
            ctx.DepthBits = 0;
            ctx.MajorVersion = 3;
            ctx.MinorVersion = 3;
            ctx.StencilBits = 0;

            Styles wnd_style = Styles.Close | Styles.Titlebar;

            if (cfg.Fullscreen)
                wnd_style |= Styles.Fullscreen;

            if (cfg.Resizable)
                wnd_style |= Styles.Resize;

            wnd_ptr = sfWindow_create(mode, cfg.Title, wnd_style, ref ctx);
            title = cfg.Title;
            sfWindow_setVerticalSyncEnabled(wnd_ptr, cfg.Vsync);
            sfWindow_setKeyRepeatEnabled(wnd_ptr, true);

            keys_state = new bool[(int)Keys.KeyCount];
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                sfWindow_setTitle(wnd_ptr, value);
            }
        }

        public Vector2 CursorPos
        {
            get
            {
                Vec2i pos = sfMouse_getPosition(wnd_ptr);
                return new Vector2(pos.X, pos.Y);
            }
            set
            {
                sfMouse_setPosition(new Vec2i((int)value.X, (int)value.Y), wnd_ptr);
            }
        }

        public Vector2 Size
        {
            get
            {
                var size = sfWindow_getSize(wnd_ptr);
                return new Vector2(size.X, size.Y);
            }
            set
            {
                sfWindow_setSize(wnd_ptr, new Vec2u((uint)value.X, (uint)value.Y));
            }
        }

        public bool IsKeyDown(Keys key) => get_key_state((int)key);

        internal void swap_buffers() => sfWindow_display(wnd_ptr);

        internal void do_events()
        {
            Event e;
            while (sfWindow_pollEvent(wnd_ptr, out e))
            {
                switch (e.Type)
                {
                    case EventType.Closed:
                        App.Quit();
                        break;
                    case EventType.Resized:
                        App.ActiveState.OnResize((int)e.Size.Width, (int)e.Size.Height);
                        break;
                    case EventType.KeyPressed:
                        if (get_key_state(e.Key.Code) == false)
                        {
                            set_key_state(e.Key.Code, true);
                            App.ActiveState.OnKeyDown((Keys)e.Key.Code, e.Key.Alt != 0, e.Key.Control != 0, e.Key.Shift != 0);
                        }
                        break;
                    case EventType.KeyReleased:
                        set_key_state(e.Key.Code, false);
                        App.ActiveState.OnKeyUp((Keys)e.Key.Code, e.Key.Alt != 0, e.Key.Control != 0, e.Key.Shift != 0);
                        break;
                    case EventType.MouseButtonPressed:
                        App.ActiveState.OnMouseDown((int)e.MouseButton.Button, new Vector2(e.MouseButton.X, e.MouseButton.Y));
                        break;
                    case EventType.MouseButtonReleased:
                        App.ActiveState.OnMouseUp((int)e.MouseButton.Button, new Vector2(e.MouseButton.X, e.MouseButton.Y));
                        break;
                    case EventType.TextEntered:
                        App.ActiveState.OnKeyChar(char.ConvertFromUtf32((int)e.Text.Unicode));
                        break;
                    case EventType.MouseWheelScrolled:
                        if (e.MouseWheelScroll.Wheel == 0)
                            App.ActiveState.OnMouseScroll(new Vector2(0, e.MouseWheelScroll.Delta)); // vertical wheel
                        else
                            App.ActiveState.OnMouseScroll(new Vector2(e.MouseWheelScroll.Delta, 0)); // horizontal wheel

                        break;
                    case EventType.MouseMoved:
                        App.ActiveState.OnMouseMove(new Vector2(e.MouseMove.X, e.MouseMove.Y));
                        break;
                }
            }
        }

        internal void shutdown()
        {
            sfWindow_close(wnd_ptr);
            sfWindow_destroy(wnd_ptr);
            wnd_ptr = IntPtr.Zero;
        }

        bool get_key_state(int code)
        {
            if ((uint)code < KeyCount)
                return keys_state[code];
            else
                return false;
        }

        void set_key_state(int code, bool value)
        {
            if ((uint)code < KeyCount)
                keys_state[code] = value;
        }
    }
}