using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var rectanglesSizes = new[]
            {
                new Size(50, 50),
                new Size(20, 80),
                new Size(70, 30),
                new Size(10, 10),
                new Size(30, 40),
                new Size(20, 20)
            };
            var center = new Point(100, 100);
            var layouter = new CircularCloudLayouter(center);
            var placedRectangles =
                rectanglesSizes.Select(x => layouter.PutNextRectangle(x)).ToList();
            var bitmap = CircularCloudDrawer.GetBitmap(placedRectangles, center);
            var bitmapSaver = new BitmapSaver(bitmap, "pictures", "random.jpg");
            bitmapSaver.Save();
        }
    }
}