﻿using System;
using System.Collections.Generic;
//пришлось скачивать через нугет так как на нетКоре не было битмапа)
using System.Drawing;

namespace TagCloud
{
    public static class TagDrawer
    {
        public static void Draw(string fileName, CircularCloudLayouter layouter)
        {
            var pens = new List<Pen>
            {
                new Pen(new SolidBrush(Color.Blue)),
                new Pen(new SolidBrush(Color.Green)),
                new Pen(new SolidBrush(Color.Black)),
                new Pen(new SolidBrush(Color.Red)),
                new Pen(new SolidBrush(Color.Yellow)),
                new Pen(new SolidBrush(Color.Magenta))
            };
            const int border = 10;
            var width = layouter.SizeOfCloud.Width + border;
            var height = layouter.SizeOfCloud.Height + border;
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            var whiteRectangle = new Rectangle(0, 0, width, height);
            graphics.Clear(Color.White);

            var colorIndex = 0;
            layouter.Rectangles.ForEach(rect =>
            {
                rect.Offset(new Point(border / 2 - layouter.LeftDownPointOfCloud.X, border / 2 - layouter.LeftDownPointOfCloud.Y));
                colorIndex = colorIndex + 1 % pens.Count;
                graphics.DrawRectangle(pens[colorIndex], rect);
            });
            bitmap.Save(fileName);
        }
    }
}