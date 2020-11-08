using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var center = new Point(540, 540);
            var random = new Random();
            var layouter = new CircularCloudLayouter(center);

            var sizes = new List<Size>();
            for (var i = 7; i < 300; i++)
                sizes.Add(new Size((int) (5 * random.NextDouble() * 1080 / i),
                    (int) (5 * random.NextDouble() * 1080 / 4 / i)));

            sizes = sizes.OrderByDescending(x => x.Height * x.Width).ToList();
            foreach (var size in sizes)
                layouter.PutNextRectangle(size);
            layouter.CreateImage($"{AppDomain.CurrentDomain.BaseDirectory}", "image");
        }
    }
}
