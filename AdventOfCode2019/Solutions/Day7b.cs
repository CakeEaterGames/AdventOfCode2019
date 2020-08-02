using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day7b : Problem
    {
        class computer
        {
            public string inputProgram;
            public int output;
            public int[] code;
            public Queue<int> inputs = new Queue<int>();

            public bool halt = false;

            public void Init()
            {
                Console.WriteLine("Reset computer");

                code = Tools.SplitToIntArray(inputProgram, ',');

                i = 0;
            }

            public int i;

            public void Calc()
            {


                while (true)
                {
                    var opcode = SplitOPcode(code[i]);
                   // Console.WriteLine(Tools.ArrayToString(opcode));
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
                            int a = 0;
                            if (inputs.Count == 0)
                            {
                                done = true;
                                Console.WriteLine("No input, pausing execution");
                                break;
                              
                                //a = int.Parse(Console.ReadLine());
                            }
                            else
                            {
                                a = inputs.Dequeue();
                            }

                          
                            code[code[i + 1]] = a;
                            i += 2;
                            break;
                        case 4:
                            Console.Write("Computer output: ");
                            output = val(i + 1, opcode[1]);
                            Console.WriteLine(output);

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
                            halt = true;
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


        public override void Calc()
        {
           // TryCombo("98765");
            rec("");
            output = output2 + "";
        }


        void rec(String order)
        {
            if (order.Length == 5)
            {
                TryCombo(order);
            }
            else
            {
                if (!order.Contains("5")) rec(order + "5");
                if (!order.Contains("6")) rec(order + "6");
                if (!order.Contains("7")) rec(order + "7");
                if (!order.Contains("8")) rec(order + "8");
                if (!order.Contains("9")) rec(order + "9");
            }
        }

        int output2 = 0;
        void TryCombo(string ord)
        {
            int res = 0;
            var order = Tools.StringToIntArray(ord);
            Console.WriteLine(Tools.ArrayToString(order));

            computer[] coms = new computer[5];

            for (int i = 0; i < 5; i++)
            {
                computer c = new computer();
                c.inputProgram = input;
                c.inputs.Enqueue(order[i]);
                c.Init();
                coms[i] = c;              
            }

            while (!coms[4].halt)
            {
                for (int i = 0; i < 5; i++)
                {
                    coms[i].inputs.Enqueue(res);
                    coms[i].Calc();
                    res = coms[i].output;
                }
            }

            output2 = Math.Max(output2, res);


        }
    }
}
