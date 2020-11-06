using System.Drawing;
using TagsCloudVisualization.Infrastructure.Environment;
using TagsCloudVisualization.Infrastructure.Layout;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ILayoutStrategy strategy;
        private readonly Environment<Rectangle> environment;

        public CircularCloudLayouter(Point center)
        {
            strategy = new SpiralPlacing(center, 1);
            environment = new PlainEnvironment();
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangleMiddle = new Size(rectangleSize.Width / 2, rectangleSize.Height / 2);

            var possiblePoint = strategy.GetPoint(point => CanPlaceRectangle(point - rectangleMiddle, rectangleSize));
            var rectangle = new Rectangle(possiblePoint - rectangleMiddle, rectangleSize);
            
            environment.Add(rectangle);
            return rectangle;
        }
        
        private bool CanPlaceRectangle(Point possiblePoint, Size rectangleSize)
        {
            var rectangle = new Rectangle(possiblePoint, rectangleSize);
            return !environment.IsColliding(rectangle);
        }
    }
}