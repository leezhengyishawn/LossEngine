/******************************************************************************/
/*!
\file       Blackhole.fs
\par        Project Name: GeroPero

\author     Seo Wee Kuan (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
out vec4 FragColor;

in vec2 TexCoord;
in vec2 blackholePosition;

// Texture samplers
uniform sampler2D texture1;

// Data for blackhole effects
uniform bool blackholeActive;
uniform float blackholeRadius;
uniform float blackholeAngle;
uniform float blackholeSpeed;
uniform vec2 blackholeCenter;

/******************************************************************************/
/*!
    Function that calculates the pixel colour of the current pixel coordinates
    based on the blackhole values.
 */
/******************************************************************************/
vec4 RenderSwirl()
{
  // Scale of the gameobject
  vec2 texSize = vec2(1.77777778, 1.0);
  
  // Obtain a perfect ratio for the calculations
  vec2 scaledCoords = TexCoord * texSize;
  
  // Offset current pixel from the center of the blackhole
  scaledCoords -= blackholeCenter;
  
  // Calculate length of scaledCoords
  float distance = length(scaledCoords);
  
  // Determine if current pixel is within blackhole range
  if (distance < blackholeRadius) 
  {
    // Calculate the percentage distance of the current pixel
    float percent = (blackholeRadius - distance) / blackholeRadius;
    
    // Calculate the supposed angle of the swirl effect of the current pixel
    float theta = percent * percent * blackholeAngle * 4.0f * blackholeSpeed;
                  
    // Calculate sine and cosine value
    float sine = sin(theta);
    float cosine = cos(theta);
    
    /*
     * Retrieve the position of the pixel which contains the colour to 
     * be set in the current pixel based on the swirl calculations
     */     
    scaledCoords = vec2(dot(scaledCoords, vec2(cosine, -sine)), 
                        dot(scaledCoords, vec2(sine, cosine)));
  }
  
  // Remove offset from blackhole center
  scaledCoords += blackholeCenter;
  
  // Retrieve the pixel colour of the pixel from the texture
  vec4 color = texture(texture1, scaledCoords / texSize);
  
  return color;
}

void main()
{
  vec4 texcoord = texture(texture1, TexCoord);
  
  if(blackholeActive) // If any blackhole is active
    texcoord = RenderSwirl();

  // Set pixel colour
  FragColor = texcoord;

  if (FragColor.a <= 0.0) 
  {
    discard;
  }
}