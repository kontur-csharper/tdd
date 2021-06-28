using System.Drawing;
using TagsCloudVisualization.TagClouds;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization.Renders
{
    public class FileCloudRender : IRender
    {
        private readonly string fileName;
        private readonly IVisualizer<RectangleTagCloud> visualizer;

        public FileCloudRender(IVisualizer<RectangleTagCloud> visualizer, string fileName)
        {
            this.visualizer = visualizer;
            this.fileName = fileName;
        }

        public void Render()
        {
            var leftUpBound = visualizer.VisualizeTarget.LeftUpBound;
            var rightDownBound = visualizer.VisualizeTarget.RightDownBound;

            var image = new Bitmap(
                rightDownBound.X - leftUpBound.X,
                rightDownBound.Y - leftUpBound.Y);
            var graphics = Graphics.FromImage(image);
            visualizer.Draw(graphics);
            image.Save(fileName);
        }
    }
}
