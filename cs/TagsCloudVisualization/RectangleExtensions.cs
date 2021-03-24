using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static double GetDiagonal(this Rectangle rectangle)
        {
            return Math.Sqrt(rectangle.Height * rectangle.Height + rectangle.Width * rectangle.Width);
        }
    }
}