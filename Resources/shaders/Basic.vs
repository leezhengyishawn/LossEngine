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

out vec4 ourColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

void main()
{
	gl_Position = projection * view * model * vec4(aPos, 1.0);
	ourColor = color;
}