using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly SpiralPoints spiralPoints;
        private readonly ImageWriter writer = new ImageWriter();
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private int rightBorder;
        private int bottomBorder;

        public CircularCloudLayouter(Point center)
        {
            spiralPoints = new SpiralPoints(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetNextRectangle(rectangleSize);
            FitImageBorders(rectangle);
            rectangles.Add(rectangle);
            return rectangle;
        }

        public string Save() => writer.Save(rectangles, rightBorder, bottomBorder);
        
        private bool HaveIntersection(Rectangle rectangle) => rectangles.Any(other => other.IntersectsWith(rectangle));

        private void FitImageBorders(Rectangle rectangle)
        {
            if (rectangle.Bottom > bottomBorder)
                bottomBorder = rectangle.Bottom;
            if (rectangle.Right > rightBorder)
                rightBorder = rectangle.Right;
        }
        
        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var halfSize = new Size(rectangleSize.Width / 2, rectangleSize.Height / 2);
            using (var points = spiralPoints.GetSpiralPoints().GetEnumerator())
            {
                points.MoveNext();
                var rectangle = new Rectangle(points.Current - halfSize, rectangleSize); 
                while (HaveIntersection(rectangle) || rectangle.Left < 0 || rectangle.Top < 0)
                {
                    points.MoveNext();
                    rectangle.Location = points.Current - halfSize;
                }

                return rectangle;
            }
        }
    }
}