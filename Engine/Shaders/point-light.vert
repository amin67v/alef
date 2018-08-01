VERTEX_STANDARD_LAYOUT

out vec3 v2f_WorldPos;
out vec4 v2f_FragPos;

uniform mat4 ModelMatrix;

void main()
{
    vec4 worldPos = ModelMatrix * vec4(v_Position, 1);
    gl_Position =  ViewProjMatrix * worldPos;
    v2f_FragPos = gl_Position;
    v2f_WorldPos = worldPos.xyz;
}