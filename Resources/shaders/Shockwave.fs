/******************************************************************************/
/*!
\file       Shockwave.fs
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
in vec2 wavePosition;

// Texture samplers
uniform sampler2D texture1;

uniform vec3 cameraPos;

uniform bool waveActive;
uniform float waveLifeTime;
uniform vec2 waveCenter;
uniform float waveAmplitude;
uniform float waveRefraction;
uniform float waveWidth;
uniform float waveSpeed;
uniform float waveReduction;

void main()
{
  // Store current pixel position
  vec2 uv = TexCoord;
  
  // Store initial pixel colour
  vec4 texcoord = texture(texture1, uv);
  
  if(waveActive)
  {
    // Determine shockwave periods (1 second = 1 wave)
    float waveTime = waveLifeTime * waveSpeed;
    float offset = (waveTime - floor(waveTime)) / waveTime;
	
	// Time is this code does not necessary represent time in seconds,
	// it also acts acts as the current "radius" of the shockwave
    float time = waveTime * offset;
  
    // Treat camera as 0.5, 0.5
    vec2 center = wavePosition + vec2(0.5, 0.5) * vec2(1.7777777778, 1.0);
  
    // Distance from pixel to the determined center
    float dist = distance(uv * vec2(1.7777777778, 1.0), center);
    
    // Limit wave distortion effect to pixels in the shockwave "width"
    if (time > 0 && dist <= time + waveWidth && dist >= time - waveWidth) 
    {
      // Calculate offset distance for the pixels based on shockwave attributes
      float diff = (dist - time);
      float diffPow = (1.0 - pow(abs(diff * waveAmplitude), waveRefraction));
      float diffTime = (diff * diffPow);
  
      // Retrieve the direction of the distortion effect
      vec2 dir = normalize(uv - center);
      
      // Perform the distortion and reduce the distortion effect over time
      uv += ((dir * diffTime) / (time * dist * waveReduction));
          
      // Grab color for the new coord
      texcoord = texture(texture1, uv);
    }
  }
  
  FragColor = texcoord;
}




  //vec2 ratio = vec2(1.77777778, 1.0);
  //
  //// Scale pixel based on ratio
  //vec2 texcoord = TexCoord;
  //
  //// Iterate through all waves
  //if(waveActive)
  //{
  //  // Calculate length of scaledCoords
  //  float distance = distance(texcoord, wavePosition);
  //  
  //  // Determine if current pixel is within shockwave range
  //  if ( (distance <= (waveRadius + waveWidth)) && 
  //      (distance >= (waveRadius - waveWidth)) ) 
  //  {
  //    // Calculate distance of pixel from the center of the shockwave radius
  //    float diffDistance = (distance - waveWidth); 
  //    
  //    // Calculate the distortion amount
  //    float powDiff = 1.0f - pow(abs(diffDistance * waveAmplitude), waveRefraction); 
  //                              
  //    float diffTime = diffDistance * powDiff;           // Distortion length
  //    vec2 diffUV = normalize(texcoord - wavePosition);  // Direction 
  //    texcoord += (diffUV * diffTime);         // Distort pixel
  //  }
  //}
  
  // Set the pixel colour of the pixel from the texture
  //FragColor = texture2D(texture1, texcoord / vec2(1920, 1080));
  //if (FragColor.a <= 0.0) 
  //{
  //  discard;
  //}
  

// Screen Resolution
//uniform float screenRatioX = 1.6f;
//uniform float screenRatioY = 1.0f;
//
//// Padding for the screen (black borders)
//uniform float paddingX = 0.0f;
//
//// Shockwave values
//uniform bool waveActive[10];
//uniform float waveRadius[10];
//uniform vec2 waveCenter[10];
//uniform float waveAmplitude[10];
//uniform float waveRefraction[10];
//uniform float waveWidth[10];
//
//void main()
//{
//  // Ensure that pixel is within camera view
//  if(TexCoord.x > paddingX && TexCoord.x < 1.0f - paddingX)
//  {
//    vec2 ratio = vec2(screenRatioX, screenRatioY);
//    
//    // Scale pixel based on ratio
//    vec2 texcoord = TexCoord * ratio;
//    
//    // Iterate through all waves
//    for(int i = 0; i < 10; ++i)
//    {
//      if(waveActive[i])
//      {
//        // Calculate length of scaledCoords
//        float distance = distance(texcoord, waveCenter[i]);
//        
//        // Determine if current pixel is within shockwave range
//        if ( (distance <= (waveRadius[i] + waveWidth[i])) && 
//             (distance >= (waveRadius[i] - waveWidth[i])) ) 
//        {
//          // Calculate distance of pixel from the center of the shockwave radius
//          float diffDistance = (distance - waveRadius[i]); 
//          
//          // Calculate the distortion amount
//          float powDiff = 1.0f - pow(abs(diffDistance * waveAmplitude[i]), 
//                                     waveRefraction[i]); 
//                                     
//          float diffTime = diffDistance * powDiff;           // Distortion length
//          vec2 diffUV = normalize(texcoord - waveCenter[i]); // Direction 
//          texcoord = texcoord + (diffUV * diffTime);         // Distort pixel
//        }
//      }
//    }
//  
//    // Set the pixel colour of the pixel from the texture
//    FragColor = texture2D(texture1, texcoord / ratio);
//  }
//  else // Not in camera view (black borders)
//  {
//    // Use default pixel colours
//    FragColor = texture2D(texture1, TexCoord);
//  }
//    
//  if (FragColor.a <= 0.0) 
//  {
//    discard;
//  }
//}