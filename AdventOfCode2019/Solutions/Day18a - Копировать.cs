using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day18a_copy : Problem
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
 


                public void genLocks2()
                {
                    foreach (var item in locks)
                    {
                        locks3.Add(item.Key, 0);
                        foreach (var c in locks[item.Key])
                        {
                            locks3[item.Key]=  MaskAdd(locks3[item.Key], charToInt(c));
                           
                        }

                       // Console.WriteLine(locks[item.Key] + " "+Convert.ToString(locks3[item.Key],toBase:2));

                    }
                }



                public static int charToInt(char c)
                {
                    if (c>='a' && c<='z')
                    {
                        return c - 'a';
                    }
                    else
                    {
                        return 27;
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

                public int search(int path, int length, int count)
                {
                    int minLen = int.MaxValue;
                    //Console.WriteLine(Convert.ToString(path, toBase: 2));
                   // Console.WriteLine(count);
                   // Console.WriteLine(links.Count);
                    if (length < minimumLength)
                    {
                        if (count == links.Count)
                        {
                            minLen = length;
                            if (length < minimumLength)
                            {
                                minimumLength = length;
                                Console.WriteLine("min:"+path + " " + length);
                            }
                        }
                        else
                        {
                            foreach (var k in links.Keys)
                            {
                                if (!isIn(path, charToInt(k)))
                                {
                                    if (isSubset(path, locks3[k]))
                                    {
                                        minLen = Math.Min(minLen, scaner.nodes[k].search(MaskAdd(path, charToInt(k)), length + links[k],count+1));
                                    }
                                }
                            }
                        }
                    }
                    
                    return minLen;
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
                a.Value.genLocks2();

            }

            int st= scaner.node.MaskAdd(0, scaner.node.charToInt('@'));
 

            output = "" + scaner.nodes['@'].search(st, 0,0);

        }


    }
}
