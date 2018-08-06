in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D InputTex;
uniform vec2 Direction;

#define NUM_SAMPLES 5
const float Offsets[NUM_SAMPLES] = float[](-2, -1, 0, 1, 2);
const float Weights[NUM_SAMPLES] = float[](0.06136, 0.24477, 0.38774, 0.24477, 0.06136);

void main()
{
    vec4 c = vec4(0);
    vec2 texel = 1.0 / textureSize(InputTex, 0).xy;
    for (int i = 0; i < NUM_SAMPLES; ++i)
    {
        vec2 offset = v2f_Texcoord + Direction * Offsets[i] * texel;
        c += texture(InputTex, offset) * Weights[i];
    }
    FragColor = c;
}