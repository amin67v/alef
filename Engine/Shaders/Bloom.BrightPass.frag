in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D InputTex;
uniform float Threshold;
uniform float Intensity;
//uniform float Desaturate;

void main()
{
    vec4 c = texture(InputTex, v2f_Texcoord);
    c = max(vec4(0), c - Threshold);
    c.rgb = pow(c.rgb, vec3(2.0));
    c.rgb = (c.rgb / (c.rgb + 8.0));
    //c.rgb = mix(c.rgb, vec3(dot(c.rgb, vec3(0.3, 0.6, 0.1))), Desaturate);
    FragColor = c * Intensity;
}