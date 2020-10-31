using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day24b : Problem
    {
        const int l = 300;
        bool[,,] grid = new bool[l, 5, 5];
        bool[,,] grid2 = new bool[l, 5, 5];
        public override void Calc()
        {
            var inp1 = input.Split('\n');

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (input[i * 7 + j] == '#')
                    {
                        grid[l / 2, j, i] = true;
                    }
                }
            }

            for (int i = 0; i < 200; i++)
            {
                iterate();
            }
        }

        void Draw(int d)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (grid[d, j, i])
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }

                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        void iterate()
        {
            for (int d = 0; d < l; d++)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (i == 2 && j == 2) continue;

                        var c = countNeighbors(d, i, j);
                        grid2[d, i, j] = grid[d, i, j];

                        if (grid[d, i, j])
                        {
                            if (c != 1)
                            {
                                grid2[d, i, j] = false;
                            }
                        }
                        else
                        {
                            if (c == 1 || c == 2)
                            {
                                grid2[d, i, j] = true;
                            }
                        }
                    }
                }
            }
            int count = 0;
            for (int d = 0; d < l; d++)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        grid[d, i, j] = grid2[d, i, j];
                        if (grid[d, i, j])
                        {
                            count++;
                        }
                    }
                }
            }
            output = count + "";
        }
        int countNeighbors(int d, int x, int y)
        {
            int cnt = 0;
            if (x > 0 && grid[d, x - 1, y]) cnt++;
            if (x < 4 && grid[d, x + 1, y]) cnt++;
            if (y > 0 && grid[d, x, y - 1]) cnt++;
            if (y < 4 && grid[d, x, y + 1]) cnt++;

            if (d > 0)
            {
                if (x == 4 && grid[d - 1, 3, 2]) cnt++;
                if (x == 0 && grid[d - 1, 1, 2]) cnt++;

                if (y == 4 && grid[d - 1, 2, 3]) cnt++;
                if (y == 0 && grid[d - 1, 2, 1]) cnt++;
            }
            if (d < l - 1)
            {
                if (x == 2 && y == 1) for (int i = 0; i < 5; i++) if (grid[d + 1, i, 0]) cnt++;
                if (x == 2 && y == 3) for (int i = 0; i < 5; i++) if (grid[d + 1, i, 4]) cnt++;

                if (x == 1 && y == 2) for (int i = 0; i < 5; i++) if (grid[d + 1, 0, i]) cnt++;
                if (x == 3 && y == 2) for (int i = 0; i < 5; i++) if (grid[d + 1, 4, i]) cnt++;
            }

            if (d == l - 1 && cnt > 1)
            {
                Console.WriteLine("Danger");
            }

            if (d == 0 && cnt > 1)
            {
                Console.WriteLine("Danger");
            }

            return cnt;
        }

        HashSet<int> Visited = new HashSet<int>();


    }
}
 