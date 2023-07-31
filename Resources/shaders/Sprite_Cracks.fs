/******************************************************************************/
/*!
\file       Sprite_Cracks.fs
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
in vec4 FragPos;

// Texture samplers
uniform sampler2D texture1;
uniform sampler2D normalTexture;

// Crack values
uniform vec2 contactPoints[20];
uniform bool contactActive[20];
uniform float crackRadius[20];
uniform float crackAlpha[20];

uniform float crackSpeed;

void main()
{
  // Calculate normal values from crack texture
  vec3 normal = texture(normalTexture, TexCoord).rgb;
  normal = normalize(normal * 2.0 - 1.0);
  
  // Retrieve pixel colour from texture
  vec3 color0 = texture(texture1, TexCoord).rgb;
  
  vec3 mixture = color0;
  float alphaVal = 1.0f;
  
  // Iterate through all crack effects
  for(int index = 0; index < 20; ++index) 
  {
    if(contactActive[index])
    {
      // Determine if current pixel is within crack range
      if(length(contactPoints[index] - FragPos.xy) < crackRadius[index])
      {
        // For line art textures
        /*
        if((normal.r + normal.g + normal.b) > 0.0001f)
        {
          mixture = mix(mixture, normal, 10.0f);
          //mixture = vec3(0.0f, 0.0f, 0.0f);
        }
        */
        // For normal map textures
        if(normal.z <= 0.8f)
        {
          // Blend the two textures together
          mixture = mix(mixture, vec3(0.0f, 0.0f, 0.0f), crackAlpha[index]);
        }
      }
    }
  }
  
  // Set pixel colour
  FragColor = vec4(mixture, 1.0f);
  if (FragColor.a <= 0.0f) 
  {
    discard;
  }
}
