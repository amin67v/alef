namespace Engine
{
    public static class DefaultShaders
    {
        static string vert_standard = @"
            #version 330 core
            layout(location = 0) in vec2 position;
            layout(location = 1) in vec2 texcoord;
            layout(location = 2) in vec4 color;

            out vec4 v2f_color;
            out vec2 v2f_texcoord;

            uniform mat3x2 model_mat;
            uniform mat4 view_mat;

            void main()
            {
                gl_Position = view_mat * vec4(model_mat * vec3(position, 1), 0, 1);
                v2f_texcoord = texcoord;
                v2f_color = color;
            }";

        static string frag_color_mix = @"
            #version 330 core
            in vec4 v2f_color;
            in vec2 v2f_texcoord;

            out vec4 frag_color;

            uniform sampler2D main_tex;

            void main() 
            {
                vec4 c = texture(main_tex, v2f_texcoord);
                c.rgb = mix(c.rgb, v2f_color.rgb, v2f_color.a);
                frag_color = c;
            }";

        static string frag_color_mult = @"
            #version 330 core
            in vec4 v2f_color;
            in vec2 v2f_texcoord;

            out vec4 frag_color;

            uniform sampler2D main_tex;

            void main() 
            {
                frag_color = texture(main_tex, v2f_texcoord) * v2f_color;
            }";

        static string frag_font = @"
            #version 330 core
            in vec2 v2f_texcoord;
           
            uniform vec4 color;
            uniform sampler2D main_tex;
           
            out vec4 frag_color;
           
            void main() 
            {
                frag_color = texture(main_tex, v2f_texcoord) * color;
            }";

        static Shader color_mult;
        static Shader color_mix;
        static Shader font;
    
        /// <summary>
        /// The default color multiply shader
        /// </summary>
        public static Shader ColorMult => color_mult ?? (color_mult = Shader.Create(vert_standard, frag_color_mult));

        /// <summary>
        /// The default color mix shader
        /// </summary>
        public static readonly Shader ColorMix = color_mix ?? ( color_mix = Shader.Create(vert_standard, frag_color_mix));

        /// <summary>
        /// The default font shader
        /// </summary>
        public static readonly Shader Font = font ?? (font = Shader.Create(vert_standard, frag_font));

        internal static void dispose_all()
        {
            color_mult.Dispose();
            color_mix.Dispose();
            font.Dispose();

            color_mult = null;
            color_mix = null;
            font = null;
        }
    }
}