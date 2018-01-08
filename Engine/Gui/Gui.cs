using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public sealed partial class Gui
    {
        Array<Texture> textures = new Array<Texture>();
        Dictionary<Type, string[]> enum_map = new Dictionary<Type, string[]>();

        Shader shader;
        MeshBuffer mb;
        MouseCursorKind last_cursor = MouseCursorKind.Arrow;

        internal Gui()
        {
            unsafe { io.GetNativePointer()->IniFilename = IntPtr.Zero; }
            set_default_style();
            io.FontAtlas.AddFontFromFileTTF(App.GetAbsolutePath("fonts/Ubuntu-R.ttf"), 14);

            io.KeyMap[GuiKey.Tab] = (int)KeyCode.Tab;
            io.KeyMap[GuiKey.LeftArrow] = (int)KeyCode.Left;
            io.KeyMap[GuiKey.RightArrow] = (int)KeyCode.Right;
            io.KeyMap[GuiKey.UpArrow] = (int)KeyCode.Up;
            io.KeyMap[GuiKey.DownArrow] = (int)KeyCode.Down;
            io.KeyMap[GuiKey.PageUp] = (int)KeyCode.PageUp;
            io.KeyMap[GuiKey.PageDown] = (int)KeyCode.PageDown;
            io.KeyMap[GuiKey.Home] = (int)KeyCode.Home;
            io.KeyMap[GuiKey.End] = (int)KeyCode.End;
            io.KeyMap[GuiKey.Delete] = (int)KeyCode.Delete;
            io.KeyMap[GuiKey.Backspace] = (int)KeyCode.BackSpace;
            io.KeyMap[GuiKey.Enter] = (int)KeyCode.Enter;
            io.KeyMap[GuiKey.Escape] = (int)KeyCode.Escape;
            io.KeyMap[GuiKey.A] = (int)KeyCode.A;
            io.KeyMap[GuiKey.C] = (int)KeyCode.C;
            io.KeyMap[GuiKey.V] = (int)KeyCode.V;
            io.KeyMap[GuiKey.X] = (int)KeyCode.X;
            io.KeyMap[GuiKey.Y] = (int)KeyCode.Y;
            io.KeyMap[GuiKey.Z] = (int)KeyCode.Z;

            FontTextureData tex_data = io.FontAtlas.GetTexDataAsRGBA32();
            unsafe
            {
                textures.Push(Texture.Create(tex_data.Width, tex_data.Height, FilterMode.Point, WrapMode.Clamp, new IntPtr(tex_data.Pixels)));
                io.FontAtlas.SetTexID(0);
            }
            io.FontAtlas.ClearTexData();

            shader = DefaultShaders.ColorMult;
            mb = MeshBuffer.CreateIndexed();
        }

        public void AddInputChar(char code) => ImGui.ImGuiIO_AddInputCharacter(code);

        public void Render(Action gui_draw)
        {
            var screen = App.Window.Size;

            io.DisplaySize = screen;
            io.DeltaTime = App.Time.FrameTime;

            io.MousePosition = App.Window.MousePosition;

            io.MouseDown[0] = App.Window.IsMouseDown(MouseButton.Left);
            io.MouseDown[1] = App.Window.IsMouseDown(MouseButton.Right);
            io.MouseDown[2] = App.Window.IsMouseDown(MouseButton.Middle);

            io.MouseWheel = App.Window.MouseScrollDelta.Y;

            io.KeysDown[(int)KeyCode.Tab] = App.Window.IsKeyDown(KeyCode.Tab);
            io.KeysDown[(int)KeyCode.Left] = App.Window.IsKeyDown(KeyCode.Left);
            io.KeysDown[(int)KeyCode.Right] = App.Window.IsKeyDown(KeyCode.Right);
            io.KeysDown[(int)KeyCode.Up] = App.Window.IsKeyDown(KeyCode.Up);
            io.KeysDown[(int)KeyCode.Down] = App.Window.IsKeyDown(KeyCode.Down);
            io.KeysDown[(int)KeyCode.PageUp] = App.Window.IsKeyDown(KeyCode.PageUp);
            io.KeysDown[(int)KeyCode.PageDown] = App.Window.IsKeyDown(KeyCode.PageDown);
            io.KeysDown[(int)KeyCode.Home] = App.Window.IsKeyDown(KeyCode.Home);
            io.KeysDown[(int)KeyCode.End] = App.Window.IsKeyDown(KeyCode.End);
            io.KeysDown[(int)KeyCode.Delete] = App.Window.IsKeyDown(KeyCode.Delete);
            io.KeysDown[(int)KeyCode.BackSpace] = App.Window.IsKeyDown(KeyCode.BackSpace);
            io.KeysDown[(int)KeyCode.Enter] = App.Window.IsKeyDown(KeyCode.KpEnter) | App.Window.IsKeyDown(KeyCode.Enter);
            io.KeysDown[(int)KeyCode.Escape] = App.Window.IsKeyDown(KeyCode.Escape);
            io.KeysDown[(int)KeyCode.A] = App.Window.IsKeyDown(KeyCode.A);
            io.KeysDown[(int)KeyCode.C] = App.Window.IsKeyDown(KeyCode.C);
            io.KeysDown[(int)KeyCode.V] = App.Window.IsKeyDown(KeyCode.V);
            io.KeysDown[(int)KeyCode.X] = App.Window.IsKeyDown(KeyCode.X);
            io.KeysDown[(int)KeyCode.Y] = App.Window.IsKeyDown(KeyCode.Y);
            io.KeysDown[(int)KeyCode.Z] = App.Window.IsKeyDown(KeyCode.Z);

            io.AltPressed = App.Window.IsKeyDown(KeyCode.LAlt) || App.Window.IsKeyDown(KeyCode.RAlt);
            io.CtrlPressed = App.Window.IsKeyDown(KeyCode.LCtrl) || App.Window.IsKeyDown(KeyCode.RCtrl);
            io.ShiftPressed = App.Window.IsKeyDown(KeyCode.LShift) || App.Window.IsKeyDown(KeyCode.RShift);

            ImGui.igNewFrame();
            gui_draw();
            ImGui.igRender();

            var cursor = MouseCursor;
            if (last_cursor != cursor)
            {
                switch (cursor)
                {
                    case MouseCursorKind.Arrow:
                        App.Window.SetCursor(SystemCursor.Arrow);
                        break;
                    case MouseCursorKind.TextInput:
                        App.Window.SetCursor(SystemCursor.IBeam);
                        break;
                    case MouseCursorKind.Move:
                        App.Window.SetCursor(SystemCursor.Hand);
                        break;
                    case MouseCursorKind.ResizeNS:
                        App.Window.SetCursor(SystemCursor.SizeNS);
                        break;
                    case MouseCursorKind.ResizeEW:
                        App.Window.SetCursor(SystemCursor.SizeWE);
                        break;
                    case MouseCursorKind.ResizeNESW:
                        App.Window.SetCursor(SystemCursor.SizeNESW);
                        break;
                    case MouseCursorKind.ResizeNWSE:
                        App.Window.SetCursor(SystemCursor.SizeNWSE);
                        break;
                }
                last_cursor = cursor;
            }


            var gfx = App.Graphics;
            gfx.SetViewport(0, 0, (int)screen.X, (int)screen.Y);
            gfx.ViewMatrix = Matrix4x4.CreateOrthographicOffCenter(0, screen.X, screen.Y, 0, -1, 1);
            gfx.SetBlendMode(BlendMode.AlphaBlend);
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

                    mb.UpdateIndices(new IntPtr(idx.Data), idx.Size);
                    mb.UpdateVertices(new IntPtr(vtx.Data), vtx.Size);
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

                        shader.SetTexture("main_tex", 0, textures[pcmd.TextureId.ToInt32()]);
                        mb.Draw(offset * sizeof(ushort), count, PrimitiveType.Triangles);

                        offset += count;
                    }
                }
            }
        }

        public int AddTexture(Texture tex)
        {
            for (int i = 1; i < textures.Count; i++)
            {
                if (tex == textures[i])
                    return i;
            }

            textures.Push(tex);
            return textures.Count - 1;
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
            style.ScrollbarRounding = 3;
            style.ChildWindowRounding = 3;
            style.FrameRounding = 3;
            style.ScrollbarSize = 18;

            style.SetColor(ColorTarget.Text, new Color(1.00f, 1.00f, 1.00f, 0.98f));
            style.SetColor(ColorTarget.TextDisabled, new Color(1.00f, 0.87f, 1.00f, 0.20f));
            style.SetColor(ColorTarget.WindowBg, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.ChildWindowBg, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.PopupBg, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.Border, new Color(0.24f, 0.26f, 0.29f, 1.00f));
            style.SetColor(ColorTarget.BorderShadow, new Color(0.00f, 0.00f, 0.00f, 0.00f));
            style.SetColor(ColorTarget.FrameBg, new Color(0.00f, 0.00f, 0.00f, 0.31f));
            style.SetColor(ColorTarget.FrameBgHovered, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.FrameBgActive, new Color(0.00f, 0.70f, 0.63f, 1.00f));
            style.SetColor(ColorTarget.TitleBg, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.TitleBgCollapsed, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.TitleBgActive, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.MenuBarBg, new Color(0.22f, 0.25f, 0.27f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarBg, new Color(0.00f, 0.00f, 0.00f, 0.31f));
            style.SetColor(ColorTarget.ScrollbarGrab, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarGrabHovered, new Color(0.23f, 0.26f, 0.29f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarGrabActive, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.ComboBg, new Color(0.13f, 0.15f, 0.16f, 1.00f));
            style.SetColor(ColorTarget.CheckMark, new Color(1.00f, 1.00f, 1.00f, 0.98f));
            style.SetColor(ColorTarget.SliderGrab, new Color(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.SliderGrabActive, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.Button, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.ButtonHovered, new Color(0.00f, 0.70f, 0.63f, 1.00f));
            style.SetColor(ColorTarget.ButtonActive, new Color(0.00f, 0.85f, 0.75f, 1.00f));
            style.SetColor(ColorTarget.Header, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.HeaderHovered, new Color(0.00f, 0.70f, 0.63f, 1.00f));
            style.SetColor(ColorTarget.HeaderActive, new Color(0.00f, 0.85f, 0.75f, 1.00f));
            style.SetColor(ColorTarget.Separator, new Color(0.24f, 0.26f, 0.29f, 1.00f));
            style.SetColor(ColorTarget.SeparatorHovered, new Color(0.18f, 0.21f, 0.24f, 1.00f));
            style.SetColor(ColorTarget.SeparatorActive, new Color(0.15f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.ResizeGrip, new Color(0.39f, 0.39f, 0.39f, 0.71f));
            style.SetColor(ColorTarget.ResizeGripHovered, new Color(0.56f, 0.56f, 0.56f, 1.00f));
            style.SetColor(ColorTarget.ResizeGripActive, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.CloseButton, new Color(0.00f, 0.00f, 0.00f, 0.50f));
            style.SetColor(ColorTarget.CloseButtonHovered, new Color(0.00f, 0.00f, 0.00f, 0.25f));
            style.SetColor(ColorTarget.CloseButtonActive, new Color(1.00f, 0.22f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotLines, new Color(1.00f, 1.00f, 1.00f, 1.00f));
            style.SetColor(ColorTarget.PlotLinesHovered, new Color(0.90f, 0.70f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotHistogram, new Color(0.90f, 0.70f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotHistogramHovered, new Color(1.00f, 0.16f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.TextSelectedBg, new Color(0.00f, 0.59f, 0.53f, 1.00f));
            style.SetColor(ColorTarget.ModalWindowDarkening, new Color(0.00f, 0.00f, 0.00f, 0.50f));
        }

        internal void shutdown()
        {
            ImGui.igShutdown();
            mb.Dispose();
            for (int i = 0; i < textures.Count; i++)
                textures[i].Dispose();
            // no need to dispose a default shader
        }
    }
}