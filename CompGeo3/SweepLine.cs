using System;
using System.Collections.Generic;

namespace CompGeo3
{
    internal class SweepLine<T>: List<Line>
    {

        public void Insert(StartPoint point)
        {
            CalculateNewYValues();

            var index = base.BinarySearch(point.line);
            if (index < 0) index = ~index;
            base.Insert(index, point.line);
        }

        private void CalculateNewYValues()
        {
            throw new NotImplementedException();
        }
    }
}