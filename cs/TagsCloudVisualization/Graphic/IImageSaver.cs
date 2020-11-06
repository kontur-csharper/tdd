using System.Drawing;

namespace TagsCloudVisualization.Graphic
{
    public interface IImageSaver
    {
        void Save(Image image, string path);
    }
}