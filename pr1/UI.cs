using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
namespace FractalRenderer
{
    public class Button
    {
        Plane plane;
        Texture texture;
        Shader shader;
        Func<bool> reaction;
        Action reaction2;
        Window base_w;
        bool pressed = false;

        public Button(string tex_path, Func<bool> func, Window base_wnd)
        {
            base_w = base_wnd;
            reaction = func;
            texture = Texture.LoadFromFile(tex_path);
            shader = new Shader("shaders/ui/button.vert", "shaders/ui/button.frag");
            plane = new Plane(Misc.fullscreenverticies, shader, texture);
        }

        public Button(string tex_path, Action func, Window base_wnd)
        {
            base_w = base_wnd;
            reaction2 = func;
            texture = Texture.LoadFromFile(tex_path);
            shader = new Shader("shaders/ui/button.vert", "shaders/ui/button.frag");
            plane = new Plane(Misc.fullscreenverticies, shader, texture);
        }

        public void ReshapeWithCoords(float top_x, float top_y, float bottom_x, float bottom_y)
        {
            plane.ReshapeWithCoords(top_x, top_y, bottom_x, bottom_y);
        }
        public void Load()
        {
            plane.Load();
        }
        public void OnPress()
        {
            if (reaction != null)
                reaction();
            if (reaction2 != null)
                reaction2();
        }
        public void Render()
        {
            plane.Render();
        }
        public void Unload()
        {
            plane.Unload();
        }

        public void Update(MouseState mouse, bool react)
        {
            float[] info = plane.GetRelativeCursorPosition(base_w, mouse.X, mouse.Y);
            if (info[0] <= 1.0 && info[0] >= 0.0 && info[1] <= 1.0 && info[1] >= 0.0)
            {
                shader.SetFloat("mouse_hover", 1.0f);
                if (mouse.IsButtonDown(MouseButton.Left) && pressed == false && react)
                {
                    OnPress();
                    pressed = true;
                }
            }
            else
            {
                shader.SetFloat("mouse_hover", 0.0f);
            }
            if (mouse.IsButtonUp(MouseButton.Left))
            {
                pressed = false;
            }
        }
    }
}
