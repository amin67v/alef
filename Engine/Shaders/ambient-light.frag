in vec3 v2f_ViewDir;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D GBufferAlbedo;
uniform sampler2D GBufferSurface;
uniform sampler2D GBufferNormal;
uniform sampler2D SSAOBuffer;

uniform vec3 SkyAmbient;
uniform vec3 GroundAmbient;
uniform float SSAOPower;

void main() 
{
    vec4 albedo = G_ALBEDO(GBufferAlbedo, v2f_Texcoord);
    vec4 surface = G_SURFACE(GBufferSurface, v2f_Texcoord);
    vec3 normal = G_NORMAL(GBufferNormal, v2f_Texcoord);

    float ao = texture(SSAOBuffer, v2f_Texcoord).r;
    ao = pow(ao, SSAOPower);

    vec3 ambient = mix(GroundAmbient, SkyAmbient, normal.y * 0.5 + 0.5) * ao * albedo.rgb;
    ambient = mix(ambient, albedo.rgb, pow(surface.x, 4));
    FragColor = vec4(ambient, 1);
}