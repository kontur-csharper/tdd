using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Graphic;


namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var center = new Point();
            var layouter = new CircularCloudLayouter(center);
            var layout = new List<Rectangle>();
            
            var random = new Random();
            var maxWidth = 50;
            var maxHeight = 20;
            for (int i = 0; i < 300; i++)
                layout.Add(layouter.PutNextRectangle(new Size(random.Next(maxWidth),random.Next(maxHeight))));
            
            var drawer = new RainbowDrawer(1);
            var saver = new BitmapSaver(@"TagsCloudVisualization/Pictures");
            var path = saver.GetPath(@"picture");
            var image = drawer.GetImage(layout);
            saver.Save(image, path);
        }
    }
}