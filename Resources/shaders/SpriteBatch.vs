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
layout (location = 2) in vec4 aColor;
layout (location = 3) in mat4 aModel;

out vec4 ourColor;
out vec2 TexCoord;

uniform mat4 view;
uniform mat4 projection;

void main()
{
  gl_Position = projection * view * aModel * vec4(aPos, 1.0);
	ourColor = aColor;
  TexCoord = vec2(aTexCoord.x, aTexCoord.y);
}