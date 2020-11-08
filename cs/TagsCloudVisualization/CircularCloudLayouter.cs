﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayout
    {
        public List<Rectangle> Rectangles { get; }
        private readonly IPointProvider pointProvider;

        public CircularCloudLayouter(IPointProvider provider)
        {
            Rectangles = new List<Rectangle>();
            pointProvider = provider;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("Width or height of rectangle was negative");

            var rectangle = GetRectangle(rectangleSize);
            Rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle GetRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(pointProvider.GetPoint(), rectangleSize);

            } while (IsCollide(rectangle));

            return rectangle;
        }

        private bool IsCollide(Rectangle rectangle)
        {
            return Rectangles.Any(rectangle.IntersectsWith) || rectangle.X < 0 || rectangle.Y < 0;
        }
    }
}

