VERTEX_STANDARD_LAYOUT

out vec4 v2f_FragPos;

uniform mat4 ModelMatrix;

void main()
{
    gl_Position = ShadowMatrix * ModelMatrix * vec4(v_Position, 1);
    v2f_FragPos = gl_Position;
}