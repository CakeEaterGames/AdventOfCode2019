using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day3b : Problem
    {


        struct point
        {
            public point(int a, int b)
            {
                x = a;
                y = b;
              
            }
            public int x;
            public int y;
        }

        public override void Calc()
        {

            var inp = input.Split('\n');
            var inpA = inp[0].Split(',');
            var inpB = inp[1].Split(',');


            List<point> points = new List<point>();
            List<String> points2 = new List<String>();
            List<int> dists = new List<int>();
        

            int px = 0;
            int py = 0;
            int path = 0;

            foreach (var p in inpA)
            {
                char dir = p[0];
                int dist = int.Parse(p.Substring(1));
                //Console.WriteLine(dir + " " + dist);
                int vx = 0;
                int vy = 0;
                switch (dir)
                {
                    case 'U': vy = 1; break;
                    case 'D': vy = -1; break;
                    case 'R': vx = 1; break;
                    case 'L': vx = -1; break;
                }

                for (int i = dist; i > 0; i--)
                {
                    px += vx;
                    py += vy;
                    path++;
                    //  points.Add(new point(px, py));
                    points2.Add(px + ":" + py);
                    dists.Add(path);
                }


            }
            Console.WriteLine(points.Count);
            px = 0;
            py = 0;
            path = 0;

            int min = int.MaxValue;
            foreach (var p in inpB)
            {
                Console.WriteLine(p);
                char dir = p[0];
                int dist = int.Parse(p.Substring(1));
                int vx = 0;
                int vy = 0;
                switch (dir)
                {
                    case 'U': vy = 1; break;
                    case 'D': vy = -1; break;
                    case 'R': vx = 1; break;
                    case 'L': vx = -1; break;
                }

                for (int i = dist; i > 0; i--)
                {
                    px += vx;
                    py += vy;
                    path++;

                    if (points2.Contains(px + ":" + py))
                    {
                        var pp = points2.IndexOf(px + ":" + py);
          

                        min = Math.Min(min, path+dists[pp]);
                        Console.WriteLine(min);
                    }
                }


            }

            output = "" + min;


        }
    }
}
