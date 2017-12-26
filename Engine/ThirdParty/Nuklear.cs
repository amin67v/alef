//   
//   using System;
//   using System.Runtime.InteropServices;
//   
//   namespace Engine
//   {
//       [SuppressUnmanagedCodeSecurity]
//       static unsafe class Nuklear
//       {
//           const string lib = "nuklear";
//   
//           const int NK_INPUT_MAX = 16;
//   
//           [StructLayout(LayoutKind.Sequential)]
//           public struct nk_vec2 { public float x, y; }
//   
//           public enum nk_keys
//           {
//               NK_KEY_NONE,
//               NK_KEY_SHIFT,
//               NK_KEY_CTRL,
//               NK_KEY_DEL,
//               NK_KEY_ENTER,
//               NK_KEY_TAB,
//               NK_KEY_BACKSPACE,
//               NK_KEY_COPY,
//               NK_KEY_CUT,
//               NK_KEY_PASTE,
//               NK_KEY_UP,
//               NK_KEY_DOWN,
//               NK_KEY_LEFT,
//               NK_KEY_RIGHT,
//               /* Shortcuts: text field */
//               NK_KEY_TEXT_INSERT_MODE,
//               NK_KEY_TEXT_REPLACE_MODE,
//               NK_KEY_TEXT_RESET_MODE,
//               NK_KEY_TEXT_LINE_START,
//               NK_KEY_TEXT_LINE_END,
//               NK_KEY_TEXT_START,
//               NK_KEY_TEXT_END,
//               NK_KEY_TEXT_UNDO,
//               NK_KEY_TEXT_REDO,
//               NK_KEY_TEXT_SELECT_ALL,
//               NK_KEY_TEXT_WORD_LEFT,
//               NK_KEY_TEXT_WORD_RIGHT,
//               /* Shortcuts: scrollbar */
//               NK_KEY_SCROLL_START,
//               NK_KEY_SCROLL_END,
//               NK_KEY_SCROLL_DOWN,
//               NK_KEY_SCROLL_UP,
//               NK_KEY_MAX
//           }
//   
//           public enum nk_buttons
//           {
//               NK_BUTTON_LEFT,
//               NK_BUTTON_MIDDLE,
//               NK_BUTTON_RIGHT,
//               NK_BUTTON_DOUBLE,
//               NK_BUTTON_MAX
//           }
//   
//           public struct nk_mouse_button
//           {
//               public int down;
//               public uint clicked;
//               public nk_vec2 clicked_pos;
//           }
//   
//           public struct nk_mouse
//           {
//               [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)nk_buttons.NK_BUTTON_MAX)]
//               public nk_mouse_button[] buttons;
//               public nk_vec2 pos;
//               public nk_vec2 prev;
//               public nk_vec2 delta;
//               public nk_vec2 scroll_delta;
//               public byte grab;
//               public byte grabbed;
//               public byte ungrab;
//           }
//   
//           [StructLayout(LayoutKind.Sequential)]
//           public struct nk_key
//           {
//               public int down;
//               public uint clicked;
//           }
//   
//           [StructLayout(LayoutKind.Sequential)]
//           public struct nk_keyboard
//           {
//               [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)nk_keys.NK_KEY_MAX)]
//               public nk_key[] keys;
//   
//               [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NK_INPUT_MAX)]
//               public string text;
//   
//               public int text_len;
//           }
//   
//           [StructLayout(LayoutKind.Sequential)]
//           public struct nk_input
//           {
//               public nk_keyboard keyboard;
//               public nk_mouse mouse;
//           }
//   
//           // [StructLayout(LayoutKind.Sequential)]
//           // public struct nk_context
//           // {
//           //     /* public: can be accessed freely */
//           //     public nk_input input;
//           //     public nk_style style;
//           //     public nk_buffer memory;
//           //     public nk_clipboard clip;
//           //     public nk_flags last_widget_state;
//           //     public nk_button_behavior button_behavior;
//           //     public nk_configuration_stacks stacks;
//           //     public float delta_time_seconds;
//   // 
//           //     /* private:
//           //         should only be accessed if you
//           //         know what you are doing */
//   // 
//           //     //# ifdef NK_INCLUDE_VERTEX_BUFFER_OUTPUT
//           //     public nk_draw_list draw_list;
//           //     //#endif
//           //     //#ifdef NK_INCLUDE_COMMAND_USERDATA
//           //     //    nk_handle userdata;
//           //     //#endif
//   // 
//           //     /* text editor objects are quite big because of an internal
//           //      * undo/redo stack. Therefore it does not make sense to have one for
//           //      * each window for temporary use cases, so I only provide *one* instance
//           //      * for all windows. This works because the content is cleared anyway */
//           //     public nk_text_edit text_edit;
//           //     /* draw buffer used for overlay drawing operation like cursor */
//           //     public nk_command_buffer overlay;
//   // 
//           //     /* windows */
//           //     public int build;
//           //     public int use_pool;
//           //     public nk_pool pool;
//           //     public nk_window* begin;
//           //     public nk_window* end;
//           //     public nk_window* active;
//           //     public nk_window* current;
//           //     public nk_page_element* freelist;
//           //     public uint count;
//           //     public uint seq;
//           // };
//       }
//   }