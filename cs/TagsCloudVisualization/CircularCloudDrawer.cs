using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudDrawer
    {
        private static readonly Random random = new Random();

        public static Bitmap GetBitmap(List<Rectangle> rectangles, Point center)
        {
            var bitmapSize = GetBitmapSize(rectangles, center);
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var bitmapCenter = new Point(bitmapSize.Width / 2, bitmapSize.Height / 2);
            var offset = new Point(bitmapCenter.X - center.X, bitmapCenter.Y - center.Y);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangleToDraw in rectangles.Select(
                rectangle => new Rectangle(rectangle.Location, rectangle.Size)))
            {
                rectangleToDraw.Offset(offset);
                var p = new Pen(
                    Color.FromArgb(
                        random.Next(256), random.Next(256), random.Next(256)
                    )
                );
                graphics.DrawRectangle(p, rectangleToDraw);
            }

            return bitmap;
        }

        private static Size GetBitmapSize(IEnumerable<Rectangle> rectangles, Point center)
        {
            var left = int.MaxValue;
            var right = int.MinValue;
            var top = int.MaxValue;
            var bottom = int.MinValue;
            foreach (var rectangle in rectangles)
            {
                if (rectangle.Left < left)
                    left = rectangle.Left;
                if (rectangle.Right > right)
                    right = rectangle.Right;
                if (rectangle.Top < top)
                    top = rectangle.Top;
                if (rectangle.Bottom > bottom)
                    bottom = rectangle.Bottom;
            }

            var size = new Size
            {
                Width = Math.Max(center.X - left, right - center.X) * 2 + 10,
                Height = Math.Max(center.Y - top, bottom - center.Y) * 2 + 10
            };
            return size;
        }
    }
}