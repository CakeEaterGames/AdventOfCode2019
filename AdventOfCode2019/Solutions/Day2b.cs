using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day2b : Problem
    {
        public override void Calc()
        {
            for (int v = 0; v <= 99; v++)
            {
                for (int u = 0; u <= 99; u++)
                {
                    var a = Tools.SplitToIntArray(input, ',');

                    a[1] = v;
                    a[2] = u;

                    for (int i = 0; i < a.Length; i += 4)
                    {
                        int c1 = a[i];
                        int c2 = a[i + 1];
                        int c3 = a[i + 2];
                        int c4 = a[i + 3];
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
                                Console.WriteLine("wtf if " + c1);
                                done = true;
                                break;
                        }
                        if (done) break;

                    }
                    if (a[0] == 19690720)
                    {
                        output = "" + (v*100+u);
                    }

                    if (output!="") break;
                }
                if (output != "") break;
            }
        }

    }
}
