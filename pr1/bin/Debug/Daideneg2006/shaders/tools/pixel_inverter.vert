#version 430
layout(local_size_x = 1, local_size_y = 1) in;
layout(rgba32f, binding = 0) uniform image2D img_output;
uniform vec4 brush_col;
uniform vec2 brush_pos2;
uniform float brush_width;
uniform int resolution_x;

void main() {
  // base pixel
  // index in global work group
  ivec2 pixel_coords = ivec2(gl_GlobalInvocationID.xy);
  vec4 pixel = imageLoad(img_output, pixel_coords);
  if (pixel_coords == brush_pos2){
  imageStore(img_output, pixel_coords, vec4(1.0 - pixel.r, 1.0 - pixel.g, 1.0 - pixel.b, 1.0));
  }
}