using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day18a_fuck : Problem
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
            int wd;
            public string map;
            Stack<point> toScan = new Stack<point>();
            public Dictionary<point, int> scanned = new Dictionary<point, int>();

            int startingDistance = 0;

            public static int minDist = int.MaxValue;
            bool stop = false;

            public scaner(string Map, int distance)
            {
                //  Console.WriteLine(Map+"\n");
                //  Console.ReadLine();
                startingDistance = distance;
                map = Map;
                wd = map.IndexOf("\n") + 1;

                var i = map.IndexOf("@");
                map = map.Replace("@", ".");

                int sy = i / wd;
                int sx = i % wd;
                point p = new point(sx, sy);

                toScan.Push(p);
                scanned.Add(p, distance);
                scan();
            }

            public void scan()
            {
                if (startingDistance < minDist)
                {

                    while (toScan.Count > 0)
                    {
                        var p = toScan.Pop();
                        var d = scanned[p];

                        //Console.WriteLine(p);
                        //Console.WriteLine(whatis(p.x, p.y));



                        if (d <= minDist)
                        {
                            checkDir(new point(p.x, p.y - 1), d);
                            checkDir(new point(p.x, p.y + 1), d);
                            checkDir(new point(p.x - 1, p.y), d);
                            checkDir(new point(p.x + 1, p.y), d);
                        }
                        else
                        {
                            stop = true;
                        }
                    }

                    if (keys.Count == 0 && !stop)
                    {
                        minDist = Math.Min(minDist, startingDistance);
                        Console.WriteLine(minDist);
                    }

                    foreach (var a in keys)
                    {
                        char k = map[a.Key.x + a.Key.y * wd];
                        string newMap = map.Replace(k, '@').Replace(Char.ToUpper(k), '.');

                        if (a.Value <= scaner.minDist)
                        {
                            scaner s = new scaner(newMap, a.Value);
                        }
                    }

                  
                }



            }

            public void checkDir(point p, int dist)
            {
                block bl = whatis(p.x, p.y);
                if (bl == block.empty)
                {
                    var np = p;
                    if (!scanned.ContainsKey(np))
                    {
                        scanned.Add(np, dist + 1);
                        toScan.Push(np);
                    }
                    else
                    {
                        if (scanned[np] > dist + 1)
                        {
                            scanned[np] = dist + 1;
                            toScan.Push(np);
                        }
                    }
                }
                else if (bl == block.key)
                {
                    var np = p;
                    if (!keys.ContainsKey(np))
                    {
                        keys.Add(np, dist + 1);
                    }
                    else
                    {
                        if (keys[np] > dist + 1)
                        {
                            keys[np] = dist + 1;
                        }
                    }
                }


            }

            public Dictionary<point, int> keys = new Dictionary<point, int>();

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


        public override void Calc()
        {
            input = input.Replace("\r\n", "\n");

            scaner s = new scaner(input, 0);


            output = scaner.minDist + "";


        }




    }
}
