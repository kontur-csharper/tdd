using System;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class ImageGenerator
    {
        public static void GenerateImageWithRectangles(Rectangle[] rectangles, string outputFile,
            Func<int, Color> colorFunc = null, Func<int, string> textFunc = null)
        {
            colorFunc ??= i => Color.FromArgb(
                0 + 255 / rectangles.Length * (i + 1),
                255 - 255 / rectangles.Length * (i + 1),
                0);
            textFunc ??= i => $"{i}";
            
            var bitmap = new Bitmap(800, 800);
            var graphics = Graphics.FromImage(bitmap);
            for(var i = 0; i < rectangles.Length; i++)
            {
                graphics.FillRectangle(new SolidBrush(colorFunc(i)), rectangles[i]);
                graphics.DrawRectangle(Pens.Black, rectangles[i]);
                var font = new Font("Arial", 24, GraphicsUnit.Pixel);
                graphics.DrawString(textFunc(i), font, Brushes.Black, rectangles[i]);
            }
            
            bitmap.Save(outputFile);
        }
        
        public static void GenerateImageWithRectanglesFromLayouter(Size[] rectangleSizes, string outputFile,
            Func<int, Color> colorFunc = null, Func<int, string> textFunc = null,
            CircularCloudLayouter layouter = null)
        {
            layouter ??= new CircularCloudLayouter(new Point(400, 400));
            var rectangles = rectangleSizes.Select(s => layouter.PutNextRectangle(s)).ToArray();
            GenerateImageWithRectangles(rectangles, outputFile, colorFunc, textFunc);
        }

        public static readonly (string outputFile, Size[] rectangleSizes)[] TestImages =
        {
            ("18_rectangles", new[]
            {
                new Size(25, 160),
                new Size(30, 150),
                new Size(30, 140),
                new Size(40, 130),
                new Size(50, 120),
                new Size(60, 110),
                new Size(70, 100),
                new Size(80, 90),
                new Size(90, 80),
                new Size(100, 70),
                new Size(110, 60),
                new Size(120, 50),
                new Size(130, 40),
                new Size(140, 30),
                new Size(150, 25),
                new Size(160, 25),
                new Size(160, 25),
                new Size(160, 25),
            }),
            ("26_horizontal_rectangles", new[]
            {
                new Size(20, 40),
                new Size(25, 40),
                new Size(30, 40),
                new Size(35, 40),
                new Size(40, 40),
                new Size(45, 40),
                new Size(50, 40),
                new Size(55, 40),
                new Size(60, 40),
                new Size(65, 40),
                new Size(70, 40),
                new Size(75, 40),
                new Size(80, 40),
                new Size(85, 40),
                new Size(90, 40),
                new Size(95, 40),
                new Size(100, 40),
                new Size(105, 40),
                new Size(110, 40),
                new Size(115, 40),
                new Size(120, 40),
                new Size(125, 40),
                new Size(130, 40),
                new Size(135, 40),
                new Size(140, 40),
                new Size(145, 40),
            })
        };

        [TestCaseSource(nameof(TestImages))]
        public void GenerateTestImages((string outputFile, Size[] rectangleSizes) data)
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                .Parent?.Parent?
                .CreateSubdirectory("Images") ?? new DirectoryInfo(".");
            data.outputFile = $"{directory}\\{data.outputFile}.bmp";
            GenerateImageWithRectanglesFromLayouter(data.rectangleSizes, data.outputFile);
        }
    }
}