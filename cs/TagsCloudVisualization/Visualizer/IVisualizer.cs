using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    public interface IVisualizer<out T>
    {
        T VisualizeTarget { get; }
        void Draw(Graphics graphics);
    }
}
