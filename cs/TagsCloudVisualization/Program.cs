using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main()
        {
            var layout = new CircularCloudLayouter(new Point(200, 200));
            var random = new Random();
            for (int i = 1; i < 50; i++)
                layout.PutNextRectangle(new Size(random.Next(30, 60), random.Next(20, 60)));
            CircularCloudVisualization.SaveImageFromLayout("Test", layout);
        }
    }
}
