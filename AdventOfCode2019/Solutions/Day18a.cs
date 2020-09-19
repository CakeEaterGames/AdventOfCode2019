using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day18a : Problem
    {


        struct point
        {
            public int x;
            public int y;
            public point(int a, int b)
            {
                x = a;
                y = b;
            }
            public override string ToString()
            {
                return "(" + x + ", " + y + ") ";
            }
        }


        class scaner
        {
            public class node
            {
                public char name;
                public Dictionary<char, int> links = new Dictionary<char, int>();
                public Dictionary<char, string> locks = new Dictionary<char, string>();
                public Dictionary<char, int> locks3 = new Dictionary<char, int>();

                //  public static int minimumLength = int.MaxValue;
                public static int minimumLength = int.MaxValue;
                public static int calls = 0;



                public void genBinMask()
                {
                    foreach (var item in locks)
                    {
                        locks3.Add(item.Key, 0);
                        foreach (var c in locks[item.Key])
                        {
                            locks3[item.Key] = MaskAdd(locks3[item.Key], charToInt(c));
                        }
                    }
                }

                public static int charToInt(char c)
                {
                    if (c >= 'a' && c <= 'z')
                    {
                        return c - 'a'+1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                public static int MaskAdd(int mask, int bit)
                {
                    return mask | (1 << bit);
                }
                public static bool isSubset(int a, int sub)
                {
                    return (a & sub) == sub;
                }
                public static bool isIn(int mask, int bit)
                {
                    return (mask & (1 << bit)) == (1 << bit);
                }

                public struct State
                {
                    public char end;
                    public int visited;
                    public State(char c, int p,int num)
                    {
                        end = c;
                        visited = p;
                        NumbOfNodes = num;
                    }
                    public int NumbOfNodes;
                    public override string ToString()
                    {
                        return String.Format("{0}, {1}, {2}", Convert.ToString(visited,toBase:2), end, NumbOfNodes);
                    }
                }

                public static Dictionary<State, int> mem = new Dictionary<State, int>();
                public static Queue<State> needUpdate = new Queue<State>();

                public static int min = int.MaxValue;

                public static int scan(char startChar)
                {
                    var startState = new State(startChar, MaskAdd(0,charToInt(startChar)), 1);
                    needUpdate.Enqueue(startState);
                    mem.Add(startState, 0);

                    int maxLength = nodes[startChar].links.Count;

                    while (needUpdate.Count > 0)
                    {
                        if (needUpdate.Count%100==0)
                        {
                            Console.WriteLine(needUpdate.Count);
                        }
                        var state = needUpdate.Dequeue();
             
                        if (state.NumbOfNodes == maxLength+1)
                        {
                            if (min > mem[state])
                            {
                                min = mem[state];
                                Console.WriteLine(state + " " + mem[state]);
                            }
                        }
                        else
                        {
                            var n1 = nodes[state.end];
                            foreach (var n2 in n1.links)
                            {
                                if (!isIn(state.visited, charToInt(n2.Key)))
                                {
                                    if (isSubset(state.visited, n1.locks3[n2.Key]))
                                    {
                                        State dest = new State();
                                        dest.end = n2.Key;
                                        dest.visited = MaskAdd(state.visited, charToInt(n2.Key));
                                        dest.NumbOfNodes = state.NumbOfNodes + 1;

                                        if (!mem.ContainsKey(dest))
                                        {
                                            mem.Add(dest, mem[state] + n2.Value);
                                        }
                                        else
                                        {
                                            mem[dest] = Math.Min(mem[dest], mem[state] + n2.Value);
                                        }

                                        if (!needUpdate.Contains(dest))
                                        {
                                            needUpdate.Enqueue(dest);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return 0;
                }




            }

            public static Dictionary<char, node> nodes = new Dictionary<char, node>();




            public static string map;
            Stack<point> toScan = new Stack<point>();
            public Dictionary<point, int> scanned = new Dictionary<point, int>();
            public Dictionary<point, string> locks = new Dictionary<point, string>();


            public static int wd;
            char stChar;




            public void scanFrom(char start)
            {
                stChar = start;
                var i = map.IndexOf(start);
                //map = map.Replace(start, ".");

                int sy = i / wd;
                int sx = i % wd;
                point sp = new point(sx, sy);

                toScan.Push(sp);
                scanned.Add(sp, 0);
                locks.Add(sp, "");

                while (toScan.Count > 0)
                {
                    var p = toScan.Pop();
                    var d = scanned[p];
                    var l = locks[p];

                    checkDir(new point(p.x, p.y - 1), d, l);
                    checkDir(new point(p.x, p.y + 1), d, l);
                    checkDir(new point(p.x - 1, p.y), d, l);
                    checkDir(new point(p.x + 1, p.y), d, l);

                }
            }

            char charAt(int x, int y)
            {
                return map[wd * y + x];
            }

            void checkDir(point p, int dist, string l)
            {
                block bl = whatis(p.x, p.y);
                var np = p;
                if (bl == block.empty)
                {

                    if (!scanned.ContainsKey(np))
                    {
                        scanned.Add(np, dist + 1);
                        locks.Add(np, l);
                        toScan.Push(np);
                    }
                    else
                    {
                        if (scanned[np] > dist + 1)
                        {
                            scanned[np] = dist + 1;
                            locks[np] = l;
                            toScan.Push(np);
                        }
                    }
                }
                else if (bl == block.door)
                {

                    if (!scanned.ContainsKey(np))
                    {
                        scanned.Add(np, dist + 1);
                        locks.Add(np, l + charAt(p.x, p.y));
                        toScan.Push(np);
                    }
                    else
                    {
                        if (scanned[np] > dist + 1)
                        {
                            scanned[np] = dist + 1;
                            locks[np] = l + charAt(p.x, p.y);
                            toScan.Push(np);
                        }
                    }


                }
                else if (bl == block.key || bl == block.player)
                {
                    var c = charAt(p.x, p.y);
                    if (c != stChar)
                    {
                        if (!nodes[stChar].links.ContainsKey(c))
                        {
                            nodes[stChar].links.Add(c, dist + 1);
                            nodes[stChar].locks.Add(c, l.ToLower());

                        }
                        else if (dist + 1 < nodes[stChar].links[c])
                        {
                            nodes[stChar].links[c] = dist + 1;
                            nodes[stChar].locks[c] = l.ToLower();
                        }
                    }
                    if (!scanned.ContainsKey(np))
                    {
                        scanned.Add(np, dist + 1);
                        locks.Add(np, l + charAt(p.x, p.y));
                        toScan.Push(np);
                    }
                    else
                    {
                        if (scanned[np] > dist + 1)
                        {
                            scanned[np] = dist + 1;
                            locks[np] = l + charAt(p.x, p.y);
                            toScan.Push(np);
                        }
                    }
                }
            }
            enum block
            {
                empty,
                wall,
                door,
                key,
                player,
                wat
            }
            block whatis(int x, int y)
            {
                char c = map[wd * y + x];
                if (c == '.')
                {
                    return block.empty;
                }
                if (c == '@')
                {
                    return block.player;
                }
                if (c >= 'a' && c <= 'z')
                {
                    return block.key;
                }
                if (c >= 'A' && c <= 'Z')
                {
                    return block.door;
                }
                if (c == '#')
                {
                    return block.wall;
                }

                return block.wat;
            }

        }

        string map;
        public override void Calc()
        {
            map = input.Replace("\r\n", "\n");

            scaner.map = map;
            scaner.wd = map.IndexOf("\n") + 1;

            var srch = input.Replace(".", "").Replace("\n", "").Replace("\r", "").Replace("#", "");

            var n = new scaner.node();
            n.name = '@';
            scaner.nodes.Add('@', n);

            foreach (char c in srch)
            {
                if (char.IsLower(c) && !scaner.nodes.ContainsKey(c))
                {
                    n = new scaner.node();
                    n.name = c;
                    scaner.nodes.Add(c, n);
                    //  Console.WriteLine(c);
                }

            }

            foreach (var c in scaner.nodes.Keys)
            {
                var s = new scaner();
                s.scanFrom(c);

            }

            foreach (var a in scaner.nodes)
            {
                Console.WriteLine();
                foreach (var b in a.Value.links.Keys)
                {
                    Console.WriteLine("{0} -> {1} : {2} : {3}", a.Key, b, a.Value.links[b], a.Value.locks[b]);
                }
            }

            foreach (var a in scaner.nodes)
            {
                a.Value.genBinMask();
            }



            int st = scaner.node.MaskAdd(0, scaner.node.charToInt('@'));

            var r = scaner.node.scan('@');





            output = "" + scaner.node.min;

        }


    }
}
