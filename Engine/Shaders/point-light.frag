in vec3 v2f_WorldPos;
in vec4 v2f_FragPos;

out vec4 FragColor;

uniform sampler2D GBufferAlbedo;
uniform sampler2D GBufferSurface;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferDepth;

uniform vec4 LightPosRadius;
uniform vec4 Color;

void main() 
{
    vec2 texCoord = (v2f_FragPos.xy / v2f_FragPos.w) * 0.5 + 0.5;

    vec4 albedo = G_ALBEDO(GBufferAlbedo, texCoord);
    vec4 surface = G_SURFACE(GBufferSurface, texCoord);
    vec3 normal = G_NORMAL(GBufferNormal, texCoord);
    float depth = G_DEPTH(GBufferDepth, texCoord);

    vec3 position = DEPTH2POS(depth, normalize(v2f_WorldPos - CameraPos.xyz));
    vec3 viewDir = normalize(CameraPos.xyz - position);
    vec3 LightDir = normalize(LightPosRadius.xyz - position);

    float atten = SATURATE(1 - (length(position - LightPosRadius.xyz) / LightPosRadius.w));
    atten = atten * atten;

    vec3 light = CalcLighting(albedo.rgb, surface.x, surface.y, atten * Color.rgb, normal, LightDir, viewDir);
    FragColor = vec4(light, 1);
}