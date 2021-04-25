using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Models;
using TagsCloudVisualization.View;

namespace TagsCloudVisualization
{
    class Program
    {
        private static readonly Dictionary<string, Action<CircularCloudLayouter>> CloudNamesToLayoutFillers =
            new Dictionary<string, Action<CircularCloudLayouter>>()
        {
            {"SameSquares",PlaceSameSquares},
            {"FixedSquares",PlaceFixedSquares},
            {"ChangingRectangles",PlaceChangingRectangles},
        };

        static void Main(string[] args)
        {
            foreach (var namesToLayoutFiller in CloudNamesToLayoutFillers)
            {
                var circularCloudLayouter = new CircularCloudLayouter(new Point(800, 600));
                namesToLayoutFiller.Value(circularCloudLayouter);

                using (var cloudImageCreator = new TagCloudCreator(circularCloudLayouter))
                    cloudImageCreator.Save(namesToLayoutFiller.Key);
            }
        }

        private static void PlaceSameSquares(CircularCloudLayouter layouter)
        {
            for (var i = 0; i < 50; i++)
                layouter.PutNextRectangle(new Size(50, 50));
        }

        private static void PlaceFixedSquares(CircularCloudLayouter layouter)
        {
            var rects = new List<Size> { new Size(30, 30), new Size(40, 40), new Size(50, 50) };
            for (var i = 0; i < 50; i++)
                layouter.PutNextRectangle(rects[i % rects.Count]);
        }

        private static void PlaceChangingRectangles(CircularCloudLayouter layouter)
        {
            for (var i = 0; i < 50; i++)
                layouter.PutNextRectangle(new Size(70 + i, 40));
        }
    }
}
