using System;
using System.Collections.Generic;


namespace CompGeo2
{
    struct City
    {
        public string name;
        public Point coordinates;
    }

    class Point
    {
        public float x { get; set; }
        public float y { get; set; }

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Point other, float epsilon)
        {
            if (Math.Abs(this.x - other.x) <= epsilon && Math.Abs(this.y - other.y) <= epsilon)
                return true;
            else
                return false;
        }
    }

    class Polygon
    {
        public List<Point> points { get; set; }
        private float pArea = 0.0f;

        public Polygon()
        {
            points = new List<Point>();
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
                Point pti = points[i];
                Point ptj = points[j];

                if (((points[i].y > y) != (points[j].y > y)) &&
                    (x < (points[j].x - points[i].x) * (y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                    c = !c;
            }
            return c;
        }

        public bool contains(Point point)
        {
            return contains(point.x, point.y);
        }

        // Checks for polygon in polygon
        // Only true if the second polygon is fully included.
        public bool contains(Polygon other)
        {
            foreach (Point v in other.points)
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

        private Polygon getMainLand()
        {
            Polygon main = new Polygon();
            foreach (Polygon p in parts)
            {
                if (p.area() > main.area())
                {
                    main = p;
                }
            }
            return main;
        }

        public float area()
        {
            if (cArea != 0)
                return cArea;

            foreach (Polygon poly in parts)
            {
                foreach (Polygon poly2 in parts)
                {
                    if (poly.contains(poly2))
                    {
                        this.cArea -= poly2.area() * 2;
                        //Console.WriteLine(-poly.area());
                    }
                }
                this.cArea += poly.area();
            }

            return this.cArea * scaling;
        }

        public bool contains(City city)
        {
            int count = 0;
            foreach (Polygon p in parts)
            {
                if (p.contains(city.coordinates))
                {
                    if (++count > 1)
                        return false;
                }
            }
            return (count == 1);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================");
            Console.WriteLine("|| Deutschland Fläche           ||");
            Console.WriteLine("==================================");

            SVGReader r = new SVGReader("DeutschlandMitStaedten.svg");
            List<Country> countries = r.getCountries();
            List<City> cities = r.getCities();

            float sum = 0;

            Console.WriteLine();
            foreach (Country ctry in countries)
            {
                foreach (City city in cities)
                {
                    if (ctry.contains(city))
                    {
                        sum += ctry.area();
                        Console.WriteLine("{0,-23}: {1,7} km² : {2}", ctry.name, ctry.area(), city.name);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("{0,-23}: {1,6} km²", "Deutschland", sum);
            Console.WriteLine("\r\n--------------\r\n");
            Console.WriteLine("Ende");

            Console.ReadKey();

        }
    }
}
