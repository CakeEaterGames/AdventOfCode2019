using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day25a : Problem
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
                done = false;
                pause = false;
                offset = 0;
                outputs = new Queue<long>();
                inputs = new Queue<long>();
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


        computer com;
        Dictionary<string, string> visited = new Dictionary<string, string>();
        Stack<string> toExplore = new Stack<string>();
        public override void Calc()
        {

            com = new computer();
            com.inputProgram = input;
            com.Init();
            com.Calc();

            var res = "";
            foreach (var a in com.outputs)
            {
                res += ((char)a);
            }
            visited.Add("", res);

            toExplore.Push("");

            while (toExplore.Count > 0)
            {
                var p = toExplore.Pop();

                if (p != "")
                {
                    var expRes = explore(p);
                  //  Console.WriteLine("Path: {0}", p);
                    //Console.WriteLine("log: {0}", expRes);
                  //  Console.ReadLine();
                    visited.Add(p, expRes);
                }

                var l = checkDirs(p, visited[p]);

                foreach (var entry in l)
                {
                    toExplore.Push(entry);
                }

            }
            com.Calc();
            var cout = "";
            foreach (var a in com.outputs)
            {
                cout += ((char)a);
            }

            travelTo(pathToGoal); ;

            QueueCom("inv");
            com.Calc();

            var itemString = getRes();

            List<string> itemsList = getItems(itemString);

            var comboList = GenerateCombinations(itemsList);


            foreach (var combo in comboList)
            {
                OrganiseInv(combo);
                QueueCom(fullAct(dirToGoal));
                com.Calc();
                cout = getRes();
                if (!cout.Contains("Alert"))
                {
                    var st = cout.IndexOf("typing")+"typing".Length+1;
                    var ed = cout.IndexOf(" ", st);
                    output = cout.Substring(st,ed-st);
                    break;
                }               
            }
 
        }

        void OrganiseInv(List<string> items)
        {
            QueueCom("inv");
            com.outputs.Clear();
            com.Calc();
            var itemString = getRes();
            var eq = getItems(itemString);

            string cmd = "";

            foreach (var item in items)
            {
                if (!eq.Contains(item))
                {
                    cmd += "take "+item;
                    cmd += (char)(10);
                }
            }
            foreach (var item in eq)
            {
                if (!items.Contains(item))
                {
                    cmd += "drop " + item;
                    cmd += (char)(10);
                }
            }
            QueueCom(cmd);
            com.Calc();
            com.outputs.Clear();

        }


        List<string> getItems(string itemString)
        {
            if (itemString.Contains("You aren't carrying any items"))
            {
                return new List<string>();
            }
            var str = "-";
            var st = itemString.IndexOf(str);
            var itemsOnly = itemString.Substring(st).Replace("- ", "");
            var res = new List<string>(itemsOnly.Split('\n'));
            res.RemoveRange(res.Count - 3, 3);
            return res;
        }

        List<List<string>> GenerateCombinations(List<string> items)
        {
            var res = new List<List<string>>();

            int l = (int)Math.Pow(2, items.Count);
            for (int i = 0; i < l; i++)
            {
                var bin = i;
                var count = 0;
                List<string> combo = new List<string>();
                while (bin>0)
                {
                    if (bin%2==1)
                    {
                        combo.Add(items[count]);
                    }
                    bin /= 2;
                    count++;
                }
                res.Add(combo);
               /* foreach (var a in combo)
                {
                    Console.WriteLine(a);
                }*/

                // Console.WriteLine("_______________");
                //Console.WriteLine(Convert.ToString(i, 2));
            }



            return res;
        }


        void QueueCom(string act)
        {
            foreach (var c in act)
            {
                com.inputs.Enqueue(c);
            }
            com.inputs.Enqueue(10);
        }

        string getRes(bool toClear = true)
        {
            var cout = "";
            foreach (var a in com.outputs)
            {
                cout += ((char)a);
            }
            if(toClear)
            com.outputs.Clear();
            return cout;
        }

        String pathToGoal = "";
        char dirToGoal = '0';
        List<string> checkDirs(string path, string log)
        {
            var res = new List<string>();

            string last = "";
            if (path.Length >= 1)
            {
                last = AntiAct(fullAct(path[path.Length - 1]));
            }

            if (log.Contains("verify your identity"))
            {
                pathToGoal = path;
                bool N2 = log.Contains("north") && last != "north";
                bool E2 = log.Contains("east") && last != "east";
                bool W2 = log.Contains("west") && last != "west";
                bool S2 = log.Contains("south") && last != "south";

                if (N2)
                {
                    dirToGoal = 'n';
                }
                if (E2)
                {
                    dirToGoal = 'e';
                }
                if (W2)
                {
                    dirToGoal = 'w';
                }
                if (S2)
                {
                    dirToGoal = 's';
                }
                pathToGoal += dirToGoal;

                return res;
            }


            bool N = log.Contains("north") && last != "north";
            bool E = log.Contains("east") && last != "east";
            bool W = log.Contains("west") && last != "west";
            bool S = log.Contains("south") && last != "south";

            if (N && !visited.ContainsKey(path + "n"))
            {
                res.Add(path + "n");
            }
            if (E && !visited.ContainsKey(path + "e"))
            {
                res.Add(path + "e");
            }
            if (W && !visited.ContainsKey(path + "w"))
            {
                res.Add(path + "w");
            }
            if (S && !visited.ContainsKey(path + "s"))
            {
                res.Add(path + "s");
            }

            return res;
        }

        List<String> BannedObjects = new List<string>(new string[] { "molten lava", "escape pod", "photons","giant electromagnet","infinite loop"});


        string travelTo(string pathTo)
        {
            foreach (var c in pathTo)
            {
                com.outputs.Clear();
                var act = fullAct(c);
                foreach (var c2 in act)
                {
                    com.inputs.Enqueue(c2);
                }
                com.inputs.Enqueue(10);
                com.Calc();
            }

            var res = "";
            foreach (var a in com.outputs)
            {
                res += ((char)a);
            }
            com.outputs.Clear();
            return res;
        }

        string explore(string pathTo)
        {
            foreach (var c in pathTo)
            {
                com.outputs.Clear();
                var act = fullAct(c);
                foreach (var c2 in act)
                {
                    com.inputs.Enqueue(c2);
                }
                com.inputs.Enqueue(10);
                com.Calc();
            }

            var res = "";
            foreach (var a in com.outputs)
            {
                res += ((char)a);
            }

            string items = "Items here:";
            if (res.Contains(items))
            {
                
                int skip = items.Length+3;
                int st = res.IndexOf(items) + skip;
                int ed = res.IndexOf("\n", st);
                string item = res.Substring(st, ed - st);

                if (!BannedObjects.Contains(item))
                {
                    string act = "take " + item;
                    //Console.WriteLine("Found ITEMS!!!!!!!!!!!!!!!!! '{0}'",res.Substring(st,ed-st));
                    foreach (var c2 in act)
                    {
                        com.inputs.Enqueue(c2);
                    }
                    com.inputs.Enqueue(10);
                    com.Calc();

                
                }
            }


            char[] charArray = pathTo.ToCharArray();
            Array.Reverse(charArray);
            pathTo = new string(charArray);


           

            foreach (var c in pathTo)
            {
             
                var act = AntiAct(fullAct(c));
                foreach (var c2 in act)
                {
                    com.inputs.Enqueue(c2);
                }
                com.inputs.Enqueue(10);
                com.Calc();
                com.outputs.Clear();
            }

            return res;
        }

        string fullAct(char act)
        {
            switch (act)
            {
                case 'n': return "north";
                case 's': return "south";
                case 'e': return "east";
                case 'w': return "west";
            }
            return act + "";
        }

        string AntiAct(string act)
        {
            switch (act)
            {
                case "north": return "south";
                case "south": return "north";
                case "east": return "west";
                case "west": return "east";

                case "n": return "south";
                case "s": return "north";
                case "e": return "west";
                case "w": return "east";
            }
            return act;
        }

        char getAscii(string s)
        {
            return (char)int.Parse(s);
        }
        char getChar(long s)
        {
            return (char)(int)(s);
        }
    }
}

/*
Console.WriteLine(cout.Replace("south", "up").Replace("north", "down").Replace("west", "left").Replace("east", "right"));
var act = Console.ReadLine();


switch (act)
{
    case "north": y++; break;
    case "south": y--; break;
    case "east": x++; break;
    case "west": x--; break;

    case "up": act = "south"; y--; break;
    case "down": act = "north"; y++; break;
    case "left": act = "west"; x--; break;
    case "right": act = "east"; x++; break;
}
map[ox + x, oy + y] = '0';
foreach (var c in act)
{
    com.inputs.Enqueue(c);
}
com.inputs.Enqueue(10);
for (int i = 0; i < h; i++)
{
    for (int j = 0; j < w; j++)
    {
        if (j==ox+x && i==oy+y)
        {
            Console.Write("@");
        }
        else
        Console.Write(map[j,i]);
    }
    Console.WriteLine();
}
*/
