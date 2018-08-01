VERTEX_STANDARD_LAYOUT

out vec2 v2f_Texcoord;
out vec3 v2f_WorldPos;
out vec4 v2f_FragPos;

void main()
{
    gl_Position = vec4(v_Position, 1);
    v2f_FragPos = gl_Position;
    vec4 wpos = InvViewProjMatrix * gl_Position;
    v2f_WorldPos = wpos.xyz / wpos.w;
    v2f_Texcoord = v_Texcoord.xy;
}