/******************************************************************************/
/*!
\file       Sprite_Cracks.vs
\par        Project Name: Bak-ka

\author     Seo Wee Kuan (100%)
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
out vec4 FragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

void main()
{
  // Set position to be in world space
	gl_Position = projection * view * model * vec4(aPos, 1.0);
  
  // Retrieve colour values
	ourColor = color;
  
  // Set texture coordinates data
	TexCoord = vec2(aTexCoord.x, aTexCoord.y);
  
  // Retrieve position in view space
  FragPos = view * model * vec4(aPos, 1.0);
}
