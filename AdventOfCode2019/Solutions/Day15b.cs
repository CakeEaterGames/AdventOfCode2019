using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day15b : Problem
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

        struct point
        {
            public int x;
            public int y;
            public point(int a, int b)
            {
                x = a;
                y = b;
            }
        }

        point pos = new point(0, 0);

        List<point> walls = new List<point>();
        List<point> spaces = new List<point>();
        List<point> special = new List<point>();
        List<point> oxygen = new List<point>();

        char[][] screen;
        int w = 75;
        int h = 50;

        Stack<int> moves = new Stack<int>();

        public override void Calc()
        {

            screen = new char[h][];

            for (int i = 0; i < h; i++)
            {
                screen[i] = new char[w];
                for (int j = 0; j < w; j++)
                {
                    screen[i][j] = ' ';
                }
            }
            spaces.Add(new point(0, 0));
            //  special.Add(new point(0, 0));



            computer com = new computer();
            com.inputProgram = input;


            // com.toLog = true;
            com.Init();

            Random rng = new Random();
            while (true)
            {
                com.Calc();

                var dirn = 0;
                //   Console.ReadLine();
                var ch = findDir();
                dirn = ch;

                bool isBack = false;

                if (dirn >= 1 && dirn <= 4)
                {
                    com.inputs.Enqueue(dirn);
                    com.Calc();
                    isBack = false;
                }
                else
                {
                    if (moves.Count == 0)
                    {
                        break;
                    }
                    dirn = anti(moves.Pop());
                    com.inputs.Enqueue(dirn);
                    com.Calc();
                    isBack = true;
                }

                if (com.outputs.Count > 0)
                {


                    var o = com.outputs.Dequeue();

                    if (o == 0)//wall
                    {
                        switch (dirn)
                        {
                            case 1: walls.Add(new point(pos.x, pos.y + 1)); break;
                            case 2: walls.Add(new point(pos.x, pos.y - 1)); break;
                            case 3: walls.Add(new point(pos.x - 1, pos.y)); break;
                            case 4: walls.Add(new point(pos.x + 1, pos.y)); break;
                        }
                    }
                    else if (o == 1) //empty
                    {
                        switch (dirn)
                        {
                            case 1: spaces.Add(new point(pos.x, pos.y + 1)); pos.y++; break;
                            case 2: spaces.Add(new point(pos.x, pos.y - 1)); pos.y--; break;
                            case 3: spaces.Add(new point(pos.x - 1, pos.y)); pos.x--; break;
                            case 4: spaces.Add(new point(pos.x + 1, pos.y)); pos.x++; break;
                        }
                        if (!isBack)
                        {
                            moves.Push(dirn);
                        }

                    }
                    else if (o == 2)//found
                    {
                        switch (dirn)
                        {
                            case 1: special.Add(new point(pos.x, pos.y + 1)); pos.y++; break;
                            case 2: special.Add(new point(pos.x, pos.y - 1)); pos.y--; break;
                            case 3: special.Add(new point(pos.x - 1, pos.y)); pos.x--; break;
                            case 4: special.Add(new point(pos.x + 1, pos.y)); pos.x++; break;
                        }
                        if (!isBack)
                        {
                            moves.Push(dirn);
                        }



                    }


                }

                /*  clear();
                  setScreen();
                  Console.Clear();
                  writeScreen();*/
                //Thread.Sleep(50);
            }
           //  clear();
            setScreen();
           // Console.Clear();
          //  writeScreen();
          
            oxygen.Add(special[0]);
            while (special.Contains(oxygen[0]))
            {
                special.Remove(special[0]);
            }
            int ox = w / 2 - pos.x;
            int oy = h / 2 - pos.y;

            int timer = 0;

            while (true)
            {
                List<point> more = new List<point>();
                foreach (var o in oxygen)
                {
                    if (screen[o.y + 1 + oy][o.x + ox] == '.')
                    {
                        more.Add(new point(o.x, o.y + 1));
                    }
                    if (screen[o.y - 1 + oy][o.x + ox] == '.')
                    {
                        more.Add(new point(o.x, o.y - 1));
                    }
                    if (screen[o.y + oy][o.x + 1 + ox] == '.')
                    {
                        more.Add(new point(o.x + 1, o.y));
                    }
                    if (screen[o.y + oy][o.x - 1 + ox] == '.')
                    {
                        more.Add(new point(o.x - 1, o.y));
                    }
                }

                if (more.Count > 0)
                {
                    foreach (var a in more)
                    {
                        oxygen.Add(a);
                    }
                   // clear();
                    setScreen();
                    //Console.Clear();
                    //writeScreen();
                    timer++;
                }
                else
                {
                    output = timer+"";
                    break;
                }

            }
          /*  clear();
            setScreen();
            Console.Clear();
            writeScreen();
            */

        }

        int anti(int a)
        {
            switch (a)
            {
                case 1: return 2;
                case 2: return 1;
                case 3: return 4;
                case 4: return 3;
            }
            return a;
        }

        int findDir()
        {
            if (!isVisible(new point(pos.x, pos.y + 1))) return 1;
            else if (!isVisible(new point(pos.x, pos.y - 1))) return 2;
            else if (!isVisible(new point(pos.x - 1, pos.y))) return 3;
            else if (!isVisible(new point(pos.x + 1, pos.y))) return 4;
            else return 0;
        }

        bool isVisible(point a)
        {
            if (walls.Contains(a) || spaces.Contains(a) || special.Contains(a))
            {
                return true;
            }
            return false;
        }

        void clear()
        {
            for (int i = 0; i < screen.Length; i++)
            {
                for (int j = 0; j < screen[i].Length; j++)
                {
                    screen[i][j] = ' ';
                }
            }
        }




        void setScreen()
        {
            int offx = w / 2 - pos.x;
            int offy = h / 2 - pos.y;


            foreach (var a in walls)
            {
                var b = new point(a.x + offx, a.y + offy);
                if (b.x >= 0 && b.y >= 0 && b.y < screen.Length && b.x < screen[0].Length)
                {
                    screen[b.y][b.x] = '#';
                }
            }


            foreach (var a in spaces)
            {
                var b = new point(a.x + offx, a.y + offy);
                if (b.x >= 0 && b.y >= 0 && b.y < screen.Length && b.x < screen[0].Length)
                {
                    screen[b.y][b.x] = '.';
                }
            }

            foreach (var a in special)
            {
                var b = new point(a.x + offx, a.y + offy);
                if (b.x >= 0 && b.y >= 0 && b.y < screen.Length && b.x < screen[0].Length)
                {
                    screen[b.y][b.x] = '%';
                }
            }

            foreach (var a in oxygen)
            {
                var b = new point(a.x + offx, a.y + offy);
                if (b.x >= 0 && b.y >= 0 && b.y < screen.Length && b.x < screen[0].Length)
                {
                    screen[b.y][b.x] = 'o';
                }
            }


            screen[h / 2][w / 2] = '@';

        }

        void writeScreen()
        {
            String res = "";
            for (int i = h - 1; i >= 0; i--)
            {
                for (int j = 0; j < w; j++)
                {
                    res += screen[i][j];
                }
                res += '\n';
            }

            Console.WriteLine(res);

        }


    }
}
