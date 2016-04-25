using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CompGeo1
{

    class Vec
    {
        public float x, y;

        public Vec(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Line
    {
        public int nr;
        public Vec p, q;
        public Vec n;
        public float a;

        public Line(int nr, Vec p, Vec q)
        {
            this.p = p;
            this.q = q;
            this.nr = nr;
            n = new Vec(p.y - q.y, q.x - p.x);
            a = p.y * q.x - p.x * q.y;
        }

        public bool intersect(Line other)
        {
            float ccw1 = ccw(this, other.p);
            float ccw2 = ccw(this, other.q);
            float ccw3 = ccw(other, this.p);
            float ccw4 = ccw(other, this.q);

            if (ccw1 == ccw2)
            {
                if (this.p.x == this.q.x)
                {
                    return (other.p.y <= Math.Max(p.y, q.y) && other.p.y >= Math.Min(p.y, q.y));
                }
                else
                {
                    return (other.p.x <= Math.Max(p.x, q.x) && other.p.x >= Math.Min(p.x, q.x));
                }
            }

            return (ccw1 * ccw2 <= 0 & ccw3 * ccw4 <= 0);
        }

        public static float ccw(Line line, Vec r)
        {
            return line.n.x * r.x + line.n.y * r.y - line.a;
        }
    }




    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Naiver Algorithmus");

            List<Line> lines = new List<Line>();

            StreamReader f = new StreamReader("s_1000_1.dat");

            int lineNr = 0;
            string txtLine;
            while ((txtLine = f.ReadLine()) != null)
            {
                txtLine = txtLine.Trim();
                string[] Vecs = txtLine.Split(' ');
                float ax = float.Parse(Vecs[0]);
                float ay = float.Parse(Vecs[1]);
                float bx = float.Parse(Vecs[2]);
                float by = float.Parse(Vecs[3]);
                lines.Add(new Line(++lineNr, new Vec(ax, ay), new Vec(bx, by)));
            }


            int found = 0;
            int all = 0;
            for (int i = 0; i < lines.Count; i++)
                for (int j = i + 1; j < lines.Count; j++)
                {
                    all++;
                    if (lines[i].intersect(lines[j]))
                    {
                        Console.WriteLine(String.Format("Intersection found between lines {0} and {1}", lines[i].nr, lines[j].nr));
                        found++;
                    }
                }


            foreach (Line l1 in lines)
            {
                foreach (Line l2 in lines)
                {

                }
            }

            Console.WriteLine(all + " Lines intersected and " + found + " intersections found");
            Console.ReadKey();
        }
    }
}
