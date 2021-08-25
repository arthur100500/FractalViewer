using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalRenderer
{
    
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Window g = new Window(800, 600, "");
            g.Run();
        }
    }
}
