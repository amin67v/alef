using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Engine
{
    public sealed partial class Gui
    {
        unsafe readonly Style style = new Style(ImGui.igGetStyle());
        unsafe readonly IO io = new IO(ImGui.igGetIO());
        Array<float> indents = new Array<float>();

        public Style Style => style;

        public void PushID(string id)
        {
            ImGui.igPushIdStr(id);
        }

        public void PushID(int id)
        {
            ImGui.igPushIdInt(id);
        }

        public void PushIDRange(string idBegin, string idEnd)
        {
            ImGui.igPushIdStrRange(idBegin, idEnd);
        }

        public void PushItemWidth(float width)
        {
            ImGui.igPushItemWidth(width);
        }

        public void PopItemWidth()
        {
            ImGui.igPopItemWidth();
        }

        public void PopID()
        {
            ImGui.igPopId();
        }

        public uint GetID(string id)
        {
            return ImGui.igGetIdStr(id);
        }

        public uint GetID(string idBegin, string idEnd)
        {
            return ImGui.igGetIdStrRange(idBegin, idEnd);
        }

        public void Text(string message)
        {
            ImGui.igText(message);
        }

        public void Text(string message, Color color)
        {
            ImGui.igTextColored(color.ToVector4(), message);
        }

        public void TextDisabled(string text)
        {
            ImGui.igTextDisabled(text);
        }

        public void TextWrapped(string text)
        {
            ImGui.igTextWrapped(text);
        }

        public unsafe void TextUnformatted(string message)
        {
            fixed (byte* bytes = System.Text.Encoding.UTF8.GetBytes(message))
            {
                ImGui.igTextUnformatted(bytes, null);
            }
        }

        public void LabelText(string label, string text)
        {
            ImGui.igLabelText(label, text);
        }

        public void Bullet()
        {
            ImGui.igBullet();
        }

        public void BulletText(string text)
        {
            ImGui.igBulletText(text);
        }

        public bool InvisibleButton(string id) => InvisibleButton(id, Vector2.Zero);

        public bool InvisibleButton(string id, Vector2 size)
        {
            return ImGui.igInvisibleButton(id, size);
        }

        public void Image(SpriteSheetFrame frame)
        {
            Image(frame, frame.Rect.Size, Color.White, Color.Transparent);
        }

        public void Image(SpriteSheetFrame frame, Vector2 size, Color tint, Color border)
        {
            var id = frame.SpriteSheet.Texture.GetHashCode();
            id_tex_map[id] = frame.SpriteSheet.Texture;
            var uv0 = new Vector2(frame.Vertices[0].Texcoord.X, frame.Vertices[1].Texcoord.Y);
            var uv1 = new Vector2(frame.Vertices[1].Texcoord.X, frame.Vertices[0].Texcoord.Y);
            ImGui.igImage(new IntPtr(id), size, uv0, uv1, tint.ToVector4(), border.ToVector4());
        }

        public bool ImageButton(SpriteSheetFrame frame, int frame_padding = 4)
        {
            return ImageButton(frame, frame.Rect.Size, frame_padding, Color.Transparent, Color.White);
        }

        public bool ImageButton(SpriteSheetFrame frame, Vector2 size, int frame_padding, Color background, Color tint)
        {
            PushID(frame.GetHashCode());
            var id = frame.SpriteSheet.Texture.GetHashCode();
            id_tex_map[id] = frame.SpriteSheet.Texture;
            var uv0 = new Vector2(frame.Vertices[0].Texcoord.X, frame.Vertices[1].Texcoord.Y);
            var uv1 = new Vector2(frame.Vertices[1].Texcoord.X, frame.Vertices[0].Texcoord.Y);
            var r = ImGui.igImageButton(new IntPtr(id), size, uv0, uv1, frame_padding, background.ToVector4(), tint.ToVector4());
            PopID();
            return r;
        }

        public bool CollapsingHeader(string label, TreeNodeFlags flags)
        {
            return ImGui.igCollapsingHeader(label, flags);
        }

        public bool Checkbox(string label, ref bool value)
        {
            return ImGui.igCheckbox(label, ref value);
        }

        public unsafe bool RadioButton(string label, ref int target, int buttonValue)
        {
            int targetCopy = target;
            bool result = ImGui.igRadioButton(label, &targetCopy, buttonValue);
            target = targetCopy;
            return result;
        }

        public bool RadioButtonBool(string label, bool active)
        {
            return ImGui.igRadioButtonBool(label, active);
        }

        public bool Combo<T>(string label, ref T value, int item_height = 5)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new Exception("The type of 'T' is not enum, only enum type is accepted in this context.");

            var arr = get_enum_arr<T>();

            int index = Array.IndexOf<string>(arr, value.ToString());
            var r = Combo(label, ref index, arr, item_height);
            if (r)
                value = (T)Enum.Parse(type, arr[index]);

            return r;
        }

        public unsafe bool Combo(string label, ref int current_item, string[] items)
        {
            return ImGui.igCombo(label, ref current_item, items, items.Length, 5);
        }

        public unsafe bool Combo(string label, ref int current_item, string[] items, int heightInItems)
        {
            return ImGui.igCombo(label, ref current_item, items, items.Length, heightInItems);
        }

        public bool ColorButton(string desc_id, Color color, ColorEditFlags flags, Vector2 size)
        {
            return ImGui.igColorButton(desc_id, color.ToVector4(), flags, size);
        }

        public unsafe bool ColorEdit(string label, ref Color color, ColorEditFlags flags = ColorEditFlags.Default)
        {
            Vector4 vec4 = color.ToVector4();
            bool result = ImGui.igColorEdit4(label, &vec4, flags);
            if (result)
                color = new Color(vec4);

            return result;
        }

        public unsafe bool ColorPicker(string label, ref Color color, ColorEditFlags flags = ColorEditFlags.Default)
        {
            Vector4 vec4 = color.ToVector4();
            bool result = ImGui.igColorPicker4(label, &vec4, flags);
            if (result)
                color = new Color(vec4);

            return result;
        }

        public unsafe void PlotLines(
            string label,
            float[] values,
            int valuesOffset,
            string overlayText,
            float scaleMin,
            float scaleMax,
            Vector2 graphSize,
            int stride)
        {
            fixed (float* valuesBasePtr = values)
            {
                ImGui.igPlotLines(
                    label,
                    valuesBasePtr,
                    values.Length,
                    valuesOffset,
                    overlayText,
                    scaleMin,
                    scaleMax,
                    graphSize,
                    stride);
            }
        }

        public unsafe void PlotHistogram(string label, float[] values, int valuesOffset, string overlayText, float scaleMin, float scaleMax, Vector2 graphSize, int stride)
        {
            fixed (float* valuesBasePtr = values)
            {
                ImGui.igPlotHistogram(
                    label,
                    valuesBasePtr,
                    values.Length,
                    valuesOffset,
                    overlayText,
                    scaleMin,
                    scaleMax,
                    graphSize,
                    stride);
            }
        }

        public bool SliderFloat(string sliderLabel, ref float value, float min, float max, string displayText, float power)
        {
            return ImGui.igSliderFloat(sliderLabel, ref value, min, max, displayText, power);
        }

        public bool SliderVector2(string label, ref Vector2 value, float min, float max, string displayText, float power)
        {
            return ImGui.igSliderFloat2(label, ref value, min, max, displayText, power);
        }

        public bool SliderVector3(string label, ref Vector3 value, float min, float max, string displayText, float power)
        {
            return ImGui.igSliderFloat3(label, ref value, min, max, displayText, power);
        }

        public bool SliderVector4(string label, ref Vector4 value, float min, float max, string displayText, float power)
        {
            return ImGui.igSliderFloat4(label, ref value, min, max, displayText, power);
        }

        public bool SliderAngle(string label, ref float radians, float minDegrees, float maxDegrees)
        {
            return ImGui.igSliderAngle(label, ref radians, minDegrees, maxDegrees);
        }

        public bool InputInt(string label, ref int v, int step, int step_fast, InputTextFlags extra_flags)
        {
            return ImGui.igInputInt(label, ref v, step, step_fast, extra_flags);
        }

        public bool SliderInt(string sliderLabel, ref int value, int min, int max, string displayText)
        {
            return ImGui.igSliderInt(sliderLabel, ref value, min, max, displayText);
        }

        public bool SliderInt2(string label, ref Int2 value, int min, int max, string displayText)
        {
            return ImGui.igSliderInt2(label, ref value, min, max, displayText);
        }

        public bool SliderInt3(string label, ref Int3 value, int min, int max, string displayText)
        {
            return ImGui.igSliderInt3(label, ref value, min, max, displayText);
        }

        public bool SliderInt4(string label, ref Int4 value, int min, int max, string displayText)
        {
            return ImGui.igSliderInt4(label, ref value, min, max, displayText);
        }

        public bool InputFloat(string label, ref float v, float step, float step_fast, int decimal_precision, InputTextFlags extra_flags)
        {
            return ImGui.igInputFloat(label, ref v, step, step_fast, decimal_precision, extra_flags);
        }

        public bool InputVector2(string label, ref Vector2 v, int decimal_precision, InputTextFlags extra_flags)
        {
            return ImGui.igInputFloat2(label, ref v, decimal_precision, extra_flags);
        }

        public bool InputVector3(string label, ref Vector3 v, int decimal_precision, InputTextFlags extra_flags)
        {
            return ImGui.igInputFloat3(label, ref v, decimal_precision, extra_flags);
        }

        public bool InputFloat4(string label, ref Vector4 v, int decimal_precision, InputTextFlags extra_flags)
        {
            return ImGui.igInputFloat4(label, ref v, decimal_precision, extra_flags);
        }

        public bool DragFloat(string label, ref float value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGui.igDragFloat(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
        }

        public bool DragVector2(string label, ref Vector2 value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGui.igDragFloat2(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
        }

        public bool DragVector3(string label, ref Vector3 value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGui.igDragFloat3(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
        }

        public bool DragVector4(string label, ref Vector4 value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGui.igDragFloat4(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
        }

        public bool DragFloatRange2(
            string label,
            ref float currentMinValue,
            ref float currentMaxValue,
            float speed = 1.0f,
            float minValueLimit = 0.0f,
            float maxValueLimit = 0.0f,
            string displayFormat = "%.3f",
            string displayFormatMax = null,
            float power = 1.0f)
        {
            return ImGui.igDragFloatRange2(label, ref currentMinValue, ref currentMaxValue, speed, minValueLimit, maxValueLimit, displayFormat, displayFormatMax, power);
        }

        public bool DragInt(string label, ref int value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGui.igDragInt(label, ref value, speed, minValue, maxValue, displayText);
        }

        public bool DragInt2(string label, ref Int2 value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGui.igDragInt2(label, ref value, speed, minValue, maxValue, displayText);
        }

        public bool DragInt3(string label, ref Int3 value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGui.igDragInt3(label, ref value, speed, minValue, maxValue, displayText);
        }

        public bool DragInt4(string label, ref Int4 value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGui.igDragInt4(label, ref value, speed, minValue, maxValue, displayText);
        }

        public bool DragIntRange2(
            string label,
            ref int currentMinValue,
            ref int currentMaxValue,
            float speed = 1.0f,
            int minLimit = 0,
            int maxLimit = 0,
            string displayFormat = "%.0f",
            string displayFormatMax = null)
        {
            return ImGui.igDragIntRange2(
                label,
                ref currentMinValue,
                ref currentMaxValue,
                speed,
                minLimit,
                maxLimit,
                displayFormat,
                displayFormatMax);
        }

        public bool Button(string message)
        {
            return ImGui.igButton(message, Vector2.Zero);
        }

        public bool Button(string message, Vector2 size)
        {
            return ImGui.igButton(message, size);
        }

        public bool Button(string message, Color c)
        {
            return Button(message, Vector2.Zero, c);
        }

        public bool Button(string message, Vector2 size, Color btn_color)
        {
            var text_color = btn_color.Brightness > .5f ? Color.Black : Color.White;
            return Button(message, size, btn_color, text_color);
        }

        public bool Button(string message, Vector2 size, Color btn_color, Color text_color)
        {
            PushStyleColor(ColorTarget.Button, btn_color);
            PushStyleColor(ColorTarget.ButtonHovered, btn_color.Lighter(0.25f));
            PushStyleColor(ColorTarget.ButtonActive, btn_color.Lighter(0.5f));
            PushStyleColor(ColorTarget.Text, text_color);
            var r = ImGui.igButton(message, size);
            PopStyleColor(4);

            return r;
        }

        public void SetNextWindowSize(Vector2 size, GuiCondition condition)
        {
            ImGui.igSetNextWindowSize(size, condition);
        }

        public void SetNextWindowFocus()
        {
            ImGui.igSetNextWindowFocus();
        }

        public void SetNextWindowPos(Vector2 position, GuiCondition condition)
        {
            ImGui.igSetNextWindowPos(position, condition);
        }

        public void SetNextWindowPosCenter(GuiCondition condition)
        {
            ImGui.igSetNextWindowPosCenter(condition);
        }

        unsafe void ScaleClipRects(DrawData* drawData, Vector2 scale)
        {
            for (int i = 0; i < drawData->CmdListsCount; i++)
            {
                NativeDrawList* cmd_list = drawData->CmdLists[i];
                for (int cmd_i = 0; cmd_i < cmd_list->CmdBuffer.Size; cmd_i++)
                {
                    DrawCmd* drawCmdList = (DrawCmd*)cmd_list->CmdBuffer.Data;
                    DrawCmd* cmd = &drawCmdList[cmd_i];
                    cmd->ClipRect = new Vector4(cmd->ClipRect.X * scale.X, cmd->ClipRect.Y * scale.Y, cmd->ClipRect.Z * scale.X, cmd->ClipRect.W * scale.Y);
                }
            }
        }

        public float GetWindowHeight()
        {
            return ImGui.igGetWindowHeight();
        }


        public float GetWindowWidth()
        {
            return ImGui.igGetWindowWidth();
        }

        public Vector2 GetWindowSize()
        {
            Vector2 size;
            ImGui.igGetWindowSize(out size);
            return size;
        }

        public Vector2 GetWindowPosition()
        {
            Vector2 pos;
            ImGui.igGetWindowPos(out pos);
            return pos;
        }


        public void SetWindowSize(Vector2 size, GuiCondition cond = 0)
        {
            ImGui.igSetWindowSize(size, cond);
        }

        public bool BeginWindow(string windowTitle) => BeginWindow(windowTitle, WindowFlags.Default);

        public bool BeginWindow(string windowTitle, WindowFlags flags)
        {
            return ImGui.igBegin(windowTitle, IntPtr.Zero, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, WindowFlags flags)
        {
            return ImGui.igBegin2(windowTitle, ref opened, new Vector2(), 1f, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, float backgroundAlpha, WindowFlags flags)
        {
            return ImGui.igBegin2(windowTitle, ref opened, new Vector2(), backgroundAlpha, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, Vector2 startingSize, WindowFlags flags)
        {
            return ImGui.igBegin2(windowTitle, ref opened, startingSize, 1f, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, Vector2 startingSize, float backgroundAlpha, WindowFlags flags)
        {
            return ImGui.igBegin2(windowTitle, ref opened, startingSize, backgroundAlpha, flags);
        }

        public bool BeginMenu(string label)
        {
            return ImGui.igBeginMenu(label, true);
        }

        public bool BeginMenu(string label, bool enabled)
        {
            return ImGui.igBeginMenu(label, enabled);
        }

        public bool BeginMenuBar()
        {
            return ImGui.igBeginMenuBar();
        }

        public void CloseCurrentPopup()
        {
            ImGui.igCloseCurrentPopup();
        }

        public void EndMenuBar()
        {
            ImGui.igEndMenuBar();
        }

        public void EndMenu()
        {
            ImGui.igEndMenu();
        }

        public void Separator()
        {
            ImGui.igSeparator();
        }

        public bool MenuItem(string label)
        {
            return MenuItem(label, string.Empty, false, true);
        }

        public bool MenuItem(string label, string shortcut)
        {
            return MenuItem(label, shortcut, false, true);
        }

        public bool MenuItem(string label, bool enabled)
        {
            return MenuItem(label, string.Empty, false, enabled);
        }

        public bool MenuItem(string label, string shortcut, bool selected, bool enabled)
        {
            return ImGui.igMenuItem(label, shortcut, selected, enabled);
        }

        public unsafe bool InputText(string label, InputBuffer value, InputTextFlags flags)
        {
            var hdl = GCHandle.Alloc(value.Buffer, GCHandleType.Pinned);
            var r = ImGui.igInputText(label, hdl.AddrOfPinnedObject(), (uint)value.Buffer.Length, flags, IntPtr.Zero, null);
            hdl.Free();
            return r;
        }

        public void EndWindow()
        {
            ImGui.igEnd();
        }

        public void PushStyleColor(ColorTarget target, Color color)
        {
            ImGui.igPushStyleColor(target, color.ToVector4());
        }

        public void PopStyleColor()
        {
            PopStyleColor(1);
        }

        public void PopStyleColor(int num)
        {
            ImGui.igPopStyleColor(num);
        }

        public void PushStyleVar(StyleVar var, float value) => ImGui.igPushStyleVar(var, value);

        public void PushStyleVar(StyleVar var, Vector2 value) => ImGui.igPushStyleVarVec(var, value);

        public void PopStyleVar() => ImGui.igPopStyleVar(1);

        public void PopStyleVar(int count) => ImGui.igPopStyleVar(count);

        unsafe void InputTextMultiline(string label, IntPtr textBuffer, uint bufferSize, Vector2 size, InputTextFlags flags, IntPtr callback)
        {
            ImGui.igInputTextMultiline(label, textBuffer, bufferSize, size, flags, callback, null);
        }

        unsafe void InputTextMultiline(string label, IntPtr textBuffer, uint bufferSize, Vector2 size, InputTextFlags flags, IntPtr callback, IntPtr userData)
        {
            ImGui.igInputTextMultiline(label, textBuffer, bufferSize, size, flags, callback, userData.ToPointer());
        }

        public bool BeginChildFrame(uint id, Vector2 size, WindowFlags flags)
        {
            return ImGui.igBeginChildFrame(id, size, flags);
        }

        public void EndChildFrame()
        {
            ImGui.igEndChildFrame();
        }

        unsafe void ColorConvertRGBToHSV(float r, float g, float b, out float h, out float s, out float v)
        {
            float h2, s2, v2;
            ImGui.igColorConvertRGBtoHSV(r, g, b, &h2, &s2, &v2);
            h = h2;
            s = s2;
            v = v2;
        }

        unsafe void ColorConvertHSVToRGB(float h, float s, float v, out float r, out float g, out float b)
        {
            float r2, g2, b2;
            ImGui.igColorConvertHSVtoRGB(h, s, v, &r2, &g2, &b2);
            r = r2;
            g = g2;
            b = b2;
        }

        public bool IsKeyDown(int keyIndex)
        {
            return ImGui.igIsKeyDown(keyIndex);
        }

        public bool IsKeyPressed(int keyIndex, bool repeat = true)
        {
            return ImGui.igIsKeyPressed(keyIndex, repeat);
        }

        public bool IsKeyReleased(int keyIndex)
        {
            return ImGui.igIsKeyReleased(keyIndex);
        }

        public bool IsMouseDown(int button)
        {
            return ImGui.igIsMouseDown(button);
        }

        public bool IsMouseClicked(int button, bool repeat = false)
        {
            return ImGui.igIsMouseClicked(button, repeat);
        }

        public bool IsMouseDoubleClicked(int button)
        {
            return ImGui.igIsMouseDoubleClicked(button);
        }

        public bool IsMouseReleased(int button)
        {
            return ImGui.igIsMouseReleased(button);
        }

        public bool IsWindowRectHovered()
        {
            return ImGui.igIsWindowRectHovered();
        }

        public bool IsAnyWindowHovered()
        {
            return ImGui.igIsAnyWindowHovered();
        }

        public bool IsWindowFocused()
        {
            return ImGui.igIsWindowFocused();
        }

        public bool IsMouseHoveringRect(Vector2 minPosition, Vector2 maxPosition, bool clip)
        {
            return ImGui.igIsMouseHoveringRect(minPosition, maxPosition, clip);
        }

        public bool IsMouseDragging(int button, float lockThreshold)
        {
            return ImGui.igIsMouseDragging(button, lockThreshold);
        }

        public Vector2 GetMousePos()
        {
            Vector2 retVal;
            ImGui.igGetMousePos(out retVal);
            return retVal;
        }

        public Vector2 GetMousePosOnOpeningCurrentPopup()
        {
            Vector2 retVal;
            ImGui.igGetMousePosOnOpeningCurrentPopup(out retVal);
            return retVal;
        }

        public Vector2 GetMouseDragDelta(int button, float lockThreshold)
        {
            Vector2 retVal;
            ImGui.igGetMouseDragDelta(out retVal, button, lockThreshold);
            return retVal;
        }

        public void ResetMouseDragDelta(int button)
        {
            ImGui.igResetMouseDragDelta(button);
        }

        public MouseCursorKind MouseCursor
        {
            get => ImGui.igGetMouseCursor();
            set => ImGui.igSetMouseCursor(value);
        }

        public Vector2 GetCursorStartPos()
        {
            Vector2 retVal;
            ImGui.igGetCursorStartPos(out retVal);
            return retVal;
        }

        public unsafe Vector2 GetCursorScreenPos()
        {
            Vector2 retVal;
            ImGui.igGetCursorScreenPos(&retVal);
            return retVal;
        }

        public void SetCursorScreenPos(Vector2 pos)
        {
            ImGui.igSetCursorScreenPos(pos);
        }

        public bool BeginChild(string id, bool border = false, WindowFlags flags = 0)
        {
            return BeginChild(id, new Vector2(0, 0), border, flags);
        }

        public bool BeginChild(string id, Vector2 size, bool border, WindowFlags flags)
        {
            return ImGui.igBeginChild(id, size, border, flags);
        }

        public bool BeginChild(uint id, Vector2 size, bool border, WindowFlags flags)
        {
            return ImGui.igBeginChildEx(id, size, border, flags);
        }

        public void EndChild()
        {
            ImGui.igEndChild();
        }

        public void PushIndent(float value = 16f)
        {
            indents.Push(value);
            ImGui.igIndent(value);
        }

        public void PopIndent() => ImGui.igUnindent(indents.Pop());

        public Vector2 GetContentRegionMax()
        {
            Vector2 value;
            ImGui.igGetContentRegionMax(out value);
            return value;
        }

        public Vector2 GetContentRegionAvailable()
        {
            Vector2 value;
            ImGui.igGetContentRegionAvail(out value);
            return value;
        }

        public float GetContentRegionAvailableWidth()
        {
            return ImGui.igGetContentRegionAvailWidth();
        }

        public Vector2 GetWindowContentRegionMin()
        {
            Vector2 value;
            ImGui.igGetWindowContentRegionMin(out value);
            return value;
        }

        public Vector2 GetWindowContentRegionMax()
        {
            Vector2 value;
            ImGui.igGetWindowContentRegionMax(out value);
            return value;
        }

        public float GetWindowContentRegionWidth()
        {
            return ImGui.igGetWindowContentRegionWidth();
        }

        public bool Selectable(string label)
        {
            return Selectable(label, false);
        }

        public bool Selectable(string label, bool isSelected)
        {
            return Selectable(label, isSelected, SelectableFlags.Default);
        }

        public bool BeginMainMenuBar()
        {
            return ImGui.igBeginMainMenuBar();
        }

        public bool BeginPopup(string id)
        {
            return ImGui.igBeginPopup(id);
        }

        public void EndMainMenuBar()
        {
            ImGui.igEndMainMenuBar();
        }

        public bool SmallButton(string label)
        {
            return ImGui.igSmallButton(label);
        }

        public bool BeginPopupModal(string name)
        {
            return BeginPopupModal(name, WindowFlags.Default);
        }

        public bool BeginPopupModal(string name, ref bool opened)
        {
            return BeginPopupModal(name, ref opened, WindowFlags.Default);
        }

        public unsafe bool BeginPopupModal(string name, WindowFlags extra_flags)
        {
            return ImGui.igBeginPopupModal(name, null, extra_flags);
        }

        public unsafe bool BeginPopupModal(string name, ref bool p_opened, WindowFlags extra_flags)
        {
            byte value = p_opened ? (byte)1 : (byte)0;
            bool result = ImGui.igBeginPopupModal(name, &value, extra_flags);

            p_opened = value == 1 ? true : false;
            return result;
        }

        public bool Selectable(string label, bool isSelected, SelectableFlags flags)
        {
            return Selectable(label, isSelected, flags, new Vector2());
        }

        public bool Selectable(string label, bool isSelected, SelectableFlags flags, Vector2 size)
        {
            return ImGui.igSelectable(label, isSelected, flags, size);
        }

        public bool SelectableEx(string label, ref bool isSelected)
        {
            return ImGui.igSelectableEx(label, ref isSelected, SelectableFlags.Default, new Vector2());
        }

        public bool SelectableEx(string label, ref bool isSelected, SelectableFlags flags, Vector2 size)
        {
            return ImGui.igSelectableEx(label, ref isSelected, flags, size);
        }

        public unsafe Vector2 GetTextSize(string text, float wrapWidth = Int32.MaxValue)
        {
            Vector2 result;
            IntPtr buffer = Marshal.StringToHGlobalAnsi(text);
            byte* textStart = (byte*)buffer.ToPointer();
            byte* textEnd = textStart + text.Length;
            ImGui.igCalcTextSize(out result, (char*)textStart, (char*)textEnd, false, wrapWidth);
            return result;
        }

        public bool BeginPopupContextItem(string id)
        {
            return BeginPopupContextItem(id, 1);
        }

        public bool BeginPopupContextItem(string id, int mouseButton)
        {
            return ImGui.igBeginPopupContextItem(id, mouseButton);
        }

        public unsafe void Dummy(float width, float height)
        {
            Dummy(new Vector2(width, height));
        }

        public void EndPopup()
        {
            ImGui.igEndPopup();
        }

        public bool IsPopupOpen(string id)
        {
            return ImGui.igIsPopupOpen(id);
        }

        public unsafe void Dummy(Vector2 size)
        {
            ImGui.igDummy(&size);
        }

        public void Spacing()
        {
            ImGui.igSpacing();
        }

        public void Columns(int count, string id, bool border)
        {
            ImGui.igColumns(count, id, border);
        }

        public void NextColumn()
        {
            ImGui.igNextColumn();
        }

        public int GetColumnIndex()
        {
            return ImGui.igGetColumnIndex();
        }

        public float GetColumnOffset(int columnIndex)
        {
            return ImGui.igGetColumnOffset(columnIndex);
        }

        public void SetColumnOffset(int columnIndex, float offsetX)
        {
            ImGui.igSetColumnOffset(columnIndex, offsetX);
        }

        public float GetColumnWidth(int columnIndex)
        {
            return ImGui.igGetColumnWidth(columnIndex);
        }

        public void SetColumnWidth(int columnIndex, float width)
        {
            ImGui.igSetColumnWidth(columnIndex, width);
        }

        public int GetColumnsCount()
        {
            return ImGui.igGetColumnsCount();
        }

        public void OpenPopup(string id)
        {
            ImGui.igOpenPopup(id);
        }

        public void SameLine(float localPositionX = 0, float spacingW = -1.0f)
        {
            ImGui.igSameLine(localPositionX, spacingW);
        }

        public void PushClipRect(Vector2 min, Vector2 max, bool intersectWithCurrentCliRect)
        {
            ImGui.igPushClipRect(min, max, intersectWithCurrentCliRect ? (byte)1 : (byte)0);
        }

        public void PopClipRect()
        {
            ImGui.igPopClipRect();
        }

        public bool IsLastItemHovered()
        {
            return ImGui.igIsItemHovered();
        }

        public bool IsItemRectHovered()
        {
            return ImGui.igIsItemRectHovered();
        }

        public bool IsLastItemActive()
        {
            return ImGui.igIsItemActive();
        }

        public bool IsLastItemVisible()
        {
            return ImGui.igIsItemVisible();
        }

        public bool IsAnyItemHovered()
        {
            return ImGui.igIsAnyItemHovered();
        }

        public bool IsAnyItemActive()
        {
            return ImGui.igIsAnyItemActive();
        }

        public void SetTooltip(string text)
        {
            ImGui.igSetTooltip(text);
        }

        public void SetNextTreeNodeOpen(bool opened)
        {
            ImGui.igSetNextTreeNodeOpen(opened, GuiCondition.Always);
        }

        public void SetNextTreeNodeOpen(bool opened, GuiCondition setCondition)
        {
            ImGui.igSetNextTreeNodeOpen(opened, setCondition);
        }

        public bool TreeNode(string label)
        {
            return ImGui.igTreeNode(label);
        }

        public bool TreeNodeEx(string label, TreeNodeFlags flags = 0)
        {
            return ImGui.igTreeNodeEx(label, flags);
        }

        public void TreePop()
        {
            ImGui.igTreePop();
        }

        public Vector2 GetLastItemRectSize()
        {
            Vector2 result;
            ImGui.igGetItemRectSize(out result);
            return result;
        }

        public Vector2 GetLastItemRectMin()
        {
            Vector2 result;
            ImGui.igGetItemRectMin(out result);
            return result;
        }

        public Vector2 GetLastItemRectMax()
        {
            Vector2 result;
            ImGui.igGetItemRectMax(out result);
            return result;
        }

        public void SetWindowFontScale(float scale)
        {
            ImGui.igSetWindowFontScale(scale);
        }

        public void SetScrollHere()
        {
            ImGui.igSetScrollHere();
        }

        public void SetScrollHere(float centerYRatio)
        {
            ImGui.igSetScrollHere(centerYRatio);
        }

        public void SetKeyboardFocusHere()
        {
            ImGui.igSetKeyboardFocusHere(0);
        }

        public void SetKeyboardFocusHere(int offset)
        {
            ImGui.igSetKeyboardFocusHere(offset);
        }

        public void CalcListClipping(int itemsCount, float itemsHeight, ref int outItemsDisplayStart, ref int outItemsDisplayEnd)
        {
            ImGui.igCalcListClipping(itemsCount, itemsHeight, ref outItemsDisplayStart, ref outItemsDisplayEnd);
        }

        unsafe void PushFont(ImFont font)
        {
            ImGui.igPushFont(font.NativeFont);
        }

        void PopFont()
        {
            ImGui.igPopFont();
        }
    }
}
