in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D InputTex;
uniform vec2 Direction;
#if defined(DEPTH_AWARE) || defined(DEPTH_FUNC)
uniform sampler2D GBufferDepth;
#endif

void main()
{   
    #define HALF_SAMPLES (NUM_SAMPLES / 2) 

    #if defined(DEPTH_AWARE) || defined(DEPTH_FUNC)
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    #endif

    vec4 c = vec4(0);
    vec2 texel = 1.0 / textureSize(InputTex, 0).xy;
    float totalW = 0.0;
    for(int i = -HALF_SAMPLES; i < HALF_SAMPLES; ++i)
    {
        #ifdef DEPTH_FUNC
            vec2 offset = v2f_Texcoord + DEPTH_FUNC(depth) * Direction * i * texel;
        #else
            vec2 offset = v2f_Texcoord + Direction * i * texel;
        #endif

        float w = (1.0 / (abs(i) + 1));
        #ifdef DEPTH_AWARE
        float sampleDepth = G_DEPTH(GBufferDepth, offset);
        w *= smoothstep(0, 1, 0.025 / abs(sampleDepth - depth));
        #endif
        c += texture(InputTex, offset) * w;
        totalW += w;
    }

    FragColor = c / totalW;
}