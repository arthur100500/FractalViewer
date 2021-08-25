#version 430
layout(local_size_x = 1, local_size_y = 1) in;
layout(rgba32f, binding = 0) uniform image2D img_output;
uniform int resolution_x;
void main() {
  // base pixel
  // index in global work group
  ivec2 pixel_coords = ivec2(gl_GlobalInvocationID.xy);
  vec4 pix = vec4(1.0, 1.0, 1.0, 1.0);
  vec4 blank = vec4(0.0, 0.0, 0.0, 1.0);
  if (pixel_coords.x < resolution_x){
  //white to canvas
  imageStore(img_output, pixel_coords, blank);
  //transparent to storage area
  imageStore(img_output, ivec2(pixel_coords.x + resolution_x, pixel_coords.y), blank);
  }
}