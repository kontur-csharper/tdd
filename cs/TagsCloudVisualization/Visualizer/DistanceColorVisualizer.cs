using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization.Visualizer
{
    public class DistanceColorVisualizer : IVisualizer<CircleTagCloud>
    {
        private readonly Color fromColor;
        private readonly double fromRadius;
        private readonly Color toColor;
        private readonly double toRadius;

        public DistanceColorVisualizer(
            CircleTagCloud cloud,
            Color fromColor,
            double fromRadius,
            Color toColor,
            double toRadius)
        {
            VisualizeTarget = cloud;
            this.fromColor = fromColor;
            this.fromRadius = fromRadius;
            this.toColor = toColor;
            this.toRadius = toRadius;
        }

        public CircleTagCloud VisualizeTarget { get; }

        public void Draw(Graphics graphics)
        {
            var leftUpBound = VisualizeTarget.LeftUpBound;
            graphics.TranslateTransform(-leftUpBound.X, -leftUpBound.Y);
            foreach (var rectangle in VisualizeTarget)
            {
                var distance = VisualizeTarget.Center.DistanceBetween(rectangle.Center());
                using var brush = GetBrush(LinearMath.LinearInterpolate(fromRadius, toRadius, distance));
                graphics.FillRectangle(brush, rectangle);
            }
        }

        private SolidBrush GetBrush(double gradientBlend)
        {
            var a = GetColorComponent(fromColor.A, toColor.A, gradientBlend);
            var r = GetColorComponent(fromColor.R, toColor.R, gradientBlend);
            var g = GetColorComponent(fromColor.G, toColor.G, gradientBlend);
            var b = GetColorComponent(fromColor.B, toColor.B, gradientBlend);
            return new SolidBrush(Color.FromArgb(a, r, g, b));
        }

        private int GetColorComponent(int from, int to, double blend)
        {
            return (int)((to - from) * blend + from);
        }
    }
}
