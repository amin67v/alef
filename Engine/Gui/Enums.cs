namespace Engine
{
    public enum InputTextFlags
    {
        Default = 0,
        /// <summary>
        /// Allow 0123456789.+-*/
        /// </summary>
        CharsDecimal = 1 << 0,
        /// <summary>
        /// Allow 0123456789ABCDEFabcdef
        /// </summary>
        CharsHexadecimal = 1 << 1,
        /// <summary>
        /// Turn a..z into A..Z
        /// </summary>
        CharsUppercase = 1 << 2,
        /// <summary>
        /// Filter out spaces, tabs
        /// </summary>
        CharsNoBlank = 1 << 3,
        /// <summary>
        /// Select entire text when first taking mouse focus
        /// </summary>
        AutoSelectAll = 1 << 4,
        /// <summary>
        /// Return 'true' when Enter is pressed (as opposed to when the value was modified)
        /// </summary>
        EnterReturnsTrue = 1 << 5,
        /// <summary>
        /// Call user function on pressing TAB (for completion handling)
        /// </summary>
        CallbackCompletion = 1 << 6,
        /// <summary>
        /// Call user function on pressing Up/Down arrows (for history handling)
        /// </summary>
        CallbackHistory = 1 << 7,
        /// <summary>
        /// Call user function every time
        /// </summary>
        CallbackAlways = 1 << 8,
        /// <summary>
        /// Call user function to filter character. Modify data->EventChar to replace/filter input, or return 1 to discard character.
        /// </summary>
        CallbackCharFilter = 1 << 9,
        /// <summary>
        /// Pressing TAB input a '\t' character into the text field
        /// </summary>
        AllowTabInput = 1 << 10,
        /// <summary>
        /// In multi-line mode, allow exiting edition by pressing Enter. Ctrl+Enter to add new line (by default adds new lines with Enter).
        /// </summary>
        CtrlEnterForNewLine = 1 << 11,
        /// <summary>
        /// Disable following the cursor horizontally
        /// </summary>
        NoHorizontalScroll = 1 << 12,
        /// <summary>
        /// Insert mode
        /// </summary>
        AlwaysInsertMode = 1 << 13,
        /// <summary>
        /// Read-only mode
        /// </summary>
        ReadOnly = 1 << 14,
        /// <summary>
        /// For internal use by InputTextMultiline()
        /// </summary>
        Multiline = 1 << 20
    }

    /// <summary>
    /// Flags for ImGui::Selectable()
    /// </summary>
    public enum SelectableFlags
    {
        // Default: 0
        Default = 0,
        /// <summary>
        /// Clicking this doesn't close parent popup window
        /// </summary>
        DontClosePopups = 1 << 0,
        /// <summary>
        /// Selectable frame can span all columns (text will still fit in current column)
        /// </summary>
        SpanAllColumns = 1 << 1
    }

    public enum TreeNodeFlags
    {
        /// <summary>
        /// Draw as selected
        /// </summary>
        Selected = 1 << 0,
        /// <summary>
        /// Full colored frame (e.g. for CollapsingHeader)
        /// </summary>
        Framed = 1 << 1,
        /// <summary>
        /// Hit testing to allow subsequent widgets to overlap this one
        /// </summary>
        AllowOverlapMode = 1 << 2,
        /// <summary>
        /// Don't do a TreePush() when open (e.g. for CollapsingHeader) = no extra indent nor pushing on ID stack
        /// </summary>
        NoTreePushOnOpen = 1 << 3,
        /// <summary>
        /// Don't automatically and temporarily open node when Logging is active (by default logging will automatically open tree nodes)
        /// </summary>
        NoAutoOpenOnLog = 1 << 4,
        /// <summary>
        /// Default node to be open
        /// </summary>
        DefaultOpen = 1 << 5,
        /// <summary>
        /// Need double-click to open node
        /// </summary>
        OpenOnDoubleClick = 1 << 6,
        /// <summary>
        /// Only open when clicking on the arrow part. If ImGuiTreeNodeFlags_OpenOnDoubleClick is also set, single-click arrow or double-click all box to open.
        /// </summary>
        OpenOnArrow = 1 << 7,
        /// <summary>
        /// No collapsing, no arrow (use as a convenience for leaf nodes).
        /// </summary>
        Leaf = 1 << 8,
        /// <summary>
        /// Display a bullet instead of arrow
        /// </summary>
        Bullet = 1 << 9,

        CollapsingHeader = Framed | NoAutoOpenOnLog
    }

    public enum WindowFlags
    {
        Default = 0,
        /// <summary>
        /// Disable title-bar
        /// </summary>
        NoTitleBar = 1 << 0,
        /// <summary>
        /// Disable user resizing with the lower-right grip
        /// </summary>
        NoResize = 1 << 1,
        /// <summary>
        /// Disable user moving the window
        /// </summary>
        NoMove = 1 << 2,
        /// <summary>
        /// Disable scrollbar (window can still scroll with mouse or programatically)
        /// </summary>
        NoScrollbar = 1 << 3,
        /// <summary>
        /// Disable user scrolling with mouse wheel
        /// </summary>
        NoScrollWithMouse = 1 << 4,
        /// <summary>
        /// Disable user collapsing window by double-clicking on it
        /// </summary>
        NoCollapse = 1 << 5,
        /// <summary>
        /// Resize every window to its content every frame
        /// </summary>
        AlwaysAutoResize = 1 << 6,
        /// <summary>
        /// Show borders around windows and items
        /// </summary>
        ShowBorders = 1 << 7,
        /// <summary>
        /// Never load/save settings in .ini file
        /// </summary>
        NoSavedSettings = 1 << 8,
        /// <summary>
        /// Disable catching mouse or keyboard inputs
        /// </summary>
        NoInputs = 1 << 9,
        /// <summary>
        /// Has a menu-bar
        /// </summary>
        MenuBar = 1 << 10,
        /// <summary>
        /// Enable horizontal scrollbar (off by default).
        /// You need to use SetNextWindowContentSize(ImVec2(width,0.0f)); prior to calling Begin() to specify width.
        /// </summary>
        HorizontalScrollbar = 1 << 11,
        /// <summary>
        /// Disable taking focus when transitioning from hidden to visible state
        /// </summary>
        NoFocusOnAppearing = 1 << 12,
        /// <summary>
        /// Disable bringing window to front when taking focus (e.g. clicking on it or programatically giving it focus)
        /// </summary>
        NoBringToFrontOnFocus = 1 << 13,
    }

    public enum StyleVar
    {
        Alpha,
        WindowPadding,
        WindowRounding,
        WindowMinSize,
        ChildWindowRounding,
        FramePadding,
        FrameRounding,
        ItemSpacing,
        ItemInnerSpacing,
        IndentSpacing,
        GrabMinSize,
        ButtonTextAlign
    }

    public enum ColorEditFlags
    {
        Default = 0,
        NoAlpha = 1 << 1,
        NoPicker = 1 << 2,
        NoOptions = 1 << 3,
        NoSmallPreview = 1 << 4,
        NoInputs = 1 << 5,
        NoTooltip = 1 << 6,
        NoLabel = 1 << 7,
        NoSidePreview = 1 << 8,
        AlphaBar = 1 << 9,
        AlphaPreview = 1 << 10,
        AlphaPreviewHalf = 1 << 11,
        HDR = 1 << 12,
        RGB = 1 << 13,
        HSV = 1 << 14,
        HEX = 1 << 15,
        Uint8 = 1 << 16,
        Float = 1 << 17,
        PickerHueBar = 1 << 18,
        PickerHueWheel = 1 << 19,
    }

    public enum ColorTarget
    {
        Text,
        TextDisabled,
        WindowBg,
        ChildWindowBg,
        PopupBg,
        Border,
        BorderShadow,
        FrameBg,
        FrameBgHovered,
        FrameBgActive,
        TitleBg,
        TitleBgCollapsed,
        TitleBgActive,
        MenuBarBg,
        ScrollbarBg,
        ScrollbarGrab,
        ScrollbarGrabHovered,
        ScrollbarGrabActive,
        ComboBg,
        CheckMark,
        SliderGrab,
        SliderGrabActive,
        Button,
        ButtonHovered,
        ButtonActive,
        Header,
        HeaderHovered,
        HeaderActive,
        Separator,
        SeparatorHovered,
        SeparatorActive,
        ResizeGrip,
        ResizeGripHovered,
        ResizeGripActive,
        CloseButton,
        CloseButtonHovered,
        CloseButtonActive,
        PlotLines,
        PlotLinesHovered,
        PlotHistogram,
        PlotHistogramHovered,
        TextSelectedBg,
        ModalWindowDarkening,

        Count,
    }

    public enum GuiCondition
    {
        Always = 1 << 0,
        Once = 1 << 1,
        FirstUseEver = 1 << 2,
        Appearing = 1 << 3
    }



    enum MouseCursorKind
    {
        Arrow = 0,
        TextInput,
        Move,
        ResizeNS,
        ResizeEW,
        ResizeNESW,
        ResizeNWSE,
    }

    enum GuiKey
    {
        Tab,
        LeftArrow,
        RightArrow,
        UpArrow,
        DownArrow,
        PageUp,
        PageDown,
        Home,
        End,
        Delete,
        Backspace,
        Enter,
        Escape,
        A,
        C,
        V,
        X,
        Y,
        Z,
        Count
    }
}