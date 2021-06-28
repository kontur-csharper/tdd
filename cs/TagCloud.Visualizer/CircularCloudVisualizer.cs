using System.Drawing;
using System.IO;
using System.Linq;

namespace TagCloud.Visualizer
{
    internal static class CircularCloudVisualizer
    {
        private static void Main()
        {
            for (var i = 1; i <= 3; i++)
            {
                var cloudLayouter = new CircularCloudLayouter(new Point(600, 600));
                var rectangles = SizesCreator.CreateSizesArray(50, 150, 50, 150, i * 20)
                    .Select(cloudLayouter.PutNextRectangle).ToList();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "images");
                var bitmap = BitmapCreator.DrawBitmap(rectangles);
                bitmap.Save(Path.Combine(path, $"{i.ToString()}.bmp"));
            }
        }
    }
}