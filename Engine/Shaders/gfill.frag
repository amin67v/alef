layout (location = 0) out vec4 GBufferAlbedo;
layout (location = 1) out vec4 GBufferSurface;
layout (location = 2) out vec2 GBufferNormal;
layout (location = 3) out float GBufferDepth;

in vec3 v2f_WorldPos;
in vec3 v2f_Normal;
in vec2 v2f_Texcoord;

uniform sampler2D Albedo;
uniform sampler2D Surface;
uniform samplerCube ReflectionSource;
uniform vec4 Color;

void main() 
{
    vec3 normal = normalize(v2f_Normal);
    vec4 surface = texture(Surface, v2f_Texcoord);

    vec3 r = reflect(normalize(v2f_WorldPos - CameraPos.xyz), normal);

    vec4 albedo = mix(texture(Albedo, v2f_Texcoord), texture(ReflectionSource, r), surface.x);

    GBufferAlbedo = albedo * Color + 0.025;
    GBufferSurface = surface;
    GBufferNormal = EncodeNormal(normal);
    GBufferDepth = length(v2f_WorldPos - CameraPos.xyz);
}