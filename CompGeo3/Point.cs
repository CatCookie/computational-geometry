using System;

namespace CompGeo3
{
    class Point : IComparable
    {
        public float x, y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public int CompareTo(object other)
        {
            if (other == null) return 1;
            Point otherPoint = other as Point;
            return this.y.CompareTo(otherPoint.y);
        }

        public bool Equals(Point other, float epsilon)
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

    class StartPoint : Point
    {
        public Line line;
        public StartPoint(Point p, Line line) : base(p.x, p.y) { this.line = line; }
        public StartPoint(float x, float y, Line line) : base(x, y) { this.line = line; }
        public override string ToString()
        {
            return String.Format("{0} {1}", this.GetType().Name, base.ToString());
        }
    }

    class EndPoint : StartPoint
    {
        public EndPoint(Point p, Line line) : base(p, line) { }
    }

    class IntersectPoint : StartPoint
    {
        public Line otherLine;
        public IntersectPoint(float x, float y, Line line, Line otherLine) : base(x, y, line) { this.otherLine = line; }
    }

}