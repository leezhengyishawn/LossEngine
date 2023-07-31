/******************************************************************************/
/*!
\file       Grayscale.vs
\par        Project Name: Bak-ka

\author     Seo Wee Kuan (100%)
\brief      Not Used

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 TexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

void main()
{
  // Set position to be in screen space
	gl_Position = vec4(aPos.x, aPos.y, 0.0, 1.0);
  
  // Set texture coordinates data
	TexCoord = vec2(aTexCoord.x, aTexCoord.y);
}
