using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day1a : Problem
    {

        /*
         to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.
             */
        public override void Calc()
        {
            int sum = 0;
            var a = input.Split('\n');
            foreach (var b in a)
            {
                var c = int.Parse(b);
                c /= 3;
                c -= 2;
                sum += c;
            }
            output = ""+sum;
        }
    }
}
