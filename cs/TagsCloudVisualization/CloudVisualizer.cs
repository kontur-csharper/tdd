using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    class CloudVisualizer
    {
        public static void VisualizeCloudFromFile(string inputFileName, string outputFileName, Size imageSize)
        {
            VisualizeCloud(GetRectanglesFromFile(inputFileName), outputFileName, imageSize);
        }

        public static void VisualizeCloud(IEnumerable<Rectangle> rectangles, string outputFileName, Size imageSize)
        {
            using (var image = new Bitmap(imageSize.Width, imageSize.Height))
            using (var g = Graphics.FromImage(image)) 
            using (var pen = new Pen(Color.Black))
            { 
                g.FillRectangle(new SolidBrush(Color.Red), rectangles.First());
                foreach (var rectangle in rectangles)
                {
                    g.DrawRectangle(pen, rectangle);
                }
                image.Save(outputFileName, ImageFormat.Png);
            }
        }

        private static IEnumerable<Rectangle> GetRectanglesFromFile(string inputFileName)
        {
            using (var reader = new StreamReader(inputFileName))
            {
                var center = GetPointFromLine(reader.ReadLine());
                var layouter = new CircularCloudLayouter(center);
                while (!reader.EndOfStream)
                {
                    yield return layouter.PutNextRectangle(GetSizeFromLine(reader.ReadLine()));
                }
            }
        }

        private static Size GetSizeFromLine(string line)
        {
            var sizeStr = line.Split(' ');
            return new Size(int.Parse(sizeStr[0]), int.Parse(sizeStr[1]));
        }

        private static Point GetPointFromLine(string line)
        {
            var centerStr = line.Split(' ');
            return new Point(int.Parse(centerStr[0]), int.Parse(centerStr[1]));
        }
    }
}