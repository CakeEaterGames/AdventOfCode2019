using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day8b : Problem
    {
        int w, h, d;
        public override void Calc()
        {
            w = 25;
            h = 6;
            d = input.Length / (w * h);
            int[][][] img = new int[d][][];

            string res = "";

            int minLayer = int.MaxValue;
            int min = int.MaxValue;
            int count12 = 0;

            var input2 = Tools.StringToIntArray(input);

            int pos = 0;

            for (int z = 0; z < d; z++)
            {
                img[z] = new int[h][];
                for (int y = 0; y < h; y++)
                {
                    img[z][y] = new int[w];
                    for (int x = 0; x < w; x++)
                    {
                        img[z][y][x] = input2[pos];
                        pos++;
                    }
                }
            }



            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    for (int z = 0; z < d; z++)
                    {
                        if (img[z][y][x] != 2)
                        {
                            if (img[z][y][x] == 0)
                            {
                                res += "_";
                            }
                            else
                            {
                                res += "8";
                            }
                         
                            break;
                        }
                        
                    }
                }
                res += "\n";
            }


            output = res;


        }
    }
}
