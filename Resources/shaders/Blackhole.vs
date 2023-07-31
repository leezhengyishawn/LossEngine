/******************************************************************************/
/*!
\file       Blackhole.vs
\par        Project Name: GeroPero

\author     Seo Wee Kuan (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 TexCoord;
out vec2 blackholePosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

uniform vec2 blackholeCenter;
uniform vec3 cameraPos;

void main()
{
  // Set position to be in world space
	gl_Position = projection * view * model * vec4(aPos, 1.0);
  
  // Set texture coordinates data
	TexCoord = vec2(aTexCoord.x, aTexCoord.y);
  
  blackholePosition = vec2((blackholeCenter.x - cameraPos.x) * 2,
                           (blackholeCenter.y - cameraPos.y) * 2);
                      
  blackholePosition /= cameraPos.z;
}
