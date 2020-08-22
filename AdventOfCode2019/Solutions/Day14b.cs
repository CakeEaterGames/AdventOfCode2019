using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day14b : Problem
    {

        class recepie
        {
            public string result = "";
            public int quantity = 0;
            public List<string> components = new List<string>();
            public List<int> quantities = new List<int>();

           public List<recepie> componentsLink = new List<recepie>();

            public long available = 0;
/*
            bool downgraded = false;
            void downgrade()
            {
                if (!downgraded)
                {

                for (int i = 0; i < componentsLink.Count; i++)
                {
                    quantities[i] /= 10;
                }
                quantity /= 10;
                downgraded = true;

                }
            }
            */
            public void craft()
            {
                if (result == "ORE")
                {
                    Console.WriteLine("Not enough ore");
                    Console.ReadLine();
                    return;
                }

                for (int i = 0; i < componentsLink.Count; i++)
                {
                    if (componentsLink[i].available<quantities[i])
                    {
                        /*if (componentsLink[i].components[0]=="ORE")
                        {
                            downgrade();
                            i = -1;
                            continue;
                        }*/
                        componentsLink[i].craft();
                        i = -1;
                        continue;
                    }
                }
                for (int i = 0; i < componentsLink.Count; i++)
                {
                    componentsLink[i].available -= quantities[i];
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
                res += "Available: " + available;
                return res;
            }

        }



        Dictionary<string, recepie> rec = new Dictionary<string, recepie>();

        Dictionary<string, int> have = new Dictionary<string, int>();
        Dictionary<string, int> need = new Dictionary<string, int>();

        const long tril = 1000000000000;
        public override void Calc()
        {
          

            input = input.Replace(" => ", "=").Replace(", ", ",");
            //Console.WriteLine(input);
            var lines = input.Split('\n');

            int mul = 1;
            foreach (var a in lines)
            {
                recepie r = new recepie();
                //2 NMWJT, 7 NXVR, 6 LNVPT => 9 TWVWC
                var b = a.Replace("\r", "").Split('=');

                var c = b[0].Split(',');
                foreach (var d in c)
                {
                    var e = d.Split(' ');
                    r.quantities.Add(int.Parse(e[0])*mul);
                    r.components.Add(e[1]);
                }

                c = b[1].Split(',');
                foreach (var d in c)
                {
                    var e = d.Split(' ');
                    r.quantity = int.Parse(e[0]) * mul;
                    r.result = e[1];
                }

                rec.Add(r.result, r);
 
            }

          

            recepie rr = new recepie();
            rr.result = "ORE";
            rr.available = tril;
            rec.Add(rr.result, rr);
/*
            foreach (var r in  rec)
            {
                Console.WriteLine(r);
            }
            Console.ReadLine();
            */
            foreach (var r in rec)
            {
                foreach (var r2 in r.Value.components)
                {
                    r.Value.componentsLink.Add(rec[r2]);
                }
            }



            int t = 0;

            while (rec["ORE"].available > 0)
            {

               /* foreach (var r in rec)
                {
                    Console.WriteLine(r);
                }
                Console.ReadLine();
                */
                rec["FUEL"].craft();
                t++;

                if (t%5000==0)
                {
                    Console.WriteLine((int)(tril/((double)(tril - rec["ORE"].available) / (double)rec["FUEL"].available)));

                }

                //  bool found = false;
                /*   foreach (var r in rec)
                   {
                       if (r.Value.available>0 && r.Key != "ORE" && r.Key != "FUEL")
                       {
                           found = true;
                           break;
                       }
                   }

                   if (!found)
                   {
                       foreach (var r in rec)
                       {
                           Console.WriteLine(r);
                       }
                   }
                   */
            }

           



          output = (rec["FUEL"].available)+"";
 
           
        }

        /*  void addIfNotFound(Dictionary a,)
          {

          }*/
    }
}
