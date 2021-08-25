#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform vec3 basecolor;
uniform sampler2D texture0;
uniform vec2 picker_pos;

float normal(float init){
	return min(max(init, 0.0), 1.0);
}
void main()
{
	float r = normal(basecolor.r + texCoord.x) * texCoord.y;
	float g = normal(basecolor.g + texCoord.x) * texCoord.y;
	float b = normal(basecolor.b + texCoord.x) * texCoord.y;
	outputColor = vec4(r, g, b, 1.0);
	float range = (texCoord.x - picker_pos.x) * (texCoord.x - picker_pos.x) + (texCoord.y - picker_pos.y) *(texCoord.y - picker_pos.y);
	if (range < 0.0025 && range > 0.0015){
	outputColor = vec4(1.0, 1.0, 1.0, 1.0);
	}
}