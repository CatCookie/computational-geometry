using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CompGeo3
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Line Sweep Algorithmus");

            List<Line> lines = new List<Line>();

            StreamReader f = new StreamReader("s_100000_1.dat");
            //StreamReader f = new StreamReader("test.dat");

            int lineNr = 0;
            string txtLine;
            while ((txtLine = f.ReadLine()) != null)
            {
                txtLine = txtLine.Trim();
                if (!txtLine.StartsWith("#"))
                {
                    string[] Vecs = txtLine.Split(' ');
                    float ax = float.Parse(Vecs[0], CultureInfo.InvariantCulture);
                    float ay = float.Parse(Vecs[1], CultureInfo.InvariantCulture);
                    float bx = float.Parse(Vecs[2], CultureInfo.InvariantCulture);
                    float by = float.Parse(Vecs[3], CultureInfo.InvariantCulture);
                    lines.Add(new Line(++lineNr, new Point(ax, ay), new Point(bx, by)));
                }
            }

           
            LineSweep sweeper = new LineSweep(lines, LineSweepMethod.IGNORE_INVALID_DATA);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<Point> result = sweeper.Sweep();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            

            foreach (IntersectPoint p in result)
                Console.WriteLine(p + " ==== " + p.line + " with " + p.otherLine);

            Console.WriteLine(elapsedMs + " ms");


            Console.ReadKey();
        }
    }
}
