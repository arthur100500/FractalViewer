#version 330

out vec4 outputColor;

in vec2 texCoord;
uniform sampler2D texture0;
// mousex mousey parralax val
uniform vec3 parallax_info;

void main()
{
	float x = (-0.5 + ((0.5 - parallax_info.x) * parallax_info.z + texCoord.x)) / (1.0 + parallax_info.z * 2.0) + 0.5;
	float y = (-0.5 + ((0.5 - parallax_info.y) * parallax_info.z + texCoord.y)) / (1.0 + parallax_info.z * 2.0) + 0.5;
	outputColor = texture(texture0, vec2(x, y));
}