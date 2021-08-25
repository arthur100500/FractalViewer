#version 330

out vec4 outputColor;

in vec2 texCoord;
uniform sampler2D texture0;

uniform vec3 cursorpos;

void main()
{

	float diff = sqrt((cursorpos.x - texCoord.x) * (cursorpos.x - texCoord.x) * 2.0 + (cursorpos.y - texCoord.y) * (cursorpos.y - texCoord.y)) * 2.0;
	outputColor = texture(texture0, texCoord) - vec4(diff, diff, diff, 0.0);
}