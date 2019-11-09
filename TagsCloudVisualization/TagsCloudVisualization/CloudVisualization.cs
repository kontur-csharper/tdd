﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CloudVisualization : IVisualization
    {
        public Bitmap DrawRectangles(List<Rectangle> rectangles)
        {
            var image = new Bitmap(900, 900);
            var random = new Random();
            using (var drawPlace = Graphics.FromImage(image))
            {
                foreach (var rectangle in rectangles)
                {
                    var color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                    drawPlace.FillRectangle(new SolidBrush(color), rectangle);
                }
            }
            return image;
        }
    }
}
