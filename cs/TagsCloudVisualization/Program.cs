using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(350, 250));
            var painter = new Painter(700, 500);
            var rand = new Random(Environment.TickCount);
            for (var i = 0; i < 100; i++)
            {
                var rectangle =
                    circularCloudLayouter.PutNextRectangle(new Size(rand.Next(10, 50), rand.Next(10, 30)));
                painter.PaintRectangle(rectangle);
            }

            painter.SavePicture("picture1.jpeg");
            circularCloudLayouter = new CircularCloudLayouter(new Point(350, 250));
            painter = new Painter(700, 500);
            for (var i = 0; i < 50; i++)
            {
                var rectangle =
                    circularCloudLayouter.PutNextRectangle(new Size(rand.Next(30, 40), rand.Next(30, 60)));
                painter.PaintRectangle(rectangle);
            }

            painter.SavePicture("picture2.jpeg");
        }
    }
}