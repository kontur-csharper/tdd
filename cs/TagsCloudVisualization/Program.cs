using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = layouter.MakeLayouter(5000, 50, 60,
                20, 25).ToList();
            try
            {
                CircularCloudVisualisation.MakeImageTagsCircularCloud(rectangles, layouter.CloudRadius,
                    "circularCloud.jpg",
                    ImageFormat.Jpeg);
                Console.WriteLine("Image saved");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Failed to save the file");
            }
        }
    }
}