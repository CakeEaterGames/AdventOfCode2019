using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day5a : Problem
    {

        int[] code;
        public override void Calc()
        {
            code = Tools.SplitToIntArray(input, ',');

            int i = 0;
            while (true)
            {
                var opcode = SplitOPcode(code[i]);
                //Console.WriteLine(Tools.ArrayToString(opcode));
                bool done = false;
                switch (opcode[0])
                {
                    case 1:
                        code[code[i + 3]] = val(code[i + 1], opcode[1]) + val(code[i + 2], opcode[2]);
                        i += 4;
                        break;
                    case 2:
                        code[code[i + 3]] = val(code[i + 1], opcode[1]) * val(code[i + 2], opcode[2]);
                        i += 4;
                        break;
                    case 3:
                        Console.Write("Waiting for input ");
                        var a = int.Parse(Console.ReadLine());
                        code[code[i+1]] = a;
                        i += 2;
                        break;
                    case 4:
                        Console.Write("Computer output: ");
                        Console.WriteLine(code[code[i + 1]]);
                        i += 2;
                        break;
                    case 99:
                        done = true;
                        break;
                    default:
                        Console.WriteLine("wtf if " + opcode[0]);
                        Console.ReadLine();
                        //done = true;
                        break;
                }


                if (done)
                {
                    break;
                }
            }

            output = "" + code[0];
        }

        int val(int param, int mode)
        {
            if (mode == 0)
            {
                return code[param];
            }
            else
            {
                return param;
            }
        }

        int[] SplitOPcode(int opc)
        {
            int[] res = new int[5];
            res[0] = opc % 100;
            opc /= 100;

            for (int i = 1; i < res.Length; i++) res[i] = 0;

            for (int i = 1;opc>0;i++)
            {
                res[i] = opc % 10;
                opc /= 10;
            }

            return res;
        }

    }
}
