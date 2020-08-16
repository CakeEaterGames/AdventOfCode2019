using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day11b : Problem
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
                Console.WriteLine("Reset computer");

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
                    // Console.WriteLine(Tools.ArrayToString(opcode)+" : "+ Params);


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
                            Console.Write("input ");
                            long a = 0;
                            if (inputs.Count == 0)
                            {
                                // done = true;
                                Console.WriteLine("No input, Pausing");
                                pause = true;
                                //a = long.Parse(Console.ReadLine());
                                //inputs.Enqueue(a);
                                break;

                            }
                            else
                            {
                                Console.WriteLine("Pulling input fron queue");
                                a = inputs.Dequeue();
                            }

                            SetCode(GetCode(i + 1), a, opcode[1]);
                            //code[code[i + 1]] = a;
                            i += 2;
                            break;
                        case 4:
                            Console.Write("Computer output: ");
                            output = val(i + 1, opcode[1]);
                            Console.WriteLine(output);
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
                            Console.WriteLine("99 Done!");
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
        }

        enum direction
        {
            up=0,right=1,down=2,left=3
        }
        direction currentDir = direction.up;
        List<PointS> whites = new List<PointS>();
        List<PointS> countColor = new List<PointS>();
        Point pos = new Point(0, 0);

        class Point
        {
            public int X;
            public int Y;
            public Point(int a, int b)
            {
                X = a;
                Y = b;
            }
            public override string ToString()
            {
                return "(" + X + ", " + Y +") ";
            }
        }

        struct PointS
        {
            public int X;
            public int Y;
            public PointS(int a, int b)
            {
                X = a;
                Y = b;
            }
            public override string ToString()
            {
                return "(" + X + ", " + Y + ") ";
            }
        }

        void move(direction d)
        {
            switch (d)
            {
                case direction.down: pos.Y--; break;
                case direction.up: pos.Y++; break;
                case direction.left: pos.X--; break;
                case direction.right: pos.X++; break;
            }
        }

        direction TurnFrom(direction curr, direction where)
        {
            int t = 0;
            if (where == direction.left)
            {
                t = -1;
            }
            else if (where == direction.right)
            {
                t = 1;
            }

            curr += t;
            curr += 4;
            curr = (direction)((int)curr % 4);
            return curr;
        }



        public override void Calc()
        {
            whites.Add(new PointS(0, 0));
            computer com = new computer();
            com.inputProgram = input;
 
            com.Init();

            while (!com.done)
            {
                if (whites.Contains(new PointS(pos.X,pos.Y)))
                {
                    com.inputs.Enqueue(1);
                }
                else
                {
                    com.inputs.Enqueue(0);
                }
                com.Calc();

                if (com.outputs.Dequeue() == 1)
                {
                    if (!whites.Contains(new PointS(pos.X, pos.Y)))
                    {
                        whites.Add(new PointS(pos.X, pos.Y));
                    }
                  
                }
                else
                {
                    whites.Remove(new PointS(pos.X, pos.Y));
                }

                if (!countColor.Contains(new PointS(pos.X, pos.Y)))
                {
                    countColor.Add((new PointS(pos.X, pos.Y)));
                }


                if (com.outputs.Dequeue() == 1)
                {
                    currentDir = TurnFrom(currentDir, direction.right);
                }
                else
                {
                    currentDir = TurnFrom(currentDir, direction.left);
                }

                move(currentDir);


            }
      


            Point min = new Point(9999, 9999);
            Point max = new Point(-9999, -9999);
            foreach (var a in whites)
            {
                min.X = Math.Min(min.X, a.X);
                max.X = Math.Max(max.X, a.X);

                min.Y = Math.Min(min.Y, a.Y);
                max.Y = Math.Max(max.Y, a.Y);
            }

            int offx = min.X;
            int offy = min.Y;
            char[][] code = new char[max.Y - min.Y+1][];
            for (int i =0;i<code.Length;i++)
            {
                code[i] = new char[max.X - min.X+1];
                for (int j = 0; j < code[i].Length; j++)
                {
                    code[i][j] = '.';
                }
            }

            foreach (var a in whites)
            {
                code[max.Y-a.Y][a.X-min.X] = '#';
            }
            
            for (int i = 0; i < code.Length; i++)
            {
                for (int j = 0; j < code[i].Length; j++)
                {
                    output += code[i][j];
                }
                output += '\n';
            }


        }
    }
}
