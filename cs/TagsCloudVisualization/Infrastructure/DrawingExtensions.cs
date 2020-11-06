using System;
using System.Drawing;

namespace TagsCloudVisualization.Infrastructure
{
    public static class DrawingExtensions
    {
        public static Point GetCenter(this Rectangle rectangle)
        {
            return new Point
            (
                rectangle.X + rectangle.Width / 2,
                rectangle.Y + rectangle.Height / 2
            );
        }

        public static Point Add(this Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y);
        }
        
        public static double DistanceFrom(this Point point, Point other)
        {
            return Math.Sqrt((point.X - other.X) * (point.X - other.X)
                             + (point.Y - other.Y) * (point.Y - other.Y));
        }
    }
}