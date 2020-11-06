using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Graphic
{
    public class RainbowDrawer : IDrawer<Rectangle>
    {
        private readonly int fieldSize;
        private readonly int brushSize;
        private readonly Random random;
        public RainbowDrawer(int brushSize, int fieldSize = 10)
        {
            this.brushSize = brushSize;
            this.fieldSize = fieldSize;
            random = new Random();
        }
        public Image GetImage(IEnumerable<Rectangle> rectangles)
        {
            var (left, right, bottom, top) = GetBoarders(rectangles);
            var image = new Bitmap(right - left + fieldSize, bottom - top + fieldSize);
            var imageGraphics = Graphics.FromImage(image);
            var pen = new Pen(Color.Red, brushSize);

            var preparedRectangles = rectangles.Select(rectangle =>
            {
                rectangle.Offset(-left + fieldSize / 2, -top + fieldSize / 2);
                return rectangle;
            });
            foreach (var rectangle in preparedRectangles)
            {
                pen.Color = GetRandomColor();
                imageGraphics.DrawRectangle(pen, rectangle);
            }
            return image;
        }
        
        private Color GetRandomColor()
        {
            return Color.FromArgb(128,
                255 * random.Next(2),
                255 * random.Next(2),
                255 * random.Next(2));
        }

        private (int left, int right, int bottom, int top) GetBoarders(IEnumerable<Rectangle> environment)
        {
            var left = 0;
            var right = 0;
            var bottom = 0;
            var top = 0;
            foreach (var rectangle in environment)
            {
                if (rectangle.Left < left)
                    left = rectangle.Left;
                if (rectangle.Right > right)
                    right = rectangle.Right;
                if (rectangle.Top < top)
                    top = rectangle.Top;
                if (rectangle.Bottom > bottom)
                    bottom = rectangle.Bottom;
            }

            return (left, right, bottom, top);
        }
    }
}