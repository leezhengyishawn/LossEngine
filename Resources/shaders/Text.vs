/******************************************************************************/
/*!
\file       Text.vs
\par        Project Name: Bak-ka

\author     Allson Teo Wei Heng (100%)
\brief

All content © 2019 DigiPen Institute of Technology Singapore. All Rights
Reserved.
*/
/******************************************************************************/
#version 330 core
layout (location = 0) in vec2 pos; 
layout (location = 1) in vec2 tex; 

out vec2 TexCoords;

uniform mat4 projection;
uniform mat4 model;
uniform mat4 view;

void main()
{
    gl_Position = projection * view * model * vec4(pos, 0.0, 1.0);
    TexCoords = tex;
}  