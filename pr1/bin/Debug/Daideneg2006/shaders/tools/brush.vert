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
void main() {
  // base pixel
  // index in global work group
  ivec2 pixel_coords = ivec2(gl_GlobalInvocationID.xy);
  vec4 pixel = imageLoad(img_output, pixel_coords);
  if (brush_pos1 == brush_pos2);
  float calculated_len;
  vec2 AB = vec2(brush_pos2.x - brush_pos1.x, brush_pos2.y - brush_pos1.y);
  vec2 AC = vec2(pixel_coords.x - brush_pos1.x, pixel_coords.y - brush_pos1.y);
  vec2 BC = vec2(pixel_coords.x - brush_pos2.x, pixel_coords.y - brush_pos2.y);
  if (len(AB) >= len(AC) && len(AB) >= len(BC)){
	calculated_len = abs((brush_pos1.x - pixel_coords.x) * (brush_pos2.y - pixel_coords.y) - (brush_pos2.x - pixel_coords.x)*(brush_pos1.y - pixel_coords.y)) / sqrt(len(AB));
	calculated_len = (brush_width - calculated_len) / brush_width;
  }
  else{
	calculated_len = min(sqrt(len(AC)), sqrt(len(BC)));
	calculated_len = (brush_width - calculated_len) / brush_width;
  }

  
    //if (sqrt(len(AC)) <= brush_width){
	//	calculated_len =  calculated_len - sqrt(len(AC)) ;// - 1.0 + min(sqrt(len(AC)), sqrt(len(BC)));
	//}
  vec4 pix = pixel * (1.0 - calculated_len) + vec4(brush_col.x, brush_col.y, brush_col.z, 1.0) * calculated_len;
  // output to a specific pixel in the image
  if (calculated_len > 0.0 && pixel_coords.x < resolution_x && pixel_coords.x >= 0){
  imageStore(img_output, pixel_coords, pix);
  }
}