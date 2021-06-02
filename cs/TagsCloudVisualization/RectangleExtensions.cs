using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Rectangle Shifted(this Rectangle rectangle, Size offset) =>
            new Rectangle(rectangle.Location + offset, rectangle.Size);

        public static Rectangle Shifted(this Rectangle rectangle, Point offset) => rectangle.Shifted(new Size(offset));

        public static int BorderInDirection(this Rectangle rectangle, Directions direction) => direction switch
        {
            Directions.Up => rectangle.Top,
            Directions.Left => rectangle.Left,
            Directions.Down => rectangle.Bottom,
            Directions.Right => rectangle.Right,
            _ => throw new InvalidOperationException("Unknown direction")
        };

        public static int MaxBorderInDirection(this IEnumerable<Rectangle> rectangles, Directions direction) =>
            rectangles.Max(r => r.BorderInDirection(direction) * direction.IsPositiveModifier())
            * direction.IsPositiveModifier();
        
        public static Rectangle ResizedToBorderInDirection(this Rectangle rectangle, Directions direction, int value)
        {
            switch (direction)
            {
                case Directions.Up:
                    var offset = direction.GetOffset(value - rectangle.Top);
                    rectangle.Location -= offset;
                    rectangle.Size += offset;
                    return rectangle;
                case Directions.Left:
                    offset = direction.GetOffset(value - rectangle.Left);
                    rectangle.Location -= offset;
                    rectangle.Size += offset;
                    return rectangle;
                case Directions.Down:
                    rectangle.Size += direction.GetOffset(value - rectangle.Bottom);
                    return rectangle;
                case Directions.Right:
                    rectangle.Size += direction.GetOffset(value - rectangle.Right);
                    return rectangle;
                default: throw new InvalidOperationException("Unknown direction");
            }
        }
        
        public static Rectangle ShiftedToBorderInDirection(this Rectangle rectangle, Directions direction, int value)
        {
            rectangle.Location += direction.GetOffset((value - rectangle.BorderInDirection(direction))
                                                      * direction.IsPositiveModifier());
            return rectangle;
        }

        public static Rectangle ShiftedToRectangleBorder(this Rectangle rectangle, Directions direction,
            Rectangle otherRectangle) =>
            rectangle.ShiftedToBorderInDirection(direction.Opposite(), otherRectangle.BorderInDirection(direction));

        public static bool IntersectInDirection(this Rectangle r1, Rectangle r2, Directions direction)
        {
            if (direction == Directions.Up || direction == Directions.Down)
            {
                r1.Location = new Point(0, r1.Location.Y);
                r2.Location = new Point(0, r2.Location.Y);
            }
            else
            {
                r1.Location = new Point(r1.Location.X, 0);
                r2.Location = new Point(r2.Location.X, 0);
            }

            return r1.IntersectsWith(r2);
        }
    }
}