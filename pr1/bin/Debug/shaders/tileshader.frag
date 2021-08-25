#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

uniform float selected;

void main()
{
	outputColor = texture(texture0, texCoord);
	outputColor.r += selected;
}