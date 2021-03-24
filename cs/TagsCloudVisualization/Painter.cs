using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class Painter
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public Painter(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
        }

        public void PaintRectangle(Rectangle rectangle)
        {
            var rand = new Random(Environment.TickCount);
            if (rectangle.Left >= 0 && rectangle.Top >= 0 && rectangle.Right <= bitmap.Width &&
                rectangle.Bottom <= bitmap.Height)
                graphics.DrawRectangle(
                    new Pen(Color.FromArgb(rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255)), 2),
                    rectangle);
            graphics.Save();
        }

        public void PaintCircle(Point center, double radius)
        {
            graphics.DrawEllipse(new Pen(Color.Red, 2),
                new Rectangle(new Point((int) (center.X - radius), (int) (center.Y - radius)),
                    new Size(2 * (int) radius, 2 * (int) radius)));
            graphics.Save();
        }

        public void SavePicture(string fileName)
        {
            bitmap.Save(fileName, ImageFormat.Jpeg);
        }
    }
}