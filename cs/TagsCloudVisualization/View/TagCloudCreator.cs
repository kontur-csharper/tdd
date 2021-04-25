using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualization.Models;

namespace TagsCloudVisualization.View
{
    class TagCloudCreator : IDisposable
    {
        private readonly CircularCloudLayouter layouter;
        private readonly Brush layoutBrush;
        private readonly Pen rectanglePen;

        public TagCloudCreator(CircularCloudLayouter layouter)
        {
            this.layouter = layouter;
            this.layoutBrush = new SolidBrush(Color.AliceBlue);
            this.rectanglePen = new Pen(Color.DodgerBlue, 2);
        }

        public void Save(string filename)
        {
            if (!filename.EndsWith(".png"))
                filename = $"{filename}.png";

            var dirPath = Path.Combine(Environment.CurrentDirectory, "Images");
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var filePath = Path.Combine(dirPath, filename);

            using (var bitmap = GetFilledBitmap())
                bitmap.Save(filePath, ImageFormat.Png);

            Console.WriteLine($"Cloud saved to path: {filePath}");
        }

        private Bitmap GetFilledBitmap()
        {
            var layoutSize = layouter.GetLayoutSize();

            var bitmap = new Bitmap(layoutSize.Width, layoutSize.Height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(layoutBrush,
                    new Rectangle(0, 0, layoutSize.Width, layoutSize.Height));

                foreach (var rectangle in layouter.Rectangles)
                    graphics.DrawRectangle(rectanglePen, rectangle);
            }

            return bitmap;
        }

        public void Dispose()
        {
            layoutBrush.Dispose();
            rectanglePen.Dispose();
        }
    }
}