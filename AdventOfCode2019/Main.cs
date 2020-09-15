using AdventOfCode2019.Solutions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class main
    {
        static void Main(string[] args)
        {
            Problem a = new Day18a();
            StreamReader sr = new StreamReader("input.txt");
            StreamWriter sw = new StreamWriter("output.txt");

            a.input = sr.ReadToEnd();

            a.Calc();

            Console.WriteLine(a.output);
            sw.WriteLine(a.output);


            sr.Close();
            sw.Close();
            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
