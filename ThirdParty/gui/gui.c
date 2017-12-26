#define NK_IMPLEMENTATION
#define NK_PRIVATE
#define NK_INCLUDE_FIXED_TYPES
#define NK_INCLUDE_DEFAULT_ALLOCATOR
#define NK_INCLUDE_STANDARD_IO
#define NK_INCLUDE_STANDARD_VARARGS
#define NK_INCLUDE_VERTEX_BUFFER_OUTPUT
#define NK_INCLUDE_FONT_BAKING
#define NK_INCLUDE_DEFAULT_FONT
#define NK_BUTTON_TRIGGER_ON_RELEASE
#define NK_ASSERT

#include "nuklear.h"

#if defined _WIN32 || defined __CYGWIN__
#ifdef CIMGUI_NO_EXPORT
#define API
#else
#define API __declspec(dllexport)
#endif
#ifndef __GNUC__
#define snprintf sprintf_s
#endif
#else
#define API
#endif

#if defined __cplusplus
#define EXTERN extern "C"
#else
#include <stdarg.h>
#include <stdbool.h>
#define EXTERN extern
#endif

#define GUI_API EXTERN API

GUI_API void* gui_init()
{
    nk_context* ctx;
    nk_init_default(ctx, 0);
    
    return ctx;
}

GUI_API void gui_begin_frame()
{

}

GUI_API void gui_end_frame()
{

}

GUI_API void gui_render()
{

}