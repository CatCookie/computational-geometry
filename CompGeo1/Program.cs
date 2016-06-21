using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace CompGeo1
{

    // Vector , Point
    class Vec
    {
        public float x, y;

        public Vec(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Vec other, float epsilon)
        {
            if (Math.Abs(this.x - other.x) <= epsilon && Math.Abs(this.y - other.y) <= epsilon)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return String.Format("({0}|{1})", x, y);
        }
    }

    class Line
    {
        public const float EPSYLON = 1E-5f;

        // Line Number
        public int nr;
        public Vec p, q;

        public Line(int nr, Vec p, Vec q)
        {
            this.p = p;
            this.q = q;
            this.nr = nr;
        }

        public bool intersect(Line other)
        {
            if (this.p.Equals(this.q, EPSYLON) && other.p.Equals(other.q, EPSYLON))
            {
                return this.p.Equals(other.p, EPSYLON);
            }
            else
            {
                float ccw1 = ccw(this.p, this.q, other.p);
                float ccw2 = ccw(this.p, this.q, other.q);
                float ccw3 = ccw(other.p, other.q, this.p);
                float ccw4 = ccw(other.p, other.q, this.q);

                // Beide Punkte von "other" liegen auf der selben Seite
                if (ccw1 * ccw2 > EPSYLON)
                    return false;

                // Die Punkte von "other" liegen auf unterschiedlichen Seiten.
                if (ccw1 * ccw2 < -EPSYLON)
                    return ccw3 * ccw4 <= EPSYLON;

                // Mindestens ein Punkt von "other" liegt auf "this"
                if (Math.Abs(ccw1) <= EPSYLON
                && other.p.x <= Math.Max(p.x, q.x) && other.p.x >= Math.Min(p.x, q.x)
                && other.p.y <= Math.Max(p.y, q.y) && other.p.y >= Math.Min(p.y, q.y))
                {
                    return true;
                }

                if (Math.Abs(ccw2) <= EPSYLON
                    && other.q.x <= Math.Max(p.x, q.x) && other.q.x >= Math.Min(p.x, q.x)
                    && other.q.y <= Math.Max(p.y, q.y) && other.q.y >= Math.Min(p.y, q.y))
                {
                    return true;
                }

                return false; //(ccw1 * ccw2 <= EPSYLON && ccw3 * ccw4 <= EPSYLON);
            }
        }

        public static float ccw(Vec p, Vec q, Vec r)
        {
            Vec n = new Vec(p.y - q.y, q.x - p.x);
            float a = p.y * q.x - p.x * q.y;

            return n.x * r.x + n.y * r.y - a;
        }

        public override string ToString()
        {
            return String.Format("{0}->{1}", p, q);
        }
    }




    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Naiver Algorithmus");

            List<Line> lines = new List<Line>();

            StreamReader f = new StreamReader("s_1000_1.dat");
            //StreamReader f = new StreamReader("test.dat");

            int lineNr = 0;
            string txtLine;
            while ((txtLine = f.ReadLine()) != null)
            {
                txtLine = txtLine.Trim();
                string[] Vecs = txtLine.Split(' ');
                float ax = float.Parse(Vecs[0], CultureInfo.InvariantCulture);
                float ay = float.Parse(Vecs[1], CultureInfo.InvariantCulture);
                float bx = float.Parse(Vecs[2], CultureInfo.InvariantCulture);
                float by = float.Parse(Vecs[3], CultureInfo.InvariantCulture);
                lines.Add(new Line(++lineNr, new Vec(ax, ay), new Vec(bx, by)));
            }

            long maxIntersect = (long)Math.Pow(lines.Count, 2) / 2;
            Console.WriteLine(maxIntersect);

            int found = 0;
            int all = 0;
            for (int i = 0; i < lines.Count; i++)
                for (int j = i + 1; j < lines.Count; j++)
                {
                    all++;
                    if (all % 1000000 == 0)
                    {
                        Console.Write(String.Format("Progress: {0}%    ", Math.Round((double)all / maxIntersect * 100, 1)));
                        Console.SetCursorPosition(0, Console.CursorTop );
                    }
                    if (lines[i].intersect(lines[j]) && lines[j].intersect(lines[i]))
                    {
                        Console.WriteLine(String.Format("Intersection found between lines {0} and {1}", lines[i], lines[j]));
                        found++;
                    }
                }

            Console.WriteLine(all + " Lines intersected and " + found + " intersections found");
            Console.ReadKey();
        }
    }
}
