using System;

namespace FractalRenderer
{
    public class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var window = new Window(800, 600, "Mandelbrot's set");
            window.Run();
        }
    }
}