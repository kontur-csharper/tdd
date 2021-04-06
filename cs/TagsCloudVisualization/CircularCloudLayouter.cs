﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        private readonly ArchimedeanSpiral _spiral;
        private readonly Point _canvasCenter;

        public CircularCloudLayouter(Point canvasCenter)
        {
            _rectangles = new List<Rectangle>();
            _canvasCenter = canvasCenter;
            _spiral = new ArchimedeanSpiral(_canvasCenter);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of the rectangle must be positive");

            Rectangle rectangle;
            do
            {
                var possiblePoint = _spiral.GetNextPoint();
                rectangle = RectangleExtensions.CreateRectangleFromMiddlePointAndSize(possiblePoint, rectangleSize);
            } while (rectangle.IntersectsWith(_rectangles)); 
            var result = MoveToCanvasCenter(rectangle);
            _rectangles.Add(result);

            return result;
        }

        public List<Rectangle> GetRectangles()
        {
            return _rectangles;
        }

        public Size GetSize()
        {
            if (!_rectangles.Any())
                throw new InvalidOperationException("Can't get the size if there are no rectangles");

            var maxX = int.MinValue;
            var minX = int.MaxValue;
            var maxY = int.MinValue;
            var minY = int.MaxValue;

            foreach (var rect in _rectangles)
            {
                if (rect.Right > maxX)
                    maxX = rect.Right;
                if (rect.Bottom > maxY)
                    maxY = rect.Bottom;
                if (rect.Left < minX)
                    minX = rect.Left;
                if (rect.Top < minY)
                    minY = rect.Top;
            }

            var width = maxX + minX;
            var height = maxY + minY;

            return new Size(width, height);
        }

        private Rectangle MoveToCanvasCenter(Rectangle rectangle, int axisStep = 1)
        {
            if (rectangle.GetMiddlePoint().Equals(_canvasCenter))
                return rectangle;

            var currentRectangle = rectangle;
            Rectangle result;

            do
            {
                result = currentRectangle;
                currentRectangle = currentRectangle.MoveOneStepTowardsPoint(_canvasCenter, axisStep);
            } while (!currentRectangle.IntersectsWith(_rectangles));
            
            return result;
        }

    }
}
