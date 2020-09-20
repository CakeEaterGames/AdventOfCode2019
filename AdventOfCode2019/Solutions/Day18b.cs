using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day18b : Problem
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
                        return c - 'a'+4;
                    }
                    else
                    {
                        switch (c)
                        {
                            case '@': return 0;
                            case '$': return 1;
                            case '&': return 2;
                            case '*': return 3;
                        }

                    }
                    return -1;
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
                    public char end1;
                    public char end2;
                    public char end3;
                    public char end4;

                    public int visited;

                    public int NumbOfNodes;

                    public State(State s)
                    {
                        end1 = s.end1;
                        end2 = s.end2;
                        end3 = s.end3;
                        end4 = s.end4;

                        visited = s.visited;
                    

                        NumbOfNodes = s.NumbOfNodes;
                    }
 

                    public override string ToString()
                    {
                        return String.Format("{0} ({1},{2},{3},{4}) {5}", Convert.ToString(visited,toBase:2), end1, end2, end3, end4, NumbOfNodes);
                    }
                }

                public static Dictionary<State, int> mem = new Dictionary<State, int>();
                public static Queue<State> needUpdate = new Queue<State>();

                public static int min = int.MaxValue;

                public static void scanN(State curState, int n)
                {
                    node n1;
                    int visited = curState.visited;
                    switch (n)
                    {
                        case 0: n1 = nodes[curState.end1];  break;
                        case 1: n1 = nodes[curState.end2];  break;
                        case 2: n1 = nodes[curState.end3]; break;
                        case 3: n1 = nodes[curState.end4];  break;
                        default: n1 = nodes[curState.end1];  break;
                    }

                
                    foreach (var n2 in n1.links)
                    {
                        if (!isIn(visited, charToInt(n2.Key)))
                        {
                            if (isSubset(visited, n1.locks3[n2.Key]))
                            {
                                State dest = new State(curState);

                                switch (n)
                                {
                                    case 0: dest.end1 = n2.Key; break;
                                    case 1: dest.end2 = n2.Key; break;
                                    case 2: dest.end3 = n2.Key; break;
                                    case 3: dest.end4 = n2.Key; break;
                                    default: dest.end1 = n2.Key; break;
                                }

                             
                                dest.visited = MaskAdd(visited, charToInt(n2.Key));
                                dest.NumbOfNodes = curState.NumbOfNodes + 1;

                                if (!mem.ContainsKey(dest))
                                {
                                    mem.Add(dest, mem[curState] + n2.Value);
                                }
                                else
                                {
                                    mem[dest] = Math.Min(mem[dest], mem[curState] + n2.Value);
                                }

                                if (!needUpdate.Contains(dest))
                                {
                                    needUpdate.Enqueue(dest);
                                }
                            }
                        }
                    }
                }

                public static void scan(char[] startChar)
                {
                    var startState = new State();

                    startState.end1 = startChar[0];
                    startState.end2 = startChar[1];
                    startState.end3 = startChar[2];
                    startState.end4 = startChar[3];

                    int mask = 0;
                    for (int i = 0; i < startChar.Length; i++)
                    {
                        //Console.WriteLine("MASK" + Convert.ToString(mask, toBase: 2));
                        mask = MaskAdd(mask, charToInt(startChar[i]));
                    }
                  //  mask = MaskAdd(mask, charToInt('a'));
                 //   mask = MaskAdd(mask, charToInt('z'));
                    //Console.WriteLine("MASK" + Convert.ToString(mask, toBase: 2));
                    startState.visited = mask;
                

                    startState.NumbOfNodes = 4;

                    needUpdate.Enqueue(startState);
                    mem.Add(startState, 0);

                    int maxLength = 0;
                    foreach (char item in startChar)
                    {
                        //Console.WriteLine("MAX LENGTH " + maxLength);
                        maxLength += nodes[item].links.Count;
                    }
                   // Console.WriteLine("MAX LENGTH "+maxLength);
                  

                    while (needUpdate.Count > 0)
                    { /*
                        if (needUpdate.Count%100==0)
                        {
                            Console.WriteLine(needUpdate.Count);
                        }
                         */
                        var curState = needUpdate.Dequeue();

                     //   Console.WriteLine(curState + " length: " + mem[curState]);
                        if (curState.NumbOfNodes == maxLength+4)
                        {
                            if (min > mem[curState])
                            {
                                min = mem[curState];
                                //Console.WriteLine(state + " " + mem[state]);
                            }
                        }
                        else
                        {
                            for (int i = 0;i<=3;i++)
                            {
                                scanN(curState, i);
                            }
                            
                        }
                    }
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
                if (c == '@' || c == '$' || c == '&' || c == '*')
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

            int wd = map.IndexOf("\n") + 1;
          
            int center = map.IndexOf('@');
            int sx = center % wd;
            int sy = center / wd;

            StringBuilder sb = new StringBuilder(map);

            sb[sx - 1 + (sy-1) * wd] = '@';
            sb[sx + (sy - 1) * wd] = '#';
            sb[sx + 1 + (sy - 1) * wd] = '$';

            sb[sx - 1 + sy * wd] = '#';
            sb[sx + sy * wd] = '#';
            sb[sx + 1 + sy * wd] = '#';

            sb[sx - 1 + (sy + 1) * wd] = '*';
            sb[sx + (sy + 1) * wd] = '#';
            sb[sx + 1 + (sy + 1) * wd] = '&';



            map = sb.ToString();
            //Console.WriteLine(map);
            var srch = input.Replace(".", "").Replace("\n", "").Replace("\r", "").Replace("#", "");


            var n1 = new scaner.node();
            n1.name = '@';
            scaner.nodes.Add('@', n1);

            var n2 = new scaner.node();
            n2.name = '$';
            scaner.nodes.Add('$', n2);

            var n3 = new scaner.node();
            n3.name = '&';
            scaner.nodes.Add('&', n3);

            var n4 = new scaner.node();
            n4.name = '*';
            scaner.nodes.Add('*', n4);


            scaner.map = map;
            scaner.wd = wd;


            foreach (char c in srch)
            {
                if (char.IsLower(c) && !scaner.nodes.ContainsKey(c))
                {
                    scaner.node n = new scaner.node();
                    n.name = c;
                    scaner.nodes.Add(c, n);
                    //Console.WriteLine(c);
                }

            }

            foreach (var c in scaner.nodes.Keys)
            {
                var s = new scaner();
                s.scanFrom(c);

            }
          /*
            foreach (var a in scaner.nodes)
            {
                Console.WriteLine();
                foreach (var b in a.Value.links.Keys)
                {
                    Console.WriteLine("{0} -> {1} : {2} : {3}", a.Key, b, a.Value.links[b], a.Value.locks[b]);
                }
            }
          */
            //Console.WriteLine("Wait for it...");
            foreach (var a in scaner.nodes)
            {
                a.Value.genBinMask();
            }



           // int st = scaner.node.MaskAdd(0, scaner.node.charToInt('@'));

            scaner.node.scan(new char[] { '@', '$', '&', '*' });
            

            output = "" + scaner.node.min;

        }
      

    }
}
