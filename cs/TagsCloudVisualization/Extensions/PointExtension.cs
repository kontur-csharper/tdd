using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Extensions
{
    public static class PointExtension
    {
        public static Point GetMaxDistanceToLayoutBorder(
            this Point point, IEnumerable<Rectangle> rectangles)
        {
            var deltaXToRight = rectangles
                .Max(rectangle => rectangle.Right - point.X);
            var deltaXToLeft = rectangles
                .Max(rectangle => point.X - rectangle.Left);

            var deltaYToBottom = rectangles
                .Max(rectangle => rectangle.Bottom - point.Y);
            var deltaYToUp = rectangles
                .Max(rectangle => point.Y - rectangle.Top);

            var maxDeltaX = Math.Max(deltaXToLeft, deltaXToRight);
            var maxDeltaY = Math.Max(deltaYToBottom, deltaYToUp);

            return new Point(maxDeltaX, maxDeltaY);
        }
    }
}
