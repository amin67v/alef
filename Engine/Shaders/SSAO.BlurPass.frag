in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D InputTex;
uniform sampler2D GBufferDepth;
uniform sampler2D GBufferNormal;
uniform vec2 Direction;

#define NUM_SAMPLES 5
const float Offsets[NUM_SAMPLES] = float[](-2, -1, 0, 1, 2);
const float Weights[NUM_SAMPLES] = float[](0.06136, 0.24477, 0.38774, 0.24477, 0.06136);

void main()
{   
    vec3 normal = G_NORMAL(GBufferNormal, v2f_Texcoord);
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    vec4 c = vec4(0);
    vec2 texel = 1.0 / textureSize(InputTex, 0).xy;
    float totalW = 0.0;
    for(int i = 0; i < NUM_SAMPLES; ++i)
    {
        vec2 offset = v2f_Texcoord + Direction * Offsets[i] * texel;
        vec3 sampleNormal = G_NORMAL(GBufferNormal, offset);
        float sampleDepth = G_DEPTH(GBufferDepth, offset);
        float w = Weights[i] * (0.5 / (abs(sampleDepth - depth) + 1.0));
                
        if (dot(sampleNormal, normal) < 0.7)
            w = 0;

        c += texture(InputTex, offset) * w;
        totalW += w;
        
    }

    FragColor = c / totalW;
}