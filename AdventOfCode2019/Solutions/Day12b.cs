using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day12b : Problem
    {

        class moon
        {
            public int x, y, z, vx, vy, vz;
            public int ox, oy, oz;
            public moon(int a, int b, int c)
            {
                x = a;
                y = b;
                z = c;

                ox = x;
                oy = y;
                oz = z;
            }
            public void move()
            {
                x += vx;
                y += vy;
                z += vz;
            }
            public bool isOnStart()
            {
                return ox == x && oy == y && oz == z;
            }

            public int energy()
            {
                return (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) * (Math.Abs(vx) + Math.Abs(vy) + Math.Abs(vz));
            }
            public override string ToString()
            {

                return vx+" ";
                //return vx + " " + vy + " " + vz + " ";


                //  return x + " " + y + " " + z + " ";

                //return x + " " + y + " " + z + " " + vx + " " + vy + " " + vz+" ";

                // return "pos=(" + x + " " + y + " " + z + ") vel=(" + vx + " " + vy + " " + vz + ")";
            }
        }

    

        List<moon> Moons = new List<moon>();

        List<string> statesX = new List<string>();
        List<string> statesY = new List<string>();
        List<string> statesZ = new List<string>();
        string GetStateX()
        {
            string result = "";
            foreach (var m in Moons)
            {
                result += m.x+" ";
            }
            return result;
        }
        string GetStateY()
        {
            string result = "";
            foreach (var m in Moons)
            {
                result += m.y + " ";
            }
            return result;
        }
        string GetStateZ()
        {
            string result = "";
            foreach (var m in Moons)
            {
                result += m.z + " ";
            }
            return result;
        }

        long repX = 0;
        long repY = 0;
        long repZ = 0;

        long _gcd(long a, long b)
        {

            while (a != b)
            {
                if (a > b)
                {
                    long tmp = a;
                    a = b;
                    b = tmp;
                }
                b = b - a;
            }
            return a;
        }

       
        public static long gcd(long a, long b)
        {
            if (a>b)
            {
                var t = a;
                a = b;
                b = t;
            }

            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public override void Calc()
        {/*
            int q = 286332;
            int w = 167624;
            int e = 96236;

            var t = gcd(q, w);
            Console.WriteLine(t);
            Console.WriteLine(q*w / t);
            Console.WriteLine(q / t * w);
            */

            Console.WriteLine(long.MaxValue.ToString().Length);

            var s = input.Replace("<", "").Replace(">", "").Replace(" ", "").Replace("\r\n", ",").Replace("x=", "").Replace("y=", "").Replace("z=", "");
            var r = s.Split(',');
            var nums = Tools.SplitToIntArray(s, ',');

            //Console.WriteLine(Tools.ArrayToString(nums));

            for (int i = 0; i < nums.Length; i += 3)
            {
                Moons.Add(new moon(nums[i], nums[i + 1], nums[i + 2]));
            }

            for (int i = 0; true; i++)
            {
               

                statesX.Add(GetStateX());
                statesY.Add(GetStateY());
                statesZ.Add(GetStateZ());


                int ci = 5;

                if (repX ==0 && statesX.Count > ci+1)
                {
                    bool found = true;
                    for (int j = 0; j < ci; j++)
                    {
                        if (statesX[statesX.Count - 1 - ci + j] == statesX[0 + j])
                        {
                            //Console.WriteLine(i);
                        }
                        else
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                       // Console.WriteLine(i-10);
                        repX = i - ci;
                    }
                }

                if (repY == 0 && statesY.Count > ci+1)
                {
                    bool found = true;
                    for (int j = 0; j < ci; j++)
                    {
                        if (statesY[statesY.Count - 1 - ci + j] == statesY[0 + j])
                        {
                            //Console.WriteLine(i);
                        }
                        else
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        //Console.WriteLine(i - 10);
                        repY = i - ci;
                    }
                }

                if (repZ == 0 && statesZ.Count > ci + 1)
                {
                    bool found = true;
                    for (int j = 0; j < ci; j++)
                    {
                        if (statesZ[statesZ.Count - 1 - ci + j] == statesZ[0 + j])
                        {
                            //Console.WriteLine(i);
                        }
                        else
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                       // Console.WriteLine(i - 10);
                        repZ = i - ci;
                    }
                }

                if (repX != 0 && repY != 0 && repZ != 0)
                {/*
                    repX = 2;
                    repY = 6;
                    repZ = 3;
                    */
                    Console.WriteLine(repX);
                    Console.WriteLine(repY);
                    Console.WriteLine(repZ);

                    long t = gcd(repY, repY);

                    long a = (repX / t * repY);
                    Console.WriteLine(a);

                    t = gcd(a, repZ);


                    output = ((a / t * repZ) ) + "";
                   // var a =  (gcd(gcd(repX, repY), repZ)) ;

                   //output = (repX / a * repY / a * repX)+"";

                    break;
                }

                foreach (var m in Moons)
                {
                    foreach (var m2 in Moons)
                    {
                        if (m.x > m2.x) m.vx--;
                        if (m.x < m2.x) m.vx++;
                        if (m.y > m2.y) m.vy--;
                        if (m.y < m2.y) m.vy++;
                        if (m.z > m2.z) m.vz--;
                        if (m.z < m2.z) m.vz++;
                    }
                }
                foreach (var m in Moons)
                {
                    m.move();                  
                }

                

            }

          
        }
    }
}
