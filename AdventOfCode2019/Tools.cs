using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public class Tools 
    {
        public static List<int> StringListToIntList(List<String> inp)
        {
            var res = new List<int>();
            foreach (var a in inp)
            {
                res.Add(int.Parse(a));
            }
            return res;
        }
        public static int[] StringArrayToIntArray(string[] inp)
        {
            var res = new int[inp.Length];
            for (int i = 0;i<res.Length;i++)
            {
                res[i] = int.Parse(inp[i]);
            }
            return res;
        }

        public static int[] StringToIntArray(string inp)
        {
            var res = new int[inp.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = inp[i]-'0';
            }
            return res;
        }

        public static int[] SplitToIntArray(string inp, char spl)
        {
            return StringArrayToIntArray(inp.Split(spl));
        }

        public static string ArrayToString(int[] arr)
        {
            string res = "";

            res += "[";

            foreach (var a in arr)
            {
                res += a + ", ";
            }

            if (res.Length>1)
            {
               res = res.Remove(res.Length - 2);
            }

            res += "]";


            return res;
        }
    }
}
