using System;
using System.Drawing;

namespace TagsCloudVisualization.Infrastructure.Layout
{
    public interface ILayoutStrategy
    {
        Point GetPoint(Func<Point, bool> isValidPoint);
    }
}