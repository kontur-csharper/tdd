using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Core
{
    public class CircularCloudLayouter
    {
        private List<Rectangle> Rectangles { get; }
        private ArchimedeanSpiral Spiral { get; }
        private Point Center { get; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Spiral = new ArchimedeanSpiral(center, 0.5);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle nextRectangle;

            do nextRectangle = new Rectangle(Spiral.GetNextPoint(), rectangleSize);
            while (Rectangles.Any(r => r.IntersectsWith(nextRectangle)));

            var shiftedNextRectangle = GetShiftedToCenterRectangle(nextRectangle);
            Rectangles.Add(shiftedNextRectangle);
            return shiftedNextRectangle;
        }

        public void SaveLayoutIntoBitmap(Bitmap bmp)
        {
            if (bmp is null)
                throw new ArgumentNullException();
            
            if (!Rectangles.Any())
                return;

            using var graphics = Graphics.FromImage(bmp);
            graphics.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
            graphics.FillRectangles(Brushes.Yellow, Rectangles.ToArray());
            graphics.DrawRectangles(Pens.Black, Rectangles.ToArray());
        }

        private Rectangle GetShiftedToCenterRectangle(Rectangle initialRectangle)
        {
            var minDistanceToCenter = double.MaxValue;
            var shiftedRectangle = initialRectangle;
            var queue = new Queue<Rectangle>();
            queue.Enqueue(shiftedRectangle);

            while (queue.Any())
            {
                var currentRectangle = queue.Dequeue();
                var distanceToCenter = currentRectangle.Location.DistanceTo(Center);
                if (Rectangles.Any(r => r.IntersectsWith(currentRectangle)) || minDistanceToCenter <= distanceToCenter)
                    continue;
                minDistanceToCenter = distanceToCenter;
                shiftedRectangle = currentRectangle;
                GetNeighboursFor(currentRectangle).ForEach(r => queue.Enqueue(r));
            }

            return shiftedRectangle;
        }


        private static List<Rectangle> GetNeighboursFor(Rectangle rectangle)
        {
            var neighbours = new (int X, int Y)[] {(1, 0), (0, 1), (-1, 0), (0, -1)};
            return neighbours
                .Select(p => new Rectangle(p.X + rectangle.X, p.Y + rectangle.Y, rectangle.Width, rectangle.Height))
                .ToList();
        }
    }
}