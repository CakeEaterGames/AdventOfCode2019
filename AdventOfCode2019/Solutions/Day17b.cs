using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day17b : Problem
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


        public override void Calc()
        {
            computer com = new computer();
            com.inputProgram = 2 + input.Remove(0, 1);

            com.Init();

            com.Calc();
            string scr = "";
            while (com.outputs.Count > 0)
            {
                scr += getAscii(com.outputs.Dequeue() + "");
            }
            Console.WriteLine(scr);




            var r = scr.Split('\n');
            int start = scr.IndexOf('^');
            int startY = start / r[0].Length;
            int startX = r[startY].IndexOf('^');

            int x = startX;
            int y = startY;

            int sx = 0;
            int sy = -1;
            int curDir = 0;
            turn(0);
            look();

            char rr(int gy, int gx)
            {
                if (gy >= 0 && gx >= 0)
                    if (r.Length > gy && r[gy].Length > gx)
                    {
                        return r[gy][gx];
                    }
                return '.';
            }

            void turn(int dir)
            {
                dir += 4;
                dir %= 4;
                switch (dir)
                {
                    case 0: sx = 0; sy = -1; break;
                    case 1: sx = 1; sy = 0; break;
                    case 2: sx = 0; sy = 1; break;
                    case 3: sx = -1; sy = 0; break;
                }
                curDir = dir;
            }
            int look()
            {
                bool l = false, rt = false, u = false, d = false;

                if (rr(y - 1, x) == '#') u = true;
                if (rr(y + 1, x) == '#') d = true;
                if (rr(y, x - 1) == '#') l = true;
                if (rr(y, x + 1) == '#') rt = true;

                //    Console.WriteLine(u + " " + d + " " + l + " " + rt);
                //   Console.WriteLine(curDir);

                int res = 0;
                switch (curDir)
                {
                    case 0: if (l) res = -1; if (rt) res = 1; break;
                    case 1: if (u) res = -1; if (d) res = 1; break;
                    case 2: if (l) res = 1; if (rt) res = -1; break;
                    case 3: if (u) res = 1; if (d) res = -1; break;
                }
                //   Console.WriteLine(res);


                return res;
            }


            List<string> paths = new List<string>();
            while (true)
            {
                int count = 0;
                while (rr(y + sy, x + sx) == '#')
                {
                    count++;
                    y += sy;
                    x += sx;
                }
                paths.Add(count + "");
                var l = look();

                turn(curDir + l);
                if (l == 1)
                {
                    paths.Add("R");
                }
                else if (l == -1)
                {
                    paths.Add("L");
                }
                else
                {
                    break;
                }
            }

            if (paths[0] == "0")
            {
                paths.RemoveAt(0);
            }

            Console.WriteLine(String.Join(",", paths));

            while (!com.done)
            {

                // Console.ReadLine();
                Console.WriteLine("waiting for input");
              
                var c = Console.ReadLine();
                foreach (char a in c)
                {
                    com.inputs.Enqueue((int)a);
                }
                com.inputs.Enqueue((int)'\n');
                com.Calc();
                while (com.outputs.Count > 0)
                {
                   Console.Write(getAscii(com.outputs.Dequeue() + ""));
                }
            }
            output = com.output + "" ;

        }
        char getAscii(string s)
        {
            return (char)int.Parse(s);
        }
    }
}
