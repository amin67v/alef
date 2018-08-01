in vec4 v2f_FragPos;

out float FragColor;

uniform float Bias;

void main()
{
    FragColor = (v2f_FragPos.z / v2f_FragPos.w) + Bias;
}