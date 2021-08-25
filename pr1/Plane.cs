using OpenTK.Graphics.OpenGL;
using System;

namespace FractalRenderer
{
    public class Plane
    {
        private int _vertexArrayObject;
        private int _elementBufferObject;
        private int _vertexBufferObject;
        public float[] zoom_info;
        public float zoom_coeff = 0.2f;
        public float[] zoom_coord;
        public bool zoom_limited = true;
        public float[] _vertices { get; private set; }
        public Shader _shader;
        public Texture _texture;
        public int textureloc = -1;
        public bool zoomable = true;
        public static readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };
        public Plane(float[] coordinates, Shader shader, Texture texture)
        {
            _vertices = coordinates;
            _shader = shader;
            _texture = texture;
            zoom_info = new float[] { _vertices[0], _vertices[15], _vertices[1], _vertices[6] };
        }
        public Plane(float[] coordinates, Shader shader, int texture_loc)
        {
            _vertices = coordinates;
            _shader = shader;
            textureloc = texture_loc;
            zoom_info = new float[] { _vertices[0], _vertices[15], _vertices[1], _vertices[6] };
        }
        public void ReshapeWithVertexarray(float[] new_coordinates)
        {
            _vertices = new_coordinates;
            zoom_info = new float[] { _vertices[0], _vertices[15], _vertices[1], _vertices[6] };
        }
        public void ReshapeWithCoords(float top_x, float top_y, float bottom_x, float bottom_y)
        {
            _vertices = new float[]
            {
                         -top_x,  top_y, 0f, _vertices[3], _vertices[4], // top right
                         -top_x, bottom_y, 0f, _vertices[8], _vertices[9], // bottom right
                        -bottom_x, bottom_y, 0f, _vertices[13], _vertices[14], // bottom left
                        -bottom_x,  top_y, 0f, _vertices[18], _vertices[19]  // top left
            };
            zoom_info = new float[] { _vertices[0], _vertices[15], _vertices[1], _vertices[6] };
        }

