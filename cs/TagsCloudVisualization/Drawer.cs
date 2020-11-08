using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Drawer
    {
        public static Bitmap DrawImage(List<Rectangle> rectangles, Point center)
        {
            CheckParameters(rectangles, center);

            var image = new Bitmap(center.X + GetDeltaX(rectangles), center.Y + GetDeltaY(rectangles));
            using var graphics = Graphics.FromImage(image);

            graphics.DrawRectangles(new Pen(Color.Red), rectangles.ToArray());

            return image;
        }

        private static void CheckParameters(List<Rectangle> rectangles, Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("X or Y of center was negative");

            if (!rectangles.Any())
                throw new ArgumentException("The sequence contains no elements");
        }

        private static int GetDeltaX(List<Rectangle> rectangles)
        {
            return rectangles.Select(elem => elem.Right).Max();
        }

        private static int GetDeltaY(List<Rectangle> rectangles)
        {
            return rectangles.Select(elem => elem.Bottom).Max();
        }
    }
}