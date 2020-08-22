using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day16a : Problem
    {

        public override void Calc()
        {

            var inp = Tools.StringToIntArray(input);

            int sum = 0;
            string line = "";
            for (int k = 0; k < 100; k++)
            {
                line = "";
                for (int j = 1; j < inp.Length + 1; j++)
                {
                    sum = 0;
                    for (int i = 0; i < inp.Length; i++)
                    {
                       // Console.WriteLine(inp[i] + " " + mul(j, i));
                        sum += inp[i] * mul(j, i);

                    }
                    line += Math.Abs(sum) % 10;
                }
               // Console.WriteLine(line);
                inp = Tools.StringToIntArray(line);
            }

           output=(line.Substring(0,8));
        }

        int mul(int position, int shift)
        {
            switch (((shift + 1) / position) % (4))
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 0;
                case 3: return -1;
            }
            return 0;
        }

    }
}
