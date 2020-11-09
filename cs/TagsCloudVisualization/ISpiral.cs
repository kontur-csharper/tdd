using System.Drawing;

namespace TagsCloudVisualization
{
    public interface ISpiral
    {
        Point Center { get; }
        Point GetNewSpiralPoint();
    }
}