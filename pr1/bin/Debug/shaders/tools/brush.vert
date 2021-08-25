#version 430
layout(local_size_x = 1, local_size_y = 1) in;
layout(rgba32f, binding = 0) uniform image2D img_output;
uniform vec4 brush_col;
uniform vec2 brush_pos1;
uniform vec2 brush_pos2;
uniform float brush_width;
uniform int resolution_x;
float len(vec2 arg){
	return arg.x * arg.x + arg.y * arg.y;
}
ivec2 anoter_sc(ivec2 inital){
	return ivec2(inital.x + resolution_x, inital.y);
}
void main() {
  // base pixel
  // index in global work group
  ivec2 pixel_coords = ivec2(gl_GlobalInvocationID.xy);
  
  if (brush_pos1 == brush_pos2);
  float calculated_len;
  vec2 AB = vec2(brush_pos2.x - brush_pos1.x, brush_pos2.y - brush_pos1.y);
  vec2 AC = vec2(pixel_coords.x - brush_pos1.x, pixel_coords.y - brush_pos1.y);
  vec2 BC = vec2(pixel_coords.x - brush_pos2.x, pixel_coords.y - brush_pos2.y);
  if (len(AB) >= len(AC) && len(AB) >= len(BC)){
	calculated_len = abs((brush_pos1.x - pixel_coords.x) * (brush_pos2.y - pixel_coords.y) - (brush_pos2.x - pixel_coords.x)*(brush_pos1.y - pixel_coords.y)) / sqrt(len(AB));
	
  }
  else{
	calculated_len = min(sqrt(len(AC)), sqrt(len(BC)));
  }
  calculated_len = (brush_width - calculated_len) / brush_width;
  if (calculated_len >= 0.0 && pixel_coords.x < resolution_x && pixel_coords.x >= 0){

	
    //normal painting
	float mask = imageLoad(img_output, anoter_sc(pixel_coords)).r;
	vec4 pixel = imageLoad(img_output, pixel_coords);
	imageStore(img_output, pixel_coords, mix(pixel, brush_col, calculated_len *(1.0 - mask * 0.9)));
	
	//adding info to buffer
	pixel = imageLoad(img_output, anoter_sc(pixel_coords));
	imageStore(img_output, anoter_sc(pixel_coords), mix(pixel, vec4(1.0, 0.0, 0.0, 1.0), calculated_len));

  }
}