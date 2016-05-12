using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CompGeo2
{
    struct City
    {
        public string name;
        public Vec coordinates;
    }

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
        private float pArea = 0.0f;

        public Polygon()
        {
            points = new List<Vec>();
        }

        public float area()
        {
            pArea = 0.0f;
            for (int i = 1; i <= points.Count; i++)
            {
                float y = points[i % points.Count].y;
                float x1 = points[(i - 1) % points.Count].x;
                float x2 = points[(i + 1) % points.Count].x;

                pArea += (y * (x1 - x2)) / 2;

            }
            //Console.WriteLine(pArea);
            return Math.Abs(pArea);
        }


        public bool contains(float x, float y)
        {
            int nrOfPointsInPoly = points.Count;
            bool c = false;
            int i, j = 0;
            for (i = 0, j = nrOfPointsInPoly - 1; i < nrOfPointsInPoly; j = i++)
            {
                Vec pti = points[i];
                Vec ptj = points[j];

                if (((points[i].y > y) != (points[j].y > y)) &&
                    (x < (points[j].x - points[i].x) * (y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                    c = !c;
            }
            return c;
        }

        public bool contains(Vec point)
        {
            return contains(point.x, point.y);
        }

        // Checks for polygon in polygon
        // Only true if the second polygon is fully included.
        public bool contains(Polygon other)
        {
            foreach (Vec v in other.points)
            {
                if (!this.contains(v))
                {
                    return false;
                }
            }
            return true;
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
            Polygon main = new Polygon();
            foreach (Polygon p in parts)
            {
                if (p.area() > main.area())
                {
                    main = p;
                }
            }
            cArea = main.area();
            Console.WriteLine(main.area());
            foreach (Polygon poly in parts)
            {
                if (poly != main)
                    if (main.contains(poly))
                    {
                        this.cArea -= poly.area();
                        Console.WriteLine(-poly.area());
                    }
                    else
                    {
                        this.cArea += poly.area();
                        Console.WriteLine(poly.area());
                    }
            }
            return this.cArea * scaling;
        }

        public bool contains(City city)
        {
            foreach (Polygon p in parts)
            {
                if (p.area() > 0 && p.contains(city.coordinates))
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Deutschland Fläche");
            SVGReader r = new SVGReader("DeutschlandMitStaedten.svg");
            //SVGReader r = new SVGReader("test.svg");

            List<Country> countries = r.getCountries();
            List<City> cities = r.getCities();

            //foreach (Country ctry in countries)
            //{
            //    foreach (City city in cities)
            //    {
            //        if (ctry.contains(city))
            //            Console.WriteLine(String.Format("{0} liegt in {1}", city.name, ctry.name));
            //    }
            //}

            foreach (Country c in countries)
            {
                //Console.WriteLine(String.Format("{0} {1}", c.name, c.area()));
                Console.WriteLine(String.Format("Fläche von {0}: {1}km²", c.name, c.area()));
            }


            //Polygon p = new Polygon();
            //p.points.Add(new Vec(0, 0));
            //p.points.Add(new Vec(10, 0));
            //p.points.Add(new Vec(10, 10));
            //p.points.Add(new Vec(0, 10));

            //Vec v = new Vec(10, 10);

            //Console.WriteLine("\r\n--------------\r\n");
            //Console.WriteLine(p.area());
            //Console.WriteLine(p.contains(10, 10));
            //Console.WriteLine(p.contains(new Vec(5, 5)));
            //Console.WriteLine(p.contains(new Vec(5, 10)));
            //Console.WriteLine(p.contains(new Vec(10, 5)));
            //Console.WriteLine(p.contains(new Vec(0, 5)));
            //Console.WriteLine(p.contains(new Vec(5, 0)));

            //Console.WriteLine("\r\n--------------\r\n");
            //Random rnd = new Random();
            //for (int i = 0; i < 20000; i++)
            //{
            //    double x = rnd.NextDouble() * 10;
            //    double y = rnd.NextDouble() * 10;
            //    if (!p.contains(x, y))
            //        Console.WriteLine(String.Format("({0}|{1})", rnd.NextDouble() * 10, rnd.NextDouble() * 10));

            //}

            Console.WriteLine("\r\n--------------\r\n");
            Console.WriteLine("Ende");
            Console.ReadKey();

        }
    }
}
