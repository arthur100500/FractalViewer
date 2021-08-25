#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform vec3 basecolor;
uniform sampler2D texture0;
uniform float picker_pos;

float normal(float init){
	return min(max(init, 0.0), 1.0);
}

void main()
{
	float r = 1.0 - normal(- 3.0 * abs(texCoord.x - 0.5) + 1.0);
	float g = 1.0 - normal(3.0 * abs(- texCoord.x + 2.0 / 3.0));
	float b = 1.0 - normal(3.0 * abs(- texCoord.x + 1.0 / 3.0));
	outputColor = vec4(r, g, b, 1.0);
	if (abs(picker_pos - texCoord.x) < 0.01){
		outputColor = vec4(1.0, 1.0, 1.0, 1.0);
	}
	}