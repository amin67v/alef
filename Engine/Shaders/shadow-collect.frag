
in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out float FragColor;

uniform sampler2D GBufferDepth;
uniform sampler2D ShadowDepth;

void main() 
{
    float depth = G_DEPTH(GBufferDepth, v2f_Texcoord);
    vec3 position = DEPTH2POS(depth, normalize(v2f_WorldPos- CameraPos.xyz));

    vec4 cpos = ShadowMatrix * vec4(position, 1);
    cpos.xyz /= cpos.w;
    cpos.xy = cpos.xy * 0.5 + 0.5;

    vec2 texel = 1.0 / textureSize(ShadowDepth, 0).xy;
    
    float shadow = float(cpos.z < texture(ShadowDepth, cpos.xy).r);

    if (cpos.x < 0 || cpos.x > 1 || cpos.y < 0 || cpos.y > 1)
        shadow = 1;

    FragColor = shadow;
}