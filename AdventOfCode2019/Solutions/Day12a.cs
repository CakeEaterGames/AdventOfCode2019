using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day12a : Problem
    {

        class moon
        {
            public int x, y, z, vx, vy, vz;
            public moon(int a, int b, int c)
            {
                x = a;
                y = b;
                z = c;


            }
            public void move()
            {
                x += vx;
                y += vy;
                z += vz;
            }
            public int energy()
            {
                return (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) * (Math.Abs(vx) + Math.Abs(vy) + Math.Abs(vz));
            }
            public override string ToString()
            { 
                return "pos=(" + x + " " + y + " " + z + ") vel=(" + vx + " " + vy + " " + vz + ")";
            }
        }

        List<moon> Moons = new List<moon>();
        public override void Calc()
        {
            var s = input.Replace("<", "").Replace(">", "").Replace(" ", "").Replace("\r\n", ",").Replace("x=", "").Replace("y=", "").Replace("z=", "");
            var r = s.Split(',');
            var nums = Tools.SplitToIntArray(s, ',');

            //Console.WriteLine(Tools.ArrayToString(nums));

            for (int i = 0; i < nums.Length; i += 3)
            {
                Moons.Add(new moon(nums[i], nums[i + 1], nums[i + 2]));
            }

            for (int i = 0; i < 1000; i++)
            {


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

            int en = 0;
            foreach (var m in Moons)
            {
                en += m.energy();
            //    Console.WriteLine(m);
            }
           // Console.WriteLine(en);
            output = en+"";
        }
    }
}
