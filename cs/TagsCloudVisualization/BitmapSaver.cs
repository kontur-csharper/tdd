using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class BitmapSaver
    {
        private readonly Bitmap bitmap;
        private readonly string directoryToSave;
        private readonly string fileName;

        public BitmapSaver(Bitmap bitmap, string directoryToSave, string fileName)
        {
            this.directoryToSave = directoryToSave;
            this.fileName = fileName;
            this.bitmap = bitmap;
        }

        public string RelativePath =>
            Path.Combine(directoryToSave, fileName);

        public void Save()
        {
            bitmap.Save(Path.Combine("..", "..", "..", directoryToSave, fileName));
        }
    }
}