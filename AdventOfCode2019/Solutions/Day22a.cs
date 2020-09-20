﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day22a : Problem
    {


        public override void Calc()
        {
            var inp2 = input.Replace("deal into new stack", "r").Replace("deal with increment", "i").Replace("cut", "c").Replace("\r", "");

            var alg = inp2.Split('\n').ToList();
            // alg.Reverse();

            int pos = 3;
            int len = 10;

         //   for (int i = 0; i < len; i++)
            {
                pos = 2019;
                len = 10007;

                foreach (var item in alg)
                {
                    var inst = item.Split(' ');
                    // Console.WriteLine(inst[0]);
                    string op = inst[0];

                    switch (op)
                    {
                        case "r":
                            pos = len - pos-1;
                            break;
                        case "c":
                            {
                                var arg = int.Parse(inst[1]);
                                if (arg > 0)
                                {
                                    if (pos >= arg) pos -= arg;
                                    else pos = len - arg + pos;
                                }
                                else
                                {
                                    if (pos < len + arg)
                                        pos -= arg;
                                    else
                                        pos = Math.Abs(len + arg - pos);
                                }
                            }
                            break;
                        case "i":
                            {
                                var arg = int.Parse(inst[1]);
                                pos = (pos * arg) % len;
                            }
                            break;
                    }
                }

                output = pos + "";
            }

            //Console.WriteLine(String.Join(", ",a));
        }
    }
}
