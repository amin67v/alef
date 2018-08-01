in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D InputTex;

void main()
{
    vec4 c = texture(InputTex, v2f_Texcoord);
    c -= 5;
    c = max(0, c);
    FragColor = c;
}