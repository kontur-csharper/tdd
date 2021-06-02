using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private Spiral spiral;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentException("Size should have positive arguments");
            
            if (spiral == null)
            {
                spiral = new Spiral(size);
                return new Rectangle(center, size);
            }

            return spiral.PutNextRectangle(size).Shifted(center);
        }

        private class Spiral
        {
            private Directions currentDirection = Directions.Up;
            private Rectangle borders;
            private Rectangle previousBorders;
            
            private readonly List<Rectangle>[] lines = new List<Rectangle>[4];
            private List<Rectangle> currentLine;

            public Spiral(Size firstRectangleSize)
            {
                borders = new Rectangle(Point.Empty, firstRectangleSize);
                currentLine = new List<Rectangle> {borders};
            }

            public Rectangle PutNextRectangle(Size size)
            {
                var toCenterDirection = currentDirection.CounterClockwise();
                var fromCenterDirection = currentDirection.Clockwise();
                var lastRectangle = currentLine[currentLine.Count - 1];
                
                var currentRectangle = new Rectangle(lastRectangle.Location, size);

                currentRectangle = currentRectangle
                    .ShiftedToRectangleBorder(currentDirection, lastRectangle)
                    .ShiftedToRectangleBorder(fromCenterDirection, previousBorders);

                var previousLine = lines[(int) currentDirection];
                var intersectedRectangles = previousLine
                    ?.Where(r => r.IntersectInDirection(currentRectangle, currentDirection)).ToArray()
                    ?? Array.Empty<Rectangle>();
                var max = intersectedRectangles.Length > 0
                    ? intersectedRectangles.MaxBorderInDirection(fromCenterDirection)
                    : previousBorders.BorderInDirection(fromCenterDirection);
                currentRectangle = currentRectangle.ShiftedToBorderInDirection(toCenterDirection, max);

                intersectedRectangles = lines[(int) fromCenterDirection]
                    ?.Where(r => r.IntersectsWith(currentRectangle)).ToArray()
                    ?? Array.Empty<Rectangle>();
                max = intersectedRectangles.Length > 0
                    ? intersectedRectangles.MaxBorderInDirection(fromCenterDirection)
                    : max;
                currentRectangle = currentRectangle.ShiftedToBorderInDirection(toCenterDirection, max);
                
                currentLine.Add(currentRectangle);
                
                var currentRectangleBorder = currentRectangle.BorderInDirection(fromCenterDirection);
                var border = borders.BorderInDirection(fromCenterDirection);
                if(fromCenterDirection.CompareInDirection(currentRectangleBorder, border) > 0)
                    borders = borders.ResizedToBorderInDirection(fromCenterDirection, currentRectangleBorder);

                if (currentDirection.CompareInDirection(
                    currentRectangle.BorderInDirection(currentDirection),
                    previousBorders.BorderInDirection(currentDirection)) > 0)
                    Turn();

                return currentRectangle;
            }

            private void Turn()
            {
                lines[(int) currentDirection] = currentLine;

                var lastRectangle = currentLine[currentLine.Count - 1];
                currentLine = new List<Rectangle> {lastRectangle};
                
                previousBorders = borders;
                
                var lastRectangleBorder = lastRectangle.BorderInDirection(currentDirection);
                var border = borders.BorderInDirection(currentDirection);
                if (currentDirection.CompareInDirection(lastRectangleBorder, border) > 0)
                    borders = borders.ResizedToBorderInDirection(currentDirection, lastRectangleBorder);

                currentDirection = currentDirection.CounterClockwise();
            }
        }
    }
}