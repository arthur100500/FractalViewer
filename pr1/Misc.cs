using System;
using System.Collections.Generic;


namespace FractalRenderer
{
    //class for useful junk
    public class Misc
    {
        public static void PrintArr(float[] rr)
        {
            foreach (float x  in rr)
            {
                Console.Write(x);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        static public float[] fullscreenverticies =
        {
             1f,  1f, 0.0f, 1.0f, 1.0f,
             1f, -1f, 0.0f, 1.0f, 0.0f,
            -1f, -1f, 0.0f, 0.0f, 0.0f,
            -1f,  1f, 0.0f, 0.0f, 1.0f
        };
        public static string str(object arg)
        {
            return Convert.ToString(arg);
        }
        
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void print(object message)
        {
            Console.WriteLine(message);
        }

        private static bool isdigit(char x)
        {
            if (x == '1' || x == '2' || x == '3' || x == '4' || x == '5' || x == '6' || x == '7' || x == '8' || x == '9' || x == '0')
            {
                return true;
            }
            return false;
        }
        public static void PrintShaderError(string shader, string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Shader compilation error!\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            string[] splitted = error.Split('\n');
            string buffer = "";
            List<int> lines = new List<int>();
            foreach (string s in splitted)
            {
                buffer = "";
                if (s.Length > 10)
                {
                    if (s[0] == 'E' && s[1] == 'R' && s[2] == 'R' && s[3] == 'O' && s[4] == 'R' && s[5] == ':')
                    {
                        for (int i = 9; i < 100; i++)
                        {
                            if (isdigit(s[i]))
                                buffer += s[i];
                            else
                            {
                                break;
                            }
                        }
                        if (buffer != "")
                            lines.Add(Convert.ToInt32(buffer) - 1);
                    }
                }
            }
            splitted = shader.Split('\n');
            for (int i = 0; i < splitted.Length; i++)
            {
                if (lines.Contains(i))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine(str(i + 1) + " " + splitted[i]);
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
