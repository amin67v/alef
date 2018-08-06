in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D GBufferDepth;
uniform sampler2D LightBuffer;
uniform sampler2D BloomTexture;

uniform vec4  FogColor;
uniform float FogDensity;
uniform float Exposure;

void main()
{
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    vec3 light = texture(LightBuffer, v2f_Texcoord).rgb;
    light += texture(BloomTexture, v2f_Texcoord).rgb;

    vec3 c = mix(light, FogColor.rgb, pow(SATURATE(depth * FogDensity), 0.5));

    c = FilmicACES(c * Exposure);
    c = pow(c, vec3(1 / 2.2));
    
    FragColor = vec4(c, dot(c, vec3(0.3, 0.6, 0.1)));
}