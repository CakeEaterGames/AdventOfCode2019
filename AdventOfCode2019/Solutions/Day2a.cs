using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day2a : Problem
    {
        public override void Calc()
        {
            var a = Tools.SplitToIntArray(input,',');
            /* before running the program, replace position 1 with the
             * value 12 and replace position 2 with the value 2. What
             * value is left at position 0 after the program halts?*/

            a[1] = 12;
            a[2] = 2;

            for (int i = 0; i<a.Length; i+=4)
            {
                int c1 = a[i];
                int c2 = a[i+1];
                int c3 = a[i+2];
                int c4 = a[i+3];
                bool done = false;
                switch (c1)
                {
                    case 1:
                        a[c4] = a[c2] + a[c3];
                        break;
                    case 2:
                        a[c4] = a[c2] * a[c3];
                        break;
                    case 99:
                        done = true;
                        break;
                    default:
                        Console.WriteLine("wtf if "+c1);
                        done = true;
                        break;
                }


                if (done)
                {
                    break;
                }
            }

            output = ""+a[0];

           
        }

    }
}
