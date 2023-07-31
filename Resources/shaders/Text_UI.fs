/******************************************************************************/
/*!
\file       Text_UI.fs
\par        Project Name: Bak-ka

\author     Seo Wee Kuan (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
in vec2 TexCoords;
out vec4 FragColor;

// Texture sampler
uniform sampler2D text;

// Text color
uniform vec4 color;

void main()
{
  // Compound pixel colour
  vec4 sampled = vec4(1.0, 1.0, 1.0, texture(text, TexCoords).r);
  
  // Set pixel colour
  FragColor = vec4(color) * sampled;
}
