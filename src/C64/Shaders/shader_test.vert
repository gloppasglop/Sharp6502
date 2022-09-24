#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec4 vCol;
out vec4 outCol;
        
void main()
{
	outCol = vCol;
	outCol = vec4(0.2,0.3,1.0,1.0);
        gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
}