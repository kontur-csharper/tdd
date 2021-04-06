using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    class TagCloudVisualizer
    {
        private readonly Brush backgroundBrush;
        private readonly Brush shapeBrush;
        private readonly Pen shapeRectangle;
        private readonly IEnumerable<Rectangle> _rectangles;

        public TagCloudVisualizer(IEnumerable<Rectangle> rectangles)
        {
            _rectangles = rectangles;
            backgroundBrush = Brushes.AliceBlue;
            shapeBrush = Brushes.BlueViolet;
            shapeRectangle = new Pen(Color.Aqua);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeOfBitmap">The size the bitmap should have</param>
        /// <param name="directoryPath">Directory where bitmap should be saved. Default current dir.</param>
        /// <returns>The path where the bitmap was saved as png</returns>
        public string CreateBitmapFromRectangles(Size sizeOfBitmap, string directoryPath = @".")
        {
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException("The specified directory does not exist");

            var bitmap = DrawRectangles(new Bitmap(sizeOfBitmap.Width, sizeOfBitmap.Height));
            BitmapSaver.Save(bitmap, directoryPath);

            bitmap.Dispose();
            return Path.GetFullPath(directoryPath);
        }

        private Bitmap DrawRectangles(Bitmap bitmap)
        {
            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(backgroundBrush, new Rectangle(new Point(0, 0), bitmap.Size));
            foreach (var rect in _rectangles)
            {
                graphics.FillRectangle(shapeBrush, rect);
                graphics.DrawRectangle(shapeRectangle, rect);
            }

            graphics.Dispose();
            return bitmap;
        }
    }
}
