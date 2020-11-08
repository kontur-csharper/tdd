using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public interface IRectangleLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }

        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}