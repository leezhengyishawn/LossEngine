/******************************************************************************/
/*!
\file       Sprite.fs
\par        Project Name: Bak-ka

\author     Allson Teo Wei Heng (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec4 ourColor;
out vec2 TexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

void main()
{
  gl_Position = projection * view * model * vec4(aPos, 1.0);
	ourColor = color;
	TexCoord = vec2(aTexCoord.x, aTexCoord.y);
}