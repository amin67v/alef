
#define PI 3.1415926535897932384626433832795
#define SATURATE(x) clamp(x, 0.0, 1.0)
#define REMAP(v, a1, b1, a2, b2) (a2 + (v - a1) * (b2 - a2) / (b1 - a1))

#define G_ALBEDO(tex, pos) texture(tex, pos)
#define G_SURFACE(tex, pos) texture(tex, pos)
#define G_NORMAL(tex, pos) DecodeNormal(texture(tex, pos).rg)
#define G_DEPTH(tex, pos) texture(tex, pos).r
#define DEPTH_OFFSET(tex, pos) (texture(tex, pos).xy * 2 - 1)

#define DEPTH2POS(depth, viewDir) CameraPos.xyz + depth * viewDir;

#define VERTEX_STANDARD_LAYOUT \
layout(location = 0) in vec3 v_Position; \
layout(location = 1) in vec3 v_Normal; \
layout(location = 2) in vec3 v_Tangent; \
layout(location = 3) in vec3 v_Texcoord;

layout(std140) uniform Constants
{
    mat4 ViewProjMatrix; 
    mat4 InvViewProjMatrix;
    mat4 ShadowMatrix;

    vec4 CameraPos;
    vec4 CameraDir;
    vec2 WindowSize;
    vec2 RenderSize;
    float FieldOfView;
    float FarClip;
    float NearClip;
    float Time;
};

vec3 FilmicACES(vec3 hdr)
{
    return (hdr * (2.51 * hdr + 0.03)) / (hdr * (2.43 * hdr + 0.59) + 0.14);
}

// encode normal to spherical coord
vec2 EncodeNormal(vec3 n)
{
    return vec2(atan(n.y, n.x + 0.000001), n.z);
}

// decode normal from spherical coord
vec3 DecodeNormal(vec2 e)
{
    vec2 scth = vec2(sin(e.x), cos(e.x));
    vec2 scphi = vec2(sqrt(1 - e.y * e.y), e.y);
    return vec3(scth.y * scphi.x, scth.x * scphi.x, scphi.y);
}

vec3 FresnelSchlick(float ndotv, vec3 f0)
{
    return f0 + (1 - f0) * pow(1 - ndotv, 5);
} 

float DistributionGGX(float ndoth, float roughness)
{
    float a = roughness * roughness;
    float a2 = a * a;
    float ndoth2 = ndoth * ndoth;
    
    float num = a2;
    float denom = (ndoth2 * (a2 - 1) + 1);
    denom = PI * denom * denom;
    
    return num / denom;
}

float GeometrySchlickGGX(float ndotv, float roughness)
{
    float r = (roughness + 1);
    float k = (r * r) / 8;

    float num   = ndotv;
    float denom = ndotv * (1 - k) + k;
    
    return num / denom;
}

float GeometrySmith(float ndotl, float ndotv, float roughness)
{
    float ggx1  = GeometrySchlickGGX(ndotl, roughness);
    float ggx2  = GeometrySchlickGGX(ndotv, roughness);
    
    return ggx1 * ggx2;
}

vec3 CalcLighting(vec3 albedo, float metallic, float roughness, vec3 atten, vec3 n, vec3 l, vec3 v)
{
    vec3 h = normalize(v + l);
    float ndotl = max(0, dot(n, l));
    float ndotv = dot(n, v);
    float ndoth = max(0, dot(n, h));

    vec3 f0 = vec3(0.04); 
    f0 = mix(f0, albedo, metallic);
    vec3 fresnel = FresnelSchlick(ndotv, f0);
    vec3 num = DistributionGGX(ndoth, roughness) * GeometrySmith(ndotl, ndotv, roughness) * fresnel;
    float denom = 4 * ndotv * ndotl;
    vec3 spec = num / denom;

    vec3 kD = vec3(1) - fresnel;
    kD *= 1 - metallic;

    return max(vec3(0), (kD * albedo / PI + spec) * atten * ndotl);
}
