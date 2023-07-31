/******************************************************************************/
/*!
\file       Grayscale.fs
\par        Project Name: Bak-ka

\author     Seo Wee Kuan (100%)
\brief      Not Used

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
  // Obtain current pixel's colour from texture
  vec4 color = texture(texture1, TexCoord);
  
  // Grayscale the pixel
  float average = (color.r + color.g + color.b) / 3.0f;
  
  // Set pixel colour to the calculated pixel's colour
  FragColor = vec4(average, average, average, 1.0);
  
  if (FragColor.a <= 0.0) 
  {
    discard;
  }
}
