using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day5b : Problem
    {

        int[] code;
        public override void Calc()
        {
           // while (true)
            {
                Console.WriteLine("Reset computer");

                code = Tools.SplitToIntArray(input, ',');

                int i = 0;
                while (true)
                {
                    var opcode = SplitOPcode(code[i]);
                    Console.WriteLine(Tools.ArrayToString(opcode));
                    bool done = false;
                    switch (opcode[0])
                    {
                        case 1:
                            code[code[i + 3]] = val(i + 1, opcode[1]) + val(i + 2, opcode[2]);
                            i += 4;
                            break;
                        case 2:
                            code[code[i + 3]] = val(i + 1, opcode[1]) * val(i + 2, opcode[2]);
                            i += 4;
                            break;
                        case 3:
                            Console.Write("Waiting for input ");
                            var a = int.Parse(Console.ReadLine());
                            code[code[i + 1]] = a;
                            i += 2;
                            break;
                        case 4:
                            Console.Write("Computer output: ");
                            Console.WriteLine(val(i+1,opcode[1]));
                            i += 2;
                            break;

                        case 5: //Jump if true (not equal to zero)

                            if (val(i + 1, opcode[1]) != 0)
                            {
                                i = val(i + 2, opcode[2]);
                            }
                            else
                            {
                                i += 3;
                            }

                            break;
                        case 6: //Jump if false

                            if (val(i + 1, opcode[1]) == 0)
                            {
                                i = val(i + 2, opcode[2]);
                            }
                            else
                            {
                                i += 3;
                            }
                            break;
                        case 7: //less then

                            if (val(i + 1, opcode[1]) < val(i + 2, opcode[2]))
                            {
                                code[code[i + 3]] = 1;
                            }
                            else
                            {
                                code[code[i + 3]] = 0;
                            }
                            i += 4;
                            break;
                        case 8: //equals

                            if (val(i + 1, opcode[1]) == val(i + 2, opcode[2]))
                            {
                                code[code[i + 3]] = 1;
                            }
                            else
                            {
                                code[code[i + 3]] = 0;
                            }
                            i += 4;
                            break;

                        case 99:
                            Console.WriteLine("Halt computer");
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

            }
        }

        String debugComputer(int index)
        {
            string res = "";


            return res;
        }


        int val(int param, int mode)
        {
            if (mode == 0)
            {
                return code[code[param]];
            }
            else
            {
                return code[param];
            }
        }

        int[] SplitOPcode(int opc)
        {
            int[] res = new int[5];
            res[0] = opc % 100;
            opc /= 100;

            for (int i = 1; i < res.Length; i++) res[i] = 0;

            for (int i = 1; opc > 0; i++)
            {
                res[i] = opc % 10;
                opc /= 10;
            }

            return res;
        }

    }
}
