using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SDPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace FractalRenderer
{
    internal class ZoomablePlane
    {
        public Plane plane;
        private int prev;
        private float[] prev_drag_pos;

        public void PassScroll(Window wnd, MouseState mouse)
        {
            if (mouse.ScrollWheelValue > prev) plane.ZoomIn(plane.GetZoomRelativeCursorPosition(wnd, mouse.X, mouse.Y));
            if (mouse.ScrollWheelValue < prev)
                plane.ZoomOut(plane.GetZoomRelativeCursorPosition(wnd, mouse.X, mouse.Y));
            if (mouse.IsButtonDown(MouseButton.Left))
                plane.ZoomDrag(plane.GetZoomRelativeCursorPosition(wnd, mouse.X, mouse.Y), prev_drag_pos);
            prev_drag_pos = plane.GetZoomRelativeCursorPosition(wnd, mouse.X, mouse.Y);
            prev = mouse.ScrollWheelValue;
        }

        public void TakeScreenshot(Size resolution, string name)
        {
            GL.Flush();
            using (var bmp = new Bitmap(resolution.Width, resolution.Height, SDPixelFormat.Format32bppArgb))
            {
                var mem = bmp.LockBits(new Rectangle(0, 0, resolution.Width, resolution.Height),
                    ImageLockMode.WriteOnly, SDPixelFormat.Format32bppArgb);
                GL.PixelStore(PixelStoreParameter.PackRowLength, mem.Stride / 4);
                GL.ReadPixels(0, 0, resolution.Width, resolution.Height, PixelFormat.Bgra, PixelType.UnsignedByte,
                    mem.Scan0);
                bmp.UnlockBits(mem);
                bmp.Save("screenshots/" + name, ImageFormat.Png);
            }
        }
    }
}