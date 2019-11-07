using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point cloudCenter;
        private List<Rectangle> rectangles;
        private const double turnFactor = 1;
        private const double distanceFactor = 5;
        private const double step = 0.1;
        private double angle = 0;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Invalid center");
            cloudCenter = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Invalid size");
            var center = cloudCenter;
            var corner = GetRectangleCorner(center, rectangleSize);
            var nextRectangle = new Rectangle(corner, rectangleSize);
            while (IsIntersectingOrOutOfBorders(nextRectangle))
            {
                center = GetNextRectangleCenter();
                corner = GetRectangleCorner(center, rectangleSize);
                nextRectangle.Location = corner;
            }
            rectangles.Add(nextRectangle);
            return new Rectangle(nextRectangle.Location, rectangleSize);
        }

        private bool IsIntersectingOrOutOfBorders(Rectangle nextRectangle) =>
            nextRectangle.X < 0 || nextRectangle.Y < 0 ||
            rectangles.Any(rectangle => rectangle.IntersectsWith(nextRectangle));

        private Point GetRectangleCorner(Point center, Size size) =>
            new Point(center.X - size.Width / 2, center.Y - size.Height / 2);

        private Point GetNextRectangleCenter()
        {
            var t = Math.PI * angle;
            var x = (turnFactor + distanceFactor * t) * Math.Cos(t);
            var y = (turnFactor + distanceFactor * t) * Math.Sin(t);
            angle += step;
            return new Point(cloudCenter.X + (int)x, cloudCenter.Y + (int)y);
        }


    }
}
