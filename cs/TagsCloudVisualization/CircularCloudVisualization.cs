using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class CircularCloudVisualization
    {
        private static Bitmap DrawRectanglesOnImage(CircularCloudLayouter layouter, int sideSize)
        {
            var layoutSize = layouter.GetLayoutSize();
            var bitmap = new Bitmap(layoutSize.Width, layoutSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            using var pen = new Pen(Color.Red, sideSize);
            graphics.DrawRectangles(pen, layouter.ToArray());
            return bitmap;
        }

        public static string SaveImageFromLayout(string name, CircularCloudLayouter layout, int penSize = 5)
        {
            if (!name.EndsWith(".png"))
                name += ".png";
            using var bitmap = DrawRectanglesOnImage(layout, penSize);
            var path = PathCreator.GetPathToFileWithName(3, "LayoutImage", name);
            bitmap.Save(path, ImageFormat.Png);
            return path;
        }
    }
}
