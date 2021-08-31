using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;
using System;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace FractalRenderer
{
    public class Window : GameWindow
    {
        float zoomer = 0.01f;
        int max_iter = 256;
        float idk_what_y = 0f;
        float idk_what_x = 0f;
        ZoomablePlane p;
        int u_frame = 0;
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        }
        protected override void OnLoad(EventArgs e)
        {

            GL.ClearColor(0.1f, 0.12f, 0.12f, 1.0f);
            p = new ZoomablePlane();
            p.plane = new Plane(Misc.fullscreenverticies, new Shader("shaders/shader.vert", "shaders/graphshader.frag"), Texture.LoadFromFile("content/ivan.png"));
            p.plane.ReshapeWithCoords(-1.0f, 1.0f * (Width / Height), 1.0f, -1.0f * (Width / Height));
            p.plane.Load();
            p.plane.zoom_limited = false;
            p.plane._shader.SetFloat("r_multiplier", zoomer);
            p.plane._shader.SetInt("maxIterations", max_iter);
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            u_frame++;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //renderer.Render();

            p.plane.Render();


            SwapBuffers();
            base.OnRenderFrame(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            MouseState mouse = Mouse.GetCursorState();
            KeyboardState kbrd = Keyboard.GetState();
            //Console.WriteLine("Update frame: " + Convert.ToString(u_frame));
            //renderer.camera_position.x += 0.03f;
            //renderer.ReassembleRenderables();
            //renderer.FindCellOnCursor(new fPos(-1 + 2 * (float)mouse.X / Width, -1 + 2 * (float)mouse.Y / Height));
            if (kbrd.IsKeyDown(Key.Plus))
            {
                if (kbrd.IsKeyDown(Key.Number1))
                    zoomer *= 1.01f;
                if (kbrd.IsKeyDown(Key.Number2))
                    max_iter++;
                if (kbrd.IsKeyDown(Key.Number3))
                    idk_what_x += 0.001f;
                if (kbrd.IsKeyDown(Key.Number4))
                    idk_what_y += 0.001f;
                try
                {
                    p.plane._shader.SetFloat("r_multiplier", zoomer);
                    p.plane._shader.SetInt("maxIterations", max_iter);
                    p.plane._shader.SetFloat("idk_x", idk_what_x);
                    p.plane._shader.SetFloat("idk_y", idk_what_y);
                }
                catch (Exception ex)
                {
                    
                }
            }
            if (kbrd.IsKeyDown(Key.Minus))
            {
                if (kbrd.IsKeyDown(Key.Number1))
                    zoomer /= 1.01f;
                if (kbrd.IsKeyDown(Key.Number2))
                    max_iter = Math.Max(max_iter, 1);
                if (kbrd.IsKeyDown(Key.Number3))
                    idk_what_x -= 0.001f;
                if (kbrd.IsKeyDown(Key.Number4))
                    idk_what_y -= 0.001f;
                try
                {
                    p.plane._shader.SetFloat("r_multiplier", zoomer);
                    p.plane._shader.SetInt("maxIterations", max_iter);
                    p.plane._shader.SetFloat("idk_x", idk_what_x);
                    p.plane._shader.SetFloat("idk_y", idk_what_y);
                }
                catch (Exception ex)
                {
                    
                }
            }
            p.PassScroll(this, mouse);
            base.OnUpdateFrame(e);

            if (kbrd.IsKeyDown(Key.F11))
            {
                if (this.WindowBorder != WindowBorder.Hidden)
                {
                    this.WindowBorder = WindowBorder.Hidden;
                    this.WindowState = WindowState.Fullscreen;
                }
                else
                {
                    this.WindowBorder = WindowBorder.Resizable;
                    this.WindowState = WindowState.Normal;
                }
            }
            if (kbrd.IsKeyDown(Key.Escape))
            {
                this.WindowBorder = WindowBorder.Resizable;
                this.WindowState = WindowState.Normal;
            }
            if (kbrd.IsKeyDown(Key.F12))
            {
                p.TakeScreenshot(new Size(Width, Height), "shot.png");
            }
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            p.plane.ReshapeWithCoords(-1.0f, 1.0f * ((float)Width / Height), 1.0f, -1.0f * ((float)Width / Height));
            base.OnResize(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }
    }
}