using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class ImageWriter
    {
        private const string SamplesDirectory = "Samples";
        private const int PenWidth = 5;
        private readonly string path;
        private readonly Pen pen = new Pen(Color.Purple, PenWidth);

        public ImageWriter()
        {
            var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            if (directoryInfo == null) return;
            path = $"{directoryInfo.FullName}\\{SamplesDirectory}";
            if(!Directory.Exists(path))
                directoryInfo.CreateSubdirectory(SamplesDirectory);
        }
        
        public string Save(List<Rectangle> rectangles, int width, int height)
        {
            if (string.IsNullOrEmpty(path)) return null;
            
            var bitmap = new Bitmap(width + PenWidth, height + PenWidth);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(pen, rectangle);

            graphics.Save();
            var imagePath = $"{path}\\{rectangles.Count}rectangles.jpg";
            bitmap.Save(imagePath);
            return imagePath;
        }
    }
}