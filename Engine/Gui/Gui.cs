using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public sealed partial class Gui
    {
        static Gui instance;

        Dictionary<int, Texture> id_tex_map = new Dictionary<int, Texture>();
        Dictionary<Type, string[]> enum_map = new Dictionary<Type, string[]>();

        MeshBuffer<Vertex> mb;
        Texture checker;
        MouseCursorKind last_cursor = MouseCursorKind.Arrow;

        Gui() { }

        public static void Render(Action<Gui> gui_draw)
        {
            if (instance == null)
                init();

            var screen = App.Window.Size;
            var io = instance.io;

            io.DisplaySize = screen;
            io.DeltaTime = Time.FrameTime;

            io.MousePosition = Input.MousePosition;

            io.MouseDown[0] = Input.IsKeyDown(MouseButton.Left);
            io.MouseDown[1] = Input.IsKeyDown(MouseButton.Right);
            io.MouseDown[2] = Input.IsKeyDown(MouseButton.Middle);

            io.MouseWheel = Input.MouseScrollDelta.Y;

            io.KeysDown[(int)KeyCode.Tab] = Input.IsKeyDown(KeyCode.Tab);
            io.KeysDown[(int)KeyCode.Left] = Input.IsKeyDown(KeyCode.Left);
            io.KeysDown[(int)KeyCode.Right] = Input.IsKeyDown(KeyCode.Right);
            io.KeysDown[(int)KeyCode.Up] = Input.IsKeyDown(KeyCode.Up);
            io.KeysDown[(int)KeyCode.Down] = Input.IsKeyDown(KeyCode.Down);
            io.KeysDown[(int)KeyCode.PageUp] = Input.IsKeyDown(KeyCode.PageUp);
            io.KeysDown[(int)KeyCode.PageDown] = Input.IsKeyDown(KeyCode.PageDown);
            io.KeysDown[(int)KeyCode.Home] = Input.IsKeyDown(KeyCode.Home);
            io.KeysDown[(int)KeyCode.End] = Input.IsKeyDown(KeyCode.End);
            io.KeysDown[(int)KeyCode.Delete] = Input.IsKeyDown(KeyCode.Delete);
            io.KeysDown[(int)KeyCode.BackSpace] = Input.IsKeyDown(KeyCode.BackSpace);
            io.KeysDown[(int)KeyCode.Enter] = Input.IsKeyDown(KeyCode.KpEnter) | Input.IsKeyDown(KeyCode.Enter);
            io.KeysDown[(int)KeyCode.Escape] = Input.IsKeyDown(KeyCode.Escape);
            io.KeysDown[(int)KeyCode.A] = Input.IsKeyDown(KeyCode.A);
            io.KeysDown[(int)KeyCode.C] = Input.IsKeyDown(KeyCode.C);
            io.KeysDown[(int)KeyCode.V] = Input.IsKeyDown(KeyCode.V);
            io.KeysDown[(int)KeyCode.X] = Input.IsKeyDown(KeyCode.X);
            io.KeysDown[(int)KeyCode.Y] = Input.IsKeyDown(KeyCode.Y);
            io.KeysDown[(int)KeyCode.Z] = Input.IsKeyDown(KeyCode.Z);

            io.AltPressed = Input.IsKeyDown(KeyCode.LAlt) || Input.IsKeyDown(KeyCode.RAlt);
            io.CtrlPressed = Input.IsKeyDown(KeyCode.LCtrl) || Input.IsKeyDown(KeyCode.RCtrl);
            io.ShiftPressed = Input.IsKeyDown(KeyCode.LShift) || Input.IsKeyDown(KeyCode.RShift);

            if (Input.GetLastKeyChar() != 0)
                ImGui.ImGuiIO_AddInputCharacter(Input.GetLastKeyChar());

            ImGui.igNewFrame();
            gui_draw(instance);
            ImGui.igRender();

            var gfx = App.Graphics;
            gfx.SetViewport(0, 0, (int)screen.X, (int)screen.Y);
            gfx.ViewMatrix = Matrix4x4.CreateOrthographicOffCenter(0, screen.X, screen.Y, 0, -1, 1);
            gfx.SetBlendMode(BlendMode.AlphaBlend);
            var shader = Data.Get<Shader>("Mult.Shader");
            gfx.SetShader(shader);
            shader.SetMatrix4x4("view_mat", gfx.ViewMatrix);
            shader.SetMatrix3x2("model_mat", Matrix3x2.Identity);

            unsafe
            {
                DrawData* draw_data = ImGui.igGetDrawData();

                for (int n = 0; n < draw_data->CmdListsCount; n++)
                {
                    NativeDrawList* cmd_list = draw_data->CmdLists[n];
                    var vtx = cmd_list->VtxBuffer;
                    var idx = cmd_list->IdxBuffer;

                    instance.mb.UpdateIndices(new IntPtr(idx.Data), idx.Size);
                    instance.mb.UpdateVertices(new IntPtr(vtx.Data), vtx.Size);
                    var cmd_buffer = cmd_list->CmdBuffer;
                    int offset = 0;
                    for (int i = 0; i < cmd_buffer.Size; i++)
                    {
                        DrawCmd pcmd = ((DrawCmd*)cmd_buffer.Data)[i];
                        int count = (int)pcmd.ElemCount;
                        gfx.SetScissor(
                            (int)pcmd.ClipRect.X,
                            (int)(io.DisplaySize.Y - pcmd.ClipRect.W),
                            (int)(pcmd.ClipRect.Z - pcmd.ClipRect.X),
                            (int)(pcmd.ClipRect.W - pcmd.ClipRect.Y));

                        shader.SetTexture("main_tex", 0, instance.id_tex_map[pcmd.TextureId.ToInt32()]);
                        instance.mb.Draw(offset * sizeof(ushort), count, PrimitiveType.Triangles);

                        offset += count;
                    }
                }
            }
        }

        string[] get_enum_arr<T>()
        {
            var type = typeof(T);
            string[] r;
            if (!enum_map.TryGetValue(type, out r))
            {
                r = Enum.GetNames(type);
                enum_map.Add(type, r);
            }

            return r;
        }

        void set_default_style()
        {
            style.WindowRounding = 0;
            style.WindowTitleAlign = Vector2.One * .5f;
            style.ScrollbarRounding = 2;
            style.ChildWindowRounding = 2;
            style.FrameRounding = 2;
            style.ScrollbarSize = 18;

            style.SetColor(ColorTarget.Text, new Color(1.00f, 1.00f, 1.00f, 1.00f));
            style.SetColor(ColorTarget.TextDisabled, new Color(1.00f, 0.87f, 1.00f, 0.39f));
            style.SetColor(ColorTarget.WindowBg, new Color(0.12f, 0.15f, 0.18f, 1.00f));
            style.SetColor(ColorTarget.ChildWindowBg, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.PopupBg, new Color(0.12f, 0.15f, 0.18f, 1.00f));
            style.SetColor(ColorTarget.Border, new Color(0.24f, 0.26f, 0.29f, 1.00f));
            style.SetColor(ColorTarget.BorderShadow, new Color(0.00f, 0.00f, 0.00f, 0.00f));
            style.SetColor(ColorTarget.FrameBg, new Color(0.22f, 0.31f, 0.37f, 1.00f));
            style.SetColor(ColorTarget.FrameBgHovered, new Color(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.FrameBgActive, new Color(0.19f, 0.57f, 0.87f, 1.00f));
            style.SetColor(ColorTarget.TitleBg, new Color(0.20f, 0.25f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.TitleBgCollapsed, new Color(0.20f, 0.25f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.TitleBgActive, new Color(0.20f, 0.25f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.MenuBarBg, new Color(0.19f, 0.22f, 0.24f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarBg, new Color(0.00f, 0.00f, 0.00f, 0.31f));
            style.SetColor(ColorTarget.ScrollbarGrab, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarGrabHovered, new Color(0.26f, 0.30f, 0.34f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarGrabActive, new Color(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.ComboBg, new Color(0.17f, 0.24f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.CheckMark, new Color(1.00f, 0.69f, 0.00f, 0.98f));
            style.SetColor(ColorTarget.SliderGrab, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.SliderGrabActive, new Color(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.Button, new Color(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.ButtonHovered, new Color(0.10f, 0.52f, 0.86f, 1.00f));
            style.SetColor(ColorTarget.ButtonActive, new Color(0.19f, 0.57f, 0.87f, 1.00f));
            style.SetColor(ColorTarget.Header, new Color(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.HeaderHovered, new Color(0.10f, 0.52f, 0.85f, 1.00f));
            style.SetColor(ColorTarget.HeaderActive, new Color(0.19f, 0.56f, 0.87f, 1.00f));
            style.SetColor(ColorTarget.Separator, new Color(0.24f, 0.26f, 0.29f, 1.00f));
            style.SetColor(ColorTarget.SeparatorHovered, new Color(0.31f, 0.33f, 0.36f, 1.00f));
            style.SetColor(ColorTarget.SeparatorActive, new Color(1.00f, 0.69f, 0.00f, 0.98f));
            style.SetColor(ColorTarget.ResizeGrip, new Color(0.39f, 0.39f, 0.39f, 0.71f));
            style.SetColor(ColorTarget.ResizeGripHovered, new Color(0.56f, 0.56f, 0.56f, 1.00f));
            style.SetColor(ColorTarget.ResizeGripActive, new Color(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.CloseButton, new Color(0.00f, 0.00f, 0.00f, 0.50f));
            style.SetColor(ColorTarget.CloseButtonHovered, new Color(0.00f, 0.00f, 0.00f, 0.25f));
            style.SetColor(ColorTarget.CloseButtonActive, new Color(1.00f, 0.22f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotLines, new Color(1.00f, 1.00f, 1.00f, 1.00f));
            style.SetColor(ColorTarget.PlotLinesHovered, new Color(0.90f, 0.70f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotHistogram, new Color(0.90f, 0.70f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotHistogramHovered, new Color(1.00f, 0.16f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.TextSelectedBg, new Color(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.ModalWindowDarkening, new Color(0.00f, 0.00f, 0.00f, 0.50f));

        }

        internal static void init()
        {
            instance = new Gui();
            unsafe { instance.io.GetNativePointer()->IniFilename = IntPtr.Zero; }
            instance.set_default_style();
            instance.io.FontAtlas.AddFontFromFileTTF(App.GetAbsolutePath("fonts/Roboto-Medium.ttf"), 14);

            instance.io.KeyMap[GuiKey.Tab] = (int)KeyCode.Tab;
            instance.io.KeyMap[GuiKey.LeftArrow] = (int)KeyCode.Left;
            instance.io.KeyMap[GuiKey.RightArrow] = (int)KeyCode.Right;
            instance.io.KeyMap[GuiKey.UpArrow] = (int)KeyCode.Up;
            instance.io.KeyMap[GuiKey.DownArrow] = (int)KeyCode.Down;
            instance.io.KeyMap[GuiKey.PageUp] = (int)KeyCode.PageUp;
            instance.io.KeyMap[GuiKey.PageDown] = (int)KeyCode.PageDown;
            instance.io.KeyMap[GuiKey.Home] = (int)KeyCode.Home;
            instance.io.KeyMap[GuiKey.End] = (int)KeyCode.End;
            instance.io.KeyMap[GuiKey.Delete] = (int)KeyCode.Delete;
            instance.io.KeyMap[GuiKey.Backspace] = (int)KeyCode.BackSpace;
            instance.io.KeyMap[GuiKey.Enter] = (int)KeyCode.Enter;
            instance.io.KeyMap[GuiKey.Escape] = (int)KeyCode.Escape;
            instance.io.KeyMap[GuiKey.A] = (int)KeyCode.A;
            instance.io.KeyMap[GuiKey.C] = (int)KeyCode.C;
            instance.io.KeyMap[GuiKey.V] = (int)KeyCode.V;
            instance.io.KeyMap[GuiKey.X] = (int)KeyCode.X;
            instance.io.KeyMap[GuiKey.Y] = (int)KeyCode.Y;
            instance.io.KeyMap[GuiKey.Z] = (int)KeyCode.Z;

            FontTextureData tex_data = instance.io.FontAtlas.GetTexDataAsRGBA32();
            unsafe
            {
                var tex = Texture.Create(tex_data.Width, tex_data.Height, FilterMode.Point, WrapMode.Clamp, new IntPtr(tex_data.Pixels));
                int id = tex.GetHashCode();
                instance.io.FontAtlas.SetTexID(id);
                instance.id_tex_map[id] = tex;
            }
            instance.io.FontAtlas.ClearTexData();

            instance.mb = MeshBuffer<Vertex>.CreateIndexed();

            Color[] checker_pixels = new Color[]
            {
                Color.LightGray, Color.White, Color.White, Color.LightGray
            };

            instance.checker = Data.Get<Texture>("Checker.Texture");
        }

        internal static void shutdown()
        {
            if (instance != null)
            {
                ImGui.igShutdown();
                instance.mb.Dispose();
            }
        }
    }
}