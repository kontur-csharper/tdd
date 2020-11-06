using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CircularCloud
{
    public class CloudObjectsContainer
    {
        protected readonly List<Rectangle> Rectangles = new List<Rectangle>();
        protected readonly List<Point> FreePoints = new List<Point>();

        public void AddRectangle(Rectangle rectangle)
        {
            Rectangles.Add(rectangle);
            FreePoints.RemoveAll(rectangle.Contains);
        }

        public void AddFreePoint(Point point)
        {
            if (!Rectangles.Any(rectangle => rectangle.Contains(point)))
                FreePoints.Add(point);
        }

        public List<Point> GetFreePoints()
        {
            return FreePoints.ToList();
        }

        public List<Rectangle> GetRectangles()
        {
            return Rectangles.ToList();
        }
    }
}