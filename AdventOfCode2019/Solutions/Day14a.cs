﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day14a : Problem
    {

        class recepie
        {
            public string result = "";
            public int quantity = 0;
            public List<string> components = new List<string>();
            public List<int> quantities = new List<int>();

           public List<recepie> componentsLink = new List<recepie>();

            public int available = 0;

            public void craft()
            {
                for (int i = 0; i < componentsLink.Count; i++)
                {
                    if (componentsLink[i].available<quantities[i])
                    {
                        componentsLink[i].craft();
                        i = -1;
                        continue;
                    }
                }
                for (int i = 0; i < componentsLink.Count; i++)
                {
                    componentsLink[i].available -= quantities[i];
                    //Console.WriteLine(componentsLink[i].available);
                }

                available += quantity;
            }

         
            public override string ToString()
            {
                string res = "";
                res += quantity + " " + result + " = ";
                for (int i = 0; i < components.Count; i++)
                {
                    res += quantities[i] + " " + components[i] + ", ";
                }
                return res;
            }
        }



        Dictionary<string, recepie> rec = new Dictionary<string, recepie>();

        Dictionary<string, int> have = new Dictionary<string, int>();
        Dictionary<string, int> need = new Dictionary<string, int>();


        public override void Calc()
        {
            input = input.Replace(" => ", "=").Replace(", ", ",");
            //Console.WriteLine(input);
            var lines = input.Split('\n');


            foreach (var a in lines)
            {
                recepie r = new recepie();
                //2 NMWJT, 7 NXVR, 6 LNVPT => 9 TWVWC
                var b = a.Replace("\r", "").Split('=');

                var c = b[0].Split(',');
                foreach (var d in c)
                {
                    var e = d.Split(' ');
                    r.quantities.Add(int.Parse(e[0]));
                    r.components.Add(e[1]);
                }

                c = b[1].Split(',');
                foreach (var d in c)
                {
                    var e = d.Split(' ');
                    r.quantity = int.Parse(e[0]);
                    r.result = e[1];
                }

                rec.Add(r.result, r);
 
            }
            recepie rr = new recepie();
            rr.result = "ORE";
            rr.available = int.MaxValue;
            rec.Add(rr.result, rr);


            foreach (var r in rec)
            {
                foreach (var r2 in r.Value.components)
                {
                    r.Value.componentsLink.Add(rec[r2]);
                }
            }

            rec["FUEL"].craft();



          output = (int.MaxValue - rec["ORE"].available)+"";
 
           
        }

        /*  void addIfNotFound(Dictionary a,)
          {

          }*/
    }
}
