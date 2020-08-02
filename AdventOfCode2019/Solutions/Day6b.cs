using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class Day6b : Problem
    {
        public static Day6b self;
        class node
        {
            public string name = "";
            public List<node> ForwardLinks = new List<node>();
            public List<node> BackLinks = new List<node>();
            public int CountDirectLinks()
            {
                return BackLinks.Count;
            }

            public int CountInDirectLinks()
            {
                int sum = BackLinks.Count;

                foreach (node n in BackLinks)
                {
                    sum += n.CountInDirectLinks();
                }

                return sum;
            }

            public void SearchFor(node dest, List<node> path)
            {
                
             

                if (BackLinks.Contains(dest))
                {
                    //Console.WriteLine(path.Count-1);
                    Day6b.self.output = ""+(path.Count - 1);
                }
                else if (ForwardLinks.Contains(dest))
                {
                   // Console.WriteLine(path.Count - 1);
                    Day6b.self.output = "" + (path.Count - 1);
                }
                else
                {
                    foreach (var n in BackLinks)
                    {
                        if (!path.Contains(n))
                        {
                            List<node> path2 = new List<node>(path);
                            path2.Add(this);
                            n.SearchFor(dest, path2);
                        }
                    }

                    foreach (var n in ForwardLinks)
                    {
                        if (!path.Contains(n))
                        {
                            List<node> path2 = new List<node>(path);
                            path2.Add(this);
                            n.SearchFor(dest, path2);
                        }
                    }

                }





            }

        }

        Dictionary<string, node> Nodes = new Dictionary<string, node>();

        public override void Calc()
        {
            self = this;
            var inp = input.Replace("\r","").Split('\n');
            foreach (var pairS in inp)
            {
                var pair = pairS.Split(')');

                if (!Nodes.ContainsKey(pair[0]))
                {
                    node n = new node();
                    n.name = pair[0];
                    Nodes.Add(pair[0],n);
                }
                if (!Nodes.ContainsKey(pair[1]))
                {
                    node n = new node();
                    n.name = pair[1];
                    Nodes.Add(pair[1], n);
                }

                Nodes[pair[0]].ForwardLinks.Add(Nodes[pair[1]]);
                Nodes[pair[1]].BackLinks.Add(Nodes[pair[0]]);

            }

            Nodes["YOU"].SearchFor(Nodes["SAN"], new List<node>());

            
 

        }
    }
}
