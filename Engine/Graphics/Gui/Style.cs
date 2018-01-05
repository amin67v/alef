using System.Numerics;

namespace Engine
{
    public unsafe class Style
    {
        private readonly NativeStyle* ptr;

        internal Style(NativeStyle* style)
        {
            ptr = style;
        }

        /// <summary>
        /// Global alpha applies to everything in ImGui.
        /// </summary>
        public float Alpha
        {
            get { return ptr->Alpha; }
            set { ptr->Alpha = value; }
        }

        /// <summary>
        /// Padding within a window.
        /// </summary>
        public Vector2 WindowPadding
        {
            get { return ptr->WindowPadding; }
            set { ptr->WindowPadding = value; }
        }

        /// <summary>
        /// Minimum window size.
        /// </summary>
        public Vector2 WindowMinSize
        {
            get { return ptr->WindowMinSize; }
            set { ptr->WindowMinSize = value; }
        }

        /// <summary>
        /// Radius of window corners rounding. Set to 0.0f to have rectangular windows.
        /// </summary>
        public float WindowRounding
        {
            get { return ptr->WindowRounding; }
            set { ptr->WindowRounding = value; }
        }

        /// <summary>
        /// Alignment for title bar text.
        /// </summary>
        public Vector2 WindowTitleAlign
        {
            get { return ptr->WindowTitleAlign; }
            set { ptr->WindowTitleAlign = value; }
        }

        /// <summary>
        /// Radius of child window corners rounding. Set to 0.0f to have rectangular windows.
        /// </summary>
        public float ChildWindowRounding
        {
            get { return ptr->ChildWindowRounding; }
            set { ptr->ChildWindowRounding = value; }
        }

        /// <summary>
        /// Padding within a framed rectangle (used by most widgets).
        /// </summary>
        public Vector2 FramePadding
        {
            get { return ptr->FramePadding; }
            set { ptr->FramePadding = value; }
        }

        /// <summary>
        /// Radius of frame corners rounding. Set to 0.0f to have rectangular frame (used by most widgets). 
        /// </summary>
        public float FrameRounding
        {
            get { return ptr->FrameRounding; }
            set { ptr->FrameRounding = value; }
        }

        /// <summary>
        /// Horizontal and vertical spacing between widgets/lines.
        /// </summary>
        public Vector2 ItemSpacing
        {
            get { return ptr->ItemSpacing; }
            set { ptr->ItemSpacing = value; }
        }

        /// <summary>
        /// Horizontal and vertical spacing between within elements of a composed widget (e.g. a slider and its label).
        /// </summary>
        public Vector2 ItemInnerSpacing
        {
            get { return ptr->ItemInnerSpacing; }
            set { ptr->ItemInnerSpacing = value; }
        }

        /// <summary>
        /// Expand reactive bounding box for touch-based system where touch position is not accurate enough. Unfortunately we don't sort widgets so priority on overlap will always be given to the first widget. So don't grow this too much!
        /// </summary>
        public Vector2 TouchExtraPadding
        {
            get { return ptr->TouchExtraPadding; }
            set { ptr->TouchExtraPadding = value; }
        }

        /// <summary>
        /// Horizontal indentation when e.g. entering a tree node
        /// </summary>
        public float IndentSpacing
        {
            get { return ptr->IndentSpacing; }
            set { ptr->IndentSpacing = value; }
        }

        /// <summary>
        /// Minimum horizontal spacing between two columns
        /// </summary>
        public float ColumnsMinSpacing
        {
            get { return ptr->ColumnsMinSpacing; }
            set { ptr->ColumnsMinSpacing = value; }
        }

        /// <summary>
        /// Width of the vertical scrollbar, Height of the horizontal scrollbar
        /// </summary>
        public float ScrollbarSize
        {
            get { return ptr->ScrollbarSize; }
            set { ptr->ScrollbarSize = value; }
        }

        /// <summary>
        /// Radius of grab corners for scrollbar
        /// </summary>
        public float ScrollbarRounding
        {
            get { return ptr->ScrollbarRounding; }
            set { ptr->ScrollbarRounding = value; }
        }

        /// <summary>
        /// Minimum width/height of a grab box for slider/scrollbar
        /// </summary>
        public float GrabMinSize
        {
            get { return ptr->GrabMinSize; }
            set { ptr->GrabMinSize = value; }
        }

        /// <summary>
        /// Radius of grabs corners rounding. Set to 0.0f to have rectangular slider grabs.
        /// </summary>
        public float GrabRounding
        {
            get { return ptr->GrabRounding; }
            set { ptr->GrabRounding = value; }
        }

        /// <summary>
        /// Window positions are clamped to be visible within the display area by at least this amount. Only covers regular windows.
        /// </summary>
        public Vector2 DisplayWindowPadding
        {
            get { return ptr->DisplayWindowPadding; }
            set { ptr->DisplayWindowPadding = value; }
        }

        /// <summary>
        /// If you cannot see the edge of your screen (e.g. on a TV) increase the safe area padding. Covers popups/tooltips as well regular windows.
        /// </summary>
        public Vector2 DisplaySafeAreaPadding
        {
            get { return ptr->DisplaySafeAreaPadding; }
            set { ptr->DisplaySafeAreaPadding = value; }
        }

        /// <summary>
        /// Enable anti-aliasing on lines/borders. Disable if you are really tight on CPU/GPU.
        /// </summary>
        public bool AntiAliasedLines
        {
            get { return ptr->AntiAliasedLines == 1; }
            set { ptr->AntiAliasedLines = value ? (byte)1 : (byte)0; }
        }

        /// <summary>
        /// Enable anti-aliasing on filled shapes (rounded rectangles, circles, etc.)
        /// </summary>
        public bool AntiAliasedShapes
        {
            get { return ptr->AntiAliasedShapes == 1; }
            set { ptr->AntiAliasedShapes = value ? (byte)1 : (byte)0; }
        }

        /// <summary>
        /// Tessellation tolerance. Decrease for highly tessellated curves (higher quality, more polygons), increase to reduce quality.
        /// </summary>
        public float CurveTessellationTolerance
        {
            get { return ptr->CurveTessellationTol; }
            set { ptr->CurveTessellationTol = value; }
        }

        /// <summary>
        /// Gets the current style color for the given UI element type.
        /// </summary>
        /// <param name="target">The type of UI element.</param>
        /// <returns>The element's color as currently configured.</returns>
        public Color GetColor(ColorTarget target) => new Color(*(Vector4*)&ptr->Colors[(int)target * 4]);

        /// <summary>
        /// Sets the style color for a particular UI element type.
        /// </summary>
        /// <param name="target">The type of UI element.</param>
        /// <param name="value">The new color.</param>
        public void SetColor(ColorTarget target, Color value)
        {
            var vec = value.ToVector4();
            ptr->Colors[(int)target * 4 + 0] = vec.X;
            ptr->Colors[(int)target * 4 + 1] = vec.Y;
            ptr->Colors[(int)target * 4 + 2] = vec.Z;
            ptr->Colors[(int)target * 4 + 3] = vec.W;
        }
    }
}
