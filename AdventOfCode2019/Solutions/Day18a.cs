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
                dist = 0;
            }
            public int dist;
        }


        class scaner
        {
            int wd;
            string map;
            Stack<point> toScan = new Stack<point>();

            public scaner(point p,string Map)
            {
                map = Map;
                wd = map.IndexOf("\n");
                toScan.Push(p);
            }

            public void scan()
            {
                while (toScan.Count > 0)
                {
                    break;
                }
            }

            public List<point> dest = new List<point>();
        }


        public override void Calc()
        {
            input = input.Replace("\r\n", "\n");
            scaner s = new scaner(new point(0, 0), input);
        }




        int whatis(char c)
        {
            if (c=='.' || c == '@')
            {
                return 0;
            }
            if(c>='a' && c <= 'z')
            {
                return 1;
            }
            if (c >= 'A' && c <= 'Z')
            {
                return 2;
            }
            if (c == '.')
            {
                return 3;
            }
            return -1;
        }
    }
}
