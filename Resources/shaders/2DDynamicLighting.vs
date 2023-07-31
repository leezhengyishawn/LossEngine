/******************************************************************************/
/*!
\file       2DDynamicLighting.vs
\par        Project Name: GeroPero

\author     Seo Wee Kuan (100%)
\brief      Basic 2D dynamic lighting shader

All content © 2020 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
#define MAX_NUM_TOTAL_LIGHTS 16
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec4 ourColor;
out vec2 TexCoord;
out vec3 FragPos;
out vec3 Normal;
out vec3 unitLightPos[MAX_NUM_TOTAL_LIGHTS];

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

uniform int lightCount;
uniform vec3 cameraPos;
uniform vec3 lightPos[MAX_NUM_TOTAL_LIGHTS];

void main()
{
  gl_Position = projection * view * model * vec4(aPos, 1.0);
  ourColor = color;
  TexCoord = vec2(aTexCoord.x, aTexCoord.y);
  
  for(int i = 0; i < lightCount && i < MAX_NUM_TOTAL_LIGHTS; ++i)
  {
    unitLightPos[i] = vec3(vec4((lightPos[i].x - cameraPos.x) * 1.867777778, 
                                (lightPos[i].y - cameraPos.y) * 1.867777778, 
                                 lightPos[i].z, 
                                 1.0));
  }

  // Pixel position
  FragPos = vec3(vec4(aPos.x * 1.777777778, -aPos.y, aPos.z, 1.0)) * cameraPos.z;

  // Set normal for 2D
  Normal = vec3(0.0, 0.0, 1.0);
}
