using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public static class TagsCloudVisualization
    {
        public static void Visualizate(IEnumerable<Rectangle> rectangles, Point center, Size size, string path)
        {
            var random = new Random();
            var bitmap = new Bitmap(2 * size.Width, 2 * size.Height);
            var vectorShift = new Point(size.Width - center.X, size.Height - center.Y);
            var graphics = Graphics.FromImage(bitmap);
            foreach(var rectangle in rectangles)
            {
                var randomColor = Color.FromArgb(255, random.Next(255), random.Next(255), random.Next(255));
                var pen = new Pen(randomColor);
                var brush = new SolidBrush(randomColor);
                graphics.DrawRectangle(pen, ShiftRectangle(rectangle));
                graphics.FillRectangle(brush, ShiftRectangle(rectangle));
            }
            bitmap.Save(path);
            
            Rectangle ShiftRectangle(Rectangle r) => 
                new Rectangle(r.X + vectorShift.X, r.Y + vectorShift.Y, r.Width, r.Height);
        }
    }
}