        public void Render()
        {
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
            if (textureloc < 0)
                _texture.Use(TextureUnit.Texture0);
            else
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, textureloc);
            }
            _shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }


        public void Load()
        {

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            if (textureloc < 0)
                _texture.Use(TextureUnit.Texture0);
            else
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, textureloc);
            }
        }
        public void Unload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);

            GL.DeleteProgram(_shader.Handle);
            if (textureloc < 0)
            {
                GL.DeleteTexture(_texture.Handle);
            }
            else
            {
                GL.DeleteTexture(textureloc);
            }
        }

        public float[] GetRelativeCursorPosition(Window window, int x, int y)
        {
            //position relative to window
            float xpos = -1 + (-window.Location.X + x - 8) / (float)window.Width * 2;
            float ypos = 1 - (-window.Location.Y + y - 30) / (float)window.Height * 2;
            //position relative toplane
            //zoom_info = new float[] { _vertices[0], _vertices[15], _vertices[1], _vertices[6] };
            xpos = 1f - (_vertices[0] - xpos) / (_vertices[0] - _vertices[15]);
            ypos = 1f - (_vertices[1] - ypos) / (_vertices[1] - _vertices[6]);
            return new float[] { xpos, ypos };
        }

        public float[] GetZoomRelativeCursorPosition(Window window, int x, int y)
        {
            float xpos = -1 + (-window.Location.X + x - 8) / (float)window.Width * 2;
            float ypos = 1 - (-window.Location.Y + y - 30) / (float)window.Height * 2;
            xpos = 1f - (_vertices[0] - xpos) / (_vertices[0] - _vertices[15]);
            xpos = _vertices[13] - xpos * (_vertices[13] - _vertices[3]);
            ypos = 1f - (_vertices[1] - ypos) / (_vertices[1] - _vertices[6]);
            ypos = _vertices[9] - ypos * (_vertices[9] - _vertices[4]);
            return new float[] { xpos, ypos };
        }


        public void ZoomIn(float[] cursor_coords)
        {
            zoom_coeff *= 0.99f;
            _vertices = new float[] { _vertices[0], _vertices[1], _vertices[2], Zoom_Normalize((_vertices[3] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff)), Zoom_Normalize((_vertices[4] + cursor_coords[1] * zoom_coeff) / (1f + zoom_coeff)),
                                                 _vertices[5], _vertices[6], _vertices[7], Zoom_Normalize((_vertices[8] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff)), Zoom_Normalize((_vertices[9] + cursor_coords[1] * zoom_coeff) / (1f + zoom_coeff)),
                                                 _vertices[10], _vertices[11], _vertices[12], Zoom_Normalize((_vertices[13] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff)), Zoom_Normalize((_vertices[14] + cursor_coords[1] * zoom_coeff) / (1f + zoom_coeff)),
                                                 _vertices[15], _vertices[16], _vertices[17],Zoom_Normalize ((_vertices[18] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff)), Zoom_Normalize((_vertices[19] + cursor_coords[1] * zoom_coeff) / (1f + zoom_coeff))};
            zoom_info = new float[] { (zoom_info[0] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff), (zoom_info[1] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff), zoom_info[2], zoom_info[3] };
        }
        public void ZoomOut(float[] cursor_coords)
        {
            zoom_coeff /= 0.99f;
            _vertices = new float[] { _vertices[0], _vertices[1], _vertices[2], Zoom_Normalize((_vertices[3] - cursor_coords[0] * zoom_coeff) / (1f - zoom_coeff)), Zoom_Normalize((_vertices[4] - cursor_coords[1] * zoom_coeff) / (1f - zoom_coeff)),
                                                 _vertices[5], _vertices[6], _vertices[7], Zoom_Normalize((_vertices[8] - cursor_coords[0] * zoom_coeff) / (1f - zoom_coeff)), Zoom_Normalize((_vertices[9] - cursor_coords[1] * zoom_coeff) / (1f - zoom_coeff)),
                                                 _vertices[10], _vertices[11], _vertices[12], Zoom_Normalize((_vertices[13] - cursor_coords[0] * zoom_coeff) / (1f - zoom_coeff)), Zoom_Normalize((_vertices[14] - cursor_coords[1] * zoom_coeff) / (1f - zoom_coeff)),
                                                 _vertices[15], _vertices[16], _vertices[17],Zoom_Normalize ((_vertices[18] - cursor_coords[0] * zoom_coeff) / (1f - zoom_coeff)), Zoom_Normalize((_vertices[19] - cursor_coords[1] * zoom_coeff) / (1f - zoom_coeff))};
            zoom_info = new float[] { (zoom_info[0] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff), (zoom_info[1] + cursor_coords[0] * zoom_coeff) / (1f + zoom_coeff), zoom_info[2], zoom_info[3] };
        }
        public void ZoomReset()
        {
            zoom_coeff = 0.2f;
            _vertices = new float[] { _vertices[0], _vertices[1], _vertices[2], 1f, 1f,
                                                 _vertices[5], _vertices[6], _vertices[7], 1f, 0f,
                                                 _vertices[10], _vertices[11], _vertices[12], 0f, 0f,
                                                 _vertices[15], _vertices[16], _vertices[17], 0f, 1f};
            zoom_info = new float[] { _vertices[0], _vertices[15], _vertices[1], _vertices[6] };
        }
        public void ZoomDrag(float[] cursor_coords1, float[] cursor_coords2)
        {
            float[] diff = new float[] { cursor_coords2[0] - cursor_coords1[0], cursor_coords2[1] - cursor_coords1[1] };
            if (IB10(_vertices[3] + diff[0]) && IB10(_vertices[8] + diff[0]) && IB10(_vertices[13] + diff[0]) && IB10(_vertices[18] + diff[0]))
                _vertices = new float[] { _vertices[0], _vertices[1], _vertices[2], _vertices[3] + diff[0], _vertices[4],
                                                 _vertices[5], _vertices[6], _vertices[7], _vertices[8] + diff[0], _vertices[9],
                                                 _vertices[10], _vertices[11], _vertices[12], _vertices[13] + diff[0], _vertices[14],
                                                 _vertices[15], _vertices[16], _vertices[17], _vertices[18] + diff[0], _vertices[19]};
            if (IB10(_vertices[4] + diff[0]) && IB10(_vertices[9] + diff[0]) && IB10(_vertices[14] + diff[0]) && IB10(_vertices[19] + diff[0]))
                _vertices = new float[] { _vertices[0], _vertices[1], _vertices[2], _vertices[3] , _vertices[4]+ diff[1],
                                                 _vertices[5], _vertices[6], _vertices[7], _vertices[8] , _vertices[9] + diff[1],
                                                 _vertices[10], _vertices[11], _vertices[12], _vertices[13], _vertices[14] + diff[1],
                                                 _vertices[15], _vertices[16], _vertices[17], _vertices[18], _vertices[19] + diff[1]};
        }
        private float Zoom_Normalize(float number)
        {
            if (this.zoom_limited)
            {
                return Math.Min(Math.Max(0f, number), 1f);
            }
            else
            {
                return number;
            }
        }

        private bool IB10(float n)
        {
            if (this.zoom_limited)
            {
                return (n <= 1.0f && n >= 0.0f);
            }
            return true;
        }
    }
}
