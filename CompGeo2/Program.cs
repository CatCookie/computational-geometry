using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CompGeo2
{

    class Vec
    {
        public float x { get; set; }
        public float y { get; set; }

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
    }

    class Polygon
    {
        public List<Vec> points { get; set; }
        private float cArea = 0.0f;

        public Polygon()
        {
            points = new List<Vec>();
        }

        public float area()
        {
            cArea = 0.0f;
            for (int i = 1; i <= points.Count; i++)
            {
                float y = points[i % points.Count].y;
                float x1 = points[(i - 1) % points.Count].x;
                float x2 = points[(i + 1) % points.Count].x;

                cArea += (y * (x1 - x2)) / 2;

            }
            return cArea;
        }
    }


    class Country
    {
        public List<Polygon> parts { get; set; }
        public string name;
        private float cArea = 0.0f;
        //private float scaling = 1.173423676f;
        private float scaling = 1f;

        public Country(string name)
        {
            parts = new List<Polygon>();
            this.name = name;
        }

        public float area()
        {
            cArea = 0.0f;
            foreach (Polygon part in parts)
            {
                this.cArea += part.area();
            }
            return this.cArea * scaling;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Deutschland Fläche");
            SVGReader r = new SVGReader("DeutschlandMitStaedten.svg");
            //SVGReader r = new SVGReader("test.svg");
            List<Country> ctry = r.parse();
            foreach (Country c in ctry)
            {
                c.area();
            }

            foreach (Country c in ctry)
            {
                Console.WriteLine(String.Format("Fläche von {0}: {1}km²", c.name, c.area()));
                //Console.WriteLine( c.area());
            }

            Console.WriteLine("Ende");
            Console.ReadKey();

        }
    }
}
