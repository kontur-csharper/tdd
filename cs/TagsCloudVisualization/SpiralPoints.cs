using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralPoints
    {
        private const int PointsInCircleCount = 360;

        private readonly Point center;
        private readonly HashSet<Point> generatedPoints;
        private int radius = 1;

        public SpiralPoints(Point center)
        {
            this.center = center;
            generatedPoints = new HashSet<Point>{center};
        }
        
        public IEnumerable<Point> GetSpiralPoints()
        {
            foreach (var point in generatedPoints)
                yield return point;
            
            while (true)
            {
                var circlePoints = GetCirclePoints();
                foreach (var point in circlePoints)
                    generatedPoints.Add(point);
                radius++;

                foreach (var point in circlePoints)
                    yield return point;
            }
        }

        private HashSet<Point> GetCirclePoints()
        {
            var circlePoints = new HashSet<Point>();
            
            for (var i = 0; i < PointsInCircleCount; ++i)
            {
                var x = (int)(Math.Cos(2 * Math.PI * i / PointsInCircleCount) * radius + 0.5) + center.X;
                var y = (int)(Math.Sin(2 * Math.PI * i / PointsInCircleCount) * radius + 0.5) + center.Y;

                var point = new Point(x, y);
                circlePoints.Add(point);
            }

            return circlePoints;
        }
    }
}