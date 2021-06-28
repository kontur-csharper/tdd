﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtension
    {
        public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Any(otherRect => rectangle.IntersectsWith(otherRect));
        }

        public static Rectangle MoveOnTheDelta(this Rectangle rectangle, Point delta)
        {
            rectangle.X += delta.X;
            rectangle.Y += delta.Y;
            return rectangle;
        }

        public static bool TryMoveRectangle(this Rectangle rectangle, Point delta, IEnumerable<Rectangle> shouldNotIntersect)
        {
            var movedRectangle = rectangle.MoveOnTheDelta(delta);
            return !movedRectangle.IntersectsWith(shouldNotIntersect);
        }
    }
}
