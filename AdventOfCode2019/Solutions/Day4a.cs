using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day4a : Problem
    {

        public override void Calc()
        {
            var inp = Tools.SplitToIntArray(input,'-');
            int count = 0;


            for (int i = inp[0];i<=inp[1];i++)
            {
                //Console.WriteLine(isValid(i) + " " + i);
                if (isValid(i))
                {
                    count++;
                }
               
            }
            output = ""+count;
        }


        bool isValid(int inp)
        {
            string s = inp+"";
            char last = '-';
            bool pair = false;
            foreach (char c in s)
            {
                if (last == c)
                {
                    pair = true;
                }

                if (last != '-')
                {
                    if (last>c)
                    {
                        return false;
                    }
                }
               
                    last = c;
            
            }

            return pair;
        }
    }
}
