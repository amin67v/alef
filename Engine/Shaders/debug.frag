in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D InputTex;
uniform int SingleChannel;

void main() 
{
    vec4 rgba = texture(InputTex, v2f_Texcoord);
    if (SingleChannel == 1)
        rgba = vec4(rgba.r);

    FragColor = rgba;
}