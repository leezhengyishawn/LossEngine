/******************************************************************************/
/*!
\file       Text.fs
\par        Project Name: Bak-ka

\author     Allson Teo Wei Heng (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D text;
uniform vec4 color;

void main()
{    
  vec4 sampled = vec4(1.0, 1.0, 1.0, texture(text, TexCoords).r);
  FragColor = vec4(color) * sampled; // Add color to the text
  if (FragColor.a <= 0.0) // Discard anything with 0 alpha
  {
    discard; 
  }
}  