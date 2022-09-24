#version 330 core
out vec4 FragColor;

in vec4 outCol;
void main()
{
    FragColor = outCol;
}