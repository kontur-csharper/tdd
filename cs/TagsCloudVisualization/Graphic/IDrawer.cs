using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Graphic
{
    public interface IDrawer<T>
    {
        public Image GetImage(IEnumerable<T> elements);
    }
}