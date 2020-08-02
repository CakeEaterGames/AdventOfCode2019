using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day8a : Problem
    {
        int w, h, d;
        public override void Calc()
        {
            w = 25;
            h = 6;
            d = input.Length / (w * h);
            int[][][] img = new int[d][][];
            int minLayer = int.MaxValue;
            int min = int.MaxValue;
            int count12 = 0;

            var input2 = Tools.StringToIntArray(input);

            int pos = 0;

            for (int z = 0;z<d;z++)
            {
                img[z] = new int[h][];
                int countZ = 0;
                int count1 = 0;
                int count2 = 0;

                for (int y = 0; y < h; y++)
                {
                    img[z][y] = new int[w];
                    for (int x = 0; x < w; x++)
                    {
                        img[z][y][x] = input2[pos];
                        if (input2[pos] == 0)
                        {
                            countZ++;
                        }
                         if (input2[pos] == 1)
                        {
                            count1++;
                        }
                         if (input2[pos] == 2)
                        {
                            count2++;
                        }
                       

                            pos++;
                    }
                }

                if (countZ<min)
                {
                    min = countZ;
                    minLayer = z+1;
                    count12 = count1 * count2;
                }
            }

            output = "" + count12;
             

        }
    }
}
