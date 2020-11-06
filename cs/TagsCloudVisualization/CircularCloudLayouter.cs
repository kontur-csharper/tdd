using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly Spiral spiral = new Spiral();
        private readonly Surface surface = new Surface();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rect = new Rectangle(spiral.GetNextPoint(), rectangleSize);
            while (surface.RectangleIntersectsWithOther(rect))
            {
                rect.Location = spiral.GetNextPoint();
            }

            if (rect.Location != Point.Empty)
                rect = surface.GetShiftedToCenterRectangle(rect);

            surface.AddRectangle(rect);
            spiral.Reset();
            return rect;
        }
    }
}