using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day13b : Problem
    {

        class computer
        {
            public string inputProgram;
            public long output;
            public Queue<long> outputs = new Queue<long>();
            public List<long> code;
            public long offset = 0;
            public Queue<long> inputs = new Queue<long>();

            public bool pause = false;

            public void Init()
            {
                Log("Reset computer");

                code = new List<long>(Tools.SplitToLongArray(inputProgram, ','));

                i = 0;
            }


            public int i;
            public bool done = false;
            public void Calc()
            {
                while (true)
                {
                    var opcode = SplitOPcode((int)code[i]);
                    var Params = DebugCom(opcode[0], i);
                    // Log(Tools.ArrayToString(opcode)+" : "+ Params);


                    done = false;
                    pause = false;
                    switch (opcode[0])
                    {
                        case 1:
                            SetCode(GetCode(i + 3), val(i + 1, opcode[1]) + val(i + 2, opcode[2]), opcode[3]);
                            // code[code[i + 3]] = val(i + 1, opcode[1]) + val(i + 2, opcode[2]);
                            i += 4;
                            break;
                        case 2:
                            SetCode(GetCode(i + 3), val(i + 1, opcode[1]) * val(i + 2, opcode[2]), opcode[3]);
                            i += 4;
                            break;
                        case 3:
                            Log("input ");
                            long a = 0;
                            if (inputs.Count == 0)
                            {
                                // done = true;
                                Log("No input, Pausing");
                                pause = true;
                                //a = long.Parse(Console.ReadLine());
                                //inputs.Enqueue(a);
                                break;

                            }
                            else
                            {
                                Log("Pulling input fron queue");
                                a = inputs.Dequeue();
                            }

                            SetCode(GetCode(i + 1), a, opcode[1]);
                            //code[code[i + 1]] = a;
                            i += 2;
                            break;
                        case 4:
                            Log("Computer output: ");
                            output = val(i + 1, opcode[1]);
                            Log(output);
                            outputs.Enqueue(output);
                            i += 2;
                            break;

                        case 5: //Jump if true (not equal to zero)

                            if (val(i + 1, opcode[1]) != 0)
                            {
                                i = (int)val(i + 2, opcode[2]);
                            }
                            else
                            {
                                i += 3;
                            }

                            break;
                        case 6: //Jump if false

                            if (val(i + 1, opcode[1]) == 0)
                            {
                                i = (int)val(i + 2, opcode[2]);
                            }
                            else
                            {
                                i += 3;
                            }
                            break;
                        case 7: //less then

                            if (val(i + 1, opcode[1]) < val(i + 2, opcode[2]))
                            {
                                SetCode(GetCode(i + 3), 1, opcode[3]);
                                //code[code[i + 3]] = 1;
                            }
                            else
                            {
                                SetCode(GetCode(i + 3), 0, opcode[3]);
                                //code[code[i + 3]] = 0;
                            }
                            i += 4;
                            break;
                        case 8: //equals

                            if (val(i + 1, opcode[1]) == val(i + 2, opcode[2]))
                            {
                                SetCode(GetCode(i + 3), 1, opcode[3]);
                                //code[code[i + 3]] = 1;
                            }
                            else
                            {
                                SetCode(GetCode(i + 3), 0, opcode[3]);
                                //code[code[i + 3]] = 0;
                            }
                            i += 4;
                            break;
                        case 9:
                            offset += val(i + 1, opcode[1]);
                            i += 2;
                            break;

                        case 99:
                            Log("99 Done!");
                            done = true;
                            break;
                        default:
                            Log("wtf if " + opcode[0]);
                            Console.ReadLine();
                            //done = true;
                            break;
                    }


                    if (done)
                    {
                        break;
                    }

                    if (pause)
                    {
                        break;
                    }
                }


            }

            long GetCode(long index)
            {
                while (code.Count <= index)
                {
                    code.Add(0);
                }
                return code[(int)index];
            }
            long SetCode(long index, long value, int mode)
            {
                if (mode == 2)
                {
                    index += offset;
                }

                while (code.Count <= index)
                {
                    code.Add(0);
                }

                return code[(int)index] = value;
            }

            long val(long param, long mode)
            {
                if (mode == 0)
                {
                    return GetCode(GetCode(param));

                }
                else if (mode == 1)
                {
                    return GetCode(param);
                }
                else if (mode == 2)
                {
                    return GetCode(GetCode(param) + offset);
                }
                return 0;
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


            String DebugCom(long opcode, int index)
            {
                string res = "";
                int amount = 0;
                switch (opcode)
                {
                    case 99:
                        amount = 1;
                        break;
                    case 3:
                    case 4:
                    case 9:
                        amount = 2;
                        break;
                    case 5:
                    case 6:
                        amount = 3;
                        break;
                    case 1:
                    case 2:
                    case 7:
                    case 8:
                        amount = 4;
                        break;
                }


                res += OpcodeToString((int)opcode) + " ";


                for (int i = 0; i < amount; i++)
                {
                    res += GetCode(index + i) + ", ";
                }

                return res;
            }
            string OpcodeToString(int code)
            {
                switch (code)
                {
                    case 1: return "ADD";
                    case 2: return "MUL";
                    case 3: return "IN";
                    case 4: return "OUT";
                    case 5: return "JIT";
                    case 6: return "JIF";
                    case 7: return "LZ<";
                    case 8: return "EQ=";
                    case 9: return "OFS";
                    case 99: return "HALT";

                }
                return "";
            }

            public bool toLog = false;
            void Log<T>(T s)
            {
                if (toLog)
                {
                    Console.WriteLine(s);
                }
            }

        }



        long score = 0;
        char[][] screen;

        public override void Calc()
        {
            int w = 50;
            int h = 25;

            screen = new char[h][];

            for (int i = 0; i < h; i++)
            {
                screen[i] = new char[w];
                for (int j = 0; j < w; j++)
                {
                    screen[i][j] = ' ';
                }
            }

            computer com = new computer();
            com.inputProgram = input;

            com.Init();
            //   com.toLog = true;
            com.code[0] = 2;
            int step = 0;
            while (true)
            {



                com.Calc();

                int ballx = 0;
                int padx = 0;

                int countB = 10;
                int wait = 10;
                while (com.outputs.Count > 0)
                {
                    var x = (int)com.outputs.Dequeue();
                    var y = (int)com.outputs.Dequeue();
                    var id = (int)com.outputs.Dequeue();

                    if (x < 0)
                    {
                        score = id;
                    }
                    else
                    {
                        /*
                        0 is an empty tile. No game object appears in this tile.
                        1 is a wall tile. Walls are indestructible barriers.
                        2 is a block tile. Blocks can be broken by the ball.
                        3 is a horizontal paddle tile. The paddle is indestructible.
                        4 is a ball tile. The ball moves diagonally and bounces off objects.
                        */
                        switch (id)
                        {
                            case 0: screen[y][x] = ' '; break;
                            case 1: screen[y][x] = '+'; break;
                            case 2: screen[y][x] = '#'; countB++; break;
                            case 3: screen[y][x] = '='; padx = x; break;
                            case 4: screen[y][x] = 'o'; ballx = x; break;
                        }
                    }

                }
                bool found = false;
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if (screen[i][j] == '#')
                        {
                            found = true;
                            break;
                        }
                        if (found) break;
                    }
                }
                if (!found) break;

                step++;
                if (step % 100 == 0)
                {
                    /*


                                    Console.Clear();

                                    for (int i = 0; i < h; i++)
                                    {
                                        for (int j = 0; j < w; j++)
                                        {
                                            Console.Write(screen[i][j]);
                                        }
                                        Console.WriteLine();
                                    }
                                     Console.WriteLine(score);
                                    */
                }
        

                //var inp = int.Parse(Console.ReadLine());
                //com.inputs.Enqueue(inp);
                //Console.ReadLine();


                if (padx > ballx)
                {
                    com.inputs.Enqueue(-1);
                }
                else if (padx < ballx)
                {
                    com.inputs.Enqueue(1);
                }
                else
                {
                    com.inputs.Enqueue(0);
                }

            }


            output = score + "";


        }
    }
}
