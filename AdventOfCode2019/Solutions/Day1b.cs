using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day1b : Problem
    {

        public override void Calc()
        {
            int sum = 0;
            var a = input.Split('\n');
            foreach (var b in a)
            {
                var c = int.Parse(b);

                while ((c / 3 - 2)>0)
                {
                    c /= 3;
                    c -= 2;
                    sum += c;
                }

            }
            output = "" + sum;
        }
    }
}
