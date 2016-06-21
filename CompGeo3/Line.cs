using System;

namespace CompGeo3
{
    class Line
    {
        public const float EPSYLON = 1E-5f;

        // Line Number
        public int nr;
        public Point p, q;

        public Line(int nr, Point p, Point q)
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

        public Point intersection(Line other)
        {
            if (this.intersect(other))
            {
                float a1 = this.q.y - this.p.y;
                float b1 = this.p.x - this.q.x;
                float c1 = a1 * this.p.x + b1 * this.p.y;
                float a2 = other.q.y - other.p.y;
                float b2 = other.p.x - other.q.x;
                float c2 = a2 * other.p.x + b2 * other.p.y;
              
                float delta = a1 * b2 - a2 * b1;
                float x = (b2 * c1 - b1 * c2) / delta;
                float y = (a1 * c2 - a2 * c1) / delta;

                return new Point(x, y);
            }
            return null;           
        }

        public static float ccw(Point p, Point q, Point r)
        {
            Point n = new Point(p.y - q.y, q.x - p.x);
            float a = p.y * q.x - p.x * q.y;

            return n.x * r.x + n.y * r.y - a;
        }

        public override string ToString()
        {
            return String.Format("{0}->{1}", p, q);
        }
    }
}