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
  vec4 pixel = imageLoad(img_output, pixel_coords);
  vec4 pixel8 = imageLoad(img_output, ivec2(pixel_coords.x + 1, pixel_coords.y + 1));
  vec4 pixel7 = imageLoad(img_output, ivec2(pixel_coords.x + 1, pixel_coords.y));
  vec4 pixel6 = imageLoad(img_output, ivec2(pixel_coords.x + 1, pixel_coords.y - 1));
  vec4 pixel5 = imageLoad(img_output, ivec2(pixel_coords.x, pixel_coords.y + 1));
  vec4 pixel4 = imageLoad(img_output, ivec2(pixel_coords.x, pixel_coords.y - 1));
  vec4 pixel3 = imageLoad(img_output, ivec2(pixel_coords.x - 1, pixel_coords.y + 1));
  vec4 pixel2 = imageLoad(img_output, ivec2(pixel_coords.x - 1, pixel_coords.y));
  vec4 pixel1 = imageLoad(img_output, ivec2(pixel_coords.x - 1, pixel_coords.y - 1));
  int c = 0;
  bool alive = false;
  bool should = false;
  if (pixel1.r + pixel1.g + pixel1.b > 1.5) c += 1;
  if (pixel2.r + pixel2.g + pixel2.b > 1.5) c += 1;
  if (pixel3.r + pixel3.g + pixel3.b > 1.5) c += 1;
  if (pixel4.r + pixel4.g + pixel4.b > 1.5) c += 1;
  if (pixel5.r + pixel5.g + pixel5.b > 1.5) c += 1;
  if (pixel6.r + pixel6.g + pixel6.b > 1.5) c += 1;
  if (pixel7.r + pixel7.g + pixel7.b > 1.5) c += 1;
  if (pixel8.r + pixel8.g + pixel8.b > 1.5) c += 1;
  if (pixel.r + pixel.g + pixel.b > 1.5) alive = true;
  if (c <= 1) should = false;
  if (c >= 4) should = false;
  if (c == 3) should = true;
  if (c == 2) should = alive;
  if (alive == should) imageStore(img_output, ivec2(pixel_coords.x + resolution_x, pixel_coords.y), vec4(pixel.r, pixel.g, pixel.b, 1.0));
  else imageStore(img_output, ivec2(pixel_coords.x + resolution_x, pixel_coords.y), vec4(1.0 - pixel.r, 1.0 - pixel.g, 1.0 - pixel.b, 1.0));
}