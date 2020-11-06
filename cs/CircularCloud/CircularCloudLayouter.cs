using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CircularCloud
{
    public class CircularCloudLayouter
    {
        protected ArchimedeanSpiral Spiral;
        protected CloudObjectsContainer Container = new CloudObjectsContainer();

        public Point Center => Spiral.Center;

        public CircularCloudLayouter(Point center) => Spiral = new ArchimedeanSpiral(center);

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Point pointForRectangle;
            var result = Rectangle.Empty;
            if (Container.GetFreePoints().Count > 0)
            {
                pointForRectangle = Container.GetFreePoints().FirstOrDefault(point =>
                    CouldPutRectangle(GetRectangleWithCenterInPoint(point, rectangleSize)));
                if (pointForRectangle != Point.Empty)
                {
                    result = GetRectangleWithCenterInPoint(pointForRectangle, rectangleSize);
                    Container.AddRectangle(result);
                    return result;
                }
            }

            result = GetRectangleInNextPoint(rectangleSize);

            Container.AddRectangle(result);
            return result;
        }

        private Rectangle GetRectangleInNextPoint(Size rectangleSize)
        {
            while (true)
            {
                var pointForRectangle = Spiral.GetNextPoint();
                var rectangle = GetRectangleWithCenterInPoint(pointForRectangle, rectangleSize);
                if (CouldPutRectangle(rectangle))
                    return rectangle;
                Container.AddFreePoint(pointForRectangle);
            }
        }

        private bool CouldPutRectangle(Rectangle rectangle)
        {
            return !(Container.GetRectangles().Count > 0 && Container.GetRectangles().Any(rect =>
                rect.IntersectsWith(rectangle)));
        }

        public static Rectangle GetRectangleWithCenterInPoint(Point point, Size rectangleSize)
        {
            point.X -= rectangleSize.Width / 2;
            point.Y -= rectangleSize.Height / 2;
            return new Rectangle(point, rectangleSize);
        }

        public Rectangle[] GetAllRectangles()
        {
            return Container.GetRectangles().ToArray();
        }
    }
}