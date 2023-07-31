/******************************************************************************/
/*!
\file       Sprite_UI.fs
\par        Project Name: Bak-ka

\author     Seo Wee Kuan (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
out vec4 FragColor;

in vec4 ourColor;
in vec2 TexCoord;

// Texture samplers
uniform sampler2D texture1;

void main()
{
  // Set pixel colour
	FragColor = texture(texture1, TexCoord) * ourColor;
  
  if (FragColor.a <= 0.0) 
  {
    discard;
  }
}