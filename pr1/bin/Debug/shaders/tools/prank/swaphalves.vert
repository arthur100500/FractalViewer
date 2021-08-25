#version 430
layout(local_size_x = 1, local_size_y = 1) in;
layout(rgba32f, binding = 0) uniform image2D img_output;
uniform vec4 brush_col;
uniform vec2 brush_pos1;
uniform vec2 brush_pos2;
uniform float brush_width;
uniform int resolution_x;

void main() {
  ivec2 pixel_coords = ivec2(gl_GlobalInvocationID.xy);
  vec4 pixel = imageLoad(img_output, ivec2(pixel_coords.x + resolution_x, pixel_coords.y));
  imageStore(img_output, pixel_coords, pixel);
}