using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ImGuiNET;

namespace Engine
{
    public class Gui : ObjectBase
    {
        const string floatDisplayFormat = "%.3f";

        Dictionary<int, Texture2D> idToTex;
        Dictionary<Type, string[]> enumMap;
        MeshBuffer meshBuffer;
        Shader shader;
        Matrix4x4 matrix;

        internal Gui()
        {
            idToTex = new Dictionary<int, Texture2D>();
            enumMap = new Dictionary<Type, string[]>();

            meshBuffer = Graphics.CreateMeshBuffer(Vertex2D.Format, PrimitiveType.Triangles, IntPtr.Zero, 0, IntPtr.Zero, 0);

            const string vertSource = @"
                #version 330 core
                layout(location = 0) in vec2 v_Position;
                layout(location = 1) in vec2 v_Texcoord;
                layout(location = 2) in vec4 v_Color;

                out vec4 v2f_color;
                out vec2 v2f_texcoord;

                uniform mat4 GuiMatrix;

                void main()
                {
                    gl_Position = GuiMatrix * vec4(v_Position.x, v_Position.y, 0, 1);
                    v2f_texcoord = v_Texcoord;
                    v2f_color = v_Color;
                }";

            const string fragSource = @"
                #version 330 core
                in vec4 v2f_color;
                in vec2 v2f_texcoord;

                out vec4 FragColor;

                uniform sampler2D Albedo;

                void main() 
                {
                    FragColor = texture(Albedo, v2f_texcoord) * v2f_color;
                }";

            {
                shader = Graphics.CreateShader(vertSource, null, fragSource);
                var id1 = shader.GetUniformID("Albedo");
                var id2 = shader.GetUniformID("GuiMatrix");
                shader.OnSetUniforms += (object obj) =>
                {
                    shader.SetUniform(id1, 0, obj as Texture2D);
                    shader.SetUniform(id2, ref matrix);
                };
            }

            var io = ImGui.GetIO();
            setDefaultStyle();
            var font = System.IO.File.ReadAllBytes(Game.Current.GetAbsolutePath("Roboto-Medium.ttf"));
            var handle = GCHandle.Alloc(font, GCHandleType.Pinned);
            io.FontAtlas.AddFontFromMemoryTTF(handle.AddrOfPinnedObject(), font.Length, 14);
            handle.Free();

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

            FontTextureData fontTexData = io.FontAtlas.GetTexDataAsRGBA32();
            unsafe
            {
                var tex = Graphics.CreateTexture2D(fontTexData.Width, fontTexData.Height, PixelFormat.Rgba8, FilterMode.Point, WrapMode.Clamp, new IntPtr(fontTexData.Pixels));
                int id = tex.GetHashCode();
                io.FontAtlas.SetTexID(id);
                idToTex[id] = tex;

                io.GetNativePointer()->IniFilename = IntPtr.Zero;
            }
            io.FontAtlas.ClearTexData();
        }

        public bool Header(string label) => ImGui.CollapsingHeader(label, TreeNodeFlags.DefaultOpen);

        public bool Button(string label) => ImGui.Button(label);

        public T EnumEdit<T>(string label, T value)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new Exception("The type of 'T' is not enum, only enum type is accepted in this context.");

            var arr = getEnumArray<T>();

            int index = Array.IndexOf<string>(arr, value.ToString());
            var r = ImGui.Combo(label, ref index, arr, 5);
            if (r)
                return (T)Enum.Parse(type, arr[index]);
            else
                return value;
        }

        public bool BoolEdit(string label, bool value)
        {
            ImGui.Checkbox(label, ref value);
            return value;
        }

        public int IntEdit(string label, int value)
        {
            ImGui.DragInt(label, ref value, 1, int.MinValue, int.MaxValue, "%.f");
            return value;
        }

        public float FloatEdit(string label, float value)
        {
            ImGui.DragFloat(label, ref value, float.MinValue, float.MaxValue, 0.05f, floatDisplayFormat);
            return value;
        }

        public Vector2 Vector2Edit(string label, Vector2 value)
        {
            ImGui.DragVector2(label, ref value, float.MinValue, float.MaxValue, 0.05f, floatDisplayFormat);
            return value;
        }

        public Vector3 Vector3Edit(string label, Vector3 value)
        {
            ImGui.DragVector3(label, ref value, float.MinValue, float.MaxValue, 0.05f, floatDisplayFormat);
            return value;
        }

