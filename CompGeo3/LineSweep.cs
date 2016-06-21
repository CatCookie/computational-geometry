using System;
using System.Collections.Generic;

namespace CompGeo3
{
    enum LineSweepMethod
    {
        IGNORE_INVALID_DATA,
        EXCEPT_ON_INVALID_DATA,
        NULL_ON_INVALID_DATA,
        STOP_ON_INVALID_DATA
    }

    internal class InvalidDataException : ArgumentException
    {
        public InvalidDataException(List<Line> invalidLines)
            : base("Invalid Lines Found: \r\n" + String.Join("\r\n", invalidLines))
        {
        }
        public InvalidDataException(Point invalidPoint)
            : base("Invalid Intersection Found: " + invalidPoint)
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
        private List<Point> result = new List<Point>();

        public LineSweep(List<Line> lines, LineSweepMethod option)
        {
            this.lines = lines;
            this.option = option;

            BuildDataStructures();

        }

        /// Create the Datastructures to work with 
        private void BuildDataStructures()
        {
            foreach (Line l in lines)
            {
                Point p = l.p;
                Point q = l.q;

                // Add points to event list if they are disjunct
                if (p.x == q.x || events.ContainsKey(p.x) || events.ContainsKey(q.x))
                {
                    invalidLines.Add(l);
                }
                else
                {
                    Point start = p.x > q.x ? q : p;
                    Point end = p.x > q.x ? p : q;
                    events.Add(start.x, new StartPoint(start, l));
                    events.Add(end.x, new EndPoint(end, l));
                }
            }

            if (option == LineSweepMethod.EXCEPT_ON_INVALID_DATA && invalidLines.Count > 0)
                throw new InvalidDataException(invalidLines);
        }


        /// Sweeps through all events 
        /// This is the core of the algorithm 
        internal List<Point> Sweep()
        {
            if (option == LineSweepMethod.NULL_ON_INVALID_DATA && invalidLines.Count > 0)
                return null;
            else if (option == LineSweepMethod.STOP_ON_INVALID_DATA && invalidLines.Count > 0)
                return result;
            if (option == LineSweepMethod.EXCEPT_ON_INVALID_DATA && invalidLines.Count > 0)
                throw new InvalidDataException(invalidLines);

            int index = 0;
            while (events.Count > 0)
            {
                Point p = events.Values[0];

                List<IntersectPoint> list = new List<IntersectPoint>();
                if (p is EndPoint)
                {
                    // Treat end point
                    list = sweepLine.TreatEndPoint((EndPoint)p);
                }
                else if (p is IntersectPoint)
                {
                    // Treat intersection point
                    result.Add(p);
                    list = sweepLine.TreatIntersectionPoint((IntersectPoint)p);
                }
                else
                {
                    // Treat start point
                    list = sweepLine.TreatStartPoint((StartPoint)p);
                }

                // Add found intersection points to the events
                foreach (IntersectPoint iP in list)
                {
                    if (!AddIntersectionToEvents(iP))
                    {
                        if (option == LineSweepMethod.NULL_ON_INVALID_DATA)
                            return null;
                        else if (option == LineSweepMethod.STOP_ON_INVALID_DATA)
                            return result;
                    }
                }

                events.Remove(p.x);
                index++;
            }

            return result;
        }

        /// Extra treatment for intersection points with nondisjunct x coordinates in the eventlist
        private bool AddIntersectionToEvents(IntersectPoint iP)
        {
            if (!events.ContainsKey(iP.x))
            {
                events.Add(iP.x, iP);
                return true;
            }
            else
            {
                switch (option)
                {
                    case LineSweepMethod.EXCEPT_ON_INVALID_DATA:
                        throw new InvalidDataException(iP);

                    case LineSweepMethod.IGNORE_INVALID_DATA:
                        Point invalid = events[iP.x];
                        if (invalid is IntersectPoint)
                        {
                            IntersectPoint invalidIntersection = (IntersectPoint)invalid;
                            events.Remove(invalidIntersection.line.p.x);
                            events.Remove(invalidIntersection.line.q.x);
                            events.Remove(invalidIntersection.otherLine.p.x);
                            events.Remove(invalidIntersection.otherLine.q.x);
                            events.Remove(invalidIntersection.x);
                            events.Add(iP.x, iP);
                            return true;
                        }
                        else
                        {
                            StartPoint invalidPoint = (StartPoint)invalid;
                            events.Remove(invalidPoint.line.p.x);
                            events.Remove(invalidPoint.line.q.x);
                            events.Add(iP.x, iP);
                            return true;
                        }

                    default:
                        return false;
                }
            }
        }
    }
}