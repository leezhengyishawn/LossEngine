/******************************************************************************/
/*!
\file       Text_UI.vs
\par        Project Name: Bak-ka

\author     Seo Wee Kuan (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
layout (location = 0) in vec4 vertex; // <vec2 pos, vec2 tex>
out vec2 TexCoords;

uniform mat4 model;
uniform mat4 view;

void main()
{
  // Set position to be in view space
  gl_Position = view * model * vec4(vertex.xy, 0.0, 1.0);
  
  // Set texture coordinates data
  TexCoords = vertex.zw;
}
