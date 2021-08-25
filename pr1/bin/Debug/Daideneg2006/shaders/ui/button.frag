#version 330

out vec4 outputColor;

in vec2 texCoord;
uniform sampler2D texture0;
uniform float mouse_hover;

void main()
{
	outputColor = mix(texture(texture0, texCoord), vec4(0.5, 0.6, 0.7, 0.9), mouse_hover * 0.2);
}