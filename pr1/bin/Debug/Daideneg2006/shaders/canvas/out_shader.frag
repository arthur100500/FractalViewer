#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
	outputColor = texture(texture0, vec2(texCoord.x / 2.0, texCoord.y));
}