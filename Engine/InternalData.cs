
namespace Engine
{
    static class InternalData
    {
        const string vert_standard = @"
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

        const string frag_color_mix = @"
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

        const string frag_color_mult = @"
            #version 330 core
            in vec4 v2f_color;
            in vec2 v2f_texcoord;

            out vec4 frag_color;

            uniform sampler2D main_tex;

            void main() 
            {
                frag_color = texture(main_tex, v2f_texcoord) * v2f_color;
            }";

        const string vert_particle = @"
            #version 330 core
            layout(location = 1) in vec2 velocity;
            layout(location = 2) in vec2 position;
            layout(location = 3) in vec2 texcoord;
            layout(location = 4) in float rotation;
            layout(location = 5) in float size;
            layout(location = 6) in vec4 color;

            out vtx_data
            {
                vec2 position;
                vec2 texcoord;
                float rot;
                float size;
                vec4 color;
            } v2g;

            void main()
            {
                v2g.position = position;
                v2g.texcoord = texcoord;
                v2g.color = color;
                v2g.rot = rotation;
                v2g.size = size;
            }";

        const string geom_particle = @"
            #version 330 core
            layout (points) in;
            layout (triangle_strip, max_vertices = 4) out;

            in vtx_data
            {
                vec2 position;
                vec2 texcoord;
                float rot;
                float size;
                vec4 color;
            } v2g[];

            out geo_data
            {
                vec2 texcoord;
                vec4 color;
            } g2f;

            uniform mat4 view_mat;
            uniform mat3x2 model_mat;

            void emit(vec2 offset)
            {
                g2f.texcoord = v2g[0].texcoord;
                g2f.color = v2g[0].color;

                vec2 pos = v2g[0].position;
                float sin_rot = sin(v2g[0].rot);
                float cos_rot = cos(v2g[0].rot);
                pos.x += (cos_rot * offset.x) - (sin_rot * offset.y);
                pos.y += (cos_rot * offset.y) + (sin_rot * offset.x);
                
                gl_Position = view_mat * vec4(pos, 0, 1);
                EmitVertex();
            }

            void main() 
            {
                float size = v2g[0].size * .5;
                emit(vec2(-size, size));
                emit(vec2(size, size));
                emit(vec2(-size, -size));
                emit(vec2(size, -size));
                EndPrimitive();
            }";

        const string frag_particle = @"
            #version 330 core
            in geo_data
            {
                vec2 texcoord;
                vec4 color;
            } g2f;

            out vec4 frag_color;

            uniform sampler2D main_tex;

            void main() 
            {
                frag_color = texture(main_tex, g2f.texcoord) * g2f.color;
            }";

        internal static void init()
        {
            DataCache.Add("Mult.Shader", Shader.Create(vert_standard, frag_color_mult));
            DataCache.Add("Mix.Shader", Shader.Create(vert_standard, frag_color_mix));
            DataCache.Add("Particle.Shader", Shader.Create(vert_particle, geom_particle, frag_particle));
            DataCache.Add("White.Texture", Texture.Create(1, 1, FilterMode.Point, WrapMode.Repeat, new Color[] { Color.White }));
            DataCache.Add("Checker.Texture", Texture.Create(2, 2, FilterMode.Point, WrapMode.Repeat, new Color[] { Color.LightGray, Color.White, Color.White, Color.LightGray }));
        }
    }
}