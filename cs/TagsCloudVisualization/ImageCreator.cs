using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class ImageCreator
    {
        public static void CreateImageFromRectangles(IEnumerable<Rectangle> rectangles, Point layouterCenter, string path, string fileName)
        {
            rectangles = rectangles.ToList();
            if (!rectangles.Any())
                return;
            var bm = new Bitmap(1080, 1080);
            var graphics = Graphics.FromImage(bm);
            graphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0, 0, 1080, 1080));
            graphics.DrawRectangles(new Pen(Color.Red), rectangles.ToArray());
            graphics.DrawEllipse(new Pen(Color.GreenYellow),
                new Rectangle(new Point(layouterCenter.X - 5, layouterCenter.Y - 5), new Size(10, 10)));

            bm.Save($"{path}\\{fileName}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.bmp", ImageFormat.Bmp);
        }
    }
}
