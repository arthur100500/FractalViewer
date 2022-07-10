using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace FractalRenderer
{
    public class Window : GameWindow
    {
        private int _maxIter = 256;
        private ZoomablePlane _plane;
        private float _zoom = 0.01f;

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
            _plane = new ZoomablePlane();
            _plane.plane = new Plane(Misc.fullscreenverticies, new Shader("shader/shader.vert", "shader/graphshader.frag"),
                new Texture(0));
            
            _plane.plane.ReshapeWithCoords(-1.0f, 1.0f * (Width / Height), 1.0f, -1.0f * (Width / Height));
            _plane.plane.Load();
            _plane.plane.zoom_limited = false;
            _plane.plane._shader.SetFloat("r_multiplier", _zoom);
            _plane.plane._shader.SetInt("maxIterations", _maxIter);
            
            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _plane.plane.Render();


            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var mouse = Mouse.GetCursorState();
            var kbrd = Keyboard.GetState();

            if (kbrd.IsKeyDown(Key.Plus))
            {
                if (kbrd.IsKeyDown(Key.Number1))
                    _zoom *= 1.01f;
                if (kbrd.IsKeyDown(Key.Number2))
                    _maxIter++;
                try
                {
                    _plane.plane._shader.SetFloat("r_multiplier", _zoom);
                    _plane.plane._shader.SetInt("maxIterations", _maxIter);
                }
                catch (Exception ex)
                {
                }
            }

            if (kbrd.IsKeyDown(Key.Minus))
            {
                if (kbrd.IsKeyDown(Key.Number1))
                    _zoom /= 1.01f;
                if (kbrd.IsKeyDown(Key.Number2))
                    _maxIter = Math.Max(_maxIter, 1);

                try
                {
                    _plane.plane._shader.SetFloat("r_multiplier", _zoom);
                    _plane.plane._shader.SetInt("maxIterations", _maxIter);

                }
                catch (Exception ex)
                {
                }
            }

            _plane.PassScroll(this, mouse);
            base.OnUpdateFrame(e);

            if (kbrd.IsKeyDown(Key.F11))
            {
                if (WindowBorder != WindowBorder.Hidden)
                {
                    WindowBorder = WindowBorder.Hidden;
                    WindowState = WindowState.Fullscreen;
                }
                else
                {
                    WindowBorder = WindowBorder.Resizable;
                    WindowState = WindowState.Normal;
                }
            }

            if (kbrd.IsKeyDown(Key.Escape))
            {
                WindowBorder = WindowBorder.Resizable;
                WindowState = WindowState.Normal;
            }

            if (kbrd.IsKeyDown(Key.F12)) _plane.TakeScreenshot(new Size(Width, Height), "shot.png");
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _plane.plane.ReshapeWithCoords(-1.0f, 1.0f * ((float)Width / Height), 1.0f, -1.0f * ((float)Width / Height));
            base.OnResize(e);
        }
    }
}