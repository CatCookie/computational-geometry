using System;
using System.Collections.Generic;

namespace CompGeo3
{
    internal class SweepLine<T> : List<Line>
    {


        private void Insert(StartPoint point)
        {
            List<float> yVal = CalculateNewYValues(point.x);

            var index = yVal.BinarySearch(point.y);
            if (index < 0) index = ~index;
            base.Insert(index, point.line);
        }

        private void Remove(EndPoint p)
        {
            base.Remove(p.line);
        }

        /// Calculate new Y-values for the sweep line 
        private List<float> CalculateNewYValues(float x)
        {
            List<float> newValues = new List<float>();

            foreach (Line l in this)
            {
                newValues.Add(l.getNewY(x));
            }

            return newValues;
        }

        /// Swap the the corresponding lines of an intersection point
        private void Swap(IntersectPoint p)
        {
            int indexA = this.IndexOf(p.line);
            int indexB = this.IndexOf(p.otherLine);

            this[indexA] = p.otherLine;
            this[indexB] = p.line;
        }

        /// Treating End Point
        ///  
        ///  Check for Intersection between upper and lower Line of endpoint
        ///  Then remove endpoint
        public List<IntersectPoint> TreatEndPoint(EndPoint p)
        {
            List<IntersectPoint> retVal = new List<IntersectPoint>();
            int index = IndexOf(p.line);

            if (index - 1 >= 0 && index + 1 < this.Count)
            {
                Line lowerLine = this[index - 1];
                Line upperLine = this[index + 1];
                Point intersect = upperLine.intersection(lowerLine);

                if (intersect != null)
                    retVal.Add( new IntersectPoint(intersect, upperLine, lowerLine));
            }

            this.Remove(p);
            return retVal;
        }

        /// Treat start point
        /// 
        /// Insert line into sweep line
        /// check for intersection with upper and lower line
        public List<IntersectPoint> TreatStartPoint(StartPoint p)
        {
            List<IntersectPoint> retVal = new List<IntersectPoint>();
            this.Insert(p);
            int index = IndexOf(p.line);

            if (index - 1 >= 0)
            {
                Line lowerLine = this[index - 1];
                Point intersect = p.line.intersection(lowerLine);

                if (intersect != null)
                    retVal.Add(new IntersectPoint(intersect, p.line, lowerLine));
            }

            if (index + 1 < this.Count)
            {
                Line upperLine = this[index + 1];
                Point intersect = p.line.intersection(upperLine);

                if (intersect != null)
                    retVal.Add(new IntersectPoint(intersect, upperLine, p.line));
            }
            
            return retVal;
        }

        /// Treat intersection point
        /// 
        /// Get lines of the intersection
        /// swap lines of the intersection
        /// check for intersection with upper and lower lines
        public List<IntersectPoint> TreatIntersectionPoint(IntersectPoint p)
        {
            List<IntersectPoint> retVal = new List<IntersectPoint>();

            // Get upper and lower intersection lines
            int indexLine = IndexOf(p.line);
            int indexOtherLine = IndexOf(p.otherLine);

            int upperIndex = Math.Max(indexLine, indexOtherLine);
            Line upperLine = this[upperIndex];
            int lowerIndex = Math.Min(indexLine, indexOtherLine);
            Line lowerLine = this[lowerIndex];

            // swap Lines
            this.Swap(p);

            // Check line below lower intersection line
            if (lowerIndex - 1 >= 0)
            {
                Line other = this[lowerIndex - 1];
                Point intersect = lowerLine.intersection(other);

                if (intersect != null)
                    retVal.Add(new IntersectPoint(intersect, lowerLine, other));
            }

            // Check line above upper intersection line
            if (upperIndex + 1 < this.Count)
            {
                Line other = this[upperIndex + 1];
                Point intersect = upperLine.intersection(other);

                if (intersect != null)
                    retVal.Add(new IntersectPoint(intersect, upperLine, other));
            }

            return retVal;
        }
    }
}