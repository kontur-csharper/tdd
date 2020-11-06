using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Infrastructure.Environment
{
    public class PlainEnvironment : Environment<Rectangle>
    {
        public PlainEnvironment()
        {
            Elements = new List<Rectangle>();
        }

        public override void Add(Rectangle element)
        {
            Elements.Add(element);
        }

        public override void Remove(Rectangle element)
        {
            Elements.Remove(element);
        }

        public override bool IsColliding(Rectangle element)
        {
            return Elements.Any(placedRectangle => placedRectangle.IntersectsWith(element));
        }
    }
}