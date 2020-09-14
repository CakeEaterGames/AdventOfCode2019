using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day16b : Problem
    {

        int[] inp;
        int offset = 0;
        public override void Calc()
        {

            inp = Tools.StringToIntArray(input);
            offset = int.Parse(input.Substring(0, 7));

             
            for (int i = 0; i < 100; i++)
            {
                int pos = inp.Length * 10000 - 1;
                int sum = 0;
                while (pos >= offset)
                {
                    sum += Get(pos);
                    set(pos, sum % 10);
                    pos--;
                }
            }
            string res = "";
            for (int i = offset; i < offset+8; i++)
            {
               res+=Get(i);
            }
            output = res;

        }

        Dictionary<int, int> dic = new Dictionary<int, int>();
        int Get(int i)
        {
            if (dic.ContainsKey(i))
            {
                return dic[i];
            }
            return inp[i % inp.Length];
        }
        void set(int i, int val)
        {
            if (dic.ContainsKey(i))
            {
                dic[i] = val;
            }
            else
            {
                dic.Add(i, val);
            }
        }




    }
}
