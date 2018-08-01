in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D GBufferDepth;
uniform sampler2D LightBuffer;

uniform vec4  FogColor;
uniform float FogDensity;
uniform float Exposure;

vec3 Tonemap(vec3 hdr)
{
    return (hdr * (2.51 * hdr + 0.03)) / (hdr * (2.43 * hdr + 0.59) + 0.14);
}

void main()
{
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    vec3 light = texture(LightBuffer, v2f_Texcoord).rgb;

    vec3 c = mix(light, FogColor.rgb, pow(SATURATE(depth * FogDensity), 0.5));

    c = Tonemap(c * Exposure);
    c = pow(c, vec3(1 / 2.2));
    
    FragColor = vec4(c, dot(c, vec3(0.3, 0.6, 0.1)));
}