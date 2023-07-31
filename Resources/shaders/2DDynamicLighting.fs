/******************************************************************************/
/*!
\file       2DDynamicLighting.fs
\par        Project Name: GeroPero

\author     Seo Wee Kuan (100%)
\brief      Basic 2D dynamic lighting shader

All content © 2020 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
#define MAX_NUM_TOTAL_LIGHTS 16
out vec4 FragColor;

in vec4 ourColor;
in vec2 TexCoord;
in vec3 FragPos;
in vec3 Normal;
in vec3 unitLightPos[MAX_NUM_TOTAL_LIGHTS];

// texture samplers
uniform sampler2D texture1;

// Light values
uniform bool multiplicative;
uniform int lightCount;
uniform vec4 lightColor[MAX_NUM_TOTAL_LIGHTS];
uniform vec4 ambientColor[MAX_NUM_TOTAL_LIGHTS];
//uniform bool multiplicative[MAX_NUM_TOTAL_LIGHTS];
uniform int type[MAX_NUM_TOTAL_LIGHTS];
uniform float intensity[MAX_NUM_TOTAL_LIGHTS];
uniform float ambientStrength[MAX_NUM_TOTAL_LIGHTS];
uniform float diffuseStrength[MAX_NUM_TOTAL_LIGHTS];
//uniform float specularStrength[MAX_NUM_TOTAL_LIGHTS];
//uniform float shininess[MAX_NUM_TOTAL_LIGHTS];
//uniform float attenConstant[MAX_NUM_TOTAL_LIGHTS];
uniform float attenLinear[MAX_NUM_TOTAL_LIGHTS];
uniform float attenQuadratic[MAX_NUM_TOTAL_LIGHTS];
uniform float attenUnit[MAX_NUM_TOTAL_LIGHTS];
uniform float lightInnerLimit[MAX_NUM_TOTAL_LIGHTS];
uniform float lightOuterLimit[MAX_NUM_TOTAL_LIGHTS];
uniform vec3 cameraPos;

//vec3 CalculateDirectionLight(int index);
//vec3 CalculatePointLight(int index);
//vec3 CalculateSpotLight(int index);

vec3 CalculateDirectionLight(vec4 lightC, vec4 ambientC, float ambientS, float diffuseS);
vec3 CalculatePointLight(vec4 lightC, vec4 ambientC, float ambientS, float diffuseS, vec3 lightP, float aUnit, float aLinear, float aQuad);
vec3 CalculateSpotLight(vec4 lightC, vec4 ambientC, float ambientS, float diffuseS, float inner, float outer, vec3 lightP);

void main()
{
  vec4 texcoord = texture(texture1, TexCoord) * ourColor;

  if(lightCount > 0)
  {
    vec3 multiplicativeColour = vec3(0.0f, 0.0f, 0.0f);

    for(int i = 0; i < lightCount && i < MAX_NUM_TOTAL_LIGHTS; ++i)
    {
      if(type[i] == 0)
        multiplicativeColour += CalculateDirectionLight(lightColor[i], ambientColor[i], ambientStrength[i], diffuseStrength[i]) * intensity[i];
      else if(type[i] == 1)
        multiplicativeColour += CalculatePointLight(lightColor[i], ambientColor[i], ambientStrength[i], diffuseStrength[i], unitLightPos[i], attenUnit[i], attenLinear[i], attenQuadratic[i]) * intensity[i];
      else if(type[i] == 2)
        multiplicativeColour += CalculateSpotLight(lightColor[i], ambientColor[i], ambientStrength[i], diffuseStrength[i], lightInnerLimit[i], lightOuterLimit[i], unitLightPos[i]) * intensity[i];
    }

    if(multiplicative)
      texcoord *= vec4(multiplicativeColour, 1.0);
    else
      texcoord += vec4(multiplicativeColour, 0.0);
      
    clamp(texcoord.r, 0.0, 255.0);
    clamp(texcoord.g, 0.0, 255.0);
    clamp(texcoord.b, 0.0, 255.0);
  }
  
  FragColor = texcoord;
  if (FragColor.a <= 0.0) // Discard anything with 0 alpha
  {
    discard; 
  }
}

// Multipass versions
//vec3 CalculateDirectionLight(int index)
//{
//  vec3 color = lightColor[index].rgb;
//  
//  // Ambient light calculation
//  //vec3 ambient = ambientStrength[index] * ambientColor[index].rgb;
//  
//  // Diffuse light calculation
//  vec3 norm = normalize(Normal);
//  vec3 lightDir = normalize(vec3(0.0f, 0.0f, 1.0f));
//  float diff = max(dot(norm, lightDir), 0.0f);
//  vec3 diffuse = diffuseStrength[index] * diff * color;
//
//  return diffuse;
//}

vec3 CalculateDirectionLight(vec4 lightC, vec4 ambientC, float ambientS, float diffuseS)
{
  vec3 color = lightC.rgb;
  
  // Ambient light calculation
  vec3 ambient = ambientS * ambientC.rgb;
  
  // Diffuse light calculation
  vec3 norm = normalize(Normal);
  vec3 lightDir = normalize(vec3(0.0f, 0.0f, 1.0f));
  float diff = max(dot(norm, lightDir), 0.0f);
  vec3 diffuse = diffuseS * diff * color;

  return ambient + diffuse;
  //return diffuse;
}

//vec3 CalculatePointLight(int index)
//{
//  vec3 color = lightColor[index].rgb;
//
//  // Vector from pixel to light source
//  vec3 lightToPixel = unitLightPos[index] - FragPos;
//  
//  // Ambient light calculation
//  //vec3 ambient = ambientStrength[index] * ambientColor[index].rgb;
//
//  // Diffuse light calculation
//  vec3 norm = normalize(Normal);
//  vec3 lightDir = normalize(lightToPixel);
//  float diff = max(dot(norm, lightDir), 0.0f);
//  vec3 diffuse = diffuseStrength[index] * diff * color;
//  
//  // Attentuation for lighting falloff
//  //vec3(lightToPixel.x, lightToPixel.y, 0.0)
//  float distance = length(lightToPixel) / attenUnit[index];
//  float attenuation = 1.0f / (1.0 + attenLinear[index] * distance + 
//                      attenQuadratic[index] * (distance * distance));
//
//  //ambient *= attenuation;
//  diffuse *= attenuation;
//  
//  //return ambient + diffuse;
//  return diffuse;
//}

vec3 CalculatePointLight(vec4 lightC, vec4 ambientC, float ambientS, float diffuseS, vec3 lightP, float aUnit, float aLinear, float aQuad)
{
  vec3 color = lightC.rgb;

  // Vector from pixel to light source
  vec3 lightToPixel = lightP - FragPos;
  
  // Ambient light calculation
  vec3 ambient = ambientS * ambientC.rgb;

  // Diffuse light calculation
  vec3 norm = normalize(Normal);
  vec3 lightDir = normalize(lightToPixel);
  float diff = max(dot(norm, lightDir), 0.0f);
  vec3 diffuse = diffuseS * diff * color;
  
  // Attentuation for lighting falloff
  //vec3(lightToPixel.x, lightToPixel.y, 0.0)
  float distance = length(lightToPixel) / aUnit;
  float attenuation = 1.0f / (1.0 + aLinear * distance + aQuad * (distance * distance));
  
  ambient *= attenuation;
  diffuse *= attenuation;
  
  return ambient + diffuse;
  //return diffuse;
}

//vec3 CalculateSpotLight(int index)
//{
//  vec3 color = lightColor[index].rgb;
//
//  // Vector from pixel to light source
//  vec3 lightToPixel = unitLightPos[index] - FragPos;
//  
//  vec3 lightDir = normalize(lightToPixel);
//  
//  // Determine cutoff "angle"
//  float angle = dot(lightDir, normalize(-vec3(0.0f, 0.0f, -1.0f)));
//  float intensity = smoothstep(lightOuterLimit[index], lightInnerLimit[index], angle);
//  
//  // Ambient light calculation
//  vec3 ambient = ambientStrength[index] * ambientColor[index].rgb;
//  
//  // Resultant colour
//  vec3 result;
//  
//  if(angle > lightOuterLimit[index])
//  {
//    // Diffuse light calculation
//    vec3 norm = normalize(Normal);
//    float diff = max(dot(norm, lightDir), 0.0f);
//    vec3 diffuse = diffuseStrength[index] * diff * color;
//
//    diffuse *= intensity;
//  
//    result = diffuse;
//  }
//  
//  return result + ambient;
//}

vec3 CalculateSpotLight(vec4 lightC, vec4 ambientC, float ambientS, float diffuseS, float inner, float outer, vec3 lightP)
{
  vec3 color = lightC.rgb;
  
  // Vector from pixel to light source
  vec3 lightToPixel = lightP - FragPos;
  
  vec3 lightDir = normalize(lightToPixel);
  
  // Determine cutoff "angle"
  float angle = dot(lightDir, normalize(-vec3(0.0f, 0.0f, -1.0f)));
  float intensity = smoothstep(outer, inner, angle);
  
  // Ambient light calculation
  vec3 ambient = ambientS * ambientC.rgb;
  
  // Resultant colour
  vec3 result;
  
  if(angle > outer)
  {
    // Diffuse light calculation
    vec3 norm = normalize(vec3(0.0, 0.0, 1.0));
    float diff = max(dot(norm, lightDir), 0.0f);
    vec3 diffuse = diffuseS * diff * color;
  
    diffuse *= intensity;
    
    result = diffuse; // + specular;
  }
  
  return result + ambient;
  //return result;
}
