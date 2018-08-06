in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out float FragColor;

uniform sampler2D GBufferNormal;
uniform sampler2D GBufferDepth;
uniform sampler2D NoiseTex;
uniform sampler2D TemporalTex;
uniform sampler2D PrevDepthTex;
uniform mat4 PrevViewProjMatrix;
uniform vec3 SampleKernel[16];
uniform vec2 BufferSize;
uniform float Radius;

float SampleAO(vec3 inputSample, vec3 normal, float depth, vec3 position, vec3 randVec)
{
    vec3 sample = reflect(inputSample, randVec);
    sample *= sign(dot(sample, normal));
    sample = position + sample * Radius;

    vec4 offset = ViewProjMatrix * vec4(sample, 1);
    offset.xyz /= offset.w;
    offset.xy = offset.xy * 0.5 + 0.5;

    if (offset.x < 0 || offset.x > 1 || offset.y < 0 || offset.y > 1)
        return 0;

    float sampleDepth = length(CameraPos.xyz - sample);
    float gDepth = texture(GBufferDepth, offset.xy).r;

    float occ = float(sampleDepth >= gDepth + 0.01);
    float dist = smoothstep(0, 1, (Radius * 0.5) / abs(gDepth - depth));

    return occ * dist;
}

void main() 
{
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    vec3 normal = G_NORMAL(GBufferNormal, v2f_Texcoord);

    if (depth < 0)
    {
        FragColor = 1.0;
        return;
    }

    vec3 position = DEPTH2POS(depth, normalize(v2f_WorldPos - CameraPos.xyz));
    vec3 randVec = texture(NoiseTex, v2f_Texcoord * (BufferSize / textureSize(NoiseTex, 0).xy)).rgb * 2 - 1;

    float ao = 0;
    for(int i = 0; i < NUM_SAMPLES; ++i)
        ao += SampleAO(SampleKernel[i], normal, depth, position, randVec);
    
    ao = 1 - (ao / NUM_SAMPLES);

    vec4 prevClipPos = PrevViewProjMatrix * vec4(position, 1);
    prevClipPos.xy /= prevClipPos.w;
    prevClipPos.xy = prevClipPos.xy * 0.5 + 0.5;

    float prevAO = texture(TemporalTex, prevClipPos.xy).r;
    float pDepth = G_DEPTH(PrevDepthTex, prevClipPos.xy);

    float temporalBlend = 1.0 / (abs(pDepth - depth) * 2.5 + 1.0);
    ao = mix(ao, prevAO , mix(0.2, 0.9, temporalBlend));

    FragColor = ao;
}