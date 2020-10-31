using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day20a : Problem
    {
        string map;
        int mapW;
        int mapH;
        public override void Calc()
        {
            map = input.Replace("\r", "") + " ";
            mapW = map.IndexOf("\n") + 1;
            mapH = map.Length / mapW;


            LocatePoints();
/*
            Console.WriteLine(mapW);
            Console.WriteLine(mapH);


            for (int i = 0; i < mapH; i++)
            {
                for (int j = 0; j < mapW; j++)
                {
                    Console.Write(pos(j, i));
                }
                Console.WriteLine();
            }
            */
            foreach (var p in KeyPoints)
            {
                if (p.Value=="AA")
                {
                    entrance = p.Key;
                }
                if (p.Value == "ZZ")
                {
                    exit = p.Key;
                }

                if (!Links.ContainsKey(p.Value))
                {
                    Link l = new Link();
                    l.from = p.Value;
                    Links.Add(p.Value,l);
                }

            }
           // Console.WriteLine(entrance);
           // Console.WriteLine(exit);

            Scan();

            Links["AA"].minPath = 0;
            Links["AA"].needUpdate =true;

            bool toupdate = true;

            while (toupdate)
            {
              //  Console.WriteLine("ROUND");
                toupdate = false;
                foreach (var l in Links)
                {
                    if (l.Value.needUpdate)
                    {
                        l.Value.Update();
                        toupdate = true;
                    }
                }
              //  Console.WriteLine(Links["ZZ"].minPath);
            }


          output = ""+(Links["ZZ"].minPath-1);

        }

        point entrance,exit;

        struct point
        {
            public int X;
            public int Y;
            public point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public override string ToString()
            {
                return String.Format("({0} {1})", X, Y);
            }
        }

        struct portal
        {
            public int X;
            public int Y;
            public string Name;
            public bool Inner;
            public portal(int x, int y, String name, bool inner)
            {
                X = x;
                Y = y;
                Name = name;
                Inner = inner;
            }
            public override string ToString()
            {
                return String.Format("({0} {1})", X, Y);
            }
        }

        class Link
        {
            public string from;
            public point fromPoint;
            public List<string> To = new List<string>();
            public List<int> Lengths = new List<int>();

            public bool needUpdate = false;
            public int minPath = int.MaxValue/2;

            public void addLink(String to, int length)
            {
                if (To.Contains(to))
                {
                    int i = To.IndexOf(to);
                    if (Lengths[i]> length)
                    {
                        Lengths[i] = length;
                    }
                }
                else
                {
                    To.Add(to);
                    Lengths.Add(length);
                }
            }

            public void Update()
            {
                needUpdate = false;
                for (int i = 0; i < To.Count; i++)
                {
                    var newl = minPath + Lengths[i];
                    if (Day20a.Links[To[i]].minPath>newl)
                    {
                        Day20a.Links[To[i]].minPath = newl;
                        Day20a.Links[To[i]].needUpdate = true;
                    }
                }
            }

        }

        //    Dictionary<portal, string> Portals = new Dictionary<portal, string>();
        Dictionary<point, string> KeyPoints = new Dictionary<point, string>();
        static Dictionary<string, Link> Links = new Dictionary<string, Link>();

        void LocatePoints()
        {
            for (int y = 0; y < mapH; y++)
            {
                for (int x = 0; x < mapW; x++)
                {
                    var c = pos(x, y);
                    if (isLetter(c))
                    {

                        string name = "";

                        char cr = pos(x + 1, y);
                        char cd = pos(x, y + 1);
                        char cl = pos(x - 1, y);
                        char cu = pos(x, y - 1);

                        if (isLetter(cr))
                        {
                            name = c + "" + cr;
                            if (cl == '.')
                            {
                                setChar(x, y, '@');
                                setChar(x + 1, y, ' ');
                                KeyPoints.Add(new point(x, y), name);
                            }
                            else
                            {
                                setChar(x + 1, y, '@');
                                setChar(x, y, ' ');
                                KeyPoints.Add(new point(x + 1, y), name);
                            }

                        }
                        else if (isLetter(cd))
                        {
                            name = c + "" + cd;
                            if (cu == '.')
                            {
                                setChar(x, y, '@');
                                setChar(x, y + 1, ' ');
                                KeyPoints.Add(new point(x, y), name);
                            }
                            else
                            {
                                setChar(x, y + 1, '@');
                                setChar(x, y, ' ');
                                KeyPoints.Add(new point(x, y + 1), name);
                            }

                        }

                    }
                }
            }
        }

        void Scan()
        {
            foreach (var start in KeyPoints)
            {
                char[] Map = map.ToCharArray();
                //setChar(Map, entrance.X, entrance.Y, '0');
                Stack<point> toExplore = new Stack<point>();
                Stack<int> lengths = new Stack<int>();
                HashSet<point> explored = new HashSet<point>();
                
                //Console.WriteLine("GO! "+start.Value);

                toExplore.Push(start.Key);
                lengths.Push(0);

                while (toExplore.Count > 0)
                {
                   
                    point p = toExplore.Pop();
                    int l = lengths.Pop();

                    if (explored.Contains(p)) continue;

                    explored.Add(p);

                    if (pos(Map, p.X, p.Y) == '@')
                    {
                        if (start.Value != KeyPoints[p])
                        {
                           // Console.WriteLine("Path from {0} to {1} takes {2}", start.Value, KeyPoints[p], l - 1);
                            Links[start.Value].addLink(KeyPoints[p], l-1);
                        }
                    }

                    if (!explored.Contains(new point(p.X + 1, p.Y)) && pos(Map, p.X + 1, p.Y) == '.' || pos(Map, p.X + 1, p.Y) == '@') { toExplore.Push(new point(p.X + 1, p.Y)); lengths.Push(l + 1); }
                    if (!explored.Contains(new point(p.X - 1, p.Y)) && pos(Map, p.X - 1, p.Y) == '.' || pos(Map, p.X - 1, p.Y) == '@') { toExplore.Push(new point(p.X - 1, p.Y)); lengths.Push(l + 1); }
                    if (!explored.Contains(new point(p.X, p.Y + 1)) && pos(Map, p.X, p.Y + 1) == '.' || pos(Map, p.X, p.Y + 1) == '@') { toExplore.Push(new point(p.X, p.Y + 1)); lengths.Push(l + 1); }
                    if (!explored.Contains(new point(p.X, p.Y - 1)) && pos(Map, p.X, p.Y - 1) == '.' || pos(Map, p.X, p.Y - 1) == '@') { toExplore.Push(new point(p.X, p.Y - 1)); lengths.Push(l + 1); }

                    /*
                    ---------------------------------
                    ---------------------------------
                    ---------------------------------
                    ALL HAIL THE MIGHTY CODE BRICK!!!
                    ---------------------------------  
                    ---------------------------------                 
                    ---------------------------------
                    */
                }

            }
        }

        bool isLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        char pos(int x, int y)
        {
            if (x >= 0 && x < mapW && y >= 0 && y < mapH && map[mapW * y + x] != '\n')
            {
                return map[mapW * y + x];
            }
            return ' ';
        }
        void setChar(int x, int y, char c)
        {
            if (x >= 0 && x < mapW && y >= 0 && y < mapH && map[mapW * y + x] != '\n')
            {
                var sb = new StringBuilder(map);
                sb[mapW * y + x] = c;
                map = sb.ToString();
            }
        }

        char pos(char[] map, int x, int y)
        {
            if (x >= 0 && x < mapW && y >= 0 && y < mapH && map[mapW * y + x] != '\n')
            {
                return map[mapW * y + x];
            }
            return ' ';
        }
        void setChar(char[] map, int x, int y, char c)
        {
            if (x >= 0 && x < mapW && y >= 0 && y < mapH && map[mapW * y + x] != '\n')
            {
                map[mapW * y + x] = c;
            }
        }
    }
}
