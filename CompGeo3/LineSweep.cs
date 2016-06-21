using System;
using System.Collections.Generic;

namespace CompGeo3
{
    enum LineSweepMethod
    {
        IGNORE_INVALID_DATA,
        EXCEPT_ON_INVALID_DATA,
        NULL_ON_INVALID_DATA,
        EMPTY_ON_INVALID_DATA
    }

    internal class InvalidDataException : ArgumentException
    {
        public InvalidDataException(List<Line> invalidLines)
            : base("Invalid Lines Found: \r\n" + String.Join("\r\n", invalidLines))
        {
        }
    }


    internal class LineSweep
    {
        private List<Line> invalidLines = new List<Line>();

        private List<Line> lines;
        private LineSweepMethod option = LineSweepMethod.IGNORE_INVALID_DATA;
        private SortedList<float, Point> events = new SortedList<float, Point>();

        private SweepLine<Line> sweepLine = new SweepLine<Line>();


        public LineSweep(List<Line> lines, LineSweepMethod option)
        {
            this.lines = lines;
            this.option = option;

            BuildDataStructures();

        }

        private void BuildDataStructures()
        {
            foreach (Line l in lines)
            {
                Point p = l.p;
                Point q = l.q;

                // Add points to event list
                if (events.ContainsKey(p.x) || events.ContainsKey(q.x))
                {
                    invalidLines.Add(l);
                }
                else
                {
                    events.Add(l.p.x, new StartPoint(p, l));
                    events.Add(l.q.x, new EndPoint(q, l));
                }
            }

            if (option == LineSweepMethod.EXCEPT_ON_INVALID_DATA && invalidLines.Count > 0)
                throw new InvalidDataException(invalidLines);
        }



        internal List<Point> Sweep()
        {
            if (option == LineSweepMethod.NULL_ON_INVALID_DATA && invalidLines.Count > 0)
                return null;
            else if (option == LineSweepMethod.EMPTY_ON_INVALID_DATA && invalidLines.Count > 0)
                return new List<Point>();
            if (option == LineSweepMethod.EXCEPT_ON_INVALID_DATA && invalidLines.Count > 0)
                throw new InvalidDataException(invalidLines);


            foreach (Point p in events.Values)
            {
                Console.WriteLine(p);
                // Sweep here
            }


            return null;
        }



    }
}