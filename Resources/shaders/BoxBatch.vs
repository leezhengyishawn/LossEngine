/******************************************************************************/
/*!
\file       Basic.vs
\par        Project Name: Bak-ka

\author     Allson Teo Wei Heng (100%)
\brief      Used for basic shapes

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec4 aColor;
layout (location = 2) in mat4 aModel;

out vec4 ourColor;

uniform mat4 view;
uniform mat4 projection;

void main()
{
	gl_Position = projection * view * aModel * vec4(aPos, 1.0);
	ourColor = aColor;
}