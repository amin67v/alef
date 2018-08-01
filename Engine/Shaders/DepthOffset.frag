in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec2 FragColor;

uniform sampler2D GBufferDepth;

vec2 SampleOffset(float depth, vec2 offset, vec2 texel)
{
        float sampleDepth = G_DEPTH(GBufferDepth, v2f_Texcoord + offset *  texel);
        float weight = smoothstep(0, 1, 0.025 / abs(sampleDepth - depth));
        return offset * weight;
}

void main()
{
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);

    vec2 texel = 1.0 / textureSize(GBufferDepth, 0).xy;

    vec2 offset = vec2(0);

    offset += SampleOffset(depth, vec2( 1,  0), texel);
    offset += SampleOffset(depth, vec2(-1,  0), texel);
    offset += SampleOffset(depth, vec2( 0,  1), texel);
    offset += SampleOffset(depth, vec2( 0, -1), texel);

    offset += SampleOffset(depth, vec2(-2, -1), texel);
    offset += SampleOffset(depth, vec2( 2,  1), texel);
    offset += SampleOffset(depth, vec2(-2,  1), texel);
    offset += SampleOffset(depth, vec2( 2, -1), texel);

    offset += SampleOffset(depth, vec2( 1,  2), texel);
    offset += SampleOffset(depth, vec2(-1, -2), texel);
    offset += SampleOffset(depth, vec2( 1, -2), texel);
    offset += SampleOffset(depth, vec2(-1,  2), texel);

    offset += SampleOffset(depth, vec2(-3, -3), texel);
    offset += SampleOffset(depth, vec2( 3,  3), texel);
    offset += SampleOffset(depth, vec2(-3,  3), texel);
    offset += SampleOffset(depth, vec2( 3, -3), texel);

    offset += SampleOffset(depth, vec2( 0,  3), texel);
    offset += SampleOffset(depth, vec2( 0, -3), texel);
    offset += SampleOffset(depth, vec2( 3,  0), texel);
    offset += SampleOffset(depth, vec2(-3,  0), texel);

    offset /= 10; // 20:sample / 2:max len

    FragColor = offset * 0.5 + 0.5;
}