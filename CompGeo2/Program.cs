using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeo2
{

    class Vec
    {
        public double x, y;

        public Vec(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Vec other, double epsilon)
        {
            if (Math.Abs(this.x - other.x) <= epsilon && Math.Abs(this.y - other.y) <= epsilon)
                return true;
            else
                return false;
        }


    }

    class Line
    {
        public int nr;
        public Vec p, q;
        public Vec n;
        public double a;

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
            double ccw1 = ccw(this, other.p);
            double ccw2 = ccw(this, other.q);
            double ccw3 = ccw(other, this.p);
            double ccw4 = ccw(other, this.q);

            if (Math.Abs(ccw1 - ccw2) <= 0.000001f)
            {
                if (Math.Abs(this.p.x - this.q.x) <= 0.000001f)
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

        public static double ccw(Line line, Vec r)
        {
            return line.n.x * r.x + line.n.y * r.y - line.a;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {






            Console.WriteLine("Ende");
            Console.ReadKey();



        }
    }
}
