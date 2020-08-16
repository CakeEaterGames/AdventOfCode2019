using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day10b : Problem
    {

        char[][] map;

        class Point
        {
            public int X;
            public int Y;
            public double angle;
            public Point(int a, int b)
            {
                X = a;
                Y = b;
                angle = 0;
            }
            public override string ToString()
            {
                return "(" + X + ", " + Y + ", " + angle + ") ";
            }
        }

        List<Point> points = new List<Point>();

        Point center = new Point(0, 0);
        public override void Calc()
        {
            /* for (int i = -5; i <= 5; i++)
             {
                 for (int j = -5; j <= 5; j++)
                 {
                     Console.WriteLine(i+" "+j+" "+((Math.Atan2(i, j) / Math.PI * 180+360)%360));
                 }
             }*/



            var temp = input.Split('\n');
            map = new char[temp.Length][];
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = new char[temp[0].Length];

            }

            for (int i = 0; i < temp.Length; i++)
            {
                for (int j = 0; j < temp[i].Length; j++)
                {
                    if (temp[i][j] == '#')
                    {
                        points.Add(new Point(j, i));
                        // Console.WriteLine(j + " " + i);
                    }
                    map[i][j] = temp[i][j];
                }
            }
            //writeMap();


            int maxI = -1;

            foreach (var p in points)
            {
                int count = -1;
                foreach (var p2 in points)
                {
                    bool br = false;
                    foreach (var p3 in points)
                    {
                        if (isBetween(p, p2, p3))
                        {
                            br = true;
                            break;
                        }

                    }
                    if (!br)
                    {
                        count++;
                    }
                }
                if (count > maxI)
                {
                    center = p;
                    maxI = count;
                }

            }
          //  Console.WriteLine(center + " " + (maxI));
            points.Remove(center);
            map[center.Y][center.X] = 'X';

            foreach (var p in points)
            {
                p.angle = ((Math.Atan2(p.X - center.X, p.Y - center.Y) / Math.PI * 180 + 360 + 180) % 360);
                if (p.angle == 0)
                {
                    p.angle = 360;
                }
            }

            int curAngle = 0;
            int newAngle = 0;


            points.Sort(Compare);
/*
            foreach (var p in points)
            {
                Console.WriteLine(p);
            }
            */

         //   Console.WriteLine();


            int remCount = 1;
            while (points.Count > 0)
            {


                for (int i = 0; i < points.Count; i++)
                {
                    var p = points[i];

                    List<Point> same = new List<Point>();
                    same.Add(p);
                    double angle = p.angle;
                    int oldI = i;
                    i++;
                    while (i < points.Count && points[i].angle == angle)
                    {
                        same.Add(points[i]);
                        i++;
                    }
                    i = oldI + same.Count - 2;

                    same.Sort(CompareDist);


                    foreach (var a in same)
                    {
                      //  Console.WriteLine(a);
                    }
                    p = same[0];
                    /*
                    map[p.Y][p.X] = '0';
                    writeMap();
                    map[p.Y][p.X] = '.';
                    */
                    points.Remove(p);
                    if (remCount==200)
                    {
                        output = (p.X * 100 + p.Y)+"";
                    }
                    //Console.WriteLine(remCount + " " + p);
                    remCount++;

                 
                }
            }





        }

        void writeMap()
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        int Compare(Point left, Point right)
        {
            return (int)(right.angle * 100000 - left.angle * 100000);
        }

        int CompareDist(Point left, Point right)
        {
            return (int)(dist(center, left) * 100000 - dist(center, right) * 100000);
        }

        int distX(Point a, Point b)
        {
            return b.X - a.X;
        }
        int distY(Point a, Point b)
        {
            return b.Y - a.Y;
        }

        double dist(Point a, Point b)
        {
            return Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
        }

        bool isSame(Point a, Point b)
        {
            return (a.X == b.X && a.Y == b.Y);
        }

        bool isBetween(Point start, Point end, Point check)
        {
            if (isSame(start, end) || isSame(check, end) || isSame(check, start))
            {
                return false;
            }

            int rX = Math.Min(start.X, end.X);
            int rY = Math.Min(start.Y, end.Y);
            int rX2 = Math.Max(start.X, end.X);
            int rY2 = Math.Max(start.Y, end.Y);

            //  if ((rX <= check.X && check.X <= rX2) && (rY <= check.Y && check.Y <= rY2))
            {
                if (check.X >= rX && check.X <= rX2 && check.Y >= rY && check.Y <= rY2)
                {
                    double dx = distX(start, end);
                    double dx2 = distX(start, check);

                    double dy = distY(start, end);
                    double dy2 = distY(start, check);


                    if (dy == 0 && dy2 == 0)
                    {
                        return true;
                    }
                    if (dx / dy == dx2 / dy2)
                    {
                        return true;
                    }

                }
            }


            return false;
        }
    }
}
