using System.Drawing;
using System.IO;

namespace TagsCloudVisualization.Graphic
{
    public class BitmapSaver : IImageSaver
    {
        private readonly string directory;
        private const string Extension = "bmp";

        public BitmapSaver(string directory)
        {
            this.directory =  directory;
        }

        public string GetPath(string filename)
        {
            var file = Path.ChangeExtension(filename, Extension);
            var path = Path.Combine(directory, file);
            return path;
        }

        public void Save(Image image, string path)
        {
            image.Save(path);
        }
    }
}