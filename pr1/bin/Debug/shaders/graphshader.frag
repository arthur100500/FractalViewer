#version 330
precision highp float;
out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

uniform float r_multiplier;
uniform int maxIterations;
uniform float idk_y;
uniform float idk_x;

float clearify(float in_c){
	if (pow(in_c, 2) < 0.1){
		return 1.0;
	}
	return 0.0;
}
vec4 get_color_gradient(float x){
	x = x * r_multiplier;
	return vec4(x - 2.0, x , x- 3.0, 1.0);
}

vec2 squareImaginary(vec2 number){
	return vec2(
		pow(number.x,2)-pow(number.y,2),
		2*number.x*number.y
	);
}

float iterateJulia(vec2 coord){
    vec2 z;
	vec2 c = vec2(idk_x, idk_y);
    z.x = 3.0 * (coord.x - 0.5);
    z.y = 2.0 * (coord.y - 0.5);

    int i;
    for(i=0; i<maxIterations; i++) {
        float x = (z.x * z.x - z.y * z.y) + c.x;
        float y = (z.y * z.x + z.x * z.y) + c.y;

        if((x * x + y * y) > 4.0) break;
        z.x = x;
        z.y = y;
    }
	
	return i;
}
float iterateMandelbrot(vec2 coord){
	vec2 z = vec2(0,0);
	for(int i=0;i<maxIterations;i++){
		z = squareImaginary(squareImaginary(z) + coord) + squareImaginary(z) + coord;
		if(length(z)>2) return i;
	}
	return maxIterations;
}

void main()
{
	outputColor = texture(texture0, texCoord);
	// wirazheniye x*2 - sin(x) + 2 * (y * y) - y 
	float x = texCoord.x * 10 - 5;
	float y = texCoord.y * 10 - 5;
	vec2 complex_number = vec2(x, y);
	float col = 2 * sin(x) / y - y;
	outputColor = vec4(vec3(0.0) + get_color_gradient(iterateMandelbrot(complex_number)).rgb, 1.0);
	//outputColor = vec4(1.0 - abs(col)) + 0.00001 * outputColor;

}