in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out float FragColor;

uniform sampler2D GBufferDepth;
uniform sampler2D InputTex;
uniform vec2 Direction;

void main() 
{
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);

    vec2 texel = (1.0 / textureSize(InputTex, 0).xy);
    float shadow = 0;
    float totalW = 0;
    for(int i = -2; i <= 2; ++i)
    {
        float dfactor = 1 / (depth + 1);
        dfactor *= dfactor;
        
        vec2 offset = v2f_Texcoord + Direction * i * texel * dfactor;
        float sampleDepth = G_DEPTH(GBufferDepth, offset);
        float weight = smoothstep(0, 1, 0.02 / abs(sampleDepth - depth));
        weight *= 1 / (length(offset - v2f_Texcoord) + 1);
        shadow += texture(InputTex, offset).r * weight;
        totalW += weight;
    }
    FragColor = shadow / totalW;
}