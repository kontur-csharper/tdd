using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }
        private const int RollBackPixelsCount = 10; // Оптимальное значение величины отката радиуса спирали в пикселях
        private readonly Spiral _spiral;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException(
                    $"Invalid coordinates of center = ({center.X}, {center.Y})", nameof(center));
            Center = center;
            Rectangles = new List<Rectangle>();
            _spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException(
                    $"Invalid rectangleSize = ({rectangleSize.Width}, {rectangleSize.Height})", nameof(rectangleSize));
            _spiral.RollBackRadius(RollBackPixelsCount);
            while (true)
            {
                var currentCoordinate = _spiral.GetNextPosition(rectangleSize);
                var currentRectangle = new Rectangle(currentCoordinate, rectangleSize);

                if (!CheckIntersections(currentRectangle))
                {
                    ShiftRectangleToCenter(ref currentRectangle);
                    Rectangles.Add(currentRectangle);
                    return currentRectangle;
                }
            }
        }

        private void ShiftRectangleToCenter(ref Rectangle rectangle, int step = 1)
        {
            var previousPosition = rectangle.Location;
            while (true)
            {
                ShiftHorizontal(ref rectangle, previousPosition.X, step);
                ShiftVertical(ref rectangle, previousPosition.Y, step);
                if (previousPosition == rectangle.Location)
                    break;
                previousPosition = rectangle.Location;
            }
        }

        private void ShiftVertical(ref Rectangle rectangle, int availablePositionY, int step)
        {
            var relativeCoordinateY = availablePositionY - Center.Y + rectangle.Height / 2;
            if (Math.Abs(relativeCoordinateY) > 1)
            {
                step = relativeCoordinateY >= 0 ? -step : step;
                var checkRectangle = new Rectangle(new Point(rectangle.X, rectangle.Y + step), rectangle.Size);
                if (!CheckIntersections(checkRectangle))
                    rectangle.Offset(0, step);
            }
        }

        private void ShiftHorizontal(ref Rectangle rectangle, int availablePositionX, int step)
        {
            var relativeCoordinateX = availablePositionX - Center.X + rectangle.Width / 2;
            if (Math.Abs(relativeCoordinateX) > 1)
            {
                step = relativeCoordinateX >= 0 ? -step : step;
                var checkRectangle = new Rectangle(new Point(rectangle.X + step, rectangle.Y), rectangle.Size);
                if (!CheckIntersections(checkRectangle))
                    rectangle.Offset(step, 0);
            }
        }

        private bool CheckIntersections(Rectangle rectangle)
        { 
            foreach (var otherRectangle in Rectangles)
                if (rectangle.IntersectsWith(otherRectangle))
                    return true; 

            return false;
        }
    }
}
