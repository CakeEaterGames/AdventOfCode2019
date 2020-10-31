using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day24a : Problem
    {

        bool[,] grid = new bool[5, 5];
        bool[,] grid2 = new bool[5, 5];
        public override void Calc()
        {
            var inp1 = input.Split('\n');

          
            


            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (input[i * 7 + j] == '#')
                    {
                        grid[i, j] = true;
                    }
                }
            }

            /*
            var g = gridToInt();
            Console.WriteLine(Convert.ToString(g, 2));
            */

            while (true)
            {
                iterate();
               int   g = gridToInt();
                if (Visited.Contains(g))
                {
                    //Console.WriteLine(g);
                    /*  for (int i = 0; i < 5; i++)
                      {
                          for (int j = 0; j < 5; j++)
                          {
                              if (grid[i, j])
                              {
                                  Console.Write('#');
                              }
                              else
                              {
                                  Console.Write('.');
                              }

                          }
                          Console.WriteLine();
                      }*/

                    // Console.ReadLine();

                    output = g + "";
                    break;
                }
                Visited.Add(g);
            }

        }

        void iterate()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    grid2[i, j] = grid[i, j];
                    if (grid[i, j])
                    {
                        if (countNeighbors(i, j) != 1)
                        {
                            grid2[i, j] = false;
                        }
                    }
                    else
                    {
                        var c = countNeighbors(i, j);
                        if (c == 1 || c == 2)
                        {
                            grid2[i, j] = true;
                        }
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    grid[i, j] = grid2[i, j];
                }
            }

        }
        int countNeighbors(int x, int y)
        {
            int cnt = 0;
            if (x > 0 && grid[x - 1, y]) cnt++;
            if (x < 4 && grid[x + 1, y]) cnt++;
            if (y > 0 && grid[x, y - 1]) cnt++;
            if (y < 4 && grid[x, y + 1]) cnt++;

            return cnt;
        }

        HashSet<int> Visited = new HashSet<int>();
        int gridToInt()
        {
            int res = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (grid[i,j])
                    {
                        res += (1<<(i*5+j));
                    }
                }
            }
            return res;
        }

    }
}
