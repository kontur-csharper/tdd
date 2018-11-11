﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudConstruction;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Size WindowSize { get; set; }
        public Point Center { get; set; }
        public List<Rectangle> Rectangles { get; set; }
        public CloudCompactor CloudCompactor { get; set; }
        public RectangleGenerator RectangleGenerator { get; set; }

        public CircularCloudLayouter(Point center)
        {
            WindowSize = new Size(2000, 2000);
            if (center.X < 0 || center.Y < 0 || center.X > WindowSize.Width || center.Y > WindowSize.Height)
                throw new ArgumentException("Center coordinates must not exceed the window size");
            Center = center;
            Rectangles = new List<Rectangle>();
            CloudCompactor = new CloudCompactor(this);
            RectangleGenerator = new RectangleGenerator(this);
        }

        public Rectangle PutNextRectangle(Size size)
        {
            var resultRect = RectangleGenerator.GetNextRectangle(size);
            resultRect = CloudCompactor.ShiftRectangleToTheNearest(resultRect);
            Rectangles.Add(resultRect);
            return resultRect;
        }
    }
}
