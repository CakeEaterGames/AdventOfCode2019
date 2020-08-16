using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day10a : Problem
    {

        char[][] map;

        struct Point
        {
            public int X;
            public int Y;
            public Point(int a, int b)
            {
                X = a;
                Y = b;
            }
            public override string ToString()
            {
                return "(" + X + ", " + Y + ") ";
            }
        }

        List<Point> points = new List<Point>();


        public override void Calc()
        {
            var temp = input.Split('\n');
            map = new char[temp[0].Length][];
            for (int i = 0; i < temp.Length; i++)
            {
                map[i] = new char[temp.Length];
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
                }
            }

            Point maxP = new Point(0, 0);
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
                    maxP = p;
                    maxI = count;
                }

            }

            //Console.WriteLine(maxP+" "+(maxI));
            output = maxI+"";
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
