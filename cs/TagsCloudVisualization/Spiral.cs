﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral : IEnumerable<Point>
    {
        private readonly Point center;
        private readonly HashSet<Point> usedPoints;

        public Spiral(Point center)
        {
            this.center = center;
            usedPoints = new HashSet<Point>();
        }

        public IEnumerator<Point> GetEnumerator()
        {
            var currentPoint = center;
            if (!usedPoints.Contains(currentPoint))
                yield return currentPoint;
            var lineLength = 1;
            var direction = -1;

            while (true)
            {
                for (var i = 0; i < lineLength * 2; i++)
                {
                    currentPoint = Point.Add(currentPoint, i < lineLength
                        ? new Size(direction, 0)
                        : new Size(0, direction));
                    if (!usedPoints.Contains(currentPoint))
                        yield return currentPoint;
                }

                direction *= -1;
                lineLength++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddUsedPoints(List<Point> points)
        {
            usedPoints.UnionWith(points);
        }
    }
}