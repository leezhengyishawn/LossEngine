#version 330 core

uniform sampler2D image;

out vec4 FragmentColor;

in vec2 TexCoord;

uniform float offset[3] = float[]( 0.0, 1.3846153846, 3.2307692308 );
uniform float weight[3] = float[]( 0.2270270270, 0.3162162162, 0.0702702703 );

void main(void)
{
	FragmentColor = texture2D(image, TexCoord) * weight[0];
	for (int i=1; i<3; i++) 
  {
		FragmentColor += texture2D( image, (vec2(TexCoord) + vec2(offset[i], 0.0) / 1920.0) ) * weight[i];
		FragmentColor += texture2D( image, (vec2(TexCoord) - vec2(offset[i], 0.0) / 1920.0) ) * weight[i];
	}
}
