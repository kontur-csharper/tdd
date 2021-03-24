using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Spiral Spiral;
        public readonly List<Rectangle> Rectangles;

        public CircularCloudLayouter(Point center)
        {
            Spiral = new Spiral(center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException();

            var rectangle = Rectangles.Count == 0
                ? new Rectangle(GetCoordinatesOfRectangle(rectangleSize), rectangleSize)
                : Rectangles.Last();
            while (RectangleIntersectAnyRectangles(rectangle))
            {
                var coordinates = GetCoordinatesOfRectangle(rectangleSize);
                rectangle = new Rectangle(new Point(coordinates.X, coordinates.Y), rectangleSize);
            }

            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Point GetCoordinatesOfRectangle(Size rectangleSize)
        {
            var pointOnSpiral = Spiral.GetNextPointOnSpiral();
            var x = pointOnSpiral.X;
            var y = pointOnSpiral.Y;
            if (y < Spiral.Center.Y)
                y -= rectangleSize.Height;
            if (x < Spiral.Center.X)
                x -= rectangleSize.Width;
            return new Point(x, y);
        }

        private bool RectangleIntersectAnyRectangles(Rectangle rectangle)
        {
            foreach (var anotherRectangle in Rectangles)
            {
                if (rectangle.IntersectsWith(anotherRectangle))
                    return true;
            }

            return false;
        }
    }
}