        public Vector4 Vector4Edit(string label, Vector4 value)
        {
            ImGui.DragVector4(label, ref value, float.MinValue, float.MaxValue, 0.05f, floatDisplayFormat);
            return value;
        }

        public Quaternion QuaternionEdit(string label, Quaternion value)
        {
            Vector4 vec = new Vector4(value.X, value.Y, value.Z, value.W);
            ImGui.DragVector4(label, ref vec, float.MinValue, float.MaxValue, 0.05f, floatDisplayFormat);

            return new Quaternion(vec.X, vec.Y, vec.Z, vec.W);
        }

        public Color ColorEdit(string label, Color value)
        {
            var vec = value.ToVector4();
            var result = ImGui.ColorEdit4(label, ref vec, ColorEditFlags.Default);
            return new Color(vec);
        }

        public void TextLabel(string label, string text) => ImGui.LabelText(label, text);

        public void Separator() => ImGui.Separator();

        public void Indent(float value) => ImGuiNative.igIndent(value);

        public void Unindent(float value) => ImGuiNative.igUnindent(value);

        public void BeginWindow(string title) => ImGui.BeginWindow(title);

        public void EndWindow() => ImGui.EndWindow();

        public void Render(Action<Gui> ongui)
        {
            var windowSize = new Vector2(Window.Width, Window.Height);
            var io = ImGui.GetIO();

            io.DisplaySize = windowSize;
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
                ImGui.AddInputCharacter(Input.GetLastKeyChar());

            ImGui.NewFrame();
            ongui?.Invoke(this);
            ImGui.Render();

            var g = Graphics;
            Graphics.Viewport = new Rect(0, 0, windowSize.X, windowSize.Y);
            matrix = Matrix4x4.CreateOrthographicOffCenter(0, windowSize.X, windowSize.Y, 0, -1, 1);
            Graphics.SetFaceCull(FaceCull.None);
            Graphics.SetDepthTest(DepthTest.Disable);
            Graphics.SetBlendMode(BlendMode.AlphaBlend);
            Graphics.SetDepthWrite(false);

            unsafe
            {
                DrawData* draw_data = ImGui.GetDrawData();

                for (int n = 0; n < draw_data->CmdListsCount; n++)
                {
                    NativeDrawList* cmd_list = draw_data->CmdLists[n];
                    var vtx = cmd_list->VtxBuffer;
                    var idx = cmd_list->IdxBuffer;

                    meshBuffer.UpdateIndices(new IntPtr(idx.Data), idx.Size);
                    meshBuffer.UpdateVertices(new IntPtr(vtx.Data), vtx.Size);
                    var cmd_buffer = cmd_list->CmdBuffer;
                    int offset = 0;
                    for (int i = 0; i < cmd_buffer.Size; i++)
                    {
                        DrawCmd pcmd = ((DrawCmd*)cmd_buffer.Data)[i];
                        int count = (int)pcmd.ElemCount;
                        Graphics.Scissor = new Rect(pcmd.ClipRect.X, (io.DisplaySize.Y - pcmd.ClipRect.W),
                            (pcmd.ClipRect.Z - pcmd.ClipRect.X), (pcmd.ClipRect.W - pcmd.ClipRect.Y));

                        Graphics.SetShader(shader, idToTex[pcmd.TextureId.ToInt32()]);
                        Graphics.Draw(meshBuffer, offset * sizeof(ushort), count);
                        offset += count;
                    }
                }

                Graphics.Scissor = new Rect();
            }

            Graphics.SetFaceCull(FaceCull.Back);
        }

