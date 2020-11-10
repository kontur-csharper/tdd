using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        public readonly List<Rectangle> Rectangles;
        private readonly List<Direction> directions;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            directions = new List<Direction> {Direction.Top, Direction.Right, Direction.Bottom, Direction.Left};
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Rectangle size should be positive.");
            Rectangle rectangle;
            if (Rectangles.Count == 0)
            {
                rectangle = new Rectangle
                {
                    Size = rectangleSize,
                    Location = new Point(Center.X - rectangleSize.Width / 2, Center.Y - rectangleSize.Height / 2)
                };
            }
            else
                rectangle = GetRectangle(rectangleSize);

            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetRectangle(Size rectangleSize)
        {
            var bestDistance = double.MaxValue;
            var bestRectangle = new Rectangle();

            foreach (var rectangle in Rectangles)
            {
                foreach (var direction in directions)
                {
                    var newRectangle = new Rectangle
                    {
                        Size = rectangleSize,
                        Location = CalculateRectangleLocation(rectangle, rectangleSize, direction)
                    };
                    var distance = GetDistanceToCenter(newRectangle);

                    if (distance < bestDistance && !IntersectsWithOtherRectangles(newRectangle))
                    {
                        bestDistance = distance;
                        bestRectangle = newRectangle;
                    }
                }
            }

            return bestRectangle;
        }

        private double GetDistanceToCenter(Rectangle rectangle)
        {
            var rectangleCenter = new Point(rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2);
            return LayouterTools.CalculateDistance(rectangleCenter, Center);
        }

        private bool IntersectsWithOtherRectangles(Rectangle rectangle)
        {
            return Rectangles.Any(previous => previous.IntersectsWith(rectangle));
        }

        private static Point GetOffset(Size rectangleSize, Rectangle previous, Direction direction)
        {
            return direction switch
            {
                Direction.Top => new Point(0, previous.Height),
                Direction.Right => new Point(previous.Width, previous.Height - rectangleSize.Height),
                Direction.Bottom => new Point(previous.Width - rectangleSize.Width, -rectangleSize.Height),
                Direction.Left => new Point(-rectangleSize.Width, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static Point CalculateRectangleLocation(Rectangle previous, Size rectangleSize, Direction direction)
        {
            var offset = GetOffset(rectangleSize, previous, direction);
            return new Point(previous.X + offset.X, previous.Y + offset.Y);
        }
    }
}