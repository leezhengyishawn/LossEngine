/******************************************************************************/
/*!
\file       Sprite_FadeToBlack_Circular.fs
\par        Project Name: Bak-ka

\author     Seo Wee Kuan (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
out vec4 FragColor;

in vec2 TexCoord;

// Texture samplers
uniform sampler2D texture1;

// Render active for circular fade
uniform bool active = false;

// Fade radius
uniform float fadeRadius = 0.0f;

void main()
{
  // Initial alpha value for pixel
  float alpha = 1.0f;
  
  if(active)
  {
    // Scale coordinates based on screen ratio
    vec2 scaledCoords = (TexCoord - vec2(0.5f, 0.5f)) * vec2(0.8f, 0.5f);
    
    // Determine if current pixel is within fade range
    float distance = length(scaledCoords);
    
    if (distance < fadeRadius) // Not black coloured
      alpha = 0.0f;
    else // Black coloured
      alpha = 1.0f;
  }

  //  Set pixel colour
  FragColor = vec4(texture2D(texture1, TexCoord).rgb, alpha);
}
