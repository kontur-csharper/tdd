using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(500, 500));
            for (var i = 0; i < 1000; i++)
                circularCloudLayouter.PutNextRectangle(new Size(20, 3));
            Console.WriteLine("Cloud Filled");
            DrawAndSaveCloudImage(circularCloudLayouter, "Cloud2");
            Console.WriteLine("Image Generated");
            Console.ReadKey();
        }

        private static void DrawAndSaveCloudImage(CircularCloudLayouter circularCloudLayouter, string name)
        {
            var imageSize = circularCloudLayouter.Center.X * 2;
            Bitmap bitmap = new Bitmap(imageSize, imageSize);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in circularCloudLayouter.Rectangles)
            {
                var rnd = new Random(rectangle.Left * rectangle.Right - rectangle.Bottom);
                graphics.FillRectangle(new SolidBrush(
                    Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))), rectangle);
            }

            bitmap.Save($"{Environment.CurrentDirectory}\\{name}.png", ImageFormat.Png);
        }
    }
}
