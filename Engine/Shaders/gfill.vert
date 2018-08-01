VERTEX_STANDARD_LAYOUT

out vec3 v2f_WorldPos;
out vec3 v2f_Normal;
out vec2 v2f_Texcoord;

uniform mat4 ModelMatrix;

void main()
{
    vec4 worldPos = ModelMatrix * vec4(v_Position, 1);
    gl_Position = ViewProjMatrix * worldPos;
    v2f_Normal = (ModelMatrix * vec4(v_Normal, 0)).xyz;
    v2f_Texcoord = v_Texcoord.xy;
    v2f_WorldPos = worldPos.xyz;
}