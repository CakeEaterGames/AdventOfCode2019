using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day22b : Problem
    {


        public override void Calc()
        {
            var inp2 = input.Replace("deal into new stack", "r").Replace("deal with increment", "i").Replace("cut", "c").Replace("\r", "");

            var alg = inp2.Split('\n').ToList();
            alg.Reverse();

            long pos = 1;
            long len = 10;

            List<long> visited = new List<long>();

            long old = pos;


            for (int i = 0; i < len; i++)

            //   for (long i = 0; i < 101741582076661; i++)
            {
                pos = i;
                len = 10;

                foreach (var item in alg)
                {
                    var inst = item.Split(' ');
                    //Console.WriteLine(inst[0]);
                    string op = inst[0];

                    switch (op)
                    {
                        case "r":
                            pos = len - pos - 1;
                            break;
                        case "c":
                            {
                                var arg = long.Parse(inst[1]);
                                if (arg > 0)
                                {
                                    if (pos >= arg)
                                        pos -= arg;
                                    else
                                        pos = pos + (len - arg);
                                }
                                else
                                {
                                    if (pos >= len + arg)
                                        pos = pos - (len + arg);
                                    else
                                        pos = pos - arg;
                                }
                            }
                            break;
                        case "i":
                            {
                                var arg = long.Parse(inst[1]);
                              
                            }
                            break;
                    }
                }




                Console.WriteLine(pos);


            }

            //Console.WriteLine(String.Join(", ",a));
        }
    }
}
