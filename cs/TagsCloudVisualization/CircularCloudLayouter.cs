using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : IEnumerable<Rectangle>
    {
        private readonly List<Rectangle> rectangles;
        private readonly Spiral spiral;

        public CircularCloudLayouter(Point center)
        {
            rectangles = new List<Rectangle>();
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            /// Идея алгоритма:
            /// изначально в каждой точке спирали  spiral.GetSpiralPoints
            /// буду пытаться разместить прямоугольник 
            /// если он в какой-то точке не пересекает остальные из rectangles
            ///     то заканчиваю перебор точек и понемногу двигаю прямоугольник по вектору center - rectangle.Location
            /// если во время перемещения прямоугольника он начинает пересекать остальные, 
            ///     то заканчиваю перемещение и возвращаю полученный прямоугольник

            var rectangle = new Rectangle();
            rectangle.Size = rectangleSize;
            foreach (var point in spiral.GetPoints())
            {
                rectangle.Location = point;
                if (!rectangle.IntersectsWith(rectangles))
                    break;
            }
            var movedRectangle = MoveToCenter(rectangle);
            rectangles.Add(movedRectangle);
            return movedRectangle;
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            var targetVector = new TargetVector(spiral.Center, rectangle.Location);
            foreach (var delta in targetVector.GetPartialDelta())
            {
                var newRectangle = rectangle.MoveOnTheDelta(delta);
                if (newRectangle.IntersectsWith(rectangles))
                    continue;
                rectangle = newRectangle;
            }
            return rectangle;
        }

        public Size GetLayoutSize()
        {
            var size = new Size();
            foreach (var rect in rectangles)
            {
                size.Width = Math.Max(rect.X - spiral.Center.X + rect.Width, size.Width);
                size.Height = Math.Max(rect.Y - spiral.Center.Y + rect.Height, size.Height);
            }
            size.Height += spiral.Center.X * 2;
            size.Width += spiral.Center.Y * 2;
            return size;
        }

        public IEnumerator<Rectangle> GetEnumerator()
        {
            return rectangles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
