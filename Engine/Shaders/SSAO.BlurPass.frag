in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D InputTex;
uniform sampler2D GBufferDepth;
uniform sampler2D GBufferNormal;
uniform vec2 Direction;

void main()
{   
    vec3 normal = G_NORMAL(GBufferNormal, v2f_Texcoord);
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    vec4 c = vec4(0);
    vec2 texel = 1.0 / textureSize(InputTex, 0).xy;
    float totalW = 0.0;
    for(int i = -4; i < 4; ++i)
    {
        vec2 offset = v2f_Texcoord + Direction * i * texel;
        vec3 sampleNormal = G_NORMAL(GBufferNormal, offset);
        float sampleDepth = G_DEPTH(GBufferDepth, offset);
        float w = 0.5 / (abs(sampleDepth - depth) + 1.0);
        
        if (dot(sampleNormal, normal) < 0.7)
            w = 0;

        c += texture(InputTex, offset) * w;
        totalW += w;
        
    }

    FragColor = c / totalW;
}