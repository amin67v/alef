in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D GBufferAlbedo;
uniform sampler2D GBufferSurface;
uniform sampler2D GBufferNormal;
uniform sampler2D GBufferDepth;
uniform sampler2D DepthOffset;
uniform sampler2D ShadowMap;

uniform vec3 LightDir;
uniform vec4 Color;

void main() 
{
    vec4 albedo = G_ALBEDO(GBufferAlbedo, v2f_Texcoord);
    vec4 surface = G_SURFACE(GBufferSurface, v2f_Texcoord);
    vec3 normal = G_NORMAL(GBufferNormal, v2f_Texcoord);
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    vec2 depthOffset = DEPTH_OFFSET(DepthOffset, v2f_Texcoord);

    vec3 position = DEPTH2POS(depth, normalize(v2f_WorldPos - CameraPos.xyz));
    vec3 viewDir = normalize(CameraPos.xyz - position);

    vec3 light = CalcLighting(albedo.rgb, surface.x, surface.y, Color.rgb, normal, -LightDir, viewDir);
    
    vec2 texel = 1.0 / textureSize(ShadowMap, 0).xy;
    float shadow = texture(ShadowMap, v2f_Texcoord + depthOffset * texel).r;

    FragColor = vec4(light * shadow, 1);
}