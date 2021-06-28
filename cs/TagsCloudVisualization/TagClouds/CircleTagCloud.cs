using System.Drawing;

namespace TagsCloudVisualization.TagClouds
{
    public class CircleTagCloud : RectangleTagCloud
    {
        public CircleTagCloud(Point center, double startRadius = 0)
        {
            Center = center;
            StartRadius = startRadius;
        }

        public Point Center { get; }
        public double StartRadius { get; }
    }
}
