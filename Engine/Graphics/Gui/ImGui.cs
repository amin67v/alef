using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Engine
{
    public sealed partial class Gui
    {
        unsafe readonly Style style = new Style(ImGuiNative.igGetStyle());
        unsafe readonly IO io = new IO(ImGuiNative.igGetIO());
        Array<float> indents = new Array<float>();

        public Style Style => style;

        public void PushID(string id)
        {
            ImGuiNative.igPushIdStr(id);
        }

        public void PushID(int id)
        {
            ImGuiNative.igPushIdInt(id);
        }

        public void PushIDRange(string idBegin, string idEnd)
        {
            ImGuiNative.igPushIdStrRange(idBegin, idEnd);
        }

        public void PushItemWidth(float width)
        {
            ImGuiNative.igPushItemWidth(width);
        }

        public void PopItemWidth()
        {
            ImGuiNative.igPopItemWidth();
        }

        public void PopID()
        {
            ImGuiNative.igPopId();
        }

        public uint GetID(string id)
        {
            return ImGuiNative.igGetIdStr(id);
        }

        public uint GetID(string idBegin, string idEnd)
        {
            return ImGuiNative.igGetIdStrRange(idBegin, idEnd);
        }

        public void Text(string message)
        {
            ImGuiNative.igText(message);
        }

        public void Text(string message, Color color)
        {
            ImGuiNative.igTextColored(color.ToVector4(), message);
        }

        public void TextDisabled(string text)
        {
            ImGuiNative.igTextDisabled(text);
        }

        public void TextWrapped(string text)
        {
            ImGuiNative.igTextWrapped(text);
        }

        public unsafe void TextUnformatted(string message)
        {
            fixed (byte* bytes = System.Text.Encoding.UTF8.GetBytes(message))
            {
                ImGuiNative.igTextUnformatted(bytes, null);
            }
        }

        public void LabelText(string label, string text)
        {
            ImGuiNative.igLabelText(label, text);
        }

        public void Bullet()
        {
            ImGuiNative.igBullet();
        }

        public void BulletText(string text)
        {
            ImGuiNative.igBulletText(text);
        }

        public bool InvisibleButton(string id) => InvisibleButton(id, Vector2.Zero);

        public bool InvisibleButton(string id, Vector2 size)
        {
            return ImGuiNative.igInvisibleButton(id, size);
        }

        public void Image(IntPtr userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor)
        {
            ImGuiNative.igImage(userTextureID, size, uv0, uv1, tintColor.ToVector4(), borderColor.ToVector4());
        }

        public bool ImageButton(
            IntPtr userTextureID,
            Vector2 size,
            Vector2 uv0,
            Vector2 uv1,
            int framePadding,
            Color backgroundColor,
            Color tintColor)
        {
            return ImGuiNative.igImageButton(userTextureID, size, uv0, uv1, framePadding, backgroundColor.ToVector4(), tintColor.ToVector4());
        }

        public bool CollapsingHeader(string label, TreeNodeFlags flags)
        {
            return ImGuiNative.igCollapsingHeader(label, flags);
        }

        public bool Checkbox(string label, ref bool value)
        {
            return ImGuiNative.igCheckbox(label, ref value);
        }

        public unsafe bool RadioButton(string label, ref int target, int buttonValue)
        {
            int targetCopy = target;
            bool result = ImGuiNative.igRadioButton(label, &targetCopy, buttonValue);
            target = targetCopy;
            return result;
        }

        public bool RadioButtonBool(string label, bool active)
        {
            return ImGuiNative.igRadioButtonBool(label, active);
        }

        public unsafe bool Combo(string label, ref int current_item, string[] items)
        {
            return ImGuiNative.igCombo(label, ref current_item, items, items.Length, 5);
        }

        public unsafe bool Combo(string label, ref int current_item, string[] items, int heightInItems)
        {
            return ImGuiNative.igCombo(label, ref current_item, items, items.Length, heightInItems);
        }

        public bool ColorButton(string desc_id, Color color, ColorEditFlags flags, Vector2 size)
        {
            return ImGuiNative.igColorButton(desc_id, color.ToVector4(), flags, size);
        }

        public unsafe bool ColorEdit(string label, ref Color color, ColorEditFlags flags = ColorEditFlags.Default)
        {
            Vector4 vec4 = color.ToVector4();
            bool result = ImGuiNative.igColorEdit4(label, &vec4, flags);
            if (result)
                color = new Color(vec4);

            return result;
        }

        public unsafe bool ColorPicker(string label, ref Color color, ColorEditFlags flags = ColorEditFlags.Default)
        {
            Vector4 vec4 = color.ToVector4();
            bool result = ImGuiNative.igColorPicker4(label, &vec4, flags);
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
                ImGuiNative.igPlotLines(
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
                ImGuiNative.igPlotHistogram(
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
            return ImGuiNative.igSliderFloat(sliderLabel, ref value, min, max, displayText, power);
        }

        public bool SliderVector2(string label, ref Vector2 value, float min, float max, string displayText, float power)
        {
            return ImGuiNative.igSliderFloat2(label, ref value, min, max, displayText, power);
        }

        public bool SliderVector3(string label, ref Vector3 value, float min, float max, string displayText, float power)
        {
            return ImGuiNative.igSliderFloat3(label, ref value, min, max, displayText, power);
        }

        public bool SliderVector4(string label, ref Vector4 value, float min, float max, string displayText, float power)
        {
            return ImGuiNative.igSliderFloat4(label, ref value, min, max, displayText, power);
        }

        public bool SliderAngle(string label, ref float radians, float minDegrees, float maxDegrees)
        {
            return ImGuiNative.igSliderAngle(label, ref radians, minDegrees, maxDegrees);
        }

        public bool SliderInt(string sliderLabel, ref int value, int min, int max, string displayText)
        {
            return ImGuiNative.igSliderInt(sliderLabel, ref value, min, max, displayText);
        }

        public bool SliderInt2(string label, ref Int2 value, int min, int max, string displayText)
        {
            return ImGuiNative.igSliderInt2(label, ref value, min, max, displayText);
        }

        public bool SliderInt3(string label, ref Int3 value, int min, int max, string displayText)
        {
            return ImGuiNative.igSliderInt3(label, ref value, min, max, displayText);
        }

        public bool SliderInt4(string label, ref Int4 value, int min, int max, string displayText)
        {
            return ImGuiNative.igSliderInt4(label, ref value, min, max, displayText);
        }

        public bool DragFloat(string label, ref float value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGuiNative.igDragFloat(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
        }

        public bool DragVector2(string label, ref Vector2 value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGuiNative.igDragFloat2(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
        }

        public bool DragVector3(string label, ref Vector3 value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGuiNative.igDragFloat3(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
        }

        public bool DragVector4(string label, ref Vector4 value, float min, float max, float dragSpeed = 1f, string displayFormat = "%.3f", float dragPower = 1f)
        {
            return ImGuiNative.igDragFloat4(label, ref value, dragSpeed, min, max, displayFormat, dragPower);
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
            return ImGuiNative.igDragFloatRange2(label, ref currentMinValue, ref currentMaxValue, speed, minValueLimit, maxValueLimit, displayFormat, displayFormatMax, power);
        }

        public bool DragInt(string label, ref int value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGuiNative.igDragInt(label, ref value, speed, minValue, maxValue, displayText);
        }

        public bool DragInt2(string label, ref Int2 value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGuiNative.igDragInt2(label, ref value, speed, minValue, maxValue, displayText);
        }

        public bool DragInt3(string label, ref Int3 value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGuiNative.igDragInt3(label, ref value, speed, minValue, maxValue, displayText);
        }

        public bool DragInt4(string label, ref Int4 value, float speed, int minValue, int maxValue, string displayText)
        {
            return ImGuiNative.igDragInt4(label, ref value, speed, minValue, maxValue, displayText);
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
            return ImGuiNative.igDragIntRange2(
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
            return ImGuiNative.igButton(message, Vector2.Zero);
        }

        public bool Button(string message, Vector2 size)
        {
            return ImGuiNative.igButton(message, size);
        }

        public void SetNextWindowSize(Vector2 size, GuiCondition condition)
        {
            ImGuiNative.igSetNextWindowSize(size, condition);
        }

        public void SetNextWindowFocus()
        {
            ImGuiNative.igSetNextWindowFocus();
        }

        public void SetNextWindowPos(Vector2 position, GuiCondition condition)
        {
            ImGuiNative.igSetNextWindowPos(position, condition);
        }

        public void SetNextWindowPosCenter(GuiCondition condition)
        {
            ImGuiNative.igSetNextWindowPosCenter(condition);
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
            return ImGuiNative.igGetWindowHeight();
        }


        public float GetWindowWidth()
        {
            return ImGuiNative.igGetWindowWidth();
        }

        public Vector2 GetWindowSize()
        {
            Vector2 size;
            ImGuiNative.igGetWindowSize(out size);
            return size;
        }

        public Vector2 GetWindowPosition()
        {
            Vector2 pos;
            ImGuiNative.igGetWindowPos(out pos);
            return pos;
        }


        public void SetWindowSize(Vector2 size, GuiCondition cond = 0)
        {
            ImGuiNative.igSetWindowSize(size, cond);
        }

        public bool BeginWindow(string windowTitle) => BeginWindow(windowTitle, WindowFlags.Default);

        public bool BeginWindow(string windowTitle, WindowFlags flags)
        {
            return ImGuiNative.igBegin(windowTitle, IntPtr.Zero, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, WindowFlags flags)
        {
            return ImGuiNative.igBegin2(windowTitle, ref opened, new Vector2(), 1f, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, float backgroundAlpha, WindowFlags flags)
        {
            return ImGuiNative.igBegin2(windowTitle, ref opened, new Vector2(), backgroundAlpha, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, Vector2 startingSize, WindowFlags flags)
        {
            return ImGuiNative.igBegin2(windowTitle, ref opened, startingSize, 1f, flags);
        }

        public bool BeginWindow(string windowTitle, ref bool opened, Vector2 startingSize, float backgroundAlpha, WindowFlags flags)
        {
            return ImGuiNative.igBegin2(windowTitle, ref opened, startingSize, backgroundAlpha, flags);
        }

        public bool BeginMenu(string label)
        {
            return ImGuiNative.igBeginMenu(label, true);
        }

        public bool BeginMenu(string label, bool enabled)
        {
            return ImGuiNative.igBeginMenu(label, enabled);
        }

        public bool BeginMenuBar()
        {
            return ImGuiNative.igBeginMenuBar();
        }

        public void CloseCurrentPopup()
        {
            ImGuiNative.igCloseCurrentPopup();
        }

        public void EndMenuBar()
        {
            ImGuiNative.igEndMenuBar();
        }

        public void EndMenu()
        {
            ImGuiNative.igEndMenu();
        }

        public void Separator()
        {
            ImGuiNative.igSeparator();
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
            return ImGuiNative.igMenuItem(label, shortcut, selected, enabled);
        }

        public unsafe bool InputText(string label, GuiString str, InputTextFlags flags)
        {
            var hdl = GCHandle.Alloc(str.Buffer, GCHandleType.Pinned);
            var r = ImGuiNative.igInputText(label, hdl.AddrOfPinnedObject(), (uint)str.Buffer.Length, flags, null, null);
            hdl.Free();
            return r;
        }

        public void EndWindow()
        {
            ImGuiNative.igEnd();
        }

        public void PushStyleColor(ColorTarget target, Color color)
        {
            ImGuiNative.igPushStyleColor(target, color.ToVector4());
        }

        public void PopStyleColor()
        {
            PopStyleColor(1);
        }

        public void PopStyleColor(int num)
        {
            ImGuiNative.igPopStyleColor(num);
        }

        public void PushStyleVar(StyleVar var, float value) => ImGuiNative.igPushStyleVar(var, value);
        public void PushStyleVar(StyleVar var, Vector2 value) => ImGuiNative.igPushStyleVarVec(var, value);

        public void PopStyleVar() => ImGuiNative.igPopStyleVar(1);
        public void PopStyleVar(int count) => ImGuiNative.igPopStyleVar(count);

        unsafe void InputTextMultiline(string label, IntPtr textBuffer, uint bufferSize, Vector2 size, InputTextFlags flags, TextEditCallback callback)
        {
            ImGuiNative.igInputTextMultiline(label, textBuffer, bufferSize, size, flags, callback, null);
        }

        unsafe void InputTextMultiline(string label, IntPtr textBuffer, uint bufferSize, Vector2 size, InputTextFlags flags, TextEditCallback callback, IntPtr userData)
        {
            ImGuiNative.igInputTextMultiline(label, textBuffer, bufferSize, size, flags, callback, userData.ToPointer());
        }

        public bool BeginChildFrame(uint id, Vector2 size, WindowFlags flags)
        {
            return ImGuiNative.igBeginChildFrame(id, size, flags);
        }

        public void EndChildFrame()
        {
            ImGuiNative.igEndChildFrame();
        }

        unsafe void ColorConvertRGBToHSV(float r, float g, float b, out float h, out float s, out float v)
        {
            float h2, s2, v2;
            ImGuiNative.igColorConvertRGBtoHSV(r, g, b, &h2, &s2, &v2);
            h = h2;
            s = s2;
            v = v2;
        }

        unsafe void ColorConvertHSVToRGB(float h, float s, float v, out float r, out float g, out float b)
        {
            float r2, g2, b2;
            ImGuiNative.igColorConvertHSVtoRGB(h, s, v, &r2, &g2, &b2);
            r = r2;
            g = g2;
            b = b2;
        }

        public bool IsKeyDown(int keyIndex)
        {
            return ImGuiNative.igIsKeyDown(keyIndex);
        }

        public bool IsKeyPressed(int keyIndex, bool repeat = true)
        {
            return ImGuiNative.igIsKeyPressed(keyIndex, repeat);
        }

        public bool IsKeyReleased(int keyIndex)
        {
            return ImGuiNative.igIsKeyReleased(keyIndex);
        }

        public bool IsMouseDown(int button)
        {
            return ImGuiNative.igIsMouseDown(button);
        }

        public bool IsMouseClicked(int button, bool repeat = false)
        {
            return ImGuiNative.igIsMouseClicked(button, repeat);
        }

        public bool IsMouseDoubleClicked(int button)
        {
            return ImGuiNative.igIsMouseDoubleClicked(button);
        }

        public bool IsMouseReleased(int button)
        {
            return ImGuiNative.igIsMouseReleased(button);
        }

        public bool IsWindowRectHovered()
        {
            return ImGuiNative.igIsWindowRectHovered();
        }

        public bool IsAnyWindowHovered()
        {
            return ImGuiNative.igIsAnyWindowHovered();
        }

        public bool IsWindowFocused()
        {
            return ImGuiNative.igIsWindowFocused();
        }

        public bool IsMouseHoveringRect(Vector2 minPosition, Vector2 maxPosition, bool clip)
        {
            return ImGuiNative.igIsMouseHoveringRect(minPosition, maxPosition, clip);
        }

        public bool IsMouseDragging(int button, float lockThreshold)
        {
            return ImGuiNative.igIsMouseDragging(button, lockThreshold);
        }

        public Vector2 GetMousePos()
        {
            Vector2 retVal;
            ImGuiNative.igGetMousePos(out retVal);
            return retVal;
        }

        public Vector2 GetMousePosOnOpeningCurrentPopup()
        {
            Vector2 retVal;
            ImGuiNative.igGetMousePosOnOpeningCurrentPopup(out retVal);
            return retVal;
        }

        public Vector2 GetMouseDragDelta(int button, float lockThreshold)
        {
            Vector2 retVal;
            ImGuiNative.igGetMouseDragDelta(out retVal, button, lockThreshold);
            return retVal;
        }

        public void ResetMouseDragDelta(int button)
        {
            ImGuiNative.igResetMouseDragDelta(button);
        }

        MouseCursorKind MouseCursor
        {
            get
            {
                return ImGuiNative.igGetMouseCursor();
            }
            set
            {
                ImGuiNative.igSetMouseCursor(value);
            }
        }

        public Vector2 GetCursorStartPos()
        {
            Vector2 retVal;
            ImGuiNative.igGetCursorStartPos(out retVal);
            return retVal;
        }

        public unsafe Vector2 GetCursorScreenPos()
        {
            Vector2 retVal;
            ImGuiNative.igGetCursorScreenPos(&retVal);
            return retVal;
        }

        public void SetCursorScreenPos(Vector2 pos)
        {
            ImGuiNative.igSetCursorScreenPos(pos);
        }

        public bool BeginChild(string id, bool border = false, WindowFlags flags = 0)
        {
            return BeginChild(id, new Vector2(0, 0), border, flags);
        }

        public bool BeginChild(string id, Vector2 size, bool border, WindowFlags flags)
        {
            return ImGuiNative.igBeginChild(id, size, border, flags);
        }

        public bool BeginChild(uint id, Vector2 size, bool border, WindowFlags flags)
        {
            return ImGuiNative.igBeginChildEx(id, size, border, flags);
        }

        public void EndChild()
        {
            ImGuiNative.igEndChild();
        }

        public void PushIndent(float value = 16f)
        {
            indents.Push(value);
            ImGuiNative.igIndent(value);
        }

        public void PopIndent() => ImGuiNative.igUnindent(indents.Pop());

        public Vector2 GetContentRegionMax()
        {
            Vector2 value;
            ImGuiNative.igGetContentRegionMax(out value);
            return value;
        }

        public Vector2 GetContentRegionAvailable()
        {
            Vector2 value;
            ImGuiNative.igGetContentRegionAvail(out value);
            return value;
        }

        public float GetContentRegionAvailableWidth()
        {
            return ImGuiNative.igGetContentRegionAvailWidth();
        }

        public Vector2 GetWindowContentRegionMin()
        {
            Vector2 value;
            ImGuiNative.igGetWindowContentRegionMin(out value);
            return value;
        }

        public Vector2 GetWindowContentRegionMax()
        {
            Vector2 value;
            ImGuiNative.igGetWindowContentRegionMax(out value);
            return value;
        }

        public float GetWindowContentRegionWidth()
        {
            return ImGuiNative.igGetWindowContentRegionWidth();
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
            return ImGuiNative.igBeginMainMenuBar();
        }

        public bool BeginPopup(string id)
        {
            return ImGuiNative.igBeginPopup(id);
        }

        public void EndMainMenuBar()
        {
            ImGuiNative.igEndMainMenuBar();
        }

        public bool SmallButton(string label)
        {
            return ImGuiNative.igSmallButton(label);
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
            return ImGuiNative.igBeginPopupModal(name, null, extra_flags);
        }

        public unsafe bool BeginPopupModal(string name, ref bool p_opened, WindowFlags extra_flags)
        {
            byte value = p_opened ? (byte)1 : (byte)0;
            bool result = ImGuiNative.igBeginPopupModal(name, &value, extra_flags);

            p_opened = value == 1 ? true : false;
            return result;
        }

        public bool Selectable(string label, bool isSelected, SelectableFlags flags)
        {
            return Selectable(label, isSelected, flags, new Vector2());
        }

        public bool Selectable(string label, bool isSelected, SelectableFlags flags, Vector2 size)
        {
            return ImGuiNative.igSelectable(label, isSelected, flags, size);
        }

        public bool SelectableEx(string label, ref bool isSelected)
        {
            return ImGuiNative.igSelectableEx(label, ref isSelected, SelectableFlags.Default, new Vector2());
        }

        public bool SelectableEx(string label, ref bool isSelected, SelectableFlags flags, Vector2 size)
        {
            return ImGuiNative.igSelectableEx(label, ref isSelected, flags, size);
        }

        public unsafe Vector2 GetTextSize(string text, float wrapWidth = Int32.MaxValue)
        {
            Vector2 result;
            IntPtr buffer = Marshal.StringToHGlobalAnsi(text);
            byte* textStart = (byte*)buffer.ToPointer();
            byte* textEnd = textStart + text.Length;
            ImGuiNative.igCalcTextSize(out result, (char*)textStart, (char*)textEnd, false, wrapWidth);
            return result;
        }

        public bool BeginPopupContextItem(string id)
        {
            return BeginPopupContextItem(id, 1);
        }

        public bool BeginPopupContextItem(string id, int mouseButton)
        {
            return ImGuiNative.igBeginPopupContextItem(id, mouseButton);
        }

        public unsafe void Dummy(float width, float height)
        {
            Dummy(new Vector2(width, height));
        }

        public void EndPopup()
        {
            ImGuiNative.igEndPopup();
        }

        public bool IsPopupOpen(string id)
        {
            return ImGuiNative.igIsPopupOpen(id);
        }

        public unsafe void Dummy(Vector2 size)
        {
            ImGuiNative.igDummy(&size);
        }

        public void Spacing()
        {
            ImGuiNative.igSpacing();
        }

        public void Columns(int count, string id, bool border)
        {
            ImGuiNative.igColumns(count, id, border);
        }

        public void NextColumn()
        {
            ImGuiNative.igNextColumn();
        }

        public int GetColumnIndex()
        {
            return ImGuiNative.igGetColumnIndex();
        }

        public float GetColumnOffset(int columnIndex)
        {
            return ImGuiNative.igGetColumnOffset(columnIndex);
        }

        public void SetColumnOffset(int columnIndex, float offsetX)
        {
            ImGuiNative.igSetColumnOffset(columnIndex, offsetX);
        }

        public float GetColumnWidth(int columnIndex)
        {
            return ImGuiNative.igGetColumnWidth(columnIndex);
        }

        public void SetColumnWidth(int columnIndex, float width)
        {
            ImGuiNative.igSetColumnWidth(columnIndex, width);
        }

        public int GetColumnsCount()
        {
            return ImGuiNative.igGetColumnsCount();
        }

        public void OpenPopup(string id)
        {
            ImGuiNative.igOpenPopup(id);
        }

        public void SameLine(float localPositionX = 0, float spacingW = -1.0f)
        {
            ImGuiNative.igSameLine(localPositionX, spacingW);
        }

        public void PushClipRect(Vector2 min, Vector2 max, bool intersectWithCurrentCliRect)
        {
            ImGuiNative.igPushClipRect(min, max, intersectWithCurrentCliRect ? (byte)1 : (byte)0);
        }

        public void PopClipRect()
        {
            ImGuiNative.igPopClipRect();
        }

        public bool IsLastItemHovered()
        {
            return ImGuiNative.igIsItemHovered();
        }

        public bool IsItemRectHovered()
        {
            return ImGuiNative.igIsItemRectHovered();
        }

        public bool IsLastItemActive()
        {
            return ImGuiNative.igIsItemActive();
        }

        public bool IsLastItemVisible()
        {
            return ImGuiNative.igIsItemVisible();
        }

        public bool IsAnyItemHovered()
        {
            return ImGuiNative.igIsAnyItemHovered();
        }

        public bool IsAnyItemActive()
        {
            return ImGuiNative.igIsAnyItemActive();
        }

        public void SetTooltip(string text)
        {
            ImGuiNative.igSetTooltip(text);
        }

        public void SetNextTreeNodeOpen(bool opened)
        {
            ImGuiNative.igSetNextTreeNodeOpen(opened, GuiCondition.Always);
        }

        public void SetNextTreeNodeOpen(bool opened, GuiCondition setCondition)
        {
            ImGuiNative.igSetNextTreeNodeOpen(opened, setCondition);
        }

        public bool TreeNode(string label)
        {
            return ImGuiNative.igTreeNode(label);
        }

        public bool TreeNodeEx(string label, TreeNodeFlags flags = 0)
        {
            return ImGuiNative.igTreeNodeEx(label, flags);
        }

        public void TreePop()
        {
            ImGuiNative.igTreePop();
        }

        public Vector2 GetLastItemRectSize()
        {
            Vector2 result;
            ImGuiNative.igGetItemRectSize(out result);
            return result;
        }

        public Vector2 GetLastItemRectMin()
        {
            Vector2 result;
            ImGuiNative.igGetItemRectMin(out result);
            return result;
        }

        public Vector2 GetLastItemRectMax()
        {
            Vector2 result;
            ImGuiNative.igGetItemRectMax(out result);
            return result;
        }

        public void SetWindowFontScale(float scale)
        {
            ImGuiNative.igSetWindowFontScale(scale);
        }

        public void SetScrollHere()
        {
            ImGuiNative.igSetScrollHere();
        }

        public void SetScrollHere(float centerYRatio)
        {
            ImGuiNative.igSetScrollHere(centerYRatio);
        }

        public void SetKeyboardFocusHere()
        {
            ImGuiNative.igSetKeyboardFocusHere(0);
        }

        public void SetKeyboardFocusHere(int offset)
        {
            ImGuiNative.igSetKeyboardFocusHere(offset);
        }

        public void CalcListClipping(int itemsCount, float itemsHeight, ref int outItemsDisplayStart, ref int outItemsDisplayEnd)
        {
            ImGuiNative.igCalcListClipping(itemsCount, itemsHeight, ref outItemsDisplayStart, ref outItemsDisplayEnd);
        }

        unsafe void PushFont(ImFont font)
        {
            ImGuiNative.igPushFont(font.NativeFont);
        }

        void PopFont()
        {
            ImGuiNative.igPopFont();
        }
    }
}