        void setDefaultStyle()
        {
            var style = ImGui.GetStyle();
            style.WindowRounding = 0;
            style.WindowTitleAlign = Vector2.One * .5f;
            style.ScrollbarRounding = 2;
            style.ChildWindowRounding = 2;
            style.FrameRounding = 2;
            style.ScrollbarSize = 18;

            style.SetColor(ColorTarget.Text, new Vector4(1.00f, 1.00f, 1.00f, 1.00f));
            style.SetColor(ColorTarget.TextDisabled, new Vector4(1.00f, 0.87f, 1.00f, 0.39f));
            style.SetColor(ColorTarget.WindowBg, new Vector4(0.12f, 0.15f, 0.18f, 1.00f));
            style.SetColor(ColorTarget.ChildBg, new Vector4(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.PopupBg, new Vector4(0.12f, 0.15f, 0.18f, 1.00f));
            style.SetColor(ColorTarget.Border, new Vector4(0.24f, 0.26f, 0.29f, 1.00f));
            style.SetColor(ColorTarget.BorderShadow, new Vector4(0.00f, 0.00f, 0.00f, 0.00f));
            style.SetColor(ColorTarget.FrameBg, new Vector4(0.22f, 0.31f, 0.37f, 1.00f));
            style.SetColor(ColorTarget.FrameBgHovered, new Vector4(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.FrameBgActive, new Vector4(0.19f, 0.57f, 0.87f, 1.00f));
            style.SetColor(ColorTarget.TitleBg, new Vector4(0.20f, 0.25f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.TitleBgCollapsed, new Vector4(0.20f, 0.25f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.TitleBgActive, new Vector4(0.20f, 0.25f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.MenuBarBg, new Vector4(0.19f, 0.22f, 0.24f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarBg, new Vector4(0.00f, 0.00f, 0.00f, 0.31f));
            style.SetColor(ColorTarget.ScrollbarGrab, new Vector4(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarGrabHovered, new Vector4(0.26f, 0.30f, 0.34f, 1.00f));
            style.SetColor(ColorTarget.ScrollbarGrabActive, new Vector4(0.00f, 0.47f, 0.84f, 1.00f));
            //style.SetColor(ColorTarget.ComboBg, new Vector4(0.17f, 0.24f, 0.28f, 1.00f));
            style.SetColor(ColorTarget.CheckMark, new Vector4(1.00f, 0.69f, 0.00f, 0.98f));
            style.SetColor(ColorTarget.SliderGrab, new Vector4(0.16f, 0.18f, 0.20f, 1.00f));
            style.SetColor(ColorTarget.SliderGrabActive, new Vector4(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.Button, new Vector4(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.ButtonHovered, new Vector4(0.10f, 0.52f, 0.86f, 1.00f));
            style.SetColor(ColorTarget.ButtonActive, new Vector4(0.19f, 0.57f, 0.87f, 1.00f));
            style.SetColor(ColorTarget.Header, new Vector4(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.HeaderHovered, new Vector4(0.10f, 0.52f, 0.85f, 1.00f));
            style.SetColor(ColorTarget.HeaderActive, new Vector4(0.19f, 0.56f, 0.87f, 1.00f));
            style.SetColor(ColorTarget.Separator, new Vector4(0.24f, 0.26f, 0.29f, 1.00f));
            style.SetColor(ColorTarget.SeparatorHovered, new Vector4(0.31f, 0.33f, 0.36f, 1.00f));
            style.SetColor(ColorTarget.SeparatorActive, new Vector4(1.00f, 0.69f, 0.00f, 0.98f));
            style.SetColor(ColorTarget.ResizeGrip, new Vector4(0.39f, 0.39f, 0.39f, 0.71f));
            style.SetColor(ColorTarget.ResizeGripHovered, new Vector4(0.56f, 0.56f, 0.56f, 1.00f));
            style.SetColor(ColorTarget.ResizeGripActive, new Vector4(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.CloseButton, new Vector4(0.00f, 0.00f, 0.00f, 0.50f));
            style.SetColor(ColorTarget.CloseButtonHovered, new Vector4(0.00f, 0.00f, 0.00f, 0.25f));
            style.SetColor(ColorTarget.CloseButtonActive, new Vector4(1.00f, 0.22f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotLines, new Vector4(1.00f, 1.00f, 1.00f, 1.00f));
            style.SetColor(ColorTarget.PlotLinesHovered, new Vector4(0.90f, 0.70f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotHistogram, new Vector4(0.90f, 0.70f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.PlotHistogramHovered, new Vector4(1.00f, 0.16f, 0.00f, 1.00f));
            style.SetColor(ColorTarget.TextSelectedBg, new Vector4(0.00f, 0.47f, 0.84f, 1.00f));
            style.SetColor(ColorTarget.ModalWindowDarkening, new Vector4(0.00f, 0.00f, 0.00f, 0.50f));
        }

        protected override void OnDestroy()
        {
            foreach (var item in idToTex)
                Destroy(item.Value);

            Destroy(shader);
            Destroy(meshBuffer);

            //ImGui.Shutdown(); // crashes !!!!
        }

        string[] getEnumArray<T>()
        {
            var type = typeof(T);
            if (!enumMap.TryGetValue(type, out string[] r))
            {
                r = Enum.GetNames(type);
                enumMap.Add(type, r);
            }

            return r;
        }
    }
}